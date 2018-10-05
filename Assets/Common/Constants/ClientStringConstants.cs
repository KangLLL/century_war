using UnityEngine;
using System.Collections;

public class ClientStringConstants
{
	public const string ND_START_SCENE_LEVEL_NAME = "NdStart";
	public const string INITIAL_SCENE_LEVEL_NAME = "Initialize";
	public const string LOADING_SCENE_LEVEL_NAME = "Loading";
	public const string BUILDING_SCENE_LEVEL_NAME = "SceneBuild";
	public const string BATTLE_SCENE_LEVEL_NAME = "Battle";
	public const string BATTLE_REPLAY_LEVEL_NAME = "Replay";
	public const string VISIT_SCENE_LEVEL_NAME = "SceneVisit";
    public const string CG_SCENE_LEVEL_NAME = "CG";
	
	public const string JSON_PRODUCT_ID_KEY = "ProductID";
	public const string JSON_PRODUCT_LOCALE_TITLE = "ProductLocaleTitle";
	public const string JSON_PRODUCT_LOCALE_DESCRIPTION = "ProductLocaleDesciption";
	public const string JSON_PRODUCT_LOCALE_CONCURRENCY_SYMBOL = "ProductLocaleConcurrencySymbol";
	public const string JSON_PRODUCT_LOCALE_PRICE = "ProductLocalePrice";
	
	public const string BATTLE_WIN_TITLE = "胜利";
	public const string BATTLE_LOSE_TITLE = "失败";
	
	public const string BUILDING_ANCHOR_OBJECT_NAME = "BuildingBackgroundAnchor";
	public const string ANCHOR_OBJECT_NAME = "BackgroundAnchor";
	public const string RUINS_PARENT_OBJECT_NAME = "Ruins";
	public const string UI_ROOT_OBJECT_NAME = "UI Root (2D)";
	public const string UI_ANCHOR_OBJECT_NAME = "Anchor";
	
	public const string ND_START_OBJECT_NAME = "NdStart";
	
	public const string UI_NEXT_RIVAL_OBJECT_NAME = "NextButton";
	public const string CLOUD_OBJECT_NAME = "Cloud";
	
	public const string REPLAY_SPEED_1_TITLE = "X1";
	public const string REPLAY_SPEED_2_TITLE = "X2";
	public const string REPLAY_SPEED_4_TITLE = "X4";
	
	public const string FULL_FILL_STAR_SPRITE_NAME = "UI_Icon_030";
	public const string EMPTY_STAR_SPRITE_NAME = "UI_Icon_031";
	
	public const string SUMMARY_SCREEN_FULL_FILL_SMALL_STAR_SPRITE_NAME = "UI_Icon_137";
	public const string SUMMARY_SCREEN_FULL_FILL_BIG_STAR_SPRITE_NAME = "UI_Icon_138";
	
	public const string BUILDING_SCENE_RESOURCE_PREFAB_PREFIX_NAME = "BuildingScene/";
	public const string BATTLE_SCENE_RESOURCE_PREFAB_PREFIX_NAME = "BattleScene/";
	public const string REMOVABLE_OBJECT_PREFAB_PREFIX_NAME = "RemovableObjects/";
	public const string BUILDING_OBJECT_PREFAB_PREFIX_NAME = "Buildings/";
	public const string ACHIEVEMENT_BULIDING_PREFAB_PREFIX_NAME = "AchievementBuildings/";
	public const string ARMY_OBJECT_PREFAB_PREFIX_NAME = "Armies/";
	public const string FACILITIES_OBJECT_PREFAB_PREFIX_NAME = "Facilities/";
	public const string ACTOR_OBJECT_PREFAB_PREFIX_NAME = "Actors/";
	public const string MERCENARY_OBJECT_PREFAB_PREFIX_NAME = "Mercenaries/";
	public const string DEFENSE_OBJECT_PREFAB_PREFIX_NAME = "DefenseObject/";
	public const string ATTACK_PROPS_PREFAB_PREFIX_NAME = "AttackProps/";
	
	
	public const string NO_ARMY_TO_DROP_WARNING_MESSAGE = "当前士兵已用完";
	public const string NO_PROPS_TO_DROP_WARNING_MESSAGE = "当前道具已用完";
	public const string CAN_NOT_DROP_ARMY_WARNING_MESSAGE = "当前区域无法放置士兵";
	public const string CAN_NOT_USE_PROPS_WARNING_MESSAGE = "当前区域无法使用道具";
	public const string NO_ENOUGH_GOLD_WARNING_MESSAGE = "金币不足";
	
	public const string TASK_PROGRESS_DESCRIPTION_TITLE = "完成进度:";
	
	public const string LOG_OUT_DIALOG_TITLE = "注销";
	public const string LOG_OUT_DIALOG_DESCRIPTION = "帐号已注销，请重新运行游戏！";
	public const string LOG_OUT_DIALOG_OK_BUTTON_TITLE = "确认";

    public const string VERSION_ERROR_TITLE = "版本更新";
    public const string VERSION_ERROR_DESCRIPTION = "请您将游戏更新到最新版本！";
    public const string VERSION_ERROR_OK_BUTTON_TITLE = "立即更新";
	
	public const string PURCHASE_SUCCESSFUL_TIPS = "购买已成功";
	public const string PURCHASE_FAIL_TIPS = "购买已取消";
	public const string REQUEST_FAIL_TIPS = "苹果商店现在不可用";
	
	public const string TIPS_TITLE = "小提示：";

	public const string ARMY_UPGRADE_TIPS = "({0})升级到{1}级";
	public const string TASK_COMPLETE_TIPS = "{0}已完成";

	public const string DISCONNECT_DIALOG_NORMAL_TIPS = "首领，您的网络不太稳定，请点确定重新登录。";
	public const string DISCONNECT_DIALOG_FROM_BACKGROUND_TIPS = "首领，为了节省您的流量，一段时间内不操作，系统会自动断线！";
	public const string DISCONNECT_DIALOG_CAN_NOT_CONNECT_TIPS = "暂时不能连上服务器，请稍后再试！";
}
