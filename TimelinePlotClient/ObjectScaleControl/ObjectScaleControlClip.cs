using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class ObjectScaleControlClip : PlayableAsset
{
    public RoleData role;
    [Tooltip("最终要变成多大")]
    public float scale;
    [Tooltip("是否按照clip的长度来逐渐变大")]
    public bool isSetGradually;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ObjectScaleControlPlayable>.Create(graph);
        var behaviour = playable.GetBehaviour();
        if (behaviour == null)
            return playable;
        behaviour.role = role;
        behaviour.scale = scale;
        behaviour.isSetGradually = isSetGradually;
        behaviour.executer = BehaviourExecuterFactory.GetScaleControlExecuter(behaviour);
        if (behaviour.executer == null)
            return playable;
        behaviour.executer.OnPlayableCreate(playable);
        return playable;
    }
}


public class ObjectScaleControlPlayable : MYPlayableBehaviour
{
    public RoleData role;
    public float scale;
    public bool isSetGradually;
}


