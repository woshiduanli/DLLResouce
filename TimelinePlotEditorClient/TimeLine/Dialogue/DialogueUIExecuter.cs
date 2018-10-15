using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.UI;

public class DialogueUIExecuter : BehaviourExecuterBase
{
    public DialogueBehaviour dialoguePlayable;

    public override void OnPlayableCreate(Playable playable)
    {
        base.OnPlayableCreate(playable);
        dialoguePlayable = behaviour as DialogueBehaviour;
    }
    public override void OnBehaviourStart(Playable playable)
    {
        if (!EditorApplication.isPlaying)
            return;
        FundamentalUI.Instance.ShowDialogue(dialoguePlayable.dialogue, "主角名称");
    }

    public override void OnBehaviourDone(Playable playable)
    {
        if (!EditorApplication.isPlaying)
            return;
        if (dialoguePlayable.isLastOne)
            FundamentalUI.Instance.HideDialogueUI();
        else
            FundamentalUI.Instance.HideText();
    }
}