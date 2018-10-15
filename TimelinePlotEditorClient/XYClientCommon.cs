#define DEBUG
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public static class ForEach
{
    public static void ExecuteChildren(GameObject go, Action<GameObject> action, bool includeRoot)
    {
        if (go)
        {
            if (includeRoot)
            {
                action(go);
            }
            foreach (Transform ts in go.transform)
            {
                ExecuteChildren(ts.gameObject, action, true);
            }
        }
    }
}

public static class XYClientCommon
{
    public const int IgnoreChangLayer = (XYDefines.Layer.Effect);

    public const int ClickMoveLayer =
        (XYDefines.Layer.Mask.Default | XYDefines.Layer.Mask.Terrain | XYDefines.Layer.Mask.MainBuilding |
         XYDefines.Layer.Mask.Prop /*| XYDefines.Layer.Mask.Ding*/);

    public const int StandingLayer = XYDefines.Layer.Mask.Terrain;
        //(XYDefines.Layer.Mask.Default | XYDefines.Layer.Mask.Terrain | XYDefines.Layer.Mask.MainBuilding |
        // XYDefines.Layer.Mask.Prop | XYDefines.Layer.Mask.IgnoreRaycast /*| XYDefines.Layer.Mask.Ding*/);

    public const int StandingLayerMonster =
        (XYDefines.Layer.Mask.Default | XYDefines.Layer.Mask.Terrain | XYDefines.Layer.Mask.MainBuilding |
         XYDefines.Layer.Mask.Prop /*| XYDefines.Layer.Mask.Ding*/);

    public const int StandingLayerBlind =
        (XYDefines.Layer.Mask.Default | XYDefines.Layer.Mask.Terrain | XYDefines.Layer.Mask.MainBuilding |
         XYDefines.Layer.Mask.Prop | XYDefines.Layer.Mask.IgnoreRaycast /*| XYDefines.Layer.Mask.Ding*//* | XYDefines.Layer.Mask.Tree*/);

    public const int ShadowProjectorIgnoreLayer =
        XYDefines.Layer.Mask.Default | XYDefines.Layer.Mask.Player /*| XYDefines.Layer.Mask.Ding*/ |
        XYDefines.Layer.Mask.Water | /*XYDefines.Layer.Mask.Tree |*/ XYDefines.Layer.Mask.Effect;

    public static readonly Vector3 GRAVITY_VECTOR3 = new Vector3(0, -20f, 0);

    public static readonly float DOUBLE_PI = Mathf.PI * 2;
    public static readonly float HALF_PI = Mathf.PI * 0.5f;
    public static readonly float QUARTER_PI = Mathf.PI * 0.25f;

    // 改变一个GameObject的Layer
    public static void ChangeLayer(GameObject obj, int layer, bool includeChildren)
    {
        if (obj)
        {
            ChangeLayer(obj.transform, layer, includeChildren, XYDefines.Layer.Default);
        }
    }

    // 改变一个Transform所附着的GameObject的Layer
    public static void ChangeLayer(Transform trans, int layer, bool includeChildren, int ignorelayer)
    {
        if (trans)
        {
            if ((IgnoreChangLayer & ignorelayer) != ignorelayer || ignorelayer == XYDefines.Layer.Default)
            {
                trans.gameObject.layer = layer;
            }
            if (includeChildren)
            {
                foreach (Transform child in trans)
                {
                    ChangeLayer(child, layer, true, ignorelayer);
                }
            }
        }
    }

    // 设置一个gameobject的render
    public static void EnableRender(GameObject obj, bool render, bool includeChildren)
    {
        if (obj && obj.activeSelf)
        {
            EnableRender(obj.transform, render, includeChildren);
        }
    }

    // 改变一个Transform所附着的GameObject的Layer
    public static void EnableRender(Transform trans, bool render, bool includeChildren)
    {
        if (!trans) return;
        if (trans.GetComponent<Renderer>() != null)
        {
            trans.GetComponent<Renderer>().enabled = render;
        }
        var pr = trans.GetComponent<ParticleRenderer>();
        if (pr != null)
        {
            pr.enabled = render;
        }
        var tr = trans.GetComponent<TrailRenderer>();
        if (tr != null)
        {
            tr.enabled = render;
        }
        if (!includeChildren) return;
        foreach (Transform child in trans)
        {
            EnableRender(child, render, true);
        }
    }

    public static void DestroyObject<T>(ref T obj) where T : Object
    {
        if (!obj) return;
        Object.Destroy(obj);
        obj = null;
    }

    public static void RemoveComponent<T>(GameObject go) where T : Component
    {
        if (go)
        {
            Component c = go.GetComponent<T>();
            if (c)
            {
                Object.Destroy(c);
            }
        }
    }


    public static GameObject FindChild(GameObject go, Predicate<GameObject> pred)
    {
        if (go)
        {
            foreach (Transform ts in go.transform)
            {
                if (pred(ts.gameObject))
                {
                    return ts.gameObject;
                }
                GameObject result = FindChild(ts.gameObject, pred);
                if (result)
                {
                    return result;
                }
            }
        }
        return null;
    }

    public static void FindChildren(GameObject go, Predicate<GameObject> pred, List<GameObject> list)
    {
        if (go)
        {
            foreach (Transform ts in go.transform)
            {
                if (pred(ts.gameObject))
                {
                    list.Add(ts.gameObject);
                }
                FindChildren(ts.gameObject, pred, list);
            }
        }
    }

    public static void ForEachChildren(GameObject go, Action<GameObject> action, bool includeRoot)
    {
        if (go)
        {
            if (includeRoot)
            {
                action(go);
            }
            foreach (Transform ts in go.transform)
            {
                ForEachChildren(ts.gameObject, action, true);
            }
        }
    }

    public static float AdjustAngleRad(float angleRad)
    {
        if (angleRad <= -DOUBLE_PI)
        {
            angleRad += DOUBLE_PI;
        }
        else if (angleRad >= DOUBLE_PI)
        {
            angleRad -= DOUBLE_PI;
        }
        return angleRad;
    }

    public static float YawFromXZRad(Vector2 v)
    {
        return YawFromXZRad(v.x, v.y);
    }

    public static float YawFromXZRad(Vector3 v)
    {
        return YawFromXZRad(v.x, v.z);
    }

    public static float YawFromXZRad(float x, float z)
    {
        float r = Mathf.Sqrt(x * x + z * z);
        float yaw = Mathf.Asin(x / r);
        if (z < 0)
        {
            yaw = Mathf.PI - yaw;
        }
        return AdjustAngleRad(yaw);
    }

    //public static float AngleBetweenNormalizedVector_Rad(Vector3 v1, Vector3 v2) {
    //    float result = Mathf.Acos(Vector3.Dot(v1, v2));
    //    if (v2.X < v1.X
    //}

    // 玩家跳跃

    /// <summary>
    ///     将给定点的位置调整到地面
    /// </summary>
    /// <param name="pos"></param>
    public static void AdjustToGround(ref Vector3 pos, bool ignoreAirWall)
    {
        pos = AdjustToGround(pos, ignoreAirWall);
    }

    public static void AdjustToGround(ref Vector3 pos)
    {
        AdjustToGround(ref pos, false);
    }

    /// <summary>
    ///     从给定平面坐标向下做射线
    /// </summary>
    /// <param name="pos">水平坐标</param>
    /// <param name="ignoreAirWall">忽略空气墙吗？</param>
    /// <param name="hit">返回信息</param>
    /// <returns>是否射到东西？</returns>
    public static bool RaycastDown(Vector3 pos, bool ignoreAirWall, out RaycastHit hit)
    {
        pos.y = 2000f;
        return Physics.Raycast(pos, Vector3.down, out hit, 5000f, ignoreAirWall ? StandingLayerMonster : StandingLayer);
    }

    /// <summary>
    ///     找出给定地点的地面法线
    /// </summary>
    /// <param name="pos">坐标</param>
    /// <param name="ignoreAirWall">是否忽略空气墙</param>
    /// <returns>给定点的地面法线</returns>
    public static Vector3 FindPosNormal(Vector3 pos, bool ignoreAirWall)
    {
        RaycastHit hit;
        if (RaycastDown(pos, ignoreAirWall, out hit))
        {
            return hit.normal;
        }
        else
        {
            return Vector3.up;
        }
    }

    /// <summary>
    ///     将给定水平坐标调整到地面点
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="ignoreAirWall"></param>
    /// <returns></returns>
    public static Vector3 AdjustToGroundBlind(Vector3 pos)
    {
        RaycastHit hit;
        pos.y = 2000f;
        Physics.Raycast(pos, Vector3.down, out hit, 5000f, StandingLayerBlind);
        return hit.point;
    }

    public static Vector3 AdjustToGround(Vector3 pos, bool ignoreAirWall)
    {
        RaycastHit hit;
        if (RaycastDown(pos, ignoreAirWall, out hit))
        {
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public static Vector3 AdjustToGround(Vector3 pos)
    {
        return AdjustToGround(pos, false);
    }

    public static void AdjustRoleToGround(Vector3 pos, CharacterController cc)
    {
        pos = AdjustToGround(pos, false);
        pos.y += 5f;
        cc.transform.position = pos;
        cc.Move(GRAVITY_VECTOR3);
    }

    public static void MoveToPosition(Vector3 pos, CharacterController cc)
    {
        pos = AdjustToGround(pos, false);
        cc.Move(pos - cc.transform.position);
        cc.Move(GRAVITY_VECTOR3);
    }

    // 将Layer加到layerMask中
    public static int AddToLayerMask(int layerMask, int layer)
    {
        return layerMask | (1 << layer);
    }

    public static int AddToLayerMask(int layerMask, params int[] layers)
    {
        foreach (int l in layers)
        {
            layerMask = AddToLayerMask(layerMask, l);
        }
        return layerMask;
    }

    // 从layerMask中去掉layer
    public static int RemoveFromLayerMask(int layerMask, int layer)
    {
        return layerMask & (~(1 << layer));
    }

    public static int RemoveFromLayerMask(int layerMask, params int[] layers)
    {
        foreach (int l in layers)
        {
            layerMask = RemoveFromLayerMask(layerMask, l);
        }
        return layerMask;
    }

    public static bool WithinDistance(float x1, float y1, float x2, float y2, float dist)
    {
        return ((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)) <= (dist * dist);
    }

    public static int CalculateRate(int standard, int rate)
    {
        return (int)(standard * (rate / 10000.0f));
    }

    public static int CalculateMoney(int price, int count, int rate)
    {
        int money = CalculateRate(price * count, rate);
        if (money < count) return count;
        return money;
    }

    public static class AirWall
    {
        public static readonly string TAG_NAME = "airwalls";

        public static GameObject FindRoot()
        {
            return GameObject.FindWithTag(TAG_NAME);
        }

        public static GameObject[] FindAll()
        {
            return GameObject.FindGameObjectsWithTag(TAG_NAME);
        }

        public static GameObject[] RemoveRenderAndMesh(bool destroyMeshFilter)
        {
            GameObject[] airwalls = FindAll();
            foreach (GameObject obj in airwalls)
            {
                ForEachChildren(
                    obj,
                    delegate(GameObject go)
                    {
                        go.layer = XYDefines.Layer.IgnoreRaycast;
                        var mf = go.GetComponent<MeshFilter>();
                        if (mf)
                        {
                            if (!go.GetComponent<Collider>())
                            {
                                go.AddComponent<MeshCollider>();
                            }
                            if (destroyMeshFilter)
                            {
                                Object.Destroy(mf);
                            }
                        }
                        RemoveComponent<Renderer>(go);
                    },
                    true
                    );
            }
            return airwalls;
        }

        public static GameObject[] EnableRender(bool enable)
        {
            GameObject[] airwalls = FindAll();
            foreach (GameObject obj in airwalls)
            {
                ForEachChildren(
                    obj,
                    delegate(GameObject go)
                    {
                        if (go.GetComponent<MeshFilter>())
                        {
                            Renderer r = go.GetComponent<Renderer>();
                            if (r)
                            {
                                r.enabled = enable;
                            }
                            else if (enable)
                            {
                                go.AddComponent<MeshRenderer>();
                            }
                        }
                    },
                    true);
            }
            return airwalls;
        }
    }

    public class PlayerJump
    {
        private float maxJumpHeight_;
        private Transform player_;
        private float vertSpeed_;

        public PlayerJump(Transform player)
        {
            Reset(player);
        }

        public void Reset()
        {
            if (player_)
            {
                maxJumpHeight_ = player_.position.y + XYDefines.Player.JumpHeight;
                vertSpeed_ = Mathf.Sqrt(2 * XYDefines.Player.Gravity * XYDefines.Player.JumpHeight);
            }
        }

        public void Reset(Transform player)
        {
            player_ = player;
            Reset();
        }

        public float UpdateVertSpeed(out bool falling, float deltaTime)
        {
            if (player_)
            {
                vertSpeed_ -= XYDefines.Player.Gravity * deltaTime;
                if (vertSpeed_ <= 0 || player_.position.y >= maxJumpHeight_)
                {
                    falling = true;
                }
                else
                {
                    falling = false;
                }
                return vertSpeed_;
            }
            else
            {
                falling = false;
                return 0;
            }
        }

        public float UpdateVertSpeed(float deltaTime)
        {
            bool falling;
            return UpdateVertSpeed(out falling, deltaTime);
        }
    }
}

public static class XYDefines
{
    public const float MAP_GRID_SIZE = 1f; // 寻路时格子大小
    public const float MAP_GRID_SIZE_HALF = (MAP_GRID_SIZE * 0.5f);
    public const float OBSTACLE_HIGH_DELTA = 0.3f; // 高度差多少算不可到达？
    public static readonly string TWO_CHINESE_SPACE = "　　";

    #region 头顶名字颜色定义

    public static class RoleTitleColor
    {
        public static Color CreateColor(int r, int g, int b, int a)
        {
            return new Color(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
        }

        public static Color CreateColor(int r, int g, int b)
        {
            return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
        }

        // 名字

        // 附加信息
        public static class Additive
        {
            public static class Npc
            {
                // NPC的称号
                public static /*readonly*/ Color Nickname = CreateColor(255, 178, 65);
                // 摊位说明
                public static /*readonly*/ Color StallComment = CreateColor(255, 216, 35);
            }

            public static class Player
            {
                // 玩家称号
                public static /*readonly*/ Color Nickname = CreateColor(0, 168, 255);
                // 军团称号
                public static /*readonly*/ Color GroupInfo = CreateColor(51, 236, 62);
                //敌对军团
                public static /*readonly*/ Color EnemyGroupInfo = CreateColor(249, 41, 0);
                // 阵营
                public static readonly Color Camps = Color.white;
                // 军团联盟
                public static readonly Color League = CreateColor(0xe2, 0x88, 0xff);
            }
        }

        public static class Name
        {
            public static /*readonly*/ Color Boss = CreateColor(239, 102, 212);

            public static class Npc
            {
                // 友好的NPC名字颜色
                public static /*readonly*/ Color Friend = CreateColor(51, 236, 62);
                //会主动攻击玩家的npc
                public static Color ActiveAttack = CreateColor(246, 152, 60);
                //会进行反击的npc
                public static Color AttackBack = CreateColor(255, 252, 0);
                //等级过高
                public static Color HighLevel = CreateColor(255, 35, 0);
                //等级过低
                public static Color LowLevel = CreateColor(164, 164, 164);
            }

            public static class Player
            {
                //友好
                public static /*readonly*/ Color Friend = CreateColor(51, 236, 62);
                // 可反击
                public static /*readonly*/ Color Attacker = CreateColor(180, 132, 71);
                //非敌非友
                public static /*readonly*/ Color None = Color.white;
                // 红名玩家
                public static /*readonly*/ Color Killer = CreateColor(255, 35, 0);
                //队友
                public static /*readonly*/ Color TeamMember = CreateColor(0, 168, 255);
                //VIP
                public static /*readonly*/ Color Vip = CreateColor(253, 207, 108);
            }

            // BOSS
        }
    }

    #endregion

    public static class CharCtrlStepOffset
    {
        public static readonly float Player = 0.8f;
        public static readonly float OtherPlayer = XYDefines.Player.JumpHeight;
        public static readonly float Monster = 1.6f;
    }

    // 速度（以下各值最终由服务器发送过来）

    public static class Player
    {
        public const float MaxClimbAngle = 45;
        // ****** 暂时放开Readonly，供调试效果用
        public static /*readonly*/ float JumpHeight = 1.5f;
        public static /*readonly*/ float Gravity = 9.8f;
    }

    public static class Role
    {
        public static class OffsetY
        {
            public static readonly float Title = 0.2f;
            public static readonly float ShopSign = Title + 0.3f;
            public static readonly float QuestPromptIcon = ShopSign + 0.2f;
            public static readonly float SpeechBubble = QuestPromptIcon;
        }
    }

    public static class Speed
    {
        private static float monsterNormal_ = 2.5f; // 怪物正常移动的速度

        public static float MonsterNormal
        {
            get { return monsterNormal_; }
        }

        public static void UpdateMonster(float normal)
        {
            monsterNormal_ = normal;
        }
    }

    #region Tag定义

    public static class Tag
    {
        public const string MainCamera = "MainCamera";
        public const string EventObject = "EventObject";
        public const string JiGuan = "jiguan";
        public const string Axis = "Axis";
        public const string Dirt = "dirt";
        public const string Water = "water";
        public const string Concrete = "concrete";
        public const string SunLight = "sunlight";
        public const string RoleLight = "rolelight";
        public const string AirWalls = "airwalls";
        public const string LoadingBackGround = "loadingBackground";
        public const string MainRoleShadow = "mainRoleShadow";
        public const string SelectedEffect = "selectedEffect";
        public const string SkyBox = "skybox";
        public const string LevelController = "levelController";
        public const string FallitemBag = "fallitembag";
        public const string SmokeEffect = "smokeEffect";
    }

    #endregion

    #region 层 Layer

    //public static class Layer
    //{
    //    public const int Default = 0;
    //    public const int IgnoreRaycast = 2;
    //    public const int Water = 4;
    //    public const int Player = 8; // 玩家自身
    //    public const int Face = 9; // 显示在FixedGUI里的玩家头像
    //    public const int FixedGui = 10; // 固定的GUI
    //    public const int Gui = 11; // 动态的GUI
    //    public const int SmallMap = 12; // 小地图
    //    public const int Terrain = 13; // 地形
    //    public const int QuestPromptIcon = 14;
    //    public const int Prop = 15; //小物件
    //    public const int MainBuilding = 16; //主体建筑
    //    public const int Tree = 17; //树木
    //    public const int Ding = 18; //建筑物顶部
    //    public const int Effect = 20; //火球类特效，原来是19，跟美术部场景里用的冲突了，改成20，陈俊，2012/6/8
    //    public const int Glow = 25; //辉光
    //    public const int Highlight = 26;
    //    public const int SkyBox = 31; //天空盒

    //    public static class Mask
    //    {
    //        public const int Default = 1 << Layer.Default;
    //        public const int Player = (1 << Layer.Player) | (1 << Layer.Glow);
    //        public const int Ignore = 1 << IgnoreRaycast;
    //        public const int Ding = 1 << Layer.Ding;
    //        public const int Terrain = 1 << Layer.Terrain;
    //        public const int MainBuilding = 1 << Layer.MainBuilding;
    //        public const int Water = 1 << Layer.Water;
    //        public const int Gui = 1 << Layer.Gui;
    //        public const int Glow = 1 << Layer.Glow;
    //        public const int HighLight = 1 << Highlight;
    //        public const int Prop = 1 << Layer.Prop;
    //        public const int Tree = 1 << Layer.Tree;
    //        public const int Effect = 1 << Layer.Effect;
    //    }
    //}
    public static class Layer
    {
        public const int Default = 0;
        public const int IgnoreRaycast = 2;
        public const int Water = 4;
        public const int UI = 5;
        public const int Player = 8;
        public const int MainPlayer = 9;
        public const int Effect = 10;
        public const int UIPlayer = 11;
        public const int Jump = 12;
        public const int Terrain = 13;
        public const int Fly = 14;
        public const int Prop = 15;
        public const int MainBuilding = 16;
        public const int MainCamera = 19;
        public const int TimelineUI = 20;
        public const int Probe = 29;
        public const int SkyBox = 31;

        public static class Mask
        {
            public const int Default = 1 << Layer.Default;
            public const int IgnoreRaycast = 1 << Layer.IgnoreRaycast;
            public const int Water = 1 << Layer.Water;
            public const int UI = 1 << Layer.UI;
            public const int Player = 1 << Layer.Player;
            public const int MainPlayer = 1 << Layer.MainPlayer;
            public const int Effect = 1 << Layer.Effect;
            public const int UIPlayer = 1 << Layer.UIPlayer;
            public const int Jump = 1 << Layer.Jump;
            public const int Terrain = 1 << Layer.Terrain;
            public const int Fly = 1 << Layer.Fly;
            public const int Prop = 1 << Layer.Prop;
            public const int MainBuilding = 1 << Layer.MainBuilding;
            public const int MainCamera = 1 << Layer.MainCamera;
            public const int TimelineUI = 1 << Layer.TimelineUI;
            public const int Probe = 1 << Layer.Probe;
            public const int SkyBox = 1 << Layer.SkyBox;
        }
    }
    #endregion

    #region  摄像机深度

    public static class CameraDepth
    {
        public static readonly float Main = 0;
        public static readonly float SmallMap = 5; // 显示小地图的摄像机
        public static readonly float Face = 8; // 显示玩家头像的摄像机
        public static readonly float FixedGui = 10; // 显示FixedGUI的摄像机
        public static readonly float Gui = 15; // 显示GUI的摄像机
    }

    #endregion
}

#region 右键菜单

public class RightMenu
{
    [EditorEnum("私聊")]
    public const byte PrivateChat = 1;

    [EditorEnum("邀请加入队伍")]
    public const byte InviteTeam = 2;

    [EditorEnum("申请进入队伍")]
    public const byte ApplyTeam = 3;

    [EditorEnum("移交队长")]
    public const byte GiveCaptain = 4;

    [EditorEnum("踢出队伍")]
    public const byte KickOutTeam = 5;

    [EditorEnum("邀请加入军团")]
    public const byte Army = 6;

    [EditorEnum("交易")]
    public const byte Trade = 7;

    [EditorEnum("单挑")]
    public const byte DuelInvite = 8;

    [EditorEnum("跟随")]
    public const byte Follow = 9;

    [EditorEnum("添加到联系人列表")]
    public const byte Freind = 10;

    [EditorEnum("从联系人列表删除")]
    public const byte DelFriend = 11;

    [EditorEnum("加入黑名单")]
    public const byte Mask = 12;

    [EditorEnum("解除黑名单屏蔽")]
    public const byte UnMask = 13;

    [EditorEnum("从仇人列表删除")]
    public const byte UnEnemy = 14;

    [EditorEnum("情侣")]
    public const byte Lover = 15;

    [EditorEnum("结拜")]
    public const byte Brother = 16;

    [EditorEnum("拜师")]
    public const byte Master = 17;

    [EditorEnum("收徒")]
    public const byte Apprentice = 18;

    [EditorEnum("查看对方资料")]
    public const byte ViewData = 19;

    [EditorEnum("复制名字")]
    public const byte CopyName = 20;

    [EditorEnum("复制军团名")]
    public const byte CopyGroupName = 21;

    [EditorEnum("举报用户")]
    public const byte ReportPlayer = 22;

    [EditorEnum("离开队伍")]
    public const byte LeaveTeam = 23;

    [EditorEnum("发送私聊窗口")]
    public const byte IMPrivateChat = 24;

    [EditorEnum("分配方式")]
    public const byte ChangeLootMode = 25;

    [EditorEnum("自由拾取")]
    public const byte FreeLootMode = 26;

    [EditorEnum("队伍分配")]
    public const byte TeamLootMode = 27;


    [EditorEnum("千里相寻")]
    public const byte Teleport = 30;
    [EditorEnum("解除师徒关系")]
    public const byte DelMaster = 31;
    [EditorEnum("解除结拜")]
    public const byte DelBrother = 32;
    [EditorEnum("解除情侣关系")]
    public const byte DelLover = 33;
    [EditorEnum("使用亲密度道具")]
    public const byte UseRSLoverItem = 34;
    [EditorEnum("使用功德值道具")]
    public const byte UseRSMasterItem = 35;


    public const int AllMenu = 0x7FFFFFFF;
}

#endregion

#region UI的一些常量

public class UIConst
{
    public const int FrameHeight = 8;
}

#endregion

public static class XYDirectory
{
    private static bool initialized_;

    private static string projectRoot_;

    private static string res_;

    private static string log_;

    static XYDirectory()
    {
        res_ = projectRoot_ = @".\";
        log_ = @".\log\";
    }

    public static string ProjectRoot
    {
        get { return projectRoot_; }
    }

    public static string Res
    {
        get { return res_; }
    }

    public static string Log
    {
        get { return log_; }
    }

    /// <summary>
    ///     如果是Unity环境，用这个来初始化
    /// </summary>

    public static void Init()
    {
        if (!initialized_)
        {
            initialized_ = true;
            string path = string.Empty;
            {
                var sb = new StringBuilder(Application.dataPath.Replace('/', '\\'), 512);
                for (int i = sb.Length - 2; i >= 0; --i)
                {
                    if (sb[i] == '\\')
                    {
                        sb.Remove(i + 1, sb.Length - i - 1);
                        break;
                    }
                }
                if (sb.Length > 0 && sb[sb.Length - 1] != '\\')
                {
                    sb.Append('\\');
                }
                projectRoot_ = sb.ToString();
                res_ = projectRoot_;
            }
        }
    }

    /// <summary>
    ///     如果不是Unity环境（比如ModuleBuilder等），用这个来初始化
    /// </summary>
    /// <param name="rootDir">根</param>
    public static void Init(string rootDir, string resDir, string logDir)
    {
        if (!initialized_)
        {
            initialized_ = true;
            projectRoot_ = AppendDirectoryChar(rootDir);
            res_ = AppendDirectoryChar(resDir);
            log_ = AppendDirectoryChar(logDir);
        }
    }

    public static string AppendDirectoryChar(string dir)
    {
        if (dir == null || dir.Length == 0)
        {
            return string.Empty;
        }
        else if (dir[dir.Length - 1] != '\\')
        {
            return dir + '\\';
        }
        else
        {
            return dir;
        }
    }
}
public static class EditorEnumAttribute_NameGetter<T>
{
    private static readonly Dictionary<int, string> dict_ = new Dictionary<int, string>();

    static EditorEnumAttribute_NameGetter()
    {
        Type t = typeof(T);
        FieldInfo[] fiList = t.GetFields();
        foreach (FieldInfo fi in fiList)
        {
            if (fi.FieldType == typeof(int) || fi.FieldType == typeof(short) || fi.FieldType == typeof(byte))
            {
                int value = Convert.ToInt32(fi.GetValue(null));
                var eea = Attribute.GetCustomAttribute(fi, typeof(EditorEnumAttribute)) as EditorEnumAttribute;
                if (eea != null && eea.Name != null)
                {
                    dict_[value] = eea.Name;
                }
            }
        }
    }

    public static string Execute(int v)
    {
        string s;
        if (dict_.TryGetValue(v, out s))
        {
            return s;
        }
        else
        {
            return string.Empty;
        }
    }
}

/// <summary>
///     杂类
/// </summary>
public static class XYMisc
{
    public enum ZeroStyle
    {
        EmptryString,
        ChineseNone,
        MinusSign,
        Zero
    }

    public static void Swap<T>(ref T t1, ref T t2)
    {
        T tmp = t1;
        t1 = t2;
        t2 = tmp;
    }


    // 将金银铜合并为整型
    public static int MoneyValue(int gold, int silver, int copper)
    {
        return copper + silver * Def.CUPRUM_SILVER + gold * Def.CUPRUM_GOLD;
    }

    // 将金钱拆分成金、银、铜
    public static void SplitMoney(int money, out int gold, out int silver, out int copper)
    {
        gold = Math.DivRem(money, Def.CUPRUM_GOLD, out silver);
        silver = Math.DivRem(silver, Def.CUPRUM_SILVER, out copper);
    }

    public static int[] SplitMoney(int money)
    {
        var result = new int[3];
        SplitMoney(money, out result[0], out result[1], out result[2]);
        return result;
    }

    // 用于摆摊处总价计算int型不够的情况
    public static void SplitMoney(long money, out long gold, out long silver, out long copper)
    {
        gold = Math.DivRem(money, Def.CUPRUM_GOLD, out silver);
        silver = Math.DivRem(silver, Def.CUPRUM_SILVER, out copper);
    }

    public static long[] SplitMoney(long money)
    {
        var result = new long[3];
        SplitMoney(money, out result[0], out result[1], out result[2]);
        return result;
    }

    public static string MoneyToStr(int money)
    {
        return MoneyToStr(money, ZeroStyle.ChineseNone);
    }

    // 将金钱转换成“X金X银X钱”的字串表示
    public static string MoneyToStr(int money, ZeroStyle zs)
    {
        int gold, silver, copper;
        if (money == 0)
        {
            switch (zs)
            {
                case ZeroStyle.EmptryString:
                    return string.Empty;
                case ZeroStyle.ChineseNone:
                    return "(无)";
                case ZeroStyle.MinusSign:
                    return "-";
                default:
                    return "0";
            }
        }
        SplitMoney(money, out gold, out silver, out copper);
        var sb = new StringBuilder(128);
        if (gold != 0)
        {
            sb.Append(gold.ToString());
            sb.Append("金");
        }
        if (silver != 0)
        {
            sb.Append(silver.ToString());
            sb.Append("银");
        }
        if (copper != 0)
        {
            sb.Append(copper.ToString());
            sb.Append("铜");
        }
        return sb.ToString();
    }

    private const string DLL_KERNEL32 = "Kernel32";
    [DllImport(DLL_KERNEL32, CharSet = CharSet.Unicode)]
    public static extern int WideCharToMultiByte(uint codePage, uint flags,
                                                 string wideChar, int cchWideChar,
                                                 IntPtr multiByteStr, int cbMultiByte,
                                                 IntPtr p1, IntPtr p2);


    public static string EncodeLocalFileURL(string filePath)
    {
        if (filePath.Length > 0)
        {
            int bytesCount = WideCharToMultiByte(0, 0, filePath, filePath.Length, IntPtr.Zero, 0, IntPtr.Zero,
                                                       IntPtr.Zero);
            if (bytesCount > 0)
            {
                IntPtr p = Marshal.AllocHGlobal(bytesCount);
                try
                {
                    WideCharToMultiByte(0, 0, filePath, filePath.Length, p, bytesCount, IntPtr.Zero, IntPtr.Zero);
                    var bytes = new byte[bytesCount];
                    Marshal.Copy(p, bytes, 0, bytesCount);
                    //
                    var sb = new StringBuilder("file:///", bytesCount * 3);
                    foreach (byte b in bytes)
                    {
                        if (b == '\\')
                        {
                            sb.Append('/');
                        }
                        else if (b <= ' ' || b > 127)
                        {
                            sb.AppendFormat("%{0:X02}", b);
                        }
                        else
                        {
                            sb.Append((char)b);
                        }
                    }
                    return sb.ToString();
                }
                finally
                {
                    Marshal.FreeHGlobal(p);
                }
            }
        }
        return string.Empty;
    }


    public static string AppendDirectorySeparatorChar(string dir)
    {
        if (dir != null && dir.Length > 0 && dir[dir.Length - 1] != Path.DirectorySeparatorChar)
        {
            return dir + Path.DirectorySeparatorChar;
        }
        else
        {
            return dir;
        }
    }

    public static string ReplaceDirectorySeparatorChar(string path)
    {
        if (path != null)
        {
            return path.Replace('/', Path.DirectorySeparatorChar);
        }
        else
        {
            return path;
        }
    }

    public static bool TryParsePosition(string text, out float x, out float y, out float z)
    {
        string[] xyz = text.Split(',');
        if (xyz.Length == 3)
        {
            if (float.TryParse(xyz[0].Trim(), out x))
            {
                if (float.TryParse(xyz[1].Trim(), out y))
                {
                    if (float.TryParse(xyz[2].Trim(), out z))
                    {
                        return true;
                    }
                }
            }
        }
        x = y = z = 0;
        return false;
    }

    public static float Get2DDistance(Vector3 v1, Vector3 v2)
    {
        v1.y = 0;
        v2.y = 0;
        return (v1 - v2).magnitude;
    }

    /// <summary>
    ///     判断两个表示方向的已归一化的二维向量是否接近（夹角小于某个阈值）
    /// </summary>
    /// <param name="x1"></param>
    /// <param name="z1"></param>
    /// <param name="x2"></param>
    /// <param name="z2"></param>
    public static bool IsDirectionClose(float x1, float z1, float x2, float z2)
    {
        if (x1 == 0 && z1 == 0)
        {
            return (x2 == 0 && z2 == 0);
        }
        if (x2 == 0 && z2 == 0)
        {
            return false;
        }
        else
        {
            return (x1 * x2 + z1 * z2) >= 0.9998f;
        }
    }

    public static string GetMapName(MapReference mr)
    {
        return (mr == null) ? "未知地图" : mr.Name;
    }
}

///////////////////////////////////////////////////


public static class XYTime
{
    private static readonly DateTime DATETIME_1970 = new DateTime(1970, 1, 1, 0, 0, 0);
    private static readonly long BEGIN_OF_1970;

    private static long deltaOfServerAndClient_; // 服务器时间与客户端时间的差值

    private static uint serverTick_;

    private static DateTime serverTime_ = DateTime.Now;

    static XYTime()
    {
        var csharp = new DateTime(1, 1, 1, 0, 0, 0);
        long delta = DATETIME_1970.Ticks - csharp.Ticks;
        BEGIN_OF_1970 = delta / (1000 * 1000 * 10);
    }

    #region 作为游戏标准的日期编排格式

    public static string StandardDateString(DateTime dt)
    {
        return dt.ToString("yyyy-MM-dd");
    }

    public static string StandardTimeString(DateTime dt)
    {
        return dt.ToString("HH:mm");
    }

    public static string StandardFullString(DateTime dt)
    {
        return dt.ToString("yyyy-MM-dd HH:mm");
    }

    #endregion

    public static long DeltaOfServerAndClient
    {
        get { return deltaOfServerAndClient_; }
    }

    public static uint ServerTick
    {
        get { return serverTick_; }
    }

    public static DateTime ServerTime
    {
        get { return serverTime_; }
    }

    public static long ClientUtcSeconds
    {
        get { return DateTime.UtcNow.Ticks / (1000 * 1000 * 10) - BEGIN_OF_1970; }
    }

    // UTC时间--自1970.1.1以来逝去的秒数（与C兼容）
    public static uint UtcSeconds
    {
        get
        {
            long result = ClientUtcSeconds + deltaOfServerAndClient_;
            if (result <= uint.MinValue)
            {
                return uint.MinValue;
            }
            else if (result >= uint.MaxValue)
            {
                return uint.MaxValue;
            }
            else
            {
                return (uint)result;
            }
        }
    }

    public static event Action<uint> OnEventServerNotifyTime;

    public static void OnServerNotifyTime(uint serverTick)
    {
        serverTick_ = serverTick;
        deltaOfServerAndClient_ = serverTick - ClientUtcSeconds;
        serverTime_ = SecondsToDateTime(serverTick);
        if (OnEventServerNotifyTime != null)
        {
            OnEventServerNotifyTime(serverTick);
        }
        //Debug.Log("Server notify time, delta = {0} seconds", deltaOfServerAndClient_);
    }

    public static string SecondsToDateString(uint seconds)
    {
        return StandardDateString(SecondsToDateTime(seconds));
    }

    public static string SecondsToFullString(uint seconds)
    {
        return StandardFullString(SecondsToDateTime(seconds));
        //.ToString( "yyyy-MM-dd HH:mm:ss" );陈俊修改，统一一下游戏里所有日期编排方式，所以把秒去掉了
    }

    public static string SecondsToTimeString(uint seconds)
    {
        return StandardTimeString(SecondsToDateTime(seconds));
    }

    public static DateTime SecondsToDateTime(uint seconds)
    {
        DateTime dt = DATETIME_1970;
        return dt.AddSeconds(seconds).ToLocalTime();
    }

    public static uint DateTimeToSeconds(DateTime dt)
    {
        TimeSpan delta = dt.ToUniversalTime() - DATETIME_1970;
        return (uint)delta.TotalSeconds;
    }
}

public static class XYStackTrace
{
    public static string GetInfo()
    {
        return GetInfo(1);
    }

    public static string GetInfo(int skipStackFrams)
    {
        var st = new StackTrace(skipStackFrams, true);
        StackFrame[] sfs = st.GetFrames();
        var sb = new StringBuilder(1024);
        foreach (StackFrame sf in sfs)
        {
            string filename = sf.GetFileName();
            if (filename == null || filename.Length == 0)
            {
                break;
            }
            MethodBase mb = sf.GetMethod();
            sb.AppendFormat("\tFunc: {0}\r\n\tLine: {1}\r\n\tFile: {2}\r\n\r\n",
                            (mb == null) ? "(Unknown)" : mb.Name,
                            sf.GetFileLineNumber().ToString(),
                            filename);
        }
        return sb.ToString();
    }
}

/// <summary>
///     常用算法
/// </summary>
public static class XYAlgorithm
{
    public delegate int Compare<T, U>(T t, U u);

    /// <summary>
    ///     在一个升序容器里，查找是否有这样一个Item，它满足：
    ///     该Item“大于或等于”给定值，
    ///     且Item是所有满足这个条件中的第一个
    /// </summary>
    /// <typeparam name="T">容器条目类型</typeparam>
    /// <typeparam name="U">给定值（要查找的值）的类型</typeparam>
    /// <param name="container">容器</param>
    /// <param name="first">起始位置</param>
    /// <param name="last">结束位置</param>
    /// <param name="u">检索关键值</param>
    /// <param name="compare">比较方法</param>
    /// <returns>成功找到返回非负索引，未找到返回last</returns>
    public static int LowerBound<T, U>(IList<T> container, int first, int last, U u, Compare<T, U> compare)
    {
        int count = last - first;
        while (count > 0)
        {
            int count2 = count >> 1;
            int middle = first + count2;
            if (compare(container[middle], u) < 0)
            {
                first = ++middle;
                count -= count2 + 1;
            }
            else
            {
                count = count2;
            }
        }
        return first;
    }

    public static int LowerBound<T, U>(IList<T> container, U u, Compare<T, U> compare)
    {
        return LowerBound(container, 0, container.Count, u, compare);
    }

    /// <summary>
    ///     在一个升序容器里，查找是否有这样一个Item，它满足：
    ///     该Item“大于”给定值，
    ///     且Item是所有满足这个条件中的第一个
    /// </summary>
    /// <typeparam name="T">容器条目类型</typeparam>
    /// <typeparam name="U">给定值（要查找的值）的类型</typeparam>
    /// <param name="container">容器</param>
    /// <param name="first">起始位置</param>
    /// <param name="last">结束位置</param>
    /// <param name="u">检索关键值</param>
    /// <param name="compare">比较方法</param>
    /// <returns>成功找到返回非负索引，未找到返回last</returns>
    public static int UpperBound<T, U>(IList<T> container, int first, int last, U u, Compare<T, U> compare)
    {
        int count = last - first;
        while (count > 0)
        {
            int count2 = count >> 1;
            int middle = first + count2;
            if (compare(container[middle], u) <= 0)
            {
                first = ++middle;
                count -= count2 + 1;
            }
            else
            {
                count = count2;
            }
        }
        return first;
    }

    public static int UpperBound<T, U>(IList<T> container, U u, Compare<T, U> compare)
    {
        return UpperBound(container, 0, container.Count, u, compare);
    }

    // 在一个升序容器里，用给定值u进行查找
    // 返回一对索引值在lower和upper里
    //		lower = LowerBound(), upper = UpperBound()
    public static void EqualBound<T, U>(
        IList<T> container, int first, int last, U u, Compare<T, U> compare,
        out int lower, out int upper
        )
    {
        int count = last - first;
        while (count > 0)
        {
            int count2 = count / 2;
            int middle = first + count2;
            int c = compare(container[middle], u);
            if (c < 0)
            {
                first = ++middle;
                count -= count2 + 1;
            }
            else if (c > 0)
            {
                count = count2;
            }
            else
            {
                lower = LowerBound(container, first, middle, u, compare) + count;
                upper = UpperBound(container, ++middle, first, u, compare);
                return;
            }
        }
        lower = upper = first;
    }

    // 堆
    public class Heap<T> where T : IComparable<T>
    {
        public static void HeapSort(T[] objects)
        {
            for (int i = objects.Length / 2 - 1; i >= 0; --i)
                heapAdjustFromTop(objects, i, objects.Length);
            for (int i = objects.Length - 1; i > 0; --i)
            {
                swap(objects, i, 0);
                heapAdjustFromTop(objects, 0, i);
            }
        }

        public static void heapAdjustFromBottom(T[] objects, int n)
        {
            while (n > 0 && objects[(n - 1) >> 1].CompareTo(objects[n]) < 0)
            {
                swap(objects, n, (n - 1) >> 1);
                n = (n - 1) >> 1;
            }
        }

        public static void heapAdjustFromTop(T[] objects, int n, int len)
        {
            while ((n << 1) + 1 < len)
            {
                int m = (n << 1) + 1;
                if (m + 1 < len && objects[m].CompareTo(objects[m + 1]) < 0)
                    ++m;
                if (objects[n].CompareTo(objects[m]) > 0)
                    return;
                swap(objects, n, m);
                n = m;
            }
        }

        private static void swap(T[] objects, int a, int b)
        {
            T tmp = objects[a];
            objects[a] = objects[b];
            objects[b] = tmp;
        }
    }

    // 优先队列
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private const int defaultCapacity = 16;
        private T[] buffer;
        private int heapLength;

        public PriorityQueue()
        {
            buffer = new T[defaultCapacity];
            heapLength = 0;
        }

        public bool Empty()
        {
            return heapLength == 0;
        }

        public T Top()
        {
            if (heapLength == 0)
            {
                throw new OverflowException();
            }
            return buffer[0];
        }

        public void Push(T obj)
        {
            if (heapLength == buffer.Length)
                expand();
            buffer[heapLength] = obj;
            Heap<T>.heapAdjustFromBottom(buffer, heapLength);
            heapLength++;
        }

        public void Pop()
        {
            if (heapLength == 0)
                throw new OverflowException();
            --heapLength;
            swap(0, heapLength);
            Heap<T>.heapAdjustFromTop(buffer, 0, heapLength);
        }

        private void expand()
        {
            Array.Resize(ref buffer, buffer.Length * 2);
        }

        private void swap(int a, int b)
        {
            T tmp = buffer[a];
            buffer[a] = buffer[b];
            buffer[b] = tmp;
        }
    }
}

/// <summary>
///     整数转成中文描述字串
/// </summary>
internal class XYIntegerToChinese
{
    private static readonly char[] NUM_1 = new[] { '零', '一', '二', '三', '四', '五', '六', '七', '八', '九' };
    private static readonly char[] NUM_2 = new[] { '零', '壹', '贰', '叁', '肆', '伍', '陆', '柒', '捌', '玖' };
    private static readonly char[] UNIT_1 = new[] { '十', '百', '千', '万', '十', '百', '千', '亿', '十', '百' };
    private static readonly char[] UNIT_2 = new[] { '拾', '佰', '仟', '万', '拾', '佰', '仟', '亿', '拾', '佰' };
    public static readonly XYIntegerToChinese General = new XYIntegerToChinese(false, true);
    private readonly bool adjustTen_;

    private readonly char[] numbers_;
    private readonly char[] units_;

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="upper">为TRUE表示用“壹贰叁……”代表“一二三……”</param>
    /// <param name="adjustTen">为TRUE表示类似于10和100000这样的值，转换为“十”和“十万”而不是“一十”和“一十万”</param>
    public XYIntegerToChinese(bool upper, bool adjustTen)
    {
        if (upper)
        {
            numbers_ = NUM_2;
            units_ = UNIT_2;
        }
        else
        {
            numbers_ = NUM_1;
            units_ = UNIT_1;
        }
        adjustTen_ = adjustTen;
    }

    /// <summary>
    ///     辅助函数，将正整数按个十百千位分开到数组中
    /// </summary>
    /// <param name="value">正整数值</param>
    /// <returns>整型数组，按个十百千位反序排列</returns>
    private static IList<int> Split(int value)
    {
        var result = new List<int>(12);
        while (value != 0)
        {
            int remainder;
            value = Math.DivRem(value, 10, out remainder);
            result.Add(remainder);
        }
        return result;
    }

    /// <summary>
    ///     将数值转换为中文表达
    /// </summary>
    /// <param name="value">要转换的整数值</param>
    /// <returns>中文表达文本</returns>
    public string Execute(int value)
    {
        var sb = new StringBuilder(64);
        // 如果是负数则令其为正，返回串前缀“负”
        if (value < 0)
        {
            value = -value;
            sb.Append('负');
        }
        // 如果不大于9，则直接返回单个字符
        if (value <= 9)
        {
            sb.Append(numbers_[value]);
            return sb.ToString();
        }
        // 处理大于9的数值
        IList<int> splited = Split(value); // 将数值按“个十百千……”位分开
        int unit = splited.Count - 2; // “单位”数组的下标
        bool lastIsZero = false; // 上一位数值是零吗？
        for (int i = splited.Count - 1; i >= 0; --i)
        {
            int num = splited[i];
            // 如果数值为0，则不需要转换为“零”字符，且：
            //		若当前位是“亿”或“万”时，加上当前位的“单位”
            //		（注，须避免“亿”和“万”两个单位相连）
            if (num == 0)
            {
                if (unit == 7 || (unit == 3 && sb[sb.Length - 1] != '亿'))
                {
                    sb.Append(units_[unit]);
                }
                lastIsZero = true;
            }
            // 如果当前位数值不为0，则判断上一位是否为0
            //		若是，则需把上一位的0转换为“零”字符
            else
            {
                if (lastIsZero)
                {
                    sb.Append('零');
                    lastIsZero = false;
                }
                sb.Append(numbers_[num]);
                if (unit >= 0)
                {
                    sb.Append(units_[unit]);
                }
            }
            --unit;
        }
        //
        if (adjustTen_ && sb.Length >= 2 && sb[0] == '一' && sb[1] == '十')
        {
            sb.Remove(0, 1);
        }
        return sb.ToString();
    }
}

public class XYBitWriter : IDisposable
{
    private byte mask_ = 0x80;
    private Stream stream_;
    private byte value_;

    public XYBitWriter(Stream output)
    {
        stream_ = output;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Write(bool bit)
    {
        if (bit)
        {
            value_ |= mask_;
        }
        mask_ >>= 1;
        if (mask_ == 0)
        {
            stream_.WriteByte(value_);
            value_ = 0;
            mask_ = 0x80;
        }
    }

    public void Flush()
    {
        if (mask_ != 0x80)
        {
            stream_.WriteByte(value_);
            mask_ = 0x80;
            value_ = 0;
        }
        stream_.Flush();
    }

    protected virtual void Dispose(bool isDisposing)
    {
        Flush();
        stream_ = null;
    }

    ~XYBitWriter()
    {
        Dispose(false);
    }
}

internal class BitReader9Z
{
    private readonly Stream stream_;
    private byte mask_;
    private byte value_;

    public BitReader9Z(Stream input)
    {
        stream_ = input;
    }

    public bool Read()
    {
        if (mask_ == 0)
        {
            mask_ = 0x80;
            int v = stream_.ReadByte();
            if (v == -1)
            {
                throw new EndOfStreamException();
            }
            value_ = (byte)(uint)v;
        }
        bool result = (value_ & mask_) != 0;
        mask_ >>= 1;
        return result;
    }
}


#region 测试对象的释放

internal static class ObjCounter<T>
{
    private static int count_;

    [Conditional("UNITY_EDITOR")]
    public static void Increment(object obj)
    {
        int i = Interlocked.Increment(ref count_);
        if (i <= 5)
        {
            Debug.Log(string.Format("\tCreate {0}: count = {1}", obj.GetType(), i));
        }
    }

    [Conditional("UNITY_EDITOR")]
    public static void Decrement(object obj)
    {
        int i = Interlocked.Decrement(ref count_);
        if (i <= 5)
        {
            Debug.Log(string.Format("\tDestroy {0}: count = {1}", obj.GetType(), i));
        }
    }
}

#endregion
