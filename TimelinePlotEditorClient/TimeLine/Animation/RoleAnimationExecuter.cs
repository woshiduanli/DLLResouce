using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;


public class RoleAnimationExecuter : BehaviourExecuterBase
{
    private AnimationPlayable animationPlayable;
    private RoleObject roleObj;

    public override void OnPlayableCreate(Playable playable)
    {
        base.OnPlayableCreate(playable);
        animationPlayable = behaviour as AnimationPlayable;
    }

    public override void OnGraphStart(Playable playable)
    {
        if (!EditorApplication.isPlaying)
            return;
        roleObj = World.Instance.GetRoleObj(animationPlayable.Role);
    }

        public override void OnBehaviourStart(Playable playable)
    {
        if (!EditorApplication.isPlaying)
            return;
        if (animationPlayable.Role && roleObj)
            roleObj.Animator.CrossFade(animationPlayable.AnimationName, 0.2f);
    }

        public override void OnBehaviourDone(Playable playable)
    {
        if (!EditorApplication.isPlaying)
            return;
        if (animationPlayable.Role && roleObj)
            roleObj.Animator.CrossFade("stand", 0.2f);
    }
}
