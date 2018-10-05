using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using ConfigUtilities.Enums;
using ConfigUtilities;

public class ArmyProduceComponent : BuildingProduciableComponent<ArmyIdentity, ArmyLogicObject> 
{
	protected override ArmyLogicObject CurrentProducingProduct 
	{
		get 
		{
			if(this.m_BuildingData.ProduceArmy != null && this.m_BuildingData.ProduceArmy.Count > 0)
			{
				return LogicController.Instance.GetArmyObject(this.m_BuildingData.ProduceArmy[0].Value[0]);
			}
			return null;
		}
	}
	
	protected override int ProduceEfficiency 
	{
		get 
		{
			return ConfigInterface.Instance.SystemConfig.ProduceArmyEfficiency;
		}
	}
	
	protected override ArmyLogicObject GetProduct (int order)
	{
		int startIndex = 0;
		foreach(KeyValuePair<ArmyType, List<ArmyIdentity>> armies in this.m_BuildingData.ProduceArmy)
		{
			order -= startIndex;
			if(order < armies.Value.Count)
			{
				return LogicController.Instance.GetArmyObject(armies.Value[order]);
			}
			startIndex += armies.Value.Count;
		}
		return null;
	}
}
