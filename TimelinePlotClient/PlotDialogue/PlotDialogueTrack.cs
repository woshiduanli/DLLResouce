using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackClipType(typeof(PlotDialogueClip))]
public class PlotDialogueTrack : TrackAsset
{

    protected override Playable CreatePlayable(PlayableGraph graph, GameObject go, TimelineClip clip)
    {
        PlayableDirector director = go.GetComponent<PlayableDirector>();
        PlotDialogueClip dialogueClip = clip.asset as PlotDialogueClip;
        Playable playable = base.CreatePlayable(graph, go, clip);
        return playable;
    }
}
