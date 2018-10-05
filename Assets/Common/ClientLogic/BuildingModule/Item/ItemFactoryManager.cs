using UnityEngine;
using System.Collections;
using System;

public class ItemFactoryManager : FactoryManager<ItemIdentity>
{
	protected override Nullable<ItemIdentity> GetFinishedHeadProduct (BuildingIdentity id)
	{
		BuildingLogicData factory = LogicController.Instance.GetBuildingObject(id);
		if(factory.ItemProducts == null)
		{
			return null;
		}
		ItemLogicObject item = LogicController.Instance.GetItemObject(factory.ItemProducts[0].Value[0]);
		if(!item.LogicProduceRemainingWorkload.IsZero())
		{
			return null;
		}
		return item.Identity;
	}
	
	protected override int GetAlreadyCapacity (BuildingLogicData building)
	{
		return building.AlreadyProduceItemCapacity;
	}
	
	protected override int GetTotalCapacity (BuildingLogicData building)
	{
		return building.ItemProduceCapacity;
	}
}
