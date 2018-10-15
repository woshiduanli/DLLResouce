using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackBindingType(typeof(RoleData))]
[TrackClipType(typeof(RoleAnimationClip))]
public class RoleAnimationTrack : TrackAsset {

    protected override Playable CreatePlayable(PlayableGraph graph, GameObject go, TimelineClip clip)
    {
        PlayableDirector director = go.GetComponent<PlayableDirector>();
        RoleData trackRole = director.GetGenericBinding(this) as RoleData;
        RoleAnimationClip animationClip = clip.asset as RoleAnimationClip;
        animationClip.Role = trackRole;
        Playable playable= base.CreatePlayable(graph, go, clip);
        return playable;
    }


    public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
    }
}
