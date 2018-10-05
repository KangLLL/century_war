using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;
using CommandConsts;

public class AchievementBuildingModule  
{
	private int m_StartNo;
	private Dictionary<int, AchievementBuildingLogicObject> m_Buildings;
	
	public void InitialAchievementBuilding(List<AchievementBuildingData> buildings, int startNo)
	{
		this.m_StartNo = startNo;
		this.m_Buildings = new Dictionary<int, AchievementBuildingLogicObject>();
		
		foreach (AchievementBuildingData building in buildings) 
		{
			AchievementBuildingLogicObject achievementBuilding = new AchievementBuildingLogicObject(building);
			this.m_Buildings.Add(building.BuildingNo, achievementBuilding);
		}
	}
	
	public AchievementBuildingLogicData BuildAchievementBuilding(AchievementBuildingType type, TilePosition position)
	{
		AchievementBuildingData data = new AchievementBuildingData();
		data.AchievementBuildingType = type;
		data.BuildingNo = ++this.m_StartNo;
		data.BuildingPosition = position;
		data.ConfigData = ConfigInterface.Instance.AchievementBuildingConfigHelper.GetAchievementBuildingData(type);
		data.Life = data.ConfigData.NeedPropsNumber;
		
		AchievementBuildingLogicObject newObject = new AchievementBuildingLogicObject(data);
		this.m_Buildings.Add(data.BuildingNo, newObject);
		return newObject.Data;
	}
	
	public void RepairAchievementBuilding(int buildingNo, int recoverLife)
	{
		this.m_Buildings[buildingNo].Repair(recoverLife);
	}
	
	public void DestroyAchievementBuilding(int buildingNo)
	{
		this.m_Buildings.Remove(buildingNo);
		
		DestroyAchievementBuildingRequestParameter request = new DestroyAchievementBuildingRequestParameter();
		request.AchievementBuildingNo = buildingNo;
		
		CommunicationUtility.Instance.DestroyAchievementBuilding(request);
	}
	
	public void MoveAchievementBuilding(int buildingNo, TilePosition newPosition)
	{
		this.m_Buildings[buildingNo].Move(newPosition);
	}
	
	public List<AchievementBuildingLogicData> AllAchievementBuildings
	{
		get
		{
			List<AchievementBuildingLogicData> result = new List<AchievementBuildingLogicData>();
			foreach (KeyValuePair<int, AchievementBuildingLogicObject> building in m_Buildings) 
			{
				result.Add(building.Value.Data);
			}
			return result;
		}
	}
	
	public AchievementBuildingLogicObject GetAchievementBuilding(int buildingNo)
	{
		return this.m_Buildings[buildingNo];
	}
}
