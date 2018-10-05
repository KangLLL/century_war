using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class ItemUpgradeComponent :  BuildingProduciableComponent<ItemType, ObjectUpgrade<ItemType>>
{
	protected override ObjectUpgrade<ItemType> CurrentProducingProduct 
	{
		get 
		{
			if(this.m_BuildingData.ItemUpgrade.HasValue)
			{
				return LogicController.Instance.GetItemUpgrade(this.m_BuildingData.ItemUpgrade.Value);
			}
			return null;
		}
	}
	
	protected override int ProduceEfficiency 
	{
		get 
		{
			return ConfigUtilities.ConfigInterface.Instance.SystemConfig.UpgradeItemEfficiency;
		}
	}
	
	protected override ObjectUpgrade<ItemType> GetProduct (int order)
	{
		return null;
	}
}
