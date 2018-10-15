using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackClipType(typeof(TextBubbleClip))]
public class TextBubbleTrack : TrackAsset
{
    protected override Playable CreatePlayable(PlayableGraph graph, GameObject go, TimelineClip clip)
    {
        PlayableDirector director = go.GetComponent<PlayableDirector>();
        TextBubbleClip dialogueClip = clip.asset as TextBubbleClip;
        Playable playable = base.CreatePlayable(graph, go, clip);
        return playable;
    }
}
