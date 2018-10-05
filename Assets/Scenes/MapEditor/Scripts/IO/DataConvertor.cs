using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommandConsts;
using ConfigUtilities.Enums;
using ConfigUtilities;
using System;

public class DataConvertor 
{
	public static FindRivalResponseParameter ConvertJSONToParameter(Hashtable map)
	{
		FindRivalResponseParameter result = new FindRivalResponseParameter();
		result.Objects = new List<BattleRemovableObjectParameter>();
		result.Buildings = new List<BattleBuildingParameter>();
		result.AchievementBuildings = new List<BattleAchievementBuildingParameter>();
		result.DefenseObjects = new List<BattleDefenseObjectParameter>();
		result.RivalHonour = 10000;
		result.RivalID = -100;
		result.RivalLevel  = -100;
		result.RivalName = "电脑";
		result.TotalFood = 0;
		result.TotalGold = 0;
		result.TotalOil = 0;
		
		if(map.ContainsKey(EditorConfigInterface.Instance.MapBuildingKey))
		{
			var buildings = (ArrayList)map[EditorConfigInterface.Instance.MapBuildingKey];
			foreach (var b in buildings) 
			{
				Hashtable building = (Hashtable)b;
				BattleBuildingParameter param = new BattleBuildingParameter();
				
				param.BuildingNO = Convert.ToInt32(building[EditorConfigInterface.Instance.MapBuildingNoKey]);
				param.BuildingType = (BuildingType)(Convert.ToInt32(building[EditorConfigInterface.Instance.MapBuildingTypeKey]));
				param.IsUpgrading = false;
				param.Level = Convert.ToInt32(building[EditorConfigInterface.Instance.MapBuildingLevelKey]);
				param.PositionColumn = Convert.ToInt32(building[EditorConfigInterface.Instance.MapBuildingColumnKey]);
				param.PositionRow = Convert.ToInt32(building[EditorConfigInterface.Instance.MapBuildingRowKey]);
				
				BuildingConfigData configData = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(param.BuildingType,param.Level);
				if(configData.StoreGoldCapacity > 0)
				{
					if(configData.CanProduceGold)
					{
						param.CurrentGold = configData.StoreGoldCapacity;
					}
					else
					{
						result.TotalGold += configData.StoreGoldCapacity;
					}
				}
				if(configData.StoreFoodCapacity > 0)
				{
					if(configData.CanProduceFood)
					{
						param.CurrentFood = configData.StoreFoodCapacity;
					}
					else
					{
						result.TotalFood += configData.StoreFoodCapacity;
					}
				}
				if(configData.StoreOilCapacity > 0)
				{
					if(configData.CanProduceOil)
					{
						param.CurrentOil = configData.StoreOilCapacity;
					}
					else
					{
						result.TotalOil += configData.StoreOilCapacity;
					}
				}
				result.Buildings.Add(param);
			}
		}
		
		if(map.ContainsKey(EditorConfigInterface.Instance.MapRemovableObjectKey))
		{
			var objects = (ArrayList)map[EditorConfigInterface.Instance.MapRemovableObjectKey];
			foreach (var o in objects) 
			{
				Hashtable removableObject = (Hashtable)o;
				BattleRemovableObjectParameter param = new BattleRemovableObjectParameter();
				
				param.ObjectType = (RemovableObjectType)(Convert.ToInt32(removableObject[EditorConfigInterface.Instance.MapRemovableObjectTypeKey]));
				param.PositionRow = Convert.ToInt32(removableObject[EditorConfigInterface.Instance.MapBuildingRowKey]);
				param.PositionColumn = Convert.ToInt32(removableObject[EditorConfigInterface.Instance.MapBuildingColumnKey]);
				
				result.Objects.Add(param);
			}
		}
		
		if(map.ContainsKey(EditorConfigInterface.Instance.MapAchievementBuildingKey))
		{
			var achievementBuildings = (ArrayList)map[EditorConfigInterface.Instance.MapAchievementBuildingKey];
			int buildingNo = 0;
			foreach (var a in achievementBuildings) 
			{
				Hashtable achievementBuilding = (Hashtable)a;
				BattleAchievementBuildingParameter param = new BattleAchievementBuildingParameter();
				
				param.AchievementBuildingNo = ++ buildingNo;
				param.AchievementBuildingType = (AchievementBuildingType)(Convert.ToInt32(achievementBuilding[EditorConfigInterface.Instance.MapAchievementBuildingTypeKey]));
				param.IsDropProps = false;
				param.PositionRow = Convert.ToInt32(achievementBuilding[EditorConfigInterface.Instance.MapAchievementBuildingRowKey]);
				param.PositionColumn = Convert.ToInt32(achievementBuilding[EditorConfigInterface.Instance.MapAchievementBuildingColumnKey]);
				
				result.AchievementBuildings.Add(param);
			}
		}
		
		if(map.ContainsKey(EditorConfigInterface.Instance.MapDefenseObjectKey))
		{
			var defenseObjects = (ArrayList)map[EditorConfigInterface.Instance.MapDefenseObjectKey];
			long objectID = 0;
			foreach (var d in defenseObjects) 
			{
				Hashtable defenseObject = (Hashtable)d;
				BattleDefenseObjectParameter param = new BattleDefenseObjectParameter();
				
				param.DefenseObjectID = objectID ++;
				param.PropsType = (PropsType)(Convert.ToInt32(defenseObject[EditorConfigInterface.Instance.MapDefenseObjectTypeKey]));
				param.PositionRow = Convert.ToInt32(defenseObject[EditorConfigInterface.Instance.MapDefenseObjectRowKey]);
				param.PositionColumn = Convert.ToInt32(defenseObject[EditorConfigInterface.Instance.MapDefenseObjectColumnKey]);
				
				result.DefenseObjects.Add(param);
			}
		}
		
		return result;
	}
}
