using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;
using CommonUtilities;
using UnityEngine;
using CommandConsts;

public class PlayerLogicObject : LogicObject
{
	private UserData m_UserData;
	private PlayerLogicData m_Data;
	
	private Dictionary<BuffData, BuffLogicData> m_Buffs;
	public PlayerLogicData Data { get { return this.m_Data; } }
	
	private CDComponent m_ShieldCDComponent;
	private Dictionary<BuffData, CDComponent> m_BuffComponents;
	
	private List<BuffData> m_RemoveBuff;
	
	public IEnumerable<BuffLogicData> Buffs 
	{ 
		get 
		{
			foreach (KeyValuePair<BuffData, BuffLogicData> buff in this.m_Buffs) 
			{
				yield return buff.Value;
			}
		} 
	}
	
	public void IntializePlayer(UserData data)
	{
		this.m_UserData = data;
		this.m_Data = new PlayerLogicData(data);
		
		this.AddShieldComponent();
		
		this.m_Buffs = new Dictionary<BuffData, BuffLogicData>();
		this.m_BuffComponents = new Dictionary<BuffData, CDComponent>();
		this.m_RemoveBuff = new List<BuffData>();
		foreach (var buff in data.PlayerBuffs) 
		{
			this.AddBuff(buff);
		}
	}
	
	public void Consume(int gold, int food, int oil, int gem)
	{
		this.m_UserData.CurrentStoreGold -= gold;
		this.m_UserData.CurrentStoreFood -= food;
		this.m_UserData.CurrentStoreOil -= oil;
		this.m_UserData.CurrentStoreGem -= gem;
	}
	
	public void Receive(int gold, int food, int oil, int gem)
	{
		this.m_UserData.CurrentStoreGold += gold;
		this.m_UserData.CurrentStoreGold = Mathf.Min(this.m_UserData.CurrentStoreGold, this.m_UserData.GoldMaxCapacity);
		this.m_UserData.CurrentStoreFood += food;
		this.m_UserData.CurrentStoreFood = Mathf.Min(this.m_UserData.CurrentStoreFood, this.m_UserData.FoodMaxCapacity);
		this.m_UserData.CurrentStoreOil += oil;
		this.m_UserData.CurrentStoreOil = Mathf.Min(this.m_UserData.CurrentStoreOil, this.m_UserData.OilMaxCapacity);
		this.m_UserData.CurrentStoreGem += gem;
	}
	
	public void Receive(int gold, int food, int oil, int gem, out int addGold, out int addFood, out int addOil, out int addGem)
	{
		int originalGold = this.m_UserData.CurrentStoreGold;
		int originalFood = this.m_UserData.CurrentStoreFood;
		int originalOil = this.m_UserData.CurrentStoreOil;
		int originalGem = this.m_UserData.CurrentStoreGem;
		
		this.Receive(gold, food, oil, gem);
		
		addGold = this.m_UserData.CurrentStoreGold - originalGold;
		addFood = this.m_UserData.CurrentStoreFood - originalFood;
		addOil = this.m_UserData.CurrentStoreOil - originalOil;
		addGem = this.m_UserData.CurrentStoreGem - originalGem;
	}
	
	public void AddExp(int exp)
	{
		if(!this.m_UserData.IsExpMaximum)
		{
			if(this.m_UserData.ConfigData.IsMaxLevel)
			{
				int addExp = Mathf.Min(this.m_UserData.ConfigData.UpgradeNeedExp - this.m_UserData.Exp, exp);
				this.m_UserData.Exp += addExp;
			}
			else
			{
				this.m_UserData.Exp += exp;
				while(this.m_UserData.Exp >= this.m_UserData.ConfigData.UpgradeNeedExp && !this.m_UserData.IsExpMaximum)
				{
					this.m_UserData.Exp -= this.m_UserData.ConfigData.UpgradeNeedExp;
					this.m_UserData.Level ++;
					this.m_UserData.ConfigData = ConfigInterface.Instance.PlayerConfigHelper.GetPlayerData(this.m_UserData.Level);
					
					if(this.m_UserData.ConfigData.IsMaxLevel)
					{
						this.m_UserData.Exp = Mathf.Min(this.m_UserData.Exp, this.m_UserData.ConfigData.UpgradeNeedExp);
					}
				}
			}
		}
	}
	
	public void WinHonour(int winValue)
	{
		this.m_UserData.Honour += winValue;
	}
	
	public void LoseHonour(int loseValue) 
	{
		this.m_UserData.Honour -= loseValue;
	}
	
	public void AddPropsCapacity(int newPropsCapacity, int oldPropsCapacity)
	{
		this.m_UserData.PropsMaxCapacity -= oldPropsCapacity;
		this.m_UserData.PropsMaxCapacity += newPropsCapacity;
	}
	
	public void AddPropsCapacity(int newPropsCapacity)
	{
		this.AddPropsCapacity(newPropsCapacity, 0);
	}
	
	public void AddGoldCapacity(int newGoldCapacity, int oldGoldCapacity)
	{
		this.m_UserData.GoldMaxCapacity -= oldGoldCapacity;
		this.m_UserData.GoldMaxCapacity += newGoldCapacity;
	}
	
	public void AddFoodCapacity(int newFoodCapacity, int oldFoodCapacity)
	{
		this.m_UserData.FoodMaxCapacity -= oldFoodCapacity;
		this.m_UserData.FoodMaxCapacity += newFoodCapacity;
	}
	
	public void AddOilCapacity(int newOilCapacity, int oldOilCapacity)
	{
		this.m_UserData.OilMaxCapacity -= oldOilCapacity;
		this.m_UserData.OilMaxCapacity += newOilCapacity;
	}
	
	public void AddGoldCapacity(int newGoldCapacity)
	{
		this.AddGoldCapacity(newGoldCapacity, 0);
	}
	
	public void AddFoodCapacity(int newFoodCapacity)
	{
		this.AddFoodCapacity(newFoodCapacity, 0);
	}
	
	public void AddOilCapacity(int newOilCapacity)
	{
		this.AddOilCapacity(newOilCapacity, 0);
	}
	
	public void AddCapacity(int newGoldCapacity, int newFoodCapacity, int newOilCapacity)
	{
		this.m_UserData.GoldMaxCapacity += newGoldCapacity;
		this.m_UserData.FoodMaxCapacity += newFoodCapacity;
		this.m_UserData.OilMaxCapacity += newOilCapacity;
	}
	
	public void AddCapacity(int newGoldCapacity, int newFoodCapacity, int newOilCapacity, int oldGoldCapacity, 
		int oldFoodCapacity, int oldOilCapacity)
	{
		this.AddCapacity(-oldGoldCapacity, -oldFoodCapacity, -oldOilCapacity);
		this.AddCapacity(newGoldCapacity, newFoodCapacity, newOilCapacity);
	}
	
	public void BuyGold(int gold)
	{
		this.m_UserData.CurrentStoreGold += gold;
		this.m_UserData.CurrentStoreGem -= MarketCalculator.GetGoldCost(gold);
	}
	
	public void BuyFood(int food)
	{
		this.m_UserData.CurrentStoreFood += food;
		this.m_UserData.CurrentStoreGem -= MarketCalculator.GetFoodCost(food);
	}
	
	public void BuyOil(int oil)
	{
		this.m_UserData.CurrentStoreOil += oil;
		this.m_UserData.CurrentStoreGem -= MarketCalculator.GetOilCost(oil);
	}

	public void UpgradeArmy(ArmyType type)
	{
		this.m_UserData.ArmyProgress[type].Level += ConfigInterface.Instance.ArmyConfigHelper.GetUpgradeStep(type);
	}
	
	public void UpgradeItem(ItemType type)
	{
		this.m_UserData.ItemProgress[type].Level ++;
	}
	
	public void AddArmy(ArmyType type)
	{
		this.m_UserData.ArmyProgress[type].StartNo ++;
	}
	
	public void AddItem(ItemType type)
	{
		this.m_UserData.ItemProgress[type].StartNo ++;
	}
	
	public void HireMercenary(MercenaryType type)
	{
		if(!this.m_UserData.MercenaryProgress.ContainsKey(type))
		{
			this.m_UserData.MercenaryProgress.Add(type, 0);
		}
		this.m_UserData.MercenaryProgress[type] ++;
		
		MercenaryConfigData configData = ConfigInterface.Instance.MercenaryConfigHelper.GetMercenaryData(type);
		this.Consume(configData.HireCostGold, configData.HireCostFood, configData.HireCostOil, configData.HireCostGem);
	}
	
	public void FindMatch()
	{
		int cost = ConfigInterface.Instance.SystemConfig.FindMatchCost + this.m_Data.CityHallLevel * 
			ConfigInterface.Instance.SystemConfig.FindMatchPlusPerCityHallLevel;
		this.m_UserData.CurrentStoreGold -= cost;
		this.m_UserData.RemainingCD = 0;
	}
	
	public void AddAttackLog(LogData logData)
	{
		this.m_UserData.AttackLogs.Insert(0, logData);
	}
	
	public void Revenge()
	{
		this.m_UserData.RemainingCD = 0;
	}
	
	public void Plunder(int gold, int food, int oil)
	{
		this.Receive(gold, food, oil, 0);
		this.m_UserData.PlunderTotalGold += gold;
		this.m_UserData.PlunderTotalFood += food;
		this.m_UserData.PlunderTotalOil += oil;
	}
	
	public void RemoveObject()
	{
		this.m_UserData.RemoveTotalObject ++;
	}
	
	public void DestroyBuilding(BuildingType type)
	{
		this.m_UserData.DestoryBuildings[type] ++;
	}
	
	public void ProducedArmy(ArmyType type)
	{
		this.m_UserData.ProduceArmies[type] ++;
	}
	
	public void ChangeName(string newName)
	{
		this.m_UserData.IsRegisterSuccessful = true;
		this.m_UserData.Name = newName;
	}
	
	public void CompleteNewbieGuide()
	{
		this.m_UserData.IsNewbie = false;
	}
	
	public void AddPlayerBuff(int propsNo)
	{
		PropsLogicData propsData = LogicController.Instance.GetProps(propsNo);
		PropsBuffConfigData newBuffConfig = propsData.FunctionConfigData as PropsBuffConfigData;
		BuffData buffData = new BuffData();
		buffData.RelatedPropsType = propsData.PropsType;
		buffData.RemainingCD = newBuffConfig.LastingSeconds;
		buffData.BuffConfigData = newBuffConfig;
		
		foreach (KeyValuePair<BuffData, BuffLogicData> buff in this.m_Buffs) 
		{
			if(newBuffConfig.BuffCategory == buff.Key.BuffConfigData.BuffCategory)
			{
				this.RemoveBuff(buff.Key);
				break;
			}
		}
		this.AddBuff(buffData);
		
		AddPlayerBuffRequestParameter request = new AddPlayerBuffRequestParameter();
		request.PropsNo = propsNo;
		request.OperateTick = LogicTimer.Instance.GetServerTick();
		CommunicationUtility.Instance.AddPlayerBuff(request);
	}
	
	public void AddPlayerShield(int propsNo)
	{
		PropsLogicData propsData = LogicController.Instance.GetProps(propsNo);
		PropsShieldConfigData shieldData = propsData.FunctionConfigData as PropsShieldConfigData;
		
		if(this.m_ShieldCDComponent == null)
		{
			this.AddShieldComponent();
		}
		this.m_UserData.RemainingCD += shieldData.AddTime;
		
		AddPlayerShieldRequestParameter request = new AddPlayerShieldRequestParameter();
		request.PropsNo = propsNo;
		request.OperateTick = LogicTimer.Instance.GetServerTick();
		CommunicationUtility.Instance.AddPlayerShield(request);
	}
	
	private void AddShieldComponent()
	{
		CDComponent shieldCD = new CDComponent(this.m_UserData, LogicTimer.Instance.CurrentTime);
		this.m_ShieldCDComponent = shieldCD;
		this.m_ShieldCDComponent.CDFinished += ShieldCDFinish;
		this.AddComponent(shieldCD);
	}
	
	private void ShieldCDFinish(ICD userData)
	{
		this.RemoveComponent(this.m_ShieldCDComponent);
		this.m_ShieldCDComponent = null;
	}
	
	private void BuffCDFinish(ICD buffData)
	{
		BuffData data = buffData as BuffData;
		this.m_RemoveBuff.Add(data);
	}
	
	private void RemoveBuff(BuffData buffData)
	{
		this.m_UserData.PlayerBuffs.Remove(buffData);
		this.m_Buffs.Remove(buffData);
		this.RemoveComponent(this.m_BuffComponents[buffData]);
		this.m_BuffComponents.Remove(buffData);
	}
	
	private void AddBuff(BuffData buffData)
	{
		BuffLogicData data = new BuffLogicData(buffData);
		this.m_Buffs.Add(buffData, data);
		CDComponent buffCD = new CDComponent(buffData, LogicTimer.Instance.CurrentTime);
		this.m_BuffComponents.Add(buffData, buffCD);
		buffCD.CDFinished += BuffCDFinish;
		this.AddComponent(buffCD);
	}
	
	public override void Process ()
	{
		this.m_RemoveBuff.Clear();
		foreach (KeyValuePair<BuffData, CDComponent> buff in m_BuffComponents) 
		{
			buff.Value.ProduceTo(LogicTimer.Instance.CurrentTime);
		}
		
		if(this.m_ShieldCDComponent != null)
		{
			this.m_ShieldCDComponent.ProduceTo(LogicTimer.Instance.CurrentTime);
		}
		
		foreach (BuffData data in this.m_RemoveBuff) 
		{
			this.RemoveBuff(data);
		}
		base.Process ();
	}
}
