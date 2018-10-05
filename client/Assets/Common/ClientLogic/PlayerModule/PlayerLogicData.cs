using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;
using CommonUtilities;

public class PlayerLogicData 
{
	private UserData m_UserData;
	
	public PlayerLogicData(UserData data)
	{
		this.m_UserData = data;
	}
	public int PropsMaxCapacity { get { return this.m_UserData.PropsMaxCapacity; } }
	
	public int GoldMaxCapacity { get { return this.m_UserData.GoldMaxCapacity; } }
	public int FoodMaxCapacity { get { return this.m_UserData.FoodMaxCapacity; } }
	public int OilMaxCapacity { get { return this.m_UserData.OilMaxCapacity; } }
	
	public int CurrentStoreGold { get { return this.m_UserData.CurrentStoreGold; } }
	public int CurrentStoreFood { get { return this.m_UserData.CurrentStoreFood; } }
	public int CurrentStoreOil { get { return this.m_UserData.CurrentStoreOil; } }
	public int CurrentStoreGem { get { return this.m_UserData.CurrentStoreGem; } }
	
	public long PlayerID { get { return this.m_UserData.PlayerID; } }
	public int Level { get { return this.m_UserData.Level; } }
	public int Exp { get { return this.m_UserData.Exp; } }
	public int CurrentLevelMaxExp { get { return this.m_UserData.ConfigData.UpgradeNeedExp; } }
	public bool IsExpMaximum { get { return this.m_UserData.IsExpMaximum; } }
	public string Name { get { return this.m_UserData.Name; } }
	public int Honour { get { return this.m_UserData.Honour; } }
	
	public int RemainingShieldSecond { get { return Mathf.RoundToInt(this.m_UserData.RemainingCD); } }
	public int CityHallLevel { get { return LogicController.Instance.GetBuildingObject(new BuildingIdentity(BuildingType.CityHall,0)).Level; } }
	
	public LogData[] AttackLogs { get { return this.m_UserData.AttackLogs.ToArray(); } }
	public LogData[] DefenseLogs { get { return this.m_UserData.DefenseLogs.ToArray(); } }
	
	public int PlunderTotalGold { get { return this.m_UserData.PlunderTotalGold; } }
	public int PlunderTotalFood { get { return this.m_UserData.PlunderTotalFood; } }
	public int PlunderTotalOil { get { return this.m_UserData.PlunderTotalOil; } }
	public int RemoveTotalObject { get { return this.m_UserData.RemoveTotalObject; } }
	
	public bool IsRegisterSuccessful { get { return this.m_UserData.IsRegisterSuccessful; } }
	public bool IsNewbie { get { return this.m_UserData.IsNewbie; } }
	
	public int GetArmyLevel(ArmyType type)
	{
		return this.m_UserData.ArmyProgress[type].Level;
	}
	
	public int GetItemLevel(ItemType type)
	{
		return this.m_UserData.ItemProgress[type].Level;
	}
	
	public int GetArmyStartNO(ArmyType type)
	{
		return this.m_UserData.ArmyProgress[type].StartNo;
	}
	
	public int GetItemStartNO(ItemType type)
	{
		return this.m_UserData.ItemProgress[type].StartNo;
	}
	
	public int GetMercenaryStartNO(MercenaryType type)
	{
		if(!this.m_UserData.MercenaryProgress.ContainsKey(type))
		{
			return 0;
		}
		return this.m_UserData.MercenaryProgress[type];
	}
	
	public int GetDestroyBuilding(BuildingType type)
	{
		return this.m_UserData.DestoryBuildings[type];
	}
	
	public int GetProduceArmyCount(ArmyType type)
	{
		return this.m_UserData.ProduceArmies[type];
	}
}
