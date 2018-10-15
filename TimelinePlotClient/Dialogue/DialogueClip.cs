using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueClip : PlayableAsset
{
    public string dialogue;
    public bool isLastOne;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DialogueBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();
        if (behaviour == null)
            return playable;
        behaviour.dialogue = dialogue;
        behaviour.isLastOne = isLastOne;
        behaviour.executer = BehaviourExecuterFactory.GetDialogueUIExecuter(behaviour);
        if (behaviour.executer == null)
            return playable;
        behaviour.executer.OnPlayableCreate(playable);
        return playable;
    }
}


public class DialogueBehaviour : MYPlayableBehaviour
{
    public string dialogue;
    public bool isLastOne;
}
