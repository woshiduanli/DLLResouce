using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TextBubbleClip : PlayableAsset
{
    public DialogueUIData bubbleData;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TextBubbleBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();
        if (behaviour == null)
            return playable;
        behaviour.bubbleData = bubbleData;
        behaviour.executer = BehaviourExecuterFactory.GetBubbleUIExecuter(behaviour);
        if (behaviour.executer == null)
            return playable;
        behaviour.executer.OnPlayableCreate(playable);
        return playable;
    }
}


public class TextBubbleBehaviour : MYPlayableBehaviour
{
    public DialogueUIData bubbleData;
}
