using System;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine;

public class SlowClip : PlayableAsset
{
    [Header("时间缩放")]
    public float timeScale;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<SlowPlayable>.Create(graph);
        var behaviour = playable.GetBehaviour();
        if (behaviour == null)
            return playable;
        behaviour.timeScale = timeScale;
        return playable;
    }
}



public class SlowPlayable : PlayableBehaviour
{
    public float timeScale;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        Time.timeScale = 1 - (1 - timeScale) * info.weight;
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Time.timeScale = 1 - (1 - timeScale) * info.weight;
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        Time.timeScale = 1;
    }

    public override void OnPlayableDestroy(Playable playable)
    {
        base.OnPlayableDestroy(playable);
        Time.timeScale = 1;
    }
}
