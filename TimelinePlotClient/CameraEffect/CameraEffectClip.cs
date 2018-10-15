using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CameraEffectClip : PlayableAsset
{
    public TimelineCameraEffect effectType;
    public Vector3 shockScope;
    public float shockFrequency=0.1f;
    public RoleData role;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<CameraEffectPlayable>.Create(graph);
        var behaviour = playable.GetBehaviour();
        if (behaviour == null)
            return playable;
        behaviour.effectType = effectType;
        behaviour.shockFrequency = shockFrequency;
        behaviour.shockScope = shockScope;
        behaviour.role = role;
        behaviour.executer = BehaviourExecuterFactory.GetCameraEffectExecuter(behaviour);
        if (behaviour.executer == null)
            return playable;
        behaviour.executer.OnPlayableCreate(playable);
        return playable;
    }
}


public class CameraEffectPlayable : MYPlayableBehaviour
{
    public TimelineCameraEffect effectType;
    public RoleData role;
    public Vector3 shockScope;
    public float shockFrequency;
}

public enum TimelineCameraEffect
{
    Shock = 0,
    Blur
}

