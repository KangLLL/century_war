using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities.Structs;

public class AchievementBuildingLogicData : IAchievementBuildingInfo  
{
	private AchievementBuildingData m_Data;
	
	public AchievementBuildingLogicData(AchievementBuildingData data)
	{
		this.m_Data = data;
	}
	
	public int Life { get { return this.m_Data.Life; } }
	public int BuildingNo { get { return this.m_Data.BuildingNo; } }
	
	public int MaxLife { get { return this.m_Data.ConfigData.NeedPropsNumber; } }
	public int MaxHP { get { return this.m_Data.ConfigData.MaxHP; } }
	public string Name { get { return this.m_Data.ConfigData.Name; } }
	public string Description { get { return this.m_Data.ConfigData.Description; } }
	public string PrefabName { get { return this.m_Data.ConfigData.PrefabName; } }
	public PropsType NeedProps { get { return this.m_Data.ConfigData.NeedPropsType; } }
	
	#region IAchievementBuildingInfo implementation
	public AchievementBuildingType AchievementBuildingType 
	{
		get 
		{
			return this.m_Data.AchievementBuildingType;
		}
	}
	#endregion

	#region IObstacleInfo implementation
	public TilePosition BuildingPosition 
	{
		get 
		{
			return this.m_Data.BuildingPosition;
		}
	}

	public TilePosition ActorPosition
	{
		get 
		{	
			return PositionConvertor.GetActorTilePositionFromBuildingTilePosition(this.m_Data.BuildingPosition);
		}
	}
	
	private List<TilePosition> m_BuildingObstacleList;
	public List<TilePosition> BuildingObstacleList 
	{
		get 
		{
			if(this.m_BuildingObstacleList == null)
			{
				this.m_BuildingObstacleList = new List<TilePosition>();
				foreach (TilePoint tp in this.m_Data.ConfigData.BuildingObstacleList) 
				{
					this.m_BuildingObstacleList.Add(tp.ConvertToTilePosition());
				}
			}
			return this.m_BuildingObstacleList;
		}
	}
	
	private List<TilePosition> m_ActorObstacleList;
	public List<TilePosition> ActorObstacleList 
	{
		get 
		{
			if(this.m_ActorObstacleList == null)
			{
				this.m_ActorObstacleList = new List<TilePosition>();
				foreach (TilePoint tp in this.m_Data.ConfigData.ActorObstacleList) 
				{
					this.m_ActorObstacleList.Add(tp.ConvertToTilePosition());
				}
			}
			return this.m_ActorObstacleList;
		}
	}
	#endregion
}
