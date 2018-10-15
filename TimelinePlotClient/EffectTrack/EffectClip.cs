using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable()]
public class EffectClip : PlayableAsset
{
    public string effectName;
    public bool isUIEffect;
    public bool destroyOnClipOver;
    public Vector3 pos;
    public Vector3 rotation;
    public Vector3 scale=Vector3.one;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<EffectPlayable>.Create(graph);
        var behaviour = playable.GetBehaviour();
        if (behaviour == null)
            return playable;
        behaviour.pos = pos;
        behaviour.scale = scale;
        behaviour.rotation = rotation;
        behaviour.effectName = effectName;
        behaviour.destroyOnClipOver = destroyOnClipOver;
        behaviour.isUIEffect = isUIEffect;
        behaviour.executer = BehaviourExecuterFactory.GetEffectExecuter(behaviour);
        if (behaviour.executer == null)
            return playable;
        behaviour.executer.OnPlayableCreate(playable);
        return playable;
    }
}


public class EffectPlayable : MYPlayableBehaviour
{
    public string effectName;
    public Vector3 pos;
    public Vector3 rotation;
    public Vector3 scale;
    public bool destroyOnClipOver;
    public bool isUIEffect;
}

