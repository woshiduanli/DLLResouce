using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Model;
using System;
using Action = Model.Action;
using UnityEditor;
using DG.Tweening;
//Action事件
public class ZWActionEvent : PListNode
{
    public static List<ZWActionEvent> CreateEffect<T>(RoleObject ro, ZWAction farther, Action.ActionEvent aevent, out float Duration, SkillContext context = null,int index=0, params object[] args) where T : ZWActionEvent
    {
        Duration = 0;
        List<ZWActionEvent> list = new List<ZWActionEvent>();
        T result = null;
        SkillContext.FireBoll fireBoll = null;
        int count = aevent.Child.Length + 1;
        int wave = 1;
        if (context != null)
        {
            if (aevent.EventType == ActionEventType.FIREBALL)
            {
                if (context.targets.Count > index)
                    fireBoll = context.targets[index];
            }
            wave = context.Wave;
        }

        for (int i = 0; i < wave; i++)
        {
            for (int j = 0; j < count; j++)
            {
                if (args == null)
                    result = Activator.CreateInstance(typeof(T)) as T;
                else
                    result = Activator.CreateInstance(typeof(T), args) as T;

                Action.Childs child = null;

                if (j > 0 && aevent.Child.Length > j - 1)
                    child = aevent.Child[j - 1];

                result.config = aevent;
                result.fireBoll = fireBoll;
                float time = result.Initialize(ro, farther, child, aevent.Duration * i);
                Duration = Mathf.Max(time, Duration);
                list.Add(result);
            }
        }
        return list;
    }

    public float bornTime = 0;
    public bool isDoEvent;
    public ZWAction farther { protected set; get; }
    public RoleObject parent { protected set; get; }
    public bool Active = true;
    private List<CActionEffect> effectdic = new List<CActionEffect>();
    public Action.ActionEvent config;
    public Vector3 Angle, Offset;
    protected float StartTime;
    public SkillContext.FireBoll fireBoll;
    public Model.PathType PathType;
    public float OffsetMaxY;

    public bool Disposed;
    public void Dispose()
    {
        if (Disposed)
            return;
        OnDestroy();
        Disposed = true;
        Reset();
        effectdic = null;
    }

    public void Reset()
    {
        for (int i = 0; i < effectdic.Count; i++)
        {
            CActionEffect ae = effectdic[i];
            if (ae == null)
                continue;
            ae.Destroy();
        }
        effectdic.Clear();
    }

    //初始化
    public float Initialize(RoleObject ro, ZWAction farther, Action.Childs child, float Duration)
    {
        this.parent = ro;
        this.farther = farther;
        this.Angle = config.Angle;
        this.Offset = config.Offset;
        this.StartTime = this.config.StartTime;
        this.PathType = config.PathType;
        this.OffsetMaxY = config.OffsetMaxY;
        if (child != null)
        {
            this.Angle += child.Angle;
            this.Offset += child.Offset;
            Duration += child.Duration;
            this.PathType = child.PathType;
            this.OffsetMaxY = child.OffsetMaxY;
        }
        this.StartTime += Duration;
        OnInit();
        return Duration;
    }

    public virtual void OnInit() { }

    //执行事件 返回false表示该时间已经结束，true表示正在执行
    public virtual void DoEvent() { }

    public virtual void StopEvent() { }

    public virtual void OnDestroy() { }
    public virtual void OnUpdate() { }
    public void Update()
    {
        for (int i = 0; i < effectdic.Count; i++)
        {
            CActionEffect ae = effectdic[i];
            if (ae == null)
            {
                effectdic.RemoveAt(i);
                break;
            }
            ae.Update();
        }

        OnUpdate();

        //防止同一帧执行StopEvent和DoEvent
        if (isDoEvent)
        {
            isDoEvent = false;
            return;
        }
        if (bornTime > 0 && this.config.EndTime > 0 && !GameTimer.Within(bornTime, this.config.EndTime))
        {
            StopEvent();
            Dispose();
        }
    }

    protected T CreateEffect<T>(string source = null) where T : CActionEffect
    {
        T effect = CActionEffect.CreateEffect<T>(source, this, source);
        if (effect == null)
            return null;
        effectdic.Add(effect);
        farther.AddEffect(effect);
        return effect;
    }
}
//ActionEvent的派生类

public class ActionEventPlayAnimation : ZWActionEvent//播放动画
{
    List<Move> DataQueue = new List<Move>();
    private Vector3 Tpos = Vector3.zero;
    private Action.MoveData Data;
    public override void DoEvent()
    {
        if (GameTimer.Within(farther.startTime, this.StartTime))
            return;

        foreach(var d in this.config.MoveDatas)
            DataQueue.Add(new Move(d, parent.transform));

        isDoEvent = true;
        Active = false;
        bornTime = GameTimer.time;
        parent.PlayMotion(this.config.State.ToString());

        //parent.Move(this.config.Speed, this.config.EndTime);
    }

    public override void OnUpdate()
    {
        foreach( var date in DataQueue)
        {
            if(date.DoEvent())
            {
                DataQueue.Remove(date);
                break;
            }
        }
    }

    public override void StopEvent()
    {
        parent.PlayMotion(MotionState.stand.ToString());
    }

    public class Move
    {
        public float bornTime;
        public Action.MoveData Data;
        public Transform transform;
        public Move(Action.MoveData data, Transform t)
        {
            this.bornTime = 0;
            this.Data = data;
            this.transform = t;
        }

        public bool DoEvent()
        {
            this.bornTime += Time.deltaTime;
            if (this.bornTime < Data.StartTime)
                return false;
            Vector3 Tpos = this.transform.position + this.transform.forward * Data.Distance;
            this.transform.DOMove(Tpos, Data.EndTime- Data.StartTime);
            return true;
        }
    }
}

public class ActionEventPlaySound : ZWActionEvent//播放音效
{
    public override void DoEvent()
    {
        if (GameTimer.Within(farther.startTime, this.StartTime))
            return;
        Active = false;
        isDoEvent = true;
        CreateEffect<CActionSound>(this.config.AudioPath);
    }
}

public class ActionEventStartEffectOnSelf : ZWActionEvent//在单位直接播放一个特效
{
    public override void DoEvent()
    {
        if (GameTimer.Within(farther.startTime, this.StartTime))
            return;
        Active = false;
        isDoEvent = true;

        string path = this.config.SourcePath;
        if (!farther.High && !string.IsNullOrEmpty(this.config.LowSourcePath))
            path = this.config.LowSourcePath;

        CreateEffect<CActionEffect>(path);

        if (!string.IsNullOrEmpty(this.config.AudioPath))
            CreateEffect<CActionSound>(this.config.AudioPath);
    }
}

public class ActionEventStartEffectFireBall : ZWActionEvent//播放一个火球类特效
{
    public override void DoEvent()
    {
        if (GameTimer.Within(farther.startTime, this.StartTime))
            return;
        Active = false;
        isDoEvent = true;
        string path = this.config.SourcePath;
        if (!farther.High && !string.IsNullOrEmpty(this.config.LowSourcePath))
            path = this.config.LowSourcePath;

        if (!fireBoll.ro)
        {
            Debug.Log("没有目标直接飞");
            CreateEffect<CActionNoTargetFireBallEffect>(this.config.SourcePath);
        }
        else
        {
            Debug.Log("CActionFireBallEffect");
            CreateEffect<CActionFireBallEffect>(this.config.SourcePath);
        }
        if (!string.IsNullOrEmpty(this.config.AudioPath))
            CreateEffect<CActionSound>(this.config.AudioPath);
    }
}

public class ActionEventScreenEffect : ZWActionEvent//开启特殊视觉效果
{
    public override void DoEvent()
    {
        if (GameTimer.Within(farther.startTime, this.StartTime))
            return;
        Active = false;
        isDoEvent = true;
        CreateEffect<CActionScreenEffect>(this.config.SourcePath);

        if (!string.IsNullOrEmpty(this.config.AudioPath))
            CreateEffect<CActionSound>(this.config.AudioPath);
    }
}

/// <summary>
/// 震屏
/// </summary>
public class ActionEventCameraShakeEffect : ZWActionEvent
{
    public override void DoEvent()
    {
        if (GameTimer.Within(farther.startTime, this.StartTime))
            return;
        Active = false;
        isDoEvent = true;
        CreateEffect<CActionCameraShakeEffect>(this.config.SourcePath);

        if (!string.IsNullOrEmpty(this.config.AudioPath))
            CreateEffect<CActionSound>(this.config.AudioPath);
    }
}

/// <summary>
/// 直线模糊
/// </summary>
public class ActionEventRadialBlur : ZWActionEvent
{
    public override void DoEvent()
    {
        if (GameTimer.Within(farther.startTime, this.StartTime))
            return;
        Active = false;
        isDoEvent = true;
        bornTime = GameTimer.time;

        if (!string.IsNullOrEmpty(this.config.AudioPath))
            CreateEffect<CActionSound>(this.config.AudioPath);

        foreach (var ca in  Camera.allCameras)
        {
            ca.gameObject.AddComponent<RadialBlur>();
        }
    }

    public override void StopEvent()
    {
        foreach (var ca in Camera.allCameras)
        {
            RadialBlur rg = ca.gameObject.GetComponent<RadialBlur>();
            if (rg)
                UnityEngine.Object.Destroy(rg);
        }
    }

    public override void OnDestroy()
    {
        StopEvent();
    }
}

public class ActionEventSlow : ZWActionEvent
{
    public override void DoEvent()
    {
        if (GameTimer.Within(farther.startTime, this.StartTime))
            return;
        Active = false;
        isDoEvent = true;
        bornTime = GameTimer.time;

        if (!string.IsNullOrEmpty(this.config.AudioPath))
            CreateEffect<CActionSound>(this.config.AudioPath);

        Time.timeScale = this.config.SlowPower;
    }

    public override void StopEvent()
    {
        Time.timeScale = 1;
    }

    public override void OnDestroy()
    {
        StopEvent();
    }
}

public class ActionEventDead : ZWActionEvent
{
    public override void DoEvent()
    {
        if (GameTimer.Within(farther.startTime, this.StartTime))
            return;
        Active = false;
        isDoEvent = true;
        CreateEffect<CActionDead>(this.config.SourcePath);

        if (!string.IsNullOrEmpty(this.config.AudioPath))
            CreateEffect<CActionSound>(this.config.AudioPath);
    }
}

/// <summary>
/// 隐藏模型
/// </summary>
public class ActionEventHiding : ZWActionEvent
{
    public override void DoEvent()
    {
        if (GameTimer.Within(farther.startTime, this.StartTime))
            return;

        Active = false;
        isDoEvent = true;
        bornTime = GameTimer.time;

        for (int i = 0; i < parent.Skins.Count; i++)
        {
            if (parent.Skins[i])
                parent.Skins[i].SetActive(false);
        }
        if (parent.Config.Skin)
            parent.Config.Skin.gameObject.SetActive(false);
        if (parent.Config.HighSkin)
            parent.Config.HighSkin.gameObject.SetActive(false);
    }

    public override void StopEvent()
    {
        base.StopEvent();
        for (int i = 0; i < parent.Skins.Count; i++)
        {
            if (parent.Skins[i])
                parent.Skins[i].SetActive(true);
        }
        if (parent.Config.Skin)
            parent.Config.Skin.gameObject.SetActive(true);
        if (parent.Config.HighSkin)
            parent.Config.HighSkin.gameObject.SetActive(true);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        StopEvent();
    }
}

/// <summary>
/// 微操
/// </summary>
public class ActionEventTransform : ZWActionEvent
{
    float lastz;
    bool isinit;
    public override void OnInit()
    {
        base.OnInit();

        parent.Transform.position = Vector3.zero;
        lastz = 0;
        isinit = true;
        bornTime = GameTimer.time;
    }

    public override void DoEvent()
    {
        if (GameTimer.Within(farther.startTime, this.StartTime))
            return;

        Active = false;
        isDoEvent = true;
        bornTime = GameTimer.time;
    }

    public override void StopEvent()
    {
        base.StopEvent();
        parent.StopActionEventMove();
        lastz = 0;
        isinit = false;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        StopEvent();
    }
}

public class ActionEventXPBodyChange : ZWActionEvent
{
    bool IsDo = false;
    RoleObject xpModel;

    public override void DoEvent()
    {
        if (GameTimer.Within(farther.startTime, this.StartTime))
            return;
        Active = false;
        isDoEvent = true;
        bornTime = GameTimer.time;
        if (IsDo)
            return;
        IsDo = true;
        GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(string.Format("Assets/res_sourcefile/role/{0}.prefab", config.CallApprid));
        Role role = obj.GetComponent<Role>();
        xpModel = ModelLoader.CreateRole(role,false);
        for (int i = 0; i < parent.Skins.Count; i++)
        {
            if (parent.Skins[i])
                parent.Skins[i].SetActive(false);
        }
    }

    public override void StopEvent()
    {
        base.StopEvent();
        for (int i = 0; i < parent.Skins.Count; i++)
        {
            if (parent.Skins[i])
                parent.Skins[i].SetActive(true);
        }
        GameObject.DestroyImmediate(xpModel.gameObject);
        xpModel = null;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        StopEvent();
    }
}