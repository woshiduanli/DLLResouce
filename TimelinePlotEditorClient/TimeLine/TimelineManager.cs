using Cinemachine;
using Cinemachine.Timeline;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.EventSystems;

public class TimelineManager : MonoBehaviour {

    public static TimelineManager Instance { get; private set; }

    private Dictionary<string, LoadedTimelineData> loadedTimeline;
    private GameObject curvePointParent;
    private LoadedTimelineData playingTimeline;
    private GameObject uiRoot;
    private Dictionary<string,AnimationClip> loadedAniClips;

    public static Dictionary<TimelineRoleOcc, string> AniClipPath = new Dictionary<TimelineRoleOcc, string>
    {
        {TimelineRoleOcc.ZhanShi,"res/model/player/zs/zs_animation_m/{0}" },
        {TimelineRoleOcc.XueZu,"res/model/player/xz/xz_animation_m/{0}" },
        {TimelineRoleOcc.FaShi,"res/model/player/fs/fs_animation_f/{0}" },
    };


    public static void Init()
    {
        if (!Instance)
        {
            var go = new GameObject("TimelineManager");
            DontDestroyOnLoad(go);
            Instance = go.AddComponent<TimelineManager>();
            Instance.enabled = true;
        }
    }

    private void Awake()
    {
        Instance = this;
        curvePointParent = new GameObject("TimelineMoveTrackPointParent");
        var eventSystem = new GameObject("EventSystem");
        eventSystem.AddComponent<EventSystem>();
        eventSystem.AddComponent<StandaloneInputModule>();
        curvePointParent.transform.position = Vector3.zero;
        DontDestroyOnLoad(curvePointParent);
        DontDestroyOnLoad(eventSystem);

        loadedTimeline = new Dictionary<string, LoadedTimelineData>();
        loadedAniClips = new Dictionary<string, AnimationClip>();
        BehaviourExecuterFactory.CreatActionExecuter =()=>new RoleActionExecuter();
        BehaviourExecuterFactory.CreatAnimationExecuter = () => new RoleAnimationExecuter();
        BehaviourExecuterFactory.CreatDialogueUiExecuter = () => new DialogueUIExecuter();
        BehaviourExecuterFactory.CreatMoveExecuter = () => new RoleMoveExecuter();
        BehaviourExecuterFactory.CreatCameraEffectExecuter = () => new CameraEffectExecuter();
        BehaviourExecuterFactory.CreatVmOperateExecuter = VMOperaterFactory.Creat;
        BehaviourExecuterFactory.CreatEffectExecuter = ()=>new EffectExecuter();
        BehaviourExecuterFactory.CreatActivationControlExecuter = () => new ActivationControlExecuter();
        BehaviourExecuterFactory.CreatScaleControlExecuter = () => new ScaleControlExecuter();
        BehaviourExecuterFactory.CreatRoleEffectExecuter = (type) => RoleEffectExecuterFactory.Creat(type);
        BehaviourExecuterFactory.CreatAudioExecuter = () => new AudioExecuter();
        BehaviourExecuterFactory.CreatPlotDialogueUiExecuter = () => new PlotDialogueExecuter();
        BehaviourExecuterFactory.CreatBubbleUiExecuter = () => new BubbleUIExecuter();
    }


    private void Update()
    {
        if (playingTimeline!=null)
        {
            if (playingTimeline.director.state == PlayState.Paused && playingTimeline.director.time==0)
            {
                OnTimelineStop();
                playingTimeline = null;
            }
        }
    }

    private void OnTimelineStop()
    {
        CinemachineBrain brain = CameraControll.Instance.gameObject.GetComponent<CinemachineBrain>();
        var vm = brain.GetComponentInChildren<CinemachineVirtualCamera>();
        if (vm != null)
            vm.gameObject.SetActive(false);
        if (brain != null)
            brain.enabled = false;
    }


    public void SavePrefab(string v)
    {
        LoadedTimelineData data;
        if (loadedTimeline.TryGetValue(v, out data))
        {
            GameObject resourcePrefab=Resources.Load<GameObject>(string.Format("Timeline/Timeline/{0}", v));
            data.prefabResource=PrefabUtility.ReplacePrefab(data.timelineGo, resourcePrefab, ReplacePrefabOptions.ReplaceNameBased);
        }
        else
            EditorUtility.DisplayDialog("", "未加载这个timeline", "ok");
    }

    public void DestroyTimeline(string name)
    {
        LoadedTimelineData data;
        if (loadedTimeline.TryGetValue(name, out data))
        {
            World.Instance.ClearTimelineReside();
            loadedTimeline.Remove(name);
            Destroy(data.timelineGo);
            if (data == playingTimeline)
            {
                playingTimeline = null;
            }
            if (uiRoot)
            {
                Destroy(uiRoot);
            }
        }
    }

    public void PlayTimeLine(string name)
    {
        LoadedTimelineData data;
        if (playingTimeline != null && playingTimeline.director.state == PlayState.Paused)
        {
            playingTimeline.director.Resume();
            return;
        }
        if (loadedTimeline.TryGetValue(name, out data))
        {
            playingTimeline = data;
            data.director.time = 0;
            data.director.Stop();
            InitTimelinePlaySetup(data);
            data.director.Play();
        }
        else
            EditorUtility.DisplayDialog("", "未加载这个timeline", "ok");
    }


    public void PauseTimeline(string name)
    {
        LoadedTimelineData data;
        if (playingTimeline != null && playingTimeline.director.state == PlayState.Playing)
        {
            playingTimeline.director.Pause();
        }else
            EditorUtility.DisplayDialog("", "没有正在播放的timeline", "ok");
    }

    private void InitTimelinePlaySetup(LoadedTimelineData data)
    {
        CinemachineBrain brain = CameraControll.Instance.gameObject.GetComponent<CinemachineBrain>();
        if (brain != null)
        {
            brain.enabled = true;
            var vm = brain.transform.GetChild(0);
            if (vm != null)
                vm.gameObject.SetActive(true);
        }

        foreach (var binding in data.director.playableAsset.outputs)
        {
            if (binding.sourceObject is MYCinemachineTrack)
            {
                SetCinemachineTrackData(binding, data.director);
                continue;
            }
        }
        List<RoleData> settedRoleData = new List<RoleData>();
        foreach (var binding in data.director.playableAsset.outputs)
        {
            TrackAsset track = binding.sourceObject as TrackAsset;
            RoleData roledata = data.director.GetGenericBinding(track) as RoleData;
            if (roledata == null || settedRoleData.Contains(roledata))
                continue;
            settedRoleData.Add(roledata);
            World.Instance.GetRoleObj(roledata).transform.position = roledata.InitialPos;
            World.Instance.GetRoleObj(roledata).transform.eulerAngles = roledata.InitialRotation;
        }
    }

    public void LoadTimeline(string timelineName)
    {
        LoadedTimelineData data;
        if (loadedTimeline.TryGetValue(timelineName, out data))
        {
            EditorUtility.DisplayDialog("", "已加载这个timeline", "ok");
            return;
        }
        data = new LoadedTimelineData();

        GameObject resource = Resources.Load<GameObject>(string.Format("Timeline/Timeline/{0}",timelineName));
        if (resource == null)
        {
            EditorUtility.DisplayDialog("", "没有这个名字的Timeline资源","ok");
            return;
        }

        data.timelineGo = GameObject.Instantiate(resource);
        if (data.timelineGo.GetComponent<TimelinePrefab>() == null)
            data.timelineGo.AddComponent<TimelinePrefab>();
        data.prefabResource = resource;
        data.director = data.timelineGo.GetComponent<PlayableDirector>();
        loadedTimeline.Add(timelineName, data);

        SetUILayer(data.director);
        LoadAndSetTimelineBindingData(data);


        GameObject uiResouce= Resources.Load<GameObject>(string.Format("Timeline/UI/Prefab/TimelineUIRoot", timelineName));
        uiRoot = GameObject.Instantiate(uiResouce);
        //uiRoot.transform.SetParent(data.timelineGo.transform);
        if (TimelineUI.Instance)
        {
            TimelineUI.Instance.DialogueUINode.AddComponent<FundamentalUI>();
            TimelineUI.Instance.PlotDialogueUINode.AddComponent<PlotDialogueUI>();
            TimelineUI.Instance.TextBubbleNode.AddComponent<TextBubbleUI>();
            FundamentalUI.Instance.skipTimeline = () => OnSkipClick(data.director);
            PlotDialogueUI.Instance.SetTimelinePlay = isPlay => OnSetTimelinePlay(isPlay);
        }

        //StartCoroutine(InitVirtualCamera(data));
    }

    private void OnSetTimelinePlay(bool isPlay)
    {
        if (playingTimeline == null || playingTimeline.director == null)
            return;
        if (isPlay)
            playingTimeline.director.Resume();
        else
            playingTimeline.director.Pause();
    }

    private void OnSkipClick(PlayableDirector director)
    {
        director.Stop();
        List<RoleData> loadedRoleData = new List<RoleData>();
        foreach (var binding in director.playableAsset.outputs)
        {
            if (binding.sourceObject is MYCinemachineTrack)
            {
                SetCinemachineTrackData(binding, director);
                continue;
            }

            UnityEngine.Object bindingObj = director.GetGenericBinding(binding.sourceObject);
            if (bindingObj != null && bindingObj is RoleData)
            {
                RoleData roledata = bindingObj as RoleData;
                if (!loadedRoleData.Contains(roledata))
                {
                    loadedRoleData.Add(roledata);
                    RoleObject role = roledata.Role as RoleObject;
                    if(role!=null && role.gameObject != null && roledata.TargetPosWhenSkip!=Vector3.zero)
                    {
                        role.gameObject.transform.position = roledata.TargetPosWhenSkip;
                    }
                }
            }
        }
    }


    private void SetUILayer(PlayableDirector director)
    {
        Canvas uiRoot = director.gameObject.GetComponentInChildren<Canvas>(true);
        if (uiRoot == null)
            return;
        // uiRoot.gameObject.layer = XYDefines.Layer.TimelineUI;
        SetLayerRecursively(uiRoot.gameObject,XYDefines.Layer.TimelineUI);
        uiRoot.worldCamera.cullingMask = XYDefines.Layer.Mask.TimelineUI;
    }

    public void SetLayerRecursively(GameObject go,int layer)
    {
        go.layer = layer;
        List<GameObject> array = go.GetAllChildren();
        if (array != null && array.Count>0)
        {
            foreach (var tf in array)
            {
                if (tf.gameObject != go)
                    SetLayerRecursively(tf,layer);
            }
        }
    }

    private void LoadAndSetTimelineBindingData(LoadedTimelineData data)
    {
        //这里可以得到每个track的绑定值，并进行一些设置，比如将绑定的类中的roleobject变量赋值
        //这里还可以对每个track中的每个clip设置，可以取到每个clip的asset，并对其中的属性赋值，这样在creatplayable的时候，就会赋值到playable上
        CinemachineBrain brain = CameraControll.Instance.gameObject.GetComponent<CinemachineBrain>();
        if (brain == null)
            brain = CameraControll.Instance.gameObject.AddComponent<CinemachineBrain>();
        brain.enabled = false;
        var childVm = brain.GetComponentInChildren<CinemachineVirtualCamera>();
        if (childVm == null)
        {
            GameObject vitrualCa = new GameObject("VM", typeof(CinemachineVirtualCamera));
            vitrualCa.transform.parent = brain.transform;
            vitrualCa.transform.localPosition = Vector3.zero;
            vitrualCa.transform.localEulerAngles = Vector3.zero;
            vitrualCa.transform.localScale = Vector3.one;
            var VM = vitrualCa.GetComponent<CinemachineVirtualCamera>();
            VM.Priority = 1;
        }

        //List<RoleData> loadedRoleData = new List<RoleData>();
        needLoadRoleData.Clear();
        foreach (var binding in data.director.playableAsset.outputs)
        {
            if (binding.sourceObject is MYCinemachineTrack)
            {
                SetCinemachineTrackData(binding, data.director);
                continue;
            }

            UnityEngine.Object bindingObj = data.director.GetGenericBinding(binding.sourceObject);
            if (bindingObj != null && bindingObj is RoleData)
            {
                RoleData roledata = bindingObj as RoleData;
                if (!needLoadRoleData.Contains(roledata))
                {
                    //LoadBindingRoleData(roledata);
                    needLoadRoleData.Add(roledata);
                }
            }
        }
        StartCoroutine(LoadBindingRoleData());
        //StartCoroutine(InitVirtualCamera(data));
    }


    private List<RoleData> needLoadRoleData = new List<RoleData>();
    private IEnumerator LoadBindingRoleData()
    {
        foreach (var role in needLoadRoleData)
        {
            yield return new WaitForSeconds(0.1f);
            LoadBindingRoleData(role);
        }
    }


    public void SetCinemachineTrackData(PlayableBinding binding,PlayableDirector director)
    {
        CinemachineBrain brain = CameraControll.Instance.gameObject.GetComponent<CinemachineBrain>();
        if (brain == null)
            brain = CameraControll.Instance.gameObject.AddComponent<CinemachineBrain>();
        director.SetGenericBinding(binding.sourceObject, brain);
        foreach (TimelineClip clip in (binding.sourceObject as TrackAsset).GetClips())
        {
            var shot = clip.asset as MYCinemachineShot;
            if (shot)
            {
                //var mainVm = brain.transform.FindChild("VM").GetComponent<CinemachineVirtualCamera>();
                //shot.VirtualCamera = new ExposedReference<CinemachineVirtualCameraBase>();
                //director.SetReferenceValue(shot.VirtualCamera.exposedName, mainVm);
                var vmGo=director.gameObject.transform.Find(shot.VmPath);
                if (vmGo)
                    shot.VirtualCamera = vmGo.GetComponent<CinemachineVirtualCameraBase>();
            }
        }
    }

    private void LoadBindingRoleData(RoleData roledata)
    {
        if (roledata.RoleType == TimelineRoleType.MainPlayer)
            World.Instance.AddMainplayer(roledata, roleObj =>
            {
                roleObj.transform.position = roledata.InitialPos;
                roleObj.transform.eulerAngles = roledata.InitialRotation;
                roledata.Role = roleObj;
                CreatAniClip(roleObj, roledata);
            });
        else
            World.Instance.CreatRoleObjIfHaveNot(roledata, roleObj =>
            {
                roleObj.transform.position = roledata.InitialPos;
                roleObj.transform.eulerAngles = roledata.InitialRotation;
                roledata.Role = roleObj;
                CreatAniClip(roleObj, roledata);
            });
    }

    private void CreatAniClip(RoleObject roleObj, RoleData roleData)
    {
        if (roleData.AnimClipPath == null)
            return;
        foreach (var path in roleData.AnimClipPath)
        {
            string pathAni=null;
            if (roleData.RoleType == TimelineRoleType.MainPlayer || roleData.RoleType == TimelineRoleType.Player)
            {
                if(AniClipPath.ContainsKey(roleData.RoleOcc))
                    pathAni = string.Format(AniClipPath[roleData.RoleOcc], path);
                else
                    pathAni = string.Format(AniClipPath[TimelineRoleOcc.ZhanShi], path);
            }
            else
                pathAni = path;
            Loader.Instance.CreatAniClips(pathAni, clip =>
            {
                AnimatorOverrideController controller = roleObj.Animator.runtimeAnimatorController as AnimatorOverrideController;
                controller[clip.name] = clip;
            });
        }
    }


    private IEnumerator InitVirtualCamera(LoadedTimelineData data)
    {
        yield return new WaitForSeconds(1);
        foreach (GameObject timelineChild in data.timelineGo.GetAllChildren())
        {
            CinemachineVirtualCameraBase cameraBase = timelineChild.GetComponent<CinemachineVirtualCameraBase>();
            if (cameraBase != null)
                SetVitrueCameraCompData(cameraBase);
        }
    }

    private void SetVitrueCameraCompData(CinemachineVirtualCameraBase virtualCamera)
    {
        RoleData roleData=null;
        if (virtualCamera.LookAt != null)
        {
            roleData = virtualCamera.LookAt.GetComponent<RoleData>();
            if (roleData != null)
            {
                RoleObject roleObj = World.Instance.GetRoleObj(roleData);
                virtualCamera.LookAt = roleObj.transform;
            }
        }
        roleData = null;
        if (virtualCamera.Follow != null)
        {
            roleData = virtualCamera.Follow.GetComponent<RoleData>();
            if (roleData != null)
            {
                RoleObject roleObj = World.Instance.GetRoleObj(roleData);
                virtualCamera.Follow = roleObj.transform;
            }
        }
    }



    public void GenerateMoveClip(PlayableDirector director, int trackIndex)
    {
        List<CurvePoint> points = GetPointsInScene();
        if (points.Count <= 0)
            return;
        string errorLog = "";
        TimelineAsset asset = director.playableAsset as TimelineAsset;
        TrackAsset track = asset.GetOutputTrack(trackIndex);
        if (track == null)
            errorLog = "Track Index Error";
        if (!(track is RoleMoveTrack))
            errorLog = "Track 类型错误，不是RoleMoveTrack";
        if (string.IsNullOrEmpty(errorLog))
        {
            List<Vector3> pointsPos = points.Select(point => point.transform.position).ToList();
            TimelineClip clip = track.CreateDefaultClip();
            RoleMoveClip moveclip = clip.asset as RoleMoveClip;
            moveclip.points = pointsPos;
            moveclip.roleData = director.GetGenericBinding(track) as RoleData;
        }
        else
            Debug.LogError(errorLog);
    }

    private List<CurvePoint> GetPointsInScene()
    {
        CurvePoint[] pointsArray;
        pointsArray = curvePointParent.transform.GetComponentsInChildren<CurvePoint>();
        if (pointsArray == null)
            return new List<CurvePoint>();
        else
            return pointsArray.ToList();
    }

    public void GenerateMovePoint(Vector3 pos)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameUtility.SetLayerRecusive(9, cube);
        cube.transform.localScale = Vector3.one;
        cube.transform.position = pos;
        cube.transform.parent = curvePointParent.transform;
        cube.AddComponent<CurvePoint>();
    }

}


public class LoadedTimelineData
{
    public PlayableDirector director;
    public GameObject prefabResource;
    public GameObject timelineGo;
}


