using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ConfigUtilities.Enums;
using ConfigUtilities;
using CommandConsts;

public class RemovableObjectModule
{
	private Dictionary<int, RemovableObjectLogicObject> m_ObjectDict;
	private BuilderManager m_BuilderManager;
	private int m_RemovableObjectStartNo;
	
	public RemovableObjectModule(BuilderManager builderManager)
	{
		this.m_ObjectDict = new Dictionary<int, RemovableObjectLogicObject>();
		this.m_BuilderManager = builderManager;
	}
	
	public bool GeneratePosition(int removableObjectNo, IMapData mapData)
	{
		RemovableObjectLogicObject logicObject = this.GetRemovableObject(removableObjectNo);
		
		List<TilePosition> validPosition = new List<TilePosition>();
		
		for(int r = 0; r < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height; r ++)
		{
			for(int c = 0; c < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width; c ++)
			{
				bool isValid = true;
				TilePosition position = new TilePosition(c, r);
				foreach(TilePosition offset in logicObject.LogicData.BuildingObstacleList)
				{
					TilePosition p = position + offset;
					if(!p.IsValidBuildingTilePosition() || mapData.GetBuildingObjectFromBuildingObstacleMap(p.Row, p.Column) != null)
					{
						isValid = false;
						break;
					}
					if(LogicController.Instance.IsNewPlayer && 
						ClientSystemConstants.INITIAL_REMOVABLE_OBJECT_INVALID_RECT.Contains(
						new Vector2(position.Column, position.Row)))
					{
						isValid = false;
						break;
					}
				}
				if(isValid)
				{
					validPosition.Add(position);
				}
			}
		}
		
		if(validPosition.Count == 0)
		{
			this.m_ObjectDict.Remove(removableObjectNo);
			return false;
		}
		else
		{
			TilePosition position = validPosition[CommonHelper.GetRandomNumber(0, validPosition.Count)];
			logicObject.GeneratePosition(position);
			return true;
		}
	}
	
	public void InitialWithData(List<RemovableObjectData> datas, int removableObjectStartNo)
	{
		foreach (RemovableObjectData data in datas) 
		{
			RemovableObjectLogicObject removable = new RemovableObjectLogicObject(data);
			removable.RemoveTimeUp += ObjectRemoveTimeUp;
			this.m_ObjectDict.Add(data.RemovableObjectNo, removable);
			if(data.BuilderBuildingNO.HasValue)
			{
				this.m_BuilderManager.AddBusyBuilder(data.BuilderBuildingNO.Value, removable.LogicData);
			}
		}
		this.m_RemovableObjectStartNo = removableObjectStartNo;
	}
	
	public void Process()
	{
		foreach (KeyValuePair<int, RemovableObjectLogicObject> item in this.m_ObjectDict) 
		{
			item.Value.Process();
		}
	}
	
	private void ObjectRemoveTimeUp(int removableObjectNo)
	{
		this.m_BuilderManager.RecycleBuilder(this.m_ObjectDict[removableObjectNo].LogicData.CurrentAttachedBuilderNO);
	}
	
	public void FinishRemove(int removableObjectNo)
	{
		this.GetRemovableObject(removableObjectNo).FinishRemove();
		this.m_ObjectDict.Remove(removableObjectNo);
	}
	
	public RemovableObjectLogicData GetRemovableObjectData(int removableObjectNo)
	{
		return this.m_ObjectDict[removableObjectNo].LogicData;
	}
	
	public RemovableObjectLogicObject GetRemovableObject(int removableObjectNo)
	{
		return this.m_ObjectDict[removableObjectNo];
	}
	
	public RemovableObjectLogicObject BuyRemovableObject(RemovableObjectType type, TilePosition position, int propsNo, ref bool isRewardProps)
	{
		RemovableObjectData data = new RemovableObjectData();
		data.Position = position;
		data.RemovableObjectNo = ++this.m_RemovableObjectStartNo;
		data.RemovableObjectType = type;
		data.ConfigData = ConfigInterface.Instance.RemovableConfigHelper.GetRemovableObjectData(type);
		
		RemovableObjectLogicObject result = new RemovableObjectLogicObject(data);
		isRewardProps = result.GenerateRewardData(propsNo);
		this.m_ObjectDict.Add(data.RemovableObjectNo, result);
		result.RemoveTimeUp += ObjectRemoveTimeUp;
		
		BuyRemovableObjectRequestParameter request = new BuyRemovableObjectRequestParameter();
		request.PositionColumn = data.Position.Column;
		request.PositionRow = data.Position.Row;
		request.RemovableObjectType = type;
		request.RewardExp = data.RewardExp;
		request.RewardGem = data.RewardGem;
		request.RewardPropsType = data.RewardPropsType;
		CommunicationUtility.Instance.BuyRemovableObject(request);
		return result;
	}
	
	public List<RemovableObjectLogicData> AllObjects
	{
		get
		{
			List<RemovableObjectLogicData> result = new List<RemovableObjectLogicData>();
			foreach (KeyValuePair<int, RemovableObjectLogicObject> item in m_ObjectDict)
			{
				result.Add(item.Value.LogicData);
			}
			return result;
		}
	}
	
	
}
