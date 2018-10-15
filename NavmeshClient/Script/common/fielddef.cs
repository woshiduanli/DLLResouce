using System;
using System.Reflection;
using System.IO;
using System.Xml;

public class StructListToXML {
    public static Type GetStructListType(string name) {
        int pos = name.IndexOf( "[]" );
        if ( pos > 0 ) {
            string type_name = name.Substring( 0, pos );
            return Type.GetType( type_name );
        }
        return null;
    }

    public static object LoadStruct(XmlTextReader xr, Type type, object obj) {
        foreach ( FieldInfo fi in type.GetFields() ) {
            string value = xr.GetAttribute( fi.Name );
            if ( value == null )
                continue;
            switch ( fi.FieldType.Name ) {   //字段有值就跟据类型读取
            case "String": fi.SetValue( obj, value ); break;
            case "Int32": fi.SetValue( obj, int.Parse( value ) ); break;
            case "Int16": fi.SetValue( obj, short.Parse( value ) ); break;
            case "Byte": fi.SetValue( obj, byte.Parse( value ) ); break;
            case "Single": fi.SetValue( obj, float.Parse( value ) ); break;
            case "Boolean": fi.SetValue( obj, int.Parse( value ) == 1 ); break;
            }
        }
        return obj;
    }

    public static Array XMLToArray(string xml, Type type) {
        if ( ( xml == null ) || ( xml == "" ) )
            return null;

        XmlTextReader xr = new XmlTextReader( new StringReader( xml ) );

        int index = 0;
        Array array = null;
        while ( xr.Read() ) {
            if ( xr.NodeType != XmlNodeType.Element )
                continue;
            if ( xr.Name == type.Name ) {
                //读数量
                int count = int.Parse( xr.GetAttribute( "count" ) );
                array = Array.CreateInstance( type, count );
            } else {
                array.SetValue( LoadStruct( xr, type, array.GetValue( index ) ), index );
                index++;
            }
        }
        xr.Close();
        return array;
    }

}


[System.AttributeUsage( System.AttributeTargets.Field )]
public partial class FieldDefAttribute : System.Attribute {
    public string caption;
    public string type;
    public int width;
    public bool enable;
    public bool key;

    public FieldDefAttribute(string caption, int width, string type, bool enable, bool key) {
        this.caption = caption;
        this.type = type;
        this.width = width;
        this.enable = enable;
        this.key = key;
    }

    public FieldDefAttribute(string caption, int width, string type, bool enable) :
        this( caption, width, type, enable, false ) { }
    public FieldDefAttribute(string caption, int width, string type) :
        this( caption, width, type, true, false ) { }
    public FieldDefAttribute(string caption, int width) :
        this( caption, width, "", true, false ) { }
}


[System.AttributeUsage( System.AttributeTargets.Field, AllowMultiple = false )]
public partial class AttributeDefAttribute : System.Attribute {
    public string type;

    public AttributeDefAttribute(string type) {
        this.type = type;
    }
}


[System.AttributeUsage( System.AttributeTargets.Field, AllowMultiple = true )]
public partial class KeyDefAttribute : System.Attribute {
    public string caption;
    public string type;
    public string name;

    public KeyDefAttribute(string name, string caption, string type) {
        this.name = name;
        this.caption = caption;
        this.type = type;
    }
}

public struct QuestMapObjectConfig {
    [FieldDef( "地图物件", 150, "MapObject" )]
    public short mapobj_ref;
    [FieldDef( "数量", 80 )]
    public short count;
    [FieldDef( "时间间隔(秒)", 120 )]
    public short tick;
    [FieldDef( "描述", 120 )]
    public string desc;
}

public struct ItemConfig {
    [FieldDef( "物品", 150, "Item" )]
    public short item_ref;
    [FieldDef( "数量", 80 )]
    public short count;
    [FieldDef("显示名", 150)]
    public string display_name;
}

public struct NPCConfig {
    [FieldDef( "角色", 150, "Role" )]
    public short role_ref;
    [FieldDef( "数量", 80 )] 
    public short count;
    [FieldDef( "显示名", 150 )]
    public string display_name;
}

public struct NPCChatConfig {
    [FieldDef( "对话标题", 80 )]
    public string label;
    [FieldDef( "对话功能", 100, "NPCChatType", true, true )]
    //key
    public short chat_proc_id;
    [FieldDef( "参数1", 80, "int", false )]
    public int p1;
    [FieldDef( "参数2", 80, "int", false )]
    public int p2;
    [FieldDef( "参数3", 80, "int", false )]
    public int p3;
    [FieldDef( "参数4", 80, "int", false )]
    public int p4;
    [FieldDef("字串参数", 100, "string", false)]
    public string str;

    /*
    [KeyDef( "p1", "目标地图", "Map" )]
    [KeyDef( "p2", "X", "short" )]
    [KeyDef( "p3", "Y", "short" )]
    [KeyDef( "p4", "价格", "short" )]
    public static int ReturnPos = 0;

    [KeyDef( "p1", "目标地图", "Map" )]
    [KeyDef( "p2", "X坐标", "short" )]
    [KeyDef( "p3", "Y坐标", "short" )]
    [KeyDef( "p4", "价格", "short" )]
    public static int Teleport = 0;

    [KeyDef( "p1", "目标地图", "Map" )]
    [KeyDef( "p2", "X坐标", "short" )]
    [KeyDef( "p3", "Y坐标", "short" )]
    public static int EscapeJail = 0;

    [KeyDef( "p1", "生活技能", "ProductSkillType" )]
    public static int ProductSkill = 0;

    [KeyDef( "p1", "BUFF", "Buff" )]
    [KeyDef( "p2", "等级", "short" )]
    public static int AddBuff = 0;

    [KeyDef( "p1", "战场地图", "Map" )]
    public const int EnterBattlefield = 0;
    [KeyDef( "p1", "副本", "Map" )]
    public const int CreateSingleDungeon = 0;
    */
}

public struct CreateDungeonConfig {
    [FieldDef( "副本", 150, "Map" )]
    public short mapref;
}

public struct ShowRoleConfig {
    [FieldDef( "SN", 150 )]
    public short role_sn;
    [FieldDef( "地图", 80, "Map" )]
    public short map;
    [FieldDef( "刷新", 80 )]
    public int refresh;

}

public struct MapBossBoxConfig {
    [FieldDef( "宝箱", 150, "MapObject" )]
    public short mapobj_ref;
    [FieldDef( "数量", 80 )]
    public int count;
}

public struct AIEventConfig {
    [FieldDef( "事件", 100, "AIEvent", true, true )]
    public short event_id;
    [FieldDef( "机率", 60 )]
    public short rate;
    [FieldDef( "重复次数", 60 )]
    public short repeat;
    [FieldDef( "条件", 80, "AIConditionType" )]
    public short condition;
    [FieldDef( "参数一", 60 )]
    public int cp1;
    [FieldDef( "参数二", 60 )]
    public int cp2;
    [FieldDef( "动作", 80, "AIActionType" )]
    public short func;
    [FieldDef( "参数一", 60 )]
    public int fp1;
    [FieldDef( "参数二", 60 )]
    public int fp2;
    [FieldDef( "参数三", 60 )]
    public int fp3;
    [FieldDef( "参数四", 60 )]
    public int fp4;
    [FieldDef( "对话", 100 )]
    public string chat;

}

public struct DropTableConfig {
    [FieldDef( "掉落物品", 150, "Item" )]
    public short item_ref;
    [FieldDef( "掉落分表", 80 )]
    public short tblid;
    [FieldDef( "掉落机率", 80 )]
    public short rate;
}

public struct SkillStudyConfig {
    [FieldDef( "可学习技能", 150, "Skill" )]
    public short skill_ref;
}

public struct ItemSaleConfig {
    [FieldDef( "售卖物品", 150, "Item" )]
    public short item_ref;
    [FieldDef( "附加参数", 150 )]
    public int param;
}

public struct BuffDirectEffectConfig {
    [FieldDef("效果", 80, "BuffTriggerType", true, true)]
    public short effect;
    [FieldDef( "持续时间", 60 )]
    public int time;
    [FieldDef( "机率", 40 )]
    public short rate;
    [FieldDef( "触发间隔", 60 )]
    public short tick;

    [FieldDef( "参数1", 50 )]
    public short p1;
    [FieldDef( "参数2", 50 )]
    public short p2;
    [FieldDef( "参数3", 50 )]
    public short p3;

  /*  [KeyDef("p1", "BuffID", "Buff")]
    [KeyDef("p2", "等级", "short")]
    public static int AddBuff = 1;

    [KeyDef( "p1", "Buff1ID", "Buff" )]
    [KeyDef( "p2", "Buff2ID", "Buff" )]
    [KeyDef( "p3", "等级", "short" )]
    public static int AddTwoBuff = DirectEffectType.AddTwoBuff;

    [KeyDef( "p1", "BuffID", "Buff" )]
    [KeyDef( "p2", "等级", "short" )]
    public static int AddBuffToSelf = DirectEffectType.AddBuffToSelf;

    [KeyDef( "p1", "BuffID", "Buff" )]
    [KeyDef( "p2", "等级", "short" )]
    public static int AddBuffToTrigger = DirectEffectType.AddBuffToTrigger;
    */
   // [KeyDef("p1", "倍率", "short")]
  //  public static int damagerebound = DirectEffectType.damagerebound;

  //  [KeyDef("p1", "技能id", "skill")]
  //  public static int useaoe = directeffecttype.useaoe;
}

public struct BuffHoldEffectConfig {
    [FieldDef( "效果", 80, "HoldEffectType", true, true )]
    public short effect;
    [FieldDef( "持续时间", 60 )]
    public int time;
    [FieldDef( "P1", 50 )]
    public int p1;
    [FieldDef( "P2", 50 )]
    public int p2;
    [FieldDef( "P3", 50 )]
    public int p3;
        
}

public struct SkillEffectConfig {
    [FieldDef( "效果类型", 80, "DirectEffectType", true, true)]
    public int effect;
    [FieldDef("等级", 40)]  
    public int skill_level;
    [FieldDef("技术附加伤害", 80)]
    public int append_skill_hurt;
    [FieldDef("冷却时间", 60)]
    public int cd;
    [FieldDef("攻击距离", 60)]
    public int distance;
    [FieldDef("AOE范围", 60)]
    public int around;
    [FieldDef("影响目标上限", 60)]
    public int target_limit;
    [FieldDef("伤害系数", 60)]
    public int hurt_rate;
    [FieldDef("释放时间", 60)]
    public int cast_time;
    [FieldDef("参数一", 60)]
    public int effectp1;
    [FieldDef("参数二", 60)]
    public int effectp2;
    [FieldDef("参数三", 60)]
    public int effectp3;
    [FieldDef("学习等级", 60)]
    public int study_level;
    [FieldDef("花费", 40)]
    public int cost;
    [FieldDef("活力", 40)]
    public int lively;
    [FieldDef("需要道具", 60)]
    public int props;
    [FieldDef("道具数量", 60)]
    public int props_num;

    [KeyDef("effectp1", "风伤害", "int")]
    public static int Wdam = DirectEffectType.WindDam;
    [KeyDef("effectp1", "火伤害", "int")]
    public static int Fdam = DirectEffectType.FireDam;
    [KeyDef("effectp1", "雷伤害", "int")]
    public static int Tdam = DirectEffectType.ThunderDam;

    [KeyDef("effectp1", "BUFFID", "Buff")]
    [KeyDef("effectp2", "BUFF等级", "int")]
    public static int Bdam = DirectEffectType.AddBuff;

    [KeyDef("effectp1", "BUFFID", "Buff")]
    [KeyDef("effectp2", "BUFF等级", "int")]
    public static int SBdam = DirectEffectType.AddBtoSelf;

    [KeyDef("effectp1", "BUFFID", "Buff")]
    [KeyDef("effectp2", "BUFFID", "Buff")]
    [KeyDef("effectp3", "BUFF等级", "int")]
    public static int TBdam = DirectEffectType.AddTwoBuff;
   
}

public struct StudySkillCondition
{
    [FieldDef("test", 120, "ItemConfigExFunType", true, true)]
    public short FunId;
}

public struct QuestItemConfigEx {
    [FieldDef( "功能", 120, "ItemConfigExFunType", true, true )]
    public short FunId;

    [FieldDef( "参数", 80, "string", false )]
    public string param1;
    [FieldDef( "参数", 80, "string", false )]
    public string param2;
    [FieldDef( "参数", 80, "string", false )]
    public string param3;
    [FieldDef( "参数", 80, "string", false )]
    public string param4;
    [FieldDef( "参数", 80, "string", false )]
    public string param5;
    [FieldDef( "参数", 80, "string", false )]
    public string param6;
    [FieldDef( "参数", 80, "string", false )]
    public string param7;


    [KeyDef( "param1", "地图", "Map" )]
    [KeyDef( "param2", "X", "short" )]
    [KeyDef( "param3", "Y", "short" )]
    [KeyDef( "param4", "半径", "short" )]
    [KeyDef( "param5", "道具", "Item" )]
    [KeyDef( "param6", "最小等级（包括）", "short" )]
    [KeyDef( "param7", "最大等级（包括）", "short" )]
    public static int ICF_Treasury = ItemConfigExFunType.ICF_Treasury;

    [KeyDef( "param1", "道具", "Item" )]
    [KeyDef( "param2", "个数", "short" )]
    public static int ICF_RandomItem = ItemConfigExFunType.ICF_RandomItem;
}

public struct QuestRequestConditionCfg {
    [FieldDef( "功能", 120, "QuestRequestConditionType", true, true )]
    public short FunId;

    [FieldDef( "参数", 80, "string", false )]
    public string param1;
    [FieldDef( "参数", 80, "string", false )]
    public string param2;
    [FieldDef( "参数", 80, "string", false )]
    public string param3;
    [FieldDef( "参数", 80, "string", false )]
    public string param4;
    [FieldDef( "参数", 80, "string", false )]
    public string param5;

    //[KeyDef("param1", "大于等于此值", "int")]
    //[KeyDef("param2", "小于等于此值", "int")]
    //public static int QRCC_LoverFami = QuestRequestConditionType.LoverFami;

    [KeyDef( "param1", "等级", "int" )]
    public static int GroupLevel = QuestRequestConditionType.GroupLevel;

    [KeyDef("param1", "前置任务", "Quest")]
    public static int BeforeQuest = QuestRequestConditionType.MultiBeforeQuest;
}

/// <summary>
/// 任务接取时行为配置
/// </summary>
public struct QuestAcceptAction : IComparable<QuestAcceptAction> {

    /// <summary>
    /// 排序比较，让传送功能放在最后面
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(QuestAcceptAction other) {
        if (this.ActionType == other.ActionType) {
            return 0;
        }
        if (this.ActionType == QuestAcceptActionType.Teleport) {
            return 1;
        } else if (other.ActionType == QuestAcceptActionType.Teleport) {
            return -1;
        } else {
            return this.ActionType - other.ActionType;
        }
    }

    [FieldDef("功能", 120, "QuestAcceptActionType", enable = true, key = true)]
    public int ActionType;

    [FieldDef("n1", 80, "int", false)]
    public int n1;
    [FieldDef("n2", 80, "int", false)]
    public int n2;
    [FieldDef("n3", 80, "int", false)]
    public int n3;
    [FieldDef("n4", 80, "int", false)]
    public int n4;
    [FieldDef("s1", 80, "string", false)]
    public string s1;

    [KeyDef("n1", "BUFF", "Buff")]
    [KeyDef("n2", "等级", "int")]
    public static int BuffId = QuestAcceptActionType.AddBuff;

    [KeyDef("n1", "地图", "Map")]
    [KeyDef("n2", "X", "int")]
    [KeyDef("n3", "Y", "int")]
    public static int Teleport = QuestAcceptActionType.Teleport;

}

public struct QuestConditionCfg {
    [FieldDef( "功能", 120, "QuestConditionType", true, true )]
    public short FunId;

    [FieldDef( "参数", 80, "string", false )]
    public string param1;
    [FieldDef( "参数", 80, "string", false )]
    public string param2;
    [FieldDef( "参数", 80, "string", false )]
    public string param3;
    [FieldDef( "参数", 80, "string", false )]
    public string param4;
    [FieldDef( "参数", 80, "string", false )]
    public string param5;

    [KeyDef( "param1", "地图", "Map" )]
    [KeyDef( "param2", "X", "short" )]
    [KeyDef( "param3", "Y", "short" )]
    [KeyDef( "param4", "镖车", "Role" )]
    [KeyDef( "param5", "AI", "AI" )]
    public static int QCT_Carriage = QuestConditionType.QCT_Carriage;

    [KeyDef( "param1", "最小等级", "short" )]
    [KeyDef( "param2", "最大等级", "short" )]
    [KeyDef( "param3", "杀怪个数", "short" )]
    public static int QCT_KillMonsterLvlRange = QuestConditionType.QCT_KillMonsterLvlRange;

    [KeyDef( "param1", "道具", "Item" )]
    [KeyDef( "param2", "个数", "short" )]
    public static int QCT_RandomUseItem = QuestConditionType.QCT_RandomUseItem;

    [KeyDef( "param1", "AI", "AI" )]
    [KeyDef( "param2", "个数", "short" )]
    [KeyDef( "param3", "描述", "string" )]
    [KeyDef("param4", "计时单位", "string")]
    public static int QCT_AI = QuestConditionType.QCT_AI;

    [KeyDef( "param1", "地图", "Map" )]
    [KeyDef( "param2", "X", "short" )]
    [KeyDef( "param3", "Y", "short" )]
    public static int QCT_QE_Teleport = QuestConditionType.QCT_QE_Teleport;

    public static int QCT_QE_TeleportBind = QuestConditionType.QCT_QE_TeleportBind;

    [KeyDef( "param1", "道具", "Item" )]
    [KeyDef( "param2", "个数", "short" )]
    [KeyDef( "param3", "邮件标题", "string" )]
    [KeyDef( "param4", "邮件内容", "string" )]
    public static int QCT_QE_Email = QuestConditionType.QCT_QE_Email;

    [KeyDef( "param1", "道具", "Item" )]
    [KeyDef( "param2", "个数", "short" )]
    [KeyDef( "param3", "邮件标题", "string" )]
    [KeyDef( "param4", "邮件内容", "string" )]
    public static int QCT_QE_SelfEmail = QuestConditionType.QCT_QE_SelfEmail;

    public static int QCT_QE_RelationMaster = QuestConditionType.QCT_QE_RelationMaster;

    [KeyDef( "param1", "动作", "QuestConditionAction" )]
    [KeyDef( "param2", "次数", "short" )]
    public static int QCT_ACTION = QuestConditionType.QCT_ACTION;

    [KeyDef("param1", "网址", "string")]
    public static int QCT_OPEN_URL = QuestConditionType.QCT_OPEN_URL;

    [KeyDef("param1", "地图ID", "int")]
    [KeyDef("param2", "需要胜利", "int")]
    public static int QCT_BattlefieldWin = QuestConditionType.QCT_BattlefieldComplete;
}

public class QuestConditionAction {
    [EditorEnum( "装备开光" )]
    public const int CQA_KG = 1;    // 开光

    [EditorEnum( "装备打孔" )]
    public const int CQA_DK = 2;

    [EditorEnum( "宝石镶嵌" )]
    public const int CQA_XQ = 3;

    [EditorEnum( "装备升星" )]
    public const int CQA_SX = 4;

    [EditorEnum( "仓库存取" )]
    public const int CQA_OpenStore = 5;

    [EditorEnum( "开天逆槽" )]
    public const int CQA_RageSlot = 6;

    [EditorEnum( "激活怒气星" )]
    public const int CQA_RageStar = 7;
}

public struct AreaConfig {
    [FieldDef( "地图", 100, "Map" )]
    public short map_ref;
    [FieldDef( "X", 80 )]
    public short X;
    [FieldDef( "Y", 80 )]
    public short Y;
    [FieldDef( "半径", 80 )]
    public short radius;
}

public struct HasSkillConfig {
    [FieldDef( "力士技能", 100, "Skill" )]
    public short skill0;
    [FieldDef( "武士技能", 100, "Skill" )]
    public short skill1;
    [FieldDef( "法师技能", 100, "Skill" )]
    public short skill2;
    [FieldDef( "术士技能", 100, "Skill" )]
    public short skill3;
    [FieldDef( "技能等级", 100 )]
    public short skilllvl;
}

public struct AskConfig {
    [FieldDef( "功能", 120, "AskFunType", true, true )]
    public short FunId;
    [FieldDef( "问题", 80, "string", false )]
    public string ask;
    [FieldDef( "答案", 80, "int", false )]
    public int correct;
    [FieldDef( "参数", 80, "string", false )]
    public string answer1;
    [FieldDef( "参数", 80, "string", false )]
    public string answer2;
    [FieldDef( "参数", 80, "string", false )]
    public string answer3;
    [FieldDef( "参数", 80, "string", false )]
    public string answer4;
    [FieldDef( "参数", 80, "string", false )]
    public string answer5;

    [KeyDef( "ask", "类型", "string" )]
    [KeyDef( "correct", "答题个数", "string" )]
    [KeyDef( "answer1", "倒计时", "string" )]
    [KeyDef( "answer2", "VIP答题个数", "string" )]
    public static int QSet = AskFunType.QSet;

    [KeyDef( "ask", "问题", "string" )]
    [KeyDef( "correct", "答案", "int" )]
    [KeyDef( "answer1", "回答一", "string" )]
    [KeyDef( "answer2", "回答二", "string" )]
    [KeyDef( "answer3", "回答三", "string" )]
    [KeyDef( "answer4", "回答四", "string" )]
    [KeyDef( "answer5", "回答五", "string" )]
    public static int Normal = AskFunType.Normal;

}

public struct ScoreDieCountConfig {
    [FieldDef( "次数(低于)", 100 )]
    public short count;
    [FieldDef( "得分", 100 )]
    public short score;
}

public struct ScoreCompleteDungeonTimeConfig {
    [FieldDef( "时间(秒)", 100 )]
    public short time;
    [FieldDef( "得分", 100 )]
    public short score;
}

public struct ScoreItemConfig {
    [FieldDef( "道具", 100, "Item" )]
    public short item;
    [FieldDef( "数量", 80 )]
    public short count;
    [FieldDef( "每次得分", 80 )]
    public short score;
    [FieldDef( "得分次数上限", 80 )]
    public short limit;
}

public struct ScoreKillNPCConfig {
    [FieldDef( "目标", 100, "Role" )]
    public short npc;
    [FieldDef( "数量", 80 )]
    public short count;
    [FieldDef( "每次得分", 80 )]
    public short score;
    [FieldDef( "得分次数上限", 80 )]
    public short limit;
}

public struct ScheduledConfig {
    [FieldDef( "计划任务", 100, "Scheduled" )]
    public short scheduled_ref;
}

public struct ScoreDotUseItemConfig {
    [FieldDef( "物品", 100, "Item" )]
    public short item_ref;
    [FieldDef( "得分", 80 )]
    public short score;
}

public struct ScoreTriggeConfig {
    [FieldDef( "目标机关", 100, "MapObject" )]
    public short obj;
    [FieldDef( "得分", 80 )]
    public short score;
}

public struct ScorePerformanceConfig {
    [FieldDef( "情节ID", 100 )]
    public short id;
    [FieldDef( "得分", 80 )]
    public short score;
}

public struct ScoreAwardItemConfig {
    [FieldDef( "物品", 100, "Item" )]
    public short item;
    [FieldDef( "数量", 80 )]
    public short count;
    [FieldDef( "机率", 80 )]
    public short rate;
}

public struct VipCountConfig {
    [FieldDef( "VIP等级", 100 )]
    public short VipLevel;
    [FieldDef( "次数", 80 )]
    public short count;
}

public struct MiscConfig {
    [FieldDef( "对话功能", 100, "MiscConfType", true, true )]
    public short MiscID;
    [FieldDef( "参数", 60, "int", false )]
    public int p1;
    [FieldDef( "参数", 60, "int", false )]
    public int p2;
    [FieldDef( "参数", 60, "int", false )]
    public int p3;
    [FieldDef( "参数", 60, "int", false )]
    public int p4;
    [FieldDef( "参数", 60, "int", false )]
    public int p5;
    [FieldDef( "参数", 60, "int", false )]
    public int p6;
    [FieldDef( "参数", 60, "int", false )]
    public int p7;
    [FieldDef( "参数", 60, "int", false )]
    public int p8;
    [FieldDef( "参数", 60, "int", false )]
    public int p9;
    [FieldDef( "参数", 60, "int", false )]
    public int p10;
    [FieldDef( "参数", 60, "int", false )]
    public int p11;
    [FieldDef( "参数", 120, "string", false )]
    public string p12;
    [FieldDef( "参数", 120, "string", false )]
    public string p13;

    [KeyDef( "p1", "装备等级", "short" )]
    [KeyDef( "p5", "金钱消耗", "short" )]
    [KeyDef( "p6", "活力消耗", "short" )]
    public static int EQUIP_HOLE = MiscConfType.EQUIP_HOLE;

    [KeyDef( "p1", "装备等级", "short" )]
    [KeyDef( "p2", "开光槽品质", "EquipHoleType" )]
    [KeyDef( "p3", "镶嵌道具效果百分比", "short" )]
    [KeyDef( "p4", "概率总和（万分比）", "short" )]
    [KeyDef( "p5", "金钱消耗", "short" )]
    [KeyDef( "p6", "活力消耗", "short" )]
    public static int EQUIP_HOLE_POLISH = MiscConfType.EQUIP_HOLE_POLISH;

    [KeyDef( "p1", "装备等级", "short" )]
    [KeyDef( "p5", "金钱消耗", "short" )]
    [KeyDef( "p6", "活力消耗", "short" )]
    public static int EQUIP_HOLE_RECOVER = MiscConfType.EQUIP_HOLE_RECOVER;

    [KeyDef( "p1", "镶嵌的次数", "int" )]
    [KeyDef( "p3", "成功几率", "short" )]
    [KeyDef( "p5", "金钱消耗", "int" )]
    [KeyDef( "p6", "活力消耗", "int" )]
    public static int EQUIP_ENCHASE = MiscConfType.EQUIP_ENCHASE;

    [KeyDef( "p1", "石头等级", "short" )]
    [KeyDef( "p2", "消耗道具个数", "short" )]
    [KeyDef( "p5", "金钱消耗", "int" )]
    [KeyDef( "p6", "活力消耗", "int" )]
    public static int EQUIP_UNENCHASE = MiscConfType.EQUIP_UNENCHASE;

    [KeyDef( "p1", "目标星级", "int" )]
    [KeyDef( "p2", "升星石头类型", "ItemKind3" )]
    [KeyDef( "p3", "绿色品质总成功几率(万分比)", "short" )]
    [KeyDef( "p4", "蓝色质总成功几率(万分比)", "short" )]
    [KeyDef( "p5", "金钱消耗", "int" )]
    [KeyDef( "p6", "活力消耗", "int" )]
    [KeyDef( "p7", "紫色品质总成功几率(万分比)", "short" )]
    [KeyDef( "p8", "绿色品质失败降级等级", "short" )]
    [KeyDef( "p9", "蓝色色品质失败降级等级", "short" )]
    [KeyDef( "p10", "紫色品质失败降级等级", "short" )]
    [KeyDef( "p11", "黄色品质总成功几率(万分比)", "short" )]
    [KeyDef( "p12", "黄色品质失败降级等级", "short" )]
    public static int EQUIP_STARS_EDURE = MiscConfType.EQUIP_STARS_EDURE;

    [KeyDef( "p1", "目标星级", "int" )]
    [KeyDef( "p2", "升星石头类型", "ItemKind3" )]
    [KeyDef( "p3", "绿色品质总成功几率(万分比)", "short" )]
    [KeyDef( "p4", "蓝色质总成功几率(万分比)", "short" )]
    [KeyDef( "p5", "金钱消耗", "int" )]
    [KeyDef( "p6", "活力消耗", "int" )]
    [KeyDef( "p7", "紫色品质总成功几率(万分比)", "short" )]
    [KeyDef( "p8", "绿色品质失败降级等级", "short" )]
    [KeyDef( "p9", "蓝色色品质失败降级等级", "short" )]
    [KeyDef( "p10", "紫色品质失败降级等级", "short" )]
    [KeyDef( "p11", "黄色品质总成功几率(万分比)", "short" )]
    [KeyDef( "p12", "黄色品质失败降级等级", "short" )]
    public static int EQUIP_STARS_PHS_ATT = MiscConfType.EQUIP_STARS_PHS_ATT;

    [KeyDef( "p1", "目标星级", "int" )]
    [KeyDef( "p2", "升星石头类型", "ItemKind3" )]
    [KeyDef( "p3", "绿色品质总成功几率(万分比)", "short" )]
    [KeyDef( "p4", "蓝色质总成功几率(万分比)", "short" )]
    [KeyDef( "p5", "金钱消耗", "int" )]
    [KeyDef( "p6", "活力消耗", "int" )]
    [KeyDef( "p7", "紫色品质总成功几率(万分比)", "short" )]
    [KeyDef( "p8", "绿色品质失败降级等级", "short" )]
    [KeyDef( "p9", "蓝色色品质失败降级等级", "short" )]
    [KeyDef( "p10", "紫色品质失败降级等级", "short" )]
    [KeyDef( "p11", "黄色品质总成功几率(万分比)", "short" )]
    [KeyDef( "p12", "黄色品质失败降级等级", "short" )]
    public static int EQUIP_STARS_MAG_ATT = MiscConfType.EQUIP_STARS_MAG_ATT;

    [KeyDef( "p1", "目标星级", "int" )]
    [KeyDef( "p2", "升星石头类型", "ItemKind3" )]
    [KeyDef( "p3", "绿色品质总成功几率(万分比)", "short" )]
    [KeyDef( "p4", "蓝色质总成功几率(万分比)", "short" )]
    [KeyDef( "p5", "金钱消耗", "int" )]
    [KeyDef( "p6", "活力消耗", "int" )]
    [KeyDef( "p7", "紫色品质总成功几率(万分比)", "short" )]
    [KeyDef( "p8", "绿色品质失败降级等级", "short" )]
    [KeyDef( "p9", "蓝色色品质失败降级等级", "short" )]
    [KeyDef( "p10", "紫色品质失败降级等级", "short" )]
    [KeyDef( "p11", "黄色品质总成功几率(万分比)", "short" )]
    [KeyDef( "p12", "黄色品质失败降级等级", "short" )]
    public static int EQUIP_STARS_PHS_DEF = MiscConfType.EQUIP_STARS_PHS_DEF;

    [KeyDef( "p1", "目标星级", "int" )]
    [KeyDef( "p2", "升星石头类型", "ItemKind3" )]
    [KeyDef( "p3", "绿色品质总成功几率(万分比)", "short" )]
    [KeyDef( "p4", "蓝色质总成功几率(万分比)", "short" )]
    [KeyDef( "p5", "金钱消耗", "int" )]
    [KeyDef( "p6", "活力消耗", "int" )]
    [KeyDef( "p7", "紫色品质总成功几率(万分比)", "short" )]
    [KeyDef( "p8", "绿色品质失败降级等级", "short" )]
    [KeyDef( "p9", "蓝色色品质失败降级等级", "short" )]
    [KeyDef( "p10", "紫色品质失败降级等级", "short" )]
    [KeyDef( "p11", "黄色品质总成功几率(万分比)", "short" )]
    [KeyDef( "p12", "黄色品质失败降级等级", "short" )]
    public static int EQUIP_STARS_MAG_DEF = MiscConfType.EQUIP_STARS_MAG_DEF;

    [KeyDef( "p1", "星级数", "int" )]
    [KeyDef( "p2", "效果类型", "EquipStarsType" )]
    [KeyDef( "p3", "基础数值", "int" )]
    [KeyDef( "p4", "装备等级幂指数参数", "int" )]
    [KeyDef( "p5", "装备等级幂指数后参数", "int" )]
    [KeyDef( "p6", "星级数效果系数", "int" )]
    public static int EQUIP_STARS_EFFECT = MiscConfType.EQUIP_STARS_EFFECT;

    [KeyDef( "p1", "装备品质", "ItemQuality" )]
    [KeyDef( "p2", "吟唱时间（毫秒）", "short" )]
    [KeyDef( "p3", "活力消耗", "short" )]
    [KeyDef( "p4", "提炼成功几率", "short" )]
    [KeyDef( "p5", "产出物品", "Item" )]
    [KeyDef( "p6", "产出物品数量", "short" )]
    [KeyDef( "p7", "额外产出几率", "short" )]
    [KeyDef( "p8", "额外产出数量", "short" )]
    public static int EQUIP_ABSTRACT = MiscConfType.EQUIP_ABSTRACT;

    [KeyDef( "p1", "格子数", "short" )]
    [KeyDef( "p2", "活力消耗", "short" )]
    [KeyDef( "p3", "金钱消耗", "short" )]
    [KeyDef( "p4", "玩家等级要求", "short" )]
    public static int STALL_COMSUMPTION = MiscConfType.STALL_COMSUMPTION;

    [KeyDef( "p1", "师德值", "int" )]
    [KeyDef( "p2", "徒弟数上限", "short" )]
    [KeyDef( "p3", "每月可用", "short" )]
    [KeyDef( "p4", "每日获取上限", "short" )]
    [KeyDef( "p12", "称号", "string" )]
    public static int RELA_MASTER_TITLE = MiscConfType.RELA_MASTER_TITLE;

    [KeyDef( "p1", "亲密度", "int" )]
    [KeyDef( "p2", "每日获取上限", "int" )]
    [KeyDef( "p3", "Buff1", "Buff" )]
    [KeyDef( "p4", "Buff1等级", "int" )]
    [KeyDef( "p5", "Buff2", "Buff" )]
    [KeyDef( "p6", "Buff2等级", "int" )]
    [KeyDef( "p12", "称号", "string" )]
    public static int RELA_BROTHER_TITLE = MiscConfType.RELA_BROTHER_TITLE;

    [KeyDef( "p1", "亲密度", "int" )]
    [KeyDef( "p2", "每日获取上限", "int" )]
    [KeyDef( "p3", "技能1", "Skill" )]
    [KeyDef( "p4", "技能1等级", "int" )]
    [KeyDef( "p5", "技能2", "Skill" )]
    [KeyDef( "p6", "技能2等级", "int" )]
    [KeyDef( "p7", "技能3", "Skill" )]
    [KeyDef( "p8", "技能3等级", "int" )]
    [KeyDef( "p12", "称号", "string" )]
    public static int RELA_LOVER_TITLE = MiscConfType.RELA_LOVER_TITLE;

    [KeyDef( "p1", "亲密度", "int" )]
    [KeyDef( "p2", "每日获取上限", "int" )]
    [KeyDef( "p12", "称号", "string" )]
    public static int RELA_SPOUSES_TITLE = MiscConfType.RELA_SPOUSES_TITLE;

    [KeyDef( "p1", "结拜消耗道具", "Item" )]
    public static int RELA_BROTHER_CONSUME = MiscConfType.RELA_BROTHER_CONSUME;

    [KeyDef( "p1", "情侣消耗道具", "Item" )]
    public static int RELA_LOVER_CONSUME = MiscConfType.RELA_LOVER_CONSUME;

    [KeyDef( "p1", "徒弟等级", "short" )]
    [KeyDef( "p2", "邮寄给师傅的道具", "Item" )]
    [KeyDef( "p3", "道具数量", "short" )]
    public static int RELA_MASTER_AWARDS = MiscConfType.RELA_MASTER_AWARDS;

    [KeyDef( "p1", "道具ID", "Item" )]
    public static int BAG_COLLATION_FIXITEM = MiscConfType.BAG_COLLATION_FIXITEM;

    [KeyDef( "p1", "装备品质", "ItemQuality" )]
    public static int BAG_COLLATION_EQUIP = MiscConfType.BAG_COLLATION_EQUIP;

    [KeyDef( "p1", "道具2级分类", "ItemKind2" )]
    public static int BAG_COLLATION_ITEM = MiscConfType.BAG_COLLATION_ITEM;

    [KeyDef( "p1", "材料品质", "ItemQuality" )]
    public static int BAG_COLLATION_MATERIAL = MiscConfType.BAG_COLLATION_MATERIAL;

    [KeyDef( "p1", "等级", "short" )]
    [KeyDef( "p2", "消耗活力", "int" )]
    [KeyDef( "p3", "消耗金钱", "int" )]
    [KeyDef( "p4", "累计效果(万分比)", "short" )]
    public static int FASTNESS_EFFECT = MiscConfType.FASTNESS_EFFECT;

    [KeyDef( "p1", "等级", "short" )]
    [KeyDef( "p2", "保留", "int" )]
    [KeyDef( "p3", "消耗活力", "int" )]
    [KeyDef( "p4", "消耗金钱", "int" )]
    public static int PASSIVESKILL_COST = MiscConfType.PASSIVESKILL_COST;

    [KeyDef( "p1", "第N个心法", "short" )]
    [KeyDef( "p2", "保留", "int" )]
    [KeyDef( "p3", "活力消耗倍率(百分比)", "int" )]
    public static int ADD_PASSIVESKILL_COST = MiscConfType.ADD_PASSIVESKILL_COST;

    [KeyDef( "p1", "月", "short" )]
    [KeyDef( "p2", "日", "short" )]
    [KeyDef( "p12", "ID", "string" )]
    public static int HOLIDAY_ICON = MiscConfType.HOLIDAY_ICON;

    [KeyDef( "p1", "道具", "Store" )]
    [KeyDef( "p2", "道具", "Store" )]
    [KeyDef( "p3", "道具", "Store" )]
    [KeyDef( "p4", "道具", "Store" )]
    [KeyDef( "p5", "道具", "Store" )]
    [KeyDef( "p6", "道具", "Store" )]
    [KeyDef( "p7", "道具", "Store" )]
    [KeyDef( "p8", "道具", "Store" )]
    [KeyDef( "p9", "道具", "Store" )]
    [KeyDef( "p10", "道具", "Store" )]
    [KeyDef( "p11", "道具", "Store" )]
    [KeyDef( "p12", "商城公告", "string" )]
    public static int STORE_HOT_ITEM = MiscConfType.STORE_HOT_ITEM;

    [KeyDef( "p1", "等级", "int" )]
    [KeyDef( "p2", "活跃度", "int" )]
    [KeyDef( "p3", "金钱", "int" )]
    [KeyDef( "p4", "人数", "int" )]
    public static int GROUP_LEVEL = MiscConfType.GROUP_LEVEL;

    [KeyDef( "p1", "等级差", "int" )]
    [KeyDef( "p2", "一天", "int" )]
    [KeyDef( "p3", "二天", "int" )]
    [KeyDef( "p4", "三天", "int" )]
    [KeyDef( "p5", "四天", "int" )]
    [KeyDef( "p6", "五天", "int" )]
    [KeyDef( "p7", "六天", "int" )]
    [KeyDef( "p8", "七天", "int" )]
    public static int GROUP_WAR = MiscConfType.GROUP_WAR;

    [KeyDef( "p1", "道具", "Item" )]
    [KeyDef( "p2", "师德值", "int" )]
    public static int MASTER_ITEM = MiscConfType.MASTER_ITEM;

    [KeyDef( "p1", "装备起始等级", "short" )]
    [KeyDef( "p2", "装备终止等级", "short" )]
    [KeyDef( "p3", "升星价格比例（万分比）", "short" )]
    public static int EQUIP_STARS_RATIO = MiscConfType.EQUIP_STARS_RATIO;

    [KeyDef( "p1", "VIP等级", "short" )]
    [KeyDef( "p2", "最大成长度", "short" )]
    [KeyDef( "p3", "每日成长度", "short" )]
    [KeyDef( "p4", "每日成长度（年会员）", "short" )]
    [KeyDef( "p5", "过期成长度衰减", "short" )]
    [KeyDef( "p6", "升级赠送道具", "Item" )]
    [KeyDef( "p12", "系统提示", "string" )]
    [KeyDef( "p13", "邮件内容", "string" )]
    public static int VIP = MiscConfType.VIP;

    [KeyDef("p1", "事件", "VideoPlayEvent")]
    [KeyDef("p2", "相关ID", "int")]
    [KeyDef("p12", "视频名", "string")]
    public static int VIDEO_PLAY = MiscConfType.VIDEO_PLAY;
}

enum VideoPlayEvent {
    [EditorEnum("接任务")]
    QuestAccept = 1,

    [EditorEnum("交任务")]
    QuestSubmit,

    [EditorEnum("任务完成")]
    QuestComplete,

    [EditorEnum("首次进入地图")]
    EnterMapFirst,

    [EditorEnum("每次进入地图")]
    EnterMapEach,

    [EditorEnum("副本大厅")]
    DungeonHall,

    [EditorEnum("副本胜利")]
    DungeonWin,

    [EditorEnum("道具使用")]
    ItemUse
}

// 任务可接取者
public class QuestGroupAccess {
    [EditorEnum( "所有人" )]
    public const int All = 0x0000;

    [EditorEnum( "军团长" )]
    public const int GroupLeader = 0x0002;
    [EditorEnum( "副军团长" )]
    public const int GroupViceLeader = 0x0004;
}

// 军团任务设置
public struct QuestGroupSetting {
    [FieldDef( "军团/国家 等级", 120 )]
    public int lvl;

    [FieldDef( "每日接取次数", 100 )]
    public int count;

    [FieldDef( "发布时长(分钟)", 100 )]
    public int continued_time;

    [FieldDef( "军团/国家金钱", 120 )]
    public int money;

    [FieldDef( "军团/国家活跃", 120 )]
    public int vigour;

    [FieldDef( "每日发布次数", 100 )]
    public int public_count;
}

// 军团任务发布
public struct QuestGroupPutOut {
    [FieldDef( "军团等级", 80 )]
    public int lvl;

    [FieldDef( "发布时长(分钟)", 100 )]
    public int ContinuedTime;

    [FieldDef( "消耗军团金钱", 100 )]
    public int money;

    [FieldDef( "消耗军团活跃", 100 )]
    public int point;
}

public struct QuestRelation {

    [FieldDef( "关系类型", 100, "RelationshipType", true, true )]
    public int relationType;

    [FieldDef( "检查队伍", 60 )]
    public bool bCheckTeam;

    [FieldDef( "检查距离", 60 )]
    public bool bCheckDistance;
}

public struct AwardCfg {
    [FieldDef( "奖励功能ID", 100, "AwardFunType", true, true )]
    public short FuncID;
    [FieldDef( "条件最小值", 100 )]
    public int Min;
    [FieldDef( "条件最大值", 100 )]
    public int Max;
    [FieldDef( "参数1", 100 )]
    public int p1;
    [FieldDef( "参数2", 100 )]
    public int p2;


    [KeyDef( "p1", "道具", "Item" )]
    [KeyDef( "p2", "数量", "int" )]
    public static int AddItem = AwardFunType.AddItem;

    [KeyDef( "p1", "任务", "Quest" )]
    public static int AutoTask = AwardFunType.AutoTask;

    [KeyDef( "p1", "道具(随机)", "Item" )]
    [KeyDef( "p2", "数量", "int" )]
    public static int AddItemOr = AwardFunType.AddItemOr;

    [KeyDef( "p1", "随机个数", "int" )]
    public static int AddItemOrSetting = AwardFunType.AddItemOrSetting;
}

public struct FunItemConf {
    [FieldDef( "道具功能", 100, "ItemFunType", true, true )]
    public short FuncID;
    [FieldDef( "参数1", 100 )]
    public int p1;
    [FieldDef( "参数2", 100 )]
    public int p2;
    [FieldDef( "参数3", 100 )]
    public int p3;
    [FieldDef( "参数4", 100 )]
    public int p4;


    [KeyDef( "p1", "无意义参数", "int" )]
    public static int None = ItemFunType.None;


    [KeyDef( "p1", "怪物ID", "Role" )]
    [KeyDef( "p2", "怪物AI", "AI" )]
    [KeyDef( "p3", "怪物阵营", "CampType" )]
    [KeyDef( "p4", "存在时间", "int" )]
    public static int SummonMonster = ItemFunType.SummonMonster;


    [KeyDef( "p1", "道具ID", "Item" )]
    [KeyDef( "p2", "道具数量", "short" )]
    public static int OpenBag = ItemFunType.OpenBag;


    [KeyDef( "p1", "技能ID", "Skill" )]
    public static int SkillBook = ItemFunType.SkillBook;

    [KeyDef( "p1", "经验", "int" )]
    public static int CountryExp = ItemFunType.CountryExp;


    [KeyDef( "p1", "活力点数", "short" )]
    public static int NAME = ItemFunType.VigourItem;


    [KeyDef( "p1", "参数(等级，修正概率)", "short" )]
    public static int EquipItem = ItemFunType.EquipItem;


    [KeyDef( "p1", "活力点数", "short" )]
    public static int MeritItem = ItemFunType.MeritItem;

    [KeyDef( "p1", "特效ID", "int" )]
    [KeyDef( "p2", "参数1", "int" )]
    [KeyDef( "p3", "参数2", "int" )]
    public static int SpecialEffects = ItemFunType.SpecialEffects;

    [KeyDef( "p1", "关系类型(8-结拜 16-情侣 32-夫妻)", "short" )]
    [KeyDef( "p2", "亲密度值", "short" )]
    public static int FamilarityItem = ItemFunType.FamilarityItem;


    [KeyDef( "p1", "增加背包格子数", "short" )]
    public static int BagExtendItem = ItemFunType.BagExtendItem;


    [KeyDef( "p1", "增加仓库格子数", "short" )]
    public static int StoreExtendItem = ItemFunType.StoreExtendItem;


    [KeyDef( "p1", "HP", "short" )]
    public static int HP = ItemFunType.HP;


    [KeyDef( "p1", "MP", "short" )]
    public static int MP = ItemFunType.MP;


    [KeyDef( "p1", "BuffID", "Buff" )]
    [KeyDef( "p2", "等级", "short" )]
    public static int AddBuff = ItemFunType.AddBuff;

    [KeyDef( "p1", "BuffID", "Buff" )]
    [KeyDef( "p2", "等级", "short" )]
    public static int AddAppridBuff = ItemFunType.AddAppridBuff;

    [KeyDef( "p1", "目标地图", "Map" )]
    [KeyDef( "p2", "X", "short" )]
    [KeyDef( "p3", "Y", "short" )]
    public static int Teleport = ItemFunType.Teleport;


    [KeyDef( "p1", "目标地图", "Map" )]
    [KeyDef( "p2", "X", "short" )]
    [KeyDef( "p3", "Y", "short" )]
    public static int ReturnPos = ItemFunType.ReturnPos;


    [KeyDef( "p1", "怒气星", "short" )]
    public static int AddMaxRage = ItemFunType.AddMaxRage;


    [KeyDef( "p1", "怒气", "short" )]
    public static int AddRage = ItemFunType.AddRage;



    [KeyDef( "p1", "是否清怒气", "short" )]
    [KeyDef( "p2", "扣除耐久修正", "short" )]
    [KeyDef( "p3", "回HP/MP百分比", "short" )]
    public static int ReliveItem = ItemFunType.ReliveItem;


    [KeyDef( "p1", "附带技能", "Skill" )]
    public static int AttachedSkill = ItemFunType.AttachedSkill;


    [KeyDef( "p1", "天赋点增加技能", "Skill" )]
    public static int TalentBook = ItemFunType.TalentBook;


    [KeyDef( "p1", "情节ID", "short" )]
    public static int Performance = ItemFunType.Performance;


    [KeyDef( "p1", "状态ID", "short" )]
    [KeyDef( "p2", "值", "short" )]
    public static int MapState = ItemFunType.MapState;


    [KeyDef( "p1", "减少恶名值", "short" )]
    public static int DecKillPoint = ItemFunType.DecKillPoint;

    [KeyDef( "p1", "活力值", "short" )]
    public static int GroupVigour = ItemFunType.GroupVigour;

    [KeyDef( "p1", "任务", "Quest" )]
    public static int OneQuest = ItemFunType.OneQuest;

    [KeyDef( "p1", "调用脚本", "GenericScript" )]
    [KeyDef( "p2", "全服消息道具ID", "short" )]
    [KeyDef( "p3", "全服消息道具ID", "short" )]
    [KeyDef( "p4", "全服消息道具ID", "short" )]
    public static int OneScript = ItemFunType.OneScript;

    [KeyDef( "p1", "会员时长(小时)", "short" )]
    [KeyDef( "p2", "成长值", "short" )]
    public static int Vip = ItemFunType.Vip;

    [KeyDef( "p1", "金额", "short" )]
    public static int GroupMoney = ItemFunType.GroupMoney;

    [KeyDef( "p1", "元宝", "short" )]
    public static int SpecialItem = ItemFunType.SpecialItem;

    [KeyDef( "p1", "生产配方", "Product" )]
    public static int Manufacture = ItemFunType.Manufacture;

    [KeyDef( "p1", "战勋", "int" )]
    public static int AddBattlefieldValue = ItemFunType.AddBattlefieldValue;

    //[KeyDef( "p1", "生产技能", "ProductSkillType" )]
    //[KeyDef( "p2", "点数", "int" )]
    //public static int AddProductSkillPoint = ItemFunType.AddProductSkillPoint;

    [KeyDef( "p1", "绑定元宝", "short" )]
    public static int SpecialItemBind = ItemFunType.SpecialItemBind;

    [KeyDef( "p1", "道具", "Item" )]
    [KeyDef( "p2", "数量", "int" )]
    public static int RemoveItem = ItemFunType.RemoveItem;
}

public struct MapValidRegion {
    [FieldDef( "X", 100 )]
    public int x;
    [FieldDef( "Y", 100 )]
    public int y;
    [FieldDef( "宽", 100 )]
    public int width;
    [FieldDef( "高", 100 )]
    public int height;
}

public struct MapPosConfig {
    [FieldDef( "类型", 100, "MapPosType" )]
    public int type;
    [FieldDef( "X", 100 )]
    public int x;
    [FieldDef( "Y", 100 )]
    public int y;
    [FieldDef( "X朝向", 100 )]
    public int rx;
    [FieldDef( "Y朝向", 100 )]
    public int ry;
}

public struct HoldEffectConfig {
    [FieldDef( "效果", 100, "HoldEffectType", true, true )]
    public short funcid;
    [FieldDef( "参数一", 100 )]
    public int p1;
    //[FieldDef( "颜色", 100, "ItemQuality" )]
    //public short p2;
}

public struct QualityEffectConfig
{
    [FieldDef("效果", 100, "HoldEffectType", true, true)]
    public short funcid;
    [FieldDef("参数一", 100)]
    public int p1;
    [FieldDef("颜色", 100, "ItemQuality")]
    public short p2;
}

//public struct RandomEffectConfig {
//    [FieldDef( "基准值", 60 )]
//    public short BaseValue;
//}

public struct PassiveSkilllConfig {
    [FieldDef("属性类型", 60, "HoldEffectType", true, true)]
    public short attriType;
    [FieldDef( "心法等级", 60 )]
    public short plevel;
    [FieldDef( "增加数值", 60 )]
    public short addValue;
    [FieldDef( "学习等级", 60 )]
    public short level;


}

public struct MaterialListConfig {
    [FieldDef( "物品ID", 60, "Item" )]
    public short p1;
    [FieldDef( "数量", 80 )]
    public short p2;
}


public struct ItemUseConditionConfig {
    [FieldDef( "条件", 100, "ItemUseCondition", true, true )]
    public short cond;
    [FieldDef( "参数1", 100 )]
    public int p1;
    [FieldDef( "参数2", 100 )]
    public int p2;
    [FieldDef( "参数3", 100 )]
    public int p3;
    [FieldDef( "参数4", 100 )]
    public int p4;



    [KeyDef( "p1", "地图", "Map" )]
    [KeyDef( "p2", "X", "short" )]
    [KeyDef( "p3", "Y", "short" )]
    [KeyDef( "p4", "半径", "short" )]
    public static int AREA = ItemUseCondition.AREA;

    [KeyDef( "p1", "性别", "RoleGender" )]
    public static int GENDER = ItemUseCondition.GENDER;

    [KeyDef( "p1", "需要空格数量", "short" )]
    public static int BAG_HAS_SPACE = ItemUseCondition.BAG_HAS_SPACE;
    [KeyDef( "p1", "地图类型", "MapType" )]
    public static int MAP_TYPE = ItemUseCondition.MAP_TYPE;
    [KeyDef( "p1", "经验", "int" )]
    public static int NEED_EXP = ItemUseCondition.NEED_EXP;
    [KeyDef( "p1", "道具", "Item" )]
    [KeyDef( "p2", "数量", "int" )]
    public static int NEED_ITEM = ItemUseCondition.NEED_ITEM;
}

#if false
public struct GuideQuestCompleteConditionConfig {
    [FieldDef( "条件", 100, "GuideQuestCompleteCondition", true, true )]
    public int condition;
    [FieldDef( "参数1", 100)]
    public int p1;
    [FieldDef( "参数2", 100)]
    public int p2;


    [KeyDef( "p1", "升星操作次数", "int" )]
    public static int EQUIP_UPGRADE = GuideQuestCompleteCondition.EQUIP_UPGRADE;

    [KeyDef( "p1", "镶嵌操作次数", "int" )]
    public static int EQUIP_INLAY = GuideQuestCompleteCondition.EQUIP_INLAY;

    [KeyDef( "p1", "打孔操作次数", "int" )]
    public static int EQUIP_SLOTTING = GuideQuestCompleteCondition.EQUIP_SLOTTING;

    [KeyDef( "p1", "开光操作次数", "int" )]
    public static int EQUIP_BRIGHTEN = GuideQuestCompleteCondition.EQUIP_BRIGHTEN;
}
#endif