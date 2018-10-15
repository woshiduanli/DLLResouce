using System;
using System.Collections.Generic;
using System.Reflection;

// 错误代码
public partial class StrDef {
    //人物相关
    [EditorEnum( "参数无效" )]
    public const int INVALID_PARAMETER         = 1;
    [EditorEnum( "等级不够" )]
    public const int LEVEL_ISNOT_ENOUGH         = 2;
    [EditorEnum( "对方等级不足" )]
    public const int OBJECT_LV_NOT_ENOUGH       = 3;
    [EditorEnum( "等级过高" )]
    public const int LEVEL_OUTOF_LIMIT         = 4;
    [EditorEnum( "职业不匹配" )]
    public const int OCCUPATION_DISMATH        = 5;
    [EditorEnum( "活力到达上限 " )]
    public const int VIGOURPOINT_MAXIMUM       = 6;
    [EditorEnum( "活力不足" )]
    public const int VIGOURPOINT_NOTENOUGH      = 7;
    [EditorEnum( "找不到此玩家" )]
    public const int CANNOT_FOUND_PLAYER       = 8;
    [EditorEnum( "对方不在线" )]
    public const int OBJECT_NOT_ONLINE         = 9;
    [EditorEnum( "最大怒气星已满" )]
    public const int MAX_RAGE_TOO_MUCH         = 11;
    [EditorEnum( "怒气已达最大" )]
    public const int RAGE_ACHIEVE_MAX          = 12;
    [EditorEnum( "已经为其他人所救" )]
    public const int HAS_BEEN_SAVE_BY_OTHER    = 13;
    [EditorEnum( "距离过远" )]
    public const int DISTANCE_TOO_FAR          = 14;
    [EditorEnum( "性别不匹配" )]
    public const int GENDER_DISMATCH           = 15;
    [EditorEnum( "角色处于删除保护" )]
    public const int ROLE_IN_DELPROTECTED      = 16;
    [EditorEnum( "红名无法传送出监狱" )]
    public const int CANNOT_TELEPORT_OUTOF_JAIL= 17;
    [EditorEnum( "目标地图无效，无法加入" )]
    public const int INVALID_MAP               = 18;
    [EditorEnum( "战斗中无法切换PVP模式" )]
    public const int CANNOT_SET_PVP_IN_FIGHT   = 19;
    [EditorEnum( "切换PVP模式需脱离PK达3分钟" )]
    public const int CANNOT_CLOSE_PVP          = 20;
    [EditorEnum( "必须双方都在擂台区域中" )]
    public const int MUST_IN_ARENA             = 21;
    [EditorEnum( "你正在与其他玩家进行单挑" )]
    public const int SELF_HAS_DUEL             = 22;
    [EditorEnum( "对方正与其他玩家进行单挑" )]
    public const int OTHER_HAS_DUEL            = 23;
    [EditorEnum( "未找到单挑目标" )]
    public const int CANNOT_FOUND_DUEL_OBJECT  = 24;
    [EditorEnum( "道具的所有者不在线")]
    public const int ITEM_OWNER_IS_NOT_ONLINE  = 25;
    [EditorEnum( "所有者身上没有此道具")]
    public const int QUERY_ITEM_IS_NOT_EXIST   = 26;
    [EditorEnum( "你现在为红名状态，回城复活点为刑狱牢营")]
    public const int RED_NAME_RELIVE_HINT      = 27;
    [EditorEnum( "你恶名值过高，将被关押进刑狱牢营。")]
    public const int RED_NAME_GOTO_JAIL        = 28;
    [EditorEnum( "需要先开启怒气槽" )]
    public const int NEED_MAX_RAGE             = 29;
    [EditorEnum( "无法发起单挑" )]
    public const int CANNOT_DUEL               = 30;
    [EditorEnum( "保护等级无法传送到擂台" )]
    public const int CANNOT_TELEPORT_ARENA     = 31;
    [EditorEnum( "已经邀请过此玩家" )]
    public const int OTHER_IN_INVITE_CACHE     = 32;
    [EditorEnum( "战斗中无法重选角色" )]
    public const int CANNOT_SELECT_ROLE_IN_FIGHT = 33;
    [EditorEnum( "无法检取已被其它玩家打开的宝箱" )]
    public const int CANNOT_OPEN_BOX           = 34;
    [EditorEnum( "你已被禁言，无法聊天" )]
    public const int CANNOT_CHAT               = 35;
    [EditorEnum( "无法用情侣传送出监狱" )]
    public const int LOVER_CANNOT_TELEPORT_OUTOF_JAIL= 36;
    [EditorEnum( "当前镖车无法替换" )]
    public const int CANNOT_REFRESH_CARRIAGE   = 37;
    [EditorEnum( "参加神兵榜排行成功" )]
    public const int EQUIP_TOTAL_SCORE_SUCCESS = 38;
    [EditorEnum( "无法与此NPC交易" )]
    public const int CANNOT_NPC_TRADE          = 39;
    [EditorEnum( "返回点无效,请先到驿站处绑定" )]
    public const int CANNOT_RETURN_POS         = 40;
    [EditorEnum( "无法Warp到目标地图" )]
    public const int CANNOT_WARPTO_MAP         = 41;
    [EditorEnum( "战斗中无法使用马车令" )]
    public const int CANNOT_USE_RETURN_POS_IN_FIGHT = 42;
    [EditorEnum( "当前地图无法召唤" )]
    public const int CANNOT_SUMMON_PLAYER      = 43;
    [EditorEnum( "没有对应生活技能" )]
    public const int NOT_SAME_PRODUCT_SKILL    = 44;
    [EditorEnum( "当日击杀荣誉已达上限（2000点）" )]
    public const int TODAY_KILL_HONOUR_OVERFLOW = 45;
    [EditorEnum( "处于防沉迷状态,无法获得道具" )]
    public const int ANTI_ADDICTION_CANNOT_ADD_ITEM = 46;
    [EditorEnum( "自己处于防沉迷状态,无法交易" )]
    public const int ANTI_ADDICTION_SELF_CANNOT_TRADE = 47;
    [EditorEnum( "对方处于防沉迷状态,无法交易" )]
    public const int ANTI_ADDICTION_OTHER_CANNOT_TRADE = 48;
    [EditorEnum( "处于防沉迷状态,无法交接任务" )]
    public const int ANTI_ADDICTION_CANNOT_QUEST = 49;
    [EditorEnum( "处于防沉迷状态,无法购买" )]
    public const int ANTI_ADDICTION_CANNOT_BUY = 50;
    [EditorEnum( "处于防沉迷状态,无法出售" )]
    public const int ANTI_ADDICTION_CANNOT_SALE = 51;
    [EditorEnum( "死亡时无法创建副本" )]
    public const int DEAD_CANNOT_CREATE_DUNGEON = 52;
    [EditorEnum( "处于防沉迷状态,无法存取仓库资金" )]
    public const int ANTI_ADDICTION_CANNOT_STOREHOUSE_MONEY = 53;
    [EditorEnum( "处于防沉迷状态,影响金钱收益" )]
    public const int ANTI_ADDICTION_CHANGE_MONEY = 54;
    [EditorEnum( "处于虚弱状态,暂时无法复活" )]
    public const int CANNOT_ITEM_RELIVE = 55;
    [EditorEnum( "玩家已经是此阵营" )]
    public const int IS_SAME_CAMP_ON_CHANGE_CAMP = 56;
    [EditorEnum( "在军团中的玩家,无法改变阵营" )]
    public const int CHANGE_CAMP_IN_GROUP_ERROR = 57;
    [EditorEnum( "有情侣,结拜,师徒等关系,无法改变阵营" )]
    public const int CHANGE_CAMP_RELATIONSHIP_ERROR = 58;
    [EditorEnum( "国公无法改变阵营" )]
    public const int CHANGE_CAMP_IS_KING_ERROR = 59;
    [EditorEnum( "没有相应的阵营改变道具" )]
    public const int CHANGE_CAMP_NOT_ITEM_ERROR = 60;
    [EditorEnum( "在摆摊中,无法改变阵营" )]
    public const int CHANGE_CAMP_STALL_ERROR = 61;
    
    //交易相关
    [EditorEnum( "背包里金钱不足" )]
    public const int NOT_ENOUGH_MONEY           = 101;
    [EditorEnum( "金钱已经到达上限" )]
    public const int MONEY_IS_BIGGEST          = 102;
    [EditorEnum( "已经处于交易中" )]
    public const int HAS_ENTER_P2PTRADE        = 103;
    [EditorEnum( "交易异常" )]
    public const int P2PTRADE_EXCEPTION        = 104;
    [EditorEnum( "对方金钱已经到达上限" )]
    public const int OTHER_MONEY_TOOBIG        = 105;
    [EditorEnum( "对方空间不足" )]
    public const int OTHER_SPACE_NOT_ENOUGH     = 106;
    [EditorEnum( "背包空间不足" )]
    public const int PACKSPACE_NOT_ENOUGH       = 107;
    [EditorEnum( "不是你的物品" )]
    public const int SELF_ISNOT_OWNER          = 108;
    [EditorEnum( "在战斗中无法交易" )]
    public const int SELF_CANNOT_TRADE_INFIGHT = 109;
    [EditorEnum( "对方现在不能进行交易" )]
    public const int OTHER_STATE_ERROR         = 110;
    [EditorEnum( "交易过于频繁" )]
    public const int P2PTADE_INTERVAL_LMT      = 111;
    [EditorEnum( "对方已经处于交易状态中" )]
    public const int OTHER_HAS_P2PTRADE        = 112;
    [EditorEnum( "对方在战斗中无法交易" )]
    public const int OTHER_CANNOT_TRADE_INFIGHT = 113;
    [EditorEnum( "自己死亡时无法交易" )]
    public const int SELF_CANNOT_TRADE_DEAD    = 114;
    [EditorEnum( "对方死亡时无法交易" )]
    public const int OTHER_CANNOT_TRADE_DEAD   = 115;
    [EditorEnum( "可用师德值不足" )]
    public const int NOT_ENOUGH_MASTERVALUE     = 116;
    [EditorEnum( "本日获取师德值已到上限" )]
    public const int MASTERVALUE_LIMITED        = 117;
    [EditorEnum( "没有足够的战勋值" )]
    public const int NOT_ENOUGH_BATTLEFIELD_VALUE = 118;
    [EditorEnum( "没有足够的军团贡献值" )]
    public const int NOT_ENOUGH_GROUP_CONTRIBUTION = 119;
    [EditorEnum( "军团等级不够,无法购买" )]
    public const int BUY_ITEM_GROUP_LEVEL_ERROR = 120;
    [EditorEnum( "没有足够的荣誉值" )]
    public const int NOT_ENOUGH_HONOUR          = 121;
    
    //道具，背包，仓库
    [EditorEnum( "道具不够" )]
    public const int ITEM_NOTENOUGH             = 201;
    [EditorEnum( "仓库密码错误" )]
    public const int PASSOWRD_ERROR            = 202;
    [EditorEnum( "仓库空间不足" )]
    public const int STORESPACE_NOT_ENOUGH      = 203;
    [EditorEnum( "背包格子已经到达上限" )]
    public const int MAX_BAG_CELL_ACHIEVED     = 204;
    [EditorEnum( "仓库格子已经到达上限" )]
    public const int MAX_STORE_CELL_ACHIEVED   = 205;
    [EditorEnum( "无法在此地图使用传送类物品" )]
    public const int CANNOT_USE_TELEPORT_ITEM  = 206;
    [EditorEnum( "该道具暂无坐标被记录")]
    public const int CANNOT_USE_RETURNPOS_ITEM  = 207;
    [EditorEnum( "没有足够经验")]
    public const int EXP_NOT_ENOUGH  = 208;

/*跟下面IB_NOT_ENOUGH_MONEY重复了，陈俊
    [EditorEnum("购买IB道具余额不足")]
    public const int BUY_IB_NOENOUGH        = 208;*/
    [EditorEnum("商城道具购买成功")]
    public const int BUY_IB_OK              = 209;
    [EditorEnum( "商城道具购买没有成功，系统繁忙，请重试一次" )]
    public const int BUY_IB_ERROR           = 210;
    [EditorEnum( "账号服务器繁忙，请稍后再试" )]
    public const int IB_LOGIN_SVR_ERROR     = 211;
    [EditorEnum("商城购买数据有误，请确保你的客户端为最新版本")]
    public const int IB_NOT_FIND            = 212;
    [EditorEnum( "商城道具没有找到，请确保你的客户端为最新版本" )]
    public const int IB_ITEM_NOT_FIND       = 213;
    [EditorEnum( "商城购买数据有误，请确保你的客户端为最新版本" )]
    public const int IB_ITEM_NOT_MATCH      = 214;
    [EditorEnum("商城元宝余额不足")]
    public const int IB_NOT_ENOUGH_MONEY    = 215;
    [EditorEnum("背包空间不足")]
    public const int IB_NOT_ENOUGH_BAGPOS   = 216;
    [EditorEnum("你尚有一个购买操作末完成，账号服务器可能繁忙，请耐心等待")]
    public const int IB_OP_QUIKLY           = 217;
    [EditorEnum("没有找到玩家")]
    public const int IB_NO_PLAYER           = 218;
    [EditorEnum("现在不能这样做")]
    public const int IB_MutexSTATE          = 219;
    [EditorEnum( "增加元宝道具使用出现异常，请联系GM" )]
    public const int IB_SPECIALITEM_FAILED  = 220;
    [EditorEnum( "元宝道具使用成功，余额已更新" )]
    public const int IB_SPECIALITEM_OK      = 221;
    [EditorEnum( "该元宝道具已经使用过，请确认你的道具是由商城购买得到的，若有疑问，可联系GM" )]
    public const int IB_SPECIALITEM_USED      = 222;
    [EditorEnum( "道具使用出错，请确认你的道具是由商城购买得到的" )]
    public const int IB_SPECIALITEM_ERROR     = 223;
    [EditorEnum( "赠送联系人没有找到，请确认操作流程正确，再试一次" )]
    public const int IB_GIFT_ROLEERROR      = 224;
    [EditorEnum( "你购买的商城道具已成功投递对方邮箱中" )]
    public const int IB_GIFT_OK             = 225;
    [EditorEnum( "商城绑定元宝余额不足" )]
    public const int IB_NOT_ENOUGH_BIND    = 226;
    [EditorEnum( "赠送功能不能使用绑定元宝" )]
    public const int IB_GIFT_BIND    = 227;
    [EditorEnum( "限时活动抢购已结束" )]
    public const int IB_Limted_Time = 228;
    [EditorEnum( "商品库存不够或已售光，请重新确定数量" )]
    public const int IB_Limted_Count = 229;

    //技能
    [EditorEnum( "技能无法升级" )]
    public const int SKILL_CANNOT_UPGRADE      = 301;
    [EditorEnum( "技能已经存在" )]
    public const int SKILL_EXIST               = 302;
    [EditorEnum( "没有对应的技能，无法增加天赋" )]
    public const int ADD_TALENT_FAIL           = 303;
    [EditorEnum( "道具增加的天赋已达到上限" )]
    public const int TALENT_HAS_MAX_LEVEL      = 304;

    //装备
    [EditorEnum( "装备位置不匹配" )]
    public const int EQUIP_POS_DISMATH         = 401;
    [EditorEnum( "此装备不能被分解" )]
    public const int ITEM_CANNOT_REFINERYED    = 403;
    [EditorEnum( "卸下装备失败，背包没有足够空位" )]
    public const int NOT_ENOUGH_SPACE_UNEQUIP   = 404;
    [EditorEnum( "无法拆分物品，没有足够的空位" )]
    public const int NOT_ENOUGH_SPACE_SPLIT     = 405;

    //组队
    [EditorEnum( "队伍已满" )]
    public const int TEAM_IS_FULL              = 501;
    [EditorEnum( "对方已经在副本等待房间中" )]
    public const int OBJECT_BE_IN_ROOM         = 502;
    [EditorEnum( "要踢的队员在副本中" )]
    public const int KICK_MEMBER_IN_DUNGEON    = 503;
    [EditorEnum( "自己在副本等待房间中" )]
    public const int SELF_BE_IN_ROOM           = 504;
    [EditorEnum( "对方不在队伍中" )]
    public const int OBJECT_NOT_IN_TEAM        = 505;
    [EditorEnum( "对方不是队长" )]
    public const int OBJECT_NOT_BE_CAPATIN     = 506;
    [EditorEnum( "自己不在队伍中" )]
    public const int SELF_NOT_IN_TEAM          = 507;
    [EditorEnum( "你不是队长" )]
    public const int NOT_CAPTAIN               = 508;
    [EditorEnum( "对方已经组队" )]
    public const int OBJECT_BE_IN_TEAM         = 509;
    [EditorEnum( "任务需要组队")]
    public const int QUEST_NEED_TEAM           = 510;
    [EditorEnum( "任务需要是队长")]
    public const int QUEST_NEED_CAPTAIN        = 511;
    [EditorEnum( "此房间已开始匹配,无法加入")]
    public const int ROOM_BEGIN_MATCH          = 512;

    //生活技能
    [EditorEnum( "采集失败" )]
    public const int COLLECT_ITEM_FAIL         = 602;

    //关系
    [EditorEnum( "对方在你的黑名单中" )]
    public const int IN_BLACK                  = 701;
    [EditorEnum( "你在对方的黑名单中")]
    public const int ROLE_IN_BLACK             = 702;
    [EditorEnum( "你的好友数量已满")]
    public const int FRIEND_SPACE_NOT_ENOUGH   = 703;
    [EditorEnum( "指定道具不存在")]
    public const int ITEM_NOT_EXIST            = 704;
    [EditorEnum( "你的徒弟数量已满")]
    public const int APPRENTICE_SPACE_NOT_ENOUGH = 705;
    [EditorEnum( "对方已经有了师徒关系")]
    public const int ROLE_HAS_MASTER           = 706;
    [EditorEnum( "对方等级不够")]
    public const int ROLE_LEVEL_NOT_ENOUGH     = 707;
    [EditorEnum( "对方等级不满足条件")]
    public const int ROLE_LEVEL_OUTOF_LIMIT    = 708;
    [EditorEnum( "自己已经有了师父")]
    public const int ALREADY_HAS_MASTER        = 709;
    [EditorEnum( "对方徒弟数量已满")]
    public const int ROLE_APPRENTICE_SPACE_NOT_ENOUGH = 710;
    [EditorEnum( "自己已经有了情侣")]
    public const int ALREADY_HAS_LOVER         = 711;
    [EditorEnum( "这种关系需要组队添加")]
    public const int RELATION_NEEDS_A_TEAM     = 712;
    [EditorEnum( "必须与对方组成二人队伍")]
    public const int RELATION_TWO_MEMBER_TEAM = 713;
    [EditorEnum( "您不在指定的NPC处")]
    public const int RELATION_SPECIAL_PLACE    = 714;
    [EditorEnum( "我们不支持同性情侣")]
    public const int RELATION_SAME_SEX         = 715;
    [EditorEnum( "你已经有结拜关系了")]
    public const int ALREADY_HAS_BROTHER       = 716;
    [EditorEnum( "人数不满足条件")]
    public const int RELATION_TOO_MANY_ROLES   = 717;
    [EditorEnum( "人家已经结拜过了")]
    public const int ROLE_ALREADY_HAS_BROTHER  = 718;
    [EditorEnum( "这个是要以好友关系为基础的")]
    public const int RELATION_NEEDS_TOBE_FRIENDS = 719;
    [EditorEnum( "都是情人了，这个操作不允许")]
    public const int RELATION_LOVERS_NOT_ALLOWED = 720;
    [EditorEnum( "都是兄弟了，这个操作不允许")]
    public const int RELATION_BROTHER_NOT_ALLOWED = 721;
    [EditorEnum( "都是师徒了，这个操作不允许")]
    public const int RELATION_MASTERAPPRENTICE_NOT_ALLOWED = 722;
    [EditorEnum ( "对方的好友数量已满")]
    public const int ROLE_FRIENDS_SPACE_NOT_ENOUGH = 723;
    [EditorEnum( "人家已经有情侣了")]
    public const int ROLE_ALREADY_HAS_LOVER    = 724;
    [EditorEnum( "跟你的伙伴中，有人关系道具不存在")]
    public const int ROLE_ITEM_NOT_EXIST  = 725;
    [EditorEnum( "你的仇人名单满了，如果需要，请手动删除")]
    public const int ENEMY_SPACE_NOT_ENOUGH = 726;
    [EditorEnum( "你还没有出师")]
    public const int HAS_MASTER_YET         = 727;
    [EditorEnum( "玩家不在指定位置")]
    public const int ROLE_RELATION_SPECIAL_PLACE  = 728;
    [EditorEnum( "黑名单数量已满")]
    public const int BLACK_SPACE_NOT_ENOUGH   = 730;
    [EditorEnum( "对方还没有出师")]
    public const int ROLE_HAS_MASTER_YET      = 731;
    [EditorEnum( "您的等级不够")]
    public const int LEVEL_NOT_ENOUGH         = 732;
    [EditorEnum( "关系已经存在")]
    public const int REL_ALREADY_EXIST        = 733;
    [EditorEnum( "不在同一张地图中")]
    public const int NOT_IN_THE_SAME_WORLD    = 734;
    [EditorEnum( "您不满足副本关系要求")]
    public const int DUNGEON_RELATION_NOT_FIT = 735;
    [EditorEnum( "不同阵营不能存在关系" )]
    public const int CAMP_NOT_ALLOWED         = 736;
    [EditorEnum("对方不在线")]
    public const int RELATION_ROLE_NOT_ONLINE = 737;
    [EditorEnum("不知道为什么，这次的关系申请失败了")]
    public const int RELATION_FAILED = 738;
    [EditorEnum("对方没有相关道具")]
    public const int RELATION_TARGET_HASNOT_ITEM = 739;
    [EditorEnum("请求已经发送过了，请等待对方回应")]
    public const int RELATION_REQUEST_ALREADY_SENT = 740;
    [EditorEnum("关系不存在")]
    public const int RELATION_NOT_EXIST = 741;
    [EditorEnum("亲密度已达今日上限")]
    public const int RELATION_FAMILI_OUT_OF_TODAY_UPPERBOUND = 742;


    //副本
    [EditorEnum( "不在队伍中，传送出副本" )]
    public const int DUNGEON_NOT_IN_TEAM       = 801;
    [EditorEnum( "已在一个副本等待房间中" )]
    public const int SELF_BE_IN_DUNGEON_ROOM   = 803;
    [EditorEnum( "无法加入地图，不满足进入条件" )]
    public const int CANNOT_ENTER_MAP          = 804;
    [EditorEnum( "副本等待列表已满，无法创建房间" )]
    public const int DUNGEON_ROOM_FULL         = 805;
    [EditorEnum( "加入房间失败" )]
    public const int ENTER_DUNGEON_ROOM_FAIL   = 807;
    [EditorEnum( "密码错误" )]
    public const int DUNGEON_ROOM_PASW_ERROR   = 808;
    [EditorEnum( "有玩家未准备，无法开始" )]
    public const int DUNGEON_ROOM_NOT_READY    = 810;
    [EditorEnum( "组建队伍失败，无法启动副本" )]
    public const int CREATE_DUNGEON_TEAM_FAIL  = 812;
    [EditorEnum( "加入副本失败，无效的副本" )]
    public const int ENTER_DUNGEON_FAIL        = 813;
    [EditorEnum( "加入副本失败，副本队伍已满" )]
    public const int ENTER_DUNGEON_QUEUE_FULL  = 814;
    [EditorEnum( "创建副本失败，副本尚未开启" )]
    public const int DUNGEON_NOT_OPEN          = 815;
    [EditorEnum( "进入副本失败，没有相应任务" )]
    public const int ENTER_DUNGEON_FAIL_QUEST  = 816;
    [EditorEnum( "刚逃离战场，无法加入" )]
    public const int ESCAPER_ENTER_BATTLEFIELD_FAIL = 818;
    [EditorEnum( "未达到最小人数,无法加入" )]
    public const int DUNGONE_MIN_PLAYER_ERROR  = 819;

    //GM指令
    [EditorEnum( "无效的map obj ID" )]
    public const int INVALID_MAP_OBJECT_ID     = 901;
    [EditorEnum( "无效的等级" )]
    public const int INVALID_LEVEL             = 902;
    [EditorEnum( "无效的SN， 没有找到此ROLE" )]
    public const int INVALID_SN                = 903;
    [EditorEnum( "传送失败" )]
    public const int TELEPORT_FAIL             = 904;
    [EditorEnum( "无效的NPC SN" )]
    public const int INVALID_NPC_SN            = 905;
    [EditorEnum( "禁言成功" )]
    public const int DISABLE_CHAT_SUCCESS      = 906;
    [EditorEnum( "禁言失败 没找到此玩家" )]
    public const int DISABLE_CHAT_FAIL         = 907;

    //军团相关
    [EditorEnum( "你已经加入了军团，操作失败" )]
    public const int HAS_GROUP                 = 1000;
    [EditorEnum( "对方已经加入了军团，操作失败" )]
    public const int OTHER_HAS_GROUP           = 1001;
    [EditorEnum( "已经有同名军团" )]
    public const int GROUP_NAME_REPEAT         = 1002;
    [EditorEnum( "无法进行此军团操作，请稍后再试" )]
    public const int GROUP_OPT_ERROR           = 1003;
    [EditorEnum( "创建军团成功" )]
    public const int GROUP_CREATE_SUCCESS      = 1004;
    [EditorEnum( "无法解散军团，不是此军团的军团长" )]
    public const int CANNOT_DISMISS_GROUP      = 1005;
    [EditorEnum( "你离开了军团" )]
    public const int REMOVE_FROM_GROUP         = 1007;
    [EditorEnum( "无法恢复此军团" )]
    public const int CANNOT_RESTORE_GROUP      = 1008;
    [EditorEnum( "恢复军团成功" )]
    public const int RESTORE_GROUP_SUCCESS     = 1009;
    [EditorEnum( "恢复军团失败" )]
    public const int RESTORE_GROUP_FAILED      = 1010;
    [EditorEnum( "你没有添加成员的权限" )]
    public const int CANNOT_INVITE             = 1011;
    [EditorEnum( "军团已达到成员上限，无法添加" )]
    public const int GROUP_IS_FULL             = 1012;
    [EditorEnum( "加入军团成功" )]
    public const int ADDTO_GROUP_SUCCESS       = 1013;
    [EditorEnum( "加入军团失败" )]
    public const int ADDTO_GROUP_FAIL          = 1014;
    [EditorEnum( "无法踢出此成员" )]
    public const int KICK_MEMBER_FAIL          = 1015;
    [EditorEnum( "军团长无法退出军团" )]
    public const int LEADER_CANNOT_REMOVE      = 1016;
    [EditorEnum( "退出军团失败" )]
    public const int EXIT_GROUP_FAIL           = 1017;
    [EditorEnum( "军团升级成功" )]
    public const int GROUP_LEVELUP_SUCCESS     = 1018;
    [EditorEnum( "军团升级失败，没有足够的军团活力" )]
    public const int GROUP_LEVELUP_FAIL        = 1019;
    [EditorEnum( "军团创建失败,名字过长或非法" )]
    public const int CREATE_GROUP_BAD_NAME     = 1020;
    [EditorEnum( "无法邀请不同阵营的玩家" )]
    public const int GROUP_ADD_INVALID_CAMP    = 1021;
    
    [EditorEnum( "发起军团战失败，错误的军团战天数" )]
    public const int GROUP_WAR_DAY_FAIL        = 1030;
    [EditorEnum( "发起军团战失败，目标军团无效" )]
    public const int GROUP_WAR_FAIL            = 1031;
    [EditorEnum( "发起军团战失败，你没有发起权限" )]
    public const int GROUP_WAR_PERMISSIONS_FAIL = 1032;
    [EditorEnum( "发起军团战失败，对方军团已达最大军团战数量" )]
    public const int GROUP_WAR_COUNT_FAIL      = 1033;
    [EditorEnum( "发起军团战失败，自己军团已达最大军团战数量" )]
    public const int GROUP_WAR_SELF_COUNT_FAIL = 1034;
    [EditorEnum( "发起军团战失败，军团活力不足" )]
    public const int GROUP_WAR_VIGOUR_FAIL     = 1035;
    [EditorEnum( "发起军团战失败，已经在军团战中" )]
    public const int HAS_GROUP_WAR             = 1036;

    [EditorEnum( "停止军团战失败，目标军团无效" )]
    public const int STOP_WAR_FIND_FAIL        = 1037;
    [EditorEnum( "停止军团战失败，你没有停止权限" )]
    public const int STOP_WAR_PERMISSIONS_FAIL = 1038;
    [EditorEnum( "停止军团战失败，不在军团战中，或本方军团不是发起方" )]
    public const int STOP_WAR_FAIL             = 1039;
    [EditorEnum( "发起军团战失败，没有必须物品" )]
    public const int GROUP_WAR_ITEM_FAIL       = 1040;
    [EditorEnum( "添加军团活力失败" )]
    public const int ADD_GROUP_VIGOUR_FAIL     = 1041;
    [EditorEnum( "军团已达最高等级" )]
    public const int GROUP_IS_MAX_LEVEL        = 1042;
    [EditorEnum( "发起军团战失败，不是同一阵营" )]
    public const int GROUP_WAR_CAMP_FAIL       = 1043;

    [EditorEnum("无效的任务")]
    public const int GROUP_QUEST_INVALID      = 1044;
    [EditorEnum("没有足够的军团资金")]
    public const int GROUP_QUEST_FAIL_MONEY     = 1045;
    //[EditorEnum("军团任务设置失败，军团等级不满足接取任务的条件")]
    //public const int GROUP_QUEST_FAIL_LVL       = 1046;
    [EditorEnum("你没有加入军团，无法进行捐赠")]
    public const int GROUP_DONATE_FAIL          = 1047;
    [EditorEnum("该道具无法进行军资捐献")]
    public const int GROUP_DONATE_FAIL_ITEM     = 1048;
    [EditorEnum("军团活跃值已到每日上限，无法进行捐赠")]
    public const int GROUP_DONATE_FAIL_LIMITED  = 1049;

    [EditorEnum("军团建筑升级失败，请先升级核心建筑")]
    public const int GROUP_BUILD_FAIL_LIMITED   = 1050;
    [EditorEnum("军团建筑升级失败，没有升级权限")]
    public const int GROUP_BUILD_FAIL           = 1051;

    [EditorEnum("你没有加入军团，无法进入军团领地")]
    public const int ENTER_GROUP_MAP_FAIL       = 1052;
    [EditorEnum("只有军团长才能购买军团领地")]
    public const int BUY_GROUP_MAP_FAIL         = 1053;
    [EditorEnum("已有军团领地，无法使用")]
    public const int HAS_GROUP_MAP              = 1054;
    [EditorEnum("添加军团领地成功")]
    public const int BUY_GROUP_MAP_SUCCESS      = 1055;
    [EditorEnum("此军团没有领地，无法进入")]
    public const int GROUP_MAP_NOT_EXIST        = 1056;
    [EditorEnum( "军团任务设置失败" )]
    public const int GROUP_QUEST_FAIL           = 1057;
    [EditorEnum( "军团活跃度已达到每日最大，不能再增加了" )]
    public const int GROUP_POINT_FAIL           = 1058;

    [EditorEnum( "还没有加入军团，无法领取" )]
    public const int GROUP_SOURCEOFMONEY_NOGROUP = 1059;
    [EditorEnum( "领取失败，今日已领取过了" )]
    public const int GROUP_SOURCEOFMONEY_ACCEPT = 1060;
    [EditorEnum( "无法领取，摇钱树还没有成熟" )]
    public const int GROUP_SOURCEOFMONEY_FULL   = 1061;

    [EditorEnum( "该道具无法进行军资捐献，道具品质必须为蓝色品质以上" )]
    public const int GROUP_DONATE_FAIL_ITEMQUALITY = 1063;
    [EditorEnum( "个人贡献已达到每日上限" )]
    public const int GROUP_CONTRIBUTION_FAIL = 1064;
    [EditorEnum( "你还没有加入军团，无法获取个人贡献度" )]
    public const int GROUP_CONTRIBUTION_NOGROUP = 1065;

    [EditorEnum("此任务每日发布次数已达上限")]
    public const int GROUP_QUEST_PUBLISH_COUNT_OVERFLOW = 1066;

    [EditorEnum( "你不是军团长" )]
    public const int NOT_GROUP_LEADER               = 1067;
    [EditorEnum( "军团联盟创建失败,名字过长或非法" )]
    public const int CREATE_GROUP_LEAGUE_BAD_NAME    = 1068;
    [EditorEnum( "已有同名军团联盟,创建失败" )]
    public const int CREATE_GROUP_LEAGUE_REPEAT_NAME = 1069;
    [EditorEnum( "已在军团联盟中,无法创建" )]
    public const int HAS_GROUP_LEAGUE                = 1070;
    [EditorEnum( "已在军团联盟中,无法解散" )]
    public const int DISMISS_GROUP_IN_LEAGUE         = 1071;
    [EditorEnum( "你不是军团联盟长,无法执行此操作" )]
    public const int NOT_GROUP_LEAGUE_LEADER         = 1072;
    [EditorEnum( "目标军团不存在,无法邀请" )]
    public const int GROUP_LEAGUE_INVITE_GROUP_NOT_EXISTS  = 1073;
    [EditorEnum( "目标军团军团长不在线,无法邀请" )]
    public const int GROUP_LEAGUE_INVITE_GROUP_LEADER_OFFLINE  = 1074;
    [EditorEnum( "目标军团联盟不存在" )]
    public const int GROUP_LEAGUE_NOT_EXISTS         = 1076;
    [EditorEnum( "目标军团联盟联盟长不在线" )]
    public const int GROUP_LEAGUE_LEADER_OFFLINE     = 1077;
    [EditorEnum( "无法设置此军团为新的联盟长军团" )]
    public const int CANNOT_SET_GROUP_LEAGUE_LEADER  = 1078;
    [EditorEnum( "无法踢出联盟长军团" )]
    public const int CANNOT_KICK_GROUP_LEAGUE_LEADER = 1079;
    [EditorEnum( "军团联盟长无法退出联盟" )]
    public const int GROUP_LEAGUE_LEADER_CANNOT_EXIT = 1080;
    [EditorEnum( "国公争夺战未开启" )]
    public const int GROUP_LEAGUE_WAR_NOT_BEGIN      = 1081;
    [EditorEnum( "你不在军团联盟中,无法参加国公争夺战" )]
    public const int CANNOT_JOIN_GROUP_LEAGUE_WAR    = 1082;
    [EditorEnum( "军团联盟无法邀请不同阵营的军团" )]
    public const int INVITE_GROUP_NOT_SAME_CAMP      = 1083;
    [EditorEnum( "无法申请加入不同阵营的军团联盟" )]
    public const int APPLY_GROUP_NOT_SAME_CAMP       = 1084;
    [EditorEnum( "军团联盟已满" )]
    public const int GROUP_LEAGUE_IS_FULL            = 1085;
    [EditorEnum( "军团已在军团联盟中" )]
    public const int GROUP_IS_IN_LEAGUE              = 1086;

    [EditorEnum("没有发布军团任务的权限")]
    public const int GROUP_QUEST_DENIED = 1087;
    [EditorEnum("没有足够的军团活跃度")]
    public const int GROUP_QUEST_FAIL_VIGOUR = 1089;

    [EditorEnum("该军团不允许被加入，或您的等级不满足条件")]
    public const int GROUP_CAN_NOT_BE_JOIN = 1090;

    [EditorEnum("军团不存在或已经解散")]
    public const int GROUP_NOT_EXISTS = 1091;
    
    [EditorEnum( "无法设置官员为不同阵营玩家" )]
    public const int SET_OFFICIAL_POSITION_CAMP_ERROR = 1100;
    [EditorEnum( "此职位已满或玩家已有职位,无法设置" )]
    public const int SET_OFFICIAL_POSITION_ERROR      = 1101;
    [EditorEnum( "任职当天无法领取，或今天已领取过俸禄" )]
    public const int COUNTRY_GET_PAY_ERROR            = 1102;
    [EditorEnum( "已到最高军衔,无法提升" )]
    public const int IS_MAX_RANK                      = 1103;
    [EditorEnum( "道具与军衔不匹配,无法使用" )]
    public const int RANK_ITEM_ERROR                  = 1104;
    [EditorEnum( "禁言次数已达上限" )]
    public const int OUT_DISABLE_COUNT                = 1105;
    [EditorEnum( "没有发布公告的权限" )]
    public const int CANNOT_PUTOUT_NOTICE             = 1106;
    [EditorEnum( "没有任命官职的权限" )]
    public const int CANNOT_SET_OFFICIAL_POSITION     = 1107;
    [EditorEnum( "不能禁言其它阵营玩家" )]
    public const int CANNOT_DISABLE_OTHER_CAMP        = 1108;
    [EditorEnum( "没有申请国战的权限" )]
    public const int CANNOT_BEGIN_COUNTRY_WAR         = 1109;
    [EditorEnum( "仅在可宣战日的00:00-19:00可进行宣战" )]
    public const int BEGIN_COUNTRY_WAR_TIME_ERROR     = 1110;
    [EditorEnum( "今天不是本国的宣战日,不能宣战" )]
    public const int BEGIN_COUNTRY_WAR_CAMP_ERROR     = 1111;
    [EditorEnum( "今日已宣战,无法再宣战" )]
    public const int TODAY_HAS_COUNTRY_WAR            = 1112;
    [EditorEnum( "没有足够的国库资金,无法宣战" )]
    public const int BEGIN_COUNTRY_WAR_MONEY_ERROR    = 1113;
    [EditorEnum( "等级不够,不能参加国战" )]
    public const int COUNTRY_WAR_PLAYER_LEVEL_ERROR   = 1114;
    [EditorEnum( "你的国家并无战事" )]
    public const int ENTRY_COUNTRY_WAR_ERROR          = 1115;
    [EditorEnum( "国战地图尚未开启,请在活动时间再来" )]
    public const int CANNOT_ENTER_COUNTRY_WAR_MAP     = 1117;
    
    //骑马
    [EditorEnum( " 没有骑乘装备 ")]         
    public const int HORSE_ISNOT_EQUIPED       = 1201;
    [EditorEnum( " 当前状态不能使用骑乘")]
    public const int CUR_STATE_NOTALLOWED_HORSE= 1202;
    [EditorEnum( " 当前没有骑马 ")]
    public const int NOT_ONHORSE               = 1203;
    [EditorEnum( " 你已经骑马了")]
    public const int ALREADY_ONHORSE           = 1204;

    // 杂项
    [EditorEnum( "序号输入有误，请重新输入后再试" )]
    public const int MISC_CARD                 = 1301;
    [EditorEnum( "使用序号没有成功，输入的序号已使用过了，或者账号已经使用过序号了" )]
    public const int MISC_CARD_USED            = 1302;
    [EditorEnum( "序号使用成功" )]
    public const int MISC_CARD_OK              = 1303;
    [EditorEnum( "使用序号没有成功，输入的序号已过期" )]
    public const int MISC_CARD_EXPIRED         = 1304;
    [EditorEnum( "使用序号没有成功，同类型的序号你已使用过一次了" )]
    public const int MISC_CARD_USEDBYTYPE      = 1305;
    [EditorEnum( "使用序号没有成功，输入的序号已使用过了" )]
    public const int MISC_CARD_USED_EXT        = 1306;
    [EditorEnum( "使用序号没有成功，你不是序号的拥有者" )]
    public const int MISC_CARD_UNAUTHORIZED    = 1307;
    [EditorEnum( "背包空间不足，不能签到" )]
    public const int MISC_SIGN_BAGSIZE         = 1308;
    [EditorEnum( "使用序号没有成功，该序列号不能在本服使用" )]
    public const int MISC_CARD_UNAUTHORIZED_SID    = 1309;


    [EditorEnum( "没有发布国家任务的权限" )]
    public const int Country_Quest_Power       = 1400;
    [EditorEnum( "国家任务发布失败，没有足够的金钱" )]
    public const int Country_Quest_Money       = 1401;

#if !SERVER
        private static Dictionary<int, string> hints = null;
        public static void InitErrorMsg() {
            hints = new Dictionary<int, string>( );
            Type type = typeof( StrDef );
            foreach( FieldInfo fi in type.GetFields() ) {
                EditorEnumAttribute att = Attribute.GetCustomAttribute( fi, typeof( EditorEnumAttribute ) )
                    as EditorEnumAttribute;
                if( att == null )
                    continue;
                int index = (int)fi.GetValue( null );

                if ( hints.ContainsKey(index) )
                    throw new Exception( string.Format( "StrDef.InitErrorMsg error, index {0} has exists", index ) );
                hints[index] = att.Name;
            }
        }

        public static string ErrorMsg( int errorCode ) {
            string hint = "";
            if ( hints.TryGetValue( errorCode, out hint ) )
                return hint;
            else
                return string.Format( "未知错误: {0}", errorCode.ToString() );
        }
#endif
}

public partial class EventMessageDef {
    public const int GET_ITEM_FROM_TREASURY = 0;
}