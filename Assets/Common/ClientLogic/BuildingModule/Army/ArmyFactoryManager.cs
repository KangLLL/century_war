using UnityEngine;
using System.Collections;
using System;

public class ArmyFactoryManager : FactoryManager<ArmyIdentity>
{
	protected override Nullable<ArmyIdentity> GetFinishedHeadProduct (BuildingIdentity id)
	{
		BuildingLogicData factory = LogicController.Instance.GetBuildingObject(id);
		if(factory.ArmyProducts == null || factory.ArmyProducts.Count == 0)
		{
			return null;
		}
		ArmyLogicData army = LogicController.Instance.GetArmyObjectData(factory.ArmyProducts[0].Value[0]);
		if(!army.LogicProduceRemainingWorkload.IsZero())
		{
			return null;
		}
		return army.Identity;
	}
	
	protected override int GetAlreadyCapacity (BuildingLogicData building)
	{
		return building.AlreadyProduceArmyCapacity;
	}
	
	protected override int GetTotalCapacity (BuildingLogicData building)
	{
		return building.ArmyProduceCapacity;
	}
}
