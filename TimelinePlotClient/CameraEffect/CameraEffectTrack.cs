using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


[TrackClipType(typeof(CameraEffectClip))]
[TrackBindingType(typeof(RoleData))]
class CameraEffectTrack:TrackAsset
{
    protected override Playable CreatePlayable(PlayableGraph graph, GameObject go, TimelineClip clip)
    {
        PlayableDirector director = go.GetComponent<PlayableDirector>();
        RoleData trackRole = director.GetGenericBinding(this) as RoleData;
        CameraEffectClip cameraClip = clip.asset as CameraEffectClip;
        cameraClip.role = trackRole;
        Playable playable = base.CreatePlayable(graph, go, clip);
        return playable;
    }
}

