using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.UI;

public class PlotDialogueExecuter : BehaviourExecuterBase
{
    public PlotDialogueBehaviour dialoguePlayable;


    public override void OnPlayableCreate(Playable playable)
    {
        base.OnPlayableCreate(playable);
        dialoguePlayable = behaviour as PlotDialogueBehaviour;
    }

    public override void OnBehaviourStart(Playable playable)
    {
        if (!EditorApplication.isPlaying)
            return;
        PlotDialogueUI.Instance.ShowDialogue(dialoguePlayable.isPauseTimeline,dialoguePlayable.dialogues);
    }
}