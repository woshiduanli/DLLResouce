using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackClipType(typeof(MYAudioClip))]
class MYAudioTrack:TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return base.CreateTrackMixer(graph, go, inputCount);
    }
}

