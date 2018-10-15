using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class VMOperateClip : PlayableAsset
{

    [HideInInspector]
    public CinemachineVirtualCameraBase cinemachineCamera;
    public ExposedReference<RoleData> roleData;
    public int priority;
    public CinemachineOperate operateType;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<VMOperateBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();
        if (behaviour == null)
            return playable;
        behaviour.cinemachineCamera = cinemachineCamera;
        behaviour.roleData = roleData.Resolve(graph.GetResolver());
        behaviour.priority = priority;
        behaviour.operateType = operateType;
        behaviour.executer = BehaviourExecuterFactory.GetVMOperateExecuter(behaviour);
        if (behaviour.executer == null)
            return playable;
        behaviour.executer.OnPlayableCreate(playable);
        return playable;
    }

}



public class VMOperateBehaviour : MYPlayableBehaviour
{
    public CinemachineVirtualCameraBase cinemachineCamera;
    public CinemachineOperate operateType;
    public RoleData roleData;
    public int priority;

}




public enum CinemachineOperate
{
    RemoveFollow,
    AddFollow,
    RemoveTarget,
    AddTarget,
    SetPriority,
}
