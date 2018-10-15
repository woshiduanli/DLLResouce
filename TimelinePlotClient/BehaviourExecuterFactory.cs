using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Playables;
using UnityEngine;


/// <summary>
/// 具体行为执行类的工厂
/// 编辑器和客户端会传入委托的具体实现去创建各自具体的行为执行类
/// 在创建每个playablebehaviour时会调用工厂中的创建方法，最终调用到传进来的委托，创建出具体的执行类
/// </summary>
public static class BehaviourExecuterFactory
{
    //执行动作
    public static Func<BehaviourExecuterBase> CreatAnimationExecuter;
    //播放action
    public static Func<BehaviourExecuterBase> CreatActionExecuter;
    //移动
    public static Func<BehaviourExecuterBase> CreatMoveExecuter;
    //对话：不弹半身像对话框
    public static Func<BehaviourExecuterBase> CreatDialogueUiExecuter;
    //对话：要弹半身像对话框
    public static Func<BehaviourExecuterBase> CreatPlotDialogueUiExecuter;
    //文字泡泡
    public static Func<BehaviourExecuterBase> CreatBubbleUiExecuter;
    //摄像机效果执行（震屏等）
    public static Func<BehaviourExecuterBase> CreatCameraEffectExecuter;
    //虚拟摄像机操作clip执行（移除或者添加follow，lookat等）
    public static Func<CinemachineOperate, BehaviourExecuterBase> CreatVmOperateExecuter;
    //特效clip执行（播放一个特效go文件）
    public static Func<BehaviourExecuterBase> CreatEffectExecuter;
    //控制显示隐藏
    public static Func<BehaviourExecuterBase> CreatActivationControlExecuter;
    //大小缩放
    public static Func<BehaviourExecuterBase> CreatScaleControlExecuter;
    //角色效果：效果类型->RoleEffectType
    public static Func<RoleEffectType,BehaviourExecuterBase> CreatRoleEffectExecuter;
    //播放音频
    public static Func<BehaviourExecuterBase> CreatAudioExecuter;



    public static BehaviourExecuterBase GetRoleEffectExecuter(MYPlayableBehaviour behaviour,RoleEffectType type)
    {
        if (CreatRoleEffectExecuter == null)
            return null;
        var executer = CreatRoleEffectExecuter(type);
        if (executer == null)
            return null;
        executer.behaviour = behaviour;
        return executer;
    }

    public static BehaviourExecuterBase GetScaleControlExecuter(MYPlayableBehaviour behaviour)
    {
        if (CreatScaleControlExecuter == null)
            return null;
        var executer = CreatScaleControlExecuter();
        if (executer == null)
            return null;
        executer.behaviour = behaviour;
        return executer;
    }

    public static BehaviourExecuterBase GetActivationControlExecuter(MYPlayableBehaviour behaviour)
    {
        if (CreatActivationControlExecuter == null)
            return null;
        var executer = CreatActivationControlExecuter();
        if (executer == null)
            return null;
        executer.behaviour = behaviour;
        return executer;
    }

    public static BehaviourExecuterBase GetAnimationExecuter(MYPlayableBehaviour behaviour)
    {
        if (CreatAnimationExecuter == null)
            return null;
        BehaviourExecuterBase animationExecuter = CreatAnimationExecuter();
        if (animationExecuter == null)
            return null;
        animationExecuter.behaviour = behaviour;
        return animationExecuter;
    }

    public static BehaviourExecuterBase GetActionExecuter(MYPlayableBehaviour behaviour)
    {
        if (CreatActionExecuter == null)
            return null;
        BehaviourExecuterBase actionExecuter = CreatActionExecuter();
        if (actionExecuter == null)
            return null;
        actionExecuter.behaviour = behaviour;
        return actionExecuter;
    }

    public static BehaviourExecuterBase GetMoveExecuter(MYPlayableBehaviour behaviour)
    {
        if (CreatMoveExecuter == null)
            return null;
        BehaviourExecuterBase moveExecuter = CreatMoveExecuter();
        if (moveExecuter == null)
            return null;
        moveExecuter.behaviour = behaviour;
        return moveExecuter;
    }

    public static BehaviourExecuterBase GetDialogueUIExecuter(MYPlayableBehaviour behaviour)
    {
        if (CreatDialogueUiExecuter == null)
            return null;
        BehaviourExecuterBase dialogueUiExecuter = CreatDialogueUiExecuter();
        if (dialogueUiExecuter == null)
            return null;
        dialogueUiExecuter.behaviour = behaviour;
        return dialogueUiExecuter;
    }

    public static BehaviourExecuterBase GetBubbleUIExecuter(MYPlayableBehaviour behaviour)
    {
        if (CreatBubbleUiExecuter == null)
            return null;
        BehaviourExecuterBase bubbleUiExecuter = CreatBubbleUiExecuter();
        if (bubbleUiExecuter == null)
            return null;
        bubbleUiExecuter.behaviour = behaviour;
        return bubbleUiExecuter;
    }

    public static BehaviourExecuterBase GetCameraEffectExecuter(MYPlayableBehaviour behaviour)
    {
        if (CreatCameraEffectExecuter == null)
            return null;
        BehaviourExecuterBase cameraEffectExecuter = CreatCameraEffectExecuter();
        if (cameraEffectExecuter == null)
            return null;
        cameraEffectExecuter.behaviour = behaviour;
        return cameraEffectExecuter;
    }

    public static BehaviourExecuterBase GetVMOperateExecuter(MYPlayableBehaviour behaviour)
    {
        VMOperateBehaviour vmBehaviour = behaviour as VMOperateBehaviour;
        if (CreatVmOperateExecuter == null || vmBehaviour == null)
            return null;
        BehaviourExecuterBase vmOperateExecuter = CreatVmOperateExecuter(vmBehaviour.operateType);
        if (vmOperateExecuter == null)
            return null;
        vmOperateExecuter.behaviour = behaviour;
        return vmOperateExecuter;
    }

    public static BehaviourExecuterBase GetEffectExecuter(MYPlayableBehaviour behaviour)
    {
        if (CreatEffectExecuter == null)
            return null;
        BehaviourExecuterBase effectExecuter = CreatEffectExecuter();
        if (effectExecuter == null)
            return null;
        effectExecuter.behaviour = behaviour;
        return effectExecuter;
    }

    public static BehaviourExecuterBase GetAudioExecuter(MYPlayableBehaviour behaviour)
    {
        if (CreatAudioExecuter == null)
            return null;
        BehaviourExecuterBase audioExecuter = CreatAudioExecuter();
        if (audioExecuter == null)
            return null;
        audioExecuter.behaviour = behaviour;
        return audioExecuter;
    }

    public static BehaviourExecuterBase GetPlotDialogueUIExecuter(PlotDialogueBehaviour behaviour)
    {
        if (CreatPlotDialogueUiExecuter == null)
            return null;
        BehaviourExecuterBase dialogueUiExecuter = CreatPlotDialogueUiExecuter();
        if (dialogueUiExecuter == null)
            return null;
        dialogueUiExecuter.behaviour = behaviour;
        return dialogueUiExecuter;
    }
}



/// <summary>
/// 执行类的基类
/// behaviour为所属的playablebehaviour
/// 各个播放过程的接口，有playable调用
/// 具体行为需以这个类为基类
/// </summary>
public class BehaviourExecuterBase
{
    public MYPlayableBehaviour behaviour;

    public virtual void OnBehaviourStart(Playable playable) { }
    public virtual void OnBehaviourDone(Playable playable) { }
    public virtual void OnBehaviourPause(Playable playable) { }
    public virtual void OnBehaviourResume(Playable playable) { }
    public virtual void OnGraphStart(Playable playable) { }
    public virtual void OnGraphStop(Playable playable) { }
    public virtual void OnPlayableCreate(Playable playable) { }
    public virtual void OnPlayableDestroy(Playable playable) { }
    public virtual void PrepareFrame(Playable playable) { }
    public virtual void ProcessFrame(Playable playable, object playerData) { }

    public static implicit operator bool(BehaviourExecuterBase x)
    {
        return x != null;
    }
}



/// <summary>
/// 对unity的playablebehaviour类进行封装
/// 维护了一些基本数据
/// 抽象了一些实用的方法供派生类继承
/// 所有具体的playablebehaviour都继承这个类，而不直接继承unity的playablebehavour类
/// </summary>
public class MYPlayableBehaviour : PlayableBehaviour
{
    protected bool isStartPlaying;//是否开始播放
    protected bool isPause;       //是否处于暂停状态
    protected bool isPlayOver;    //是否播放完
    public float duration;
    public float curTime;

    public BehaviourExecuterBase executer;


    //每次暂停或者播放完毕执行（执行PlayableCreat时，也会执行一次）
    public sealed override void OnBehaviourPause(Playable playable, FrameData info)
    {
        base.OnBehaviourPause(playable, info);
        if (!isStartPlaying)
            return;
        isPause = true;
        isPlayOver = playable.GetDuration() - playable.GetTime() <= 0.05f;
        if (isPlayOver)
            OnMYBehaviourDone(playable);
        else
            OnMYBehaviourPause(playable);
    }

    //开始播放以及暂停后重新播放会执行
    public sealed override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        base.OnBehaviourPlay(playable, info);
        if (isStartPlaying)
        {
            OnMYBehaviourResume(playable);
            return;
        }
        isStartPlaying = true;
        OnMYBehaviourStart(playable);
    }

    public sealed override void OnGraphStart(Playable playable)
    {
        base.OnGraphStart(playable);
        duration = (float)playable.GetDuration();
        OnMYGraphStart(playable);
    }

    public sealed override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        base.ProcessFrame(playable, info, playerData);
        curTime = (float)playable.GetTime();
        OnProcessFrame(playable, playerData);
    }




    public virtual void OnMYGraphStart(Playable playable)
    {
        if (executer)
            executer.OnGraphStart(playable);
    }

    public virtual void OnProcessFrame(Playable playable, object playerData)
    {
        if (executer)
            executer.ProcessFrame(playable, playerData);
    }

    //播放完毕执行
    public virtual void OnMYBehaviourDone(Playable playable)
    {
        if (executer)
            executer.OnBehaviourDone(playable);
    }

    //播放中途暂停执行
    public virtual void OnMYBehaviourPause(Playable playable)
    {
        if (executer)
            executer.OnBehaviourPause(playable);
    }

    //开始播放执行
    public virtual void OnMYBehaviourStart(Playable playable)
    {
        if (executer)
            executer.OnBehaviourStart(playable);
    }

    //暂停后继续播放执行
    public virtual void OnMYBehaviourResume(Playable playable)
    {
        if (executer)
            executer.OnBehaviourResume(playable);
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        base.PrepareFrame(playable, info);
        if(executer)
            executer.PrepareFrame(playable);
    }

    public override void OnPlayableDestroy(Playable playable)
    {
        base.OnPlayableDestroy(playable);
        if(executer)
            executer.OnPlayableDestroy(playable);
    }
}