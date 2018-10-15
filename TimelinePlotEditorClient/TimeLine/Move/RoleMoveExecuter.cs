using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using System;
using UnityEngine.AI;
using UnityEditor;

public class RoleMoveExecuter : BehaviourExecuterBase
{
    private RoleMovePlayable movePlayable;

    private RoleObject roleObj;
    private Vector3 originalPos;

    private float originalSpeed;
    private NavMeshAgent roleNav;
    private float speed;//nav's speed caculated by totaltime and pathLegth 

    private float totalPathLength;

    public override void OnPlayableCreate(Playable playable)
    {
        base.OnPlayableCreate(playable);
        movePlayable = behaviour as RoleMovePlayable;
    }

    public override void OnGraphStart(Playable playable)
    {
        if (!EditorApplication.isPlaying)
            return;
        roleObj = World.Instance.GetRoleObj(movePlayable.roleData);
        roleNav = roleObj.GetComponent<NavMeshAgent>();
    }

    public override void OnBehaviourStart(Playable playable)
    {
        if (!EditorApplication.isPlaying)
            return;
        if (movePlayable.moveMotion == MoveMotion.Run)
            roleObj.Animator.CrossFade(Enum.GetName(typeof(Model.MotionState), Model.MotionState.run), 0.2f);
        else
            roleObj.Animator.CrossFade(Enum.GetName(typeof(Model.MotionState), Model.MotionState.walk), 0.2f);
        movePlayable.AddPlayerCurrentPos(roleObj.transform.position);
        originalPos = roleObj.position;
        totalPathLength = movePlayable.curve.GetLength();
        if (movePlayable.moveWay == MoveWay.NavMesh)
            InitPlayDataWhenNavmeshMove();
        else
            InitPlayDataWhenOtherWayMove();
    }

    private void InitPlayDataWhenNavmeshMove()
    {
        originalSpeed = roleNav.speed;
        speed = (float)(totalPathLength / movePlayable.duration);
        roleNav.speed = speed;
        roleNav.enabled = true;
    }

    private void InitPlayDataWhenOtherWayMove()
    {
        roleNav.enabled = false;
    }


    public override void ProcessFrame(Playable playable, object playerData)
    {
        if (!EditorApplication.isPlaying)
            return;
        Vector3 posShouldBe = movePlayable.curve.GetPosition(movePlayable.curTime / movePlayable.duration);

        if (movePlayable.moveWay == MoveWay.NavMesh)
            roleNav.SetDestination(posShouldBe);
        else
        {
            roleObj.transform.position = posShouldBe;
            Vector3 forward = movePlayable.curve.GetForwardNormal((movePlayable.curTime / movePlayable.duration), 0.01f);
            Vector3 forwardToset = Vector3.zero;
            if (roleObj.transform.forward != forward && forward.magnitude > 0)
            {
                forwardToset = Vector3.Slerp(roleObj.transform.forward, forward, 2 * Time.deltaTime);
                if (movePlayable.rotateWay == RotateWay.Hard)
                    roleObj.transform.forward = forward;
                else
                    roleObj.transform.forward = forwardToset;
            }
            forward.y = 0;
            if (forwardToset != Vector3.zero)
                roleObj.transform.forward = forwardToset;
        }
    }

    public override void OnBehaviourPause(Playable playable)
    {
        if (!EditorApplication.isPlaying)
            return;
        roleObj.Animator.CrossFade(Enum.GetName(typeof(Model.MotionState), Model.MotionState.stand), 0.2f);
    }

    public override void OnBehaviourResume(Playable playable)
    {
        if (movePlayable.moveMotion == MoveMotion.Run)
            roleObj.Animator.CrossFade(Enum.GetName(typeof(Model.MotionState), Model.MotionState.run), 0.2f);
        else
            roleObj.Animator.CrossFade(Enum.GetName(typeof(Model.MotionState), Model.MotionState.walk), 0.2f);
    }


    public override void OnBehaviourDone(Playable playable)
    {
        if (!EditorApplication.isPlaying)
            return;
        //roleNav.Warp(targetPostion);
        //roleObj.transform.position = movePlayable.curve.GetPosition(1);
        if (movePlayable.moveWay == MoveWay.NavMesh)
            roleNav.speed = originalSpeed;
        roleNav.enabled = false;
    }


}