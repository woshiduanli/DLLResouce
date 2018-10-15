using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Model;

public class SkillContext
{
    public int Wave;
    public List<FireBoll> targets = new List<FireBoll>();

    public class FireBoll
    {
        public RoleObject ro;
        public int limit;
    }
}

//XYAction
public class ZWAction : PListNode, System.IDisposable
{
    public PList actionEvents = null;//动作事件
    public Action config;
    public ActionPerformer ActionPerformer_;
    public bool High;

    public float startTime = 0; //本Action开始时间
    public EventPart eventpart;
    private List<CActionEffect> effectdic = new List<CActionEffect>();
    private float LifeTime;
    public bool Disposed;
    private SkillContext context;
    public void Dispose()
    {
        if (Disposed) 
            return;
        Disposed = true;
        EndAction();
        if (Application.isEditor) 
            return;
        System.GC.SuppressFinalize(this);
    }

    public ZWAction(Action config, ActionPerformer af, EventPart ep, bool high, SkillContext context = null)
    {
        this.High = high;
        this.startTime = GameTimer.time;
        this.config = config;
        this.LifeTime = this.config.LifeTime;
        this.eventpart = ep;
        this.ActionPerformer_ = af;
        this.actionEvents = new PList();
        this.context = context;
        this.Initialize(af.owerObject);
    }

    public void Initialize(RoleObject parent)
    {
        for (int i = 0; i < config.ActionEvents.Length; ++i)
        {
            Action.ActionEvent actionevent = config.ActionEvents[i];
            if (actionevent.EventPart != eventpart)
                continue;
            List<ZWActionEvent> eventlist = null;
            float time = 0;
            float Duration = 0;
            int fireballNum = 0;
            switch (actionevent.EventType)
            {
                case ActionEventType.ANIMATION:
                    eventlist = ZWActionEvent.CreateEffect<ActionEventPlayAnimation>(parent, this, actionevent, out Duration);
                    break;
                case ActionEventType.EFFECT:
                    eventlist = ZWActionEvent.CreateEffect<ActionEventStartEffectOnSelf>(parent, this, actionevent, out Duration);
                    break;
                case ActionEventType.FIREBALL:
                    eventlist = ZWActionEvent.CreateEffect<ActionEventStartEffectFireBall>(parent, this, actionevent, out Duration, this.context, fireballNum);
                    fireballNum++;
                    break;
                case ActionEventType.AUDIO:
                    eventlist = ZWActionEvent.CreateEffect<ActionEventPlaySound>(parent, this, actionevent, out Duration);
                    break;
                case ActionEventType.CAMERA_SHACK:
                    eventlist = ZWActionEvent.CreateEffect<ActionEventCameraShakeEffect>(parent, this, actionevent, out Duration);
                    break;
                case ActionEventType.SCREEN_BLACK:
                    eventlist = ZWActionEvent.CreateEffect<ActionEventScreenEffect>(parent, this, actionevent, out Duration);
                    break;
                case ActionEventType.RADIALBLUR:
                    eventlist = ZWActionEvent.CreateEffect<ActionEventRadialBlur>(parent, this, actionevent, out Duration);
                    break;
                case ActionEventType.DEAD:
                    eventlist = ZWActionEvent.CreateEffect<ActionEventDead>(parent, this, actionevent, out Duration);
                    break;
                case ActionEventType.SLOW:
                    eventlist = ZWActionEvent.CreateEffect<ActionEventSlow>(parent, this, actionevent, out Duration);
                    break;
                case ActionEventType.HEATDISTORT:
                    eventlist = ZWActionEvent.CreateEffect<ActionEventStartEffectOnSelf>(parent, this, actionevent, out Duration);
                    break;
                case ActionEventType.HIDING:
                    eventlist = ZWActionEvent.CreateEffect<ActionEventHiding>(parent, this, actionevent, out Duration);
                    break;
                case ActionEventType.TRANSFORM:
                    eventlist = ZWActionEvent.CreateEffect<ActionEventTransform>(parent, this, actionevent, out Duration);
                    break;
                case ActionEventType.XPBODYCHANGE:
                    eventlist = ZWActionEvent.CreateEffect<ActionEventXPBodyChange>(parent, this, actionevent, out Duration);
                    break;
            }
            time = Mathf.Max(Duration, time);
            for (int j = 0; j < eventlist.Count; j++)
                this.actionEvents.AddTail(eventlist[j]);

            this.LifeTime += time;
        }
    }

    public void Update()
    {
        if (startTime <= 0)
            return;

        if (!GameTimer.Within(startTime, this.LifeTime))
        {
            Dispose();
            return;
        }

        for( PListNode pos = this.actionEvents.next, next;pos != this.actionEvents; pos = next )
        {
             next = pos.next;//先保存下一个节点
             ZWActionEvent ae = ( ZWActionEvent )pos;
             if (ae.Disposed)
             {
                 this.actionEvents.Remove(ae);
                 return;
             }
             if (ae.Active)
                 ae.DoEvent();
             ae.Update();
        }
    }

    //结束Action
    void EndAction()
    {
        for (PListNode pos = this.actionEvents.next; pos != this.actionEvents; pos = pos.next)
        {
            ZWActionEvent ae = (ZWActionEvent)pos;
            ae.Dispose();
        }

        if (ActionPerformer_)
            ActionPerformer_.RemoveZWAction(this);
    }

    public void AddEffect(CActionEffect effect)
    {
        if (effect == null)
            return;
        for (int i = 0; i < effectdic.Count; i++)
        {
            CActionEffect ae = effectdic[i];
            if (ae == null)
                continue;
        }
        effectdic.Add(effect);
    }
}
