using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System;
using UnityEngine.Timeline;

public class ActionPlayable : MYPlayableBehaviour
{
    public RoleData Role;
    public string ActionName;
}




public class RoleActionClip : PlayableAsset
{
    [HideInInspector]
    public RoleData Role;
    public string ActionName;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ActionPlayable>.Create(graph);
        var actionPlayable = playable.GetBehaviour();
        if (actionPlayable == null)
            return playable;
        actionPlayable.ActionName = ActionName;
        actionPlayable.Role = Role;
        actionPlayable.executer = BehaviourExecuterFactory.GetActionExecuter(actionPlayable);
        if (actionPlayable.executer == null)
            return playable;
        actionPlayable.executer.OnPlayableCreate(playable);
        return playable;
    }
}
