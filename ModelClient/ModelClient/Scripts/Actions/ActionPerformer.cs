using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using Action = Model.Action;
public class ActionPerformer : MonoBehaviour
{
    private PList actionList_ = null;
    private PListNode start_;
    public RoleObject owerObject;
    public void Awake() 
    {
        this.actionList_ = new PList();
        this.start_ = this.actionList_;
        this.actionList_.Init();
    }

    public void RemoveZWAction( ZWAction action ) 
    {
        if ( this.start_ == action )
            this.start_ = action.prev;
        this.actionList_.Remove( action );
    }

    public void Update() 
    {
        for ( PListNode pos = this.start_.next, next; pos != this.actionList_; pos = next, this.start_ = next )
        {
            //先保存下一个节点
            next = pos.next;
            ZWAction action = ( ZWAction )pos;
            action.Update();
        }

        if ( this.start_.next == this.actionList_ )
            start_ = this.actionList_;
    }

    //启动一个Action
    public bool StartAction(Action config, EventPart ep, bool high,SkillContext context = null)
    {
        ZWAction zwaction = new ZWAction(config, this, ep, high, context);
        this.actionList_.AddTail(zwaction);
        return true;
    }
}
