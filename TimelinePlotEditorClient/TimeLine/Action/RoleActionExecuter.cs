using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Playables;
using System;
using UnityEngine.Timeline;

public class RoleActionExecuter : BehaviourExecuterBase
{
    private ActionPlayable actionPlayable;
    private Model.Action roleAction;
    private RoleObject roleObj;

    public override void OnPlayableCreate(Playable playable)
    {
        base.OnPlayableCreate(playable);
        actionPlayable = behaviour as ActionPlayable;
    }
    public override void OnGraphStart(Playable playable)
    {
        if (!EditorApplication.isPlaying)
            return;
        Loader.Instance.CreatAction(actionPlayable.ActionName, actionConfig=> {
            roleAction = actionConfig;
        });
        roleObj = World.Instance.GetRoleObj(actionPlayable.Role);
    }

        public override void OnBehaviourStart(Playable playable)
    {
        if (!EditorApplication.isPlaying)
            return;
        roleObj.mActionPerformer.StartAction(roleAction, Model.EventPart.FIRE, null);
    }
}
