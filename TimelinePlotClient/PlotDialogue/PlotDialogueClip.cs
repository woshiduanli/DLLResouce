using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlotDialogueClip : PlayableAsset
{
    public bool isPauseTimeline;
    public List<DialogueUIData> dialogues;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<PlotDialogueBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();
        if (behaviour == null)
            return playable;
        behaviour.isPauseTimeline = isPauseTimeline;
        behaviour.dialogues = dialogues;
        behaviour.executer = BehaviourExecuterFactory.GetPlotDialogueUIExecuter(behaviour);
        if (behaviour.executer == null)
            return playable;
        behaviour.executer.OnPlayableCreate(playable);
        return playable;
    }
}


public class PlotDialogueBehaviour : MYPlayableBehaviour
{
    public bool isPauseTimeline;
    public List<DialogueUIData> dialogues;
}


[Serializable]
public class DialogueUIData
{
    public int Id;
    public TimelineRoleType RoleType;
    [Multiline]
    public string Dialogue;
    public string Name;
    public float Time;
}
