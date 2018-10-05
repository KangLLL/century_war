using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ConfigUtilities.Enums;
using ConfigUtilities;
using CommandConsts;
using CommonUtilities;

public class BuildingModule  
{
	public event Action<ArmyType> ArmyUpgradeFinished;
	public event Action<ItemType> ItemUpgradeFinished;
	
	private Dictionary<BuildingType, Dictionary<int,BuildingLogicObject>> m_Buildings;
	
	private BuilderManager m_BuilderManager;
	private SelfResourceManager m_ResourceManager;
	
	private ArmyCampManager m_ArmyCampManager;
	private ItemCampManager m_ItemCampManager;
	private ArmyFactoryManager m_ArmyFactoryManager;
	private ItemFactoryManager m_ItemFactoryManager;
	
	private BuildingObjectCommunicationHelper m_CommunicationHelper;
	
	private Dictionary<BuildingIdentity, List<ProductFacade<ArmyIdentity>>> m_ProducedArmies;
	private Dictionary<BuildingIdentity, List<ProductFacade<ItemIdentity>>> m_ProducedItems;
	
	public BuildingModule(BuilderManager builderManager)
	{
		this.m_BuilderManager = builderManager;
		this.m_ResourceManager = new SelfResourceManager(this);
		
		this.m_ArmyCampManager = new ArmyCampManager();
		this.m_ItemCampManager = new ItemCampManager();
		this.m_ArmyFactoryManager = new ArmyFactoryManager();
		this.m_ItemFactoryManager = new ItemFactoryManager();
		
		this.m_CommunicationHelper = new BuildingObjectCommunicationHelper();
	}
	
	public List<BuildingLogicData> AllBuildings
	{
		get
		{
			List<BuildingLogicData> result = new List<BuildingLogicData>();
			foreach (KeyValuePair<BuildingType, Dictionary<int,BuildingLogicObject>> building in this.m_Buildings) 
			{
				foreach (KeyValuePair<int,BuildingLogicObject> item in building.Value) 
				{
					result.Add(item.Value.BuildingLogicData);
				}
			}
			return result;
		}
	}
	
	public int CurrentCityHallLevel
	{
		get
		{
			return this.m_Buildings[BuildingType.CityHall][0].BuildingLogicData.Level;
		}
	}
	
	public BuildingLogicData GetBuildingObject(BuildingIdentity id)
	{
		if(this.m_Buildings.ContainsKey(id.buildingType))
		{
			if(this.m_Buildings[id.buildingType].ContainsKey(id.buildingNO))
			{
				return this.m_Buildings[id.buildingType][id.buildingNO].BuildingLogicData;
			}
		}
		return null;
	}
	
	public List<BuildingLogicData> GetBuildings(BuildingType type)
	{
		List<BuildingLogicData> result = new List<BuildingLogicData>();
		if(this.m_Buildings.ContainsKey(type))
		{
			Dictionary<int, BuildingLogicObject> typeDict = this.m_Buildings[type];
			foreach(KeyValuePair<int, BuildingLogicObject> building in typeDict)
			{
				result.Add(building.Value.BuildingLogicData);
			}
		}
		return result;
	}
	
	public List<BuildingLogicData> GetBuildingsForTypes(HashSet<BuildingType> types)
	{
		List<BuildingLogicData> result = new List<BuildingLogicData>();
		foreach (BuildingType type in types) 
		{
			result.AddRange(this.GetBuildings(type));	
		}
		return result;
	}
	
	public List<BuildingLogicData> GetBuildingsExceptTypes(HashSet<BuildingType> types)
	{
		HashSet<BuildingType> validTypes = new HashSet<BuildingType>();
		for(int i = 0; i < (int)BuildingType.Length; i++)
		{
			BuildingType type = (BuildingType)i;
			if(!types.Contains(type))
			{
				validTypes.Add(type);
			}
		}
		return this.GetBuildingsForTypes(validTypes);
	}
	
	public int GetBuildingNumber(BuildingType type)
	{
		if(!this.m_Buildings.ContainsKey(type))
		{
			return 0;
		}
		return this.m_Buildings[type].Count;
	}
	
	public int CampsTotalArmyCapacity
	{
		get { return this.m_ArmyCampManager.CampsTotalCapacity; }
	}
	
	public int TotalArmyCapacity
	{
		get { return this.m_ArmyCampManager.CampsTotalAlreadyCapacity + this.m_ArmyFactoryManager.FactoriesTotalAlreadyCapacity; }
	}
	
	public int CurrentAvailableArmyCapacity
	{
		get { return this.m_ArmyCampManager.CampsTotalAlreadyCapacity; }
	}
	
	public List<KeyValuePair<ArmyType, List<ArmyIdentity>>> CurrentAvailableArmies
	{
		get
		{
			List<KeyValuePair<ArmyType, List<ArmyIdentity>>> result = new List<KeyValuePair<ArmyType, List<ArmyIdentity>>>();
			Dictionary<ArmyType, List<ArmyIdentity>> allArmies = this.m_ArmyCampManager.AvailableObjects;
			foreach (KeyValuePair<ArmyType, List<ArmyIdentity>> armies in allArmies) 
			{
				if(armies.Value.Count > 0)
				{
					result.Add(new KeyValuePair<ArmyType, List<ArmyIdentity>>(armies.Key, armies.Value));
				}
			}
			return result;
		}
	}
	
	public List<KeyValuePair<MercenaryType, List<MercenaryIdentity>>> CurrentAvailableMercenaries
	{
		get
		{
			List<KeyValuePair<MercenaryType, List<MercenaryIdentity>>> result = new List<KeyValuePair<MercenaryType, List<MercenaryIdentity>>>();
			Dictionary<MercenaryType, List<MercenaryIdentity>> allMercenaries = this.m_ArmyCampManager.AvailableMercenaries;
			foreach (KeyValuePair<MercenaryType, List<MercenaryIdentity>> mercenaries in allMercenaries) 
			{
				if(mercenaries.Value.Count > 0)
				{
					result.Add(new KeyValuePair<MercenaryType, List<MercenaryIdentity>>(mercenaries.Key, mercenaries.Value));
				}
			}
			return result;
		}
	}
	
	public List<KeyValuePair<ItemType, List<ItemIdentity>>> CurrentAvailableItems
	{
		get
		{
			List<KeyValuePair<ItemType, List<ItemIdentity>>> result = new List<KeyValuePair<ItemType, List<ItemIdentity>>>();
			Dictionary<ItemType, List<ItemIdentity>> allItems = this.m_ItemCampManager.AvailableObjects;
			foreach (KeyValuePair<ItemType, List<ItemIdentity>> items in allItems) 
			{
				if(items.Value.Count > 0)
				{
					result.Add(new KeyValuePair<ItemType, List<ItemIdentity>>(items.Key, items.Value));
				}
			}
			return result;
		}
	}
	
	public void IntializeBuilding(List<BuildingData> buildings)
	{
		this.m_Buildings = new Dictionary<BuildingType, Dictionary<int, BuildingLogicObject>>();
		List<int> alreadyWorkingBuilder = new List<int>();
		List<BuildingIdentity> buildBuilding = new List<BuildingIdentity>();
		foreach (BuildingData building in buildings) 
		{	
			if(!this.m_Buildings.ContainsKey(building.BuildingID.buildingType))
			{
				this.m_Buildings.Add(building.BuildingID.buildingType, new Dictionary<int,BuildingLogicObject>());
			}
			
			BuildingLogicObject buildingObject = this.ConstructBuildingLogicObject(building);
			this.m_Buildings[building.BuildingID.buildingType].Add(building.BuildingID.buildingNO, buildingObject);
			
			if(building.ConfigData.CanStoreArmy && building.Level > 0)
			{
				this.m_ArmyCampManager.AddCamp(building.BuildingID);
			}
			if(building.ConfigData.CanProduceArmy && building.Level > 0)
			{
				this.m_ArmyFactoryManager.AddFactory(building.BuildingID);
			}
			
			if(building.ConfigData.CanStoreItem && building.Level > 0)
			{
				this.m_ItemCampManager.AddCamp(building.BuildingID);
			}
			if(building.ConfigData.CanProduceItem && building.Level > 0)
			{
				this.m_ItemFactoryManager.AddFactory(building.BuildingID);
			}
			
			if(building.ConfigData.CanStoreGold && building.ConfigData.ProduceGoldEfficiency == 0 && building.Level > 0)
			{
				this.m_ResourceManager.AddStorage(ResourceType.Gold, building.BuildingID, 
					building.ConfigData.StoreGoldCapacity);
			}
			if(building.ConfigData.CanStoreFood && building.ConfigData.ProduceFoodEfficiency == 0 && building.Level > 0)
			{
				this.m_ResourceManager.AddStorage(ResourceType.Food, building.BuildingID,
					building.ConfigData.StoreFoodCapacity);
			}
			if(building.ConfigData.CanStoreOil && building.ConfigData.ProduceOilEfficiency == 0 && building.Level > 0)
			{
				this.m_ResourceManager.AddStorage(ResourceType.Oil, building.BuildingID,
					building.ConfigData.StoreOilCapacity);
			}
			
			if(building.BuildingID.buildingType == BuildingType.BuilderHut && !building.BuilderBuildingNO.HasValue)
			{
				this.m_BuilderManager.AddBuilder(building.BuildingID.buildingNO);
			}
			if(building.BuilderBuildingNO.HasValue)
			{
				alreadyWorkingBuilder.Add(building.BuilderBuildingNO.Value);
				buildBuilding.Add(building.BuildingID);
			}
			/*
			if(building.AvailableArmy != null)
			{
				foreach (ArmyIdentity army in building.AvailableArmy) 
				{
					BuildingSceneDirector.Instance.GenerateArmyInCamp(army.armyType, 
						LogicController.Instance.GetArmyLevel(army.armyType), buildingObject.BuildingLogicData);
				}
			}
			*/
		}
		
		for(int i = 0; i < alreadyWorkingBuilder.Count; i ++)
		{
			int builderNO = alreadyWorkingBuilder[i];
			BuildingIdentity buildingID = buildBuilding[i];
			BuildingLogicData targetInfo = this.m_Buildings[buildingID.buildingType][buildingID.buildingNO].BuildingLogicData;
			this.m_BuilderManager.AddBusyBuilder(builderNO, targetInfo);
		}
		
		this.m_ResourceManager.RecalculateStorage();
	}
	
	
	public void Process()
	{
		this.m_ProducedArmies = new Dictionary<BuildingIdentity, List<ProductFacade<ArmyIdentity>>>();
		this.m_ProducedItems = new Dictionary<BuildingIdentity, List<ProductFacade<ItemIdentity>>>();
		if(this.m_Buildings != null)
		{
			for(int i = 0; i < (int)BuildingType.Length; i ++)
			{
				BuildingType buildingType = (BuildingType)i;
				if(this.m_Buildings.ContainsKey(buildingType))
				{
					Dictionary<int, BuildingLogicObject> dict = m_Buildings[buildingType];
					foreach(int NO in dict.Keys)
					{
						dict[NO].Process();
					}
				}
			}
		}
		this.AssignAllArmyProducts();
		this.AssignAllItemProducts();
	}
	
	#region Assign
	
	private void AssignAllArmyProducts()
	{
		CombineQueue<ProductFacade<ArmyIdentity>> armyQueue = new CombineQueue<ProductFacade<ArmyIdentity>>(this.m_ProducedArmies.Count);
		List<BuildingIdentity> factories = new List<BuildingIdentity>();
		int index = 0;
		foreach(KeyValuePair<BuildingIdentity, List<ProductFacade<ArmyIdentity>>> products in this.m_ProducedArmies)
		{
			factories.Add(products.Key);
			foreach(ProductFacade<ArmyIdentity> p in products.Value)
			{
				armyQueue.Enqueue(p, index);
			}
			index ++;
		}
		while(!armyQueue.IsEmpty)
		{
			int factoryID;
			ProductFacade<ArmyIdentity> product = armyQueue.Peek(out factoryID);
			BuildingIdentity? campID = this.m_ArmyCampManager.FindCamp(product.Product.Identity.armyType);
			if(campID.HasValue)
			{
				this.m_Buildings[campID.Value.buildingType][campID.Value.buildingNO].AddArmyToCamp(product.Product.Identity,factories[factoryID]);
				this.m_Buildings[factories[factoryID].buildingType][factories[factoryID].buildingNO].AssignHeadArmyProduct();
				armyQueue.Dequeue(out factoryID);
				this.m_CommunicationHelper.SendFinishProduceArmyRequest(product.Product.Identity, campID.Value, product.RemainingSeond);
				
				LogicController.Instance.FinishArmyProduced(product.Product.Identity.armyType);
			}
			else
			{
				armyQueue.BlockHeadSubQueue();
			}
		}
		
		foreach(Queue<ProductFacade<ArmyIdentity>> blockQueue in armyQueue.BlockQueues)
		{
			int length = blockQueue.Count;
			for(int i = 0; i < length; i++)
			{
				ProductFacade<ArmyIdentity> product = blockQueue.Dequeue();
				if(i != 0)
				{
					product.Product.Reset();
				}
			}
		}
	}
	
	private void AssignAllItemProducts()
	{
	}
	
	#endregion
	private BuildingLogicObject ConstructBuildingLogicObject(BuildingData data)
	{
		BuildingLogicObject result = new BuildingLogicObject(data);
		result.ArmyProduceFinished += FinishArmyProduced;
		result.ItemProduceFinished += FinishItemProduced;
		result.ArmyUpgradeFinished += FinishArmyUpgraded;
		result.ItemUpgradeFinished += FinishItemUpgraded;
		result.UpgradeTimeUp += (obj) => FreeBuilder(obj);
		return result;
	}
	#region Construct & Upgrade
	public BuildingIdentity ConstructBuilding(BuildingType type, int builderBuildingNO, TilePosition position)
	{
		BuildingData data = new BuildingData();
		BuildingConfigData configData = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(type, 0);
		//int builderHutLevel = this.m_Buildings[BuildingType.BuilderHut][builderBuildingNO].Level;
		//BuilderConfigData builderData = ConfigInterface.Instance.BuilderConfigHelper.GetBuilderData(builderHutLevel);
		
		data.ConfigData = configData;
		int NO = this.m_Buildings.ContainsKey(type) ? this.m_Buildings[type].Count : 0;
		data.BuildingID = new BuildingIdentity(type, NO);
		data.Level = 0;
		data.UpgradeRemainingWorkload = configData.UpgradeWorkload;
		data.BuilderBuildingNO = builderBuildingNO;
		data.BuildingPosition = position;
		
		if(type == BuildingType.Tavern)
		{
			List<MercenaryType> mercenaryTypes = ConfigInterface.Instance.MercenaryConfigHelper.GetAvailableMercenaries(configData.InitialLevel);
			Dictionary<MercenaryType, MercenaryProductData> dict = new Dictionary<MercenaryType, MercenaryProductData>();
			foreach (MercenaryType mercenaryType in mercenaryTypes) 
			{	
				MercenaryProductData productData = new MercenaryProductData();
				MercenaryConfigData config = ConfigInterface.Instance.MercenaryConfigHelper.GetMercenaryData(mercenaryType);
				
				productData.ConfigData = config;
				productData.ReadyNumber = config.MaxProduceNumber;
				dict.Add(mercenaryType, productData);
			}
			data.ProduceMercenary = new MercenaryProductCollectionLogicObject(dict);
		}
		
		if(!this.m_Buildings.ContainsKey(type))
		{
			this.m_Buildings.Add(type, new Dictionary<int, BuildingLogicObject>()); 
		}

		this.m_Buildings[type].Add(data.BuildingID.buildingNO, this.ConstructBuildingLogicObject(data));
		this.m_BuilderManager.SendBuilder(builderBuildingNO, this.GetBuildingObject(data.BuildingID));
		this.m_CommunicationHelper.SendConstructBuildingRequest(data.BuildingID, position, builderBuildingNO);
		
		BuildingIdentity result = new BuildingIdentity();
		result.buildingType = type;
		result.buildingNO = data.BuildingID.buildingNO;
		return result;
	}
	
	public void MoveBuilding(BuildingIdentity id, TilePosition newPosition)
	{
		this.m_Buildings[id.buildingType][id.buildingNO].MovePosition(newPosition);
	}
	
	public void UpgradeBuilding(BuildingIdentity id, int builderBuildingNO)
	{
		int builderLevel = this.m_Buildings[BuildingType.BuilderHut][builderBuildingNO].BuildingLogicData.Level;
		float efficiency = ConfigInterface.Instance.BuilderConfigHelper.GetBuilderData(builderLevel).BuildEfficiency;
		this.m_Buildings[id.buildingType][id.buildingNO].UpgradeBuilding(builderBuildingNO, efficiency);
		this.m_BuilderManager.SendBuilder(builderBuildingNO, this.GetBuildingObject(id));
		this.ReCalculateResource();
	}
	
	public void CancelBuildingConstruct(BuildingIdentity id)
	{
		this.FreeBuilder(id);
		this.m_Buildings[id.buildingType][id.buildingNO].CancelUpgrade();
		this.m_Buildings[id.buildingType].Remove(id.buildingNO);
	}
	
	public void CancelBuildingUpgrade(BuildingIdentity id)
	{
		this.FreeBuilder(id);
		this.m_Buildings[id.buildingType][id.buildingNO].CancelUpgrade();
		this.ReCalculateResource();
	}
	
	public void FinishBuildingConstruct(BuildingIdentity id)
	{
		BuildingLogicObject building = this.m_Buildings[id.buildingType][id.buildingNO];
		building.FinishUpgrade();
		
		if(id.buildingType == BuildingType.BuilderHut)
		{
			this.m_BuilderManager.AddBuilder(id.buildingNO);
		}
		if(building.BuildingLogicData.CanStoreGold && building.BuildingLogicData.ProduceGoldEfficiency == 0)
		{
			this.m_ResourceManager.AddStorage(ResourceType.Gold, id,
				building.BuildingLogicData.StoreGoldCapacity);
		}
		if(building.BuildingLogicData.CanStoreFood && building.BuildingLogicData.ProduceFoodEfficiency == 0)
		{
			this.m_ResourceManager.AddStorage(ResourceType.Food, id,
				building.BuildingLogicData.StoreFoodCapacity);
		}
		if(building.BuildingLogicData.CanStoreOil && building.BuildingLogicData.ProduceOilEfficiency == 0)
		{
			this.m_ResourceManager.AddStorage(ResourceType.Oil, id,
				building.BuildingLogicData.StoreOilCapacity);
		}
		this.ReCalculateResource();
		
		if(building.BuildingLogicData.ArmyCapacity > 0)
		{
			this.m_ArmyCampManager.AddCamp(id);
			this.AssignArmies();
		}
		if(building.BuildingLogicData.ArmyProduceCapacity > 0)
		{
			this.m_ArmyFactoryManager.AddFactory(id);
		}
		if(building.BuildingLogicData.StoreItemCapacity > 0)
		{
			this.AssignItems();
			this.m_ItemCampManager.AddCamp(id);
		}
		if(building.BuildingLogicData.ItemProduceCapacity > 0)
		{
			this.m_ItemFactoryManager.AddFactory(id);
		}
	}
	
	public void FinishBuildingUpgrade(BuildingIdentity id)
	{
		BuildingLogicObject building = this.m_Buildings[id.buildingType][id.buildingNO];
		int oldArmyCapacity = building.BuildingLogicData.ArmyCapacity;
		int oldItemCapacity = building.BuildingLogicData.StoreItemCapacity;
		building.FinishUpgrade();
		int newArmyCapacity = building.BuildingLogicData.ArmyCapacity;
		int newItemCapacity = building.BuildingLogicData.StoreItemCapacity;
		
		if(building.BuildingLogicData.BuildingType == BuildingType.Tavern && 
			building.BuildingLogicData.Level != building.BuildingLogicData.InitialLevel)
		{
			building.ReloadNewMercenaryProduct(ConfigInterface.Instance.MercenaryConfigHelper.
				GetAvailableMercenaries(building.BuildingLogicData.Level));
		}
		
		if(oldArmyCapacity != newArmyCapacity)
		{
			this.AssignArmies();
		}
		if(oldItemCapacity != newItemCapacity)
		{
			this.AssignItems();
		}
	}
	
	public void FinishBuildingUpgradeInstantly(BuildingIdentity id, int costGem)
	{
		this.FreeBuilder(id);
		BuildingLogicObject building = this.m_Buildings[id.buildingType][id.buildingNO];
		building.FinishUpgradeInstantly(costGem);
	}
	#endregion
	
	#region Builder
	public void BuyBuilderHut(int builderNO, TilePosition position)
	{
		this.BuyBuilding(BuildingType.BuilderHut, builderNO, position);
		this.m_BuilderManager.AddBuilder(builderNO);
	}
	
	public void BuyBuilderHutUpgrade(int builderNO)
	{
		this.BuyBuildingUpgrade(BuildingType.BuilderHut, builderNO);
	}
	#endregion
	
	#region Wall
	public void BuyWall(int wallNO, TilePosition position)
	{
		this.BuyBuilding(BuildingType.Wall, wallNO, position);
	}
	
	public void BuyWallUpgrade(int wallNO)
	{
		this.BuyBuildingUpgrade(BuildingType.Wall, wallNO);
	}
	
	private void BuyBuilding(BuildingType type, int buildingNO, TilePosition position)
	{
		BuildingData data = new BuildingData();
		int initialLevel = ConfigInterface.Instance.BuildingConfigHelper.GetInitialLevel(type);
		BuildingConfigData configData = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(type, initialLevel);
		
		data.ConfigData = configData;
		data.BuildingID = new BuildingIdentity(type, buildingNO);
		data.Level = initialLevel;
		data.BuildingPosition = position;
		
		if(!this.m_Buildings.ContainsKey(type))
		{
			this.m_Buildings.Add(type, new Dictionary<int, BuildingLogicObject>()); 
		}
		
		this.m_Buildings[type].Add(data.BuildingID.buildingNO, this.ConstructBuildingLogicObject(data));
		this.ReCalculateResource();
	}
	
	private void BuyBuildingUpgrade(BuildingType type, int buildingNO)
	{
		this.m_Buildings[type][buildingNO].FinishUpgradeWithoutCommunication();
		this.ReCalculateResource();
	}
	
	#endregion
	
	#region Item
	private void FinishItemProduced(BuildingIdentity factoryID, List<ProductFacade<ItemIdentity>> products)
	{
		this.m_ProducedItems.Add(factoryID, products);
	}
	
	private void FinishItemUpgraded(ItemType type)
	{
		if(this.ItemUpgradeFinished != null)
		{
			this.ItemUpgradeFinished(type);
		}
	}
	#endregion
	
	#region Army
	private void FinishArmyProduced(BuildingIdentity factoryID, List<ProductFacade<ArmyIdentity>> products)
	{
		this.m_ProducedArmies.Add(factoryID, products);
	}
	
	private void FinishArmyUpgraded(ArmyType type)
	{
		if(this.ArmyUpgradeFinished != null)
		{
			this.ArmyUpgradeFinished(type);
		}
	}
	
	public void ProduceArmy(ArmyIdentity armyID, BuildingIdentity factoryID)
	{
		BuildingLogicObject factory = this.m_Buildings[factoryID.buildingType][factoryID.buildingNO];
		factory.ProduceArmy(armyID);
		this.ReCalculateResource();
		
	}
	
	public void CancelArmyProduce(ArmyType type, BuildingIdentity factoryID)
	{
		BuildingLogicObject factory = this.m_Buildings[factoryID.buildingType][factoryID.buildingNO];
		factory.CancelProduceArmy(type);
	}
	
	public void UpgradeArmy(ArmyType type, BuildingIdentity laboratoryID, int currentLevel)
	{
		BuildingLogicObject laboratory = this.m_Buildings[laboratoryID.buildingType][laboratoryID.buildingNO];
		laboratory.UpgradeArmy(type, currentLevel);
	 	this.ReCalculateResource();
	}
	
	public void CancelArmyUpgrade(BuildingIdentity laboratoryID)
	{
		BuildingLogicObject laboratory = this.m_Buildings[laboratoryID.buildingType][laboratoryID.buildingNO];
		laboratory.CancelUpgradeArmy();
	 	this.ReCalculateResource();
	}
	
	public void FinishArmyProduceInstantly(BuildingIdentity factoryID)
	{
		BuildingLogicObject factory = this.m_Buildings[factoryID.buildingType][factoryID.buildingNO];
		List<BuildingIdentity> destinations = new List<BuildingIdentity>();
		foreach(KeyValuePair<ArmyType, List<ArmyIdentity>> armies in factory.BuildingLogicData.ArmyProducts)
		{
			foreach (ArmyIdentity army in armies.Value) 
			{
				BuildingIdentity? campID = this.m_ArmyCampManager.FindCamp(armies.Key);
				BuildingLogicObject camp = this.m_Buildings[campID.Value.buildingType][campID.Value.buildingNO];
				camp.AddArmyToCamp(army, factoryID);
				destinations.Add(campID.Value);
				
				LogicController.Instance.FinishArmyProduced(army.armyType);
			}
		}
		factory.FinishProduceArmyInstantly(destinations);
	}
	
	public void FinishArmyUpgradeInstantly(BuildingIdentity laboratoryID)
	{
		BuildingLogicObject laboratory = this.m_Buildings[laboratoryID.buildingType][laboratoryID.buildingNO];
		laboratory.FinishArmyUpgradeInstantly();
	}
	#endregion
	
	#region Mercenary
	public BuildingIdentity HireMercenary(MercenaryIdentity id, BuildingIdentity tavernID)
	{
		BuildingIdentity campID =  this.m_ArmyCampManager.FindMercenaryCamp(id.mercenaryType).Value;
		this.m_Buildings[tavernID.buildingType][tavernID.buildingNO].HireMercenary(id.mercenaryType);
		this.m_Buildings[campID.buildingType][campID.buildingNO].AddMercenaryToCamp(id, tavernID);
		
		HireMercenaryRequestParameter request = new HireMercenaryRequestParameter();
		request.CampType = campID.buildingType;
		request.CampNO = campID.buildingNO;
		request.MercenaryType = id.mercenaryType;
		request.MercenaryNO = id.mercenaryNO;
		request.TavernNO = tavernID.buildingNO;
		request.OperateTick = LogicTimer.Instance.GetServerTick();
		CommunicationUtility.Instance.HireMercenary(request);
		
		return campID;
	}
	#endregion
	
	#region Accelerate
	
	public void AddResourceAccelerate(BuildingIdentity id)
	{
		this.m_Buildings[id.buildingType][id.buildingNO].AddResourceAccelerate();
	}
	
	public void AddArmyAccelerate(BuildingIdentity id)
	{
		this.m_Buildings[id.buildingType][id.buildingNO].AddArmyAccelerate();
	}
	
	public void AddItemAccelerate(BuildingIdentity id)
	{
		this.m_Buildings[id.buildingType][id.buildingNO].AddItemAccelerate();
	}
	#endregion
	
	public void ReCalculateResource()
	{
		this.m_ResourceManager.RecalculateStorage();
	}
	
	private void AssignArmies()
	{
		Dictionary<BuildingIdentity, ArmyIdentity> producedProducts = this.m_ArmyFactoryManager.CurrentFinishProducts;
		if(producedProducts != null)
		{
			foreach(KeyValuePair<BuildingIdentity, ArmyIdentity> product in producedProducts)
			{
				BuildingIdentity? camp = this.m_ArmyCampManager.FindCamp(product.Value.armyType);
				if(camp.HasValue)
				{
					this.m_Buildings[product.Key.buildingType][product.Key.buildingNO].AssignHeadArmyProduct();
					this.m_Buildings[camp.Value.buildingType][camp.Value.buildingNO].AddArmyToCamp(product.Value, product.Key);
					this.m_CommunicationHelper.SendFinishProduceArmyRequest(product.Value, camp.Value, 0);
				}
			}
		}
	}
	
	private void AssignItems()
	{
		Dictionary<BuildingIdentity, ItemIdentity> producedProducts = this.m_ItemFactoryManager.CurrentFinishProducts;
		if(producedProducts != null)
		{
			foreach(KeyValuePair<BuildingIdentity, ItemIdentity> product in producedProducts)
			{
				BuildingIdentity? camp = this.m_ItemCampManager.FindCamp(product.Value.itemType);
				if(camp.HasValue)
				{
					this.m_Buildings[product.Key.buildingType][product.Key.buildingNO].AssignHeadItemProduct();
					this.m_Buildings[camp.Value.buildingType][camp.Value.buildingNO].AddItemToCamp(product.Value);
				}
			}
		}
	}
	
	public int Collect(BuildingIdentity id, ResourceType type)
	{
		int quantity = this.m_Buildings[id.buildingType][id.buildingNO].Collect(type);
		return quantity;
	}
	
	private void FreeBuilder(BuildingIdentity id)
	{
		BuildingLogicData building = this.m_Buildings[id.buildingType][id.buildingNO].BuildingLogicData;
		this.m_BuilderManager.RecycleBuilder(building.CurrentAttachedBuilderNO);
	}
	
	#region Resource
	public void ModifyResourceStoreage(BuildingIdentity id, int? gold, int? food, int? oil)
	{
		this.m_Buildings[id.buildingType][id.buildingNO].ModifyResourceStoreage(gold, food, oil);
	}
	#endregion
	
	#region Match
	public void DropArmy(ArmyIdentity id)
	{
		ArmyLogicData armyData = LogicController.Instance.GetArmyObjectData(id);
		this.m_Buildings[armyData.CampID.buildingType][armyData.CampID.buildingNO].DropArmy(id);
		this.AssignArmies();
	}
	
	public void DropMercenary(MercenaryIdentity id)
	{
		MercenaryLogicData mercenaryData = LogicController.Instance.GetMercenaryData(id);
		this.m_Buildings[mercenaryData.CampID.buildingType][mercenaryData.CampID.buildingNO].DropMercenary(id);
		this.AssignArmies();
	}
	#endregion
}
