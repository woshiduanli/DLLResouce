using System; 

public partial class RoleReference : PropDefine {
    public override void LoadReference( ResourceUtilReader resource_reader ) {
        short.TryParse( resource_reader.GetValueByCol( "ID" ), out this.ID );
        this.Name = resource_reader.GetValueByCol( "Name" );
        //short.TryParse( resource_reader.GetValueByCol( "EditionType" ), out this.EditionType );
        //this.Title = resource_reader.GetValueByCol( "Title" );
        int.TryParse( resource_reader.GetValueByCol( "ResId" ), out this.Apprid );
        //short.TryParse( resource_reader.GetValueByCol( "type" ), out this.type );
        //short.TryParse( resource_reader.GetValueByCol( "gender" ), out this.gender );
        //short.TryParse( resource_reader.GetValueByCol( "occupation" ), out this.occupation );
        //short.TryParse( resource_reader.GetValueByCol( "level" ), out this.level );
        //int.TryParse( resource_reader.GetValueByCol( "nextlevelexp" ), out this.nextlevelexp );

        //int.TryParse(resource_reader.GetValueByCol("deathexp"), out this.deathexp);
        //int.TryParse(resource_reader.GetValueByCol("reliveinterval"), out this.reliveinterval);
        //int.TryParse(resource_reader.GetValueByCol("BodyRadius"), out this.BodyRadius);

        //int.TryParse(resource_reader.GetValueByCol("BaseMaxHp"), out this.BaseMaxHp);
        //short.TryParse(resource_reader.GetValueByCol("BaseMaxRet"), out this.BaseHpRet);
        //short.TryParse(resource_reader.GetValueByCol("BaseNormalAtk"), out this.BaseNormalAtk);
        //short.TryParse(resource_reader.GetValueByCol("BaseNormalDef"), out this.BaseNormalDef);
        //short.TryParse(resource_reader.GetValueByCol("BaseWalkSpeed"), out this.BaseWalkSpeed);
        //short.TryParse(resource_reader.GetValueByCol("BaseAtkSpeed"), out this.BaseAtkSpeed);
        //short.TryParse(resource_reader.GetValueByCol("BaseHitValue"), out this.BaseHitValue);
        //short.TryParse(resource_reader.GetValueByCol("BaseDodgeValue"), out this.BaseDodgeValue);
        //short.TryParse(resource_reader.GetValueByCol("BaseCritValue"), out this.BaseCritValue);
        //short.TryParse(resource_reader.GetValueByCol("BaseCritAvoidValue"), out this.BaseCritAvoidValue);
        //short.TryParse(resource_reader.GetValueByCol("BaseCritMulti"), out this.BaseCritMulti);
        //short.TryParse(resource_reader.GetValueByCol("BaseWindAtk"), out this.BaseWindAtk);
        //short.TryParse(resource_reader.GetValueByCol("BaseFireAtk"), out this.BaseFireAtk);
        //short.TryParse(resource_reader.GetValueByCol("BaseThunderAtk"), out this.BaseThunderAtk);
        //short.TryParse(resource_reader.GetValueByCol("BaseWindDef"), out this.BaseWindDef);
        //short.TryParse(resource_reader.GetValueByCol("BaseFireDef"), out this.BaseFireDef);
        //short.TryParse(resource_reader.GetValueByCol("BaseThunderDef"), out this.BaseThunderDef);
        
        //short.TryParse( resource_reader.GetValueByCol( "monster_type" ), out this.monster_type );
        //short.TryParse( resource_reader.GetValueByCol( "monster_level" ), out this.monster_level );
        //this.chat_text = resource_reader.GetValueByCol("chat_text");
        //try {
        //   Type struct_type = StructListToXML.GetStructListType( "NPCChatConfig[]" );
        //   string obj_value = resource_reader.GetValueByCol( "npc_chat" );
        //   Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
        //   this.npc_chat = (NPCChatConfig[])array;
        //} catch ( Exception e ) {
        //   throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        //}
        //try {
        //   Type struct_type = StructListToXML.GetStructListType( "DropTableConfig[]" );
        //   string obj_value = resource_reader.GetValueByCol( "drop_item" );
        //   Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
        //   this.drop_item = (DropTableConfig[])array;
        //} catch ( Exception e ) {
        //   throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        //}
        //try {
        //   Type struct_type = StructListToXML.GetStructListType( "SkillStudyConfig[]" );
        //   string obj_value = resource_reader.GetValueByCol( "skill_study" );
        //   Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
        //   this.skill_study = (SkillStudyConfig[])array;
        //} catch ( Exception e ) {
        //   throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        //}
        //short.TryParse( resource_reader.GetValueByCol( "currency_type" ), out this.currency_type );
        //try {
        //   Type struct_type = StructListToXML.GetStructListType( "ItemSaleConfig[]" );
        //   string obj_value = resource_reader.GetValueByCol( "item_sale" );
        //   Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
        //   this.item_sale = (ItemSaleConfig[])array;
        //} catch ( Exception e ) {
        //   throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        //}
        //int.TryParse( resource_reader.GetValueByCol( "item_sale_rate" ), out this.item_sale_rate );
        //int.TryParse( resource_reader.GetValueByCol( "item_buy_rate" ), out this.item_buy_rate );
        //try {
        //   Type struct_type = StructListToXML.GetStructListType( "CreateDungeonConfig[]" );
        //   string obj_value = resource_reader.GetValueByCol( "create_dungeon" );
        //   Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
        //   this.create_dungeon = (CreateDungeonConfig[])array;
        //} catch ( Exception e ) {
        //   throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        //}
        //short.TryParse( resource_reader.GetValueByCol( "AIType" ), out this.AIType );
        //this.Escape = resource_reader.GetValueByCol( "Escape" ) == "1";
        //this.AutoAttack = resource_reader.GetValueByCol( "AutoAttack" ) == "1";
        //int.TryParse( resource_reader.GetValueByCol( "AutoAttackDistance" ), out this.AutoAttackDistance );
        //int.TryParse( resource_reader.GetValueByCol( "FollowDistance" ), out this.FollowDistance );
        //short.TryParse( resource_reader.GetValueByCol( "AttackSkillID" ), out this.AttackSkillID );
        //short.TryParse( resource_reader.GetValueByCol( "SecondSkillID" ), out this.SecondSkillID );
        //short.TryParse( resource_reader.GetValueByCol( "SecondSkillRate" ), out this.SecondSkillRate );
        //this.AttackResource = resource_reader.GetValueByCol( "AttackResource" );
        //this.Comment = resource_reader.GetValueByCol( "Comment" );

        //this.BaseCritMulti = 150;   //< 默认基准暴击倍率为1.5倍
        //this.BaseWalkSpeed = 300;   //< 基准移动速度为300
        //this.BaseAtkSpeed = 1;      //< 基准攻击速度为1per/s
        //this.AttackSkillID = 1;
    }
} 

public partial class BuffReference : PropDefine {
    public override void LoadReference( ResourceUtilReader resource_reader ) {
        short.TryParse( resource_reader.GetValueByCol( "ID" ), out this.ID );
        this.Name = resource_reader.GetValueByCol( "Name" );
        short.TryParse( resource_reader.GetValueByCol( "EditionType" ), out this.EditionType );
        this.Hide = resource_reader.GetValueByCol( "Hide" ) == "1";
        this.CanOverride = resource_reader.GetValueByCol( "CanOverride" ) == "1";
        this.IsBuff = resource_reader.GetValueByCol( "IsBuff" ) == "1";
        this.IsSave = resource_reader.GetValueByCol( "IsSave" ) == "1";
        this.CanBeDispel = resource_reader.GetValueByCol( "CanBeDispel" ) == "1";
        this.IsDeleteWhenDie = resource_reader.GetValueByCol( "IsDeleteWhenDie" ) == "1";
        this.PveBuff = resource_reader.GetValueByCol( "PveBuff" ) == "1";
        short.TryParse(resource_reader.GetValueByCol("TypeID"), out this.TypeID);
        short.TryParse( resource_reader.GetValueByCol( "ProcessType" ), out this.ProcessType );
        try {
           Type struct_type = StructListToXML.GetStructListType( "BuffHoldEffectConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "HoldEF" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.HoldEF = (BuffHoldEffectConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        try {
           Type struct_type = StructListToXML.GetStructListType( "BuffDirectEffectConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "DirectEF" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.DirectEF = (BuffDirectEffectConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        try {
           Type struct_type = StructListToXML.GetStructListType( "BuffDirectEffectConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "SpecialEF" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        this.RemoveOnAttack = resource_reader.GetValueByCol( "RemoveOnAttack" ) == "1";
        short.TryParse( resource_reader.GetValueByCol( "RemoveRate" ), out this.RemoveRate );
        this.Icon = resource_reader.GetValueByCol( "Icon" );
        this.IconFile = resource_reader.GetValueByCol( "IconFile" );
        this.ClientEffect = resource_reader.GetValueByCol( "ClientEffect" );
        this.Comment = resource_reader.GetValueByCol( "Comment" );
        this.BuffComment = resource_reader.GetValueByCol( "BuffComment" );
    }
} 

public partial class ItemReference : PropDefine {
    public override void LoadReference( ResourceUtilReader resource_reader ) {
        short.TryParse( resource_reader.GetValueByCol( "ID" ), out this.ID );
        this.Name = resource_reader.GetValueByCol( "Name" );
        short.TryParse( resource_reader.GetValueByCol( "EditionType" ), out this.EditionType );
        short.TryParse( resource_reader.GetValueByCol( "occupation" ), out this.occupation );
        short.TryParse( resource_reader.GetValueByCol( "level" ), out this.level );
        short.TryParse( resource_reader.GetValueByCol( "MaxUselevel" ), out this.MaxUselevel );
        short.TryParse( resource_reader.GetValueByCol( "ItemLevel" ), out this.ItemLevel );
        short.TryParse( resource_reader.GetValueByCol( "Count" ), out this.Count );
        this.CanUse = resource_reader.GetValueByCol( "CanUse" ) == "1";
        this.CanNotUseInFight = resource_reader.GetValueByCol( "CanNotUseInFight" ) == "1";
        //this.OnlySelf = resource_reader.GetValueByCol( "OnlySelf" ) == "1";
        //short.TryParse( resource_reader.GetValueByCol( "TargetType" ), out this.TargetType );
        //short.TryParse( resource_reader.GetValueByCol( "CastTime" ), out this.CastTime );
        //short.TryParse( resource_reader.GetValueByCol( "Distance" ), out this.Distance );
        this.NotRemove = resource_reader.GetValueByCol( "NotRemove" ) == "1";
        //int.TryParse( resource_reader.GetValueByCol( "CD" ), out this.CD );
        //short.TryParse( resource_reader.GetValueByCol( "NeedNPC" ), out this.NeedNPC );
        try {
           Type struct_type = StructListToXML.GetStructListType( "ItemUseConditionConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "UseCondition" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.UseCondition = (ItemUseConditionConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        int.TryParse( resource_reader.GetValueByCol( "EquipAppearID" ), out this.EquipAppearID );
        short.TryParse( resource_reader.GetValueByCol( "Quality" ), out this.Quality );
        short.TryParse( resource_reader.GetValueByCol( "IK1" ), out this.IK1 );
        short.TryParse( resource_reader.GetValueByCol( "IK2" ), out this.IK2 );
        short.TryParse( resource_reader.GetValueByCol( "IK3" ), out this.IK3 );
        int.TryParse( resource_reader.GetValueByCol( "Attribute" ), out this.Attribute );
        //this.HERO_SOUL_EX = resource_reader.GetValueByCol( "HERO_SOUL_EX" ) == "1";
        int.TryParse( resource_reader.GetValueByCol( "FitPart" ), out this.FitPart );
        //int.TryParse( resource_reader.GetValueByCol( "MaxHoles" ), out this.MaxHoles );
        int.TryParse( resource_reader.GetValueByCol( "Price" ), out this.Price );
        int.TryParse( resource_reader.GetValueByCol( "EndTime" ), out this.EndTime );
        //short.TryParse(resource_reader.GetValueByCol("LifeMax"), out this.LifeMax);
        //short.TryParse(resource_reader.GetValueByCol("LifeReplenish"), out this.LifeReplenish);
        //short.TryParse(resource_reader.GetValueByCol("NormalAtt"), out this.NormalAtt);
        //short.TryParse(resource_reader.GetValueByCol("NormalDef"), out this.NormalDef);
        //short.TryParse(resource_reader.GetValueByCol("WalkSpeed"), out this.WalkSpeed);
        //short.TryParse(resource_reader.GetValueByCol("AttSpeed"), out this.AttSpeed);
        //short.TryParse(resource_reader.GetValueByCol("HitValue"), out this.HitValue);
        //short.TryParse(resource_reader.GetValueByCol("DodgeValue"), out this.DodgeValue);
        //short.TryParse(resource_reader.GetValueByCol("CritValue"), out this.CritValue);
        //short.TryParse(resource_reader.GetValueByCol("CritAvoidValue"), out this.CritAvoidValue);
        //short.TryParse(resource_reader.GetValueByCol("CritMultiply"), out this.CritMultiply);
        //short.TryParse(resource_reader.GetValueByCol("WindAtt"), out this.WindAtt);
        //short.TryParse(resource_reader.GetValueByCol("FireAtt"), out this.FireAtt);
        //short.TryParse(resource_reader.GetValueByCol("ThunderAtt"), out this.ThunderAtt);
        //short.TryParse(resource_reader.GetValueByCol("WindDef"), out this.WindDef);
        //short.TryParse(resource_reader.GetValueByCol("FireDef"), out this.FireDef);
        //short.TryParse(resource_reader.GetValueByCol("ThunderDef"), out this.ThunderDef);

        //this.NotStar = resource_reader.GetValueByCol( "NotStar" ) == "1";
        //short.TryParse( resource_reader.GetValueByCol( "EndureStar" ), out this.EndureStar );
        //short.TryParse( resource_reader.GetValueByCol( "AttStar" ), out this.AttStar );
        //short.TryParse( resource_reader.GetValueByCol( "PhyDefStar" ), out this.PhyDefStar );
        short.TryParse(resource_reader.GetValueByCol("MaxStar"), out this.MaxStar);
        try {// 基础属性
           Type struct_type = StructListToXML.GetStructListType( "HoldEffectConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "HoldEffect" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.HoldEffect = (HoldEffectConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        try{// 品质属性
            Type struct_type = StructListToXML.GetStructListType("QualityEffectConfig[]");
            string obj_value = resource_reader.GetValueByCol("QualityEffect");
            Array array = StructListToXML.XMLToArray(obj_value as string, struct_type);
            this.HoldEffect = (HoldEffectConfig[])array;
        }
        catch (Exception e)
        {
            throw new Exception(string.Format("ID:{0} {1} ", this.ID, e.ToString()));
        }
        // try {
        //   Type struct_type = StructListToXML.GetStructListType( "RandomEffectConfig[]" );
        //   string obj_value = resource_reader.GetValueByCol( "RandomEffect" );
        //   Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
        //   this.RandomEffect = (RandomEffectConfig[])array;
        // } catch ( Exception e ) {
        //   throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        // }
        // 随机属性
        short.TryParse(resource_reader.GetValueByCol("RandomEffectBaseValue"), out this.RandomEffectBaseValue);

        try {
           Type struct_type = StructListToXML.GetStructListType( "FunItemConf[]" );
           string obj_value = resource_reader.GetValueByCol( "ItemFunParam" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.ItemFunParam = (FunItemConf[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        try {
           Type struct_type = StructListToXML.GetStructListType( "DropTableConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "DropItem" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.DropItem = (DropTableConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        this.Icon = resource_reader.GetValueByCol( "Icon" );
        this.IconFile = resource_reader.GetValueByCol( "IconFile" );
        this.Resource = resource_reader.GetValueByCol( "Resource" );
        this.FemaleResource = resource_reader.GetValueByCol( "FemaleResource" );
        //this.ActionPath = resource_reader.GetValueByCol( "ActionPath" );
        this.Comment = resource_reader.GetValueByCol( "Comment" );
       // this.AvatarComment = resource_reader.GetValueByCol( "AvatarComment" );
        this.ItemComment = resource_reader.GetValueByCol( "ItemComment" );
    }
} 

public partial class MapReference : PropDefine {
    public override void LoadReference( ResourceUtilReader resource_reader ) {
        int.TryParse( resource_reader.GetValueByCol( "ID" ), out this.ID );
        this.Name = resource_reader.GetValueByCol( "Name" );
        //short.TryParse( resource_reader.GetValueByCol( "EditionType" ), out this.EditionType );
        //short.TryParse( resource_reader.GetValueByCol( "Type" ), out this.Type );
        //short.TryParse( resource_reader.GetValueByCol( "DungeonType" ), out this.DungeonType );
        //this.DungeonOpen = resource_reader.GetValueByCol( "DungeonOpen" ) == "1";
        //this.PathSearchable = resource_reader.GetValueByCol( "PathSearchable" ) == "1";
        this.FileName = resource_reader.GetValueByCol( "FileName" );
        //this.SceneMapFilename = resource_reader.GetValueByCol( "SceneMapFilename" );
        //this.RadarFilename = resource_reader.GetValueByCol( "RadarFilename" );
        //this.ZoneFileName = resource_reader.GetValueByCol( "ZoneFileName" );
        //this.ConfigFileName = resource_reader.GetValueByCol( "ConfigFileName" );
        //short.TryParse( resource_reader.GetValueByCol( "MinPlayer" ), out this.MinPlayer );
        //short.TryParse( resource_reader.GetValueByCol( "MaxPlayer" ), out this.MaxPlayer );
        int.TryParse( resource_reader.GetValueByCol( "Width" ), out this.Width );
        int.TryParse( resource_reader.GetValueByCol( "Height" ), out this.Height );
        //try {
        //   Type struct_type = StructListToXML.GetStructListType( "MapValidRegion[]" );
        //   string obj_value = resource_reader.GetValueByCol( "valid_region" );
        //   Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
        //   this.valid_region = (MapValidRegion[])array;
        //} catch ( Exception e ) {
        //   throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        //}
        //int.TryParse( resource_reader.GetValueByCol( "BuffRef" ), out this.BuffRef );
        //int.TryParse( resource_reader.GetValueByCol( "BuffLevel" ), out this.BuffLevel );
        //try {
        //   Type struct_type = StructListToXML.GetStructListType( "MapPosConfig[]" );
        //   string obj_value = resource_reader.GetValueByCol( "MapPosConf" );
        //   Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
        //   this.MapPosConf = (MapPosConfig[])array;
        //} catch ( Exception e ) {
        //   throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        //}
        //short.TryParse( resource_reader.GetValueByCol( "ProtectSec" ), out this.ProtectSec );
        //short.TryParse( resource_reader.GetValueByCol( "LifeTime" ), out this.LifeTime );
        //short.TryParse( resource_reader.GetValueByCol( "FreeEnterCount" ), out this.FreeEnterCount );
        //short.TryParse( resource_reader.GetValueByCol( "NeedItem" ), out this.NeedItem );
        //short.TryParse( resource_reader.GetValueByCol( "NeedItemCount" ), out this.NeedItemCount );
        //short.TryParse( resource_reader.GetValueByCol( "NeedQuest" ), out this.NeedQuest );
        //short.TryParse( resource_reader.GetValueByCol( "NeedLevelMin" ), out this.NeedLevelMin );
        //short.TryParse( resource_reader.GetValueByCol( "NeedLevelMax" ), out this.NeedLevelMax );
        //short.TryParse( resource_reader.GetValueByCol( "RecommendLevel" ), out this.RecommendLevel );
        //try {
        //   Type struct_type = StructListToXML.GetStructListType( "ScoreDieCountConfig[]" );
        //   string obj_value = resource_reader.GetValueByCol( "DieCount" );
        //   Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
        //} catch ( Exception e ) {
        //   throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        //}
        //try {
        //   Type struct_type = StructListToXML.GetStructListType( "ScoreCompleteDungeonTimeConfig[]" );
        //   string obj_value = resource_reader.GetValueByCol( "UseTime" );
        //   Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
        //} catch ( Exception e ) {
        //   throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        //}
        //try {
        //   Type struct_type = StructListToXML.GetStructListType( "ScoreItemConfig[]" );
        //   string obj_value = resource_reader.GetValueByCol( "UseItem" );
        //   Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
        //} catch ( Exception e ) {
        //   throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        //}
        //try {
        //   Type struct_type = StructListToXML.GetStructListType( "ScoreItemConfig[]" );
        //   string obj_value = resource_reader.GetValueByCol( "NotUseItem" );
        //   Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
        //} catch ( Exception e ) {
        //   throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        //}
        //try {
        //   Type struct_type = StructListToXML.GetStructListType( "ScoreKillNPCConfig[]" );
        //   string obj_value = resource_reader.GetValueByCol( "KillNPC" );
        //   Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
        //} catch ( Exception e ) {
        //   throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        //}
        //try {
        //   Type struct_type = StructListToXML.GetStructListType( "ScoreItemConfig[]" );
        //   string obj_value = resource_reader.GetValueByCol( "CollectItem" );
        //   Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
        //} catch ( Exception e ) {
        //   throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        //}
        //try {
        //   Type struct_type = StructListToXML.GetStructListType( "ScoreTriggeConfig[]" );
        //   string obj_value = resource_reader.GetValueByCol( "Trigge" );
        //   Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
        //} catch ( Exception e ) {
        //   throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        //}
        //try {
        //   Type struct_type = StructListToXML.GetStructListType( "ScorePerformanceConfig[]" );
        //   string obj_value = resource_reader.GetValueByCol( "Performance" );
        //   Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
        //} catch ( Exception e ) {
        //   throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        //}
        //int.TryParse( resource_reader.GetValueByCol( "Level1Score" ), out this.Level1Score );
        //try {
        //   Type struct_type = StructListToXML.GetStructListType( "ScoreAwardItemConfig[]" );
        //   string obj_value = resource_reader.GetValueByCol( "Level1Award" );
        //   Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
        //   this.Level1Award = (ScoreAwardItemConfig[])array;
        //} catch ( Exception e ) {
        //   throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        //}
        //int.TryParse( resource_reader.GetValueByCol( "Level2Score" ), out this.Level2Score );
        //try {
        //   Type struct_type = StructListToXML.GetStructListType( "ScoreAwardItemConfig[]" );
        //   string obj_value = resource_reader.GetValueByCol( "Level2Award" );
        //   Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
        //   this.Level2Award = (ScoreAwardItemConfig[])array;
        //} catch ( Exception e ) {
        //   throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        //}
        //int.TryParse( resource_reader.GetValueByCol( "Level3Score" ), out this.Level3Score );
        //try {
        //   Type struct_type = StructListToXML.GetStructListType( "ScoreAwardItemConfig[]" );
        //   string obj_value = resource_reader.GetValueByCol( "Level3Award" );
        //   Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
        //   this.Level3Award = (ScoreAwardItemConfig[])array;
        //} catch ( Exception e ) {
        //   throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        //}
        //try {
        //   Type struct_type = StructListToXML.GetStructListType( "ScoreAwardItemConfig[]" );
        //   string obj_value = resource_reader.GetValueByCol( "Level4Award" );
        //   Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
        //   this.Level4Award = (ScoreAwardItemConfig[])array;
        //} catch ( Exception e ) {
        //   throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        //}
        //try {
        //   Type struct_type = StructListToXML.GetStructListType( "DropTableConfig[]" );
        //   string obj_value = resource_reader.GetValueByCol( "DropItem" );
        //   Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
        //   this.DropItem = (DropTableConfig[])array;
        //} catch ( Exception e ) {
        //   throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        //}
        //this.Music = resource_reader.GetValueByCol( "Music" );
        //this.MapDesc = resource_reader.GetValueByCol( "MapDesc" );
    }
} 

public partial class MapObjectReference : PropDefine {
    public override void LoadReference( ResourceUtilReader resource_reader ) {
        short.TryParse( resource_reader.GetValueByCol( "ID" ), out this.ID );
        this.Name = resource_reader.GetValueByCol( "Name" );
        short.TryParse( resource_reader.GetValueByCol( "EditionType" ), out this.EditionType );
        int.TryParse( resource_reader.GetValueByCol( "Apprid" ), out this.Apprid );
        short.TryParse( resource_reader.GetValueByCol( "RefreshSec" ), out this.RefreshSec );
        short.TryParse( resource_reader.GetValueByCol( "Trigge" ), out this.Trigge );
        int.TryParse( resource_reader.GetValueByCol( "OpenTime" ), out this.OpenTime );
        short.TryParse( resource_reader.GetValueByCol( "CollectCount" ), out this.CollectCount );
        short.TryParse( resource_reader.GetValueByCol( "NeedItemRef" ), out this.NeedItemRef );
        short.TryParse( resource_reader.GetValueByCol( "RoleLevel" ), out this.RoleLevel );
        short.TryParse( resource_reader.GetValueByCol( "ProductSkill" ), out this.ProductSkill );
        short.TryParse( resource_reader.GetValueByCol( "GrayLevel" ), out this.GrayLevel );
        short.TryParse( resource_reader.GetValueByCol( "GreenLevel" ), out this.GreenLevel );
        short.TryParse( resource_reader.GetValueByCol( "YellowLevel" ), out this.YellowLevel );
        short.TryParse( resource_reader.GetValueByCol( "EF1" ), out this.EF1 );
        try {
           Type struct_type = StructListToXML.GetStructListType( "DropTableConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "Data" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.Data = (DropTableConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        int.TryParse( resource_reader.GetValueByCol( "radius" ), out this.radius );
        int.TryParse( resource_reader.GetValueByCol( "P1" ), out this.P1 );
        int.TryParse( resource_reader.GetValueByCol( "P2" ), out this.P2 );
        int.TryParse( resource_reader.GetValueByCol( "P3" ), out this.P3 );
        int.TryParse( resource_reader.GetValueByCol( "P4" ), out this.P4 );
        int.TryParse( resource_reader.GetValueByCol( "P5" ), out this.P5 );
        this.Comment = resource_reader.GetValueByCol( "Comment" );
    }
} 

public partial class MiscReference : PropDefine {
    public override void LoadReference( ResourceUtilReader resource_reader ) {
        short.TryParse( resource_reader.GetValueByCol( "ID" ), out this.ID );
        this.Name = resource_reader.GetValueByCol( "Name" );
        short.TryParse( resource_reader.GetValueByCol( "Type" ), out this.Type );
        short.TryParse( resource_reader.GetValueByCol( "EditionType" ), out this.EditionType );
        this.object_json = resource_reader.GetValueByCol( "object_json" );
        try {
           Type struct_type = StructListToXML.GetStructListType( "MiscConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "misc_conf" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.misc_conf = (MiscConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
    }
} 

public partial class ProductReference : PropDefine {
    public override void LoadReference( ResourceUtilReader resource_reader ) {
        short.TryParse( resource_reader.GetValueByCol( "ID" ), out this.ID );
        this.Name = resource_reader.GetValueByCol( "Name" );
        short.TryParse( resource_reader.GetValueByCol( "EditionType" ), out this.EditionType );
        short.TryParse( resource_reader.GetValueByCol( "ProductTime" ), out this.ProductTime );
        short.TryParse( resource_reader.GetValueByCol( "RoleLevel" ), out this.RoleLevel );
        short.TryParse( resource_reader.GetValueByCol( "NeedMapObject" ), out this.NeedMapObject );
        short.TryParse( resource_reader.GetValueByCol( "ProductSkill" ), out this.ProductSkill );
        short.TryParse( resource_reader.GetValueByCol( "ProductSubSkill" ), out this.ProductSubSkill );
        short.TryParse( resource_reader.GetValueByCol( "ThirdType" ), out this.ThirdType );
        short.TryParse( resource_reader.GetValueByCol( "GrayLevel" ), out this.GrayLevel );
        short.TryParse( resource_reader.GetValueByCol( "GreenLevel" ), out this.GreenLevel );
        short.TryParse( resource_reader.GetValueByCol( "YellowLevel" ), out this.YellowLevel );
        short.TryParse( resource_reader.GetValueByCol( "AddSkillPoint" ), out this.AddSkillPoint );
        this.Avatar = resource_reader.GetValueByCol( "Avatar" ) == "1";
        this.AvatarTitle = resource_reader.GetValueByCol( "AvatarTitle" );
        this.AvatarIconPath = resource_reader.GetValueByCol( "AvatarIconPath" );
        this.AvatarIcon = resource_reader.GetValueByCol( "AvatarIcon" );
        int.TryParse( resource_reader.GetValueByCol( "CostVigour" ), out this.CostVigour );
        int.TryParse( resource_reader.GetValueByCol( "CostMoney" ), out this.CostMoney );
        try {
           Type struct_type = StructListToXML.GetStructListType( "MaterialListConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "MaterialList" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.MaterialList = (MaterialListConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        short.TryParse( resource_reader.GetValueByCol( "ProductItemRef" ), out this.ProductItemRef );
        try {
           Type struct_type = StructListToXML.GetStructListType( "DropTableConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "Item" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.Item = (DropTableConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        this.Comment = resource_reader.GetValueByCol( "Comment" );
    }
} 

public partial class QuestReference : PropDefine {
    public override void LoadReference( ResourceUtilReader resource_reader ) {
        short.TryParse( resource_reader.GetValueByCol( "ID" ), out this.ID );
        this.Name = resource_reader.GetValueByCol( "Name" );
        short.TryParse( resource_reader.GetValueByCol( "EditionType" ), out this.EditionType );
        this.AutoAccept = resource_reader.GetValueByCol( "AutoAccept" ) == "1";
        short.TryParse( resource_reader.GetValueByCol( "QuestGroup" ), out this.QuestGroup );
        this.NoCancel = resource_reader.GetValueByCol( "NoCancel" ) == "1";
        short.TryParse( resource_reader.GetValueByCol( "TaskType" ), out this.TaskType );
        this.TaskDesc = resource_reader.GetValueByCol( "TaskDesc" );
        short.TryParse( resource_reader.GetValueByCol( "CleanType" ), out this.CleanType );
        short.TryParse( resource_reader.GetValueByCol( "PlayerLevel" ), out this.PlayerLevel );
        byte.TryParse( resource_reader.GetValueByCol( "Difficult" ), out this.Difficult );
        int.TryParse( resource_reader.GetValueByCol( "Mutex" ), out this.Mutex );
        short.TryParse( resource_reader.GetValueByCol( "FindPathType" ), out this.FindPathType );
        short.TryParse( resource_reader.GetValueByCol( "RepeatCount" ), out this.RepeatCount );
        try {
           Type struct_type = StructListToXML.GetStructListType( "VipCountConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "VipRepeatCnt" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.VipRepeatCnt = (VipCountConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        short.TryParse( resource_reader.GetValueByCol( "HolidayType" ), out this.HolidayType );
        this.HolidayBegin = resource_reader.GetValueByCol( "HolidayBegin" );
        this.HolidayEnd = resource_reader.GetValueByCol( "HolidayEnd" );
        this.HolidayPeriod = resource_reader.GetValueByCol( "HolidayPeriod" );
        this.Hot = resource_reader.GetValueByCol( "Hot" ) == "1";
        this.HideInPanel = resource_reader.GetValueByCol( "HideInPanel" ) == "1";
        this.HideCondition = resource_reader.GetValueByCol( "HideCondition" ) == "1";
        this.HolidayDesc = resource_reader.GetValueByCol( "HolidayDesc" );
        int.TryParse( resource_reader.GetValueByCol( "StartNpcTypeId" ), out this.StartNpcTypeId );
        this.StartAskText = resource_reader.GetValueByCol( "StartAskText" );
        try {
           Type struct_type = StructListToXML.GetStructListType( "QuestAcceptAction[]" );
           string obj_value = resource_reader.GetValueByCol( "AcceptActions" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.AcceptActions = (QuestAcceptAction[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        this.AutoSubmit = resource_reader.GetValueByCol( "AutoSubmit" ) == "1";
        int.TryParse( resource_reader.GetValueByCol( "EndNpcTypeId" ), out this.EndNpcTypeId );
        this.EndNpcTalk = resource_reader.GetValueByCol( "EndNpcTalk" );
        short.TryParse( resource_reader.GetValueByCol( "InGroup" ), out this.InGroup );
        try {
           Type struct_type = StructListToXML.GetStructListType( "QuestGroupSetting[]" );
           string obj_value = resource_reader.GetValueByCol( "GroupSetting" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.GroupSetting = (QuestGroupSetting[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        int.TryParse( resource_reader.GetValueByCol( "Accessor" ), out this.Accessor );
        try {
           Type struct_type = StructListToXML.GetStructListType( "QuestGroupPutOut[]" );
           string obj_value = resource_reader.GetValueByCol( "GroupPutOut" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        this.HideWhenCannotNow = resource_reader.GetValueByCol( "HideWhenCannotNow" ) == "1";
        try {
           Type struct_type = StructListToXML.GetStructListType( "QuestRequestConditionCfg[]" );
           string obj_value = resource_reader.GetValueByCol( "RequestConfig" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.RequestConfig = (QuestRequestConditionCfg[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        short.TryParse( resource_reader.GetValueByCol( "MinPlayerLevel" ), out this.MinPlayerLevel );
        short.TryParse( resource_reader.GetValueByCol( "MaxPlayerLevel" ), out this.MaxPlayerLevel );
        short.TryParse( resource_reader.GetValueByCol( "Gender" ), out this.Gender );
        short.TryParse( resource_reader.GetValueByCol( "Occupation" ), out this.Occupation );
        short.TryParse( resource_reader.GetValueByCol( "Camp" ), out this.Camp );
        short.TryParse( resource_reader.GetValueByCol( "BeforeQuest" ), out this.BeforeQuest );
        short.TryParse( resource_reader.GetValueByCol( "Money" ), out this.Money );
        short.TryParse( resource_reader.GetValueByCol( "BeforeItemRef" ), out this.BeforeItemRef );
        short.TryParse( resource_reader.GetValueByCol( "BeforeItemCount" ), out this.BeforeItemCount );
        short.TryParse( resource_reader.GetValueByCol( "NeedSkill" ), out this.NeedSkill );
        try {
           Type struct_type = StructListToXML.GetStructListType( "QuestRelation[]" );
           string obj_value = resource_reader.GetValueByCol( "RelationCfg" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.RelationCfg = (QuestRelation[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        short.TryParse( resource_reader.GetValueByCol( "TimeDown" ), out this.TimeDown );
        short.TryParse( resource_reader.GetValueByCol( "Time" ), out this.Time );
        this.FailIfDie = resource_reader.GetValueByCol( "FailIfDie" ) == "1";
        this.ScreenMsgItemIds = resource_reader.GetValueByCol( "ScreenMsgItemIds" );
        this.ScreenMsgCompleted = resource_reader.GetValueByCol( "ScreenMsgCompleted" ) == "1";
        try {
           Type struct_type = StructListToXML.GetStructListType( "ItemConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "AcquireItemWhenAcceptEx" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.AcquireItemWhenAcceptEx = (ItemConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        try {
           Type struct_type = StructListToXML.GetStructListType( "QuestItemConfigEx[]" );
           string obj_value = resource_reader.GetValueByCol( "ItemCfgEx" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.ItemCfgEx = (QuestItemConfigEx[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        this.KeepItemWhenSubmit = resource_reader.GetValueByCol( "KeepItemWhenSubmit" ) == "1";
        int.TryParse( resource_reader.GetValueByCol( "ConditionScriptId" ), out this.ConditionScriptId );
        try {
           Type struct_type = StructListToXML.GetStructListType( "QuestConditionCfg[]" );
           string obj_value = resource_reader.GetValueByCol( "ConditionCfg" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.ConditionCfg = (QuestConditionCfg[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        try {
           Type struct_type = StructListToXML.GetStructListType( "AreaConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "ArriveArea" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.ArriveArea = (AreaConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        try {
           Type struct_type = StructListToXML.GetStructListType( "NPCConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "KillNPC" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.KillNPC = (NPCConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        try {
           Type struct_type = StructListToXML.GetStructListType( "NPCConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "KillMultiNPC" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.KillMultiNPC = (NPCConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        try {
           Type struct_type = StructListToXML.GetStructListType( "NPCConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "KillMultiNPC_Both" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.KillMultiNPC_Both = (NPCConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        try {
           Type struct_type = StructListToXML.GetStructListType( "ItemConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "CollectItem" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.CollectItem = (ItemConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        try {
           Type struct_type = StructListToXML.GetStructListType( "ItemConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "CollectAnyOne" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.CollectAnyOne = (ItemConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        short.TryParse( resource_reader.GetValueByCol( "NoItem" ), out this.NoItem );
        try {
           Type struct_type = StructListToXML.GetStructListType( "HasSkillConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "HasSkill" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.HasSkill = (HasSkillConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        short.TryParse( resource_reader.GetValueByCol( "UpgradeToLevel" ), out this.UpgradeToLevel );
        try {
           Type struct_type = StructListToXML.GetStructListType( "ItemConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "UseItemEx" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.UseItemEx = (ItemConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        this.UseItemEx_Desc = resource_reader.GetValueByCol( "UseItemEx_Desc" );
        try {
           Type struct_type = StructListToXML.GetStructListType( "QuestMapObjectConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "TriggerMapObject" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.TriggerMapObject = (QuestMapObjectConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        try {
           Type struct_type = StructListToXML.GetStructListType( "AskConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "Ask" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.Ask = (AskConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        int.TryParse( resource_reader.GetValueByCol( "AwardExp" ), out this.AwardExp );
        int.TryParse( resource_reader.GetValueByCol( "AwardMoney" ), out this.AwardMoney );
        this.award_bind = resource_reader.GetValueByCol( "award_bind" ) == "1";
        int.TryParse( resource_reader.GetValueByCol( "AwardScriptId" ), out this.AwardScriptId );
        int.TryParse( resource_reader.GetValueByCol( "AwardFixedItemTypeId1" ), out this.AwardFixedItemTypeId1 );
        short.TryParse( resource_reader.GetValueByCol( "AwardFixedItemCount1" ), out this.AwardFixedItemCount1 );
        int.TryParse( resource_reader.GetValueByCol( "AwardFixedItemTypeId2" ), out this.AwardFixedItemTypeId2 );
        short.TryParse( resource_reader.GetValueByCol( "AwardFixedItemCount2" ), out this.AwardFixedItemCount2 );
        int.TryParse( resource_reader.GetValueByCol( "AwardOptionItemTypeId1" ), out this.AwardOptionItemTypeId1 );
        short.TryParse( resource_reader.GetValueByCol( "AwardOptionItemCount1" ), out this.AwardOptionItemCount1 );
        int.TryParse( resource_reader.GetValueByCol( "AwardOptionItemTypeId2" ), out this.AwardOptionItemTypeId2 );
        short.TryParse( resource_reader.GetValueByCol( "AwardOptionItemCount2" ), out this.AwardOptionItemCount2 );
        int.TryParse( resource_reader.GetValueByCol( "AwardOptionItemTypeId3" ), out this.AwardOptionItemTypeId3 );
        short.TryParse( resource_reader.GetValueByCol( "AwardOptionItemCount3" ), out this.AwardOptionItemCount3 );
        int.TryParse( resource_reader.GetValueByCol( "AwardOptionItemTypeId4" ), out this.AwardOptionItemTypeId4 );
        short.TryParse( resource_reader.GetValueByCol( "AwardOptionItemCount4" ), out this.AwardOptionItemCount4 );
        int.TryParse( resource_reader.GetValueByCol( "AwardConditionType" ), out this.AwardConditionType );
        try {
           Type struct_type = StructListToXML.GetStructListType( "AwardCfg[]" );
           string obj_value = resource_reader.GetValueByCol( "AwardConditionItem" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.AwardConditionItem = (AwardCfg[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        int.TryParse( resource_reader.GetValueByCol( "Param" ), out this.Param );
    }
} 

public partial class SkillReference : PropDefine {
    public override void LoadReference( ResourceUtilReader resource_reader ) {
        short.TryParse( resource_reader.GetValueByCol( "ID" ), out this.ID );
        this.Name = resource_reader.GetValueByCol( "Name" );
        short.TryParse( resource_reader.GetValueByCol( "EditionType" ), out this.EditionType );
        int.TryParse( resource_reader.GetValueByCol( "Filter" ), out this.Filter );
        this.IgnoreFlee = resource_reader.GetValueByCol( "IgnoreFlee" ) == "1";
        this.IgnorePublicCD = resource_reader.GetValueByCol( "IgnorePublicCD" ) == "1";
        short.TryParse( resource_reader.GetValueByCol( "SkillTargetType" ), out this.SkillTargetType );
        short.TryParse( resource_reader.GetValueByCol( "TargetType" ), out this.TargetType );
        this.HasDelay = resource_reader.GetValueByCol( "HasDelay" ) == "1";
        int.TryParse( resource_reader.GetValueByCol( "DelayTime" ), out this.DelayTime );
        try {
           Type struct_type = StructListToXML.GetStructListType( "SkillEffectConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "level_effect" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.level_effect = (SkillEffectConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }

        this.SkillDesc = resource_reader.GetValueByCol( "SkillDesc" );
        this.Icon = resource_reader.GetValueByCol( "Icon" );
        this.IconFile = resource_reader.GetValueByCol( "IconFile" );
        this.AssetPath1 = resource_reader.GetValueByCol( "AssetPath1" );
        this.Comment = resource_reader.GetValueByCol( "Comment" );
    }
} 

public partial class TalentReference : PropDefine {
    public override void LoadReference( ResourceUtilReader resource_reader ) {
        short.TryParse( resource_reader.GetValueByCol( "ID" ), out this.ID );
        this.Name = resource_reader.GetValueByCol( "Name" );
        short.TryParse( resource_reader.GetValueByCol( "EditionType" ), out this.EditionType );
        short.TryParse( resource_reader.GetValueByCol( "MaxLevel" ), out this.MaxLevel );
        short.TryParse( resource_reader.GetValueByCol( "EF1" ), out this.EF1 );
        int.TryParse( resource_reader.GetValueByCol( "EF1P1" ), out this.EF1P1 );
        int.TryParse( resource_reader.GetValueByCol( "EF1P2" ), out this.EF1P2 );
        int.TryParse( resource_reader.GetValueByCol( "EF1P3" ), out this.EF1P3 );
        this.Comment = resource_reader.GetValueByCol( "Comment" );
        this.Icon = resource_reader.GetValueByCol( "Icon" );
        this.IconPath = resource_reader.GetValueByCol( "IconPath" );
    }
} 

public partial class EffectReference : PropDefine {
    public override void LoadReference( ResourceUtilReader resource_reader ) {
        short.TryParse( resource_reader.GetValueByCol( "ID" ), out this.ID );
        this.Name = resource_reader.GetValueByCol( "Name" );
        short.TryParse( resource_reader.GetValueByCol( "EditionType" ), out this.EditionType );
        short.TryParse( resource_reader.GetValueByCol( "EffectType" ), out this.EffectType );
        short.TryParse( resource_reader.GetValueByCol( "MinVal" ), out this.MinVal );
        short.TryParse( resource_reader.GetValueByCol( "MaxVal" ), out this.MaxVal );
        short.TryParse( resource_reader.GetValueByCol( "EquipQuality" ), out this.EquipQuality );
        short.TryParse(resource_reader.GetValueByCol( "EquipLevel" ), out this.EquipLevel);
    }


} 

public partial class PassiveSkillReference : PropDefine {
    public override void LoadReference( ResourceUtilReader resource_reader ) {
        short.TryParse( resource_reader.GetValueByCol( "ID" ), out this.ID );
        this.Name = resource_reader.GetValueByCol( "Name" );
        short.TryParse( resource_reader.GetValueByCol( "EditionType" ), out this.EditionType );
      //  this.Warrior = resource_reader.GetValueByCol( "Warrior" ) == "1";
        //this.Cavalier = resource_reader.GetValueByCol( "Cavalier" ) == "1";
     //   this.Mage = resource_reader.GetValueByCol( "Mage" ) == "1";
     //   this.Warlock = resource_reader.GetValueByCol( "Warlock" ) == "1";
        try {
           Type struct_type = StructListToXML.GetStructListType( "PassiveSkilllConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "Conf" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.Conf = (PassiveSkilllConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        this.Icon = resource_reader.GetValueByCol( "Icon" );
        this.IconPath = resource_reader.GetValueByCol( "IconPath" );
        this.Descriptions = resource_reader.GetValueByCol( "Descriptions" );
    }
} 

public partial class AIReference : PropDefine {
    public override void LoadReference( ResourceUtilReader resource_reader ) {
        short.TryParse( resource_reader.GetValueByCol( "ID" ), out this.ID );
        this.Name = resource_reader.GetValueByCol( "Name" );
        short.TryParse( resource_reader.GetValueByCol( "EditionType" ), out this.EditionType );
        short.TryParse( resource_reader.GetValueByCol( "Map" ), out this.Map );
        this.Performance = resource_reader.GetValueByCol( "Performance" ) == "1";
        try {
           Type struct_type = StructListToXML.GetStructListType( "AIEventConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "ai_str" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.ai_str = (AIEventConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
    }
} 

public partial class ScheduledReference : PropDefine {
    public override void LoadReference( ResourceUtilReader resource_reader ) {
        short.TryParse( resource_reader.GetValueByCol( "ID" ), out this.ID );
        this.Name = resource_reader.GetValueByCol( "Name" );
        short.TryParse( resource_reader.GetValueByCol( "EditionType" ), out this.EditionType );
        this.Close = resource_reader.GetValueByCol( "Close" ) == "1";
        short.TryParse( resource_reader.GetValueByCol( "BeginMonth" ), out this.BeginMonth );
        short.TryParse( resource_reader.GetValueByCol( "BeginDay" ), out this.BeginDay );
        short.TryParse( resource_reader.GetValueByCol( "BeginWeekDay" ), out this.BeginWeekDay );
        short.TryParse( resource_reader.GetValueByCol( "BeginHour" ), out this.BeginHour );
        short.TryParse( resource_reader.GetValueByCol( "BeginMinute" ), out this.BeginMinute );
        short.TryParse( resource_reader.GetValueByCol( "EndMonth" ), out this.EndMonth );
        short.TryParse( resource_reader.GetValueByCol( "EndDay" ), out this.EndDay );
        short.TryParse( resource_reader.GetValueByCol( "EndWeekDay" ), out this.EndWeekDay );
        short.TryParse( resource_reader.GetValueByCol( "EndHour" ), out this.EndHour );
        short.TryParse( resource_reader.GetValueByCol( "EndMinute" ), out this.EndMinute );
        this.IsMovement = resource_reader.GetValueByCol( "IsMovement" ) == "1";
        this.BeginNotify = resource_reader.GetValueByCol( "BeginNotify" ) == "1";
        short.TryParse( resource_reader.GetValueByCol( "EF" ), out this.EF );
        int.TryParse( resource_reader.GetValueByCol( "EFP1" ), out this.EFP1 );
        int.TryParse( resource_reader.GetValueByCol( "EFP2" ), out this.EFP2 );
        this.EFP3 = resource_reader.GetValueByCol( "EFP3" );
        try {
           Type struct_type = StructListToXML.GetStructListType( "ScheduledConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "scheduled_list" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.scheduled_list = (ScheduledConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        try {
           Type struct_type = StructListToXML.GetStructListType( "ShowRoleConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "ShowRole" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.ShowRole = (ShowRoleConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
        try {
           Type struct_type = StructListToXML.GetStructListType( "DropTableConfig[]" );
           string obj_value = resource_reader.GetValueByCol( "DropItem" );
           Array array = StructListToXML.XMLToArray( obj_value as string, struct_type );
           this.DropItem = (DropTableConfig[])array;
        } catch ( Exception e ) {
           throw new Exception( string.Format( "ID:{0} {1} ", this.ID, e.ToString() ) );
        }
    }
} 

public partial class MapObjectConfigReference : PropDefine {
    public override void LoadReference( ResourceUtilReader resource_reader ) {
        int.TryParse( resource_reader.GetValueByCol( "ID" ), out this.ID );
        this.Name = resource_reader.GetValueByCol( "Name" );
        short.TryParse( resource_reader.GetValueByCol( "EditionType" ), out this.EditionType );
        int.TryParse( resource_reader.GetValueByCol( "map" ), out this.map );
        this.isRole = resource_reader.GetValueByCol( "isRole" ) == "1";
        int.TryParse( resource_reader.GetValueByCol( "sn" ), out this.sn );
        int.TryParse( resource_reader.GetValueByCol( "refid" ), out this.refid );
        this.available = resource_reader.GetValueByCol( "available" ) == "1";
        this.position = resource_reader.GetValueByCol( "position" );
        this.direction = resource_reader.GetValueByCol( "direction" );
        short.TryParse( resource_reader.GetValueByCol( "aiRefid" ), out this.aiRefid );
        int.TryParse( resource_reader.GetValueByCol( "camp" ), out this.camp );
        this.patrolType = resource_reader.GetValueByCol( "patrolType" );
        this.route = resource_reader.GetValueByCol( "route" );
        int.TryParse( resource_reader.GetValueByCol( "teamID" ), out this.teamID );
    }
} 

public partial class StoreReference : PropDefine {
    public override void LoadReference( ResourceUtilReader resource_reader ) {
        int.TryParse( resource_reader.GetValueByCol( "ID" ), out this.ID );
        this.Name = resource_reader.GetValueByCol( "Name" );
        short.TryParse( resource_reader.GetValueByCol( "EditionType" ), out this.EditionType );
        int.TryParse( resource_reader.GetValueByCol( "Item" ), out this.Item );
        int.TryParse( resource_reader.GetValueByCol( "Count" ), out this.Count );
        this.Hot = resource_reader.GetValueByCol( "Hot" ) == "1";
        this.Vip = resource_reader.GetValueByCol( "Vip" ) == "1";
        int.TryParse( resource_reader.GetValueByCol( "MainType" ), out this.MainType );
        int.TryParse( resource_reader.GetValueByCol( "SubType" ), out this.SubType );
        int.TryParse( resource_reader.GetValueByCol( "Price" ), out this.Price );
        int.TryParse( resource_reader.GetValueByCol( "VipPrice" ), out this.VipPrice );
        this.Hide = resource_reader.GetValueByCol( "Hide" ) == "1";
        int.TryParse( resource_reader.GetValueByCol( "Sort" ), out this.Sort );
        this.begin_time = resource_reader.GetValueByCol( "begin_time" );
        this.end_time = resource_reader.GetValueByCol( "end_time" );
        this.show_time = resource_reader.GetValueByCol( "show_time" );
        int.TryParse( resource_reader.GetValueByCol( "limited_count" ), out this.limited_count );
        this.Comment = resource_reader.GetValueByCol( "Comment" );
    }
} 

public partial class QuestionReference : PropDefine {
    public override void LoadReference( ResourceUtilReader resource_reader ) {
        int.TryParse( resource_reader.GetValueByCol( "ID" ), out this.ID );
        this.Name = resource_reader.GetValueByCol( "Name" );
        short.TryParse( resource_reader.GetValueByCol( "EditionType" ), out this.EditionType );
        this.Q1 = resource_reader.GetValueByCol( "Q1" );
        int.TryParse( resource_reader.GetValueByCol( "AIndex" ), out this.AIndex );
        this.A1 = resource_reader.GetValueByCol( "A1" );
        this.A2 = resource_reader.GetValueByCol( "A2" );
        this.A3 = resource_reader.GetValueByCol( "A3" );
        this.A4 = resource_reader.GetValueByCol( "A4" );
        this.A5 = resource_reader.GetValueByCol( "A5" );
    }
} 

public partial class GenericScriptReference : PropDefine {
    public override void LoadReference( ResourceUtilReader resource_reader ) {
        int.TryParse( resource_reader.GetValueByCol( "ID" ), out this.ID );
        this.Name = resource_reader.GetValueByCol( "Name" );
        short.TryParse( resource_reader.GetValueByCol( "EditionType" ), out this.EditionType );
        this.GenericScript = resource_reader.GetValueByCol( "GenericScript" );
    }
} 

