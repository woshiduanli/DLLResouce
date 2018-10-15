using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackBindingType(typeof(RoleData))]
[TrackClipType(typeof(RoleMoveClip))]
public class RoleMoveTrack : TrackAsset
{

    protected override Playable CreatePlayable(PlayableGraph graph, GameObject go, TimelineClip clip)
    {
        PlayableDirector director = go.GetComponent<PlayableDirector>();
        RoleMoveClip moveClip = clip.asset as RoleMoveClip;
        moveClip.roleData = (RoleData)director.GetGenericBinding(clip.parentTrack);
        Playable playable = base.CreatePlayable(graph, go, clip);
        return playable;
    }

    public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
    }
}
