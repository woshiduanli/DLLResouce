//#define test
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Model
{
    //Action播放模式
    public enum ActionWrapMode
    {
        ONCE = 1,               //单次播放，播放完所有ActionEvent就停止
        EXT_END = 2,            //等待外部通知停止
    }

    //Action事件类型
    public enum ActionEventType
    {
        ANIMATION = 0,//播放动画
        EFFECT,//播放特效
        FIREBALL, //播放一个火球类特效
        AUDIO,//播放声音
        SCREEN_BLACK,//开启压屏效果
        CAMERA_SHACK,  //开启震屏
        RADIALBLUR,//开启直线模糊效果
        DEAD,//死亡效果
        SLOW,//慢动作
        HEATDISTORT,//扭曲
        TRANSFORM,//位移
        HIDING,// 隐藏，隐身，用于不显示模型的效果
        XPBODYCHANGE,//xp变身事件
    }

    public enum EventPart
    {
        SING = 0,   //吟唱阶段
        FIRE,       //施放阶段
        HIT,        //命中阶段
        FIREBALLHIT,//轨迹特效命中
        BUFF,       //BUFF
    }

    public enum PathType
    {
        Linear = 0,
        CatmullRom = 1,
        LeftCatmullRom =2,
        RightCatmullRom =3
    }

    public class Action : ScriptableObject
    {
        [CommonAttribute("Action名")]
        public string ActionName;

        [CommonAttribute("Action时长")]
        public float LifeTime = 5.0f;

        [CommonAttribute("Action事件")]
        public ActionEvent[] ActionEvents;

        [System.Serializable()]
        public class ActionEvent
        {
            [EnumPopup("事件类型", new string[] {"播放动画", "播放特效", "播放火球", "播放声音", "压屏", "震屏", "直线模糊", "死亡", "慢动作", "扭曲", "位移", "隐身", "xp变身" })]
            public ActionEventType EventType = ActionEventType.ANIMATION;

            [CommonAttribute("事件名")]
            public string ActionEventName;

            [CommonAttribute("激活时间")]
            public float StartTime;

            [CommonAttribute("持续时间"), Tooltip("相对于当前事件的开始时间")]
            public float EndTime;

            [EnumPopup("事件阶段", new string[] { "吟唱", "攻击", "命中", "火球命中", "BUFF" })]
            public EventPart EventPart = EventPart.FIRE;

            [ConditionalHideAttribute("动作名", "EventType", true, (int)ActionEventType.ANIMATION), Tooltip("与动作控制器挂钩")]
            public MotionState State = MotionState.stand;

            [ConditionalHideAttribute("动作不可打断时间", "EventType", true, (int)ActionEventType.ANIMATION)]
            public float RigorTime;

            [ConditionalHideAttribute("是否可以移动", "EventType", true, (int)ActionEventType.ANIMATION)]
            public bool CanMove;

            [ConditionalHideAttribute("移动数据", "EventType", true, (int)ActionEventType.ANIMATION)]
            public List<MoveData> MoveDatas;

            [AssetToFilePath("音效路劲", ".audio", "EventType", true, (int)ActionEventType.EFFECT , (int)ActionEventType.FIREBALL , (int)ActionEventType.AUDIO)]
            public string AudioPath = string.Empty;

            [AssetToFilePath("资源路劲", ".go", "EventType", true, (int)ActionEventType.EFFECT , (int)ActionEventType.FIREBALL ,
                                                             (int)ActionEventType.CAMERA_SHACK , (int)ActionEventType.SCREEN_BLACK)]
            public string SourcePath = string.Empty;

            [AssetToFilePath("低配资源路劲", ".go", "EventType", true, (int)ActionEventType.EFFECT, (int)ActionEventType.FIREBALL,
                                                 (int)ActionEventType.CAMERA_SHACK, (int)ActionEventType.SCREEN_BLACK)]
            public string LowSourcePath = string.Empty;

            [ConditionalHideAttribute("控制波束", "SourcePath|EventType", true, (int)ActionEventType.EFFECT, (int)ActionEventType.FIREBALL,
                                                                          (int)ActionEventType.CAMERA_SHACK, (int)ActionEventType.SCREEN_BLACK), 
                                                                           Tooltip("防止这个事件在每个波束都出现，暂时由程序控制")]
            public bool IsControlWave = false;

            [ConditionalHideAttribute("以目标计算坐标", "SourcePath|EventType", true, (int)ActionEventType.EFFECT, (int)ActionEventType.FIREBALL,
                                                                                (int)ActionEventType.CAMERA_SHACK, (int)ActionEventType.SCREEN_BLACK), 
                                                                                Tooltip("特效的坐标将根据目标或者自身计算")]
            public bool IsActOnTarget = false;

            [ConditionalHideAttribute( "绑定骨骼名", "SourcePath|EventType", true, (int)ActionEventType.EFFECT, (int)ActionEventType.FIREBALL,
                                                                             (int)ActionEventType.CAMERA_SHACK, (int)ActionEventType.SCREEN_BLACK)]
            public string BindBone = string.Empty;

            [ConditionalHideAttribute("坐标", "SourcePath|EventType", true, (int)ActionEventType.EFFECT, (int)ActionEventType.FIREBALL,
                                                                      (int)ActionEventType.CAMERA_SHACK, (int)ActionEventType.SCREEN_BLACK)]
            public Vector3 Offset;

            [ConditionalHideAttribute("按比例偏移坐标(值为0-1)", "SourcePath|EventType", true, (int)ActionEventType.EFFECT, (int)ActionEventType.FIREBALL,
                                                                      (int)ActionEventType.CAMERA_SHACK, (int)ActionEventType.SCREEN_BLACK)]
            public bool IsRatioOffset;

            [ConditionalHideAttribute("偏移距离", "SourcePath|EventType", true, (int)ActionEventType.EFFECT, (int)ActionEventType.FIREBALL,
                                                                          (int)ActionEventType.CAMERA_SHACK, (int)ActionEventType.SCREEN_BLACK)]
            public float Distance;

            [ConditionalHideAttribute("旋转", "SourcePath|EventType", true, (int)ActionEventType.EFFECT, (int)ActionEventType.FIREBALL,
                                                                      (int)ActionEventType.CAMERA_SHACK, (int)ActionEventType.SCREEN_BLACK)]
            public Vector3 Angle;

            [ConditionalHideAttribute( "移动速度", "SourcePath|EventType", true, (int)ActionEventType.EFFECT, (int)ActionEventType.FIREBALL,
                                                                          (int)ActionEventType.CAMERA_SHACK, (int)ActionEventType.SCREEN_BLACK)]
            public float Speed = 1;

            [ConditionalHideAttribute("顿帧强度", "SourcePath|EventType", true, (int)ActionEventType.SLOW)]
            public float SlowPower = 0.2f;

            [ConditionalHideAttribute("产生间隔", "SourcePath|EventType", true, (int)ActionEventType.EFFECT, (int)ActionEventType.FIREBALL,
                                                                          (int)ActionEventType.CAMERA_SHACK, (int)ActionEventType.SCREEN_BLACK)]
            public float Duration;

            [ConditionalHideAttribute("召唤ID", "EventType", true, (int)ActionEventType.XPBODYCHANGE)]
            public int CallApprid;

            [EnumPopup("轨迹类型", new string[] { "直线", "顶曲线", "左曲线", "右曲线" }), ConditionalHideAttribute("轨迹类型", "EventType", true, (int)ActionEventType.FIREBALL)]
            public PathType PathType = PathType.Linear;

            [ConditionalHideAttribute("曲线最高点偏移", "PathType", true, (int)PathType.CatmullRom, (int)PathType.LeftCatmullRom, (int)PathType.RightCatmullRom)]
            public float OffsetMaxY = 1;

            [ConditionalHideAttribute("Child", "SourcePath|EventType", true, (int)ActionEventType.EFFECT, (int)ActionEventType.FIREBALL,
                                                                           (int)ActionEventType.CAMERA_SHACK, (int)ActionEventType.SCREEN_BLACK)]
            public Childs[] Child;
        }

        [System.Serializable()]
        public class Childs
        {
            [CommonAttribute("旋转"), Tooltip("相对于事件资源的旋转，计算旋转时将叠加事件的本身的旋转")]
            public Vector3 Angle;
            [CommonAttribute("偏移"), Tooltip("相对于事件资源的偏移，计算旋转时将叠加事件的本身的坐标")]
            public Vector3 Offset;
            [CommonAttribute("产生间隔"), Tooltip("相对于事件资源的激活时间，计算旋转时将叠加事件的本身的激活时间")]
            public float Duration;
            [EnumPopup("轨迹类型", new string[] { "直线", "顶曲线", "左曲线", "右曲线" }), ConditionalHideAttribute("轨迹类型", "EventType", true, (int)ActionEventType.FIREBALL)]
            public PathType PathType = PathType.Linear;
            [ConditionalHideAttribute("曲线最高点偏移", "PathType", true, (int)PathType.CatmullRom, (int)PathType.LeftCatmullRom,(int)PathType.RightCatmullRom)]
            public float OffsetMaxY = 1;
        }

        [System.Serializable()]
        public class MoveData
        {
            [CommonAttribute("开始时间")]
            public float StartTime;
            [CommonAttribute("距离")]
            public float Distance;
            [CommonAttribute("结束时间")]
            public float EndTime;
        }
    }
}

