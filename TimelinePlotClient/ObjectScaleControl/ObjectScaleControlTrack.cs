using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackBindingType(typeof(RoleData))]
[TrackClipType(typeof(ObjectScaleControlClip))]
[TrackMediaType(TimelineAsset.MediaType.Script)]
public class ObjectScaleControlTrack : TrackAsset
{

    protected override Playable CreatePlayable(PlayableGraph graph, GameObject go, TimelineClip clip)
    {
        ObjectScaleControlClip clipAsset = clip.asset as ObjectScaleControlClip;
        PlayableDirector dirctor = go.GetComponent<PlayableDirector>();
        var binding = dirctor.GetGenericBinding(this) as RoleData;
        clipAsset.role = binding;
        return base.CreatePlayable(graph, go, clip);
    }

    public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
        //driver.AddFromName("playbackState");
    }

}
