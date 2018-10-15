using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Playables;

class CameraEffectExecuter : BehaviourExecuterBase
{
    CameraEffectPlayable cameraBehaviour;
    private GameObject tempShockScopeGo;
    private int roleOriginalLayer;

    public override void OnGraphStart(Playable playable)
    {
        cameraBehaviour = behaviour as CameraEffectPlayable;
    }

    public override void OnBehaviourStart(Playable playable)
    {
        if (cameraBehaviour.effectType == TimelineCameraEffect.Shock)
        {
            tempShockScopeGo = new GameObject("tempShockScopeGo");
            tempShockScopeGo.transform.localPosition = cameraBehaviour.shockScope;
            CameraControll.Instance.AddImageEffect(ImageEffect.Shock, tempShockScopeGo, cameraBehaviour.shockFrequency);
        }
        else if (cameraBehaviour.effectType == TimelineCameraEffect.Blur)
        {
            RoleObject roleObj = World.Instance.GetRoleObj(cameraBehaviour.role);
            roleOriginalLayer = roleObj.gameObject.layer;
            roleObj.gameObject.layer = XYDefines.Layer.MainPlayer;
            CameraControll.Instance.AddImageEffect(ImageEffect.Blur, null, 0);
        }
    }

    public override void OnBehaviourDone(Playable playable)
    {
        if (cameraBehaviour.effectType == TimelineCameraEffect.Shock)
        {
            if (tempShockScopeGo != null)
                GameObject.Destroy(tempShockScopeGo);
            CameraControll.Instance.RemoveImageEffect(ImageEffect.Shock);
        }
        else if (cameraBehaviour.effectType == TimelineCameraEffect.Blur)
        {
            RoleObject roleObj = World.Instance.GetRoleObj(cameraBehaviour.role);
            roleObj.gameObject.layer = roleOriginalLayer;
            CameraControll.Instance.RemoveImageEffect(ImageEffect.Blur);
        }
    }
}
