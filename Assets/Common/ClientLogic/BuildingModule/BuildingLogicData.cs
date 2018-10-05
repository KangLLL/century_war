using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ConfigUtilities;
using ConfigUtilities.Enums;
using CommandConsts;
using CommonUtilities;

public class BuildingLogicData : IBuildingInfo
{
	private BuildingData m_Data;
	private BuildingLogicObject m_LogicObject;
	
	public BuildingLogicData(BuildingData data, BuildingLogicObject logicOject)
	{
		this.m_Data = data;
		this.m_LogicObject = logicOject;
	}
	
		
	public int MaxHP { get{ return this.m_Data.ConfigData.MaxHP; } }
	
	private List<TilePosition> m_BuildingObstacleList;
	public List<TilePosition> BuildingObstacleList
	{
		get
		{
			if(this.m_BuildingObstacleList == null)
			{
				this.m_BuildingObstacleList = new List<TilePosition>();
				foreach(ConfigUtilities.Structs.TilePoint point in this.m_Data.ConfigData.BuildingObstacleList)	
				{
					this.m_BuildingObstacleList.Add(point.ConvertToTilePosition());
				}
			}
			return this.m_BuildingObstacleList;
		}
	}
	
	private List<TilePosition> m_ActorObstacleList;
	public List<TilePosition> ActorObstacleList 
	{ 
		get
		{
			if(this.m_ActorObstacleList == null)
			{
				this.m_ActorObstacleList = new List<TilePosition>();
				foreach(ConfigUtilities.Structs.TilePoint point in this.m_Data.ConfigData.ActorObstacleList)	
				{
					this.m_ActorObstacleList.Add(point.ConvertToTilePosition());
				}
			}
			return this.m_ActorObstacleList;
		}
	}
	
	public BuildingIdentity BuildingIdentity { get { return this.m_Data.BuildingID; } }
    public string Name { get{ return this.m_Data.ConfigData.Name; } }
    public string Description { get{ return this.m_Data.ConfigData.Description; } }
	public int MaximumLevel { get{ return this.m_Data.ConfigData.MaximumLevel; } }
	public bool IsMaximumLevel { get{ return this.m_Data.ConfigData.IsMaximumLevel; } }
    public int Level { get { return this.m_Data.Level; } }
    public int UpgradeGold { get { return this.m_Data.ConfigData.UpgradeGold; } }
    public int UpgradeOil { get { return this.m_Data.ConfigData.UpgradeOil; } }
    public int UpgradeFood { get { return this.m_Data.ConfigData.UpgradeFood; } }
    public int UpgradeGem { get { return this.m_Data.ConfigData.UpgradeGem; } }
	
	public int UpgradeRewardGold { get { return this.m_Data.ConfigData.UpgradeRewardGold; } }
	public int UpgradeRewardFood { get { return this.m_Data.ConfigData.UpgradeRewardFood; } }
	public int UpgradeRewardOil { get { return this.m_Data.ConfigData.UpgradeRewardOil; } }
	public int UpgradeRewardGem { get { return this.m_Data.ConfigData.UpgradeRewardGem; } }
	public int UpgradeRewardExp { get { return this.m_Data.ConfigData.UpgradeRewardExp; } }
	
	public int UpgradeWorkload { get { return this.m_Data.ConfigData.UpgradeWorkload; } }
    public float UpgradeRemainingWorkload 
	{ 
		get 
		{ 
			return this.m_Data.UpgradeRemainingWorkload.HasValue ? this.m_Data.UpgradeRemainingWorkload.Value : 0; 
		} 
	}
	public bool CanAttack { get { return this.m_Data.ConfigData.CanAttack; } }
    public bool CanProduceArmy { get { return this.m_Data.ConfigData.CanProduceArmy; } }
    public bool CanUpgradeArmy { get { return this.m_Data.ConfigData.CanUpgradeArmy; } }
    public bool CanProduceItem { get { return this.m_Data.ConfigData.CanProduceItem; } }
    public bool CanUpgradeItem { get { return this.m_Data.ConfigData.CanUpgradeItem; } }
    public bool CanStoreItem { get { return this.m_Data.ConfigData.CanStoreItem; } }
    public bool CanClan { get { return this.m_Data.ConfigData.CanClan; } }
    public bool CanHelpArmy { get { return this.m_Data.ConfigData.CanHelpArmy; } }
    public float ProduceGoldEfficiency{ get { return this.m_Data.ConfigData.ProduceGoldEfficiency; } }
    public float ProduceOilEfficiency{ get { return this.m_Data.ConfigData.ProduceOilEfficiency; } }
    public float ProduceFoodEfficiency { get { return this.m_Data.ConfigData.ProduceFoodEfficiency; } }
    public int ProduceArmyEfficiency { get { return ConfigInterface.Instance.SystemConfig.ProduceArmyEfficiency; } }
    public int ProduceItemEfficiency { get { return ConfigInterface.Instance.SystemConfig.ProduceItemEfficiency; } }
    public int StoreGoldCapacity { get { return this.m_Data.ConfigData.StoreGoldCapacity; } }
    public int StoreOilCapacity { get { return this.m_Data.ConfigData.StoreOilCapacity; } }
    public int StoreFoodCapacity { get { return this.m_Data.ConfigData.StoreFoodCapacity; } }
	public bool CanStoreGold { get { return this.m_Data.ConfigData.CanStoreGold; } }
    public bool CanStoreOil { get { return this.m_Data.ConfigData.CanStoreOil; } }
    public bool CanStoreFood { get { return this.m_Data.ConfigData.CanStoreFood; } }
    public bool CanCollectGold { get { return this.m_Data.ConfigData.CanProduceGold; } }
    public bool CanCollectFood { get { return this.m_Data.ConfigData.CanProduceFood; } }
    public bool CanCollectOil { get { return this.m_Data.ConfigData.CanProduceOil; } }

    public int CurrentStoreGold 
	{ 
		get 
		{
			if(this.m_Data.ConfigData.CanProduceGold)
			{
				if(this.m_Data.UpgradeRemainingWorkload.HasValue)
				{
					return this.m_Data.CollectedGold.Value;
				}
				else
				{
					float efficiency = this.m_Data.RemainResourceAccelerateTime.HasValue ? this.m_Data.ConfigData.ProduceGoldEfficiency * 
						ConfigInterface.Instance.SystemConfig.ProduceResourceAccelerateScale :
							this.m_Data.ConfigData.ProduceGoldEfficiency;
				    return CommonUtilities.WorkloadCalculator.CalculateProduceWorkload(this.m_Data.LastCollectedGoldTick.Value,
					                                                                   LogicTimer.Instance.GetServerTick(),
					                                                                   this.m_Data.CollectedGold,
					                                                                   efficiency,
					                                                                   this.m_Data.ConfigData.StoreGoldCapacity);
				}
			}
			else
			{
				return this.m_Data.CurrentStoreGold; 
			}
		} 
	}
	
    public int CurrentStoreOil 
	{ 
		get 
		{ 
			if(this.m_Data.ConfigData.CanProduceOil)
			{
				if(this.m_Data.UpgradeRemainingWorkload.HasValue)
				{
					return this.m_Data.CollectedOil.Value;
				}
				else
				{
					float efficiency = this.m_Data.RemainResourceAccelerateTime.HasValue ? this.m_Data.ConfigData.ProduceOilEfficiency * 
						ConfigInterface.Instance.SystemConfig.ProduceResourceAccelerateScale :
							this.m_Data.ConfigData.ProduceOilEfficiency;
					return CommonUtilities.WorkloadCalculator.CalculateProduceWorkload(this.m_Data.LastCollectedOilTick.Value,
					                                                                   LogicTimer.Instance.GetServerTick(),
					                                                                   this.m_Data.CollectedOil,
					                                                                   efficiency,
					                                                                   this.m_Data.ConfigData.StoreOilCapacity);
				}
			}
			else
			{
				return this.m_Data.CurrentStoreOil; 
			}
		} 
	}
    public int CurrentStoreFood 
	{ 
		get 
		{
			if(this.m_Data.ConfigData.CanProduceFood)
			{
				if(this.m_Data.UpgradeRemainingWorkload.HasValue)
				{
					return this.m_Data.CollectedFood.Value;
				}
				else
				{
					float efficiency = this.m_Data.RemainResourceAccelerateTime.HasValue ? this.m_Data.ConfigData.ProduceFoodEfficiency * 
						ConfigInterface.Instance.SystemConfig.ProduceResourceAccelerateScale :
							this.m_Data.ConfigData.ProduceFoodEfficiency;
					return CommonUtilities.WorkloadCalculator.CalculateProduceWorkload(this.m_Data.LastCollectedFoodTick.Value,
					                                                                   LogicTimer.Instance.GetServerTick(),
					                                                                   this.m_Data.CollectedFood,
					                                                                   efficiency,
					                                                                   this.m_Data.ConfigData.StoreFoodCapacity);
				}
			}
			else
			{
				return this.m_Data.CurrentStoreFood; 
			}
		} 
	}
    
	public float PlunderRate { get { return this.m_Data.ConfigData.PlunderRate; } }
    public int ArmyCapacity { get { return this.m_Data.ConfigData.ArmyCapacity; } }
    public int ArmyProduceCapacity { get { return this.m_Data.ConfigData.ArmyProduceCapacity; } }
    public int StoreItemCapacity { get { return this.m_Data.ConfigData.StoreItemCapacity; } }
	public int ItemProduceCapacity { get { return this.m_Data.ConfigData.ItemProduceCapacity; } }
	
    public int AttackValue { get{ return this.m_Data.ConfigData.AttackValue; } }
    public int ApMaxScope { get { return this.m_Data.ConfigData.ApMaxScope; } }
    public int ApMinScope { get { return this.m_Data.ConfigData.ApMinScope; } }
    public float AttackCD { get { return this.m_Data.ConfigData.AttackCD; } }
    
	public AttackType AttackType{ get { return (AttackType)this.m_Data.ConfigData.AttackType; } }
    public TargetType TargetType { get { return (TargetType)this.m_Data.ConfigData.TargetType; } }
    public ArmyCategory Favorite { get { return (ArmyCategory)this.m_Data.ConfigData.Favorite; } }
    public TilePosition BuildingPosition { get { return this.m_Data.BuildingPosition; } }
	public TilePosition ActorPosition 
	{ 
		get 
		{ 
			return PositionConvertor.GetActorTilePositionFromBuildingTilePosition(this.m_Data.BuildingPosition); 
		} 
	}
    public string BuildingPrefabName { get { return this.m_Data.ConfigData.BuildingPrefabName; } }
    
	public int AlreadyArmyCapacity { get { return this.m_LogicObject.AlreadyArmyCapacity; } } 
	public int AlreadyItemCapacity { get { return this.m_LogicObject.AlreadyItemCapacity; } }
	public int AlreadyProduceArmyCapacity { get { return this.m_LogicObject.AlreadyProduceArmyCapacity; } }
	public int AlreadyProduceItemCapacity { get { return this.m_LogicObject.AlreadyProduceItemCapacity; } }
	
	public int RemainingAccelerateTime
	{
		get
		{	
			return this.m_Data.UpgradeRemainingWorkload.HasValue ? -1 :
				this.m_Data.RemainResourceAccelerateTime.HasValue ? Mathf.CeilToInt(this.m_Data.RemainResourceAccelerateTime.Value) :
				this.m_Data.RemainArmyAccelerateTime.HasValue ? Mathf.CeilToInt(this.m_Data.RemainArmyAccelerateTime.Value) :
				this.m_Data.RemainItemAccelerateTime.HasValue ? Mathf.CeilToInt(this.m_Data.RemainItemAccelerateTime.Value) :
					-1;
		}
	}
	
	public int RemainResourceAccelerateTime 
	{ 
		get 
		{ 
			return this.m_Data.UpgradeRemainingWorkload.HasValue ? -1 :
				this.m_Data.RemainResourceAccelerateTime.HasValue ? 
				Mathf.CeilToInt(this.m_Data.RemainResourceAccelerateTime.Value) : -1;
		} 
	}
	
	public int RemainArmyAccelerateTime 
	{
		get 
		{ 
			return this.m_Data.UpgradeRemainingWorkload.HasValue ? -1:
				this.m_Data.RemainArmyAccelerateTime.HasValue ? 
				Mathf.CeilToInt(this.m_Data.RemainArmyAccelerateTime.Value) : -1;
		} 
	}
	
	public int RemainItemAccelerateTime 
	{ 
		get 
		{ 
			return this.m_Data.UpgradeRemainingWorkload.HasValue ? -1 :
				this.m_Data.RemainArmyAccelerateTime.HasValue ? 
				Mathf.CeilToInt(this.m_Data.RemainItemAccelerateTime.Value) : -1; 
		} 
	}
	
	public int CurrentAttachedBuilderNO 
	{ 
		get 
		{ 
			if(this.m_Data.BuilderBuildingNO.HasValue)
				return this.m_Data.BuilderBuildingNO.Value;
			return -1;
		} 
	}
	public int InitialLevel { get { return this.m_Data.ConfigData.InitialLevel; } }
	public int UpgradeStep { get { return this.m_Data.ConfigData.UpgradeStep; } }
	
	public List<KeyValuePair<ArmyType, List<ArmyIdentity>>> ArmyProducts
	{
		get
		{
			if(this.m_Data.ProduceArmy == null)
				return null;
			List<KeyValuePair<ArmyType, List<ArmyIdentity>>> result = new List<KeyValuePair<ArmyType, List<ArmyIdentity>>>();
			foreach (KeyValuePair<ArmyType,List<ArmyIdentity>> productList in this.m_Data.ProduceArmy) 
			{
				List<ArmyIdentity> list = new List<ArmyIdentity>();
				foreach(ArmyIdentity id in productList.Value)
				{
					list.Add(id);
				}
				result.Add(new KeyValuePair<ArmyType, List<ArmyIdentity>>(productList.Key,list));
			}
			return result;
		}
	}
	
	public bool IsArmyProduceBlock
	{
		get
		{
			if(Application.loadedLevelName.Equals(ClientStringConstants.BUILDING_SCENE_LEVEL_NAME))
			{
				if(this.m_Data.ProduceArmy == null || this.m_Data.ProduceArmy.Count == 0)
					return false;
				return LogicController.Instance.GetArmyObjectData(this.m_Data.ProduceArmy[0].Value[0]).ProduceRemainingWorkload == 0;
			}
			else
			{
				return false;
			}
		}
	}
	
	public int ArmyProductsRemainingTime
	{
		get
		{
			int result = 0;
			if(this.m_Data.ProduceArmy == null)
				return result;
			foreach(KeyValuePair<ArmyType, List<ArmyIdentity>> productList in this.m_Data.ProduceArmy)
			{
				foreach(ArmyIdentity id in productList.Value)
				{
					result += Mathf.CeilToInt(LogicController.Instance.GetArmyObject(id).LogicProduceRemainingWorkload / this.ProduceArmyEfficiency);
				}
			}
			return result;
		}
	}

	public ArmyIdentity[] Armies 
	{ 
		get 
		{ 
			if(this.m_Data.AvailableArmy == null)
				return null;
			return this.m_Data.AvailableArmy.ToArray(); 
		} 
	}
	
	public MercenaryIdentity[] Mercenaries
	{
		get
		{
			if(this.m_Data.AvailableMercenary == null)
				return null;
			return this.m_Data.AvailableMercenary.ToArray();
		} 
	}
	
	public Dictionary<MercenaryType, MercenaryProductLogicData> MercenaryProducts
	{
		get
		{
			if(this.m_Data.ProduceMercenary == null)
				return null;
			Dictionary<MercenaryType, MercenaryProductLogicData> result = new Dictionary<MercenaryType, MercenaryProductLogicData>();
			foreach (KeyValuePair<MercenaryType, MercenaryProductLogicObject> item in this.m_Data.ProduceMercenary.Products) 
			{
				result.Add(item.Key, item.Value.Data);
			}
			return result;
		}
	}
	
	public Nullable<ArmyType> ArmyUpgrade { get { return this.m_Data.ArmyUpgrade; } }
	public Nullable<ItemType> ItemUpgrade { get { return this.m_Data.ItemUpgrade; } }
	
	public int ArmyUpgradeTotalWorkload 
	{
		get
		{
			if(this.m_Data.ArmyUpgrade.HasValue)
			{
				return ConfigInterface.Instance.ArmyConfigHelper.GetUpgradeWorkload(this.m_Data.ArmyUpgrade.Value, 
					LogicController.Instance.GetArmyLevel(this.m_Data.ArmyUpgrade.Value));
			}
			else
			{
				return -1;
			}
		}
	}
	
	public int ItemUpgradeTotalWorkload
	{
		get
		{
			/*
			if(this.m_Data.ItemUpgrade.HasValue)
			{
				return ConfigInterface.Instance.ItemConfigHelper.GetUpgradeWorkload(this.m_Data.ItemUpgrade.Value,
					LogicController.Instance.GetItemLevel(this.m_Data.ItemUpgrade.Value));
			}
			else
			{
			*/
			return -1;
			//}
		}
	}
	
	public int ArmyUpgradeEfficiency
	{
		get
		{
			return ConfigInterface.Instance.SystemConfig.UpgradeArmyEfficiency;
		}
	}
	
	public int ItemUpgradeEfficiency
	{
		get
		{
			return ConfigInterface.Instance.SystemConfig.UpgradeItemEfficiency;
		}
	}
	
	public int ArmyUpgradeRemainWorkload
	{
		get
		{
			if(this.m_Data.ArmyUpgrade.HasValue)
			{
				return LogicController.Instance.GetArmyUpgrade
					(this.m_Data.ArmyUpgrade.Value).ProduceRemainingWorkload;
			}
			else
			{
				return -1;
			}
		}
	}
	
	public int ItemUpgradeRemainingWorkload
	{
		get
		{
			if(this.m_Data.ItemUpgrade.HasValue)
			{
				return LogicController.Instance.GetItemUpgrade
					(this.m_Data.ItemUpgrade.Value).ProduceRemainingWorkload;
			}
			else
			{
				return -1;
			}
		}
	}
	
	public int ArmyUpgradeRemainingTime
	{
		get
		{
			if(this.m_Data.ArmyUpgrade.HasValue)
			{
				return Mathf.CeilToInt(this.ArmyUpgradeRemainWorkload / (float)this.ArmyUpgradeEfficiency);
			}
			else
			{
				return -1;
			}
		}
	}
	
	public int ItemUpgradeRemainingTime
	{
		get
		{
			if(this.m_Data.ItemUpgrade.HasValue)
			{
				return Mathf.CeilToInt(this.ItemUpgradeRemainingWorkload / (float)this.ItemUpgradeEfficiency);
			}
			else
			{
				return -1;
			}
		}
	}
	
	public List<KeyValuePair<ItemType, List<ItemIdentity>>> ItemProducts
	{
		get
		{
			if(this.m_Data.ProduceItem == null)
				return null;
			List<KeyValuePair<ItemType, List<ItemIdentity>>> result = new List<KeyValuePair<ItemType, List<ItemIdentity>>>();
			foreach (KeyValuePair<ItemType,List<ItemIdentity>> productList in this.m_Data.ProduceItem) 
			{
				List<ItemIdentity> list = new List<ItemIdentity>();
				foreach(ItemIdentity id in productList.Value)
				{
					list.Add(id);
				}
				result.Add(new KeyValuePair<ItemType, List<ItemIdentity>>(productList.Key,list));
			}
			return result;
		}
	}
	
	public int ItemProductsRemainingTime
	{
		get
		{
			int result = 0;
			if(this.m_Data.ProduceItem == null)
				return result;
			foreach(KeyValuePair<ItemType, List<ItemIdentity>> productList in this.m_Data.ProduceItem)
			{
				foreach(ItemIdentity id in productList.Value)
				{
					result += Mathf.CeilToInt(LogicController.Instance.GetItemObject(id).LogicProduceRemainingWorkload / this.ProduceItemEfficiency);
				}
			}
			return result;
		}
	}
	
	public ItemIdentity[] Items
	{
		get
		{
			if(this.m_Data.AvailableItem == null)
				return null;
			return this.m_Data.AvailableItem.ToArray();
		}
	}
	
	public float AttachedBuilderEfficiency
	{
		get	
		{
			if(this.m_Data.BuilderBuildingNO.HasValue)
			{
				BuildingIdentity id = new BuildingIdentity(BuildingType.BuilderHut, this.m_Data.BuilderBuildingNO.Value);
				return ConfigInterface.Instance.BuilderConfigHelper.GetBuilderData(
					LogicController.Instance.GetBuildingObject(id).Level).BuildEfficiency;
			}
			return -1;
		}
	}
	
	public BuildingEditorState CurrentBuilidngState
	{
		get
		{
			if(this.m_Data.BuilderBuildingNO.HasValue)
			{
				return BuildingEditorState.Update;
			}
			if(this.m_Data.UpgradeRemainingWorkload.HasValue)
			{
				return BuildingEditorState.ReadyForUpdate;
			}
			return BuildingEditorState.Normal;
		}
	}
	
	public BuildingType BuildingType 
	{
		get 
		{
			return this.BuildingIdentity.buildingType;
		}
	}

}
