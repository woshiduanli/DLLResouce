using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

[TrackBindingType(typeof(CinemachineVirtualCameraBase))]
[TrackClipType(typeof(VMOperateClip))]
public class VMOperateTrack : TrackAsset
{
    protected override Playable CreatePlayable(PlayableGraph graph, GameObject go, TimelineClip clip)
    {
        PlayableDirector director = go.GetComponent<PlayableDirector>();
        CinemachineVirtualCameraBase camera = director.GetGenericBinding(this) as CinemachineVirtualCameraBase;
        VMOperateClip vmclip = clip.asset as VMOperateClip;
        vmclip.cinemachineCamera = camera;
        Playable playable = base.CreatePlayable(graph, go, clip);
        return playable;
    }
}
