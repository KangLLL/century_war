using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
using ConfigUtilities;

public class AchievementBuildingData  
{
	public AchievementBuildingType AchievementBuildingType { get;set; }
	public int BuildingNo { get;set; }
	public TilePosition BuildingPosition { get;set; }
	public int Life { get;set; }
	public AchievementBuildingConfigData ConfigData { get;set; }
}
