using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities;
using ConfigUtilities.Enums;

public class ArmyModule
{
	private Dictionary<ArmyType, Dictionary<int, ArmyLogicObject>> m_Armies;
	private Dictionary<ArmyType, ObjectUpgrade<ArmyType>> m_Upgrades;
	
	public ArmyModule()
	{
		this.m_Armies = new Dictionary<ArmyType, Dictionary<int, ArmyLogicObject>>();
		this.m_Upgrades = new Dictionary<ArmyType, ObjectUpgrade<ArmyType>>();
	}
	
	public void InitializeArmy(List<ArmyData> armies, List<ObjectUpgrade<ArmyType>> upgrades)
	{
		foreach (ArmyData army in armies) 
		{
			if(!this.m_Armies.ContainsKey(army.ArmyID.armyType))
			{
				this.m_Armies.Add(army.ArmyID.armyType, new Dictionary<int, ArmyLogicObject>());
			}
			
			this.m_Armies[army.ArmyID.armyType].Add(army.ArmyID.armyNO, new ArmyLogicObject(army));
		}
		
		foreach (ObjectUpgrade<ArmyType> upgrade in upgrades) 
		{
			this.m_Upgrades.Add(upgrade.Identity, upgrade);
		}
	}
	
	public ArmyIdentity ProduceArmy(ArmyType type, int level, int NO)
	{
		ArmyData armyData = new ArmyData();
		armyData.ProduceRemainingWorkload = ConfigInterface.Instance.ArmyConfigHelper.GetProduceWorkload(type, level);
		armyData.ConfigData = ConfigInterface.Instance.ArmyConfigHelper.GetArmyData(type, level);
		armyData.ArmyID = new ArmyIdentity(type, NO);
		armyData.ProduceRemainingWorkload = armyData.ConfigData.ProduceWorkload;
		if(!this.m_Armies.ContainsKey(armyData.ArmyID.armyType))
		{
			this.m_Armies.Add(armyData.ArmyID.armyType, new Dictionary<int, ArmyLogicObject>());
		}
			
		this.m_Armies[armyData.ArmyID.armyType].Add(armyData.ArmyID.armyNO, new ArmyLogicObject(armyData));
		return armyData.ArmyID;
	}
	
	public void UpgradeArmy(ArmyType type, int currentLevel)
	{
		int workload = ConfigInterface.Instance.ArmyConfigHelper.GetUpgradeWorkload(type, currentLevel);
		ObjectUpgrade<ArmyType> upgrade = new ObjectUpgrade<ArmyType>(type, workload);
		this.m_Upgrades.Add(type, upgrade);
	}
	
	public ArmyType[] CurrentUpgradeArmies
	{
		get
		{
			ArmyType[] result = new ArmyType[this.m_Upgrades.Count];
			this.m_Upgrades.Keys.CopyTo(result, 0);
			return result;
		}
	}
	
	public void FinishUpgrade(ArmyType type)
	{
		this.m_Upgrades.Remove(type);
	}
	
	public ArmyLogicData GetArmyObjectData(ArmyIdentity id)
	{
		return this.m_Armies[id.armyType][id.armyNO].ArmyLogicData;
	}
	
	public ArmyLogicObject GetArmyObject(ArmyIdentity id)
	{
		return this.m_Armies[id.armyType][id.armyNO];
	}
	
	public ObjectUpgrade<ArmyType> GetUpgrade(ArmyType type)
	{
		return this.m_Upgrades[type];
	}
	
	public void DropArmy(ArmyIdentity id)
	{
		this.m_Armies[id.armyType].Remove(id.armyNO);
		if(this.m_Armies[id.armyType].Count == 0)
		{
			this.m_Armies.Remove(id.armyType);
		}
	}
}
