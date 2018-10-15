using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;
using Cinemachine.Timeline;

public sealed class MYCinemachineShot : PlayableAsset, IPropertyPreview
{
    [HideInInspector]
    public CinemachineVirtualCameraBase VirtualCamera;
    public string VmPath;
    public string LookAtBoneName;
    public string FollowBoneName;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        GetCinemachineVm(owner);

        var playable = ScriptPlayable<CinemachineShotPlayable>.Create(graph);
        var behaviour = playable.GetBehaviour();
        if (behaviour == null)
            return playable;
        behaviour.VirtualCamera = VirtualCamera;
        behaviour.LookAtBoneName = LookAtBoneName;
        behaviour.FollowBoneName = FollowBoneName;
        return playable;
    }

    public CinemachineVirtualCameraBase GetCinemachineVm(GameObject rootGo)
    {
        var cameraGo = rootGo.transform.Find(VmPath);
        if (cameraGo != null)
        {
            VirtualCamera = cameraGo.GetComponent<CinemachineVirtualCameraBase>();
            return VirtualCamera;
        }
        else
        {
            Debug.LogError("未能找到对应路径的虚拟相机");
            return null;
        }
    }

    // IPropertyPreview implementation
    public void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
        driver.AddFromName<Transform>("m_LocalPosition.x");
        driver.AddFromName<Transform>("m_LocalPosition.y");
        driver.AddFromName<Transform>("m_LocalPosition.z");
        driver.AddFromName<Transform>("m_LocalRotation.x");
        driver.AddFromName<Transform>("m_LocalRotation.y");
        driver.AddFromName<Transform>("m_LocalRotation.z");

        driver.AddFromName<Camera>("field of view");
        driver.AddFromName<Camera>("near clip plane");
        driver.AddFromName<Camera>("far clip plane");
    }
}

