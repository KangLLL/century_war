using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;

public class ArmyCampManager : CampManager<ArmyType, ArmyIdentity>
{
	protected override int GetAlreadyCapacity (BuildingLogicData building)
	{
		return building.AlreadyArmyCapacity;
	}
	
	protected override int GetTotalCapacity (BuildingLogicData building)
	{
		return building.ArmyCapacity;
	}
	
	protected override int GetCapacityCost (ArmyType type)
	{
		int capacityCost = ConfigInterface.Instance.ArmyConfigHelper.GetArmyCapacityCost(type);
		return capacityCost;
	}
	
	public Dictionary<MercenaryType, List<MercenaryIdentity>>AvailableMercenaries
	{
		get
		{
			Dictionary<MercenaryType, List<MercenaryIdentity>> result = new Dictionary<MercenaryType, List<MercenaryIdentity>>();
			for(int i = 0; i < (int)MercenaryType.Length; i ++)
			{
				MercenaryType mercenaryType = (MercenaryType)i;
				result.Add(mercenaryType, new List<MercenaryIdentity>());
			}
			
			foreach(BuildingIdentity camp in this.m_Camps)
			{
				MercenaryIdentity[] mercenaries = LogicController.Instance.GetBuildingObject(camp).Mercenaries;
				if(mercenaries != null)
				{
					foreach (MercenaryIdentity mercenary in mercenaries) 
					{
						result[mercenary.mercenaryType].Add(mercenary);
					}
				}
			}
			return result;
			
		}
	}
	
	public BuildingIdentity? FindMercenaryCamp(MercenaryType type)
	{
		int capacityCost = ConfigInterface.Instance.MercenaryConfigHelper.GetMercenaryData(type).CapcityCost;
		
		if(capacityCost + this.CampsTotalAlreadyCapacity > this.CampsTotalCapacity)
			return null;
		
		foreach (BuildingIdentity id in this.m_Camps) 
		{
			BuildingLogicData building = LogicController.Instance.GetBuildingObject(id);
			if(this.GetAlreadyCapacity(building) < this.GetTotalCapacity(building))
			{
				return id;
			}
		}
		return null;
	}
	
	public override Dictionary<ArmyType, List<ArmyIdentity>> AvailableObjects 
	{
		get 
		{
			Dictionary<ArmyType, List<ArmyIdentity>> result = new Dictionary<ArmyType, List<ArmyIdentity>>();
			
			for(int i = 0; i < (int)ArmyType.Length; i ++)
			{
				ArmyType armyType = (ArmyType)i;
				result.Add(armyType, new List<ArmyIdentity>());
			}
			
			foreach(BuildingIdentity camp in this.m_Camps)
			{
				ArmyIdentity[] armies = LogicController.Instance.GetBuildingObject(camp).Armies;
				if(armies != null)
				{
					foreach (ArmyIdentity army in armies) 
					{
						result[army.armyType].Add(army);
					}
				}
			}
			return result;
		}
	}
}
