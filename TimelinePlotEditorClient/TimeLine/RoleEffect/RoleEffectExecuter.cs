using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System.Linq;

public static class RoleEffectExecuterFactory
{
    public static BehaviourExecuterBase Creat(RoleEffectType effectType)
    {
        switch (effectType)
        {
            case RoleEffectType.DissolveBurn:
                return new RoleDissolveEffectExecuter();
            case RoleEffectType.Scale:
                return new RoleScaleEffectExecuter();
            case RoleEffectType.Visualble:
                return new RoleVisuableEffectExecuter();
            default:
                return null;
        }
    }
}


public class RoleDissolveEffectExecuter : BehaviourExecuterBase
{

    RoleEffectBehaviour roleEffectBehaviour;
    private bool forAppear;
    private float start;
    private float end;
    private float speed;

    public override void OnPlayableCreate(Playable playable)
    {
        roleEffectBehaviour = behaviour as RoleEffectBehaviour;
    }


    public override void OnBehaviourStart(Playable playable)
    {
        //return;
        roleEffectBehaviour = behaviour as RoleEffectBehaviour;
        RoleObject obj = World.Instance.GetRoleObj(roleEffectBehaviour.roleData);
        if (obj != null)
        {
            var renders = obj.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            XYCoroutineEngine.Execute(DoDissolveEffect(renders.ToList()));
        }
    }

    public IEnumerator DoDissolveEffect(List<SkinnedMeshRenderer> meshs)
    {
        //return;
        if (meshs == null)
            yield break;
        foreach (var render in meshs)
        {
            yield return null;
            Shader shader = render.sharedMaterial.shader;
            var textureClothes = render.sharedMaterial.GetTexture("_Part1Tex");
            var textureEquips = render.sharedMaterial.GetTexture("_Part2Tex");
            Loader.Instance.CreatTexture("noise", tex =>
            {
                DissolveBurn db = DissolveBurn.Begin(render.gameObject, roleEffectBehaviour.speed, roleEffectBehaviour.begein, roleEffectBehaviour.end);
                db.SetMats(render.sharedMaterial, tex, Color.white);
                render.sharedMaterial.mainTexture = textureClothes ?? textureEquips;
                db.SetFinish(() =>
                {
                    render.sharedMaterial.shader = shader;
                });
            });
            //break;
        }

    }
}





public class RoleScaleEffectExecuter : BehaviourExecuterBase
{

    RoleEffectBehaviour osBehaviour;
    float originalScale;
    RoleObject roleObj;

    public override void OnPlayableCreate(Playable playable)
    {
        osBehaviour = behaviour as RoleEffectBehaviour;
    }

    public override void OnBehaviourStart(Playable playable)
    {
        roleObj = World.Instance.GetRoleObj(osBehaviour.roleData);
        originalScale = roleObj.transform.localScale.x;
        //roleObj.gameObject.transform.localScale = UnityEngine.Vector3.one * osBehaviour.scale;
    }

    public override void ProcessFrame(Playable playable, object playerData)
    {
        float valueCurve = osBehaviour.curve.Evaluate(osBehaviour.curTime / osBehaviour.duration);
        float thisFrameScale = valueCurve;
        roleObj.transform.localScale = UnityEngine.Vector3.one * thisFrameScale;
    }

    public override void OnBehaviourDone(Playable playable)
    {
        //roleObj.transform.localScale = UnityEngine.Vector3.one * osBehaviour.scale;
    }

}





public class RoleVisuableEffectExecuter : BehaviourExecuterBase
{
    RoleEffectBehaviour acBehaviour;
    private bool originalActivation;

    public override void OnPlayableCreate(Playable playable)
    {
        acBehaviour = behaviour as RoleEffectBehaviour;
    }

    public override void OnBehaviourStart(Playable playable)
    {
        if (acBehaviour == null || acBehaviour.roleData == null)
            return;
        var roleObj = World.Instance.GetRoleObj(acBehaviour.roleData);
        originalActivation = roleObj.gameObject.activeSelf;
        if (acBehaviour.activateType == ActivateType.Activate)
        {
            roleObj.gameObject.SetActive(true);
        }
        else
        {
            roleObj.gameObject.SetActive(false);
        }
    }

    public override void OnBehaviourDone(Playable playable)
    {
        var roleObj = World.Instance.GetRoleObj(acBehaviour.roleData);
        switch (acBehaviour.playbackState)
        {
            case ActivationControlTrack.PostPlaybackState.Active:
                roleObj.gameObject.SetActive(true);
                break;
            case ActivationControlTrack.PostPlaybackState.Inactive:
                roleObj.gameObject.SetActive(false);
                break;
            case ActivationControlTrack.PostPlaybackState.LeaveAsIs:
                break;
            case ActivationControlTrack.PostPlaybackState.Revert:
                roleObj.gameObject.SetActive(originalActivation);
                break;
        }
    }
}
