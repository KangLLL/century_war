using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities;
using ConfigUtilities.Enums;

public class TaskProgressFactory  
{
	public static void PopulateTaskInformation(TaskInformation information)
	{
		if(information.Status == TaskStatus.Completed)	
		{
			return;
		}
		else
		{
			TaskConfigData taskData = ConfigInterface.Instance.TaskConfigHelper.GetTaskData(information.TaskID);
		
			foreach(KeyValuePair<int, TaskConditionConfigData> condition in taskData.Conditions)
			{
				if(information.ConditionProgresses == null)
				{
					information.ConditionProgresses = new Dictionary<int, TaskProgressInformation>();
				}
				if(!information.ConditionProgresses.ContainsKey(condition.Key))
				{
					information.ConditionProgresses.Add(condition.Key, new TaskProgressInformation(){
						StartValue = 0, CurrentValue = GetCurrentValueFromConfig(condition.Value)});
				}
				else
				{
					information.ConditionProgresses[condition.Key].CurrentValue = GetCurrentValueFromConfig(condition.Value);
				}
			}
		}
	}
	
	public static int GetCurrentValueFromConfig(TaskConditionConfigData configData)
	{
		TaskConditionType conditionType = configData.ConditionType;
		
		switch(conditionType)
		{
			case TaskConditionType.ConstructBuildingCondition:
			{
				BuildingType type = (BuildingType)configData.Value1;
				int result = 0;
				List<BuildingLogicData> buildings = LogicController.Instance.GetBuildings(type);
				foreach(BuildingLogicData building in buildings)
				{
					if(building.Level != 0)
					{
						result ++;
					}
				}
				return result;	
			}
			case TaskConditionType.DestroyBuildingCondition:
			{
				BuildingType type = (BuildingType)configData.Value1;
				return LogicController.Instance.PlayerData.GetDestroyBuilding(type);
			}
			case TaskConditionType.HonourCondition:
			{
				return LogicController.Instance.PlayerData.Honour;
			}
			case TaskConditionType.PlunderCondition:
			{
				if((int)configData.Value1 == 0)
				{
					return LogicController.Instance.PlayerData.PlunderTotalGold;
				}
				else if((int)configData.Value1 == 1)
				{
					return LogicController.Instance.PlayerData.PlunderTotalFood;
				}
				else
				{
					return LogicController.Instance.PlayerData.PlunderTotalOil;
				}
			}
			case TaskConditionType.ProduceArmyCondition:
			{
				ArmyType armyType = (ArmyType)configData.Value1;
				return LogicController.Instance.PlayerData.GetProduceArmyCount(armyType);
			}
			case TaskConditionType.RemoveObjectCondition:
			{
				return LogicController.Instance.PlayerData.RemoveTotalObject;
			}
			case TaskConditionType.UpgradeArmyCondition:
			{
				ArmyType armyType = (ArmyType)configData.Value1;
				return LogicController.Instance.PlayerData.GetArmyLevel(armyType);
			}
			case TaskConditionType.UpgradeBuildingCondition:
			{
				BuildingType buildingType = (BuildingType)configData.Value1;
				List<BuildingLogicData> buildings = LogicController.Instance.GetBuildings(buildingType);
				int result = 0;
				foreach (BuildingLogicData building in buildings) 
				{
					if(building.Level > result)
					{
						result = building.Level;
					}	
				}
				return result;
			}
			case TaskConditionType.HireMercenaryCondition:
			{
				MercenaryType mercenaryType = (MercenaryType)configData.Value1;
				return LogicController.Instance.PlayerData.GetMercenaryStartNO(mercenaryType);
			}
			default:
				return 0;
		}
	}
}
