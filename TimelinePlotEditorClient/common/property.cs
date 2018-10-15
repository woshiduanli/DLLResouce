using System;
using System.Collections.Generic;
using System.Reflection;

public partial class PropDefine {
    public virtual void LoadReference( ResourceUtilReader resource_reader ) {}
    public virtual bool LoadConfig() { return true; }
}

// 角色配置表
public partial class RoleReference : PropDefine {
    [Editor( "ID,40" )]
    public short ID;
    [Editor( "名字,100" )]
    public string Name;
    [Editor( "版本,60" )]
    public short Version;
    [Editor( "版本类型,60", "EditionType" )]
    public short EditionType;
    [Editor( "称号,100" )]
    public string Title;
    [Editor( "外观ID,60" )]
    public int Apprid;
    [Editor( "类型,70", "GameObjectType" )]
    public short type;
    [Editor( "性别,70", "RoleGender" )]
    public short gender;
    [Editor( "职业,70", "RoleOccupation" )]
    public short occupation;
    [Editor( "等级,60" )]
    public short level;
    [Editor( "下一级经验,80" )]
    public int nextlevelexp;

    [Editor("死亡经验,70")]
    public int deathexp;
    [Editor( "重生时间间隔(秒),90" )]
    public int reliveinterval;
    [Editor( "身体半径,90" )]
    public int BodyRadius;

    [Editor("基准生命值上限,80")]
    public int BaseMaxHp;
    [Editor("基准生命值回复值,80")]
    public short BaseHpRet;
    [Editor("基准普通攻击,80")]
    public short BaseNormalAtk;
    [Editor("基准普通防御,80")]
    public short BaseNormalDef;
    [Editor("基准移动速度,80")]
    public short BaseWalkSpeed;
    [Editor("基准攻击速度,80")]
    public short BaseAtkSpeed;
    [Editor("基准命中值,80")]
    public short BaseHitValue;
    [Editor("基准闪避值,80")]
    public short BaseDodgeValue;
    [Editor("基准暴击值,80")]
    public short BaseCritValue;
    [Editor("基准免暴值,80")]
    public short BaseCritAvoidValue;
    [Editor("基准暴击伤害倍率,80")]
    public short BaseCritMulti;
    [Editor("基准风系攻击,80")]
    public short BaseWindAtk;
    [Editor("基准雷系攻击,80")]
    public short BaseThunderAtk;
    [Editor("基准火系攻击,80")]
    public short BaseFireAtk;
    [Editor("基准风系抗性,80")]
    public short BaseWindDef;
    [Editor("基准雷系抗性,80")]
    public short BaseThunderDef;
    [Editor("基准火系抗性,80")]
    public short BaseFireDef;

    // 怪物,NPC相关
    [Editor( "怪物类型,80", "MonsterType" )]
    public short monster_type;
    [Editor( "怪物难度,80", "MonsterLevel" )]
    public short monster_level;

    [Editor("对话内容,120")]
    public string chat_text;
    [Editor( "对话菜单,60" )]
    public NPCChatConfig[] npc_chat;
    [Editor( "掉落表,60" )]
    public DropTableConfig[] drop_item;
    [Editor( "技能学习,60" )]
    public SkillStudyConfig[] skill_study;
    [Editor( "代币类型,90", "CurrencyType" )]
    public short currency_type;
    [Editor( "物品售卖,60" )]
    public ItemSaleConfig[] item_sale;
    [Editor( "售卖价格比率,60" )]
    public int item_sale_rate;
    [Editor( "回收价格比率,60" )]
    public int item_buy_rate;

    [Editor( "副本创建,60" )]
    public CreateDungeonConfig[] create_dungeon;

    //-----AI--------
    [Editor( "AI|AI类型,80", "NPCAIType" )]
    public short AIType;
    [Editor( "AI|逃跑,60" )]
    public bool Escape;
    [Editor( "AI|主动攻击,60" )]
    public bool AutoAttack;
    [Editor( "AI|警戒范围,60" )]
    public int AutoAttackDistance;
    [Editor( "AI|追踪范围,60" )]
    public int FollowDistance;
    [Editor( "AI|攻击技能一,80", "Skill" )]
    public short AttackSkillID;
    [Editor( "AI|技能二,80", "Skill" )]
    public short SecondSkillID;
    [Editor( "AI|技能二机率,80" )]
    public short SecondSkillRate;

    [Editor( "攻击动作,350" )]
    public string AttackResource;

    [Editor( "注释,350" )]
    public string Comment;
}

//AI配置表
public partial class AIReference : PropDefine {
    [Editor( "ID,40" )]
    public short ID;
    [Editor( "AI注释,450" )]
    public string Name;
    [Editor( "版本,40" )]
    public short Version;
    [Editor( "版本类型,60", "EditionType" )]
    public short EditionType;

    [Editor( "所属地图,150", "Map" )]
    public short Map;
    [Editor( "情节表演,60" )]
    public bool Performance;
    [Editor( "AI,80" )]
    public AIEventConfig[] ai_str;
}


// 道具配置表
public partial class ItemReference : PropDefine{
    [Editor("ID,40")]
    public short ID;
    [Editor("物品名字,80")]
    public string Name;
    [Editor("版本,40")]
    public short Version;
    [Editor("版本类型,60", "EditionType")]
    public short EditionType;


    [Editor("职业,70", "RoleOccupation")]
    public short occupation;
    [Editor("使用等级,30")]
    public short level;
    [Editor("使用等级上限,60")]
    public short MaxUselevel;
    [Editor("道具等级,30")]
    public short ItemLevel;
    [Editor("堆叠数量,60")]
    public short Count;

    [Editor("使用|可使用,90")]
    public bool CanUse;
    [Editor("使用|战斗中无法使用,100")]
    public bool CanNotUseInFight;

    ///[Editor("使用|只作用于自己,90")]
    //public bool OnlySelf;
    //[Editor("使用|目标类型,80", "EffectTargetType")]
    //public short TargetType;
    //[Editor("使用|施放时间,60")]
    //public short CastTime;
    //[Editor("使用|距离,40")]
   //public short Distance;
    [Editor("使用|不扣除,60")]
    public bool NotRemove;
    //[Editor("使用|CD(毫秒),60")]
    //ublic int CD;

    //[Editor("使用|必要NPC,60", "Role")]
    //public short NeedNPC;
    [Editor("使用|扩展条件,60")]
    public ItemUseConditionConfig[] UseCondition;

    [Editor("装备变身外观(套装效果),60")]
    public int EquipAppearID;
    [Editor("品质,80", "ItemQuality")]
    public short Quality;
    [Editor("物品IK1,80", "ItemKind1")]
    public short IK1;
    [Editor("物品IK2,80", "ItemKind2")]
    public short IK2;
    [Editor("物品IK3,80", "ItemKind3")]
    public short IK3;
    [Editor("属性配置,80")]
    public int Attribute;
    //[Editor("次名将,60")]
    //public bool HERO_SOUL_EX;

    [Editor("适用装备位置,80")]
    public int FitPart;
    [Editor("价格,60")]
    public int Price;
    [Editor("过期时间,60")]
    public int EndTime;

    //[Editor("装备升星|不升星,60")]
    //public bool NotStar;
    //[Editor("装备升星|耐久初星,60")]
    //public short EndureStar;
    //[Editor("装备升星|攻击初星,60")]
   // public short AttStar;
    //[Editor("装备升星|物防初星,60")]
    //public short PhyDefStar;
    //[Editor("装备升星|法防初星,60")]
    //public short MagDefStar;
    [Editor("装备升星上限,60")]
    public short MaxStar;
    [Editor("装备属性(固定),80")]
    public HoldEffectConfig[] HoldEffect;
    [Editor("装备属性(品质),80")]
    public QualityEffectConfig[] QualityEffect;
    [Editor("装备属性(随机),80")]
    public short RandomEffectBaseValue;
    [Editor("使用效果,80")]
    public FunItemConf[] ItemFunParam;
    [Editor("抽奖物品,80")]
    public DropTableConfig[] DropItem;

    [Editor("图标|图标ID,100")]
    public string Icon;
    [Editor("图标|图标路径,100")]
    public string IconFile;
    [Editor("男性资源路径,150")]
    public string Resource;
    [Editor("女性资源路径,150")]
    public string FemaleResource;
    [Editor("说明,300")]
    public string Comment;
    [Editor("注释,300")]
    public string ItemComment;


    public bool IsAV()// 当前配置项是否是时装
    {
        return this.IK2 == ItemKind2.EQUIP_AV_PROTECTION ||
            this.IK2 == ItemKind2.EQUIP_AV_ARMS;
    }
}



// BUFF配置信息表
public partial class BuffReference : PropDefine {
    [Editor( "ID,40" )]
    public short ID;
    [Editor( "Buff名字,100" )]
    public string Name;
    [Editor( "版本,40" )]
    public short Version;
    [Editor( "版本类型,60", "EditionType" )]
    public short EditionType;

    [Editor( "是否隐藏,60" )]
    public bool Hide;
    [Editor( "能否覆盖,60" )]
    public bool CanOverride;
    [Editor( "增益,60" )]
    public bool IsBuff;
    [Editor( "存档,60" )]
    public bool IsSave;
    [Editor( "能否驱散,60" )]
    public bool CanBeDispel;
    [Editor( "死亡消失,60" )]
    public bool IsDeleteWhenDie;
    [Editor( "对玩家无效,60" )]
    public bool PveBuff;
    [Editor("类型ID,60")]
    public short TypeID;

    //这地方要对一下。
    [Editor("作用时机,80", "BuffProcessType")]
    public short ProcessType;
    [Editor( "一直作用效果,100" )]
    public BuffHoldEffectConfig[] HoldEF;
    [Editor( "触发效果,100" )]
    public BuffDirectEffectConfig[] DirectEF;
    [Editor("特殊效果, 60")]
    public BuffDirectEffectConfig[] SpecialEF;

    [Editor( "被攻击时|消失,60" )]
    public bool RemoveOnAttack;
    [Editor( "被攻击时|机率,60" )]
    public short RemoveRate;

    [Editor( "图标|图标ID,150" )]
    public string Icon;
    [Editor( "图标|图标路径,150" )]
    public string IconFile;
    [Editor( "客户端表现,150" )]
    public string ClientEffect;
    [Editor( "描述信息,500" )]
    public string Comment;
    [Editor( "注释,500" )]
    public string BuffComment;
}


// 技能信息配置表                                          //注："ID,40"  :其中40为本row的长度。
public partial class SkillReference : PropDefine {
    [Editor("通用|ID,40")]
    public short ID;
    [Editor("通用|技能名字,100")]
    public string Name;
    [Editor("通用|版本,40")]
    public short Version;
    [Editor("通用|版本类型,60", "EditionType")]
    public short EditionType;
    [Editor("通用|触发类型,60", "ActionTypes")]
    public short TriggerType = 0;
    [Editor("职业要求, 60", "RoleOccupation")]
    public short RoleNeed;  
    [Editor("目标过滤,60", "TargetFilter")]
    public int Filter;      
    [Editor("必然命中,60")]
    public bool IgnoreFlee;
    [Editor("无视公共CD,60")]
    public bool IgnorePublicCD;
    [Editor( "技能目标类型,100", "CastTargetType" )]
    public short SkillTargetType; 
    [Editor( "技能效果|影响目标类型,100", "EffectTargetType" )]   
    public short TargetType;
    [Editor("技能效果|作用次数, 60")]
    public short ActionTime;
    [Editor("技能效果|飞行轨迹,80")]
    public bool HasDelay;
    [Editor("技能效果|动作延时,80")]
    public int DelayTime;
    [Editor("技能效果|每等级效果,80")]
    public SkillEffectConfig[] level_effect;
    [Editor( "技能描述,350" )]
    public string SkillDesc;
    [Editor( "图标|图标ID,150" )]
    public string Icon;
    [Editor( "图标|图标路径,350" )]
    public string IconFile;
    [Editor( "吟唱资源路径,350" )]
    public string AssetPath1;
    [Editor( "注释,150" )]
    public string Comment;
}



// 天赋配置信息表
public partial class TalentReference : PropDefine {
    [Editor( "ID,40" )]
    public short ID;
    [Editor( "天赋名字,160" )]
    public string Name;
    [Editor( "版本,40" )]
    public short Version;
    [Editor( "版本类型,60", "EditionType" )]
    public short EditionType;

    [Editor( "最大等级,60" )]
    public short MaxLevel;
    [Editor( "天赋效果|效果,160", "TalentEffectType" )]
    public short EF1;
    [Editor( "天赋效果|参数1,60" )]
    public int EF1P1;
    [Editor( "天赋效果|参数2,60" )]
    public int EF1P2;
    [Editor( "天赋效果|参数3,60" )]
    public int EF1P3;

    [Editor( "描述,200" )]
    public string Comment;

    [Editor( "图标|图标ID,100" )]
    public string Icon;
    [Editor( "图标|图标路径,100" )]
    public string IconPath;
}



// 地图上对象配置信息表(比如可采集物品,机关)
public partial class MapObjectReference : PropDefine {
    [Editor( "ID,40" )]
    public short ID;
    [Editor( "名字,160" )]
    public string Name;
    [Editor( "版本,40" )]
    public short Version;
    [Editor( "版本类型,60", "EditionType" )]
    public short EditionType;

    [Editor( "外观ID,60" )]
    public int Apprid;
    [Editor( "刷新时间(秒),60" )]
    public short RefreshSec;

    [Editor( "触发条件,80", "TriggeType" )]
    public short Trigge;
    [Editor( "打开时间(毫秒),60" )]
    public int OpenTime;
    [Editor( "打开次数(0为无限),60" )]
    public short CollectCount;
    [Editor( "打开条件|所需物品,90", "Item" )]
    public short NeedItemRef;
    [Editor( "打开条件|角色等级,60" )]
    public short RoleLevel;
    [Editor( "打开条件|生活技能,60", "ProductSkillType" )]
    public short ProductSkill;
    [Editor( "打开条件|灰色等级,100" )]
    public short GrayLevel;
    [Editor( "打开条件|绿色等级,100" )]
    public short GreenLevel;
    [Editor( "打开条件|黄色等级,100" )]
    public short YellowLevel;

    [Editor( "效果|效果,80", "MapObjectEffectType" )]
    public short EF1;
    [Editor( "效果|扩展掉落,60" )]
    public DropTableConfig[] Data;

    [Editor( "半径,40" )]
    public int radius;
    [Editor( "P1,40" )]
    public int P1;
    [Editor( "P2,40" )]
    public int P2;
    [Editor( "P3,40" )]
    public int P3;
    [Editor( "P4,40" )]
    public int P4;
    [Editor( "P5,40" )]
    public int P5;
    [Editor( "注释,130" )]
    public string Comment;
}



// 配方信息配置表
public partial class ProductReference : PropDefine {
    [Editor( "ID,40" )]
    public short ID;
    [Editor( "名字,100" )]
    public string Name;
    [Editor( "版本,40" )]
    public short Version;
    [Editor( "版本类型,60", "EditionType" )]
    public short EditionType;

    [Editor( "制造时间(毫秒),60" )]
    public short ProductTime;
    [Editor( "生活技能|角色等级,60" )]
    public short RoleLevel;
    [Editor( "生活技能|需要的对象,80", "MapObject" )]
    public short NeedMapObject;
    [Editor( "生活技能|生活技能,60", "ProductSkillType" )]
    public short ProductSkill;
    [Editor( "生活技能|生活技能子类,120", "ProductSkillSubType" )]
    public short ProductSubSkill;
    [Editor( "生活技能|配方细类,120", "ProductThirdType" )]
    public short ThirdType;
    [Editor( "生活技能|灰色等级,100" )]
    public short GrayLevel;
    [Editor( "生活技能|绿色等级,100" )]
    public short GreenLevel;
    [Editor( "生活技能|黄色等级,100" )]
    public short YellowLevel;

    [Editor( "生活技能|增加熟练度,100" )]
    public short AddSkillPoint;

    [Editor( "名将装备,70" )]
    public bool Avatar;
    [Editor( "八字真言,70" )]
    public string AvatarTitle;
    [Editor( "图标路径,70" )]
    public string AvatarIconPath;
    [Editor( "图标,70" )]
    public string AvatarIcon;
    [Editor( "活力消耗,60" )]
    public int CostVigour;
    [Editor( "金钱消耗,60" )]
    public int CostMoney;
    [Editor( "材料消耗列表,300" )]
    public MaterialListConfig[] MaterialList;

    [Editor( "产出物品,130", "Item" )]
    public short ProductItemRef;
    [Editor( "附加产出物品,100" )]
    public DropTableConfig[] Item;
    [Editor( "注释,130" )]
    public string Comment;
}



// 地图信息配置表
public partial class MapReference : PropDefine {
    [Editor( "ID,40" )]
    public int ID;
    [Editor( "名字,100" )]
    public string Name;
    [Editor( "版本,40" )]
    public short Version;
    [Editor( "版本类型,60", "EditionType" )]
    public short EditionType;

    [Editor( "地图类型,100", "MapType" )]
    public short Type;
    [Editor( "副本难度类型,100", "DungeonType" )]
    public short DungeonType;
    [Editor( "默认开启,100" )]
    public bool DungeonOpen;
    [Editor("可寻路,100")]
    public bool PathSearchable;

    [Editor( "地图文件名,100" )]
    public string FileName;
    [Editor( "M地图,100" )]
    public string SceneMapFilename;
    [Editor("雷达地图,100")]
    public string RadarFilename;
    [Editor("安全区域文件,100")]
    public string ZoneFileName;
    [Editor("配置文件名,100")]
    public string ConfigFileName; // 无用
    [Editor( "最小玩家数量,60" )]
    public short MinPlayer;
    [Editor( "最大玩家数量,60" )]
    public short MaxPlayer;
    [Editor( "宽,50" )]
    public int Width;
    [Editor( "高,50" )]
    public int Height;
    [Editor("有效区域,80")]
    public MapValidRegion[] valid_region;

    [Editor( "地图BUFF,60", "Buff" )]
    public int BuffRef;
    [Editor( "BUFF等级,60" )]
    public int BuffLevel;
    
    [Editor( "地图位置配置,100" )]
    public MapPosConfig[] MapPosConf;

    [Editor("副本计分|死亡次数,60")]
    public ScoreDieCountConfig[] DieCount;
    [Editor("副本计分|完成副本时间,100")]
    public ScoreCompleteDungeonTimeConfig[] UseTime;
    [Editor("副本计分|使用道具,60")]
    public ScoreItemConfig[] UseItem;
    [Editor("副本计分|不使用道具,60")]
    public ScoreItemConfig[] NotUseItem;
    [Editor("副本计分|杀死怪物,60")]
    public ScoreKillNPCConfig[] KillNPC;
    [Editor("副本计分|道具收集,60")]
    public ScoreItemConfig[] CollectItem;
    [Editor("副本计分|触发机关,80")]
    public ScoreTriggeConfig[] Trigge;
    [Editor("副本计分|触发情节,80")]
    public ScorePerformanceConfig[] Performance;
    [Editor("副本计分|队伍人数计分,80")]
    public short TeamPlayerCount;
    [Editor("副本计分|人数计分分数,80")]
    public short TeamPlayerCountScore;

    [Editor( "副本|保护时间(秒),100" )]
    public short ProtectSec;
    [Editor( "副本|存在时间(秒),100" )]
    public short LifeTime;
    [Editor( "副本|免费进入次数, 80" )]
    public short FreeEnterCount;
    [Editor( "副本|需要物品,100", "Item" )]
    public short NeedItem;
    [Editor( "副本|物品数量,60" )]
    public short NeedItemCount;
    [Editor( "副本|需要任务,60", "Quest" )]
    public short NeedQuest;
    [Editor( "副本|最低等级,60" )]
    public short NeedLevelMin;
    [Editor( "副本|最高等级,60" )]
    public short NeedLevelMax;
    [Editor( "副本|推荐等级,60" )]
    public short RecommendLevel;

    [Editor( "副本奖励|上上品分数,140" )]
    public int Level1Score;
    [Editor( "副本奖励|上上品奖励,140" )]
    public ScoreAwardItemConfig[] Level1Award;
    [Editor( "副本奖励|上品分数,140" )]
    public int Level2Score;
    [Editor( "副本奖励|上品奖励,140" )]
    public ScoreAwardItemConfig[] Level2Award;
    [Editor( "副本奖励|中品分数,140" )]
    public int Level3Score;
    [Editor( "副本奖励|中品奖励,140" )]
    public ScoreAwardItemConfig[] Level3Award;
    [Editor( "副本奖励|下品奖励,140" )]
    public ScoreAwardItemConfig[] Level4Award;

    [Editor( "副本掉落,140" )]
    public DropTableConfig[] DropItem;

    [Editor( "地图音乐,100" )]
    public string Music;
    [Editor( "地图描述,100" )]
    public string MapDesc;

}


// 任务配置信息表
public partial class QuestReference : PropDefine {

    public static bool IsIdValid(short questId) {
        return questId >= MIN_ID && questId <= MAX_ID;
    }

    public bool IsPublicQuest() {
        return (InGroup != QuestGroupType.NoGroup) ? true : false;
    }

    public bool TryGetGroupSetting(int levelCountryOrGuild, out QuestGroupSetting setting) {
        if (this.GroupSetting != null) {
            for (int i = this.GroupSetting.Length - 1; i >= 0; --i) {
                setting = this.GroupSetting[i];
                if (levelCountryOrGuild >= setting.lvl) {
                    return true;
                }
            }
        }
        setting = new QuestGroupSetting();
        return false;
    }

    public static readonly short MIN_ID = 0;
    public static readonly short MAX_ID = Def.MAX_QUEST - 1;

    [Editor( "ID,40" )]
    public short ID;
    [Editor( "任务名称,100", Export = EditorAttribute.ExportType.ClientOnly )]
    public string Name;                                  // 任务名
    [Editor( "版本,40" )]
    public short Version;
    [Editor( "版本类型,60", "EditionType" )]
    public short EditionType;

    [Editor( "初始任务,40" )]
    public bool AutoAccept;

    [Editor( "分组,100", "QuestGroup" )]
    public short QuestGroup;									// 分组（主线、支线、军团……）
    // 仅用于客户端描述、排序、分组等
    [Editor( "不可放弃,40" )]
    public bool NoCancel;
    [Editor( "任务类型,100", "QuestType" )]
    public short TaskType;                               // 任务类型
    [Editor( "任务描述,100" )]
    public string TaskDesc;                              // 任务描述

    [Editor( "放弃后操作,100", "QuestCleanType" )]
    public short CleanType;

    [Editor( "适合等级,50" )]
    public short PlayerLevel;                            // 适合等级
    [Editor( "难度,50" )]
    public byte Difficult;                               // 任务难度
    [Editor( "互斥,20" )]
    public int Mutex;		// 为0表示与其它任务不互斥，否则与具有相同此值的任务互斥

    [Editor( "杂项设置,50" )]                      // 客户端使用
    public short FindPathType;

    [Editor( "重复次数,72" )]
    public short RepeatCount;                            // 对同一玩家而言，本任务可重复做几次（通常为1）

    [Editor( "VIP重复次数,72" )]
    public VipCountConfig[] VipRepeatCnt;

    [Editor( "活动时间|类型,72", "TaskHolidayType" )]
    public short HolidayType;

    [Editor( "活动时间|起始日期,72" )]
    public string HolidayBegin;
    [Editor( "活动时间|结束日期,72" )]
    public string HolidayEnd;
    [Editor( "活动时间|时间段,72" )]
    public string HolidayPeriod;
    [Editor( "火热,40" )]
    public bool Hot;									// 是否在这个任务前面显示“火热”图标

    [Editor( "面板隐藏,40" )]
    public bool HideInPanel;	// 不在活动面板上显示
    [Editor( "完成条件隐藏,60" )]
    public bool HideCondition;	// 不在活动面板上显示

    [Editor( "活动简述,160" )]
    public string HolidayDesc;

    [Editor( "接任务|NPC,100", "Role" )]
    public int StartNpcTypeId;                        // 接任务NPC的类型ID

    [Editor( "接任务|任务对话,160" )]
    public string StartAskText;                          // 对话时NPC的提问

    [Editor("接任务|行为,80")]
    public QuestAcceptAction[] AcceptActions;

    [Editor( "交任务|自动完成,60" )]
    public bool AutoSubmit;                             // 交任务    
    [Editor( "交任务|NPC,100", "Role" )]
    public int EndNpcTypeId;                             // 交任务NPC的类型ID
    [Editor( "交任务|任务对话,160" )]
    public string EndNpcTalk;                            // 交任务NPC的对话

    [Editor( "发布任务|类型,80", "QuestGroupType" )]
    public short InGroup;
    [Editor( "发布任务|设置,80" )]
    public QuestGroupSetting[] GroupSetting;
    [Editor( "发布任务|可接取者,80" )]
    public int Accessor;


    [Editor( "不能接时不显示,60" )]
    public bool HideWhenCannotNow;				// 为真时表示如果先决条件不满足，则不显示头顶叹号
    [Editor( "先决条件|先决条件,80" )]
    public QuestRequestConditionCfg[] RequestConfig;
    [Editor( "先决条件|最低等级,72" )]
    public short MinPlayerLevel;
    [Editor( "先决条件|最高等级,72" )]
    public short MaxPlayerLevel;
    [Editor( "先决条件|性别,60", "RoleGender" )]
    public short Gender;
    [Editor( "先决条件|职业,60", "RoleOccupation" )]
    public short Occupation;
    [Editor( "先决条件|阵营,60", "CampType" )]
    public short Camp;
    [Editor( "先决条件|前置任务,80", "Quest" )]
    public short BeforeQuest;
    [Editor( "先决条件|金钱,60" )]
    public short Money;

    [Editor( "先决条件|物品,60", "Item" )]
    public short BeforeItemRef;
    [Editor( "先决条件|物品数量,60" )]
    public short BeforeItemCount;

    [Editor( "先决条件|生活技能,60", "ProductSkillType" )]
    public short NeedSkill;

    [Editor( "先决条件|关系,60" )]
    public QuestRelation[] RelationCfg;

    [Editor( "倒计时(秒),60" )]
    public short TimeDown;

    [Editor( "时间限制(分),60" )]
    public short Time;

    [Editor( "死亡时失败,72" )]
    public bool FailIfDie;

    [Editor( "全服消息|领取道具,80" )]
    public string ScreenMsgItemIds;
    [Editor( "全服消息|完成后提示,80" )]
    public bool ScreenMsgCompleted;

    [Editor( "获得道具|一般道具,60" )]
    public ItemConfig[] AcquireItemWhenAcceptEx;
    [Editor( "获得道具|特殊道具,60" )]
    public QuestItemConfigEx[] ItemCfgEx;
    [Editor( "获得道具|保留道具,64" )]
    public bool KeepItemWhenSubmit;

    [Editor( "完成条件|脚本ID,70", "GenericScript" )]
    public int ConditionScriptId;
    [Editor( "完成条件|通用配置,80" )]
    public QuestConditionCfg[] ConditionCfg;
    [Editor( "完成条件|到达区域,80" )]
    public AreaConfig[] ArriveArea;
    [Editor( "完成条件|杀死怪物,80" )]
    public NPCConfig[] KillNPC;
    [Editor( "完成条件|杀多怪(或),80" )]
    public NPCConfig[] KillMultiNPC;
    [Editor( "完成条件|杀多怪(与),80" )]
    public NPCConfig[] KillMultiNPC_Both;
    [Editor( "完成条件|收集物品,80" )]
    public ItemConfig[] CollectItem;
    [Editor("完成条件|收集物品(或),80")]
    public ItemConfig[] CollectAnyOne;
    [Editor( "完成条件|不拥有道具,80", "Item" )]
    public short NoItem;
    [Editor( "完成条件|学习技能,80" )]
    public HasSkillConfig[] HasSkill;
    [Editor( "完成条件|升级到,80" )]
    public short UpgradeToLevel;

    [Editor( "完成条件|使用道具,80" )]
    public ItemConfig[] UseItemEx;
    [Editor( "完成条件|使用道具自定描述,80" )]
    public string UseItemEx_Desc;

    [Editor( "完成条件|触发地图物件,80" )]
    public QuestMapObjectConfig[] TriggerMapObject;

    //[Editor("完成条件|激活怒气星,80")]
    //public short MaxRage;   

#if false
    [Editor("完成条件|引导类,80")]
    public GuideQuestCompleteConditionConfig[] Guide;
#endif

    [Editor( "完成条件|问答,80" )]
    public AskConfig[] Ask;

    [Editor( "任务完成奖励|经验值,70" )]
    public int AwardExp;
    [Editor( "任务完成奖励|金钱,70" )]
    public int AwardMoney;
    [Editor( "任务完成奖励|道具绑定,70" )]
    public bool award_bind;
    [Editor( "任务完成奖励|脚本ID,70", "GenericScript" )]
    public int AwardScriptId;

    [Editor( "固定奖励1|奖励物品,120", "Item" )]
    public int AwardFixedItemTypeId1;
    [Editor( "固定奖励1|奖励数量,72" )]
    public short AwardFixedItemCount1;
    [Editor( "固定奖励2|奖励物品,120", "Item" )]
    public int AwardFixedItemTypeId2;
    [Editor( "固定奖励2|奖励数量,72" )]
    public short AwardFixedItemCount2;

    [Editor( "可选奖励1|奖励物品,120", "Item" )]
    public int AwardOptionItemTypeId1;
    [Editor( "可选奖励1|奖励数量,72" )]
    public short AwardOptionItemCount1;
    [Editor( "可选奖励2|奖励物品,120", "Item" )]
    public int AwardOptionItemTypeId2;
    [Editor( "可选奖励2|奖励数量,72" )]
    public short AwardOptionItemCount2;
    [Editor( "可选奖励3|奖励物品,120", "Item" )]
    public int AwardOptionItemTypeId3;
    [Editor( "可选奖励3|奖励数量,72" )]
    public short AwardOptionItemCount3;
    [Editor( "可选奖励4|奖励物品,120", "Item" )]
    public int AwardOptionItemTypeId4;
    [Editor( "可选奖励4|奖励数量,72" )]
    public short AwardOptionItemCount4;

    [Editor( "条件奖励|条件, 72", "AwardCfgType" )]
    public int AwardConditionType;
    [Editor( "条件奖励|奖励物品, 72" )]
    public AwardCfg[] AwardConditionItem;

    [Editor("附加参数,80", EditorAttribute.ExportType.ClientOnly)]
    public int Param;

}



// 杂项配置表
public partial class MiscReference : PropDefine {
    [Editor( "ID,160" )]
    public short ID;
    [Editor( "名称,100" )]
    public string Name;
    [Editor( "注释,100" )]
    public short Version;
    [Editor( "类型,160", "MiscConfType" )]
    public short Type;
    [Editor( "版本类型,60", "EditionType" )]
    public short EditionType;

    [Editor( "对象数据,100" )]
    public string object_json;
    [Editor( "配置信息,800" )]
    public MiscConfig[] misc_conf;
}

// 装备随机属性配置表
public partial class EffectReference : PropDefine {
    [Editor( "ID,40" )]
    public short ID;
    [Editor( "名称,100" )]
    public string Name;
    [Editor( "版本,40" )]
    public short Version;
    [Editor( "版本类型,60", "EditionType" )]
    public short EditionType;

    [Editor( "效果类型,80", "HoldEffectType" )]
    public short EffectType;

    [Editor( "下限值,60" )]
    public short MinVal;
    [Editor( "上限值,60" )]
    public short MaxVal;
    [Editor("对应品质,60")]
    public short EquipQuality;
    [Editor("对应装备等级,60")]
    public short EquipLevel;
}



// 心法配置表(被动技能)
public partial class PassiveSkillReference : PropDefine {
   // [Editor( "ID,160", "HoldEffectType" )]
    [Editor("ID, 40")]
    public short ID;
    [Editor( "名称,100" )]
    public string Name;
    [Editor( "版本,40" )]
    public short Version;
    [Editor( "版本类型,60", "EditionType" )]
    public short EditionType;

    [Editor( "配置信息,300" )]
    public PassiveSkilllConfig[] Conf;
    [Editor( "图标|图标ID,100" )]
    public string Icon;
    [Editor( "图标|图标路径,100" )]
    public string IconPath;
    [Editor( "心法描述,300" )]
    public string Descriptions;
    [Editor("备注, 200")]
    public string Remark;
}


//计划任务
public partial class ScheduledReference : PropDefine {
    [Editor( "ID,40" )]
    public short ID;
    [Editor( "名称,100" )]
    public string Name;
    [Editor( "版本,40" )]
    public short Version;
    [Editor( "版本类型,60", "EditionType" )]
    public short EditionType;

    [Editor( "关闭,40" )]
    public bool Close;

    [Editor( "开始时间|月,40" )]
    public short BeginMonth;
    [Editor( "开始时间|日,40" )]
    public short BeginDay;
    [Editor( "开始时间|周,40" )]
    public short BeginWeekDay;
    [Editor( "开始时间|时,40" )]
    public short BeginHour;
    [Editor( "开始时间|分,40" )]
    public short BeginMinute;

    [Editor( "关闭时间|月,40" )]
    public short EndMonth;
    [Editor( "关闭时间|日,40" )]
    public short EndDay;
    [Editor( "关闭时间|周,40" )]
    public short EndWeekDay;
    [Editor( "关闭时间|时,40" )]
    public short EndHour;
    [Editor( "关闭时间|分,40" )]
    public short EndMinute;

    [Editor( "是否是活动,80" )]
    public bool IsMovement;
    [Editor( "提前通知,80" )]
    public bool BeginNotify;

    [Editor( "效果|效果,80", "ScheduledEffectType" )]
    public short EF;
    [Editor( "效果|参数1,80" )]
    public int EFP1;
    [Editor( "效果|参数2,80" )]
    public int EFP2;
    [Editor( "效果|参数3,80" )]
    public string EFP3;

    [Editor( "效果|任务列表,100" )]
    public ScheduledConfig[] scheduled_list;
    [Editor( "效果|Role显示配置,100" )]
    public ShowRoleConfig[] ShowRole;
    [Editor( "效果|掉落配置,100" )]
    public DropTableConfig[] DropItem;
}


public partial class MapObjectConfigReference : PropDefine {
    [Editor( "ID,40" )]
    public int ID;
    [Editor( "名称,40" )]
    public string Name;
    [Editor( "版本,40" )]
    public short Version;
    [Editor( "版本类型,60", "EditionType" )]
    public short EditionType;

    [Editor( "地图,100", "Map" )]
    public int map;
    [Editor( "是否是ROLE,40" )]
    public bool isRole;
    [Editor( "SN,40" )]
    public int sn;
    [Editor( "REFID,40" )]
    public int refid;
    [Editor( "是否可见,60" )]
    public bool available;
    [Editor( "位置,80" )]
    public string position;
    [Editor( "朝向,80" )]
    public string direction;

    [Editor( "Role配置|AI,80", "AI" )]
    public short aiRefid;
    [Editor( "Role配置|阵营,80" )]
    public int camp;
    [Editor( "Role配置|巡逻类型,80" )]
    public string patrolType;
    [Editor( "Role配置|路径,40" )]
    public string route;
    [Editor( "Role配置|队伍ID,50" )]
    public int teamID;
}

public partial class GenericScriptReference : PropDefine {
    [Editor( "ID,40" )]
    public int ID;
    [Editor( "名称,50" )]
    public string Name;
    [Editor( "版本,40" )]
    public short Version;
    [Editor( "版本类型,60", "EditionType" )]
    //test
    public short EditionType;
    [Editor("角色, 40", "Role")]
    public short roleid;
    [Editor("技能, 60")] 
    public short skillid;

    [Editor("脚本,180")]
    public string GenericScript;
}

public partial class QuestionReference : PropDefine {
    [Editor( "ID,40" )]
    public int ID;
    [Editor( "名称,50" )]
    public string Name;
    [Editor( "版本,40" )]
    public short Version;
    [Editor( "版本类型,60", "EditionType" )]
    public short EditionType;

    [Editor( "题目,200", Export = EditorAttribute.ExportType.ClientOnly )]
    public string Q1;

    [Editor( "正确项,50", Export = EditorAttribute.ExportType.ServerOnly )]
    public int AIndex;

    [Editor( "答案1,60", Export = EditorAttribute.ExportType.ClientOnly )]
    public string A1;

    [Editor( "答案2,60", Export = EditorAttribute.ExportType.ClientOnly )]
    public string A2;

    [Editor( "答案3,60", Export = EditorAttribute.ExportType.ClientOnly )]
    public string A3;

    [Editor( "答案4,60", Export = EditorAttribute.ExportType.ClientOnly )]
    public string A4;

    [Editor( "答案5,60", Export = EditorAttribute.ExportType.ClientOnly )]
    public string A5;
}

public partial class StoreReference : PropDefine {
    [Editor( "ID,40" )]
    public int ID;
    [Editor( "名称,40" )]
    public string Name;
    [Editor( "版本,40" )]
    public short Version;
    [Editor( "版本类型,60", "EditionType" )]
    public short EditionType;

    [Editor( "道具,100", "Item" )]
    public int Item;
    [Editor( "数量,100" )]
    public int Count;

    [Editor( "推荐, 60" )]
    public bool Hot;
    [Editor( "VIP, 60" )]
    public bool Vip;

    [Editor( "分类,80", "StoreMainType" )]
    public int MainType;
    [Editor( "子类,80", "StoreSubType" )]
    public int SubType;
    [Editor( "价格,60" )]
    public int Price;
    [Editor( "会员价格,60" )]
    public int VipPrice;

    [Editor( "下架,50" )]
    public bool Hide;
    [Editor( "排序,50" )]
    public int Sort;

    [Editor( "限时(yyyy-MM-dd HH:mm:ss)|开始,80" )]
    public string begin_time;
    [Editor( "限时(yyyy-MM-dd HH:mm:ss)|结束,80" )]
    public string end_time;
    [Editor( "限时(yyyy-MM-dd HH:mm:ss)|客户端显示时间,100" )]
    public string show_time;

    [Editor( "限量,50" )]
    public int limited_count;

    [Editor( "介绍,180" )]
    public string Comment;
}

[System.AttributeUsage( System.AttributeTargets.Field )]
public partial class EditorAttribute : System.Attribute {

    public enum ExportType {
        All = 0,        // 缺省的，服务器和客户端都需要的字段
        ServerOnly,
        ClientOnly
    }

    public string Comment;      // 注释
    public string Type;         // 类型
    public ExportType Export;   // 导出

    public EditorAttribute(string comment)
        : this( comment, string.Empty, ExportType.All ) { }

    public EditorAttribute(string comment, ExportType export)
        : this( comment, string.Empty, export ) { }

    public EditorAttribute(string comment, string type)
        : this( comment, type, ExportType.All ) { }

    public EditorAttribute(string comment, string type, ExportType exportType) {
        this.Comment = comment;
        this.Type = type;
        this.Export = exportType;
    }

}
