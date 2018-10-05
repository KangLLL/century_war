using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class ItemProduceComponent : BuildingProduciableComponent<ItemIdentity, ItemLogicObject> 
{
	protected override ItemLogicObject CurrentProducingProduct 
	{
		get 
		{
			if(this.m_BuildingData.ProduceItem != null && this.m_BuildingData.ProduceItem.Count > 0)
			{
				return LogicController.Instance.GetItemObject(this.m_BuildingData.ProduceItem[0].Value[0]);
			}
			return null;
		}
	}
	
	protected override int ProduceEfficiency 
	{
		get 
		{
			return ConfigUtilities.ConfigInterface.Instance.SystemConfig.ProduceItemEfficiency;
		}
	}
	
	protected override ItemLogicObject GetProduct (int order)
	{
		int startIndex = 0;
		foreach(KeyValuePair<ItemType, List<ItemIdentity>> items in this.m_BuildingData.ProduceItem)
		{
			order -= startIndex;
			if(order < items.Value.Count)
			{
				return LogicController.Instance.GetItemObject(items.Value[order]);
			}
			startIndex += items.Value.Count;
		}
		return null;
	}
}
