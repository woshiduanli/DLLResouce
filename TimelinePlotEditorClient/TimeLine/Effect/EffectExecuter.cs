using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EffectExecuter : BehaviourExecuterBase
{
    private string effectName;
    private Vector3 pos;
    private bool destoryOnClipOver;
    private EffectObject effectObj;

    public override void OnPlayableCreate(Playable playable)
    {
        effectName = (behaviour as EffectPlayable).effectName;
        pos = (behaviour as EffectPlayable).pos;
        destoryOnClipOver = (behaviour as EffectPlayable).destroyOnClipOver;
    }

    public override void OnBehaviourStart(Playable playable)
    {
        Loader.Instance.CreatEffect(effectName, effectObj =>
        {
            effectObj.SetPostion(pos);
            effectObj.SetScale((behaviour as EffectPlayable).scale);
            effectObj.gameObject.transform.eulerAngles = (behaviour as EffectPlayable).rotation;
            this.effectObj = effectObj;
            if ((behaviour as EffectPlayable).isUIEffect)
                SetLayerRecursively(effectObj.gameObject);
            World.Instance.AddEffect(effectObj);
        }, (behaviour as EffectPlayable).isUIEffect);
    }

    private void SetLayerRecursively(GameObject go)
    {
        go.layer = XYDefines.Layer.TimelineUI;
        var array = go.GetAllChildren();
        if (array != null)
        {
            foreach (var tf in array)
                SetLayerRecursively(tf);
        }
    }

    public override void OnBehaviourDone(Playable playable)
    {
        if (destoryOnClipOver && effectObj != null)
            effectObj.Destroy();
    }

    public override void OnPlayableDestroy(Playable playable)
    {
        base.OnPlayableDestroy(playable);
        if (effectObj != null)
            effectObj.Destroy();
    }
}

