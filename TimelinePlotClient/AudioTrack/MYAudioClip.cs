using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable()]
public class MYAudioClip : PlayableAsset
{
    public string audioName;
    public bool isLoop;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<MYAudioPlayable>.Create(graph);
        var behaviour = playable.GetBehaviour();
        if (behaviour == null)
            return playable;
        behaviour.audioName = audioName;
        behaviour.isLoop = isLoop;
        behaviour.executer = BehaviourExecuterFactory.GetAudioExecuter(behaviour);
        if (behaviour.executer == null)
            return playable;
        behaviour.executer.OnPlayableCreate(playable);
        return playable;
    }
}


public class MYAudioPlayable : MYPlayableBehaviour
{
    public string audioName;
    public bool isLoop;
}

