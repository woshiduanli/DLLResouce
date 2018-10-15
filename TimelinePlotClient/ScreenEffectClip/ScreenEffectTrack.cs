using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackClipType(typeof(ScreenEffectClip))]
public class ScreenEffectTrack : TrackAsset
{

    protected override Playable CreatePlayable(PlayableGraph graph, GameObject go, TimelineClip clip)
    {
        PlayableDirector director = go.GetComponent<PlayableDirector>();
        ScreenEffectClip moveClip = clip.asset as ScreenEffectClip;
        Playable playable = base.CreatePlayable(graph, go, clip);
        return playable;
    }

    public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
    }
}
