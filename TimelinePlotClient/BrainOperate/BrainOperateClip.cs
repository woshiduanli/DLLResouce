using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;

public class BrainOperateClip:PlayableAsset
{
    public CinemachineBlendDefinition.Style style;
    public float time;
    public bool isRestore;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<BrainOperateBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();
        if (behaviour == null)
            return playable;
        behaviour.isRestore = isRestore;
        behaviour.style = style;
        behaviour.time = time;
        return playable;    
    }
}

public class BrainOperateBehaviour : MYPlayableBehaviour
{
    public CinemachineBlendDefinition.Style style;
    public float time;
    public bool isRestore;
    private CinemachineBlendDefinition.Style originalStyle;
    private float originalTime;


    public override void OnMYBehaviourStart(Playable playable)
    {
        if (!Camera.main)
            return;
        var brain = Camera.main.GetComponent<CinemachineBrain>();
        if (brain)
        {
            originalStyle = brain.m_DefaultBlend.m_Style;
            originalTime = brain.m_DefaultBlend.m_Time;
            brain.m_DefaultBlend.m_Style = style;
            brain.m_DefaultBlend.m_Time = time;
        }
    }


    public override void OnMYBehaviourDone(Playable playable)
    {
        if (!isRestore)
            return;
        if (!Camera.main)
            return;
        var brain = Camera.main.GetComponent<CinemachineBrain>();
        if (brain)
        {
            brain.m_DefaultBlend.m_Style = originalStyle;
            brain.m_DefaultBlend.m_Time = originalTime;
        }
    }
}

