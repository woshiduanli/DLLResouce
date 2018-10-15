using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackBindingType(typeof(RoleData))]
[TrackClipType(typeof(ActivationControlClip))]
[TrackMediaType(TimelineAsset.MediaType.Script)]
public class ActivationControlTrack:TrackAsset
{
    public PostPlaybackState playbackState = PostPlaybackState.LeaveAsIs;

    protected override Playable CreatePlayable(PlayableGraph graph, GameObject go, TimelineClip clip)
    {
        ActivationControlClip clipAsset = clip.asset as ActivationControlClip;
        PlayableDirector dirctor = go.GetComponent<PlayableDirector>();
        var binding= dirctor.GetGenericBinding(this) as RoleData;
        clipAsset.role = binding;
        clipAsset.playbackState = playbackState;
        return base.CreatePlayable(graph, go, clip);
    }

    public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
        //driver.AddFromName("playbackState");
    }

    public enum PostPlaybackState
    {
        Active = 0,
        Inactive = 1,
        Revert = 2,
        LeaveAsIs = 3
    }
}
