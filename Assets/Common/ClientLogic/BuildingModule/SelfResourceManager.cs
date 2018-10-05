using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelfResourceManager : ResourceManager 
{
	private BuildingModule m_BuildingModule;
	
	public SelfResourceManager(BuildingModule module)
	{
		this.m_BuildingModule = module;
	}
	
	public void RecalculateStorage()
	{
		this.m_MaxCapacity = new Dictionary<ResourceType, int>()
		{
			{ResourceType.Gold, LogicController.Instance.PlayerData.GoldMaxCapacity},
			{ResourceType.Food, LogicController.Instance.PlayerData.FoodMaxCapacity},
			{ResourceType.Oil, LogicController.Instance.PlayerData.OilMaxCapacity}
		};
		
		this.m_CapacityDict = new Dictionary<ResourceType, Dictionary<BuildingIdentity, int>>()
		{
			{ResourceType.Gold, new Dictionary<BuildingIdentity, int>()},
			{ResourceType.Food, new Dictionary<BuildingIdentity, int>()},
			{ResourceType.Oil, new Dictionary<BuildingIdentity, int>()}
		};
		
		foreach (KeyValuePair<ResourceType, List<BuildingIdentity>> storage in this.m_StorageDict)
		{
			foreach(BuildingIdentity id in storage.Value)
			{
				BuildingLogicData building = this.m_BuildingModule.GetBuildingObject(id);
				int capacity = storage.Key == ResourceType.Gold ? building.StoreGoldCapacity :
					storage.Key == ResourceType.Food ? building.StoreFoodCapacity : building.StoreOilCapacity;
				this.m_CapacityDict[storage.Key][id] = capacity;
			}
		}
		
		Dictionary<ResourceType, Dictionary<BuildingIdentity, int>> result = this.CalculateStorage(LogicController.Instance.PlayerData.CurrentStoreGold,
			LogicController.Instance.PlayerData.CurrentStoreFood, LogicController.Instance.PlayerData.CurrentStoreOil);
		
		foreach (KeyValuePair<ResourceType, Dictionary<BuildingIdentity, int>> storage in result) 
		{
			foreach(KeyValuePair<BuildingIdentity, int> calculatedStorage in storage.Value)
			{
				BuildingLogicData building = this.m_BuildingModule.GetBuildingObject(calculatedStorage.Key);
				
				int? newGold = null;
				if(storage.Key == ResourceType.Gold)
				{
					newGold = calculatedStorage.Value;
				}
				int? newFood = null;
				if(storage.Key == ResourceType.Food)
				{
					newFood = calculatedStorage.Value;
				}
				int? newOil = null;
				if(storage.Key == ResourceType.Oil)
				{
					newOil = calculatedStorage.Value;
				}
				
				this.m_BuildingModule.ModifyResourceStoreage(building.BuildingIdentity, newGold, newFood, newOil);
			}
		}
	}
}
