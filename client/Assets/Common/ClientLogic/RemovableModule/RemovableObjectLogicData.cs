using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Structs;
using ConfigUtilities.Enums;
using ConfigUtilities;
using System;

public class RemovableObjectLogicData  : IRemovableObjectInfo
{
	private RemovableObjectData m_Data;
	
	public RemovableObjectLogicData(RemovableObjectData data)
	{
		this.m_Data = data;
	}
	
	public int RemovableObjectNo { get { return this.m_Data.RemovableObjectNo; } }
	
	private List<TilePosition> m_BuildingObstacleList;
	public List<TilePosition> BuildingObstacleList
	{
		get
		{
			if(this.m_BuildingObstacleList == null)
			{
				this.m_BuildingObstacleList = new List<TilePosition>();
				foreach (TilePoint point in this.m_Data.ConfigData.BuildingObstacleList)
				{
					this.m_BuildingObstacleList.Add(point.ConvertToTilePosition());
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
				foreach (TilePoint point in this.m_Data.ConfigData.ActorObstacleList) 
				{
					this.m_ActorObstacleList.Add(point.ConvertToTilePosition());
				}
			}
			return this.m_ActorObstacleList;
		}
	}
	
	public string Name { get { return this.m_Data.ConfigData.Name; } }
	
	public int GoldCost { get { return this.m_Data.ConfigData.GoldCost; } }
	public int FoodCost { get { return this.m_Data.ConfigData.FoodCost; } }
	public int OilCost { get { return this.m_Data.ConfigData.OilCost; } }
	public int GemCost { get { return this.m_Data.ConfigData.GemCost; } }
	public string PrefabName { get { return this.m_Data.ConfigData.PrefabName; } }
	
	public int RemoveWorkload { get { return this.m_Data.ConfigData.RemoveWorkload; } }
	
	public TilePosition BuildingPosition { get { return this.m_Data.Position; } }
	public TilePosition ActorPosition
	{
		get
		{
			return PositionConvertor.GetActorTilePositionFromBuildingTilePosition(this.m_Data.Position);
		}
	}
	
	public float AttachedBuilderEfficiency
	{
		get	
		{
			if(this.m_Data.BuilderBuildingNO.HasValue)
			{
				return 1;
				/*
				BuildingIdentity id = new BuildingIdentity(BuildingType.BuilderHut, this.m_Data.BuilderBuildingNO.Value);
				return ConfigInterface.Instance.BuilderConfigHelper.GetBuilderData(
					LogicController.Instance.GetBuildingObject(id).Level).BuildEfficiency;
				*/
			}
			return -1;
		}
	}
	
	public int CurrentAttachedBuilderNO
	{
		get
		{
			return this.m_Data.BuilderBuildingNO.HasValue ? this.m_Data.BuilderBuildingNO.Value : -1;
		}
	}
	
	public float RemoveRemainingWorkload
	{
		get
		{
			return this.m_Data.RemainingWorkload.HasValue ? this.m_Data.RemainingWorkload.Value : 0;
		}
	}

	
	public RemovableObjectType ObjectType 
	{
		get 
		{
			return this.m_Data.RemovableObjectType;
		}
	}
	
	public RemovableObjectEditorState EditorState
	{
		get
		{
			if(this.m_Data.BuilderBuildingNO.HasValue)
			{
				return RemovableObjectEditorState.Removing;
			}
			if(this.m_Data.RemainingWorkload.HasValue)
			{
				return RemovableObjectEditorState.ReadyForComplete;
			}
			return RemovableObjectEditorState.Normal;
		}
	}
	
	public int RewardExp { get { return this.m_Data.RewardExp; } }
	public int RewardGem { get { return this.m_Data.RewardGem; } }
	public Nullable<int> RewardProps { get { return this.m_Data.RewardProps; } }
	public Nullable<PropsType> RewardPropsType { get { return this.m_Data.RewardPropsType; }}
	
	public bool IsCountable { get { return ConfigInterface.Instance.RemovableConfigHelper.GetCountableRemovableObjects().Contains(this.ObjectType); } }		
}
