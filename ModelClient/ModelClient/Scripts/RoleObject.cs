using UnityEngine;
using Model;
using System.Collections.Generic;
using System.Collections;
using System;

public class RoleObject : MonoBehaviour 
{
    public List<Animation> Animations = new List<Animation>();
    public Role Config;
    public Animator Animator;
    private List<AnimationClip> clips;
    public ActionPerformer mActionPerformer;
    public List<GameObject> Skins = new List<GameObject>();
    public bool needAnimationButtonGui=true;
    private string Btnname = string.Empty;
    public float MoveSpeed;
    public float StartTime;
    public float pauseBeginTime;
    public float TotalPause;
    public float OffsetTime;
    public Vector3 BoxCollider
    {
        get
        {
            if (!Config || !Config.BoxCollider)
                return Vector3.zero;
            return Config.BoxCollider.transform.lossyScale;
        }
    }
    public Transform Transform
    {
        get
        {
            return this.gameObject.transform;
        }
    }
    public Vector3 position
    {
        get
        {
            return this.gameObject.transform.position;
        }
    }
    public int modelLayer;

    private bool isActionMove = false;
    private bool isNormalMove = false;


    string resetTriggerName;
    public void CheckAnimator()
    {
        if (!Config)
            return;
        if (!Animator)
            Animator = this.gameObject.GetComponent<Animator>();
        if (!Animator)
            return;
        if (clips == null || clips.Count == 0)
        {
            clips = new List<AnimationClip>();
            Animator = gameObject.GetComponent<Animator>();
            if (Animator)
            {
                Animator.updateMode = AnimatorUpdateMode.Normal;
                Animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;

                if (!Animator.runtimeAnimatorController)
                {
                    Debug.LogError(String.Format("{0}Animator Erro", gameObject.name));
                    UnityEngine.Object.Destroy(Animator);
                    return;
                }
                if (Config.AnimiClips != null && Config.AnimiClips.Length > 0)
                {
                    AnimatorOverrideController overrideController = new AnimatorOverrideController(Animator.runtimeAnimatorController);
                    Animator.runtimeAnimatorController = overrideController;
                    for (int j = 0; j < Config.AnimiClips.Length; j++)
                    {
                        overrideController[Config.AnimiClips[j].Key] = Config.AnimiClips[j].Clip;
                        clips.Add(Config.AnimiClips[j].Clip);
                    }

                }
                else//兼容没有重新打Role的资源
                {
                    AnimatorOverrideController Controller = Animator.runtimeAnimatorController as AnimatorOverrideController;
                    List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
                    Controller.GetOverrides(overrides);
                    for (int m = 0; m < overrides.Count; m++)
                    {
                        KeyValuePair<AnimationClip, AnimationClip> kvp = overrides[m];
                        if (kvp.Value == null) continue;
                        clips.Add(kvp.Value);
                    }
                }
            }
        }
        //if (clips == null || clips.Length == 0)
        //    clips = Animator.runtimeAnimatorController.animationClips;
        if (clips == null)
            return;
        if (!needAnimationButtonGui)
            return;
        GUI.Box(new Rect(0, 0, 120, clips.Count * 30 + 25), "播放动画");
        resetTriggerName=GUI.TextArea(new Rect(320, 0, 150, 30),resetTriggerName);
        if (GUI.Button(new Rect(170, 0, 150, 25), "ResetTrigger"))
        {
            ResetTrigger();
        }
        if (GUI.Button(new Rect(170, 30, 150, 25), "Play"))
        {
            PlayTrigger();
        }
        int i = 0;
        foreach (var clip in clips)
        {
            if (GUI.Button(new Rect(0, 30 * i + 25, 150, 25), clip.name + "   " + clip.length))
            {

                ResetTime();
                PlayMotion(clip.name);
            }

            i++;
        }
    }

    public void ResetTrigger()
    {
        if (Animator)
            Animator.ResetTrigger(resetTriggerName);
    }

    public void PlayTrigger()
    {
        if (Animator)
            Animator.Play(resetTriggerName);
    }

    public void ResetTime()
    {
        pauseBeginTime = 0;
        StartTime = GameTimer.time;
        TotalPause = 0;
    }

    public void PlayMotion(string name)
    {
        if (Animator)
            Animator.SetTrigger(name);

        foreach (Animation ani in Animations)
            ani.Play(name);
    }

    public void Move(float speed,float endtime)
    {
        this.MoveSpeed = speed;
        isNormalMove = true;
        //Invoke("StopMove", endtime);
    }

    void StopMove()
    {
        isNormalMove = false;
        this.MoveSpeed = 0;
        this.transform.position = Vector3.zero;
    }

    private void DoMove()
    {
        if (this.MoveSpeed == 0)
            return;
        Vector3 pos = this.transform.position;
        pos.z += Time.deltaTime * this.MoveSpeed;
        this.transform.position = pos;
    }

    public void StarActionEventMove(float speed)
    {
        MoveSpeed = speed;
        isActionMove = true;
    }

    private void ActionEventMove()
    {
        if (this.MoveSpeed == 0)
            return;

        Vector3 pos = Transform.position;
        pos.z += MoveSpeed * Time.deltaTime;//Transform.position.z;
        Transform.position = pos;
    }

    public void StopActionEventMove()
    {
        isActionMove = false;
        MoveSpeed = 0;
    }

    void OnGUI()
    {
        if (needAnimationButtonGui)
        {
            if (UnityEditor.EditorApplication.isPaused)
            {
                if (pauseBeginTime == 0)
                    pauseBeginTime = GameTimer.time;
            }

            GUI.Label(new Rect(Screen.width / 2, 20, 100, 100), "Time:" + OffsetTime.ToString());
        }
        CheckAnimator();
        //DoMove();
    }

    private void Update()
    {
        if (needAnimationButtonGui)
        {
            if (pauseBeginTime > 0)
            {
                TotalPause += (GameTimer.time - pauseBeginTime);
                pauseBeginTime = 0;
            }
            OffsetTime = GameTimer.time - StartTime - TotalPause;
        }
    }

    void LateUpdate()
    {
        //if (isNormalMove)
        //   DoMove();

        if (isActionMove)
            ActionEventMove();
    }

    #region 根据给予的名称找出子节点 FindChildWithName FindChildTransformWithName
    public static GameObject FindChildWithName(GameObject parentGO, string childName)
    {
        if (parentGO.name == childName)
        {
            return parentGO;
        }
        foreach (Transform tf in parentGO.transform)
        {
            GameObject result = FindChildWithName(tf.gameObject, childName);
            if (result)
                return result;
        }
        return null;
    }

    public static Transform FindChildTransformWithName(GameObject parentGO, string childName)
    {
        if (!parentGO || string.IsNullOrEmpty(childName))
            return null;

        if (parentGO.name == childName)
            return parentGO.transform;

        foreach (Transform tf in parentGO.transform)
        {
            GameObject result = FindChildWithName(tf.gameObject, childName);
            if (result)
                return result.transform;
        }
        return null;
    }
    #endregion
}