using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;

public struct VectorSize
{
	public int width;
	public int height;
	
	public VectorSize(int w, int h)
	{
		this.width = w;
		this.height = h;
	}
}

public struct VectorPosition
{
	public int x;
	public int y;
	
	public VectorPosition(int offsetX, int offsetY)
	{
		this.x = offsetX;
		this.y = offsetY;
	}
}

public class ClientSystemConstants
{
	public static VectorSize BUILDING_TILE_MAP_SIZE = new VectorSize(42, 42);
	public static VectorSize ACTOR_TILE_MAP_SIZE = new VectorSize(84, 84);
	public static VectorPosition BUILDING_TILE_MAP_OFFSET = new VectorPosition(0, 0);
	public static VectorPosition ACTOR_TILE_MAP_OFFSET = new VectorPosition(0, 0);
	public static VectorSize BUILDING_TILE_MAP_TILE_SIZE = new VectorSize(40, 40);
	public static VectorSize ACTOR_TILE_MAP_TILE_SIZE = new VectorSize(20, 20);
    public static WorldRange WORLDRANGE = new WorldRange(1852, -196, -180, 1868);
	
	public const int INITIAL_REMOVABLE_OBJECT_NUMBER = 24;
	public static Rect INITIAL_REMOVABLE_OBJECT_INVALID_RECT = new Rect(
		BUILDING_TILE_MAP_SIZE.width / 2 - 6, BUILDING_TILE_MAP_SIZE.height / 2 - 6, 12, 12);

    public static Color HP_BAR_FULL_COLOR = Color.green;
    public static Color HP_BAR_MIDDLE_COLOR = Color.yellow;
    public static Color HP_BAR_EMPTY_COLOR = Color.red;
    public const float HP_MIDDLE_PERCENTAGE = 0.5f;

    public const string CAMARE_MANAGER_OBJECT_NAME = "CameraManager";
    public const float CAMARE_ORIGINAL_SIZE = 320;
    public const float CAMERA_SIZE_STANDARD = 320;
    public static float CAMERA_SIZE_MIN = 210;
    public static float CAMERA_SIZE_MAX = 576;
	
	public static Color PURCHASE_SUCCESS_TIPS_COLOR = new Color(254,185,0,255);

    //public const float PROGRESS_INTERVAL = 50;
    //public const int USER_NAME_LENNTH = 16;
    public const string BACKGROUND_NAME = "BuildingBackgroundAnchor/BuildingBackground";
    public const string WALL_RIGHT = "BuildingBackgroundAnchor/BuildingBackground/BuildingBackgroundRight";
    public const string WALL_TOP = "BuildingBackgroundAnchor/BuildingBackground/BuildingBackgroundTop";
	public static ScreenResolution SCREENRESOLUTION = ScreenResolution.Size960X640;
	
    //big icon
    //public static Dictionary<ArmyType, string> ARMY_ICON_INFO_DICTIONARY = new Dictionary<ArmyType, string>()
    //{
    //    {ArmyType.Berserker, "UI_Icon_118"},
    //    {ArmyType.Ranger, "UI_Icon_123"},
    //    {ArmyType.Marauder, "UI_Icon_153"}
    //};
    //big icon
    //public static Dictionary<MercenaryType, string> MERCENARY_ICON_INFO_DICTIONARY = new Dictionary<MercenaryType, string>()
    //{
    //    {MercenaryType.Slinger, "UI_Icon_192"},
    //    {MercenaryType.Hercules, "UI_Icon_189"},
    //    {MercenaryType.Kodo ,"UI_Icon_203"},
    //    {MercenaryType.HerculesII ,"UI_Icon_210"},
    //    {MercenaryType.KodoII ,"UI_Icon_207"},
    //    {MercenaryType.Arsonist,"UI_Icon_200"}
    //};
    //Train Army in scene
    public static Dictionary<ArmyType, string> ARMY_ICON_COMMON_DICTIONARY = new Dictionary<ArmyType, string>()
    {
        {ArmyType.Berserker, "UI_Icon_119"},
		{ArmyType.Ranger, "UI_Icon_124"},
        {ArmyType.Marauder, "UI_Icon_154"},
        {ArmyType.MT, "UI_Icon_243"},
        {ArmyType.Bomberman, "UI_Icon_246"}
    };
    //Upgrade Army in scene
    public static Dictionary<ArmyType, string> ARMY_UPGRADE_ICON_COMMON_DICTIONARY = new Dictionary<ArmyType, string>()
    {
        {ArmyType.Berserker, "UI_Icon_195"},
		{ArmyType.Ranger, "UI_Icon_196"},
        {ArmyType.Marauder, "UI_Icon_197"},
        {ArmyType.MT, "UI_Icon_260"},
        {ArmyType.Bomberman, "UI_Icon_261"}
    };
    //public static Dictionary<MercenaryType, string> MERCENARY_ICON_COMMON_DICTIONARY = new Dictionary<MercenaryType, string>()
    //{
    //    {MercenaryType.Slinger, "UI_Icon_193"},
    //    {MercenaryType.Hercules, "UI_Icon_190"},
    //    {MercenaryType.Kodo ,"UI_Icon_204"},
    //    {MercenaryType.HerculesII ,"UI_Icon_211"},
    //    {MercenaryType.KodoII ,"UI_Icon_208"},
    //    {MercenaryType.Arsonist,"UI_Icon_201"}
    //};
    //Spell icon common
    public static Dictionary<ItemType, string> Spell_ICON_COMMON_DICTIONARY = new Dictionary<ItemType, string>()
    {
        {ItemType.Bomb, "???????????"}
    };
    //Upgrade building append item
    public static Dictionary<BuildingType, string> BUILDING_ICON_DICTIONARY = new Dictionary<BuildingType, string>()
    {
       { BuildingType.CityHall,""}, 
       { BuildingType.Barracks,"UI_Icon_113"},
       { BuildingType.ArmyCamp ,"UI_Icon_111"},
       { BuildingType.Fortress ,"UI_Icon_108"},
       { BuildingType.GoldMine,"UI_Icon_110"},
       { BuildingType.Farm ,"UI_Icon_106"},
       { BuildingType.GoldStorage ,"UI_Icon_109"},
       { BuildingType.FoodStorage ,"UI_Icon_107"},
       { BuildingType.BuilderHut ,"UI_Icon_112"},
       { BuildingType.Wall ,"UI_Icon_114"},
       { BuildingType.DefenseTower,"UI_Icon_113"},
	   { BuildingType.Tavern,"UI_Icon_206"},
       { BuildingType.PropsStorage,"UI_Icon_225"},
       { BuildingType.Artillery,"UI_Icon_250"}
	
    };
    public static Dictionary<ItemType, string> SPELL_ICON_DICTIONARY = new Dictionary<ItemType, string>()
    {
        {ItemType.Bomb,"Ui_Icon_UpgradeMagicBottle"}
    };

    public static Dictionary<BuildingCategory, string> BUILDINGCATEGORY_DICTIONARY = new Dictionary<BuildingCategory, string>()
    {
        {BuildingCategory.None,"任何"},
        {BuildingCategory.Any,"任何"},
        {BuildingCategory.Defense,"防御设施"},
        {BuildingCategory.Resource,"资源建筑"},
        {BuildingCategory.Wall,"墙"}
    };
    public static Dictionary<TargetType, string> TARGETTYPE_DICTIONARY = new Dictionary<TargetType, string>()
    {
        {TargetType.Air,"空中"},
        {TargetType.AirGround,"空中&地面"},
        {TargetType.Ground,"地面"}, 
    };
    public static Dictionary<AttackType, string> ATTACKTYPE_DICTIONARY = new Dictionary<AttackType, string>()
    {
        {AttackType.Single,"单体"},
        {AttackType.Group,"群体"}
    };
    public static Dictionary<UIMenuType, string> UIMENU_TYPE_DICTIONARY = new Dictionary<UIMenuType, string>()
    {
        {UIMenuType.Resource,"资 源 类"},
        {UIMenuType.Military,"军 事 类"},
        {UIMenuType.Composite,"综 合 类"},
        {UIMenuType.Defense,"防 御 类"}
    };
    
    public static Dictionary<BuildingType, string> BUILDING_NAME_DICTIONARY = new Dictionary<BuildingType, string>()
    {
       { BuildingType.CityHall,"市政厅"}, 
       { BuildingType.Barracks,"兵营"},
       { BuildingType.ArmyCamp ,"集结地"},
       { BuildingType.Fortress ,"堡垒"},
       { BuildingType.GoldMine,"金矿"},
       { BuildingType.Farm ,"农场"},
       { BuildingType.GoldStorage ,"金库"},
       { BuildingType.FoodStorage ,"食物库"},
       { BuildingType.BuilderHut ,"工棚"},
       { BuildingType.Wall ,"墙"},
       { BuildingType.DefenseTower,"防御塔"},
       { BuildingType.Tavern,"酒馆"},
       { BuildingType.PropsStorage,"道具仓库"}
    };
    public static Dictionary<ResourceType, string> RESOURCETYPEG_NAME_DICTIONARY = new Dictionary<ResourceType, string>()
    {
       { ResourceType.Gold,"金币"}, 
       { ResourceType.Food,"食物"}, 
       { ResourceType.Oil,"石油"}
    };
    public static Dictionary<ArmyType, string> ARMYTYPE_NAME_DICTIONARY = new Dictionary<ArmyType, string>()
    {
       { ArmyType.Berserker,"狂战士"}, 
       { ArmyType.Ranger,"游侠"}, 
       {ArmyType.Marauder,"掠夺者"}, 
    };
    public static Dictionary<ArmyCategory, string> ARMYCATEGORY_DICTIONARY = new Dictionary<ArmyCategory, string>()
    {
        { ArmyCategory.Any,"任何"}, 
        { ArmyCategory.None,"任何"}, 
        { ArmyCategory.MT,"肉盾"}, 
        { ArmyCategory.Magic,"魔法师"}, 
    };
    public static Dictionary<int, string> EXPRESSION_ICON_DICTIONARY = new Dictionary<int, string>() 
    {
        { 0,"/GOLD"}, 
        { 1,"/FOOD"}, 
        { 2,"/OIL"}, 
        { 3,"/GEM"}
    };
    public static Dictionary<int, string> ARROW_ICON_DICTIONARY = new Dictionary<int, string>()
    {
        { 0,"Right"},
        { 1,"Up"}, 
        { 2,"Left"}, 
        { 3,"Down"}
    };
    public static Dictionary<ResourceType, string> SCENE_RESOURCE_ICON_DICTIONARY = new Dictionary<ResourceType, string>()
    {
        { ResourceType.Gold,"UI_Icon_059"}, 
        { ResourceType.Food,"UI_Icon_062"}, 
        { ResourceType.Oil,"UI_Icon_068"},  
    };
    public static Dictionary<int, string> RANK_ORDER_ICON_DICTIONARY = new Dictionary<int, string>()
    {
        {1,"UI_Icon_156"},
        {2,"UI_Icon_157"},
        {3,"UI_Icon_158"}
    };
    public static Dictionary<PropsQuality, string> PROPS_QUALITY_COLOR = new Dictionary<PropsQuality, string>()
    {
        {PropsQuality.Excellent,"[4be753]"},
	    {PropsQuality.Sophisticated,"[5591ff]"},
	    {PropsQuality.Epic,"[c946ff]"},
	    {PropsQuality.Legend,"[ff9c31]"}
    };
    public static Dictionary<PropsQuality, string> PROPS_QUALITY= new Dictionary<PropsQuality, string>()
    {
        {PropsQuality.Excellent,"品质：优秀"},
	    {PropsQuality.Sophisticated,"品质：精良"},
	    {PropsQuality.Epic,"品质：史诗"},
	    {PropsQuality.Legend,"品质：传说"}
    };
    public static Dictionary<PropsQuality, string> PROPS_QUALITY_WIN_NAME = new Dictionary<PropsQuality, string>()
    {
        {PropsQuality.Excellent,"UI_BackGround_108"},
	    {PropsQuality.Sophisticated,"UI_BackGround_107"},
	    {PropsQuality.Epic,"UI_BackGround_106"},
	    {PropsQuality.Legend,"UI_BackGround_105"}
    };
    public static Dictionary<int, string> PROPS_QUALITY_BG_NAME = new Dictionary<int, string>()
    {
        {-1,"Props_BackGround_011"},
	    {0,"Props_BackGround_007"},
	    {1,"Props_BackGround_008"},
	    {2,"Props_BackGround_009"},
        {3,"Props_BackGround_010"}
    };
    public static Dictionary<PropsCategory, string> PROPS_CATEGORY = new Dictionary<PropsCategory, string>()
    {
        {PropsCategory.Attack,"类型：攻击"},
	    {PropsCategory.Defense,"类型：防御"},
	    {PropsCategory.Auxiliary,"类型：辅助"},
	    {PropsCategory.Special,"类型：特殊"}
    };
    public static string PROPS_DESCRIPTION = "功能描述：\n";

    public static Dictionary<Age, string> AGE_SCENE_MUSIC = new Dictionary<Age, string>() 
    {
        {Age.Prehistoric,"BuildingScene01"},
        {Age.Stone,"BuildingScene02"},
        {Age.Bronze,"BuildingScene03"},
        {Age.Iron,"BuildingScene04"}
    };
}
