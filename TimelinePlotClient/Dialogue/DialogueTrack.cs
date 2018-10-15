using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackClipType(typeof(DialogueClip))]
public class DialogueTrack : TrackAsset
{

    protected override Playable CreatePlayable(PlayableGraph graph, GameObject go, TimelineClip clip)
    {
        PlayableDirector director = go.GetComponent<PlayableDirector>();
        DialogueClip dialogueClip = clip.asset as DialogueClip;
        Playable playable = base.CreatePlayable(graph, go, clip);
        return playable;
    }
}
