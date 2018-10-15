using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Model
{
    public enum MotionState
    {
        stand = 0,
        walk,
        run,
        attack01,
        attack02,
        attack03,
        attack04,
        attack05,
        skill01,
        skill01_01,
        skill01_02,
        skill01_03,
        skill02,
        skill03,
        skill04,
        hit,
        dead,
        chong,
        bianshen,
        jump1,
        jump2,
        jump3,
        ride,
        show,
        hitfly,
        stun,   // 被控制，比如：眩晕，冰冻等
        idle,//主城待机
        skill02_1,
        talk,
        collect,
        direct,
        skill05,
        skill06,
        skill07,
        skill08,
        skill09,
        skill10,
        hit01,
        uiidle,//ui待机
        leisure,//休闲待机
        worship,//跪拜
        bless,//女王平身手势
        send,//NPC掏蛋
        weak,//NPC虚弱
        ease,//NPC虚弱回复
        forge,//锻造
        inspire,//鼓舞
        run1,
        stare,//回眸一笑
        hide,//隐身
        born,//出生
        combine,//合体
        angry,//生气
        forgetoidle,//锻造过度到idle
        laugh,//笑
        uishow,
    }

    /// <summary>
    /// 角色完整配置
    /// </summary>
    public class Role : MonoBehaviour
    {
        public const string ParameterName = "state";

        [CommonAttribute("离地高度")]
        public float FlyHeight;

        [CommonAttribute("骨骼列表")]
        public GameObject[] Bones;

        [CommonAttribute("基本外观")]
        public BaseLook[] Equips = new BaseLook[0];

        [CommonAttribute("碰撞盒")]
        public GameObject BoxCollider;

        [AssetToFilePath("贴图", ".tex")]
        public string MainTex;

        [AssetToFilePath("模型", ".mesh")]
        public string Mesh = string.Empty;

        [CommonAttribute("蒙皮模型")]
        public SkinnedMeshRenderer Skin;

        [CommonAttribute("高模蒙皮模型")]
        public SkinnedMeshRenderer HighSkin;

        [ConditionalHideAttribute("普通模型", "Mesh|MainTex",false)]
        public MeshFilter Filter;

        [SerializeField]
        public OverrideAnimiClip[] AnimiClips;
    }

    [System.Serializable]
    public class BaseLook
    {
        [IntPopup("占位", new string[] { "身体", "备用", "右手武器", "头", "脸", "翅膀" }, new int[] { 0, 1, 2, 3, 4, 5, })]
        public int Mask;
        [AssetToFilePath("基本外观", ".equip")]
        public string Equip;
    }

    [System.Serializable]
    public class OverrideAnimiClip
    {
        public string Key;
        public AnimationClip Clip;

        public OverrideAnimiClip() { }
        public OverrideAnimiClip(string key, AnimationClip clip)
        {
            this.Key = key;
            this.Clip = clip;
        }
    }
}