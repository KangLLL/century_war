using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceManager
{
	protected Dictionary<ResourceType, List<BuildingIdentity>> m_StorageDict;
	protected Dictionary<ResourceType, Dictionary<BuildingIdentity, int>> m_CapacityDict;
	protected Dictionary<ResourceType, int> m_MaxCapacity;
	
	public ResourceManager()
	{
		this.m_StorageDict = new Dictionary<ResourceType, List<BuildingIdentity>>();
		this.m_CapacityDict = new Dictionary<ResourceType, Dictionary<BuildingIdentity, int>>();
		this.m_MaxCapacity = new Dictionary<ResourceType, int>(){
			{ResourceType.Gold, 0},
			{ResourceType.Food, 0},
			{ResourceType.Oil, 0}
		};
	}
	
	public void AddStorage(ResourceType resourceType, BuildingIdentity identity, int capacity)
	{
		if(!this.m_StorageDict.ContainsKey(resourceType))
		{
			this.m_StorageDict.Add(resourceType, new List<BuildingIdentity>());
		} 
		this.m_StorageDict[resourceType].Add(identity);
		
		if(!this.m_CapacityDict.ContainsKey(resourceType))
		{
			this.m_CapacityDict.Add(resourceType, new Dictionary<BuildingIdentity, int>());
		}
		this.m_CapacityDict[resourceType].Add(identity, capacity);
		
		this.m_MaxCapacity[resourceType] += capacity;
	}
	
	public Dictionary<ResourceType, Dictionary<BuildingIdentity, int>> CalculateStorage(int totalGold, int totalFood, int totalOil)
	{
		Dictionary<ResourceType, Dictionary<BuildingIdentity, int>> result = new Dictionary<ResourceType, Dictionary<BuildingIdentity, int>>();
		Dictionary<ResourceType, int> total = new Dictionary<ResourceType, int>(){
			{ResourceType.Gold, totalGold},
			{ResourceType.Food, totalFood},
			{ResourceType.Oil, totalOil}
		};
		
		foreach (KeyValuePair<ResourceType, List<BuildingIdentity>> storage in this.m_StorageDict) 
		{
			result.Add(storage.Key, new Dictionary<BuildingIdentity, int>());
			int sumValue = 0;
			for(int i =0; i < storage.Value.Count; i++)
			{
				BuildingIdentity id = storage.Value[i];
				int newValue = 0;
				if(i != storage.Value.Count - 1)
				{
					int capacity = this.m_CapacityDict[storage.Key][id];
					float percentage = ((float)capacity) / this.m_MaxCapacity[storage.Key];
					newValue = Mathf.FloorToInt(total[storage.Key] * percentage);
					sumValue += newValue;
				}
				else
				{
					newValue = total[storage.Key] - sumValue;
				}
				result[storage.Key][id] = newValue;
			}
		}
		
		return result;
	}
}
