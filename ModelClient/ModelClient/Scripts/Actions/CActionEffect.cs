using System;
using System.Collections.Generic;
using UnityEngine;
using Model;
using Object = UnityEngine.Object;
using System.Collections;
using DG.Tweening;

public class CActionEffect 
{
    protected ZWActionEvent host;
    protected RoleObject parent;
    public string sourcePath;
    private float start = 0;
    protected float duration = 0;
    protected bool isdone;
    private Object asset;
    public GameObject gameObject;
    public bool destroy;
    public Vector3 Angle, Offset;
    protected Vector3 targetPos = Vector3.zero;
    protected float MaxDistance = 1000;   //最大追踪距离
    
    public CActionEffect(ZWActionEvent host, string path)
    {
        this.sourcePath = path;
        this.host = host;
        this.parent = host.parent;
        this.Angle = host.Angle;
        this.Offset = host.Offset;
    }
    public static T CreateEffect<T>(string path, params object[] args) where T : CActionEffect
    {
        T result = null;
        if (args == null)
            result = Activator.CreateInstance(typeof(T)) as T;
        else
            result = Activator.CreateInstance(typeof(T), args) as T;

        result.asset = EditorWww.Create(path);
        result.OnCreate();
        return result;
    }

    public Object GetAsset()
    {
        return asset;
    }

    protected void OnCreate()
    {
        if (parent == null)//目标丢失
            return;
        this.start = GameTimer.time;

        duration = host.config.EndTime;
        if (this.asset is GameObject)
        {
            this.gameObject = UnityEngine.Object.Instantiate(this.asset) as GameObject;
            EffectQuality effectQuality = this.gameObject.GetComponent<EffectQuality>();
            if (effectQuality)
                effectQuality.Show = true;
            DestoryByTime component = this.gameObject.GetComponent<DestoryByTime>();
            if (component != null)
            {
                duration = component.time;
                Object.DestroyImmediate(component);
            }
        }
        OnInit();
        isdone = true;
    }

    /// <summary>
    /// 效果初始化
    /// </summary>
    protected virtual void OnInit()
    {
        if (!parent)
            return;

        Transform parentTF = null;
        if (host.config.IsRatioOffset)
            Offset = new Vector3(parent.BoxCollider.x * Offset.x, parent.BoxCollider.y * Offset.y, parent.BoxCollider.z * Offset.z);
        if (host.config.BindBone != "world")
        {
            if (!string.IsNullOrEmpty(host.config.BindBone))
            {
                GameObject bindgo = RoleObject.FindChildWithName(parent.gameObject, host.config.BindBone);
                if (bindgo)
                    parentTF = bindgo.transform;
            }
            if (!parentTF)
                parentTF = parent.transform;

            SetParentTransform(parentTF);
            SetRotation(this.Angle);
            SetPosition(this.Offset);
        }
        else
        {
            SetRotation(this.Angle + parent.transform.eulerAngles);
            SetPosition(this.Offset + parent.transform.position + parent.transform.forward * host.config.Distance);
        }

        if (host.config.Speed <= 0)
            return;
        this.targetPos = this.gameObject.transform.position + (Quaternion.Euler(this.Angle) * parent.transform.forward) * MaxDistance;
        this.gameObject.transform.LookAt(this.targetPos);//向目标飞行
    }


    protected virtual float DoMove()
    {
        if (host.config.Speed <= 0)
            return 0;
        float distance = Vector3.Distance(this.targetPos, this.gameObject.transform.position);
        this.gameObject.transform.Translate(0, 0, Mathf.Min(this.host.config.Speed * Time.deltaTime + 0.5f * this.host.config.Speed * 0.01f * Time.deltaTime * Time.deltaTime, distance), Space.Self);
        return distance;
    }

    public void Destroy()
    {
        OnDestroy();
    }

    protected virtual void OnDestroy()
    {
        destroy = true;
        if (this.gameObject != null)
        {
            Object.DestroyImmediate(gameObject);
            this.gameObject = null;
        }
    }

    public void SetParentTransform(Transform ts)
    {
        if (ts == null)
            return;
        if (this.gameObject)
            this.gameObject.transform.parent = ts;
        SetPosition(Vector3.zero);
        SetRotation(Vector3.zero);
        SetScale(Vector3.one);
    }

    public void SetPosition(Vector3 position)
    {
        if (this.gameObject != null)
            this.gameObject.transform.localPosition = position;
    }

    public void SetRotation(Vector3 euler)
    {
        if (this.gameObject != null)
            this.gameObject.transform.localEulerAngles = euler;
    }

    public virtual void SetScale(Vector3 scale)
    {
        if (this.gameObject)
            this.gameObject.transform.localScale = scale;
    }

    public void Update()
    {
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {
        if (destroy)
        {
            Destroy();
            return;
        }

        if (!isdone)
            return;

        if (parent == null )//目标丢失
        {
            Destroy();
            return;
        }

        if (host.farther == null || host.farther.Disposed)
        {
            Destroy();
            return;
        }

        DoMove();

        if (!DoCheck())
        {
            Destroy();
            return;
        }

        if (this.start != 0 && this.duration != 0)
        {
            if (!GameTimer.Within(this.start, this.duration))
            {
                Destroy();
                this.duration = 0;
            }
        }
    }

    /// <summary>
    /// 执行判断
    /// </summary>
    /// <returns>false表示停止执行</returns>
    protected virtual bool DoCheck() { return true; }

}

/// <summary>
/// 播放音效
/// </summary>
public class CActionSound : CActionEffect
{
    public CActionSound(ZWActionEvent host, string path) : base(host, path) { }

    protected override void OnInit()
    {
        AudioClip ac = GetAsset() as AudioClip;
        if (ac)
        {
            this.gameObject = new GameObject();
            this.gameObject.name = ac.name;
            SetParentTransform(parent.transform);
            AudioSource audios = this.gameObject.AddComponent<AudioSource>();
            audios.dopplerLevel = 0;
            audios.spatialBlend = 1.0f;
            audios.minDistance = 5.0f;
            audios.maxDistance = 20.0f;
            audios.rolloffMode = AudioRolloffMode.Linear;
            audios.clip = ac;
            audios.volume = 1;
            audios.Play();
        }
        base.OnInit();
    }
}

public class CActionNoTargetFireBallEffect : CActionEffect
{
    public CActionNoTargetFireBallEffect(ZWActionEvent host, string path) : base(host, path) { }
    private int count;
    private int limit;

    protected override void OnInit()
    {
        limit = host.fireBoll.limit;
        Vector3 pos = parent.position;
        pos.y += parent.BoxCollider.y / 2;
        if (host.config.IsRatioOffset)
            Offset = new Vector3(parent.BoxCollider.x * Offset.x, parent.BoxCollider.y * Offset.y, parent.BoxCollider.z * Offset.z);
        this.gameObject.transform.position = pos + this.Offset;

        this.targetPos = this.gameObject.transform.position + (Quaternion.Euler(this.Angle) * parent.transform.forward) * MaxDistance;
        this.gameObject.transform.LookAt(this.targetPos);//向目标飞行

        ColliderEventComponent colliderEvent = this.gameObject.AddComponent<ColliderEventComponent>();
        colliderEvent.isTrigger = true;
        colliderEvent.onTriggerEnter += OnTriggerEnter;
    }

    private void OnTriggerEnter(GameObject sender, Collider other)
    {
        if (!other && !other.gameObject)
            return;
        RoleObject target = other.gameObject.transform.root.GetComponentInChildren<RoleObject>();
        if (!target)
            return;
        count++;
        target.mActionPerformer.StartAction(host.farther.config, EventPart.FIREBALLHIT, host.farther.High);
    }

    protected override bool DoCheck()
    {
        if (this.gameObject == null)
            return false;
        if (count >= limit)
            return false;
        return true;
    }
}

public class CActionFireBallEffect : CActionEffect
{
    private RoleObject target;
    private int count;
    private int limit;
    public CActionFireBallEffect(ZWActionEvent host, string path) : base(host, path) { }

    protected override void OnInit()
    {
        target = host.fireBoll.ro;
        //Debug.Log(target.transform.position);
        limit = host.fireBoll.limit;
        if (!target)
            return;

        Vector3 pos = parent.position;
        pos.y += parent.BoxCollider.y / 2;
        if (host.config.IsActOnTarget)
        {
            pos = target.transform.position;
        }
        if (host.config.IsRatioOffset)
            Offset = new Vector3(parent.BoxCollider.x * Offset.x, parent.BoxCollider.y * Offset.y, parent.BoxCollider.z * Offset.z);
        this.gameObject.transform.position = pos + this.Offset + this.gameObject.transform.forward * host.config.Distance;
        MaxDistance = Vector3.Distance(target.position, this.gameObject.transform.position) - target.BoxCollider.z / 2;
        //Vector3 forward = (target.position - this.gameObject.transform.position).normalized;
        //forward.y = 0;

        //this.targetPos = this.gameObject.transform.position + (Quaternion.Euler(this.Angle) * forward) * MaxDistance;
        //this.targetPos.y = target.position.y + target.BoxCollider.y / 2;
        this.targetPos = target.transform.position;
        this.gameObject.transform.LookAt(this.targetPos);//向目标飞行

        float speed = host.config.Speed;
        if (speed <= 0)
            speed = 1;

        //路径点
        Vector3[] paths = new Vector3[] { this.targetPos / 2, this.targetPos };
        DG.Tweening.PathType pathtype = DG.Tweening.PathType.Linear;
        if (host.PathType != Model.PathType.Linear)
        {
            pathtype = DG.Tweening.PathType.CatmullRom;
            switch (host.PathType)
            {
                case Model.PathType.CatmullRom:
                    paths[0].y = this.gameObject.transform.position.y + host.OffsetMaxY;
                    break;
                case Model.PathType.LeftCatmullRom:
                    paths[0].x -= host.OffsetMaxY;
                    break;
                case Model.PathType.RightCatmullRom:
                    paths[0].x += host.OffsetMaxY;
                    break;
            }
        }
        Debug.Log(paths);
        Debug.Log(MaxDistance + "/" + host.config.Speed + "=" + MaxDistance / host.config.Speed);
        DG.Tweening.Tween tween = null;
        if (pathtype == DG.Tweening.PathType.Linear)
           tween = this.gameObject.transform.DOPath(new Vector3[] { targetPos }, MaxDistance / host.config.Speed, pathtype).SetLookAt(0.001f);
        else
            tween = this.gameObject.transform.DOPath(paths, MaxDistance / host.config.Speed, pathtype).SetLookAt(0.001f);
        tween.OnComplete(() => target.mActionPerformer.StartAction(host.farther.config, EventPart.FIREBALLHIT,host.farther.High));

        ColliderEventComponent colliderEvent = this.gameObject.AddComponent<ColliderEventComponent>();
        colliderEvent.isTrigger = true;
        colliderEvent.onTriggerEnter += OnTriggerEnter;
    }

    private void OnTriggerEnter(GameObject sender, Collider other)
    {
        if (!other  && !other.gameObject )
            return;
        RoleObject target = other.gameObject.transform.root.GetComponentInChildren<RoleObject>();
        if (!target)
            return;
        count++;
        target.mActionPerformer.StartAction(host.farther.config, EventPart.FIREBALLHIT, host.farther.High);
    }


    // protected override bool DoCheck()
    // {
    //    if (host.config.Child.Length <= 0)
    //        return false;
    //    if (!target)
    //        return false;
    //    if (this.gameObject == null)
    //        return false;
    //    if (count >= limit)
    //        return false;
    //    if (DoMove() <= 0.1f)
    //        return false;

    //    return true;
    //}
}

public class CActionScreenEffect:CActionEffect
{
    public CActionScreenEffect(ZWActionEvent host, string path) : base(host, path) { }
    protected override void OnInit()
    {
        if (!Camera.main)
            return;
        SetParentTransform(Camera.main.transform);
        Camera camera = this.gameObject.GetComponentInChildren<Camera>();
        if (camera)
        {
            camera.backgroundColor = Camera.main.backgroundColor;
            camera.cullingMask = Camera.main.cullingMask;
        }
    }
}

public class CActionCameraShakeEffect : CActionEffect
{
    public CActionCameraShakeEffect(ZWActionEvent host, string path) : base(host, path) { }
    protected override void OnInit()
    {
        if (!Camera.main)
            return;
        SetParentTransform(Camera.main.transform);

        bool rb = false;
        foreach (var ca in Camera.allCameras)
        {
            if (ca.GetComponent<RadialBlur>())
            {
                rb = true;
                break;
            }

        }

        Camera camera = this.gameObject.AddComponent<Camera>();
        if (camera)
        {
            camera.clearFlags = CameraClearFlags.Skybox;
            camera.backgroundColor = Camera.main.backgroundColor;
            camera.cullingMask = Camera.main.cullingMask;
            camera.depth = 100;
        }
        if (rb)
            this.gameObject.AddComponent<RadialBlur>();
    }
}

public class CActionDead : CActionEffect
{
    public CActionDead(ZWActionEvent host, string path) : base(host, path) { }
    protected override void OnInit()
    {
        DissolveBurn db = DissolveBurn.Begin(parent.gameObject, 0.2f, 0, 1.2f);
        Renderer[] renders = parent.gameObject.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renders.Length; i++)
        {
            if (renders[i] && renders[i].material)
                db.SetMats(renders[i].material, GetAsset() as Texture, Color.red);
        }
    }
}