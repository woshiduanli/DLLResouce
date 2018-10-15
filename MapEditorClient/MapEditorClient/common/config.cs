using System;
using System.ComponentModel;
using System.Collections.Generic;


public class GameConfig {
    [CategoryAttribute( "恶名值" ), DescriptionAttribute( "等级差30~60恶名值" )]
    public IntArray kill_point_30_60 { get; set; }
    [CategoryAttribute( "恶名值" ), DescriptionAttribute( "等级差60以上" )]
    public IntArray kill_point_60 { get; set; }

    [CategoryAttribute( "经验" ), DescriptionAttribute( "与怪物等级差,经验衰减" )]
    public IntArray delta_level_exp { get; set; }
    [CategoryAttribute( "耐久" ), DescriptionAttribute( "战斗中耐久消耗" )]
    public EquipEndureCost equip_endure_cost { get; set; }
    [CategoryAttribute( "字符串" ), DescriptionAttribute( "服务端用到的字符串常量" )]
    public ConstString strings { get; set; }

    [CategoryAttribute( "战场" ), DescriptionAttribute( "战场通用配置" )]
    public BattlefieldConfig battlefield { get; set; }
    [CategoryAttribute( "战场" ), DescriptionAttribute( "冀州战场配置" )]
    public JiZhouBattlefieldConfig jizhou_battlefield { get; set; }
    [CategoryAttribute( "战场" ), DescriptionAttribute( "黄巾战场配置" )]
    public HuangJinBattlefieldConfig huangjin_battlefield { get; set; }

    [CategoryAttribute( "礼包卡" ), DescriptionAttribute( "礼包卡配置" )]
    public CardMgr card_mgr { get; set; }
    [CategoryAttribute( "IB商城" ), DescriptionAttribute( "IB商城配置" )]
    public IBStoreSettings store_setting { get; set; }
    [CategoryAttribute( "军团" ), DescriptionAttribute( "军团配置" )]
    public GroupSetting group_setting { get; set; }
    [CategoryAttribute( "换车令" ), DescriptionAttribute( "换车令镖车列表" )]
    public IntArray carriage_list { get; set; }
    [CategoryAttribute( "极品镖车" ), DescriptionAttribute( "极品镖车列表" )]
    public IntArray best_carriage_list { get; set; }

    [CategoryAttribute( "清除技能CD列表" ), DescriptionAttribute( "技能清除特定技能CD" )]
    public IntArray clear_cd_skill_list { get; set; }
    [CategoryAttribute( "关系" ), DescriptionAttribute( "关系相应配置" )]
    public RelationshipConfig relationship { get; set; }

    [CategoryAttribute("离线经验"), DescriptionAttribute("换取道具")]
    public OfflineExpItemCfgArray OfflineExpItems { get; set; }
    [CategoryAttribute("离线经验"), DescriptionAttribute("等级对应")]
    public OfflineExpPerLevelArray OfflineExpPerLevels { get; set; }

    [CategoryAttribute("军团联盟"), DescriptionAttribute("军团联盟")]
    public GroupLeagueConfig group_league { get; set; }
    [CategoryAttribute("国家"), DescriptionAttribute("国家")]
    public CountryConfig country { get; set; }

    [CategoryAttribute( "道具检取通知" ), DescriptionAttribute( "道具ID列表" )]
    public IntArray pickup_item_notice { get; set; }
    
    [CategoryAttribute( "技能学习配置" ), DescriptionAttribute( "技能学习" )]
    public LearnSkillConfig learn_skill { get; set; }

    [CategoryAttribute("进入地图自动装备"), DescriptionAttribute("进入地图自动装备")]
    public AutoEquipWhenEnterMapConfig auto_equip_when_enter_map_config { get; set; }

    [CategoryAttribute( "签到" ), DescriptionAttribute( "签到" )]
    public SignManager sign_manager { get; set; }

    public GameConfig() {
        kill_point_30_60 = new IntArray();
        kill_point_60 = new IntArray();
        delta_level_exp = new IntArray();
        carriage_list = new IntArray();
        best_carriage_list = new IntArray();
        clear_cd_skill_list = new IntArray();
        pickup_item_notice = new IntArray();

        learn_skill = new LearnSkillConfig();
        equip_endure_cost = new EquipEndureCost();
        strings = new ConstString();
        battlefield = new BattlefieldConfig();
        jizhou_battlefield = new JiZhouBattlefieldConfig();
        huangjin_battlefield = new HuangJinBattlefieldConfig();
        card_mgr = new CardMgr();
        group_setting = new GroupSetting();
        store_setting = new IBStoreSettings();
        relationship = new RelationshipConfig();

        OfflineExpItems = new OfflineExpItemCfgArray();
        OfflineExpPerLevels = new OfflineExpPerLevelArray();

        group_league = new GroupLeagueConfig();
        country = new CountryConfig();

        auto_equip_when_enter_map_config = new AutoEquipWhenEnterMapConfig();
        sign_manager = new SignManager();
    }
}

[TypeConverter( typeof( ExpandableObjectConverter ) )]
public class IntArray {
    [DescriptionAttribute( "数组" )]
    public int[] data { get; set; }
}

[TypeConverter( typeof( ExpandableObjectConverter ) )]
public class EquipEndureCost {
    [DescriptionAttribute( "武器耐久消耗概率(万分比)" )]
    public int weapon_rate { get; set; }
    [DescriptionAttribute( "防具耐久消耗概率(万分比)" )]
    public int shiled_rate { get; set; }
    [DescriptionAttribute( "每次耐久消耗值" )]
    public int endure_value { get; set; }
    [DescriptionAttribute( "普通复活耐久扣除修正" )]
    public int endure_rate_with_normal_relive { get; set; }
    [DescriptionAttribute( "战场复活耐久扣除修正" )]
    public int endure_rate_with_battlefield_relive { get; set; }
}

[TypeConverter( typeof( ExpandableObjectConverter ) )]
public class CardSetting {
    [DescriptionAttribute( "礼包卡类型" )]
    public string ItemFlag { get; set; }
    [DescriptionAttribute( "道具ID" )]
    public int ItemId { get; set; }
}

[TypeConverter( typeof( ExpandableObjectConverter ) )]
public class SignManager {
    [DescriptionAttribute( "签到配置" )]
    public SignSetting[] sign_setting { get; set; }

    public SignSetting GetConfig(int sign_count , int level) {
        SignSetting rs = null;
        foreach ( SignSetting setting in sign_setting ) {
            if ( setting.sign_count != sign_count )
                continue;

            if ( setting.level > level )
                continue;

            if ( rs == null ) {
                rs = setting;
                continue;
            }

            if ( rs.CompareTo( setting ) < 0 )
                rs = setting;
        }
        return rs;
    }
}

[TypeConverter( typeof( ExpandableObjectConverter ) )]
public class SignItemInfo {
    [DescriptionAttribute( "道具ID" )]
    public int id { get; set; }
    [DescriptionAttribute( "道具数量" )]
    public int count { get; set; }
}

[TypeConverter( typeof( ExpandableObjectConverter ) )]
public class SignSetting : IComparable{
    [DescriptionAttribute( "签到天数" )]
    public int sign_count { get; set; }
    [DescriptionAttribute( "等级" )]
    public int level { get; set; }
    [DescriptionAttribute( "道具" )]
    public SignItemInfo[] items { get; set; }

    public int CompareTo( object obj ) {
        SignSetting sign_ = obj as SignSetting;
        if ( sign_count != sign_.sign_count )
            return sign_count.CompareTo( sign_.sign_count );

        return level.CompareTo( sign_.level );            
    }
}

[TypeConverter( typeof( ExpandableObjectConverter ) )]
public class RelationshipConfig {
    [DescriptionAttribute( "仇敌过期时间" )]
    public int ExpireEnemy { get; set; }
    [DescriptionAttribute( "仇敌上限" )]
    public int MaxEnemy { get; set; }
    [DescriptionAttribute( "黑名单上限" )]
    public int MaxBlack { get; set; }
    [DescriptionAttribute( "联系人上限" )]
    public int MaxFriend { get; set; }
    [DescriptionAttribute( "师父所需等级" )]
    public int MinMasterLevel { get; set; }
    [DescriptionAttribute( "徒弟最高等级" )]
    public int MaxApprenticeLevel { get; set; }
    [DescriptionAttribute( "结拜所需等级" )]
    public int MinBrotherLevel { get; set; }
    [DescriptionAttribute( "情侣所需等级" )]
    public int MinLoverLevel { get; set; }
}

[TypeConverter( typeof( ExpandableObjectConverter ) )]
public class GroupSetting {
    [DescriptionAttribute( "道具捐献 活跃度 百分比，例:200 -> 200%，捐献得到的 活跃度 为（道具分数 * 200%）" )]
    public int DonatePoint { get; set; }

    [DescriptionAttribute( "道具捐献 个人贡献 百分比，例:200 -> 200%，捐献得到的 个人贡献 为（道具分数 * 200%）" )]
    public int DonateContribution { get; set; }

    [DescriptionAttribute( "军团活跃度每日上限" )]
    public GroupPoint[] points { get; set; }

    [DescriptionAttribute( "军团捐赠道具" )]
    public GroupDonate[] donate { get; set; }

    [DescriptionAttribute( "军团摇钱树成熟值" )]
    public int SourceOfMoneyMax { get; set; }
    [DescriptionAttribute( "军团个人贡献每日上限" )]
    public int DonateNum { get; set; }
    [DescriptionAttribute( "军团建筑升级" )]
    public GroupBuildingPoint[] building_point { get; set; }
}

[TypeConverter( typeof( ExpandableObjectConverter ) )]
public class GroupPoint {
    [DescriptionAttribute( "军团等级" )]
    public int level { get; set; }
    [DescriptionAttribute( "军团活跃度每日最大上限" )]
    public int VigourPointDayMax { get; set; }
}

[TypeConverter( typeof( ExpandableObjectConverter ) )]
public class GroupDonate {
    [DescriptionAttribute( "道具ID" )]
    public int item_id { get; set; }
    [DescriptionAttribute( "捐献值" )]
    public int value { get; set; }
}

[TypeConverter( typeof( ExpandableObjectConverter ) )]
public class GroupBuildingPoint {
    [DescriptionAttribute( "等级" )]
    public int level { get; set; }
    [DescriptionAttribute( "消耗活跃度" )]
    public int vigour_point { get; set; }
}

[TypeConverter( typeof( ExpandableObjectConverter ) )]
public class IBStoreSettings {
    [DescriptionAttribute( "商城邮件标题，本人购买背包满的情况" )]
    public string title { get; set; }

    [DescriptionAttribute( "商城邮件标题，赠送" )]
    public string title_gift { get; set; }

    [DescriptionAttribute( "商城邮件内容" )]
    public string content { get; set; }
}

[TypeConverter( typeof( ExpandableObjectConverter ) )]
public class CardMgr {
    [DescriptionAttribute( "礼包卡邮件标题" )]
    public string title { get; set; }

    [DescriptionAttribute( "礼包卡邮件内容" )]
    public string content { get; set; }

    [DescriptionAttribute( "礼包卡配置" )]
    public CardSetting[] cards { get; set; }

    public CardSetting GetSetting( string card_type ) {
        for ( int i = 0; i < this.cards.Length; i++ ) {
            if ( !card_type.ToLower().Equals( this.cards[i].ItemFlag.ToLower() ) )
                continue;

            return cards[i];
        }

        return null;
    }
}

[TypeConverter( typeof( ExpandableObjectConverter ) )]
public class ConstString {
    [DescriptionAttribute( "停机维护提示(分)" )]
    public string ShutdownHintMinute { get; set; }
    [DescriptionAttribute( "停机维护提示(秒)" )]
    public string ShutdownHintSecond { get; set; }
    [DescriptionAttribute( "公会战开始提示" )]
    public string GroupWarBeginHint { get; set; }
    [DescriptionAttribute( "公会战结束提示" )]
    public string GroupWarStopHint { get; set; }
    [DescriptionAttribute( "马车名" )]
    public string CarriageName { get; set; }
    [DescriptionAttribute( "副本奖励邮件头" )]
    public string DungeonAwardMailTitle { get; set; }
    [DescriptionAttribute( "副本奖励邮件内容" )]
    public string DungeonAwardMailContent { get; set; }
    [DescriptionAttribute( "师徒奖励邮件头" )]
    public string MasterAwardMailTitle { get; set; }
    [DescriptionAttribute( "师徒奖励邮件内容" )]
    public string MasterAwardMailContent { get; set; }
    [DescriptionAttribute( "计划任务开始提示" )]
    public string ScheduledBeginNotify { get; set; }
    [DescriptionAttribute( "师徒" )]
    public string RelationMasterAndApprentic { get; set; }
    [DescriptionAttribute( "情侣" )]
    public string RelationLover { get; set; }
    [DescriptionAttribute( "结义" )]
    public string RelationBrother { get; set; }
    [DescriptionAttribute( "关系解除" )]
    public string FmtMailtitleRelationRemove { get; set; }
    [DescriptionAttribute( "关系时间" )]
    public string FmtTimeDesc { get; set; }
    [DescriptionAttribute( "解除关系" )]
    public string FmtMailcontentRelationRemove1 { get; set; }
    [DescriptionAttribute( "解除关系" )]
    public string FmtMailcontentRelationRemove2 { get; set; }

    [DescriptionAttribute( "军团" )]
    public string Group { get; set; }
    [DescriptionAttribute( "军团创建失败提示" )]
    public string CreateGroupFail { get; set; }

    [DescriptionAttribute( "副本完成提示" )]
    public string DungeonFinishedHint { get; set; }
    [DescriptionAttribute( "副本失败提示" )]
    public string DungeonFailHint { get; set; }
    [DescriptionAttribute( "副本关闭提示" )]
    public string DungeonCloseHint { get; set; }

    [DescriptionAttribute( "补偿道具邮件标题" )]
    public string CompensateItemMailTitle { get; set; }
    [DescriptionAttribute( "补偿道具邮件内容" )]
    public string CompensateItemMailContent { get; set; }

    [DescriptionAttribute( "军团夺取地图提示" )]
    public string SetMapOwnerGroupHint { get; set; }
    [DescriptionAttribute( "个人夺取地图提示" )]
    public string SetMapOwnerHint { get; set; }

    [DescriptionAttribute( "无防沉迷上线提示" )]
    public string NoAntiAddictionOnlineHint { get; set; }
    [DescriptionAttribute( "防沉迷上线提示" )]
    public string AntiAddictionOnlineHint { get; set; }
    [DescriptionAttribute( "防沉迷收益减半提示" )]
    public string IncomeHalfHint { get; set; }
    [DescriptionAttribute( "防沉迷无收益提示" )]
    public string IncomeZeroHint { get; set; }
    [DescriptionAttribute( "防沉迷一小时提示" )]
    public string OneHourHint { get; set; }
    [DescriptionAttribute( "防沉迷三小时提示" )]
    public string ThreeHourHint { get; set; }
}


[TypeConverter( typeof( ExpandableObjectConverter ) )]
public class GroupLeagueConfig {
    [DescriptionAttribute( "创建金钱" )]
    public int CreateMoney { get; set; }
    [DescriptionAttribute( "军团联盟" )]
    public string GroupLeague { get; set; }
    [DescriptionAttribute( "创建失败邮件内容" )]
    public string CreateLeagueFail { get; set; }
    [DescriptionAttribute( "军团联盟创建系统消息" )]
    public string CreateLeagueSystem { get; set; }

    [DescriptionAttribute( "军团联盟战地图" )]
    public int[] GroupLeagueWarMapRef { get; set; }
    [DescriptionAttribute( "军团联盟战建筑积分增加速度 n/分" )]
    public int GroupLeagueWarBuildingAddScore { get; set; }
    [DescriptionAttribute( "国公争夺战失效提示" )]
    public string GroupLeagueWarWinnerInvalid { get; set; }
}

[TypeConverter( typeof( ExpandableObjectConverter ) )]
public class OfficialPositionConfig {
    [DescriptionAttribute( "官职ID" )]
    public int id { get; set; }
    [DescriptionAttribute( "官职名称" )]
    public string name { get; set; }
    [DescriptionAttribute( "数量" )]
    public int count { get; set; }
    [DescriptionAttribute( "职位BUFF ID" )]
    public int buff_ref_id { get; set; }
    [DescriptionAttribute( "工资(金)" )]
    public int pay { get; set; }
    [DescriptionAttribute( "是否国公" )]
    public bool is_king { get; set; }

    [DescriptionAttribute( "能否发布任务" )]
    public bool can_putout_quest { get; set; }
    [DescriptionAttribute( "能否发动战争" )]
    public bool can_begin_war { get; set; }
    [DescriptionAttribute( "能否任命职务" )]
    public bool can_set_position { get; set; }
    [DescriptionAttribute( "能否发布公告" )]
    public bool can_putout_notice { get; set; }
    [DescriptionAttribute( "能否禁言" )]
    public bool can_disable_chat { get; set; }
    [DescriptionAttribute( "能否召唤玩家" )]
    public bool can_summon_player { get; set; }

    [DescriptionAttribute("头顶图标名")]
    public string icon { get; set; }
}


[TypeConverter( typeof( ExpandableObjectConverter ) )]
public class RankConfig {
    [DescriptionAttribute( "军衔名" )]
    public string name { get; set; }
    [DescriptionAttribute( "升级所需道具" )]
    public int item_ref_id { get; set; }
    [DescriptionAttribute( "军衔BUFF" )]
    public int buff_ref_id { get; set; }
    [DescriptionAttribute("说明文字")]
    public string desc { get; set; }
}

[TypeConverter( typeof( ExpandableObjectConverter ) )]
public class CountryWarConfig {
    [DescriptionAttribute( "发起阵营" )]
    public int ApplyCamp { get; set; }
    [DescriptionAttribute( "目标阵营" )]
    public int TargetCamp { get; set; }
}

[TypeConverter( typeof( ExpandableObjectConverter ) )]
public class CountryConfig {
    [DescriptionAttribute( "官职" )]
    public OfficialPositionConfig[] official_position { get; set; }
    [DescriptionAttribute( "军衔" )]
    public RankConfig[] rank { get; set; }
    [DescriptionAttribute( "国公将魂" )]
    public int[] king_equip_refid { get; set; }
    [DescriptionAttribute( "国公将魂奖励标题" )]
    public string king_equip_mail_title { get; set; }
    [DescriptionAttribute( "国公将魂奖励内容" )]
    public string king_equip_mail_content { get; set; }
    [DescriptionAttribute( "每天禁言次数" )]
    public int disable_chat_count { get; set; }
    [DescriptionAttribute( "每天召唤次数" )]
    public int summon_player_count { get; set; }
    [DescriptionAttribute( "升级所需经验" )]
    public int[] levelup_exp { get; set; }
    [DescriptionAttribute( "每天维护资金" )]
    public int maintain_money { get; set; }

    [DescriptionAttribute( "国战发起配置,第一周" )]
    public CountryWarConfig[] country_war_1 { get; set; }
    [DescriptionAttribute( "国战发起配置,第二周" )]
    public CountryWarConfig[] country_war_2 { get; set; }
    [DescriptionAttribute( "国战发起配置,第三周" )]
    public CountryWarConfig[] country_war_3 { get; set; }
    [DescriptionAttribute( "国战发起配置,资金" )]
    public int country_war_money { get; set; }
    [DescriptionAttribute( "国战发起配置,通知" )]
    public string country_war_notice { get; set; }
    [DescriptionAttribute( "国战地图ID" )]
    public int[] country_war_map{ get; set; }
    [DescriptionAttribute( "国战胜利提示" )]
    public string country_war_win_hint{ get; set; }
    [DescriptionAttribute( "国战据点ref id" )]
    public int[] country_war_npc_id{ get; set; }

    [DescriptionAttribute( "国战奖励经验等级" )]
    public int[] country_war_exp{ get; set; }
    [DescriptionAttribute( "国战奖励荣誉等级" )]
    public int[] country_war_honor{ get; set; }
}

[TypeConverter( typeof( ExpandableObjectConverter ) )]
public class BattlefieldBuffConfig {
    [DescriptionAttribute( "最小平均等级" )]
    public int MinLevel { get; set; }
    [DescriptionAttribute( "最大平均等级" )]
    public int MaxLevel { get; set; }
    [DescriptionAttribute( "Buff1" )]
    public int Buff1Ref { get; set; }
    [DescriptionAttribute( "Buff1等级" )]
    public int Buff1Level { get; set; }
    [DescriptionAttribute( "Buff2" )]
    public int Buff2Ref { get; set; }
    [DescriptionAttribute( "Buff2等级" )]
    public int Buff2Level { get; set; }
    [DescriptionAttribute( "Buff3" )]
    public int Buff3Ref { get; set; }
    [DescriptionAttribute( "Buff3等级" )]
    public int Buff3Level { get; set; }
    [DescriptionAttribute( "Buff4" )]
    public int Buff4Ref { get; set; }
    [DescriptionAttribute( "Buff4等级" )]
    public int Buff4Level { get; set; }
}

[TypeConverter( typeof( ExpandableObjectConverter ) )]
public class BattlefieldConfig {
    [DescriptionAttribute( "第一阶连续击杀数量" )]
    public int Level1KillCount { get; set; }
    [DescriptionAttribute( "第一阶连续击杀提示" )]
    public string Level1KillHint { get; set; }
    [DescriptionAttribute( "第二阶连续击杀数量" )]
    public int Level2KillCount { get; set; }
    [DescriptionAttribute( "第二阶连续击杀提示" )]
    public string Level2KillHint { get; set; }
    [DescriptionAttribute( "第三阶连续击杀数量" )]
    public int Level3KillCount { get; set; }
    [DescriptionAttribute( "第三阶连续击杀提示" )]
    public string Level3KillHint { get; set; }
    [DescriptionAttribute( "NPC自适应Buff" )]
    public BattlefieldBuffConfig[] buff { get; set; }
}

[TypeConverter( typeof( ExpandableObjectConverter ) )]
public class BattlefieldFlagConfig {
    [DescriptionAttribute( "计分" )]
    public int score { get; set; }
    [DescriptionAttribute( "旗帜ID" )]
    public int flag_refid { get; set; }
}

[TypeConverter( typeof( ExpandableObjectConverter ) )]
public class JiZhouBattlefieldConfig {
    [DescriptionAttribute( "胜利方战勋" )]
    public int WinBattlefieldValue { get; set; }
    [DescriptionAttribute( "失败方战勋" )]
    public int FailBattlefieldValue { get; set; }
    [DescriptionAttribute( "平局战勋" )]
    public int DrawBattlefieldValue { get; set; }
    [DescriptionAttribute( "第一名战勋" )]
    public int RankBattlefieldValue1 { get; set; }
    [DescriptionAttribute( "第二名战勋" )]
    public int RankBattlefieldValue2 { get; set; }
    [DescriptionAttribute( "第三至五名战勋" )]
    public int RankBattlefieldValue3 { get; set; }
    [DescriptionAttribute( "第六至十名战勋" )]
    public int RankBattlefieldValue4 { get; set; }

    [DescriptionAttribute( "胜利条件分数" )]
    public int WinScore { get; set; }

    [DescriptionAttribute( "阵营0旗帜返回点X" )]
    public int Camp0FlagDestX { get; set; }
    [DescriptionAttribute( "阵营0旗帜返回点Y" )]
    public int Camp0FlagDestY { get; set; }
    [DescriptionAttribute( "阵营0旗帜计分" )]
    public BattlefieldFlagConfig[] Camp0Flag { get; set; }

    [DescriptionAttribute( "阵营1旗帜返回点X" )]
    public int Camp1FlagDestX { get; set; }
    [DescriptionAttribute( "阵营1旗帜返回点Y" )]
    public int Camp1FlagDestY { get; set; }
    [DescriptionAttribute( "阵营1旗帜计分" )]
    public BattlefieldFlagConfig[] Camp1Flag { get; set; }

    [DescriptionAttribute( "旗帜名字" )]
    public string FlagName { get; set; }
    [DescriptionAttribute( "夺旗成功提示" )]
    public string FlagHint { get; set; }

    [DescriptionAttribute( "战场地图ID" )]
    public int[] MapRefs { get; set; }
}

[TypeConverter( typeof( ExpandableObjectConverter ) )]
public class HuangJinBattlefieldConfig {
    [DescriptionAttribute( "胜利方战勋" )]
    public int WinBattlefieldValue { get; set; }
    [DescriptionAttribute( "失败方战勋" )]
    public int FailBattlefieldValue { get; set; }
    [DescriptionAttribute( "平局战勋" )]
    public int DrawBattlefieldValue { get; set; }
    [DescriptionAttribute( "第一名战勋" )]
    public int RankBattlefieldValue1 { get; set; }
    [DescriptionAttribute( "第二名战勋" )]
    public int RankBattlefieldValue2 { get; set; }
    [DescriptionAttribute( "第三至五名战勋" )]
    public int RankBattlefieldValue3 { get; set; }
    [DescriptionAttribute( "第六至十名战勋" )]
    public int RankBattlefieldValue4 { get; set; }

    [DescriptionAttribute( "胜利条件分数" )]
    public int WinScore { get; set; }

    [DescriptionAttribute( "战场地图ID" )]
    public int[] MapRefs { get; set; }
}


#region 技能学习配置
[TypeConverter(typeof(ExpandableObjectConverter))]
public class LearnSkill {
    [DescriptionAttribute( "玩家等级" )]
    public int Level { get; set; }
    [DescriptionAttribute( "技能RefID" )]
    public int SkillRef { get; set; }
}


[TypeConverter(typeof(ExpandableObjectConverter))]
public class LearnSkillConfig {
    [DescriptionAttribute( "豪杰" )]
    public LearnSkill[] warrior { get; set; }
    [DescriptionAttribute("月翎")]
    public LearnSkill[] mage { get; set; }
    [DescriptionAttribute("天机")]
    public LearnSkill[] warlock { get; set; }
}

#endregion

#region 离线经验换取道具

[TypeConverter(typeof(ExpandableObjectConverter))]
public class OfflineExpItemCfg {
    [DescriptionAttribute("道具ID")]
    public int ItemID { get; set; }

    [DescriptionAttribute("换取离线经验系数 （万分比）")]
    public int Ratio  { get; set; }
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public class OfflineExpItemCfgArray {
    [DescriptionAttribute("离线经验道具配置")]
    public OfflineExpItemCfg[] ItemList { get; set; }
}

#endregion

#region 离线经验是“每等级兑换经验”表

[TypeConverter(typeof(ExpandableObjectConverter))]
public class OfflineExpPerLevel {
    [DescriptionAttribute("玩家等级")]
    public int PlayerLevel { get; set; }

    [DescriptionAttribute("每小时兑换经验值")]
    public int ExpPerHour { get; set; }
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public class OfflineExpPerLevelArray {
    [DescriptionAttribute("等级对应离线经验")]
    public OfflineExpPerLevel[] LevelList { get; set; }
}

#endregion

[TypeConverter(typeof(ExpandableObjectConverter))]
public class AutoEquipWhenEnterMapConfig {
    [DescriptionAttribute("地图ID")]
    public int MapId { get; set; }

    [DescriptionAttribute("自动装备道具ID列表")]
    public int[] ItemIdList { get; set; }
}