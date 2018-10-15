using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.UI;

public class BubbleUIExecuter : BehaviourExecuterBase
{
    public TextBubbleBehaviour bubblePlayable;

    public override void OnPlayableCreate(Playable playable)
    {
        base.OnPlayableCreate(playable);
        bubblePlayable = behaviour as TextBubbleBehaviour;
    }
    public override void OnBehaviourStart(Playable playable)
    {
        if (!EditorApplication.isPlaying)
            return;
        if (bubblePlayable.bubbleData.Time <= 0)
            bubblePlayable.bubbleData.Time = (float)playable.GetDuration();
        TextBubbleUI.Instance.ShowBubble(bubblePlayable.bubbleData);
    }

    public override void OnBehaviourDone(Playable playable)
    {
        if (!EditorApplication.isPlaying)
            return;
    }
}