using System;

public struct Def {
    public const int DB_VER = 7;                //DB的记录的version,用于判断数据版本和服务器是否一至
    public const int INVALID_ID = -1;               //无效ID
    public const int ROLE_ACTIVE_TICK = 1;                //每秒活动帧数

    public const short MAX_RATE = 10000;
    public const float MAX_RATE_F = 10000F;
    public const int MAX_DB_BLOB = 32000;
    public const int SYNC_BUFFER_LEN = 4 * 1024;
    public static int MAX_SYNC_PLAYER = 50;

    public const int BORN_MAP = 1;                //出生地图的ID,桃源村

    //某些字串的最大长度，以字节计，汉字占2字节
    public static class MaxLength {
        public const int Account = 20;
        public const int RoleName = 14;
        public const int Password = 20;
        public const int MailTitle = 50;
        public const int MailContent = 800;
        public const int GroupName = 16;
    }

    public static class MaxChars {
        public const int GuildNotice = 50;
        public const int CountryNotice = 50;
        public const int LoverManifestor = 50;
    }

    public const int MAX_OBJ_SN = 1000000;          //最大的非玩家OBJ SN
    public const int MAX_MAP_STATE = 30;               //最大地图状态
    public const int MAX_PERFORMANCE = 20;               //一个地图上最多同时进行的情节表演

    public const int MAX_ACCOUNT_ROLE = 5;                //每个账号最多可以有的角色数量
    public const int PING_INTERVAL = 30;                  //ping的间隔时间
    public const int PING_DB_INTERVAL = 300;              //PingDB的时间间隔 

    public const int MAX_PROTO_SIZE = 64 * 1024;

    public const int MAX_OCCUPATION = 4;                //职业数量
    public const int MAX_LEVEL = 200;              //最大等级
    public const int MAX_LEVEL_LOCKED = 70;         // 本阶段只开放到这个级别
    public const int MIN_PLAYER_LEVEL_CAN_EXCHANGE_OFFLINE_EXP = 15;    // 多少级的玩家可兑换离线经验
    public const int MIN_PLAYER_LEVEL_CAN_CREATE_GUILD = 30;    // 多少级的玩家可创建军团

    // 多少级的玩家可以接受军团推荐？
    public const int MIN_PLAYER_LEVEL_CAN_RECOMMEND_GUILD = 16;
    public const int MAX_PLAYER_LEVEL_CAN_RECOMMEND_GUILD = 39;

    // 自动推荐军团时间在哪两个值之间？
    public const int LOW_SECONDS_RECOMMEND_GUILD = 5400;
    public const int HIGH_SECONDS_RECOMMEND_GUILD = 9000;

    public const int MAX_EXP = int.MaxValue;        // 最大经验值

    public const int PROCESS_MSEC_INTERVAL = 100;
    public const int MAX_REF_ID = 32000;
    public const int MAX_EFFECT = 256;
    public const int MAX_SHORTCUT = 24;

    public const int MAX_QUEST = 10000;            //整个游戏最大任务数量

    public const int MAX_PLAYER_QUEST = 20;         //单个玩家最大任务数量
    public const int MAX_PLAYER_QUEST_SYS = 8;      //单个玩家系统分配的任务数（总数量为 20 + 8）

    //金钱信息
    public const int MAX_ROLE_MONEY = 400000000;      // 最大金钱数量
    public const int MAX_STORE_MONEY = 999999999;     // 仓库最大金钱数量 
    public const int CUPRUM_SILVER = 100;             // 铜到银的转换 
    public const int CUPRUM_GOLD = 10000;             // 铜到金的转换 

    public const int MIN_STORE_LEVEL = 1;
    public const int MAX_SCREEN_LEN = 15;             // 当前频道聊天范围

    public const int ONE_HUNDRED = 100;

    //防沉迷相关
    public const int ANTI_ADDICTION_CLEAR_TIME  = 5 * 3600; //下线5小时后清空防沉迷信息
    public const int ADD_ONLINE_SECOND =  60;               //每分钟添加一次在线时间
    public const int ANTI_ADDICTION_INCOME_HALF_TIME = 3 * 3600;
    public const int ANTI_ADDICTION_INCOME_ZERO_TIME = 5 * 3600;

    //邮件相关
#if GM
    public static int MAIL_MAX_COUNT_CAN_VIEW = 100;     // 玩家最多可以看到多少封邮件？
    public static uint MAIL_EXPIRE_SECONDES = 3600 * 24 * 30;        // 多少秒以前的邮件视为过期邮件？
#else
    public const int MAIL_MAX_COUNT_CAN_VIEW = 100;     // 玩家最多可以看到多少封邮件？
    public const uint MAIL_EXPIRE_SECONDES = 3600 * 24 * 30;        // 多少秒以前的邮件视为过期邮件？
#endif

    //道具相关
    public const int MAX_DROP_ITEM = 8;
    public const int MAX_NPC_ITEM = 3;
    public const int DEFAULT_PLAYER_ITEM = 60;             //默认玩家背包格子数
    public const int MAX_PLAYER_ITEM = 196;                //最大玩家背包格子数
    public const int DEFAULT_PLAYER_STOREHOUSE = 53;       //默认玩家仓库格子数
    public const int MAX_PLAYER_STOREHOUSE = 224;          //最大玩家仓库格子数
    public const int MAX_ITEM_SKILL = 12;                  //最大使用道具提高技能等级的次数
    public const int MAX_ITEM_TALENT = 5;                  //最大使用道具提高技能总天赋点数
    public const uint MAX_TIME_OUT = 0xFFFFFFFF;           //AVARTA过期时间
    public const int MAX_IBSTORE_MUN = 18;                 //IB商城一次够道具的最在数量
    public const int MAX_RETURN_POS = 10;                  //车马令最大位置

    //技能相关
    public const int MAX_BUFF = 16;
    public const int MAX_PLAYER_SKILL = 30;
    public const int MAX_PASSIVE_SKILL = 14;
    public const int MAX_PASSIVE_SKILL_LV = 20;             //心法最大等级  
    public const int MAX_NPC_RANDOM_MOVE = 5;
    public const int MAX_SKILL_DISTANCE = 30;             //最大技能的距离
    public const int MAX_TARGET_LIMIT = 1024;
    public const int SKILL_NORMAL_ATTACK = 0;              //普通攻击的技能INDEX
    public const int SKILL_PUBLIC_CD = 1000;           //技能公共CD(毫秒)
    public const int SKILL_FLY_SPEED = 15;             //技能(光球)飞行速度(米)
    public const int SUMMON_OBJ_TIMEOUT = 2 * 60;         //召唤出来的对象/怪的存在时间

    //生产技能
    public const short MAX_PRODUCT_SKILL_LEVEL = 400;        //最大生产技能等级
    public const short MAX_COMMON_PRODUCT_SKILL_LEVEL = 75;  //最大通用合成技能等级
    public const short MAX_COLLECT_SKILL_LEVEL = 1000;
    public const int MIN_LEARN_LIFE_SKILL_LV = 10;           //学习生活技能的最小等级

    public const int LEAVE_FIGHT_TIME = 10;              //离开战斗状态的时间
    public const int SAVE_TIME = 5 * 60;                 //每5分钟保存一次
    public const int REDIS_SAVE_TIME = 5;                //redis每5秒保存一次玩家

    //镖车相关
    public const int CARRIAGE_VALID_DISTANCE = 15;           //多少距离内,运镖有效

    //角色
    public const int MAX_HP = 200000000;
    public const short MAX_MP = 30000;
    public const short MAX_ATTR = 30000;             //最大的属性数值
    public const short MAX_RAGE_STAR = 5;            //最大怒气星
    public const int RAGE_STAR_VALUE = 10000;        //单个怒气星需要多少怒气
    public const int MAX_SPEED = 10;                 //最大速度10
    public const int MIN_ATT_SPEED = 300;            //最小攻击速度
    public const int MAX_VIGOURPOINT = 20000000;     //最大活力
    public const int REFRESH_VIGOURINTERVAL = 5 * 60;  //活力刷新时间间隔
    public const int RELIVE_INTERVAL_TIME = 5;       //重生间隔时间
    public const int DEAD_BODY_SHOW_TIME = 2;        //< 尸体显示时间
    public const short MAX_CRIT_MULTI = 1000;        //< 最大伤害倍率

    //怒气相关(从MISC中读取,这上面的值只是默认值)
    public static int RAGE_VALUE = 65;             //副本上,怒气增加倍率
    public static int PVP_RAGE_VALUE = 125;            //PVP的怒气增加倍率

    public const int MAP_KILL_POINT_DEC_TIME = 20 * 60;      //普通地图减恶名值的时间
    public const int JAIL_KILL_POINT_DEC_TIME = 5 * 60;      //监狱地图减恶名值的时间

    public const int MAX_DUNGEON_ROOM = 100;            //最大副本地图房间
    public const int MAX_DUNGEON_PLAYER = 20;             //副本中最大玩家数

    //PVP相关
    public const int PVP_PROTECT_LEVEL = 15;             //保护模式的等级
    public const int PVP_DUEL_LEVEL = 15;             //能够决斗的等级
    public const int RED_NAME_KILL_POINT = 10;             //达到红名的恶名值
    public const int GOTO_JAIL_KILL_POINT = 20;             //需要放在监狱的恶名值

    public const int MAP_OWNER_CAMP_WAR_LEVEL = 45;    //地图阵营战算军团贡献的等级
    public const int MAP_OWNER_CAMP_WAR_POINT = 2;

    //一些hard code的REF ID
    public const int JAIL_MAP_REF = 17;                //监狱地图的ID
    public const int DUEL_END_BUFF_REF = 376;          //决斗结束时的保护BUFF
    public const int MAP_PROTECT_BUFF_REF = 375;       //切换地图时的保护BUFF
    public const int REMOTE_BUY_ITEM_NPC_REF = 2644;   //远程买物品的NPC
    public const int GROUP_MAP_REF = 61;               //军团地图
    public const int BATTLEFIELD_ESCAPE_BUFF_REF = 466;//战场逃跑buff
    public const int PVE_PROTECT_BUFF_REF = 467;       //pve服务器保护buff
    public const int INCOME_HALF_BUFF_REF = 636;       //防沉迷收益减半buff
    public const int INCOME_ZERO_BUFF_REF = 637;       //防沉迷无收益buff
    public const int NPC_ESCAPE_BUFF_REF = 198;        //npc 逃跑buff

    //军团相关
    public const int MAX_GROUP_LEVEL = 5;
    public const int CREATE_GROUP_LEVEL = 30;
    public const int JOIN_GROUP_LEVEL = 10;
    public const int MAX_GROUP_VICE_LEADER = 2;
    public const int MAX_GROUP_MANAGER = 3;
    public const int GROUP_SAVE_TIME = 5 * 60;            //改变了的情况下,每5分钟存一下军团
    public const int MAX_GROUP_WAR = 10;
    public const int MAX_GROUP_WAR_DAY = 7;
    public const int MAX_GROUP_VIGOUR = 2000000000;

    public const int MIN_GROUP_LEAGUE_WAR_LEVEL = 40;

    public const int MAX_GROUP_LEAGUE_MEMBER = 30;

    //国战相关
    public const int MAX_APPLY_COUNTRY_WAR_HOUR = 19;
    public const int MIN_COUNTRY_WAR_PLAYER_LEVEL = 30;
    public const int MAX_DEFEND_POINT = 9;
    public const int FAIL_EXP = 10000;
    public const int WIN_EXP = 20000;

    //角色特殊状态
    public const int STATE_MOVE = 0x00000001;     //移动的状态,走,跑
    public const int STATE_JUMP = 0x00000002;     //跳 (暂时不用)
    public const int STATE_CAST = 0x00000004;     //施放技能
    public const int STATE_CAST_EX = 0x00000008;     //不能被打断的施放

    public const int STATE_FIGHT = 0x00000010;     //是否在战斗状态中
    public const int STATE_DIE = 0x00000020;     //死亡状态
    public const int STATE_MAGE_SHIELD = 0x00000040;     //有法术盾的状态
    public const int STATE_NPC_RETURN = 0x00000080;     //NPC离开战斗状态
    public const int STATE_SIT_DOWN = 0x00000100;     //坐下状态
    public const int STATE_DUEL = 0x00000200;     //决斗状态
    public const int STATE_MOCK = 0x00000400;     //被嘲讽技能吸引 (暂时不用)
    public const int STATE_HORSE = 0x00000800;     //骑马的状态

    public const int RESTRICT_MOVE = 0x00001000;        //< 禁止移动状态
    public const int RESTRICT_SKILL = 0x00002000;       //< 禁止使用技能状态
    public const int RESTRICT_ITEM = 0x00004000;        //< 禁止使用道具状态
    public const int RESTRICT_ATTACK = 0x00008000;      //< 禁止攻击状态

    public const int IMMU_RESTRICT_MOVE = 0x00010000;   //<免疫禁止移动
    public const int IMMU_RESTRICT_SKILL = 0x00020000;  //<免疫禁止使用技能
    public const int IMMU_RESTRICT_ITEM = 0x00040000;   //<免疫禁止使用道具
    public const int IMMU_RESTRICT_ATTACK = 0x00080000; //<免疫禁止攻击
    public const int IMMU_DAMAGE = 0x00100000;     //不受伤害
    public const int STATE_PROTECT = 0x00200000;     //保护模式

    public const int MAX_OPEN_DISTANCE = 3;
    public const int NPC_CHAT_DISTANCE = 8;
    public const int PICKUP_ITEM_DISTANCE = 6;
    public const int MAX_ROLL_TIME = 60;
    public const int DROP_ITEM_TIMEOUT = 90;    //东西拾取的超时时间
    public const int BOSS_ITEM_TIMEOUT = 5 * 60;


    //DB相关定义
    public const int SUCCESS = 0;    // 成功
    public const int ERROR = 1;    // 失败
    public const int DB_ERROR = 2;    // DB错误
    public const int DB_NAME_REPEAT = 3;    // 名字重复
    public const int DB_NAME_BADWORD = 4;    // 名字非法
    public const int DB_REMOVE_GROUP_LEADER = 5;    // 删除的角色是军团长

    public const int GLOBAL_TABLE_RANK = 0;         //排行榜在global表中的ID
    public const int GLOBAL_TABLE_WORLD_OWNER = 1;  //地图拥有权在global表中的ID

    //P2P交易相关定义
    public const int MAX_P2PTRADE_DISTANCE = 7;     // 最大P2P交易距离
    public const int MIN_P2PTRADE_INTERVAL = 10;    // P2P发起交易最小时间间隔
    public const int MAX_P2PTRADE_ITEMNUM = 12;    // 最大P2P交易数量  

    //队伍相关定义
    public const int MAX_TEAM_MEMBER_TOTAL = 5;    // 最大队伍成员个数
    public const int MAX_TEAM_MEMBER_NUM = MAX_TEAM_MEMBER_TOTAL - 1;
    public const int MAX_TEAM_EXP_DISTANCE = 30;   // 分配经验的组队范围
    public const int MAX_TEAMMEM_RSV_TIME = 360;  // 队员离线之后的保留时间
    public const int UPDATE_TEAM_INTERVAL = 2;    // 每个须更新的队伍的更新间隔时间
    public const int UPDATE_MEMBER_INFO_INTERVAL = 10; //每10秒更新一次队伍中成员信息
    public const int MIN_DICE_POINTS = 1;    // 最小股子点数
    public const int MAX_DICE_POINTS = 100;   // 最大股子点数

    //装备相关的常量
    public const int MAX_EFFECTLIST_LEN = 7;    // 最大备选效果个数
    public const int MAX_RANDOM_EFFECT_NUM = 5;    // 最大效果个数，根据xx三国需求装备有3-5个随机属性
    public const int MAX_HOLES_NUM = 10;   // 最大毛坯孔个数
    public const int MAX_ENCHASE_NUM = 7;    // 最大镶嵌物品个数
    public const int MAX_RECAST_NUM = 7;    // 最大改造次数
    public const int MAX_STARS_LEVEL = 10;    // 最大星级数
    public const int MAX_STARS_STONE_NUM = 4;    // 最大升星石头个数
    public const int MAX_HERO_MATERIAL_NUM = 8;	// 
    public const int MIN_EQUIP_ENDURE = 10;

    //摊位相关宏
    public struct Stall {
        public const float ROLE_BEHIND_STALL = 1.5f;	// 摆摊成功后角色站在摊位后面哪个位置？
        public const int MAX_TRADE_LOG = 100;           // 交易记录保留多少条？
    }

    //public const int MIN_STALL_OP_LEVEL     = 15;   // 摊位最小操作等级
    //public const int MAX_STALL_CELLS        = 10;    // 大摊位格子数
    //public const int MIN_STALL_CELLS        = 3;    // 小摊位格子数
    public const int MAX_STALL_NAME_LENGTH = 14;	// 最大摊位名称字符数
    public const int MAX_STALL_SHOPSIGN_LENGTH = 48;	// 最大摊位吆喝语字符数

    // 给定的摆摊时间合法吗？
    public static bool IsValidStallTime(int hours) {
        return Array.IndexOf<int>( VALID_STALL_TIME, hours ) >= 0;
    }

    public static readonly int[] VALID_STALL_TIME = new int[] {
        2, 4, 6, 8, 12, 14, 16, 18, 20, 24
    };

    //关系相关的宏
    public const int THREE_YEARS_SECONDS = 94608000;
    public const int ONE_DAY_SECONDS = 86400;    // 一天的秒数

    #region 废弃不用
    //public const int MAX_VIP_MEMBERS_NUM = 100;      // 会员的最大好友个数
    //public const int MAX_ORD_MEMBERS_NUM    = 50;       // 普通玩家的最大好友个数
    //public const int MAX_BLACK_NUM = 10;       //最大黑名单的数量
    //public const int MAX_ENEMY_NUM = 20;       //最大仇敌的数量
    //public const int MAX_BROTHER_REL_NUM    = 3;        // 最大结拜个数
    //public const int MAX_LOVERS_NUM         = 1;        // 最大异性知己个数
    //public const int MAX_APPRENTICE_NUM     = 3;       // 最多只能是3个徒弟(废弃不用了 YHB 2008.8.18)
    #endregion

    public const int MAX_REL_DISTANCE = 30;       // 距离

    //public const int MIN_BROTHER_LEVEL      = 40;       // 结拜的最低等级
    //public const int MIN_MASTER_LEVEL       = 40;       // 师傅最低等级
    public const int MIN_APPRENTICE_LEVEL = 1;       // 徒弟最低等级
    //public const int MAX_APPRENTICE_LEVEL   = 30;       // 徒弟最高等级
    //public const int MIN_LOVERS_LEVEL       = 20;       // 异性知己的最低等级
    //public const int MIN_SPOUSE_LEVEL       = 35;       // 夫妻最低等级

    public static class VipType {
        public const byte Normal = 0;       // 普通会员
        public const byte Year = 1;         // 年会
    }

    // 账号服务器返回的一些状态
    public static class AccountRsState {
        public const byte Ok = 0;
        public const byte NotFind = 1;  // 没有找到
        public const byte PswError = 2;  // 密码有误
        public const byte AmountNotEnough = 3;  // 余额不足
        public const byte AccountOnline = 4;   // 帐号在线
        public const byte Ing = 5;   // 正在验证
        public const byte Forbid = 6;   // 被禁号
        public const byte Busy = 7;   // 服务器繁忙，出错
        public const byte Inactive = 8;   // 账号没有被激活
        public const byte TimeOut = 9;
        public const byte ServerError = 10; // 服务器出错
        public const byte Version = 11; // 版本不对
        public const byte AcceptProtocol = 12; // 还没有同意Login协议
        public const byte illegal = 13; // 非法用户
        public const byte MysqlError = 14;
    }

    public static class GameServerState {
        public const int GS_Null = 0;    // 需要验证信息
        public const int GS_Check = 1;    // 需要验证信息
        public const int GS_CheckIng = 2;    // 正在验证信息
        public const int GS_Ok = 3;    // 正常运行中
        public const int GS_Error = 4;    // 错误，马上停止
    }

    // 角色重连相关，断开连接的类型
    public const sbyte Role_disconnect = 0;    // 正常连接
    public const sbyte Role_disconnect_Account = 1;    // 断开一个帐号，完全Logout
    public const sbyte Role_disconnect_Role = 2;       // 只是断开一个角色，非完全断开
    public const sbyte Role_disconnect_Replace = 3;    
}

public enum AccountType {
    no_type     = 0x0000,
    game9z      = 0x0001,
    tw          = 0x0002,
    c360        = 0x0004,
    kingsoft    = 0x0008,
    lyto        = 0x0010, 
    c360Box     = 0x0020,
    kankan      = 0x0040,
}

public enum BagType {
    BAG,
    EQUIP,
    STOREHOUSE,
}

public class PropertyType {
    public const int APPRID = 1;
    public const int LEVEL = 2;
    public const int EXP = 3;
    public const int MONEY = 4;
    public const int CAMP = 5;
    public const int MOVEDATA = 6;

    public const int STATE = 7;
    public const int MUTEX_STATE = 8;
    public const int MP_CHANGE = 9;
    public const int EXP_CHANGE = 10;

    // 角色(玩家、NPC)战斗属性
    public const int HP = 11;
    public const int MAX_HP = 12;
    public const int HP_RET = 13;
    public const int NORMAL_ATK = 14;
    public const int NORMAL_DEF = 15;
    public const int WALK_SPEED = 16;
    public const int ATK_SPEED = 17;
    public const int HIT_VALUE = 18;
    public const int DODGE_VALUE = 19;
    public const int CRIT_VALUE = 20;
    public const int CRIT_AVOID_VALUE = 21;
    public const int CRIT_MULTI = 22;
    public const int WIND_ATK = 23;
    public const int FIRE_ATK = 24;
    public const int THUNDER_ATK = 25;
    public const int WIND_DEF = 26;
    public const int FIRE_DEF = 27;
    public const int THUNDER_DEF = 28;
    public const int WIND_HURT = 29;
    public const int FIRE_HURT = 30;
    public const int THUNDER_HURT = 31;
    public const int NORMAL_HURT = 32;

    public const int MAX_PROPERTY = 41;
}

//游戏中的系统互斥,只针对玩家操作
public class GameMutex {
    public const int SKILL = 0x00000001;

    public const int USE_ITEM = 0x00000002;
    public const int EQUIP = 0x00000004;
    public const int STORE_HOUSE = 0x00000008;
    public const int EQUIP_DEAL = 0x00000010;
    public const int STALL = 0x00000020;
    public const int P2P_TRADE = 0x00000040;
    public const int ITEM_MISC = 0x00000080;
    public const int PICKUP_ITEM = 0x00000100;

    public const int MOVE = 0x00000200;
    public const int OPEN_MAPOBJECT = 0x00000800;
    public const int QUEST = 0x00001000;
    public const int MAIL = 0x00002000;
    public const int NPCCHAT = 0x00004000;
    public const int DUNGEON_TEAM = 0x00008000;
    public const int IB = 0x00010000;

    //互斥状态
    //使用道具/技能/装备升级/制造/打开地图物件时只允许移动打断
    public const int MUTEX_CAST = 0x7FFFFFFF & ~( MOVE );
    //摆摊时不能做其它事
    public const int MUTEX_STALL = 0x7FFFFFFF & ~( STALL );
    //P2P交易时不能做其它事
    public const int MUTEX_P2P_TRADE = 0x7FFFFFFF & ~( P2P_TRADE );
    //团队副本准备时不能做其它事
    public const int MUTEX_DUNGEON_TEAM = 0x7FFFFFFF & ~( DUNGEON_TEAM );
    //答题时不能做其它事
    //public const int MUTEX_QUESTION         = 0x7FFFFFFF & ~( QUEST );
    //死亡时,一切动作都不能
    public const int MUTEX_DEAD = 0x7FFFFFFF & ~( IB );
}

public class ZoneType {
    public const int NORMAL = 0;
    public const int SAFE = 1;
    public const int ARENA = 2;
}

public class GameSystem {
    public const int SKILL = 1;
    public const int BUFF = 2;
    public const int ITEM = 3;
    public const int MAP_OBJECT = 4;
}

public class InviteType {
    public const int INVITE_TEAM = 0;
    public const int APPLY_TEAM = 1;
    public const int TRADE = 2;
    //public const int FRIEND = 3;
    public const int DUEL = 4;
    public const int GROUP = 5;

    public const int INVITE_GROUP_LEAGUE = 7;
    public const int APPLY_GROUP_LEAGUE = 8;
}

[System.AttributeUsage( System.AttributeTargets.Field )]
public class EditorEnumAttribute : System.Attribute {
    public EditorEnumAttribute( string name )
        : this( Def.INVALID_ID, name, String.Empty ) {
    }

    public EditorEnumAttribute( int id, string name )
        : this( id, name, String.Empty ) {
    }

    public EditorEnumAttribute( string name, string display )
        : this( Def.INVALID_ID, name, display ) {
    }

    public EditorEnumAttribute( int id, string name, string display ) {
        this.id = id;
        this.name = name;
        this.display = display;
    }

    public string Name { get { return name; } }
    public string Comment { get { return name; } }
    public string Display { get { return display; } }
    public int ID { get { return id; } }

    private int id;
    private string name;
    private string display;      //客户端用于显示的自定义字符串
}

public class PickupItemMode {
    [EditorEnum( "队伍分配" )]
    public const int ROLL = 0;
    [EditorEnum( "自由拾取" )]
    public const int COMMON = 1;
}

public class CastType {
    [EditorEnum( "无" )]
    public const int NONE = 0xFF;
    [EditorEnum( "技能" )]
    public const int SKILL = 0;
    [EditorEnum( "道具" )]
    public const int ITEM = 1;
    [EditorEnum( "打开" )]
    public const int OPEN = 2;

    // 该部分需要整理 by figo 2013/03/23
    [EditorEnum("硬直")]
    public const int HITBACK = 3;
    [EditorEnum("制造")]
    public const int PRODUCT = 4;
    [EditorEnum("分解")]
    public const int ABSTRACT = 5;
}

//版本类型
public class EditionType {
    [EditorEnum( "通用" )]
    public const int COMMON = 0;

    [EditorEnum( "Game9z" )]
    public const int CHINA = 3;
    [EditorEnum( "360游戏中心" )]
    public const int ET360 = 4;
    [EditorEnum( "LYTO印尼" )]//陈俊添加，2013/1/23
    public const int LYTO = 5;
    [EditorEnum( "360保险箱" )]//陈俊添加，2013/1/23
    public const int Box360 = 6;
    [EditorEnum( "迅雷" )]
    public const int KanKan = 7;
    
    public const int ALL = 99;
}

public class ActionTypes
{
    [EditorEnum("主动")]
    public const int ACTIVE = 1;
    [EditorEnum("被动")]
    public const int  PASSIVITY = 2;
}

//性别
public class RoleGender {
    [EditorEnum( "男性" )]
    public const int MALE = 0;
    [EditorEnum( "女性" )]
    public const int FEMALE = 1;
    [EditorEnum( "无性别" )]
    public const int NONE = 2;

    public const int MAX_GENDER = 2;

    public static string ToString(int gender) {
        switch ( gender ) {
        case MALE:
            return "男性";
        case FEMALE:
            return "女性";
        default:
            return "无性别";
        }
    }
}

public class GroupMemberType {
    public const int MEMBER = 0;
    public const int MANAGER = 1;
    public const int VICE_LEADER = 2;
    public const int LEADER = 3;

    public static string ToString(int type) {
        string sMemberType = "成员";
        switch ( type ) {
        case GroupMemberType.MEMBER:
            sMemberType = "成员";
            break;
        case GroupMemberType.MANAGER:
            sMemberType = "参军";
            break;
        case GroupMemberType.VICE_LEADER:
            sMemberType = "副军团长";
            break;
        case GroupMemberType.LEADER:
            sMemberType = "军团长";
            break;
        }

        return sMemberType;
    }

    public static int ToInt(string sMemberType) {
        switch ( sMemberType ) {
        case "成员": return GroupMemberType.MEMBER;
        case "参军": return GroupMemberType.MANAGER;
        case "副军团长": return GroupMemberType.VICE_LEADER;
        case "军团长": return GroupMemberType.LEADER;
        }

        return GroupMemberType.MEMBER;
    }
}

//职业
public class RoleOccupation {
    [EditorEnum("豪杰")]
    public const int WARRIOR = 0;
    [EditorEnum("月翎")]
    public const int MAGE = 1;
    [EditorEnum( "天机" )]
    public const int WARLOCK = 2;

    [EditorEnum( "无职业" )]
    public const int NONE = 100;

    public const int MAX_OCCUPATION = 3;

    public static string ToString(int occupation) {
        switch ( occupation ) {
        case WARRIOR:
            return "豪杰";
        case MAGE:
            return "月翎";
        case WARLOCK:
            return "天机";
        default:
            return "(无)";
        }
    }
}

//游戏可见对象的类型
public class GameObjectType {
    [EditorEnum( "玩家" )]
    public const int PLAYER = 0;
    [EditorEnum( "NPC" )]
    public const int NPC = 1;
    [EditorEnum( "宠物" )]
    public const int PET = 2;
}


//地图点类型
public class MapPosType {
    [EditorEnum( "默认复活点" )]
    public const int DEFAULT_RELIVE = 1;
    [EditorEnum( "默认出生点" )]
    public const int INIT_POS = 2;
}

public class BattlefieldResult {
    public const int WIN = 1;
    public const int FAIL = 2;
    public const int DRAW = 3;
}

//BUFF作用时机
public class BuffProcessType {
    [EditorEnum( "一直作用" )]
    public const int HOLD = 0;
    [EditorEnum( "定时生效" )]
    public const int TICK = 1;
    [EditorEnum( "使用技能时" )]
    public const int USE_SKILL = 2;
    [EditorEnum( "被伤害时" )]
    public const int BE_DAMAGE = 3;
}

public class MiscAttributeType {
    [EditorEnum( "显示装备" )]
    public const int SHOW_EQUIP = 1;
    [EditorEnum( "显示时装" )]
    public const int SHOW_AVATAR = 2;
    [EditorEnum( "显示名将" )]
    public const int SHOW_HERO = 3;
}

public enum QuestTimeDownType {
    QTDT_inline,
    QTDT_offline,
}

// Misc表类型信息
public class MiscConfType {
    [EditorEnum( "无效配置配置" )]
    public const int DEFAULR = 0;
    [EditorEnum( "装备打孔几率配置" )]
    public const int EQUIP_HOLE = 5;
    [EditorEnum( "装备镶嵌几率配置" )]
    public const int EQUIP_ENCHASE = 6;
    [EditorEnum( "装备取消镶嵌消耗配置" )]
    public const int EQUIP_UNENCHASE = 7;
    [EditorEnum( "装备升星耐久消耗配置" )]
    public const int EQUIP_STARS_EDURE = 8;
    [EditorEnum( "装备升星物攻消耗配置" )]
    public const int EQUIP_STARS_PHS_ATT = 9;
    [EditorEnum( "装备升星魔攻消耗配置" )]
    public const int EQUIP_STARS_MAG_ATT = 10;
    [EditorEnum( "装备升星物防消耗配置" )]
    public const int EQUIP_STARS_PHS_DEF = 11;
    [EditorEnum( "装备升星魔防消耗配置" )]
    public const int EQUIP_STARS_MAG_DEF = 12;
    [EditorEnum( "装备分解配置" )]
    public const int EQUIP_ABSTRACT = 13;
    [EditorEnum( "摊位消耗配置" )]
    public const int STALL_COMSUMPTION = 15;
    [EditorEnum( "师父称号配置" )]
    public const int RELA_MASTER_TITLE = 16;
    [EditorEnum( "结拜称号配置" )]
    public const int RELA_BROTHER_TITLE = 17;
    [EditorEnum( "情侣称号配置" )]
    public const int RELA_LOVER_TITLE = 18;
    [EditorEnum( "夫妻称号配置" )]
    public const int RELA_SPOUSES_TITLE = 19;
    [EditorEnum( "结拜消耗配置" )]
    public const int RELA_BROTHER_CONSUME = 20;
    [EditorEnum( "情侣消耗配置" )]
    public const int RELA_LOVER_CONSUME = 21;
    [EditorEnum( "徒弟升级师傅奖励配置" )]
    public const int RELA_MASTER_AWARDS = 22;
    [EditorEnum( "背包整理指定道具配置" )]
    public const int BAG_COLLATION_FIXITEM = 23;
    [EditorEnum( "背包整理装备次序配置" )]
    public const int BAG_COLLATION_EQUIP = 24;
    [EditorEnum( "背包整理道具次序配置" )]
    public const int BAG_COLLATION_ITEM = 25;
    [EditorEnum( "背包整理材料次序配置" )]
    public const int BAG_COLLATION_MATERIAL = 26;
    [EditorEnum( "装备毛坯空还原配置" )]
    public const int EQUIP_HOLE_RECOVER = 27;
    [EditorEnum( "装备毛坯空开光配置" )]
    public const int EQUIP_HOLE_POLISH = 28;
    [EditorEnum( "装备升星后的效果配置" )]
    public const int EQUIP_STARS_EFFECT = 29;
    [EditorEnum( "天书系统配置" )]
    public const int FASTNESS_EFFECT = 30;
    [EditorEnum( "心法消耗配置" )]
    public const int PASSIVESKILL_COST = 31;
    [EditorEnum( "新修心法消耗倍率配置" )]
    public const int ADD_PASSIVESKILL_COST = 32;

    [EditorEnum( "活动任务图标配置" )]
    public const int HOLIDAY_ICON = 35;
    [EditorEnum( "商城热卖排行" )]
    public const int STORE_HOT_ITEM = 36;
    [EditorEnum( "军团等级配置" )]
    public const int GROUP_LEVEL = 38;
    [EditorEnum( "军团战配置" )]
    public const int GROUP_WAR = 39;
    [EditorEnum( "师德物品兑换" )]
    public const int MASTER_ITEM = 40;
    [EditorEnum( "装备消耗比率" )]
    public const int EQUIP_STARS_RATIO = 41;
    [EditorEnum( "VIP数据" )]
    public const int VIP = 42;
    [EditorEnum("视频播放")]
    public const int VIDEO_PLAY = 43;
}

//代币类型
public class CurrencyType {
    [EditorEnum( "游戏币" )]
    public const int MONEY = 0;
}

// 道具相关
public class ItemFlagType {
    //ITEM对象的标志位掩码
    [EditorEnum( "不可删除" )]
    public const int ITEM_FLAG_CANNOT_REMOVE = 0x00000001;       //不可删除
    [EditorEnum( "不可P2E贩卖" )]
    public const int ITEM_FLAG_CANNOT_P2ETRADE = 0x00000002;       //不可P2E贩卖
    [EditorEnum( "不可P2P交易" )]
    public const int ITEM_FLAG_CANNOT_P2PTRADE = 0x00000004;       //不可P2P交易（含摆摊）
    [EditorEnum( "不可存仓" )]
    public const int ITEM_FLAG_CANNOT_STORE = 0x00000008;       //不可存仓(防止摊位、P2P交易时发生物品转移)
    [EditorEnum( "装备后换头" )]
    public const int ITEM_FLAG_EQUIP_CHG_HEAD = 0x00000040;       //装备后换头
    [EditorEnum( "装备后换全身" )]
    public const int ITEM_FLAG_EQUIP_CHG_ALL = 0x00000080;       //装备后换全身
    [EditorEnum( "装备后保留武器" )]
    public const int ITEM_FLAG_EQUIP_KEEP_WEAPON = 0x00000100;       //装备后保留武器
    [EditorEnum( "装备后绑定" )]
    public const int ITEM_FLAG_EQUIP_BIND = 0x00000200;       //是否装备后绑定
    [EditorEnum("获得时自动装备")]
    public const int ITEM_FLAG_AUTO_EQUIP_WHEN_GET = 0x00000400;	// 获得时自动装备
}

//这里的属性不能和ItemFlagType里面的属性值重复
public class ItemDynamicFlagType {
    [EditorEnum( "是否有装备属性" )]
    public const int ITEM_FLAG_EQUIP_ATTR = 0x00010000;
    [EditorEnum( "是否动态绑定" )]
    public const int ITEM_BIND = 0x00020000;
    [EditorEnum( "是否有过期时间" )]
    public const int ITEM_FLAG_HAS_ENDTIME = 0x00040000;
    [EditorEnum( "是否有使用地点" )]
    public const int ITEM_FLAG_HAS_LOCAL = 0x00080000;
    [EditorEnum( "是否是从商城购买" )]
    public const int ITEM_IB = 0x00100000;
    [EditorEnum( "是否是从商城购买(绑定币）" )]
    public const int ITEM_MONEY_BIND = 0x00200000;
}

public class ItemUseCondition {
    [EditorEnum( "无条件" )]
    public const int NONE = 0;
    [EditorEnum( "特定位置" )]
    public const int AREA = 1;
    [EditorEnum( "特定性别" )]
    public const int GENDER = 2;
    [EditorEnum( "背包中有足够空格" )]
    public const int BAG_HAS_SPACE = 3;
    [EditorEnum( "地图类型" )]
    public const int MAP_TYPE = 4;
    [EditorEnum( "当前经验值大于" )]
    public const int NEED_EXP = 5;
    [EditorEnum( "需要道具" )]
    public const int NEED_ITEM = 6;
}

public class StoreMainType {
    //[EditorEnum( "推荐" )]
    //public const int HOT_ITEM   = 0;
    [EditorEnum( "道具" )]
    public const int ITEM = 2;
    [EditorEnum( "装饰" )]
    public const int DECORATION = 3;
    [EditorEnum( "宠物" )]
    public const int PET_ITEM = 4;
    [EditorEnum( "会员" )]
    public const int VIP_ITEM = 1;
}

public class StoreSubType {

    [EditorEnum( StoreMainType.VIP_ITEM, "全部" )]
    public const int VIP_ITEM_NONE = 10;

    [EditorEnum( StoreMainType.ITEM, "练功用" )]
    public const int ITEM_SUPPLY = 20;
    [EditorEnum( StoreMainType.ITEM, "装备强化" )]
    public const int ITEM_STRENGTHEN = 21;
    [EditorEnum( StoreMainType.ITEM, "技能辅助" )]
    public const int ITEM_SKILL = 22;
    [EditorEnum( StoreMainType.ITEM, "其他功能" )]
    public const int ITEM_FUNC = 23;
    [EditorEnum( StoreMainType.ITEM, "活动用" )]
    public const int ITEM_PROMOTION = 24;

    [EditorEnum( StoreMainType.DECORATION, "男性将魂" )]
    public const int AVATAR_HERO_MALE = 30;
    [EditorEnum( StoreMainType.DECORATION, "女性将魂" )]
    public const int AVATAR_HERO_FEMALE = 31;
    [EditorEnum( StoreMainType.DECORATION, "其它装饰" )]
    public const int AVATAR_MISC = 32;
    [EditorEnum( StoreMainType.DECORATION, "男性装饰" )]
    public const int AVATAR_MALE = 33;
    [EditorEnum( StoreMainType.DECORATION, "女性装饰" )]
    public const int AVATAR_FEMALE = 34;
    [EditorEnum( StoreMainType.DECORATION, "其他将魂" )]
    public const int AVATAR_HERO_MISC = 35;

    [EditorEnum( StoreMainType.PET_ITEM, "骑宠" )]
    public const int PET_ITEM_MOUNT = 41;
}

public class ItemKind1 {
    [EditorEnum("装备")]
    public const int EQUIP = 0;
    [EditorEnum("道具")]
    public const int ITEM = 1;
}

public class ItemKind2{
    [EditorEnum(ItemKind1.EQUIP, "防具")]
    public const int EQUIP_PROTECTION = 0;
    [EditorEnum(ItemKind1.EQUIP, "时装防具")]
    public const int EQUIP_AV_PROTECTION = 1;
    [EditorEnum(ItemKind1.EQUIP, "武器")]
    public const int EQUIP_ARMS = 2;
    [EditorEnum(ItemKind1.EQUIP, "时装武器")]
    public const int EQUIP_AV_ARMS = 3;
    [EditorEnum(ItemKind1.EQUIP, "饰品")]
    public const int EQUIP_JEWELRY = 4;
    [EditorEnum(ItemKind1.EQUIP, "将魂")]
    public const int EQUIP_HERO_SOUL = 5;
    [EditorEnum(ItemKind1.EQUIP, "骑乘")]
    public const int EQUIP_HERO_HORSE = 6;

    [EditorEnum(ItemKind1.ITEM, "可使用道具")]
    public const int ITEM_CANUSE = 20;
    [EditorEnum(ItemKind1.ITEM, "任务道具")]
    public const int ITEM_QUEST = 21;
    [EditorEnum(ItemKind1.ITEM, "生活技能道具")]
    public const int ITEM_PRODUCT = 22;
    [EditorEnum(ItemKind1.ITEM, "装备升级材料")]
    public const int ITEM_EQUIP_MATERIAL = 23;
    [EditorEnum(ItemKind1.ITEM, "杂物")]
    public const int ITEM_MISC = 24;
}


public class ItemKind3 {
    //武器、衣服、头盔、腰带、护肩、鞋子、护腕、项链、戒指、挂饰
    [EditorEnum(ItemKind2.EQUIP_PROTECTION, "头盔")]
    public const int EQUIP_PROTECTION_HEAD = 0;
    [EditorEnum(ItemKind2.EQUIP_PROTECTION, "项链")]
    public const int EQUIP_PROTECTION_NECK = 1;
    [EditorEnum(ItemKind2.EQUIP_PROTECTION, "护肩")]
    public const int EQUIP_PROTECTION_SHOULDER = 2;
    [EditorEnum(ItemKind2.EQUIP_PROTECTION, "衣服")]
    public const int EQUIP_PROTECTION_BODY = 3;
    [EditorEnum(ItemKind2.EQUIP_PROTECTION, "腰带")]
    public const int EQUIP_PROTECTION_BELT = 4;
    [EditorEnum(ItemKind2.EQUIP_PROTECTION, "护腕")]
    public const int EQUIP_PROTECTION_FOREARM = 5;
    [EditorEnum(ItemKind2.EQUIP_PROTECTION, "戒指")]
    public const int EQUIP_PROTECTION_RING = 6;
    [EditorEnum(ItemKind2.EQUIP_PROTECTION, "鞋子")]
    public const int EQUIP_PROTECTION_CRUS = 7;
    [EditorEnum(ItemKind2.EQUIP_PROTECTION, "挂饰")]
    public const int EQUIP_PROTECTION_DECOR = 8;

    [EditorEnum(ItemKind2.EQUIP_ARMS, "武器")]
    public const int EQUIP_ARMS_WEAPON = 9;

    //前面的是需要修理的物品
    public const int MAX_NEED_REPAIR = 10;

    public const int AV_Min = 10;
    // 留给以后时装用 
    public const int AV_Max = 10;

    [EditorEnum(ItemKind2.EQUIP_HERO_HORSE, "骑乘")]
    public const int EQUIP_HORSE = 10;
    //最大装备数量
    public const int MAX_EQUIP = 10;

    //道具
    [EditorEnum(ItemKind2.ITEM_CANUSE, "宝箱")]
    public const int ITEM_CANUSE_BOX = 100;
    [EditorEnum(ItemKind2.ITEM_CANUSE, "普通红药")]
    public const int ITEM_CANUSE_NORMAL_HP = 101;
    [EditorEnum(ItemKind2.ITEM_CANUSE, "IB红药")]
    public const int ITEM_CANUSE_IB_HP = 102;
    [EditorEnum(ItemKind2.ITEM_CANUSE, "普通蓝药")]
    public const int ITEM_CANUSE_NORMAL_MP = 103;
    [EditorEnum(ItemKind2.ITEM_CANUSE, "IB蓝药")]
    public const int ITEM_CANUSE_IB_MP = 104;
    [EditorEnum(ItemKind2.ITEM_CANUSE, "怒气")]
    public const int ITEM_CANUSE_RAGE = 105;
    [EditorEnum(ItemKind2.ITEM_CANUSE, "传送卷轴")]
    public const int ITEM_CANUSE_TELPORT_SCROLL = 106;
    [EditorEnum(ItemKind2.ITEM_CANUSE, "学习卷轴")]
    public const int ITEM_CANUSE_LEARN_SCROLL = 107;
    [EditorEnum(ItemKind2.ITEM_CANUSE, "攻击卷轴")]
    public const int ITEM_CANUSE_ATT_SCROLL = 108;
    [EditorEnum(ItemKind2.ITEM_CANUSE, "防御卷轴")]
    public const int ITEM_CANUSE_DEF_SCROLL = 109;
    [EditorEnum(ItemKind2.ITEM_CANUSE, "经验卷轴")]
    public const int ITEM_CANUSE_EXP_SCROLL = 110;
    [EditorEnum(ItemKind2.ITEM_CANUSE, "回城类")]
    public const int ITEM_CANUSE_RETURN_CITY = 111;
    [EditorEnum(ItemKind2.ITEM_CANUSE, "技能类")]
    public const int ITEM_CANUSE_SKILL = 112;
    [EditorEnum(ItemKind2.ITEM_CANUSE, "生命食品")]
    public const int ITEM_CANUSE_FOOD = 113;
    [EditorEnum(ItemKind2.ITEM_CANUSE, "晶石")]
    public const int ITEM_CANUSE_STONE = 114;
    [EditorEnum(ItemKind2.ITEM_CANUSE, "任务完成条件")]
    public const int ITEM_CANUSE_QUEST = 115;
    [EditorEnum(ItemKind2.ITEM_CANUSE, "法力食品")]
    public const int ITEM_CANUSE_MAGIC_FOOD = 116;
    [EditorEnum(ItemKind2.ITEM_CANUSE, "驿站关碟")]
    public const int ITEM_BIND_RETURN_POS = 117;

    [EditorEnum(ItemKind2.ITEM_CANUSE, "藏宝图")]
    public const int ITEM_CANUSE_TREASURY = 123;
    [EditorEnum(ItemKind2.ITEM_CANUSE, "VIP充值")]
    public const int ITEM_CANUSE_VIP = 124;
    [EditorEnum(ItemKind2.ITEM_CANUSE, "制造卷轴")]
    public const int ITEM_CANUSE_MANUFACTURE_SCROLL = 125;

    //任务
    [EditorEnum(ItemKind2.ITEM_QUEST, "任务需要物品")]
    public const int ITEM_QUEST_NORMAL = 200;
    [EditorEnum(ItemKind2.ITEM_QUEST, "答题-免错金牌")]
    public const int ITEM_QUEST_QUESTION_REPEAT = 201;
    [EditorEnum(ItemKind2.ITEM_QUEST, "答题-如意金牌")]
    public const int ITEM_QUEST_QUESTION_MODIFY = 202;
    [EditorEnum(ItemKind2.ITEM_QUEST, "答题-携款潜逃")]
    public const int ITEM_QUEST_QUESTION_AWARD = 203;
    [EditorEnum(ItemKind2.ITEM_QUEST, "答题-双倍积分")]
    public const int ITEM_QUEST_QUESTION_INTEGRAL = 204;
    [EditorEnum(ItemKind2.ITEM_QUEST, "答题-组队免错")]
    public const int ITEM_QUEST_QUESTION_TEAM = 205;

    //生活技能
    [EditorEnum(ItemKind2.ITEM_PRODUCT, "采集道具")]
    public const int ITEM_PRODUCT_COLLECT_TOOLS = 300;
    [EditorEnum(ItemKind2.ITEM_PRODUCT, "锻造材料")]
    public const int ITEM_PRODUCT_FORGING = 301;
    [EditorEnum(ItemKind2.ITEM_PRODUCT, "裁缝材料")]
    public const int ITEM_PRODUCT_SEWING = 302;
    [EditorEnum(ItemKind2.ITEM_PRODUCT, "炼药材料")]
    public const int ITEM_PRODUCT_PHARMACY = 303;
    [EditorEnum(ItemKind2.ITEM_PRODUCT, "附灵材料")]
    public const int ITEM_PRODUCT_GET_SOUL = 304;
    [EditorEnum(ItemKind2.ITEM_PRODUCT, "饰品材料")]
    public const int ITEM_PRODUCT_JEWELERY = 305;
    [EditorEnum(ItemKind2.ITEM_PRODUCT, "宝石材料")]
    public const int ITEM_PRODUCT_GEMSTONE = 306;
    [EditorEnum(ItemKind2.ITEM_PRODUCT, "制造道具")]
    public const int ITEM_PRODUCT_TOOLS = 307;

    //装备升级
    [EditorEnum(ItemKind2.ITEM_EQUIP_MATERIAL, "镶嵌宝石")]
    public const int ITEM_EQUIP_MATERIAL_ENCHASE = 400;
    [EditorEnum(ItemKind2.ITEM_EQUIP_MATERIAL, "初级升星石")]
    public const int ITEM_EQUIP_MATERIAL_STAR1 = 401;
    [EditorEnum(ItemKind2.ITEM_EQUIP_MATERIAL, "中级升星石")]
    public const int ITEM_EQUIP_MATERIAL_STAR2 = 402;
    [EditorEnum(ItemKind2.ITEM_EQUIP_MATERIAL, "高级升星石")]
    public const int ITEM_EQUIP_MATERIAL_STAR3 = 403;

    [EditorEnum(ItemKind2.ITEM_EQUIP_MATERIAL, "开光道具")]
    public const int ITEM_EQUIP_MATERIAL_KGS = 404;
    [EditorEnum(ItemKind2.ITEM_EQUIP_MATERIAL, "还原道具")]
    public const int ITEM_EQUIP_MATERIAL_BTF = 405;
    [EditorEnum(ItemKind2.ITEM_EQUIP_MATERIAL, "完璧符")]
    public const int ITEM_EQUIP_MATERIAL_WFB = 406;
    [EditorEnum(ItemKind2.ITEM_EQUIP_MATERIAL, "神匠笔录")]
    public const int ITEM_EQUIP_MATERIAL_SJBL = 407;
    [EditorEnum(ItemKind2.ITEM_EQUIP_MATERIAL, "打孔道具")]
    public const int ITEM_EQUIP_MATERIAL_QTT = 408;

    [EditorEnum(ItemKind2.ITEM_EQUIP_MATERIAL, "其它")]
    public const int ITEM_EQUIP_MATERIAL_OTHER = 411;

    [EditorEnum(ItemKind2.ITEM_EQUIP_MATERIAL, "级品升星石")]
    public const int ITEM_EQUIP_MATERIAL_STAR4 = 430;

    //杂物
    [EditorEnum(ItemKind2.ITEM_MISC, "功能性杂物")]
    public const int ITEM_MISC_FUNC = 500;
    [EditorEnum(ItemKind2.ITEM_MISC, "其它杂物")]
    public const int ITEM_MISC_OTHER = 501;
    [EditorEnum(ItemKind2.ITEM_MISC, "副本计分物品")]
    public const int ITEM_MISC_SCORE = 502;
    [EditorEnum(ItemKind2.ITEM_MISC, "废弃 保留ID")]
    public const int ITEM_MISC_NOT_USE1 = 503;
    [EditorEnum(ItemKind2.ITEM_MISC, "世界频道")]
    public const int ITEM_MISC_CHAT = 504;
    [EditorEnum(ItemKind2.ITEM_MISC, "洗髓丹")]
    public const int ITEM_MISC_XSD = 505;

    [EditorEnum(ItemKind2.ITEM_MISC, "一阶战勋值")]
    public const int ITEM_MISC_BATTLEFIELD_VALUE1 = 506;
    [EditorEnum(ItemKind2.ITEM_MISC, "二阶战勋值")]
    public const int ITEM_MISC_BATTLEFIELD_VALUE2 = 507;

    [EditorEnum(ItemKind2.ITEM_MISC, "马车令")]
    public const int ITEM_MISC_RETURN_POS = 508;
    [EditorEnum(ItemKind2.ITEM_MISC, "转换阵营")]
    public const int ITEM_MISC_CHANGE_CAMP = 509;
}

public class ItemQuality
{
    [EditorEnum("白色")]
    public const int WHITE = 0;
    [EditorEnum("绿色")]
    public const int GREEN = 1;
    [EditorEnum("蓝色")]
    public const int BLUE = 2;
    [EditorEnum("紫色")]
    public const int PURPLE = 3;
    [EditorEnum("橙色")]// 将魂三国原来的设定是黄色，差不多啊
    public const int YELLOW = 4;
    [EditorEnum("完美橙色")]// 新增的品质
    public const int PERFECT_YELLOW = 5;

    public const int MAX_QUALITY = 6;
}//效果定义(有新的直接递增,不能重用已用的index)

public class BuffTriggerType
{
    [EditorEnum("伤害值")]
    public const int HurtValue = 0;
    [EditorEnum("最大生命值百分比")]
    public const int MaxHPRate= 1;

}

#region DirectEffectType 技能效果

public class DirectEffectType {
    [EditorEnum("无效果")]
    public const int None = 0;
    [EditorEnum("普通伤害", "普通伤害")]
    public const int Attack = 1;
    [EditorEnum("造成雷伤害", "+{0}雷伤害")]
    public const int ThunderDam = 2;
    [EditorEnum("造成火伤害", "+{0}火伤害")]
    public const int FireDam = 3;
    [EditorEnum("造成风伤害", "+{0}风伤害")]
    public const int WindDam = 4;
    [EditorEnum("添加BUFF", "添加BUFF")]
    public const int AddBuff = 5;
    [EditorEnum("添加BUFF给自己", "添加BUFF给自己")]
    public const int AddBtoSelf = 6;
    [EditorEnum("添加两个BUFF", "添加两个BUFF")]
    public const int AddTwoBuff = 7;
 /*   [EditorEnum("普通防御", "普通防御")]
    public const int Defense = 2;
    [EditorEnum("命中", "命中")]
    public const int HitTarget = 3;
    [EditorEnum("闪避", "闪避")]
    public const int Dodge = 4;
    [EditorEnum("暴击", "暴击")]
    public const int Critical = 5;
    [EditorEnum("免暴", "免暴")]
    public const int NoCritical = 6;
    [EditorEnum("生命值", "+{0}生命值")]
    public const int HP = 7;
    [EditorEnum("眩晕", "眩晕")]
    public const int MakeFaint = 8;
    [EditorEnum("缠绕", "缠绕")]
    public const int MakeBind = 9;
    [EditorEnum("冰冻", "冰冻")]
    public const int  MakeIce= 10;
    [EditorEnum("移动速度", "移动速度")]
    public const int WalkSpeed = 11;
    [EditorEnum("流血", "流血")]
    public const int LoseBlood = 12;*/

}

#endregion

#region BUFF效果
public class HoldEffectType {
    [EditorEnum( "无效果" )]
    public const int None = 0;
    [EditorEnum( "最大生命", "+{0}最大生命" )]
    public const int VMaxHP = 1;
    [EditorEnum( "生命回复值", "+{0}生命回复值" )]
    public const int VHPRe = 2;
    [EditorEnum( "生命回复比", "+{0}生命回复比" )]
    public const int PHPRe = 3;
    [EditorEnum("普通攻击", "+{0}普通攻击")]
    public const int VAttack = 4;
    [EditorEnum("普通防御", "+{0}普通防御")]
    public const int VDefense = 5;
    [EditorEnum( "移动速度", "+{0}移动速度" )]
    public const int VWalkSpeed = 6;
    [EditorEnum("命中值", "+{0}命中值")]
    public const int VHitTarget = 7;
    [EditorEnum("闪避值", "+{0}闪避值")]
    public const int VDodge = 8;
    [EditorEnum("暴击值", "+{0}暴击值")]
    public const int VCritical = 9;
    [EditorEnum("免暴值", "+{0}免暴值")]
    public const int VNoCritical = 10;
    [EditorEnum("风系攻击", "+{0}风系攻击")]
    public const int VWindAttack = 11;
    [EditorEnum("雷系攻击", "+{0}雷系攻击")]
    public const int VThunderAttack = 12;
    [EditorEnum("火系攻击", "+{0}火系攻击")]
    public const int VFireAttack = 13;
    [EditorEnum("全属性攻击", "+{0}全属性攻击")]
    public const int VAllAttrAttack = 14;
    [EditorEnum("风系抗性", "+{0}风系抗性")]
    public const int VWindDefense = 15;
    [EditorEnum("雷系抗性", "+{0}雷系抗性")]
    public const int VThunderDefense =16;
    [EditorEnum("火系抗性", "+{0}火系抗性")]
    public const int VFireDefense = 17;
    [EditorEnum("全属性抗性", "+{0}全属性抗性")]
    public const int VAllAttrDefense = 18;
    [EditorEnum("普通伤害", "+{0}普通伤害")]
    public const int VAttackHurt = 19;
    [EditorEnum("风系伤害", "+{0}风系伤害")]
    public const int VWindAtt = 20;
    [EditorEnum("雷系伤害", "+{0}雷系伤害")]
    public const int VThunderAtt = 21;
    [EditorEnum("火系伤害", "+{0}火系伤害")]
    public const int VFireAtt = 22;
    [EditorEnum("最大生命万分比", "+{0}%生命")]
    public const int PMaxHPRate = 23;
    [EditorEnum("普通攻击万分比", "+{0}%普通攻击")]
    public const int PhyAttRate = 24;
    [EditorEnum("普通防御万分比", "+{0}%普通防御")]
    public const int PDefenseRate = 25;
    [EditorEnum("移动速度万分比", "+{0}%移动速度")]
    public const int PWalkSpeedRate = 26;
    [EditorEnum("命中值万分比", "+{0}%命中值")]
    public const int PHitTargetRate = 27;
    [EditorEnum("闪避值万分比", "+{0}%闪避值")]
    public const int PDodgeRate = 28;
    [EditorEnum("暴击值万分比", "+{0}%暴击值")]
    public const int PCriticalRate = 29;
    [EditorEnum("免暴值万分比", "+{0}%免暴值")]
    public const int PNoCriticalRate = 30;
    [EditorEnum("风系攻击万分比", "+{0}%风系攻击比")]
    public const int PWindAtt = 31;
    [EditorEnum("雷系攻击万分比", "+{0}%雷系攻击比")]
    public const int PThunderAtt = 32;
    [EditorEnum("火系攻击万分比", "+{0}%火系攻击比")]
    public const int PFireAtt = 33;
    [EditorEnum( "全属性攻击万分比", "+{0}全属性攻击比" )]
    public const int PVAllAtt = 34;
    [EditorEnum("风系抗性万分比", "+{0}风系抗性比")]
    public const int PVWindDef = 35;
    [EditorEnum("雷系抗性万分比", "+{0}雷系抗性比")]
    public const int PTWindDef = 36;
    [EditorEnum("火系抗性万分比", "+{0}火系抗性比")]
    public const int PVFireDef = 37;
    [EditorEnum("全属性抗性万分比", "+{0}全属性抗性比")]
    public const int PVAllDef = 38;
    [EditorEnum("全属性万分比", "+{0}全属性万分比")]
    public const int PAllDef = 39;
    [EditorEnum("禁足", "+{0}禁足")]
    public const int VNoWalk = 40;
    [EditorEnum("禁技", "+{0}v")]
    public const int VNoSkill = 41;
    [EditorEnum("禁足/禁技", "+{0}禁足/禁技")]
    public const int VNoWalkAndSkill = 42;
}
#endregion

// 功能道具功能类型定义
public class ItemFunType {
    [EditorEnum( "无特殊效果" )]
    public const int None = 0;
    [EditorEnum( "召唤怪物" )]
    public const int SummonMonster = 2;
    [EditorEnum( "打开包裹" )]
    public const int OpenBag = 3;
    [EditorEnum( "技能书籍" )]
    public const int SkillBook = 4;
    [EditorEnum( "活力道具" )]
    public const int VigourItem = 5;
    [EditorEnum( "装备道具" )]
    public const int EquipItem = 6;  // 参数含义 等级或者修正概率
    [EditorEnum( "师德值道具" )]
    public const int MeritItem = 7;
    [EditorEnum( "亲密度道具" )]
    public const int FamilarityItem = 8;
    [EditorEnum( "背包扩展道具" )]
    public const int BagExtendItem = 9;
    [EditorEnum( "仓库扩展道具" )]
    public const int StoreExtendItem = 10;

    [EditorEnum( "增加HP" )]
    public const int HP = 11;
    [EditorEnum( "增加MP" )]
    public const int MP = 12;
    [EditorEnum( "增加Buff" )]
    public const int AddBuff = 13;
    [EditorEnum( "传送" )]
    public const int Teleport = 14;
    [EditorEnum( "清除移动限制" )]
    public const int ClearRestrictMove = 15;

    [EditorEnum( "增加怒气星上限" )]
    public const int AddMaxRage = 17;
    [EditorEnum( "增加怒气" )]
    public const int AddRage = 18;

    [EditorEnum( "天赋洗点道具" )]
    public const int ClearTalent = 19;
    [EditorEnum( "替身摊位道具" )]
    public const int StandInStall = 20;

    [EditorEnum( "修理道具" )]
    public const int RepairItem = 22;
    [EditorEnum( "复活道具" )]
    public const int ReliveItem = 23;
    [EditorEnum( "附带技能" )]
    public const int AttachedSkill = 24;
    [EditorEnum( "天赋书" )]
    public const int TalentBook = 25;

    [EditorEnum( "情节表演" )]
    public const int Performance = 26;
    [EditorEnum( "地图状态" )]
    public const int MapState = 27;
    [EditorEnum( "减少恶名值" )]
    public const int DecKillPoint = 28;
    [EditorEnum( "驿站关碟传送" )]
    public const int ReturnPos = 29;
    [EditorEnum( "增加公会活力" )]
    public const int GroupVigour = 30;
    [EditorEnum( "接任务" )]
    public const int OneQuest = 31;
    [EditorEnum( "脚本（获得物品）" )]
    public const int OneScript = 32;

    [EditorEnum( "特效" )]
    public const int SpecialEffects = 33;
    [EditorEnum( "随身仓库" )]
    public const int ItemStoreHouse = 34;

    [EditorEnum( "VIP充值" )]
    public const int Vip = 35;
    [EditorEnum( "远程购买" )]
    public const int RemoteBuyItem = 36;

    [EditorEnum( "军团资金注入" )]
    public const int GroupMoney = 37;
    [EditorEnum( "购买军团领地" )]
    public const int GroupMap = 38;
    [EditorEnum( "军团地图传送" )]
    public const int GroupMapTeleport = 39;

    [EditorEnum( "换车令" )]
    public const int Carriage = 40;

    [EditorEnum( "增加元宝" )]
    public const int SpecialItem = 41;

    [EditorEnum( "添加变身BUFF", "添加变身BUFF" )]
    public const int AddAppridBuff = 42;

    [EditorEnum( "生产配方" )]
    public const int Manufacture = 43;
    [EditorEnum( "扣除经验" )]
    public const int RemoveExp = 44;

    [EditorEnum( "国家经验" )]
    public const int CountryExp = 45;
    [EditorEnum( "升级军衔" )]
    public const int UpRank = 46;
    [EditorEnum( "添加战勋" )]
    public const int AddBattlefieldValue = 47;
    //[EditorEnum( "添加生活技能熟练度" )]
    //public const int AddProductSkillPoint = 48;
    [EditorEnum( "绑定元宝" )]
    public const int SpecialItemBind = 49;
    [EditorEnum( "添加荣誉" )]
    public const int Honour = 50;
    [EditorEnum( "扣除道具" )]
    public const int RemoveItem = 51;
}

public class EquipHoleType {
    [EditorEnum( "劣质开光槽" )]
    public const int TINPOT = 1;    // 劣质开光槽
    [EditorEnum( "裂纹开光槽" )]
    public const int CRACKED = 2;    // 裂纹开光槽
    [EditorEnum( "精致开光槽" )]
    public const int EXQUISITE = 3;    // 精致开光槽
    [EditorEnum( "完美开光槽" )]
    public const int PERFECT = 4;    // 完美开光槽
    [EditorEnum( "无双开光槽" )]
    public const int UNPARALLELED = 5;    // 无双开光槽 

    public const int MAX_COUNT = 5;
}

public class EquipStarsType {
    [EditorEnum( "升级耐久" )]
    public const int ENDURE = 1;
    [EditorEnum( "升级物理攻击" )]
    public const int PHS_ATT = 2;
    [EditorEnum( "升级法术攻击" )]
    public const int MAG_ATT = 3;
    [EditorEnum( "升级物理防御" )]
    public const int PHS_DEF = 4;
    [EditorEnum( "升级法术防御" )]
    public const int MAG_DEF = 5;

    //min max用于有效性检查
    public const int MIN = 1;
    public const int MAX = 5;
    public const int MAX_COUNT = 5;
}

public class TalentEffectType {
    [EditorEnum( "无效果" )]
    public const int None = 0;
    [EditorEnum( "增加伤害" )]
    public const int Damage = 1;
    [EditorEnum( "施放时间" )]
    public const int CastTime = 2;
    [EditorEnum( "机率" )]
    public const int EffectRate = 3;
    [EditorEnum( "技能冷却" )]
    public const int Cooldown = 4;
    [EditorEnum( "范围半径" )]
    public const int Around = 5;
    [EditorEnum( "影响目标上限" )]
    public const int TargetLimit = 6;
    [EditorEnum( "技能施放距离" )]
    public const int Distance = 7;
    [EditorEnum( "效果参数1" )]
    public const int EffectP1 = 8;
    [EditorEnum( "效果参数2" )]
    public const int EffectP2 = 9;
    [EditorEnum( "效果参数3" )]
    public const int EffectP3 = 10;
    [EditorEnum( "精气消耗" )]
    public const int MP = 11;
}

public class TriggeType {
    [EditorEnum( "不触发" )]
    public const int NONE = 0;
    [EditorEnum( "靠近触发" )]
    public const int NEAR = 1;
    [EditorEnum( "主动触发" )]
    public const int OPEN = 2;
}

public class MapObjectEffectType {
    [EditorEnum( "无效果" )]
    public const int None = 0;
    [EditorEnum( "掉落物品" )]
    public const int DropItem = 1;
    [EditorEnum( "传送点" )]
    public const int Teleport = 2;
    [EditorEnum( "采集物品" )]
    public const int Collect = 3;
    [EditorEnum( "情节表演" )]
    public const int Performance = 4;
    [EditorEnum( "地图状态" )]
    public const int MapState = 5;
    [EditorEnum( "影响HP" )]
    public const int HP = 6;
    [EditorEnum( "给指定角色发送AI消息" )]
    public const int SendAIMsg = 7;
    [EditorEnum( "扣除必需物品" )]
    public const int RemoveNeedItem = 8;
    [EditorEnum( "传送出副本" )]
    public const int ExitDungeon = 9;
    [EditorEnum( "宝箱" )]
    public const int Box = 10;
    [EditorEnum( "幻境继续挑战" )]
    public const int CanContinue = 11;
    [EditorEnum( "触发任务" )]
    public const int Quest = 12;
    [EditorEnum( "添加BUFF" )]
    public const int AddBuff = 13;
    [EditorEnum( "召唤冀州战场旗帜" )]
    public const int SummonBattlefieldFlag = 14;
    [EditorEnum( "影响MP" )]
    public const int MP = 15;
    [EditorEnum( "军团盛宴" )]
    public const int GroupExp = 16;
    [EditorEnum( "国公争夺战建筑" )]
    public const int GroupLeagueWarMapBuild = 17;
    [EditorEnum( "调用脚本" )]
    public const int Script = 18;
}

public class ScheduledEffectType {
    [EditorEnum( "系统消息" )]
    public const int SystemMessage = 0;
    [EditorEnum( "日常任务" )]
    public const int Quest = 1;
    [EditorEnum( "显示角色/地图对象" )]
    public const int ShowObject = 2;
    [EditorEnum( "开启副本" )]
    public const int OpenDungeon = 3;
    [EditorEnum( "开启世界掉落" )]
    public const int DropItem = 4;
    [EditorEnum( "开启军团联盟战" )]
    public const int GroupLeagueWar = 5;
    [EditorEnum( "多倍经验" )]
    public const int ExpRate = 6;
    [EditorEnum( "多倍活力" )]
    public const int VigourRate = 7;
    [EditorEnum( "开始计划任务" )]
    public const int OpenScheduled = 8;
    [EditorEnum( "国战通知" )]
    public const int CountryWarNotice = 9;
    [EditorEnum( "开始国战" )]
    public const int CreateCountryWarMap = 10;
}

//NPC对话类型定义
public class NPCChatType {
    [EditorEnum("测试类型")]
    public const int TEST = 0;
}

//------AI相关---------
public class NPCAIType {
    [EditorEnum( "NPC" )]
    public const int NPC = 0;
    [EditorEnum( "怪物" )]
    public const int Monster = 1;
    [EditorEnum( "巡逻NPC" )]
    public const int PatrolNPC = 2;
    [EditorEnum( "物件" )]
    public const int Object = 3; //不移动,不反击,可破坏的物件
}

public class AIEvent {
    public const int NO_TARGET = 0;
    public const int HAS_TARGET = 1;
    public const int MAX_EVENT = 14;

    [EditorEnum( AIEvent.NO_TARGET, "无事件", "无事件目标" )]
    public const int NONE = 0;
    [EditorEnum( AIEvent.NO_TARGET, "每秒执行", "无事件目标" )]
    public const int ACTIVE = 1;
    [EditorEnum( AIEvent.HAS_TARGET, "自已死亡", "至死者" )]
    public const int SELF_DIE = 2;
    [EditorEnum( AIEvent.HAS_TARGET, "受到伤害", "伤害者" )]
    public const int DAMAGE = 3;
    [EditorEnum( AIEvent.HAS_TARGET, "受到治疗", "治疗者" )]
    public const int RECOVER = 4;
    [EditorEnum( AIEvent.HAS_TARGET, "收到信息", "发信息者" )]
    public const int MSG = 5;
    [EditorEnum( AIEvent.NO_TARGET, "AI初始化", "无事件目标" )]
    public const int AI_INIT = 6;
    [EditorEnum( AIEvent.HAS_TARGET, "杀死其它角色", "被杀者" )]
    public const int KILL_ROLE = 7;
    [EditorEnum( AIEvent.HAS_TARGET, "进入战斗状态", "引发战斗者" )]
    public const int ENTER_FIGHT = 8;
    [EditorEnum( AIEvent.HAS_TARGET, "开始逃跑", "逃避者" )]
    public const int BEGIN_ESCAPE = 9;
    [EditorEnum( AIEvent.HAS_TARGET, "完成逃跑", "无事件目标" )]
    public const int ESCAPE_DONE = 10;
    [EditorEnum( AIEvent.HAS_TARGET, "完成按路径行走", "无事件目标" )]
    public const int END_PATROL = 11;
    [EditorEnum( AIEvent.NO_TARGET, "脱离战斗状态" )]
    public const int EXIT_FIGHT = 12;
    [EditorEnum( AIEvent.NO_TARGET, "显示", "无事件目标" )]
    public const int SHOW = 13;
}

public class AIConditionType {
    [EditorEnum( AIEvent.NO_TARGET, "无条件" )]
    public const int NONE = 0;
    [EditorEnum( AIEvent.NO_TARGET, "附加参数等于" )]
    public const int PARAM = 1;
    [EditorEnum( AIEvent.NO_TARGET, "事件达到次数" )]
    public const int EVENT_COUNT = 2;
    [EditorEnum( AIEvent.NO_TARGET, "自己生命小于" )]
    public const int SELF_HP = 3;
    [EditorEnum( AIEvent.HAS_TARGET, "事件目标生命小于" )]
    public const int TARGET_HP = 4;
    [EditorEnum( AIEvent.NO_TARGET, "战斗目标生命小于" )]
    public const int FIGHT_TARGET_HP = 5;
    [EditorEnum( AIEvent.NO_TARGET, "情节表演" )]
    public const int PERFORMANCE = 6;
    [EditorEnum( AIEvent.NO_TARGET, "地图状态等于" )]
    public const int MAP_STATE = 7;
    [EditorEnum( AIEvent.NO_TARGET, "地图状态设置时间大于" )]
    public const int MAP_STATE_TIME = 8;
    [EditorEnum( AIEvent.NO_TARGET, "指定角色是否死亡" )]
    public const int IS_DEAD = 9;
    [EditorEnum( AIEvent.NO_TARGET, "指定角色是否存活" )]
    public const int IS_LIVE = 10;
    [EditorEnum( AIEvent.NO_TARGET, "自己SN等于" )]
    public const int SN = 11;
    [EditorEnum( AIEvent.NO_TARGET, "指定角色距离小于" )]
    public const int DISTANCE = 12;
    [EditorEnum( AIEvent.NO_TARGET, "地图状态不等于" )]
    public const int MAP_STATE_NOT_EQUAL = 13;
    [EditorEnum( AIEvent.NO_TARGET, "地图状态小于" )]
    public const int MAP_STATE_LESS = 14;
}

//注意,这里面的常量不能修改(因为已经存到数据库里面了)
//2011-9-24做了一下调整,相关动作归类到一起,新增时注意ID不能重复
public class AIActionType {
    [EditorEnum( AIEvent.NO_TARGET, "无行为" )]
    public const int NONE = 0;
    [EditorEnum( AIEvent.NO_TARGET, "对自己使用技能" )]
    public const int USE_SKILL_ON_SELF = 1;
    [EditorEnum( AIEvent.HAS_TARGET, "对事件目标使用技能" )]
    public const int USE_SKILL_ON_TARGET = 2;
    [EditorEnum( AIEvent.NO_TARGET, "对战斗目标使用技能" )]
    public const int USE_SKILL_ON_FIGHT_TARGET = 3;
    [EditorEnum( AIEvent.NO_TARGET, "对随机目标使用技能" )]
    public const int USE_SKILL_ON_RANDOM_TARGET = 40;
    [EditorEnum( AIEvent.NO_TARGET, "攻击指定SN角色" )]
    public const int ATTACK = 16;
    [EditorEnum( AIEvent.NO_TARGET, "停止攻击" )]
    public const int STOP_ATTACK = 27;
    [EditorEnum( AIEvent.NO_TARGET, "对指定SN使用技能" )]
    public const int USE_SKILL_ON_SN = 44;

    [EditorEnum( AIEvent.NO_TARGET, "给自己添加BUFF" )]
    public const int ADD_BUFF_ON_SELF = 5;
    [EditorEnum( AIEvent.HAS_TARGET, "给事件目标添加BUFF" )]
    public const int ADD_BUFF_ON_TARGET = 6;
    [EditorEnum( AIEvent.NO_TARGET, "给战斗目标添加BUFF" )]
    public const int ADD_BUFF_ON_FIGHT_TARGET = 7;
    [EditorEnum( AIEvent.NO_TARGET, "给指定SN角色添加BUFF" )]
    public const int ADD_BUFF_TO_SN = 22;
    [EditorEnum( AIEvent.NO_TARGET, "给自己添加战场BUFF(自适应BUFF)" )]
    public const int ADD_BATTLEFIELD_BUFF_ON_SELF = 45;

    [EditorEnum( AIEvent.NO_TARGET, "召唤怪物", "XY如果为0,就在身边召唤" )]
    public const int SUMMON = 4;
    [EditorEnum( AIEvent.NO_TARGET, "召唤地图对象", "XY如果为0,就在身边刷" )]
    public const int SUMMON_MAP_OBJECT = 8;
    [EditorEnum( AIEvent.NO_TARGET, "显示指定SN角色", "为1显示,为0隐藏" )]
    public const int SHOW_TARGET = 17;
    [EditorEnum( AIEvent.NO_TARGET, "显示指定SN地图对象", "为1显示,为0隐藏" )]
    public const int SHOW_MAPOBJECT = 18;

    [EditorEnum( AIEvent.NO_TARGET, "移动到目标点" )]
    public const int MOVE = 15;
    [EditorEnum( AIEvent.NO_TARGET, "移动到指定SN" )]
    public const int MOVE_TO_SN = 41;
    [EditorEnum( AIEvent.NO_TARGET, "等待移动完成,并设置朝向" )]
    public const int WAIT_MOVE = 26;
    [EditorEnum( AIEvent.NO_TARGET, "传送到目标点" )]
    public const int TELEPORT = 20;
    [EditorEnum( AIEvent.HAS_TARGET, "逃跑" )]
    public const int ESCAPE = 10;
    [EditorEnum( AIEvent.NO_TARGET, "设置移动路径" )]
    public const int SET_PATROL_PATH = 32;
    [EditorEnum( AIEvent.NO_TARGET, "移动到下个路径点" )]
    public const int MOVE_TO_NEXT_PATROL = 42;

    [EditorEnum( AIEvent.NO_TARGET, "给指定SN角色发消息" )]
    public const int SEND_MSG = 11;
    [EditorEnum( AIEvent.NO_TARGET, "向队友发消息", "接消息的处理在队友身上配" )]
    public const int TEAM_MSG = 9;

    [EditorEnum( AIEvent.NO_TARGET, "设置阵营", "阵营A:0/B:1/C:2 全友好99" )]
    public const int CAMPS = 25;
    [EditorEnum( AIEvent.NO_TARGET, "设置战场阵营", "攻方阵营:0 守防阵营:1" )]
    public const int BATTLEFIELD_CAMPS = 47;
    [EditorEnum( AIEvent.NO_TARGET, "开始情节表演" )]
    public const int PERFORMANCE = 12;
    [EditorEnum( AIEvent.NO_TARGET, "设置表演模式", "表演模式1为开启0关闭" )]
    public const int PERFORMANCE_MODE = 13;
    [EditorEnum( AIEvent.NO_TARGET, "设置怪物活动时间" )]
    public const int SET_ACTIVE_CLICK = 33;
    [EditorEnum( AIEvent.NO_TARGET, "设置保护HP" )]
    public const int SET_PROTECT_HP = 28;
    [EditorEnum( AIEvent.NO_TARGET, "设置拥有者为最大仇恨者" )]
    public const int SET_OWNER_FROM_HATELIST = 29;
    [EditorEnum( AIEvent.NO_TARGET, "播放动作", "为1开始,为0结束" )]
    public const int ACTION = 23;

    [EditorEnum( AIEvent.NO_TARGET, "设置地图状态" )]
    public const int MAP_STATE = 14;
    [EditorEnum( AIEvent.NO_TARGET, "地图状态递增" )]
    public const int MAP_STATE_INC = 21;
    [EditorEnum( AIEvent.NO_TARGET, "发送地图信息", "0当前地图,1当前地图帮助信息,2全服信息" )]
    public const int SEND_SYS_MSG = 19;
    [EditorEnum( AIEvent.NO_TARGET, "副本完成/失败", "为1完成,为0失败" )]
    public const int DUNGEON_CONTROL = 24;
    [EditorEnum( AIEvent.NO_TARGET, "设置/清除地图拥有权", "为1设置,为0清除" )]
    public const int SET_MAP_OWNER = 38;

    [EditorEnum( AIEvent.NO_TARGET, "重置所有玩家位置" )]
    public const int RESET_ALL_PLAYER_POS = 31;
    [EditorEnum( AIEvent.NO_TARGET, "全部玩家任务" )]
    public const int PLAYER_QUEST = 34;

    [EditorEnum( AIEvent.NO_TARGET, "添加定时器" )]
    public const int ADD_TIMER = 35;
    [EditorEnum( AIEvent.NO_TARGET, "切换AI" )]
    public const int SET_AI = 36;
    [EditorEnum( AIEvent.NO_TARGET, "清除所有定时器" )]
    public const int CLEAR_ALL_TIMER = 37;

    [EditorEnum( AIEvent.NO_TARGET, "设置主动攻击模式", "为1设置,为0清除" )]
    public const int SET_AUTO_ATTACK_MODE = 39;

    [EditorEnum( AIEvent.HAS_TARGET, "给击杀者加战场积分" )]
    public const int ADD_BATTLEFIELD_SCORE_TO_KILLER = 43;
    [EditorEnum( AIEvent.NO_TARGET, "检查迷失幻林任务" )]
    public const int CHECK_DUNGEON_QUEST = 46;
    [EditorEnum( AIEvent.NO_TARGET, "发送据点阵营变化消息" )]
    public const int SEND_COUNTRY_WAR_NPC_LIST = 48;

    //用来记新增时的ID,新增后,这个递增
    public const int MAX_AI_ACTION = 49;
}

public class MonsterLevel {
    [EditorEnum( "难度一" )]
    public const int LEVEL1 = 0;
    [EditorEnum( "难度二" )]
    public const int LEVEL2 = 1;
    [EditorEnum( "难度三" )]
    public const int LEVEL3 = 2;
    [EditorEnum( "难度四" )]
    public const int LEVEL4 = 3;
    [EditorEnum( "难度五" )]
    public const int LEVEL5 = 4;
    [EditorEnum( "难度六" )]
    public const int LEVEL6 = 5;
    [EditorEnum( "难度七" )]
    public const int LEVEL7 = 6;
    [EditorEnum( "难度八" )]
    public const int LEVEL8 = 7;
    [EditorEnum( "难度九" )]
    public const int LEVEL9 = 8;
    [EditorEnum( "难度十" )]
    public const int LEVEL10 = 9;
}
/*具体分类需求如下：
1、怪物分为三类,分别为：动物型怪物、人型怪物、特殊型怪物
        每一类有三个级别的头像，分别为：普通、精英、BOSS
2、NPC分为两类，分别为：功能类NPC、场景类NPC
*/
public partial class MonsterType {
    [EditorEnum( "功能角色" )]//[EditorEnum( "功能类NPC" )]
    public const int NPC_FUNCTIONAL = 0;
    [EditorEnum( "平民" )]//[EditorEnum( "场景类NPC" )]
    public const int NPC_SCENE = 1;

    [EditorEnum( "兽类喽啰" )]//[EditorEnum( "普通动物型" )]
    public const int NORMAL_ANIMAL = 0x10;
    [EditorEnum( "人型喽啰" )]//[EditorEnum( "普通人型" )]
    public const int NORMAL_HUMAN = 0x11;
    [EditorEnum( "特殊喽啰" )]//[EditorEnum( "普通特殊型" )]
    public const int NORMAL_SPECIAL = 0x12;

    [EditorEnum( "兽类精英" )]//[EditorEnum( "精英动物型" )]
    public const int ELITE_ANIMAL = 0x20;
    [EditorEnum( "人型精英" )]//[EditorEnum( "精英人型" )]
    public const int ELITE_HUMAN = 0x21;
    [EditorEnum( "特殊精英" )]//[EditorEnum( "精英特殊型" )]
    public const int ELITE_SPECIAL = 0x22;

    [EditorEnum( "del兽类精英" )]//[EditorEnum( "特殊精英动物型" )]
    public const int S_ELITE_ANIMAL = 0x30;
    [EditorEnum( "del人型精英" )]//[EditorEnum( "特殊精英人型" )]
    public const int S_ELITE_HUMAN = 0x31;
    [EditorEnum( "del特殊精英" )]//[EditorEnum( "特殊精英特殊型" )]
    public const int S_ELITE_SPECIAL = 0x32;

    [EditorEnum( "兽类统领" )]//[EditorEnum( "BOSS动物型" )]
    public const int BOSS_ANIMAL = 0x40;
    [EditorEnum( "人型统领" )]//[EditorEnum( "BOSS人型" )]
    public const int BOSS_HUMAN = 0x41;
    [EditorEnum( "特殊统领" )]//[EditorEnum( "BOSS特殊型" )]
    public const int BOSS_SPECIAL = 0x42;

    private const int NORMAL_ = 0x10;
    private const int ELITE_ = 0x20;
    private const int S_ELITE_ = 0x30;
    private const int BOSS_ = 0x40;

    public static bool IsNormal(int v) {
        return ( v & 0xF0 ) == NORMAL_;
    }
    public static bool IsElite(int v) {
        return ( v & 0xF0 ) == ELITE_;
    }
    public static bool IsSElite(int v) {
        return ( v & 0xF0 ) == S_ELITE_;
    }
    public static bool IsBoss(int v) {
        return ( v & 0xF0 ) == BOSS_;
    }
    public static bool IsNpc(int v) {
        return v == NPC_FUNCTIONAL || v == NPC_SCENE;
    }
}

public class PVPMode {
    public const int PEACE = 0;
    public const int MASSACRE = 1;
}

public class CampType {
    [EditorEnum("玩家阵营")]
    public const int A = 0;
    [EditorEnum("怪物物件阵营")]
    public const int B = 1;
    [EditorEnum("全部友好阵营")]
    public const int C = 100;

    public const int MAX_PLAYER_CAMP = 3;

    [EditorEnum("玩家友好阵营")]
    public const int D = 101;
    [EditorEnum( "阵营五" )]
    public const int E = 4;
    [EditorEnum( "阵营六" )]
    public const int F = 5;

    public const int DEFAULT_MONSTER_CAMP = D;
    
    [EditorEnum( "全部友好" )]
    public const int FRIEND = 99;
    
    public static string GetCampName( int camp ) {
        switch ( camp ) {
        case A: return "蜀";
        case B: return "魏";
        case C: return "吴";
        default: return "";
        }
    }
}

public class MapType {
    [EditorEnum("常规")]
    public const int NORMAL = 0;
    [EditorEnum("副本")]
    public const int DUNGEON = 1;
}

public class DungeonType
{
    [EditorEnum("普通")]
    public const int NORMAL = 0;
    [EditorEnum("困难")]
    public const int HARD = 1;
    [EditorEnum("英雄")]
    public const int HERO = 2;
    [EditorEnum("史诗")]
    public const int EPIC = 3;
    [EditorEnum("个人")]
    public const int Single = 4;
}


public class CastTargetType {
    [EditorEnum( "目标" )]
    public const int TARGET = 0;
    [EditorEnum( "自己" )]
    public const int SELF = 1;
}

public class EffectTargetType {
    [EditorEnum( "敌人" )]
    public const int ENEMY = 0;
    [EditorEnum( "友军" )]
    public const int FRIEND = 1;
    [EditorEnum( "队友" )]
    public const int TEAM = 3;
}

public class TargetFilter {
    [EditorEnum( "无" )]
    public const int NONE = 0;
    [EditorEnum( "对玩家无效" )]
    public const int PLAYER = 1;
    [EditorEnum( "对NPC无效" )]
    public const int NPC = 2;
}

public class ProductSkillType {
    [EditorEnum( "无" )]
    public const int NONE = 0;
    [EditorEnum( "采集矿石" )]
    public const int MINING = 1;
    [EditorEnum( "采集药草" )]
    public const int HERBGATHERING = 2;
    [EditorEnum( "丹药炼制" )]
    public const int PHARMACY = 3;
    [EditorEnum( "装备锻造" )]
    public const int FORGING = 6;
    [EditorEnum( "裁缝制衣" )]
    public const int SEWING = 7;
    [EditorEnum( "饰品打造" )]
    public const int JEWELERY = 8;
    [EditorEnum( "通用合成" )]
    public const int REFINERY = 9;
}

public class ProductSkillSubType {
    [EditorEnum( "无" )]
    public const int NONE = 0;
    [EditorEnum( ProductSkillType.PHARMACY, "灵丹妙药" )]
    public const int MED_MEDICINES = 301;
    [EditorEnum( ProductSkillType.PHARMACY, "色料炼制" )]
    public const int MED_MAKE = 302;
    [EditorEnum( ProductSkillType.PHARMACY, "生命丹药" )]
    public const int MED_HP = 303;
    [EditorEnum( ProductSkillType.PHARMACY, "精气丹药" )]
    public const int MED_MP = 304;

    [EditorEnum( ProductSkillType.SEWING, "布料纺织" )]
    public const int SW_MAKE_CLOTH = 601;
    [EditorEnum( ProductSkillType.SEWING, "披风刺绣" )]
    public const int SW_EMBROIDER = 602;
    [EditorEnum( ProductSkillType.SEWING, "缝纫•谋士" )]
    public const int SW_CLOTHING_MAGE = 603;
    [EditorEnum( ProductSkillType.SEWING, "缝纫•术士" )]
    public const int SW_CLOTHING_WARLOCK = 604;

    [EditorEnum( ProductSkillType.JEWELERY, "雕饰制作" )]
    public const int JEW_FOIL = 701;
    [EditorEnum( ProductSkillType.JEWELERY, "精工•项链" )]
    public const int JFW_NECKLACE = 702;
    [EditorEnum( ProductSkillType.JEWELERY, "精工•戒指" )]
    public const int JFW_RING = 703;
    [EditorEnum( ProductSkillType.JEWELERY, "精工•配饰" )]
    public const int JFW_MISC = 704;

    [EditorEnum( ProductSkillType.FORGING, "金属冶炼" )]
    public const int FG_SMELT = 801;
    [EditorEnum( ProductSkillType.FORGING, "武器打造" )]
    public const int FG_WEAPON = 802;
    [EditorEnum( ProductSkillType.FORGING, "铸甲•力士" )]
    public const int FG_ARMOUR_WARRIOR = 803;
    [EditorEnum( ProductSkillType.FORGING, "铸甲•侠士" )]
    public const int FG_ARMOUR_CAVALIER = 804;

    [EditorEnum( ProductSkillType.REFINERY, "好运符兑换" )]
    public const int REFINERY_LUCKY = 903;
    [EditorEnum( ProductSkillType.REFINERY, "材料兑换" )]
    public const int REFINERY_MATERIALS = 904;
    [EditorEnum( ProductSkillType.REFINERY, "宝石兑换" )]
    public const int REFINERY_GEM_EX = 905;
    [EditorEnum( ProductSkillType.REFINERY, "宝石合成" )]
    public const int REFINERY_GEM = 906;
    [EditorEnum( ProductSkillType.REFINERY, "晶石合成" )]
    public const int REFINERY_CRYSTAL = 907;
    [EditorEnum( ProductSkillType.REFINERY, "珍材合成" )]
    public const int REFINERY_VALUEABLE = 908;
    [EditorEnum( ProductSkillType.REFINERY, "星魂合成" )]
    public const int REFINERY_STARSOUL = 909;
    [EditorEnum( ProductSkillType.REFINERY, "其他合成" )]
    public const int REFINERY_OTHER = 910;
    [EditorEnum( ProductSkillType.REFINERY, "活动兑换" )]
    public const int REFINERY_MARKET = 911;    
}

public class ProductThirdType {
    [EditorEnum( "无" )]
    public const int NONE = 0;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM, "金刚石" )]
    public const int REFINERY_JINGANG = 1;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM, "月光石" )]
    public const int REFINERY_MOONLIGHT = 2;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM, "海蓝石" )]
    public const int REFINERY_SEABLUE = 3;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM, "虎晴石" )]
    public const int REFINERY_TIGGER = 4;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM, "天青石" )]
    public const int REFINERY_SKYCYAN = 5;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM, "孔雀石" )]
    public const int REFINERY_PEACOCK = 6;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM, "金玉" )]
    public const int REFINERY_GOLDJADES = 7;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM, "温玉" )]
    public const int REFINERY_WARMJADES = 8;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM, "白玉" )]
    public const int REFINERY_WHITEJADES = 9;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM, "红玉" )]
    public const int REFINERY_REDJADES = 10;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM, "紫玉" )]
    public const int REFINERY_PURPLEJADES = 11;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM, "青玉" )]
    public const int REFINERY_CYANJADES = 12;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM, "翡翠" )]
    public const int REFINERY_BUZZON = 13;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM, "碧玺" )]
    public const int REFINERY_COLOREDSTONE = 14;

    [EditorEnum( ProductSkillSubType.REFINERY_GEM, "琥珀" )]
    public const int REFINERY_HUPO = 15;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM, "水晶" )]
    public const int REFINERY_CRYSTAL = 16;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM, "曜石" )]
    public const int REFINERY_OBSIDIAN = 17;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM, "玛瑙" )]
    public const int REFINERY_MANO = 18;

    [EditorEnum( ProductSkillSubType.REFINERY_GEM_EX, "金刚石" )]
    public const int REFINERY_JINGANG_EX = 51;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM_EX, "月光石" )]
    public const int REFINERY_MOONLIGHT_EX = 52;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM_EX, "海蓝石" )]
    public const int REFINERY_SEABLUE_EX = 53;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM_EX, "虎晴石" )]
    public const int REFINERY_TIGGER_EX = 54;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM_EX, "天青石" )]
    public const int REFINERY_SKYCYAN_EX = 55;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM_EX, "孔雀石" )]
    public const int REFINERY_PEACOCK_EX = 56;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM_EX, "金玉" )]
    public const int REFINERY_GOLDJADES_EX = 57;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM_EX, "温玉" )]
    public const int REFINERY_WARMJADES_EX = 58;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM_EX, "白玉" )]
    public const int REFINERY_WHITEJADES_EX = 59;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM_EX, "红玉" )]
    public const int REFINERY_REDJADES_EX = 60;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM_EX, "紫玉" )]
    public const int REFINERY_PURPLEJADES_EX = 61;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM_EX, "青玉" )]
    public const int REFINERY_CYANJADES_EX = 62;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM_EX, "翡翠" )]
    public const int REFINERY_BUZZON_EX = 63;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM_EX, "碧玺" )]
    public const int REFINERY_COLOREDSTONE_EX = 64;

    [EditorEnum( ProductSkillSubType.REFINERY_GEM_EX, "琥珀" )]
    public const int REFINERY_HUPO_EX = 65;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM_EX, "水晶" )]
    public const int REFINERY_CRYSTAL_EX = 66;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM_EX, "曜石" )]
    public const int REFINERY_OBSIDIAN_EX = 67;
    [EditorEnum( ProductSkillSubType.REFINERY_GEM_EX, "玛瑙" )]
    public const int REFINERY_MANO_EX = 68;

    [EditorEnum( ProductSkillSubType.REFINERY_MATERIALS, "金属锭" )]
    public const int REFINERY_MATERIALS_01 = 70;
    [EditorEnum( ProductSkillSubType.REFINERY_MATERIALS, "雕饰" )]
    public const int REFINERY_MATERIALS_02 = 71;
    [EditorEnum( ProductSkillSubType.REFINERY_MATERIALS, "布卷" )]
    public const int REFINERY_MATERIALS_03 = 72;
    [EditorEnum( ProductSkillSubType.REFINERY_MATERIALS, "色料" )]
    public const int REFINERY_MATERIALS_04 = 73;

    [EditorEnum( ProductSkillSubType.REFINERY_MARKET, "春节活动" )]
    public const int REFINERY_MARKET_01 = 90;
    
}

// 聊天频道
public class ChatChannelMiniTime {
    public const int SCREEN = 3;      // 当前频道
    public const int GROUP = 0;       // 军团频道
    public const int CAMP = 30;       // 阵营频道
    public const int MAP = 30;        // 地图频道
    public const int SERVER = 180;    // 全服
    public const int TEAM = 0;        // 组队
    public const int BATTLEFIELD = 3; // 战场
    public const int GROUP_LEAGUE = 3;// 军团联盟
}

public class ChatChannelType {
    public const int PRIVATE = 0;    // 私聊
    public const int SCREEN = 1;    // 当前频道
    public const int TEAM = 2;    // 队伍频道
    public const int GROUP = 3;    // 军团频道
    public const int CAMP = 4;    // 陈营频道
    public const int MAP = 5;    // 地图频道
    public const int SERVER = 6;    // 小喇叭
    public const int IM = 7;    // IM聊天
    public const int BATTLEFIELD = 8;    // 战场
    public const int DUNGEON_ROOM = 9;    // 副本等待
    public const int GROUP_LEAGUE = 10;
}

public class DungeonScoreType {
    public const int TOTAL = 0;    //总得分
    public const int PLAYER_DIE_COUNT = 1;    //玩家死亡次数
    public const int USE_TIME = 2;    //完成副本时间
    public const int TRIGGE_TRIP = 3;    //触发机关
    public const int USE_ITEM = 4;    //使用道具计分
    public const int KILL_NPC = 5;    //击杀NPC
    public const int COLLECT_ITEM = 6;    //收集道具
    public const int KILL_BOSS = 7;    //击杀boss
    public const int USE_SKILL = 8;    //使用技能
    public const int NOT_USE_ITEM = 9;    //不使用某道具
    public const int ESCORT_NPC = 10;   //被保护NPC到达指定区域
    public const int PERFORMANCE = 11;   //情节触发
    public const int PLAYER_COUNT = 12;   //玩家数量
}

// 四种天书类型，取值不可再更改了！！
// 因为存储的时候是按这个为索引的
public class FastnessType {
    public const int FastnessPhsAtt = 1;    //物理伤害
    public const int FastnessMagAtt = 2;    //法术伤害
    public const int FastnessPhsDef = 3;    //物理防御
    public const int FastnessMagDef = 4;    //法术防御
}

public class C2SEvent {
    public const int Accept = 100;
    public const int Disconnect = 101;
    public const int RecvData = 102;
}

// 协议ID定义
public partial class Protocol {
    //系统消息,子协议如果是系统消息,必须大于此值
    public const int SYSTEM = 500;
    public const int MAX_PROTOCOL = 1000;


    #region 任务相关的协议定义，废弃不用！！
#if false
    public struct Quest {
        public const int Main	           = 55;
        public const int Accept            = 1;    // 玩家从NPC处接任务
        public const int Submit            = 2;    // 玩家向NPC交任务
        public const int Cancel            = 3;    // 放弃任务        

        public const int MonsterKilled     = 5;
        public const int CheckGoto         = 6;    // 客户端请求检查一下到达条件
        public const int GotoCompleted     = 7;    // 服务器通知客户端
        public const int QuestFailed       = 8;    // 服务器通知：某个任务已失败
        //public const int CheckStatus       = 9;
        public const int RemoveQuest       = 10;   // 服务器通知，从玩家身上移除任务
        public const int QuestTrace        = 11;   // 任务追踪
        public const int QuestSetCompleted = 12;
        public const int ItemUsed          = 13;
        public const int Ai                = 14;
        public const int MapobjTriggered   = 15;

        public const int QuestQuestion     = 16;     // 答题协议
        public const int MonsterKilledLevelRange = 17;  // 攻能一种等级的怪

        // 失败原因
        public enum QuestFailReason {
            Unknown,		// 未知
            Timeout,		// 超时
            PlayerDead		// 玩家死亡
        }
    }
#endif
    #endregion

    #region 关系相关的协议，废弃不用，暂时不删
    public struct Relation {
        public const int Main = 115;
        public const int AddInvite = 1;        // 添加关系的邀请
        public const int EchoAdd = 2;        // 响应添加关系的消息
        public const int AddByWhole = 4;        // 成功添加好友/师徒/结拜/情侣/夫妻
        public const int Delete = 5;        // 删除成员
        public const int SyncData = 6;        // 同步关系成员给客户端
        public const int QueryInfo = 8;        // 查询信息
        public const int OnlineStateChanged = 12;       // 上下线通知消息
        public const int AddByName = 13;       // 根据名字加好友
        public const int FamiliarityChange = 14;       // 复杂关系的亲密度改变
    }
    #endregion



    // 摊位相关协议定义
    public static partial class Stall {

        #region 废弃不用了
#if false
        public const int Main               = 100;   // 主ID
        public const int Create             = 1;     // 创建摊位
        public const int CloseStall         = 2;     // 收摊
        public const int AddItem            = 3;     // 物品上架
        public const int FetchItem          = 4;     // 物品下架
        public const int ChangeInfo         = 5;     // 修改摊位信息
        public const int ChangePrice        = 6;     // 修改价格
        public const int GetGoods           = 7;     // 买家取摊位货物列表
        public const int Buy                = 8;     // 买指定货物
#endif
        #endregion

        public enum ErrorCode {
            [Describe( "成功" )]
            Ok = 0,
            [Describe( "内部错误" )]
            Internal,
            [Describe( "级别不足" )]
            LevelNotEnough,
            [Describe( "此摊位已有主了" )]
            StallAlreadyHasOwner,
            [Describe( "已在摆摊中" )]
            AlreadyStalling,
            [Describe( "无效的货物" )]
            InvalidGoods,
            [Describe( "无效的摆摊时间" )]
            InvalidTime,
            [Describe( "摊位名字长度超限或其中含有不被允许的字符" )]
            InvalidStallName,
            [Describe( "摊位吆喝语长度超限或其中含有不被允许的字符" )]
            InvalidShopSign,
            [Describe( "金钱不足" )]
            NotEnoughMoney_ForCreate,
            [Describe( "活力不足" )]
            NotEnoughVigour_ForCreate,
            [Describe( "无效的NPC" )]
            InvalidNPC,
            [Describe( "太远了" )]
            TooFar,
            [Describe( "主角已死亡" )]
            PlayerDead,
            [Describe( "未知错误" )]
            Generic,
            [Describe( "不是摊位的所有者" )]
            NotOwner,
            [Describe( "无效的价格" )]
            InvalidPrice,
            [Describe( "无效的数量" )]
            InvalidCount,
            [Describe( "买家背包空间不足" )]
            NotEnoughBag,
            [Describe( "买家金钱不足" )]
            NotEnoughMoney_ForBuyer,
            [Describe( "卖家已离线" )]
            OwnerNotExists,
            [Describe( "金钱上溢出（卖家钱太多）" )]
            MoneyOverflow,
            [Describe( "当前不能摆摊" )]
            CannotNow,
            [Describe( "摆摊时间已到" )]
            TimeOut,
            [Describe( "该物品不能被交易" )]
            ItemNotTradable,
            [Describe("卖家处于防沉迷状态，不能出售物品")]
            SellerInAntiAddiction,
            [Describe("买家处于沉迷状态，无法购买")]
            BuyerInAntiAddiction
        }
    }

    #region 邮件相关协议，废弃不用
#if false
    public struct Mail {
        public const int Main                = 105;
        public const int MailCount = 1;  // 邮件数量
        public const int PostMail = 3;  // 发信件相关
        public const int MailDigestList = 4;  // 邮件摘要列表
        public const int Content             = 5;  // 指定邮件内容
        public const int Drop                = 6;  // 删除邮件
        public const int TakeAttachment      = 7;  // 收取附件
        public const int GmInfo              = 9;  // GM命令请求相关信息
        public const int HasUnread           = 10; // 服务器通知客户端，有未读邮件
    }

    public struct S2SDBMail {
        public const int Main                = 110 + SYSTEM;
        public const int CheckReceiver = 2;
        public const int PostMail            = 3;
        public const int Drop                = 6;
        public const int TakeAttachment      = 7;  // 收取附件
        public const int CheckNewMail        = 8;  // 检查是否有新邮件
        public const int NotifyReceiver      = 9;  // 通知收件人
        //public const int CreateCache         = 10;
        public const int SetToReaded         = 11;
        public const int HasUnread           = 12; 
    }
#endif
    #endregion

}

public enum GroupOpertionType {
    Dismiss,
    Invite,
    War,
    Kick,
    SetLeader,
    SetViceLeader,
    SetManager,
    SetMember,
    SetTitle,
    SetNotice,
    LevelUp,
    Quest,
    Building,
}

// 响应邀请类型
public class EchoInviteType {
    public const byte Trade = 1;        // 交易
    public const byte Team = 2;        // 组队
    public const byte Group = 3;       // 军团
    public const byte PVP = 4;         // PVP
    public const byte InviteGroupLeague = 5; // 军团联盟邀请
    public const byte ApplyGroupLeague = 6; // 军团联盟申请
}

// 响应邀请
public class EchoInvite {
    public const byte ALLOW = 1;     // 允许
    public const byte REFUSE = 2;    // 拒绝
}


// P2P交易相关部分

public class P2PTradeState {
    public const int NONE = 0;    // 正常状态
    public const int INVITE = 1;    // 邀请状态
    public const int INVITED = 2;    // 被邀请状态
    public const int AGREED = 3;    // 同意状态
    public const int ITEMLOCKED = 4;    // 道具锁定状态
    public const int ITEMTRADE = 5;    // 成交状态
}

public class ItemConfigExFunType {
    [EditorEnum( "藏宝图" )]
    public const int ICF_Treasury = 1;

    [EditorEnum( "随机道具" )]
    public const int ICF_RandomItem = 2;
}

/// <summary>
/// 任务接取时先决条件
/// </summary>
public class QuestRequestConditionType {
    [EditorEnum( "摇钱树满值可接" )]
    public const int SourceOfMoney = 1;
    //[EditorEnum("情侣亲密度")]
    //public const int LoverFami = 2;
    [EditorEnum("领取需防外挂验证")]
    public const int NeedVerify = 3;
    [EditorEnum( "军团等级" )]
    public const int GroupLevel = 4;
    [EditorEnum("前置任务(或)")]
    public const int MultiBeforeQuest = 5;
}

/// <summary>
/// 任务接取时行为类型
/// </summary>
public class QuestAcceptActionType {
    [EditorEnum("添加BUFF")]
    public const int AddBuff = 1;

    [EditorEnum("传送")]
    public const int Teleport = 2;
}

/// <summary>
/// 任务完成条件、动作
/// </summary>
public class QuestConditionType {
    [EditorEnum("运镖")]
    public const int QCT_Carriage = 1;

    [EditorEnum( "杀等级怪" )]
    public const int QCT_KillMonsterLvlRange = 2;

    [EditorEnum( "随机使用道具" )]
    public const int QCT_RandomUseItem = 3;

    [EditorEnum( "AI触发" )]
    public const int QCT_AI = 4;

    [EditorEnum( "执行动作" )]
    public const int QCT_ACTION = 5;

    [EditorEnum("访问网址")]
    public const int QCT_OPEN_URL = 6;

    [EditorEnum( "完成后-传送" )]
    public const int QCT_QE_Teleport = 101;

    [EditorEnum( "完成后-发送邮件给师傅" )]
    public const int QCT_QE_Email = 102;
    
    [EditorEnum("完成后-解除师徒关系")]
    public const int QCT_QE_RelationMaster = 103;

    [EditorEnum("完成后-发送邮件给自己")]
    public const int QCT_QE_SelfEmail = 104;

    [EditorEnum("完成后-传送绑定地图")]
    public const int QCT_QE_TeleportBind = 105;
    
    [EditorEnum("战场完成")]
    public const int QCT_BattlefieldComplete = 106;

    // Max 105
}

// 军团建筑类型
public class GroupBuildingType {
    [EditorEnum( "结义堂" )]
    public const int Core = 0;  // 结义堂
    [EditorEnum( "资源作坊" )]
    public const int Res = 1;   // 资源作坊
    [EditorEnum( "城防 " )]
    public const int Defense = 2;   // 城防 

    public const int Max = 3;
}

public class QuestGroupType {
    [EditorEnum( "非发布" )]
    public const int NoGroup = 0;

    [EditorEnum( "仅发布" )]
    public const int PutOut = 2;

    [EditorEnum( "任务+发布" )]
    public const int All = 3;
}

public class QuestCleanType {
    [EditorEnum( "放弃不完成" )]
    public const int NORMAL = 0;

    [EditorEnum( "放弃完成1次" )]
    public const int CompletedOne = 1;

    [EditorEnum( "放弃完成所有" )]
    public const int CompletedAll = 2;
}

public class QuestType {
    [EditorEnum( "普通任务" )]
    public const int NORMAL = 0;
    [EditorEnum( "每日任务" )]
    public const int DAILY = 1;
    [EditorEnum( "活动任务" )]
    public const int HOLIDAY = 2;

    //[EditorEnum( "师徒任务")]
    //public const int MASTER  = 3;
    //[EditorEnum( "情侣任务")]
    //public const int LOVERS  = 4;
    //[EditorEnum( "结拜任务")]
    //public const int BROTHERS = 5;
    //[EditorEnum( "好友任务")]
    //public const int FRIENDS = 6;

}

public class QuestGroup {
    [EditorEnum( "主线" )]
    public const int MAIN = 0;
    [EditorEnum( "支线" )]
    public const int BRANCH = 5;
    [EditorEnum( "日常" )]
    public const int DAILY = 10;
    [EditorEnum( "活动" )]
    public const int HOLIDAY = 15;
    [EditorEnum( "军团" )]
    public const int ARMY_GROUP = 20;

    [EditorEnum( "家族" )]
    public const int FAMILY = 22;
    [EditorEnum( "阵营" )]
    public const int CAMP = 23;

    [EditorEnum( "师徒" )]
    public const int MASTER = 25;
    [EditorEnum( "循环" )]
    public const int CYCLE = 30;
    [EditorEnum( "副本" )]
    public const int DUNGEON = 35;
    [EditorEnum( "情侣" )]
    public const int LOVER = 38;
    [EditorEnum( "夫妻" )]
    public const int SPOUSES = 40;
    [EditorEnum( "结拜" )]
    public const int BROTHER = 45;

    //[EditorEnum("福利")]
    //public const int BOON = 50;
}

public class DescribeAttribute : Attribute {
    public readonly string Name;
    public DescribeAttribute(string name) {
        this.Name = name;
    }
#if !SERVER
    public readonly string Data;
    public readonly int Tag;
    public static readonly DescribeAttribute Empty = new DescribeAttribute( string.Empty, string.Empty, 0 );
    public DescribeAttribute(string name, string data)
        : this( name ) {
        this.Data = data;
    }
    public DescribeAttribute(string name, int tag)
        : this( name ) {
        this.Tag = tag;
    }
    public DescribeAttribute(string name, string data, int tag)
        : this( name, data ) {
        this.Tag = tag;
    }
#endif
}


public enum AcceptQuestResult {
    [Describe( "未知错误" )]
    Unknown = -1,

    [Describe( "接任务成功" )]
    Ok = 0,

    [Describe( "已接任务数已达上限" )]
    TooManyQuest,

    [Describe( "没有这个任务" )]
    QuestNotExists,

    [Describe( "没有这个NPC" )]
    NpcNotExists,

    [Describe( "任务不是在这里接" )]
    NpcIsNotStartNpc,

    [Describe( "距离过远" )]
    CanNotTrade,

    [Describe( "玩家级别过低" )]
    PlayerLevelTooLow,

    [Describe( "玩家级别过高" )]
    PlayerLevelTooHigh,

    [Describe( "这个任务已经接过了" )]
    AlreadyAccepted,

    [Describe( "这个任务已经完成过了" )]
    AlreadyCompleted,

    [Describe( "其它错误" )]
    Other,

    [Describe( "玩家性别不能接这个任务" )]
    IncorrectGender,

    [Describe( "玩家职业不能接这个任务" )]
    IncorrectOccupation,

    [Describe( "金线不足" )]
    IncorrectMoney,

    [Describe( "缺少必要道具" )]
    IncorrectItem,

    [Describe( "前置任务尚未完成" )]
    RequireBeforeQuest,

    [Describe( "背包空间不足" )]
    BagFull,

    [Describe( "当前不能接任务" )]
    CanNotNow,

    [Describe( "尚未习得相应的生活技能" )]
    NoProductSkill,

    [Describe( "现在不是活动开启的时间段" )]
    NotInHolidayRange,

    [Describe( "任务需要特定关系" )]
    IncorrectRelation,

    [Describe( "玩家阵营不能接这个任务" )]
    IncorrectCamp,

    [Describe( "已接任务与欲接任务互斥" )]
    HasMutex,

    [Describe( "没有军团，无法接取军团任务" )]
    GroupNo,

    [Describe( "任务发布已过期，接取任务失败" )]
    GroupNoQuest,

    [Describe( "无法接取军团任务，该任务只能由军团长或是副军团长接取" )]
    GroupPower,

    [Describe( "无法接取军团任务，军团等级不够" )]
    GroupLvl,

    [Describe( "无法接取军团任务，军团摇钱树还没有成熟" )]
    GroupSourceMoney,

    [Describe( "无法接取军团任务，已到今日接取上限" )]
    GroupCount,

    [Describe("领取该任务须具有情侣关系")]
    NeedLover,

    [Describe("情侣亲密度不符合领取任务条件")]
    IncorrectLoverFami,

    [Describe("需要验证码")]
    NeedVerifyCode,

    [Describe("接取任务失败，已接取的镖车任务尚未完成")]
    Carriage,
}

public enum SubmitQuestResult {

    Empty = -1,

    [Describe( "交任务成功" )]
    Ok,

    [Describe( "没有这个任务" )]
    QuestNotExists,

    [Describe( "此任务尚未接到" )]
    QuestNotAccept,

    [Describe( "任务尚未完成" )]
    QuestNotComplete,

    [Describe( "没有这个NPC" )]
    NpcNotExists,

    [Describe( "任务不是在这里交" )]
    NpcIsNotEndNpc,

    [Describe( "距离过远" )]
    CanNotTrade,

    [Describe( "背包空间不足" )]
    BagFull,

    [Describe( "未选择装备奖励" )]
    NoAwardChoose,

    [Describe( "任务已失败" )]
    QuestAlreayFail,

    [Describe( "您的金钱超过上限" )]
    MoneyOverflow,

    [Describe( "现在不能进行交任务的操作" )]
    CanNotNow,

    [Describe( "现在不是活动开启的时间段" )]
    NotInHolidayRange,

    [Describe( "没有道具" )]
    NoItem,

    [Describe( "任务已超时" )]
    TimeOut,

    [Describe( "任务是军团任务，必须加入军团" )]
    HasGroup,
}

public enum CancelQuestResult {
    [Describe( "任务已成功放弃" )]
    Ok,
    [Describe( "没有这个任务" )]
    QuestNotExists,
    [Describe( "此任务尚未接到" )]
    QuestNotAccept,
    [Describe( "此任务已完成，不能放弃" )]
    QuestCompleted,
    [Describe( "现在不能进行此项操作" )]
    CanNotNow,
    [Describe( "这个任务是不可放弃的" )]
    CanNotCancel
}

// 活动名字，客户端用，便于显示
public class TaskHolidayType {
    [EditorEnum( "无" )]
    public const int THT_NONE = 0;

    [EditorEnum( "新手倒计时" )]
    public const int THT_NEWPLAYER = 1;

    [EditorEnum( "IB商城领取" )]
    public const int THT_IBSHOP = 2;

    [EditorEnum( "答题活动" )]
    public const int THT_QUESTION = 3;
}

// 任务条件奖励的条件定义
public class AwardCfgType {
    [EditorEnum( "无" )]
    public const int AC_NONE = 0;
    [EditorEnum( "等级" )]
    public const int AC_LEVEL = 1;
    [EditorEnum( "完成次数" )]
    public const int AC_REPEAT = 2;
}

// 答题协议返回的状态
public class AskState {
    public const int AS_NEXT = 0x01;    // Client提交给客户端（带答案），正常情况下，Server也返回给客户端（带题目）
    public const int AS_AWARD = 0x02;    // [Client]领取奖励
    public const int AS_NEXT_PHASE = 0x04;    // 下一轮答题

    public const int AS_ERROR_MODIFY = 0x08;    // [Client]答错，修正答案
    public const int AS_ERROR_REPEAT = 0x10;    // [Client]答错，再答一次
    public const int AS_ERROR_AWARD = 0x20;    // [Client]答错，领取上一次的奖励

    public const int AS_ERROR = 0x40;    // 答错
    public const int AS_UNKNOWN = 0x80;    // 未知错误
    public const int AS_OVER = 0x100;   // 本轮活动已完
}

// 编辑用，答题的类型
public class AskFunType {
    [EditorEnum( "一般问题" )]
    public const int Normal = 0;
    [EditorEnum( "问题集" )]
    public const int QSet = 1;

}

// 任务条件奖励
public class AwardFunType {
    [EditorEnum( "奖励道具(必奖)" )]
    public const int AddItem = 1;

    [EditorEnum( "奖励道具(随机)" )]
    public const int AddItemOr = 3;

    [EditorEnum( "奖励道具配置(随机)" )]
    public const int AddItemOrSetting = 4;

    [EditorEnum( "自动接取任务" )]
    public const int AutoTask = 2;

    [EditorEnum( "金钱奖励" )]
    public const int AddMoney = 5;

    [EditorEnum( "经验奖励" )]
    public const int AddExp = 6;
}

// 活动时间
public class TaskDayRange {
    [EditorEnum( "周一" )]
    public const int Monday = 1;
    [EditorEnum( "周二" )]
    public const int Tuesday = 2;
    [EditorEnum( "周三" )]
    public const int Wendesday = 3;
    [EditorEnum( "周四" )]
    public const int Thursday = 4;
    [EditorEnum( "周五" )]
    public const int Friday = 5;
    [EditorEnum( "周六" )]
    public const int Saturday = 6;
    [EditorEnum( "周日" )]
    public const int Sunday = 7;
}

// 关系类型定义
public class RelationshipType {
    public const int NONE = 0x00000000;
    public const int FRIENDS = 0x00000001;   // 好友关系
    [EditorEnum( "师父" )]
    public const int MASTER = 0x00000002;   // 师父
    [EditorEnum( "徒弟" )]
    public const int APPRENTICE = 0x00000004;   // 徒弟
    [EditorEnum( "结拜" )]
    public const int BROTHER = 0x00000008;   // 结拜
    [EditorEnum( "情侣" )]
    public const int LOVERS = 0x00000010;   // 情侣

    //[EditorEnum("夫妻")]
    //public const int SPOUSES        = 0x00000020;   // 夫妻

    public const int ENEMY = 0x00000040;   // 仇敌
    public const int BLACK = 0x00000080;   // 黑名单
}


//  客户端描述快捷栏元素的类型
public class ShortcutElementType {
    public const int PD_BASE = 0;
    public const int PD_EQUIP = 1;
    public const int PD_ITEM = 2;
    public const int PD_STOREHOUSE = 3;
    public const int PD_BUFF = 4;
    public const int PD_SKILL = 5;
    public const int PD_PRODUCTSKILL = 6;
    public const int PD_QUEST = 7;
    public const int PD_QUEST_MASK = 8;
    public const int PD_SHORTCUT = 9;
    public const int PD_EXT = 10;  //扩展数据
}

//金钱消耗类型
public class MoneyCostType {
    public const int Mail = 0;   //发邮件附件收钱
    public const int BuyStuff = 1;   //向npc购买东西
    public const int Stall = 2;   //摆摊收钱
    public const int EquipPolish = 3;   //装备开光
    public const int Teleport = 4;   //npc传送
    public const int SkillLearn = 5;   //学习技能
    public const int UpGradeSkill = 6;   //升级技能
    public const int UpGradeFastness = 7;//天书系统
    public const int OnProduct = 8;   //生活技能
    public const int DungeonCreate = 9;//副本创建
    public const int EquipRepair = 10;  //装备修理
    public const int GroupCreate = 11;  //军团创建
    public const int GroupLevelUp = 12;  //军团升级

    public const int EquipHole = 13;    // 装备开孔
    public const int EquipEnchase = 14;    // 装备镶嵌
    public const int EquipStar = 16;    // 装备升星
    public const int Gm = 17;   // GM操作
    public const int EquipRecoverHole = 18;    // 装备还原
    public const int UseItem = 19;  // 使用道具消耗
    public const int EquipUnEnchase = 20;    // 装备宝石取下

    public const int GroupLeagueCreate = 21;
    public const int MAX = 22;
}

public class MoneyGetType {
    public const int SellStuff = 0;  //向npc售卖东西
    public const int Quest = 1;  //任务得钱
    public const int UseItem = 2;  // 使用道具产生

    public const int MAX = 3;
}

// 角色引导类型
public class GuideActionType {
    public const int FirstLogin = 0;
    public const int WelcomeFreshman = 1;         // 新手欢迎界面
    public const int FirstOpenBaggagePanel = 2;         // 第一次打开背包界面
    public const int NoneUse = 3;         // 第一次显示任务追踪
    public const int FirstViewTaskInfo = 4;         // 第一次显示任务信息
    public const int FirstOpenSkillPanel = 5;         // 第一次打开技能界面
    public const int TaskNpc1_TaskListAccept = 6;         // 跟第一个任务Npc对话(接任务)
    public const int TaskNpc1_TaskInfoAccept = 7;            // 跟第一个任务Npc对话
    public const int TaskNpc1_TaskListSubmit = 8;            // 跟第一个任务Npc对话（交任务）
    public const int TaskNpc1_TaskInfoSubmit = 9;            // 跟第一个任务Npc对话
    public const int TaskNpc2_HeadIconAccept = 10;         // 第二个任务Npc头顶显示“可接任务”标志
    public const int TaskNpc3_HeadIconAccept = 11;         // 第三个任务Npc头顶显示“可接任务”标志
    public const int TaskNpc3_HeadIconAleadyAccept = 12;         // 第三个任务Npc头顶显示“任务已接”标志（灰色问号）
    public const int FirstMonsterQuestTrace = 13;             // 木桩，自动寻路
    public const int FirstShowSelectableAward = 14;             // 第一次显示可选奖励
    public const int TaskNpc4_TaskListAccept = 15;             // 第四个可接任务的Npc

    public const int ViewTaskTrace1 = 16;
    public const int ViewTaskTrace2 = 17;
    public const int ViewTaskTrace3 = 18;
    public const int ViewTaskTrace4 = 19;
    public const int ViewTaskTrace5 = 20;
    public const int ViewTaskTrace6 = 21;

    public const int TipsOnShortcutButton_Baggage_1 = 22;
    public const int TipsOnShortcutButton_Baggage_2 = 23;

    public const int TipsOnBaggageItemBox_1 = 24;
    public const int TipsOnBaggageItemBox_2 = 25;

    public const int Ui_Baggage_Btn_Sort = 26; // 背包界面上的“整理”按钮
    public const int Ui_Skill_Btn_Tianfu = 27; // 技能界面上的“天赋点”按钮
    public const int Ui_Task_Btn_Accept = 28;

    public const int Ib_Promotion_1 = 29;
    public const int Ib_Promotion_2 = 30;
    public const int Ib_Promotion_3 = 31;
    public const int Ib_Promotion_4 = 32;
    public const int Ib_Promotion_5 = 33;

    public const int FreshmanHelpSystem_Season2_LevelUp_5 = 34;    // 升到5级时的帮助界面
    public const int FreshmanHelpSystem_Season2_LevelUp_10 = 35;   // 升到10级时的帮助界面
    public const int FreshmanHelpSystem_Season2_tianfudian = 36;        // 天赋点
    public const int FreshmanHelpSystem_Season2_tianfudian_done = 37;   // 天赋点
    public const int FreshmanHelpSystem_Season2_p2ptrade_myitem = 38;
    public const int FreshmanHelpSystem_Season2_p2ptrade_money = 39;
    public const int FreshmanHelpSystem_Season2_p2ptrade_lock = 40;

    public const int FreshmanHelpSystem_Season2_close_help_window_level_10 = 41;

    public const int FreshmanHelpSystem_Season2_getitem_extend_baggage = 42;    // 获得了扩展背包道具
    public const int FreshmanHelpSystem_Season2_tip_point_item_extend_baggage = 43;    // Tips指向扩展背包道具
    public const int FreshmanHelpSystem_Season2_useitem_successfully_extend_baggage = 44;    // 扩展背包成功

    public const int FreshmanHelpSystem_Season2_live_skill1 = 45;    // 生活技能
    public const int FreshmanHelpSystem_Season2_live_skill2 = 46;    // 生活技能
    public const int FreshmanHelpSystem_Season2_live_skill3 = 47;    // 生活技能

    public const int FreshmanHelpSystem_Season2_LevelUp_11 = 48; // 升到11级

    public const int FreshmanHelpSystem_Season2_mail1 = 49;    // 邮件
    public const int FreshmanHelpSystem_Season2_mail2 = 50;    // 邮件
    public const int FreshmanHelpSystem_Season2_mail3 = 51;    // 邮件

    public const int FreshmanHelpSystem_Season2_LevelUp_12 = 52;    // 升到12级

    public const int FreshmanHelpSystem_Season2_horse1 = 53;    // 坐骑
    public const int FreshmanHelpSystem_Season2_horse2 = 54;    // 坐骑
    public const int FreshmanHelpSystem_Season2_horse3 = 55;    // 坐骑
    public const int FreshmanHelpSystem_Season2_LevelUp_15 = 56; // 15级升级提示

    public const int FreshmanHelpSystem_first_learn_skill_popshortcut = 57;
    public const int FreshmanHelpSystem_first_learn_skill_mainfunction = 58;
    public const int FreshmanHelpSystem_this_is_your_new_learned_skill = 59;

    public const int FreshmanHelpSystem_60 = 60;
    public const int FreshmanHelpSystem_61 = 61;
    public const int FreshmanHelpSystem_62 = 62;
    public const int FreshmanHelpSystem_63 = 63;
    public const int FreshmanHelpSystem_64 = 64;
    public const int FreshmanHelpSystem_65 = 65;
    public const int FreshmanHelpSystem_66 = 66;
    public const int FreshmanHelpSystem_67 = 67;
    public const int FreshmanHelpSystem_68 = 68;
    public const int FreshmanHelpSystem_69 = 69;
    public const int FreshmanHelpSystem_70 = 70;
    public const int FreshmanHelpSystem_71 = 71;
    public const int FreshmanHelpSystem_72 = 72;
    public const int FreshmanHelpSystem_73 = 73;
    public const int FreshmanHelpSystem_74 = 74;
    public const int FreshmanHelpSystem_75 = 75;
    public const int FreshmanHelpSystem_76 = 76;
    public const int FreshmanHelpSystem_77 = 77;
    public const int FreshmanHelpSystem_78 = 78;
    public const int FreshmanHelpSystem_79 = 79;
    public const int FreshmanHelpSystem_80 = 80;
    public const int FreshmanHelpSystem_81 = 81;
    public const int FreshmanHelpSystem_82 = 82;
    public const int FreshmanHelpSystem_83 = 83;
    public const int FreshmanHelpSystem_84 = 84;
    public const int FreshmanHelpSystem_85 = 85;
    public const int FreshmanHelpSystem_86 = 86;
    public const int FreshmanHelpSystem_87 = 87;
    public const int FreshmanHelpSystem_88 = 88;
    public const int FreshmanHelpSystem_89 = 89;
    public const int FreshmanHelpSystem_90 = 90;
    public const int FreshmanHelpSystem_91 = 91;
    public const int FreshmanHelpSystem_92 = 92;
    public const int FreshmanHelpSystem_93 = 93;
    public const int FreshmanHelpSystem_94 = 94;
    public const int FreshmanHelpSystem_95 = 95;
    public const int FreshmanHelpSystem_96 = 96;
    public const int FreshmanHelpSystem_97 = 97;
    public const int FreshmanHelpSystem_98 = 98;
    public const int FreshmanHelpSystem_99 = 99;
    public const int FreshmanHelpSystem_100 = 100;
    public const int FreshmanHelpSystem_101 = 101;
    public const int FreshmanHelpSystem_102 = 102;
    public const int FreshmanHelpSystem_103 = 103;
    public const int FreshmanHelpSystem_104 = 104;
    public const int FreshmanHelpSystem_105 = 105;
    public const int FreshmanHelpSystem_106 = 106;
    public const int FreshmanHelpSystem_107 = 107;
    public const int FreshmanHelpSystem_108 = 108;
    public const int FreshmanHelpSystem_109 = 109;
    public const int FreshmanHelpSystem_110 = 110;
    public const int FreshmanHelpSystem_111 = 111;
    public const int FreshmanHelpSystem_112 = 112;
    public const int FreshmanHelpSystem_113 = 113;
    public const int FreshmanHelpSystem_114 = 114;
    public const int FreshmanHelpSystem_115 = 115;
    public const int FreshmanHelpSystem_116 = 116;
    public const int FreshmanHelpSystem_117 = 117;
    public const int FreshmanHelpSystem_118 = 118;
    public const int FreshmanHelpSystem_119 = 119;
    public const int FreshmanHelpSystem_120 = 120;
    public const int FreshmanHelpSystem_121 = 121;
    public const int FreshmanHelpSystem_122 = 122;
    public const int FreshmanHelpSystem_123 = 123;
    public const int FreshmanHelpSystem_124 = 124;
    public const int FreshmanHelpSystem_125 = 125;
    public const int FreshmanHelpSystem_126 = 126;
    public const int FreshmanHelpSystem_127 = 127;
    public const int FreshmanHelpSystem_153 = 153;

    /// <summary>
    /// 预留150位作为地图展示用的标志位
    /// 用法是SceneShow_Begin + MapReferenceID
    /// 因为目前地图的ID已经到101了，所以预留到150
    /// 新的标志位请从SceneShow_End + 1 ( 279 ) 开始
    /// 陈俊，2012/6/7
    /// </summary>
    public const int SceneShow_Begin = 128;
    public const int SceneShow_End = 278;

    public const int FreshmanHelpSystem_279 = 279;
    public const int FreshmanHelpSystem_280 = 280;
    public const int FreshmanHelpSystem_281 = 281;
    public const int FreshmanHelpSystem_282 = 282;
    public const int FreshmanHelpSystem_283 = 283;
    public const int FreshmanHelpSystem_284 = 284;
    public const int FreshmanHelpSystem_285 = 285;
    public const int FreshmanHelpSystem_286 = 286;
    public const int FreshmanHelpSystem_287 = 287;
    public const int FreshmanHelpSystem_288 = 288;
    public const int FreshmanHelpSystem_289 = 289;
    public const int FreshmanHelpSystem_290 = 290;
    public const int FreshmanHelpSystem_291 = 291;
    public const int FreshmanHelpSystem_292 = 292;
    public const int FreshmanHelpSystem_293 = 293;
    public const int FreshmanHelpSystem_294 = 294;
    public const int FreshmanHelpSystem_295 = 295;
    public const int FreshmanHelpSystem_296 = 296;
    public const int FreshmanHelpSystem_297 = 297;
    public const int FreshmanHelpSystem_298 = 298;
    public const int FreshmanHelpSystem_299 = 299;
    public const int FreshmanHelpSystem_300 = 300;

    public const int FreshmanHelpSystem_301 = 301;
    public const int FreshmanHelpSystem_302 = 302;
    public const int FreshmanHelpSystem_303 = 303;
    public const int FreshmanHelpSystem_304 = 304;
    public const int FreshmanHelpSystem_305 = 305;
    public const int FreshmanHelpSystem_306 = 306;
    public const int FreshmanHelpSystem_307 = 307;
    public const int FreshmanHelpSystem_308 = 308;
    public const int FreshmanHelpSystem_309 = 309;
    public const int FreshmanHelpSystem_310 = 310;
    public const int FreshmanHelpSystem_311 = 311;
    public const int FreshmanHelpSystem_312 = 312;
    public const int FreshmanHelpSystem_313 = 313;
    public const int FreshmanHelpSystem_314 = 314;
    public const int FreshmanHelpSystem_315 = 315;
    public const int FreshmanHelpSystem_316 = 316;
    public const int FreshmanHelpSystem_317 = 317;
    public const int FreshmanHelpSystem_318 = 318;
    public const int FreshmanHelpSystem_319 = 319;
    public const int FreshmanHelpSystem_320 = 320;
    public const int FreshmanHelpSystem_321 = 321;
    public const int FreshmanHelpSystem_322 = 322;
    public const int FreshmanHelpSystem_323 = 323;
    public const int FreshmanHelpSystem_324 = 324;
    public const int FreshmanHelpSystem_325 = 325;
    public const int FreshmanHelpSystem_326 = 326;
    public const int FreshmanHelpSystem_327 = 327;
    public const int FreshmanHelpSystem_328 = 328;
    public const int FreshmanHelpSystem_329 = 329;
    public const int FreshmanHelpSystem_330 = 330;

    public const int NoPromptPeaceItemWhenEnterPublicMap = 331;	// 进入公用地图时，不提示使用免战牌
    
    public const int Max = 64;        // 64 个 8位 最大可以定义到 Max * 8
}

//注意,因为类型数据要存DB,所以这里的定义的ID不能重复
public enum QuestCompleteConditionType {
    KillMonster = 0,			// 杀怪
    Collect = 1,				// 采集
    Goto = 2,					// 到达
    HasSkill = 3,				// 获得技能
    UseItem = 4,				// 使用道具
    //RageStar = 5,				// 怒气星
    NoItem = 6,					// 不拥有道具
    KillMultiMonster = 7,		// 杀多怪
    TriggerMapObject = 8,		// 触发地图物件
    UpgradeToLevel = 9,			// 升级到
    Carriage = 10,				// 镖车
    KillMonsterLevelRange = 11,	// 杀级别范围怪
    UseItemRandom = 12,			// 随机使用道具
    AICondition = 13,    		// AI触发
    Question = 14,              // 答题数据
    TriggerAction = 15,          // 触发某种动作
    CollectAnyOne = 16,         // 采集其中任何一种
    BattlefieldComplete = 17        // 战场获胜
}

namespace Quest {

    // 完成条件的状态：未知、已完成和已失败。
    // 注意：与客户端公用。要存DB。不能修改
    public enum Status {
        Completed = 1,
        Unknown = 2,
        Failed = 3,
        Time = 4,               // 时间未到
    }

    [Flags]
    public enum FindPathType : short {
        Normal = 0,

        [Describe("完成NPC不寻路")]
        DisableEndNpc = 1,

        [Describe( "完成条件不寻路" )]
        DisableCondition = 2,

        [Describe("任务面板上不显示完成次数")]
        NoPanelQuestCount = 4,

        [Describe("寻路后自动攻击")]
        AutoAttack = 8
    }

}

namespace Relation {

    /// <summary>
    /// 关系系统的错误代码定义
    /// </summary>
    public enum ErrorCode {
        Ok = 0,

        [Describe( "等级不满足条件" )]
        IncorrectPlayerLevel,

        [Describe( "对方等级不满足条件" )]
        NotEnoughYourLevel,

        [Describe( "缺少加关系需要的道具" )]
        IHaveNotItem,

        [Describe( "对方缺少相关道具" )]
        YouHaveNotItem,

        [Describe( "对方与自己不在同一地图" )]
        NotInSameWorld,

        [Describe( "对方与自己不是同一阵营" )]
        NotInSameCamp,

        [Describe( "情侣必须是异性之间的" )]
        IncorrectSex,

        [Describe( "关系已经存在" )]
        RelationAlreadyExist,

        [Describe( "关系不存在" )]
        RelationNotExist,

        [Describe( "已存在其它互斥关系" )]
        MutexRelationAlreadyExists,

        [Describe( "已被对方列入黑名单，无法加关系" )]
        InYourBlackList,

        [Describe( "对方在您的黑名单中，无法加关系" )]
        InMyBlackList,

        [Describe( "好友数已达上限，无法增加新的好友" )]
        FriendCountOverflow,

        [Describe( "黑名单已满，无法增加新的黑名单" )]
        BlackCountOverflow,

        [Describe( "对方徒弟数已达上限" )]
        YourApprenticeOverflow,

        [Describe( "您的徒弟数已上限" )]
        MyApprenticeOverflow,

        [Describe( "对方不在线" )]
        YouAreNotOnLine,

        [Describe( "邀请已经发出，请耐心等待对方回应" )]
        InviteAlreadySent,

        [Describe( "加关系失败" )]
        Failed,

        [Describe( "对方已和其他人具有此种关系了" )]
        PeerRelationAlreadyExists,

        [Describe( "未出师前不能收徒" )]
        HasMaster,

        [Describe( "查询操作过于频繁，请稍后再试" )]
        WaitFor
    }
}

// 任务失败原因
public enum QuestFailReason {
    Unknown,		// 未知
    Timeout,		// 超时
    PlayerDead,		// 玩家死亡
    Carriage,       
}

// IB推荐定位 相关数据结构
public struct IbRecommendInfo {
    public int nMainType; // IB商城中的主分页
    public int nSubType;  // IB商城中的子分页
    public int nID;       // 道具ID
}

// 天书注入操作的返回值
enum FastnessStoreResult {
    [Describe("成功")]
    Ok = 0,
    [Describe("无效的参数")]
    InvalidParams,
    [Describe("配置未找到")]
    ConfigNotFound,
    [Describe("要注入的金钱超限")]
    MoneyOverflow,
    [Describe("要注入的活力超限")]
    VigourOverflow,
    [Describe("金钱不足")]
    MoneyNotEnough,
    [Describe("活力不足")]
    VigourNotEnough
}

// 天书注入操作的返回值
enum FastnessUpgradeResult {
    [Describe("成功")]
    Ok = 0,
    [Describe("无效的参数")]
    InvliadParams,
    [Describe("配置未找到")]
    ConfigNotFound,
    [Describe("已注入的金钱不足")]
    MoneyNotEnough,
    [Describe("已注入的活力不足")]
    VigourNotEnough

}