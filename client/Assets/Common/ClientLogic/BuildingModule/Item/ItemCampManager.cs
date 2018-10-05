using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class ItemCampManager : CampManager<ItemType, ItemIdentity>
{
	protected override int GetAlreadyCapacity (BuildingLogicData building)
	{
		return building.AlreadyItemCapacity;
	}
	
	protected override int GetTotalCapacity (BuildingLogicData building)
	{
		return building.StoreItemCapacity;
	}
	
	protected override int GetCapacityCost (ItemType type)
	{
		//return ConfigUtilities.ConfigInterface.Instance.ItemConfigHelper.GetItemCapacityCost(type);
		return 1;
	}
	
	public override Dictionary<ItemType, List<ItemIdentity>> AvailableObjects 
	{
		get 
		{
			Dictionary<ItemType, List<ItemIdentity>> result = new Dictionary<ItemType, List<ItemIdentity>>();
			
			for(int i = 0; i < (int)ItemType.Length; i ++)
			{
				ItemType itemType = (ItemType)i;
				result.Add(itemType, new List<ItemIdentity>());
			}
			
			foreach(BuildingIdentity camp in this.m_Camps)
			{
				ItemIdentity[] items = LogicController.Instance.GetBuildingObject(camp).Items;
				if(items != null)
				{
					foreach (ItemIdentity item in items) 
					{
						result[item.itemType].Add(item);
					}
				}
			}
			return result;
		}
	}
}
