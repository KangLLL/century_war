using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities;
using ConfigUtilities.Enums;
using ConfigUtilities.Structs;

public class AchievementBuildingFunction : EditorCommonBehavior<AchievementBuildingType, AchievementBuildingConfigData>
{
	protected override void Construct (AchievementBuildingType type, TilePosition position)
	{
		EditorFactory.Instance.ConstructAchievementBuilding(type, position);
	}
	
	public override void Delete ()
	{
		EditorFactory.Instance.DestroyAchievementBuilding(this.Position, this.ConfigData);
	}
	
	protected override List<TilePosition> GetBuildingObstacleInfo (AchievementBuildingType type)
	{
		List<TilePosition> result = new List<TilePosition>();
		foreach (TilePoint tp in ConfigInterface.Instance.AchievementBuildingConfigHelper.GetAchievementBuildingData(type).BuildingObstacleList) 
		{
			result.Add(tp.ConvertToTilePosition());
		}
		return result;
	}
	
	protected override AchievementBuildingConfigData GetConfigData ()
	{
		return ConfigInterface.Instance.AchievementBuildingConfigHelper.GetAchievementBuildingData(this.ObjectType);
	}
	
	protected override int GetIndexFromType (AchievementBuildingType type)
	{
		return (int)type;
	}
	
	protected override AchievementBuildingType GetTypeFromIndex (int index)
	{
		return (AchievementBuildingType)index;
	}
	
	protected override AchievementBuildingType StartType 
	{
		get 
		{
			return AchievementBuildingType.AncientTotem;
		}
	}
}
