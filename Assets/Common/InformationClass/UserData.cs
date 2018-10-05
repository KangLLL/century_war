using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
using System.Collections.Generic;
using ConfigUtilities;

public class UserData : ICD
{
	public long PlayerID { get;set; }
    public string Name { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public int Honour { get; set; }
	public int PropsMaxCapacity { get;set; }
    public int GoldMaxCapacity { get; set; }
    public int CurrentStoreGold { get; set; }
    public int OilMaxCapacity { get; set; }
    public int CurrentStoreOil { get; set; }
    public int FoodMaxCapacity { get; set; }
    public int CurrentStoreFood { get; set; }
    public int CurrentStoreGem { get; set; }
	
	public int PlunderTotalGold { get;set; }
	public int PlunderTotalFood { get;set; }
	public int PlunderTotalOil { get;set; }
	public int RemoveTotalObject { get;set; }
	
	public float RemainingCD { get;set; }
	
	public bool IsExpMaximum { get { return this.Exp == this.ConfigData.UpgradeNeedExp && this.ConfigData.IsMaxLevel; } }
	public PlayerConfigData ConfigData { get; set; }
	
	public Dictionary<ArmyType, ProgressInformation> ArmyProgress { get;set; }
	public Dictionary<ItemType, ProgressInformation> ItemProgress { get;set; } 
	public Dictionary<MercenaryType, int> MercenaryProgress { get;set; }
	public Dictionary<BuildingType, int> DestoryBuildings { get;set; }
	public Dictionary<ArmyType, int> ProduceArmies { get;set; }
	
	public bool IsRegisterSuccessful { get;set; }
	public bool IsNewbie { get;set; }
	
	public List<LogData> AttackLogs { get; set; }
	public List<LogData> DefenseLogs { get; set; }
	
	public List<BuffData> PlayerBuffs { get; set; }
}
