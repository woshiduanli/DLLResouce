using System;
using UnityEngine;
using UnityEngine.Playables;

public class ScaleControlExecuter : BehaviourExecuterBase
{
    ObjectScaleControlPlayable osBehaviour;
    float originalScale;
    RoleObject roleObj;

    public override void OnPlayableCreate(Playable playable)
    {
        osBehaviour = behaviour as ObjectScaleControlPlayable;
    }

    public override void OnBehaviourStart(Playable playable)
    {
        roleObj = World.Instance.GetRoleObj(osBehaviour.role);
        originalScale = roleObj.transform.localScale.x;
        if (!osBehaviour.isSetGradually)
        {
            roleObj.gameObject.transform.localScale = UnityEngine.Vector3.one * osBehaviour.scale;
        }
    }

    public override void ProcessFrame(Playable playable, object playerData)
    {
        if (osBehaviour.isSetGradually)
        {
            float thisFrameScale = osBehaviour.curTime /osBehaviour.duration * (osBehaviour.scale - originalScale) + originalScale;
            roleObj.transform.localScale = UnityEngine.Vector3.one * thisFrameScale;
        }
    }

    public override void OnBehaviourDone(Playable playable)
    {
        roleObj.transform.localScale = UnityEngine.Vector3.one * osBehaviour.scale;
    }
}

