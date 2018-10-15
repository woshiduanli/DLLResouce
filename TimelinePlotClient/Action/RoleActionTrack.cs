using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

[TrackClipType(typeof(RoleActionClip))]
[TrackBindingType(typeof(RoleData))]
public class RoleActionTrack : TrackAsset
{
    public RoleData trackRole;

    protected override Playable CreatePlayable(PlayableGraph graph, GameObject go, TimelineClip clip)
    {
        PlayableDirector director = go.GetComponent<PlayableDirector>();
        RoleData trackRole = (RoleData)director.GetGenericBinding(this);
        RoleActionClip actionClip = clip.asset as RoleActionClip;
        actionClip.Role = trackRole;
        Playable playable = base.CreatePlayable(graph, go, clip);
        return playable;
    }

    public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
    }
}
