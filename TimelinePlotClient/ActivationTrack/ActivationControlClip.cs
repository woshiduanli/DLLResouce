using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class ActivationControlClip : PlayableAsset
{
    public RoleData role;
    public ActivateType type;
    [HideInInspector]
    public ActivationControlTrack.PostPlaybackState playbackState;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ActivationControlPlayable>.Create(graph);
        var behaviour = playable.GetBehaviour();
        if (behaviour == null)
            return playable;
        behaviour.playbackState = playbackState;
        behaviour.role = role;
        behaviour.type = type;
        behaviour.executer = BehaviourExecuterFactory.GetActivationControlExecuter(behaviour);
        if (behaviour.executer == null)
            return playable;
        behaviour.executer.OnPlayableCreate(playable);
        return playable;
    }
}


public class ActivationControlPlayable : MYPlayableBehaviour
{
    public RoleData role;
    public ActivateType type;
    public ActivationControlTrack.PostPlaybackState playbackState;
}


public enum ActivateType
{
    Activate,
    InActivate,
}

