using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ConfigUtilities;
using ConfigUtilities.Enums;
using CommandConsts;
using CommonUtilities;

public class BuildingLogicObject : LogicObject
{
	public event Action<BuildingIdentity, List<ProductFacade<ArmyIdentity>>> ArmyProduceFinished;
	public event Action<ArmyType> ArmyUpgradeFinished;
	public event Action<BuildingIdentity, List<ProductFacade<ItemIdentity>>> ItemProduceFinished;
	public event Action<ItemType> ItemUpgradeFinished;
	public event Action<BuildingIdentity> UpgradeTimeUp;
	
	private BuildingData m_Data;
	private BuildingLogicData m_BuildingLogicData;
	
	private int m_AlreadyArmyCapacity;
	private int m_AlreadyItemCapacity;
	private int m_AlreadyProduceArmyCapacity;
	private int m_AlreadyProduceItemCapacity;
	
	private BuildingObjectCommunicationHelper m_CommunicationHelper;

	private ArmyProduceComponent m_ArmyProduceComponent;
	private ItemProduceComponent m_ItemProduceComponent;
	private ArmyUpgradeComponent m_ArmyUpgradeComponent;
	private ItemUpgradeComponent m_ItemUpgradeComponent;
	
	private Dictionary<MercenaryType, MercenaryProduceComponent> m_MercenaryComponents;
	
	private List<ProductFacade<ArmyIdentity>> m_FinishArmyProducts;
	private List<ProductFacade<ItemIdentity>> m_FinishItemProducts;
	
	public BuildingLogicData BuildingLogicData
	{
		get
		{
			return this.m_BuildingLogicData;
		}
	}
	
	public BuildingLogicObject(BuildingData data, int functionMask, bool isNeedCommunicate)
	{
		this.m_CommunicationHelper = isNeedCommunicate ? 
			new BuildingObjectCommunicationHelper() : null;
		this.m_Data = data;
		this.m_BuildingLogicData = new BuildingLogicData(data, this);
		if(data.AvailableArmy != null && (functionMask & ((int)BuildingFunction.StoreArmy)) != 0)
		{
			foreach (ArmyIdentity army in data.AvailableArmy) 
			{
				this.m_AlreadyArmyCapacity += LogicController.Instance.GetArmyObjectData(army).CapacityCost;
			}
		}
		if(data.AvailableMercenary != null && (functionMask & ((int)BuildingFunction.StoreArmy)) != 0)
		{
			foreach (MercenaryIdentity mercenary in data.AvailableMercenary) 
			{
				this.m_AlreadyArmyCapacity += LogicController.Instance.GetMercenaryData(mercenary).CapacityCost;
			}
		}
		if(data.ProduceArmy != null && (functionMask & ((int)BuildingFunction.ProduceArmy)) != 0)
		{
			foreach(KeyValuePair<ArmyType, List<ArmyIdentity>> product in data.ProduceArmy)
			{
				foreach (ArmyIdentity id in product.Value) 
				{
					this.m_AlreadyProduceArmyCapacity += LogicController.Instance.GetArmyObjectData(id).CapacityCost;
				}
			}
		}
		if(data.AvailableItem != null && (functionMask & ((int)BuildingFunction.StoreItem)) != 0)
		{
			this.m_AlreadyItemCapacity = data.AvailableItem.Count;
		}
		if(data.ProduceItem != null && (functionMask & ((int)BuildingFunction.ProduceItem)) != 0)
		{
			foreach(KeyValuePair<ItemType, List<ItemIdentity>> product in data.ProduceItem)
			{
				this.m_AlreadyProduceArmyCapacity += product.Value.Count;
			}
		}
		
		if((functionMask & ((int)BuildingFunction.Update)) != 0)
		{
			BuildingUpgradeLogicComponent upgradeComponent = new BuildingUpgradeLogicComponent();
			this.InitialComponent(upgradeComponent);
			upgradeComponent.UpgradeTimeUp += this.BuildingUpgradeTimeUp;
		}
		
		this.AddComponentAccordingToConfigData(data.ConfigData, functionMask);
		
		if(data.RemainResourceAccelerateTime.HasValue &&  (functionMask & ((int)BuildingFunction.AccelerateResource)) != 0)
		{
			this.AddResourceAccelerateWithoutCommunication();
		}
		if(data.RemainArmyAccelerateTime.HasValue && (functionMask & ((int)BuildingFunction.AccelerateArmy)) != 0)
		{
			this.AddArmyAccelerateWithoutCommunication();
		}
		if(data.RemainItemAccelerateTime.HasValue && (functionMask & ((int)BuildingFunction.AccelerateItem)) != 0)
		{
			this.AddItemAccelerateWithoutCommunication();
		}
	}
	
	public BuildingLogicObject(BuildingData data) : this(data, (int)BuildingFunction.All, true)
	{
	}
	
	private void AddComponentAccordingToConfigData(BuildingConfigData configData, int functionMask)
	{
		if(configData.CanProduceArmy && (functionMask & ((int)BuildingFunction.ProduceArmy)) != 0 
			&& this.m_ArmyProduceComponent == null)
		{
			ArmyProduceComponent comp = new ArmyProduceComponent();
			this.InitialComponent(comp);
			comp.ProduceFinished += this.ArmyProduced;
			this.m_ArmyProduceComponent = comp;
		}
		if(configData.CanProduceItem && (functionMask & ((int)BuildingFunction.ProduceItem)) != 0
			&& this.m_ItemProduceComponent == null)
		{
			ItemProduceComponent comp = new ItemProduceComponent();
			this.InitialComponent(comp);
			comp.ProduceFinished += this.ItemProduced;
			this.m_ItemProduceComponent = comp;
		}
		
		if(configData.CanUpgradeArmy && (functionMask & ((int)BuildingFunction.UpgradeArmy)) != 0
			&& this.m_ArmyUpgradeComponent == null)
		{
			ArmyUpgradeComponent comp = new ArmyUpgradeComponent();
			this.InitialComponent(comp);
			comp.ProduceFinished += this.ArmyUpgraded;
			this.m_ArmyUpgradeComponent = comp;
		}
		if(configData.CanUpgradeItem && (functionMask & ((int)BuildingFunction.UpgradeItem)) != 0
			&& this.m_ItemUpgradeComponent == null)
		{
			ItemUpgradeComponent comp = new ItemUpgradeComponent();
			this.InitialComponent(comp);
			comp.ProduceFinished += this.ItemUpgraded;
			this.m_ItemUpgradeComponent = comp;
		}
		if(this.m_Data.ProduceMercenary != null && (functionMask & ((int)BuildingFunction.ProduceMercenary)) != 0
			&& this.m_MercenaryComponents == null)
		{
			this.m_MercenaryComponents  = new Dictionary<MercenaryType, MercenaryProduceComponent>();
			foreach (MercenaryType type in this.m_Data.ProduceMercenary.Products.Keys) 
			{	
				MercenaryProduceComponent comp = new MercenaryProduceComponent(type);
				this.InitialComponent(comp);
				this.m_MercenaryComponents.Add(type, comp);
			}
		}
	}
	
	private void InitialComponent(BuildingLogicComponent component, int order)
	{
		this.AddComponent(component, order);
		component.Initial(this.m_Data);
	}
	
	private void InitialComponent(BuildingLogicComponent component)
	{
		this.AddComponent(component);
		component.Initial(this.m_Data);
	}
	
	private void BuildingUpgradeTimeUp(float remainingSecond)
	{
		if(this.UpgradeTimeUp != null)
		{
			this.UpgradeTimeUp(this.m_Data.BuildingID);
		}
		this.m_Data.BuilderBuildingNO = null;
		this.m_CommunicationHelper.SendTimeUpUpgradeBuildingRequest(this.m_Data.BuildingID, remainingSecond);
	}
	
	private void ArmyProduced(IProduciable<ArmyIdentity> army, int orderSecond, float remainingSecond)
	{
		ProductFacade<ArmyIdentity> product = new ProductFacade<ArmyIdentity>();
		product.Product = army;
		product.OrderSecond = orderSecond;
		product.RemainingSeond = remainingSecond;
		this.m_FinishArmyProducts.Add(product);
	}
	
	private void ArmyUpgraded(IProduciable<ArmyType> type, int orderSecond, float remainingSecond)
	{
		if(this.ArmyUpgradeFinished != null)
		{
			this.m_Data.ArmyUpgrade = null;
			this.m_CommunicationHelper.SendFinishUpgradeArmyRequest(type.Identity, remainingSecond);
			this.ArmyUpgradeFinished(type.Identity);
		}
		//return true;
	}
	
	private void ItemProduced(IProduciable<ItemIdentity> item, int orderSecond, float remainingSecond)
	{
		ProductFacade<ItemIdentity> product = new ProductFacade<ItemIdentity>();
		product.Product = item;
		product.OrderSecond = orderSecond;
		this.m_FinishItemProducts.Add(product);
	}
	
	private void ItemUpgraded(IProduciable<ItemType> type, int orderSecond, float remainingSecond)
	{
		if(this.ItemUpgradeFinished != null)
		{
			this.ItemUpgradeFinished(type.Identity);
			this.m_CommunicationHelper.SendFinishUpgradeItemRequest(type.Identity);
		}
		//return true;
	}
	
	public void MovePosition(TilePosition newPosition)
	{
		this.m_Data.BuildingPosition = newPosition;
		this.m_CommunicationHelper.SendMoveBuildingRequest(this.m_Data.BuildingID, newPosition);
	}
	
	public void UpgradeBuilding(int builderBuildingNO, float builderEfficiency)
	{
		if(this.m_Data.Level > 0)
		{
			if(this.m_Data.ConfigData.CanProduceGold)
			{
				this.m_Data.CollectedGold = this.m_BuildingLogicData.CurrentStoreGold;
			}
			if(this.m_Data.ConfigData.CanProduceFood)
			{
				this.m_Data.CollectedFood = this.m_BuildingLogicData.CurrentStoreFood;
			}
			if(this.m_Data.ConfigData.CanProduceOil)
			{
				this.m_Data.CollectedOil = this.m_BuildingLogicData.CurrentStoreOil;
			}
		}

		this.m_Data.UpgradeRemainingWorkload = this.m_Data.ConfigData.UpgradeWorkload;
		this.m_Data.BuilderBuildingNO = builderBuildingNO;
		
		this.m_CommunicationHelper.SendUpgradeBuildingRequest(this.m_Data.BuildingID, builderBuildingNO);
	}
	
	public void CancelUpgrade()
	{
		this.m_Data.UpgradeRemainingWorkload = null;
		this.m_Data.BuilderBuildingNO = null;

		if(this.m_Data.Level > 0)
		{
			if(this.m_Data.ConfigData.CanProduceGold)
			{
				this.m_Data.LastCollectedGoldTick = LogicTimer.Instance.GetServerTick();
			}
			if(this.m_Data.ConfigData.CanProduceFood)
			{
				this.m_Data.LastCollectedFoodTick = LogicTimer.Instance.GetServerTick();
			}
			if(this.m_Data.ConfigData.CanProduceOil)
			{
				this.m_Data.LastCollectedOilTick = LogicTimer.Instance.GetServerTick();
			}
		}

		this.m_CommunicationHelper.SendCancelUpgradeBuildingRequest(this.m_Data.BuildingID);
	}
	
	public void FinishUpgrade()
	{
		this.FinishUpgradeWithoutCommunication();
		this.m_CommunicationHelper.SendUpgradeFinishRequest(this.m_Data.BuildingID);
	}
	
	public void FinishUpgradeWithoutCommunication()
	{
		this.m_Data.UpgradeRemainingWorkload = null;
		this.m_Data.BuilderBuildingNO = null;
		
		int nextLevel = this.m_Data.Level == 0 ? this.m_Data.ConfigData.InitialLevel : 
			this.m_Data.Level + this.m_Data.ConfigData.UpgradeStep;
	
		this.m_Data.Level = nextLevel;
		BuildingConfigData configData = ConfigInterface.Instance.BuildingConfigHelper.
			GetBuildingData(this.m_Data.BuildingID.buildingType, this.m_Data.Level);
		this.m_Data.ConfigData = configData;

		if(this.m_Data.Level > 0)
		{
			if(this.m_Data.ConfigData.CanProduceGold)
			{
				this.m_Data.LastCollectedGoldTick = LogicTimer.Instance.GetServerTick();
			}
			if(this.m_Data.ConfigData.CanProduceFood)
			{
				this.m_Data.LastCollectedFoodTick = LogicTimer.Instance.GetServerTick();
			}
			if(this.m_Data.ConfigData.CanProduceOil)
			{
				this.m_Data.LastCollectedOilTick = LogicTimer.Instance.GetServerTick();
			}
		}
		
		this.AddComponentAccordingToConfigData(configData, ((int)BuildingFunction.All));
	}
	
	public void FinishUpgradeInstantly(int costGem)
	{
		this.m_Data.UpgradeRemainingWorkload = 0;
		this.m_Data.BuilderBuildingNO = null;
		this.m_CommunicationHelper.SendUpgradeBuildingInstantlyRequest(this.m_Data.BuildingID, costGem);
	}
	
	public int Collect(ResourceType type)
	{
		int quantity = 0;
		long currentTick = LogicTimer.Instance.GetServerTick();
		switch(type)
		{
			case ResourceType.Gold:
			{
				int total =  this.m_BuildingLogicData.CurrentStoreGold;
				int playerTotal = LogicController.Instance.PlayerData.CurrentStoreGold;
				int playerMaxCapacity = LogicController.Instance.PlayerData.GoldMaxCapacity;
				quantity = Mathf.Min(total, playerMaxCapacity - playerTotal);
				if(quantity > 0)
				{
					this.m_Data.LastCollectedGoldTick = currentTick;
					this.m_Data.CollectedGold = total - quantity;
					this.m_CommunicationHelper.SendCollectGoldRequest(this.m_Data.BuildingID, quantity, total, currentTick);
				}
			}
			break;
			case ResourceType.Food:
			{
				int total = this.m_BuildingLogicData.CurrentStoreFood;
				int playerTotal = LogicController.Instance.PlayerData.CurrentStoreFood;
				int playerMaxCapacity = LogicController.Instance.PlayerData.FoodMaxCapacity;
				quantity = Mathf.Min(total, playerMaxCapacity - playerTotal);
				if(quantity > 0)
				{
					this.m_Data.LastCollectedFoodTick = currentTick;
					this.m_Data.CollectedFood = total - quantity;
					this.m_CommunicationHelper.SendCollectFoodRequest(this.m_Data.BuildingID, quantity, total, currentTick);
				}
			}
			break;
			case ResourceType.Oil:
			{
				int total = this.m_BuildingLogicData.CurrentStoreOil;
				int playerTotal = LogicController.Instance.PlayerData.CurrentStoreOil;
				int playerMaxCapacity = LogicController.Instance.PlayerData.OilMaxCapacity;
				quantity = Mathf.Min(total, playerMaxCapacity - playerTotal);
				if(quantity > 0)
				{
					this.m_Data.LastCollectedOilTick = currentTick;
					this.m_Data.CollectedOil = total - quantity;
					this.m_CommunicationHelper.SendCollectOilRequest(this.m_Data.BuildingID, quantity, total, currentTick);
				}
			}
			break;
		}
		return quantity;
	}
	
	public void ProduceArmy(ArmyIdentity id)
	{		
		bool alreadyHaveThisType = false;
		int order = 0;
		if(this.m_Data.ProduceArmy == null)
		{
			this.m_Data.ProduceArmy = new List<KeyValuePair<ArmyType, List<ArmyIdentity>>>();
		}
		else
		{
			foreach(KeyValuePair<ArmyType, List<ArmyIdentity>> productList in this.m_Data.ProduceArmy)
			{
				order += productList.Value.Count;
				if(productList.Key == id.armyType)
				{
					alreadyHaveThisType = true;
					productList.Value.Add(id);
					break;
				}
			}
		}
		if(!alreadyHaveThisType)
		{
			List<ArmyIdentity> newList = new List<ArmyIdentity>(){id};
			this.m_Data.ProduceArmy.Add(new KeyValuePair<ArmyType, List<ArmyIdentity>>(id.armyType, newList));
		}
		
		ArmyLogicData army = LogicController.Instance.GetArmyObjectData(id);
		this.m_AlreadyProduceArmyCapacity += army.CapacityCost;
		this.m_CommunicationHelper.SendProduceArmyRequest(this.m_Data.BuildingID, id, order);
	}
	
	public void FinishProduceArmyInstantly(List<BuildingIdentity> camps)
	{
		List<ArmyIdentity> armies = new List<ArmyIdentity>();
		int costGem = MarketCalculator.GetProduceTimeCost(this.BuildingLogicData.ArmyProductsRemainingTime);
	
		if(this.m_Data.ProduceArmy != null)
		{
			foreach(KeyValuePair<ArmyType, List<ArmyIdentity>> armyList in this.m_Data.ProduceArmy)
			{
				armies.AddRange(armyList.Value);
			}
			this.m_Data.ProduceArmy.Clear();
		}
		this.m_AlreadyProduceArmyCapacity = 0;
		
		this.m_CommunicationHelper.SendFinishProduceArmyInstantlyRequest(armies, camps, costGem);
	}
	
	public void CancelProduceArmy(ArmyType type)
	{
		foreach(KeyValuePair<ArmyType, List<ArmyIdentity>> productList in this.m_Data.ProduceArmy)
		{
			if(productList.Key == type)
			{
				ArmyLogicData army = LogicController.Instance.GetArmyObjectData(productList.Value[productList.Value.Count - 1]);
				this.m_AlreadyProduceArmyCapacity -= army.CapacityCost;
				productList.Value.RemoveAt(productList.Value.Count - 1);
				if(productList.Value.Count == 0)
				{
					this.m_Data.ProduceArmy.Remove(productList);
				}
				this.m_CommunicationHelper.SendCancelProduceArmyRequest(army.Identity);
				break;
			}
		}
	}
	
	
	
	public void ProduceItem(ItemIdentity id)
	{
		bool alreadyHaveThisType = false;
		int order = 0;
		if(this.m_Data.ProduceItem == null)
		{
			this.m_Data.ProduceItem = new List<KeyValuePair<ItemType, List<ItemIdentity>>>();
		}
		else
		{
			foreach(KeyValuePair<ItemType, List<ItemIdentity>> productList in this.m_Data.ProduceItem)
			{
				order += productList.Value.Count;
				if(productList.Key == id.itemType)
				{
					alreadyHaveThisType = true;
					productList.Value.Add(id);
					break;
				}
			}
		}
		if(!alreadyHaveThisType)
		{
			List<ItemIdentity> newList = new List<ItemIdentity>(){id};
			this.m_Data.ProduceItem.Add(new KeyValuePair<ItemType, List<ItemIdentity>>(id.itemType, newList));
		}
		this.m_CommunicationHelper.SendProduceItemRequest(this.m_Data.BuildingID, id, order);
		this.m_AlreadyProduceItemCapacity ++;
	}
	
	public void CancelProduceItem(ItemType type)
	{
		foreach(KeyValuePair<ItemType, List<ItemIdentity>> productList in this.m_Data.ProduceItem)
		{
			if(productList.Key == type)
			{
				this.m_CommunicationHelper.SendCancelProduceItemRequest(productList.Value[productList.Value.Count - 1]);
				this.m_AlreadyProduceItemCapacity --;
				productList.Value.RemoveAt(productList.Value.Count - 1);
				if(productList.Value.Count == 0)
				{
					this.m_Data.ProduceItem.Remove(productList);
				}
				break;
			}
		}
	}
	
	public void UpgradeArmy(ArmyType armyType,int currentLevel)
	{
		this.m_CommunicationHelper.SendUpgradeArmyRequest(armyType, this.m_Data.BuildingID);
		Debug.Log(armyType.ToString());
		this.m_Data.ArmyUpgrade = armyType;
	}
	
	public void CancelUpgradeArmy()
	{
		if(this.m_Data.ArmyUpgrade.HasValue)
		{
			this.m_CommunicationHelper.SendCancelUpgradeArmyRequest(this.m_Data.ArmyUpgrade.Value);
		}
		this.m_Data.ArmyUpgrade = null;
	}
	
	public void FinishArmyUpgradeInstantly()
	{
		if(this.m_Data.ArmyUpgrade.HasValue)
		{
			this.m_CommunicationHelper.SendFinishUpgradeArmyInstantlyRequest(this.m_Data.ArmyUpgrade.Value,
				MarketCalculator.GetUpdateTimeCost(this.BuildingLogicData.ArmyUpgradeRemainingTime));
		}
		this.m_Data.ArmyUpgrade = null;
	}
	
	public void ModifyResourceStoreage(int? gold, int? food, int? oil)
	{
		if(gold.HasValue)
		{
			this.m_Data.CurrentStoreGold = gold.Value;
		}
		if(food.HasValue)
		{
			this.m_Data.CurrentStoreFood = food.Value;
		}
		if(oil.HasValue)
		{
			this.m_Data.CurrentStoreOil = oil.Value;
		}
	}
	
	public void DropArmy(ArmyIdentity army)
	{
		foreach(ArmyIdentity armyID in this.m_Data.AvailableArmy)
		{
			if(army == armyID)
			{
				ArmyLogicData armyData  = LogicController.Instance.GetArmyObjectData(armyID);
				this.m_Data.AvailableArmy.Remove(army);
				this.m_AlreadyArmyCapacity -= armyData.CapacityCost;
				break;
			}
		}
	}
	
	public void DropMercenary(MercenaryIdentity mercenary)
	{
		foreach (MercenaryIdentity mercenaryID in this.m_Data.AvailableMercenary) 
		{
			if(mercenary == mercenaryID)
			{
				this.m_Data.AvailableMercenary.Remove(mercenary);
				this.m_AlreadyArmyCapacity -= ConfigInterface.Instance.MercenaryConfigHelper.GetMercenaryData(mercenary.mercenaryType).CapcityCost;
				break;
			}
		}
	}
	
	public void AddArmyToCamp(ArmyIdentity army, BuildingIdentity factoryID)
	{
		if(this.m_Data.AvailableArmy == null)
		{
			this.m_Data.AvailableArmy = new List<ArmyIdentity>();
		}
		this.m_Data.AvailableArmy.Add(army);
		
		ArmyLogicObject armyObject = LogicController.Instance.GetArmyObject(army);
		this.m_AlreadyArmyCapacity += armyObject.ArmyLogicData.CapacityCost;
		armyObject.AddArmyToCamp(this.m_Data.BuildingID);
		
		if(Application.loadedLevelName == ClientStringConstants.BUILDING_SCENE_LEVEL_NAME)
		{
			BuildingSceneDirector.Instance.SendArmyToCamp(army.armyType, LogicController.Instance.GetArmyLevel(army.armyType),
				this.m_BuildingLogicData, LogicController.Instance.GetBuildingObject(factoryID));
			AudioController.Play("TrainingFinished");
		}
	}
	
	public void AddMercenaryToCamp(MercenaryIdentity mercenary, BuildingIdentity tavernID)
	{
		if(this.m_Data.AvailableMercenary == null)
		{
			this.m_Data.AvailableMercenary = new List<MercenaryIdentity>();
		}
		this.m_Data.AvailableMercenary.Add(mercenary);
		
		this.m_AlreadyArmyCapacity += ConfigInterface.Instance.MercenaryConfigHelper.GetMercenaryData(mercenary.mercenaryType).CapcityCost;
		if(Application.loadedLevelName == ClientStringConstants.BUILDING_SCENE_LEVEL_NAME)
		{
			BuildingSceneDirector.Instance.SendMercenaryToCamp(mercenary.mercenaryType,
				this.m_BuildingLogicData, LogicController.Instance.GetBuildingObject(tavernID));
			AudioController.Play("TrainingFinished");
		}
	}
	
	public void HireMercenary(MercenaryType type)
	{
		this.m_Data.ProduceMercenary.HireMercenary(type);
	}
	
	public void ReloadNewMercenaryProduct(List<MercenaryType> newProducts)
	{
		this.m_Data.ProduceMercenary.ReloadNewProduct(newProducts);
		
		List<MercenaryType> deletedTypes = new List<MercenaryType>();
		foreach (MercenaryType mercenaryType in m_MercenaryComponents.Keys) 
		{
			if(!this.m_Data.ProduceMercenary.Products.ContainsKey(mercenaryType))
			{
				deletedTypes.Add(mercenaryType);
			}
		}
		foreach (MercenaryType type in deletedTypes) 
		{
			this.RemoveComponent(this.m_MercenaryComponents[type]);
			this.m_MercenaryComponents.Remove(type);
		}
		
		foreach (MercenaryType type in this.m_Data.ProduceMercenary.Products.Keys) 
		{
			if(!this.m_MercenaryComponents.ContainsKey(type))
			{
				MercenaryProduceComponent comp = new MercenaryProduceComponent(type);
				this.InitialComponent(comp);
				this.m_MercenaryComponents.Add(type, comp);
			}
		}
	}
	
	public void AssignHeadArmyProduct()
	{
		ArmyIdentity army = this.m_Data.ProduceArmy[0].Value[0];
		ArmyLogicData armyObject = LogicController.Instance.GetArmyObjectData(army);

		this.m_Data.ProduceArmy[0].Value.RemoveAt(0);
		if(this.m_Data.ProduceArmy[0].Value.Count == 0)
		{
			this.m_Data.ProduceArmy.RemoveAt(0);
		}
		this.m_AlreadyProduceArmyCapacity -= armyObject.CapacityCost;
	}
	
	public void AssignHeadItemProduct()
	{
		this.m_Data.ProduceItem.RemoveAt(0);
		this.m_AlreadyProduceItemCapacity --;
	}
	
	public void AddItemToCamp(ItemIdentity item)
	{
		if(this.m_Data.AvailableItem == null)
		{
			this.m_Data.AvailableItem = new List<ItemIdentity>();
		}
		this.m_Data.AvailableItem.Add(item);
		this.m_AlreadyItemCapacity ++;
	}
	
	private void AddResourceAccelerateWithoutCommunication()
	{
		ResourceAccelerateComponent comp = new ResourceAccelerateComponent();
		this.InitialComponent(comp,0);
		if(!this.m_Data.RemainResourceAccelerateTime.HasValue)
		{
			this.m_Data.RemainResourceAccelerateTime = ConfigInterface.Instance.SystemConfig.ProduceResourceAccelerateLastTime;
		}

		comp.AccelerateFinish += (noAccelerateTime) => 
		{
			this.RemoveComponent(comp);
			long finishTick = LogicTimer.Instance.GetServerTick(noAccelerateTime);
			if(this.m_Data.ConfigData.CanProduceGold)
			{
				float efficiency = this.m_Data.ConfigData.ProduceGoldEfficiency * ConfigInterface.Instance.SystemConfig.ProduceResourceAccelerateScale;
				this.m_Data.LastCollectedGoldTick = finishTick;
				this.m_Data.CollectedGold = CommonUtilities.WorkloadCalculator.CalculateProduceWorkload(this.m_Data.LastCollectedGoldTick.Value,
				                                                                   finishTick,
				                                                                   this.m_Data.CollectedGold,
				                                                                   efficiency,
				                                                                   this.m_Data.ConfigData.StoreGoldCapacity);
			}
			if(this.m_Data.ConfigData.CanProduceFood)
			{
				float efficiency = this.m_Data.ConfigData.ProduceFoodEfficiency * ConfigInterface.Instance.SystemConfig.ProduceResourceAccelerateScale;
				this.m_Data.LastCollectedFoodTick = finishTick;
				this.m_Data.CollectedFood = CommonUtilities.WorkloadCalculator.CalculateProduceWorkload(this.m_Data.LastCollectedFoodTick.Value,
				                                                                                        finishTick,
				                                                                                        this.m_Data.CollectedFood,
				                                                                                        efficiency,
				                                                                                        this.m_Data.ConfigData.StoreFoodCapacity);
			}
			if(this.m_Data.ConfigData.CanProduceOil)
			{
				float efficiency = this.m_Data.ConfigData.ProduceOilEfficiency * ConfigInterface.Instance.SystemConfig.ProduceResourceAccelerateScale;
				this.m_Data.LastCollectedOilTick = finishTick;
				this.m_Data.CollectedOil = CommonUtilities.WorkloadCalculator.CalculateProduceWorkload(this.m_Data.LastCollectedOilTick.Value,
				                                                                                        finishTick,
				                                                                                        this.m_Data.CollectedOil,
				                                                                                        efficiency,
				                                                                                        this.m_Data.ConfigData.StoreOilCapacity);
			}
			this.m_Data.RemainResourceAccelerateTime = null;
			this.m_CommunicationHelper.SendFinishAccelerateRequest(this.m_Data.BuildingID, AccelerateType.Resource, noAccelerateTime);
		};
	}
	
	public void AddResourceAccelerate()
	{
		if(this.m_Data.ConfigData.CanProduceGold)
		{
			this.m_Data.LastCollectedGoldTick = LogicTimer.Instance.GetServerTick();
			this.m_Data.CollectedGold = this.m_BuildingLogicData.CurrentStoreGold;
		}
		if(this.m_Data.ConfigData.CanProduceFood)
		{
			this.m_Data.LastCollectedFoodTick = LogicTimer.Instance.GetServerTick();
			this.m_Data.CollectedFood = this.m_BuildingLogicData.CurrentStoreFood;
		}
		if(this.m_Data.ConfigData.CanProduceOil)
		{
			this.m_Data.LastCollectedOilTick = LogicTimer.Instance.GetServerTick();
			this.m_Data.CollectedOil = this.m_BuildingLogicData.CurrentStoreOil;
		}
		this.AddResourceAccelerateWithoutCommunication();
		this.m_CommunicationHelper.SendAccelerateRequest(this.m_Data.BuildingID, AccelerateType.Resource);
	}
	
	private void AddArmyAccelerateWithoutCommunication()
	{
		ArmyAccelerateComponent comp = new ArmyAccelerateComponent();
		this.InitialComponent(comp,0);
		this.m_ArmyProduceComponent.Accelerate = comp;
		if(!this.m_Data.RemainArmyAccelerateTime.HasValue)
		{
			this.m_Data.RemainArmyAccelerateTime = ConfigInterface.Instance.SystemConfig.ProduceArmyAccelerateLastTime;
		}
		comp.AccelerateFinish += (noAccelerateTime) => 
		{
			this.m_ArmyProduceComponent.ProduceAdvanceTo(LogicTimer.Instance.CurrentTime - noAccelerateTime);
			this.m_ArmyProduceComponent.Accelerate = null;
			this.m_Data.RemainArmyAccelerateTime = null;
			this.m_CommunicationHelper.SendFinishAccelerateRequest(this.m_Data.BuildingID, AccelerateType.Army, noAccelerateTime);
		};
	}
	
	public void AddArmyAccelerate()
	{
		this.AddArmyAccelerateWithoutCommunication();
		this.m_CommunicationHelper.SendAccelerateRequest(this.m_Data.BuildingID, AccelerateType.Army);
	}
	
	public void AddItemAccelerateWithoutCommunication()
	{
		ItemAccelerateComponent comp = new ItemAccelerateComponent();
		this.InitialComponent(comp,0);
		this.m_ItemProduceComponent.Accelerate = comp;
		if(!this.m_Data.RemainItemAccelerateTime.HasValue)
		{
			this.m_Data.RemainItemAccelerateTime = ConfigInterface.Instance.SystemConfig.ProduceItemAccelerateLastTime;
		}
		comp.AccelerateFinish += (noAccelerateTime) => 
		{
			this.m_ItemProduceComponent.ProduceAdvanceTo(LogicTimer.Instance.CurrentTime - noAccelerateTime);
			this.m_ItemProduceComponent.Accelerate = null;
			this.m_Data.RemainItemAccelerateTime = null;
			this.m_CommunicationHelper.SendAccelerateRequest(this.m_Data.BuildingID, AccelerateType.Item);
		};
	}
	
	public void AddItemAccelerate()
	{
		this.AddItemAccelerateWithoutCommunication();
		this.m_CommunicationHelper.SendAccelerateRequest(this.m_Data.BuildingID, AccelerateType.Item);
	}
    
	public int AlreadyArmyCapacity { get { return this.m_AlreadyArmyCapacity; } } 
	public int AlreadyItemCapacity { get { return this.m_AlreadyItemCapacity; } }
	public int AlreadyProduceArmyCapacity { get { return this.m_AlreadyProduceArmyCapacity; } }
	public int AlreadyProduceItemCapacity { get { return this.m_AlreadyProduceItemCapacity; } }
	
	public override void Process ()
	{
		this.m_FinishArmyProducts = new List<ProductFacade<ArmyIdentity>>();
		this.m_FinishItemProducts = new List<ProductFacade<ItemIdentity>>();
		base.Process ();
		if(this.ArmyProduceFinished != null && this.m_FinishArmyProducts.Count > 0)
		{
			this.ArmyProduceFinished(this.m_Data.BuildingID, this.m_FinishArmyProducts);
		}
		if(this.ItemProduceFinished != null && this.m_FinishItemProducts.Count > 0)
		{
			this.ItemProduceFinished(this.m_Data.BuildingID, this.m_FinishItemProducts);
		}
	}
}
