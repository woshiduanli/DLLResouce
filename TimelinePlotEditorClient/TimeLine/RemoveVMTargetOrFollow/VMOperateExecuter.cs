using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;


public class VMOperateExecuter : BehaviourExecuterBase
{
    private VMOperateBehaviour vmOperatePlayable;
    public CinemachineVirtualCameraBase cinemachineCamera;
    public RoleData roleData;
    public int priority;
    protected RoleObject roleObj;

    public override void OnPlayableCreate(Playable playable)
    {
        base.OnPlayableCreate(playable);
        vmOperatePlayable = behaviour as VMOperateBehaviour;
        cinemachineCamera = vmOperatePlayable.cinemachineCamera;
        priority = vmOperatePlayable.priority;
        roleData = vmOperatePlayable.roleData;
        if(roleData!=null)
            roleObj = World.Instance.GetRoleObj(roleData);
    }
}


public class RemoveFollow : VMOperateExecuter
{
        public override void OnBehaviourStart(Playable playable)
    {
        if (cinemachineCamera != null)
            cinemachineCamera.Follow = null;
    }

    public override void OnPlayableCreate(Playable playable)
    {
        base.OnPlayableCreate(playable);

    }
}

public class AddFollow : VMOperateExecuter
{
        public override void OnBehaviourStart(Playable playable)
    {
        if (!EditorApplication.isPlaying)
            return;
        if (cinemachineCamera != null)
            cinemachineCamera.Follow = roleData.transform;
    }
}

public class AddLookAt : VMOperateExecuter
{
        public override void OnBehaviourStart(Playable playable)
    {
        if (!EditorApplication.isPlaying)
            return;
        if (cinemachineCamera != null)
            cinemachineCamera.LookAt = roleData.transform;
    }
}


public class RemoveLookAt : VMOperateExecuter
{
        public override void OnBehaviourStart(Playable playable)
    {
        if (!EditorApplication.isPlaying)
            return;
        if (cinemachineCamera != null)
            cinemachineCamera.LookAt = null;
    }
}

public class SetPriority : VMOperateExecuter
{
        public override void OnBehaviourStart(Playable playable)
    {
        if (!EditorApplication.isPlaying)
            return;
        if (cinemachineCamera != null)
            cinemachineCamera.Priority = priority;
    }
}



public class VMOperaterFactory
{
    public static BehaviourExecuterBase Creat(CinemachineOperate operate)
    {
        switch (operate)
        {
            case CinemachineOperate.AddFollow:
                return new AddFollow();
            case CinemachineOperate.RemoveFollow:
                return new RemoveFollow();
            case CinemachineOperate.AddTarget:
                return new AddLookAt();
            case CinemachineOperate.RemoveTarget:
                return new RemoveLookAt();
            case CinemachineOperate.SetPriority:
                return new SetPriority();
            default:
                return new VMOperateExecuter();
        }
    }
}