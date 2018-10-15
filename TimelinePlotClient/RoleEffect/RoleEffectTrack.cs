using UnityEngine.Timeline;
using System;
using UnityEngine;
using UnityEngine.Playables;


[TrackBindingType(typeof(RoleData))]
[TrackClipType(typeof(RoleEffectClip))]
public class RoleEffectTrack : TrackAsset
{
    protected override Playable CreatePlayable(PlayableGraph graph, GameObject go, TimelineClip clip)
    {
        RoleEffectClip effectClip = clip.asset as RoleEffectClip;
        PlayableDirector director = go.GetComponent<PlayableDirector>();
        effectClip.roleData = director.GetGenericBinding(this) as RoleData;
        return base.CreatePlayable(graph, go, clip);
    }
}


public enum RoleEffectType
{
    DissolveBurn,
    Scale,
    Visualble,
}


