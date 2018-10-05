using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

using ConfigUtilities.Enums;
using ConfigUtilities;

public class BuildingData 
{ 
	public BuildingIdentity BuildingID { get; set; }
    public int Level { get; set; }
	public BuildingConfigData ConfigData { get; set; }
    
    public int CurrentStoreGold { get; set; }
    public int CurrentStoreOil { get; set; }
    public int CurrentStoreFood { get; set; }

	public Nullable<long> LastCollectedGoldTick { get; set; }
	public Nullable<int> CollectedGold { get; set; }
	public Nullable<long> LastCollectedFoodTick { get; set; }
	public Nullable<int> CollectedFood { get; set; }
	public Nullable<long> LastCollectedOilTick { get; set; }
	public Nullable<int> CollectedOil { get; set; }
 
    public TilePosition BuildingPosition { get; set; }
	public Nullable<int> BuilderBuildingNO { get;set;}
	public Nullable<float> UpgradeRemainingWorkload { get; set; }
	
	public List<ArmyIdentity> AvailableArmy { get;set; }
	public List<KeyValuePair<ArmyType, List<ArmyIdentity>>> ProduceArmy { get;set;}

	public List<ItemIdentity> AvailableItem { get;set; }
	public List<KeyValuePair<ItemType, List<ItemIdentity>>> ProduceItem { get;set; }
	
	public List<MercenaryIdentity> AvailableMercenary { get;set; }
	public MercenaryProductCollectionLogicObject ProduceMercenary { get;set; }
	
	public Nullable<ArmyType> ArmyUpgrade { get;set; }
	public Nullable<ItemType> ItemUpgrade { get;set; }
	
	public Nullable<float> RemainResourceAccelerateTime { get;set; }
	public Nullable<float> RemainArmyAccelerateTime { get;set; }
	public Nullable<float> RemainItemAccelerateTime { get;set; }
}


