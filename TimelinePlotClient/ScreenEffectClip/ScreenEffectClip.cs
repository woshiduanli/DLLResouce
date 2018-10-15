using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using System;

public enum ScreenEffectType
{
    Block,
    White,
}

public class ScreenEffectClip : PlayableAsset
{
    public ScreenEffectType effectType;
    public AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        PlayableDirector director = owner.GetComponent<PlayableDirector>();
        var playable = ScriptPlayable<ScreenEffectPlayable>.Create(graph);
        ScreenEffectPlayable effectPlayable = playable.GetBehaviour();
        if (effectPlayable == null)
            return playable;
        effectPlayable.curve = curve;
        effectPlayable.effectType = effectType;
        return playable;
    }


}



public class ScreenEffectPlayable : MYPlayableBehaviour
{

    public ScreenEffectType effectType;
    public AnimationCurve curve;

    public override void OnMYBehaviourStart(Playable playable)
    {

    }

    public override void OnProcessFrame(Playable playable, object playerData)
    {
        base.OnProcessFrame(playable, playerData);
        if(effectType== ScreenEffectType.Block)
        {
            ProcessBlockEffectFrame(playable, playerData);
        }
        else if(effectType == ScreenEffectType.White)
        {
            ProcessWhiteEffectFrame(playable, playerData);
        }
    }

    private void ProcessBlockEffectFrame(Playable playable1, object playerData)
    {
        if (ScreenEffectUI.Instance != null)
        {
            Color color = ScreenEffectUI.Instance.blockImage.color;
            color.a = curve.Evaluate(curTime / duration);
            ScreenEffectUI.Instance.blockImage.color = color;
        }
    }

    private void ProcessWhiteEffectFrame(Playable playable1, object playerData)
    {
        if (ScreenEffectUI.Instance != null)
        {
            Color color = ScreenEffectUI.Instance.blockImage.color;
            color.a = curve.Evaluate(curTime / duration);
            ScreenEffectUI.Instance.whiteImage.color = color;
        }
    }
}