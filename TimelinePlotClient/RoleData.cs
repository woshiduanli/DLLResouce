using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色数据类
/// </summary>
public class RoleData : MonoBehaviour
{
    //地图表中配置的sn，如果需要通过sn查找游戏中物体，则使用这个
    public int Sn;
    //角色的配置Id
    public int Id;
    //运行时赋值的角色或地图物件（roleobject或者storyobject或者storymapobject）
    public object Role;
    [Header("是否通过sn到游戏中查找角色:")]
    public bool IsFindGameRoleBySn;
    public bool IsHideRoleInGame;
    public TimelineRoleType RoleType= TimelineRoleType.Monster;
    public TimelineRoleOcc RoleOcc= TimelineRoleOcc.ZhanShi;
    public RoleSourceType RoleSourceType= RoleSourceType.Alone;
    //剧情开始时的初始位置和朝向
    public Vector3 InitialPos;
    public Vector3 InitialRotation;
    //剧情播放中需要的单独的animationclip，需提前加载
    public string[] AnimClipPath;
    [Header("跳过剧情后的位置")]
    public Vector3 TargetPosWhenSkip;
}


/// <summary>
/// 角色类型
/// </summary>
public enum TimelineRoleType
{
    MainPlayer,
    Npc,
    Monster,
    Object,//地图物件
    Player
}


/// <summary>
/// 角色来源类型：单独加载或查找游戏中物体
/// </summary>
public enum RoleSourceType
{
    InGame,
    Alone
}


/// <summary>
/// 角色职业
/// </summary>
public enum TimelineRoleOcc
{
    ZhanShi=1,
    FaShi=2,
    XueZu=3
}
