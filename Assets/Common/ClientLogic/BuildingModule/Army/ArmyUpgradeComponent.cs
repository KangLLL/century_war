using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class ArmyUpgradeComponent : BuildingProduciableComponent<ArmyType, ObjectUpgrade<ArmyType>>
{
	protected override ObjectUpgrade<ArmyType> CurrentProducingProduct 
	{
		get 
		{
			if(this.m_BuildingData.ArmyUpgrade.HasValue)
			{
				return LogicController.Instance.GetArmyUpgrade(this.m_BuildingData.ArmyUpgrade.Value);
			}
			return null;
		}
	}
	
	protected override int ProduceEfficiency 
	{
		get 
		{
			return ConfigUtilities.ConfigInterface.Instance.SystemConfig.UpgradeArmyEfficiency;
		}
	}
	
	protected override ObjectUpgrade<ArmyType> GetProduct (int order)
	{
		return null;
	}
}
