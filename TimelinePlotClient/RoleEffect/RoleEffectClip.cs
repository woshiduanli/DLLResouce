
using System;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class RoleEffectClip : PlayableAsset
{
    public RoleEffectType type;
    [HideInInspector]
    public RoleData roleData;

    //Dissolve parameter
    public bool isForAppear;
    public float begein;
    public float end;
    public float speed;

    //scale parameter
    //public bool isSetGradually;
    //public float scale;
    public AnimationCurve curve=new AnimationCurve(new Keyframe(0,1),new Keyframe(1,1));

    //visualble parameter
    public ActivateType activateType;
    public ActivationControlTrack.PostPlaybackState playbackState;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playableScript = ScriptPlayable<RoleEffectBehaviour>.Create(graph);
        var behaviour = playableScript.GetBehaviour();
        if (behaviour == null)
            return playableScript;
        behaviour.type = type;
        behaviour.roleData = roleData;

        behaviour.isForAppear = isForAppear;
        behaviour.speed = speed;
        behaviour.begein = begein;
        behaviour.end = end;

        //behaviour.isSetGradually = isSetGradually;
       // behaviour.scale = scale;
        behaviour.curve = curve;

        behaviour.activateType = activateType;
        behaviour.playbackState = playbackState;

        behaviour.executer = BehaviourExecuterFactory.GetRoleEffectExecuter(behaviour,type);
        if (behaviour.executer == null)
            return playableScript;
        behaviour.executer.OnPlayableCreate(playableScript);
        return playableScript;
    }
}


public class RoleEffectBehaviour : MYPlayableBehaviour
{
    public RoleEffectType type;
    public RoleData roleData;

    //Dissolve parameter
    public bool isForAppear;
    public float begein;
    public float end;
    public float speed;

    //scale parameter
    //public bool isSetGradually;
    public AnimationCurve curve;
    //public float scale;

    //visualble parameter
    public ActivateType activateType;
    public ActivationControlTrack.PostPlaybackState playbackState;

}
