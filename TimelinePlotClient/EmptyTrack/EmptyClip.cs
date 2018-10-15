using System;
using UnityEngine;
using UnityEngine.Playables;

class EmptyClip : PlayableAsset
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<EmptyPlayable>.Create(graph);
    }
}

class EmptyPlayable : PlayableBehaviour
{

}

