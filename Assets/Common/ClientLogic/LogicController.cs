using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;
using CommonUtilities;
using CommandConsts;
using System;

public class LogicController 
{
	private static LogicController s_Sigleton;
	
	private BuildingModule m_BuildingModule;
	private PlayerLogicObject m_PlayerModule;
	private ArmyModule m_ArmyModule;
	private ItemModule m_ItemModule;
	private MercenaryModule m_MercenaryModule;
	private ShopModule m_ShopModule;
	private BuilderManager m_BuilderModule;
	private RemovableObjectModule m_RemovableModule;
	private TaskManager m_TaskModule;
	private PropsModule m_PropsModule;
	private DefenseObjectModule m_DefenseModule;
	private AchievementBuildingModule m_AchievementBuildingModule;
	
	private FriendData m_CurrentFriend;
	
	private bool m_IsNewPlayer;

	public LogicController()
	{
	}
	
	public static LogicController Instance
	{
		get
		{
			if(s_Sigleton == null)
			{
				s_Sigleton = new LogicController();
			}
			return s_Sigleton;
		}
	}
	
	public bool IsNewPlayer
	{
		get { return this.m_IsNewPlayer; }
	}
	
	public void ProcessLogic()
	{
		LogicTimer.Instance.Process();
		if(this.m_PlayerModule != null)
		{
			this.m_PlayerModule.Process();
		}
		if(this.m_BuildingModule != null)
		{
			this.m_BuildingModule.Process();
		}
		if(this.m_RemovableModule != null)
		{
			this.m_RemovableModule.Process();
		}
		if(this.m_PropsModule != null)
		{
			this.m_PropsModule.Process();
		}
		if(this.m_TaskModule != null)
		{
			this.m_TaskModule.Process();
		}
		if(this.m_CurrentFriend != null)
		{
			this.m_CurrentFriend.ProcessLogic();
		}
	}
	
	public void Destory()
	{
		this.m_BuilderModule = null;
		this.m_BuildingModule = null;
		this.m_PlayerModule = null;
		this.m_ArmyModule = null;
		this.m_ItemModule = null;
		this.m_ShopModule = null;
		this.m_RemovableModule = null;
		this.m_TaskModule = null;
		this.m_PropsModule = null;
		this.m_DefenseModule = null;
		this.m_AchievementBuildingModule = null;
		this.m_CurrentFriend = null;
	}
	
	public void Initialize(UserData userData, List<BuildingData> buildingData, List<ArmyData> armyData, List<ItemData> itemData,
		List<ObjectUpgrade<ArmyType>> armyUpgrade, List<ObjectUpgrade<ItemType>> itemUpgrade, List<RemovableObjectData> removableObjects,
		List<TaskInformation> tasks, Dictionary<MercenaryIdentity, MercenaryData> mercenaries, Dictionary<int, PropsData> props, List<DefenseObjectData> defenseObjects,
		int removableObjectStartNo, int propsStartNo, List<AchievementBuildingData> achievementBuildings, int achievementBuildingStartNo)
	{
		this.m_BuilderModule = new BuilderManager();
		
		this.m_PlayerModule = new PlayerLogicObject();
		this.m_BuildingModule = new BuildingModule(this.m_BuilderModule);
		this.m_ArmyModule = new ArmyModule();
		this.m_ItemModule = new ItemModule();
		this.m_MercenaryModule = new MercenaryModule();
		this.m_ShopModule = new ShopModule();
		this.m_RemovableModule = new RemovableObjectModule(this.m_BuilderModule);
		
		this.m_MercenaryModule.InitializeMercenaries(mercenaries);
		this.m_ItemModule.InitializeItem(itemData, itemUpgrade);
		this.m_ArmyModule.InitializeArmy(armyData, armyUpgrade);
		this.m_PlayerModule.IntializePlayer(userData);
		this.m_BuildingModule.IntializeBuilding(buildingData);
		this.m_RemovableModule.InitialWithData(removableObjects, removableObjectStartNo);
		
		this.m_BuildingModule.ItemUpgradeFinished += ItemUpgradeFinished;
		this.m_BuildingModule.ArmyUpgradeFinished += ArmyUpgradeFinished;
		
		this.m_IsNewPlayer = (removableObjects.Count == ClientSystemConstants.INITIAL_REMOVABLE_OBJECT_NUMBER) &&
			(removableObjects[0].Position == null);
		
		
		this.m_TaskModule = new TaskManager();
		foreach(TaskInformation info in tasks)
		{
			TaskProgressFactory.PopulateTaskInformation(info);
		}
		this.m_TaskModule.InitialTask(tasks);
		
		this.m_PropsModule = new PropsModule();
		this.m_PropsModule.InitializeProps(props, propsStartNo);
		this.m_DefenseModule = new DefenseObjectModule();
		this.m_DefenseModule.InitialDefenseObject(defenseObjects);
		this.m_AchievementBuildingModule = new AchievementBuildingModule();
		this.m_AchievementBuildingModule.InitialAchievementBuilding(achievementBuildings, achievementBuildingStartNo);
	}
	
	#region AchievementBuilding
	public AchievementBuildingLogicData GetAchievementBuilding(int buildingNo)
	{
		return this.m_AchievementBuildingModule.GetAchievementBuilding(buildingNo).Data;
	}
	
	public AchievementBuildingLogicData BuildAchievementBuilding(AchievementBuildingType achievementBuildingType, TilePosition position)
	{
		AchievementBuildingLogicData result = this.m_AchievementBuildingModule.BuildAchievementBuilding(achievementBuildingType, position);
		List<int> useProps = this.m_PropsModule.BuildAchievementBuilding(achievementBuildingType);
		
		BuildAchievementBuildingRequestParameter request = new BuildAchievementBuildingRequestParameter();
		request.AchievementBuildingType = achievementBuildingType;
		request.PositionRow = position.Row;
		request.PositionColumn = position.Column;
		request.UseProps = useProps;
		CommunicationUtility.Instance.BuildAchievementBuilding(request);
		
		return result;
	}
	
	public void MoveAchievementBuiding(int buildingNo, TilePosition position)
	{
		this.m_AchievementBuildingModule.MoveAchievementBuilding(buildingNo, position);
	}
	
	public void DestroyAchievementBuilding(int buildingNo)
	{
		this.m_AchievementBuildingModule.DestroyAchievementBuilding(buildingNo);
	}
	
	public void RepairAchievementBuilding(int buildingNo)
	{
		List<int> useProps = this.m_PropsModule.RepairAchievementBuilding(this.GetAchievementBuilding(buildingNo));
		this.m_AchievementBuildingModule.RepairAchievementBuilding(buildingNo, useProps.Count);
		
		RepairAchievementBuildingRequestParameter request = new RepairAchievementBuildingRequestParameter();
		request.AchievementBuildingNo = buildingNo;
		request.UseProps = useProps;
		CommunicationUtility.Instance.RepairAchievementBuilding(request);
	}
	#endregion
	
	#region Building
	public BuildingLogicData GetBuildingObject(BuildingIdentity id)
	{
		return this.m_BuildingModule.GetBuildingObject(id);
	}
	
	public BuildingIdentity ConstructBuilding(BuildingType type, int builderBuildingNO, TilePosition position)
	{
		
		this.ConsumeBuildingConstructResource(type);
		BuildingIdentity result = this.m_BuildingModule.ConstructBuilding(type, builderBuildingNO, position);
		return result;
		
		
		//return new BuildingIdentity(BuildingType.GoldMine, 0);
	}
	
	public void MoveBuilding(BuildingIdentity id, TilePosition newPosition) 
	{
		this.m_BuildingModule.MoveBuilding(id, newPosition);
	}
	
	public void UpgradeBuilding(BuildingIdentity id, int builderBuildingNO)
	{
		BuildingLogicData data = this.GetBuildingObject(id);
		this.ConsumeResource(data.UpgradeGold, data.UpgradeFood, data.UpgradeOil, data.UpgradeGem);
		this.m_BuildingModule.UpgradeBuilding(id, builderBuildingNO); 
		//ActorDirector.Instance.SendBuilderToBuild(builderBuildingNO, this.GetBuildingObject(id));
	}
	
	public void CancelBuildingConstruct(BuildingIdentity id)
	{
		BuildingLogicData data = this.GetBuildingObject(id);
		this.m_BuildingModule.CancelBuildingConstruct(id);
		this.ReturnResource(data.UpgradeGold, data.UpgradeFood, data.UpgradeOil, data.UpgradeGem);
	}
	
	public void CancelBuildingUpgrade(BuildingIdentity id)
	{
		BuildingLogicData data = this.GetBuildingObject(id);
		this.m_BuildingModule.CancelBuildingUpgrade(id);
		this.ReturnResource(data.UpgradeGold, data.UpgradeFood, data.UpgradeOil, data.UpgradeGem);
	}
	
	public void FinishBuildingConstruct(BuildingIdentity id)
	{
		int initialLevel = ConfigInterface.Instance.BuildingConfigHelper.GetInitialLevel(id.buildingType);
		
		BuildingConfigData configData = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(id.buildingType, initialLevel);
		if(!configData.CanProduceGold)
		{
			this.m_PlayerModule.AddGoldCapacity(configData.StoreGoldCapacity);
		}
		if(!configData.CanProduceFood)
		{
			this.m_PlayerModule.AddFoodCapacity(configData.StoreFoodCapacity);
		}
		if(!configData.CanProduceOil)
		{
			this.m_PlayerModule.AddOilCapacity(configData.StoreOilCapacity);
		}
		if(configData.CanStoreProps)
		{
			this.m_PlayerModule.AddPropsCapacity(configData.StorePropsCapacity);
		}
		this.m_BuildingModule.FinishBuildingConstruct(id);
		
		BuildingConfigData initialConfigData = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(id.buildingType, 0);
		this.RewardResource(initialConfigData.UpgradeRewardGold, initialConfigData.UpgradeRewardFood, initialConfigData.UpgradeRewardOil, initialConfigData.UpgradeRewardGem, initialConfigData.UpgradeRewardExp);
		
		this.m_TaskModule.OnConstructBuilding(id.buildingType);
	}
	
	public void FinishBuildingUpgrade(BuildingIdentity id)
	{
		BuildingLogicData building = this.m_BuildingModule.GetBuildingObject(id);
		
		BuildingConfigData oldData = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(id.buildingType, building.Level);
		BuildingConfigData newData = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(id.buildingType, building.Level + oldData.UpgradeStep);
		
		if(!newData.CanProduceGold)
		{
			this.m_PlayerModule.AddGoldCapacity(newData.StoreGoldCapacity, oldData.StoreGoldCapacity);
		}
		if(!newData.CanProduceFood)
		{
			this.m_PlayerModule.AddFoodCapacity(newData.StoreFoodCapacity, oldData.StoreFoodCapacity);
		}
		if(!newData.CanProduceOil)
		{
			this.m_PlayerModule.AddOilCapacity(newData.StoreOilCapacity, oldData.StoreOilCapacity);
		}
		if(newData.CanStoreProps)
		{
			this.m_PlayerModule.AddPropsCapacity(newData.StorePropsCapacity, oldData.StorePropsCapacity);
		}
		this.m_BuildingModule.FinishBuildingUpgrade(id);
		
		this.RewardResource(oldData.UpgradeRewardGold, oldData.UpgradeRewardFood, oldData.UpgradeRewardOil, oldData.UpgradeRewardGem,
			oldData.UpgradeRewardExp);
		
		this.TaskManager.OnUpgradeBuilding(id.buildingType, this.m_BuildingModule.GetBuildingObject(id).Level);
	}
	
	public void FinishBuildingUpgradeInstantly(BuildingIdentity id)
	{
		BuildingLogicData data = this.GetBuildingObject(id);
		if(data.AttachedBuilderEfficiency > 0)
		{
			int remainingTime = Mathf.CeilToInt(data.UpgradeRemainingWorkload / data.AttachedBuilderEfficiency);
			int costGem = MarketCalculator.GetUpdateTimeCost(remainingTime);
			
			this.m_PlayerModule.Consume(0,0,0,costGem);
			this.m_BuildingModule.FinishBuildingUpgradeInstantly(id, costGem);
			
			if(data.Level == 0)
			{
				this.FinishBuildingConstruct(id);
			}
			else
			{
				this.FinishBuildingUpgrade(id);
			}
		}
	}
	
	#endregion
	
	#region Resource
	public int Collect(BuildingIdentity id, ResourceType type)
	{
		int quantity = this.m_BuildingModule.Collect(id, type);
		
		int gold = type == ResourceType.Gold ? quantity : 0;
		int food = type == ResourceType.Food ? quantity : 0;
		int oil = type == ResourceType.Oil ? quantity : 0;
		
		this.m_PlayerModule.Receive(gold,food, oil,0);
		this.m_BuildingModule.ReCalculateResource();
		return quantity;
	}
	#endregion
	
	#region Army
	public ArmyLogicObject GetArmyObject(ArmyIdentity id)
	{
		return this.m_ArmyModule.GetArmyObject(id);
	}
	
	public ArmyLogicData GetArmyObjectData(ArmyIdentity id)
	{
		return this.m_ArmyModule.GetArmyObjectData(id);
	}
	
	public ObjectUpgrade<ArmyType> GetArmyUpgrade(ArmyType type)
	{
		return this.m_ArmyModule.GetUpgrade(type);
	}
	
	public ArmyIdentity ProduceArmy(ArmyType type, BuildingIdentity factoryID)
	{
		CostConfigData cost = ConfigInterface.Instance.ArmyConfigHelper.GetProduceCostData(type,this.PlayerData.GetArmyLevel(type));
		this.m_PlayerModule.Consume(cost.CostGold, cost.CostFood, cost.CostOil, cost.CostGem);
		
		int armyLevel = this.m_PlayerModule.Data.GetArmyLevel(type);
		int armyNO = this.m_PlayerModule.Data.GetArmyStartNO(type);
		this.m_PlayerModule.AddArmy(type);
		ArmyIdentity result = this.m_ArmyModule.ProduceArmy(type, armyLevel,armyNO);
		this.m_BuildingModule.ProduceArmy(result, factoryID);
		return result;
	}
	
	public void CancelProduceArmy(ArmyType type, BuildingIdentity factoryID)
	{
		CostConfigData cost = ConfigInterface.Instance.ArmyConfigHelper.GetProduceCostData(type, this.PlayerData.GetArmyLevel(type));
		this.m_PlayerModule.Receive(cost.CostGold, cost.CostFood, cost.CostOil, cost.CostGem);
		this.m_BuildingModule.CancelArmyProduce(type, factoryID);
		this.m_BuildingModule.ReCalculateResource();
	}
	
	public void FinishProduceArmyInstantly(BuildingIdentity factoryID)
	{
		int remainingTime = this.GetBuildingObject(factoryID).ArmyProductsRemainingTime; 
		int costGem = MarketCalculator.GetProduceTimeCost(remainingTime);
		this.m_PlayerModule.Consume(0,0,0,costGem);
		this.m_BuildingModule.FinishArmyProduceInstantly(factoryID);
	}
	
	public void FinishUpgradeArmyInstantly(BuildingIdentity laboratoryID)
	{
		BuildingLogicData data = this.GetBuildingObject(laboratoryID);
		if(data.ArmyUpgrade.HasValue)
		{	
			ArmyType armyType = data.ArmyUpgrade.Value;
			int remainingTime = data.ArmyUpgradeRemainingTime;
			int costGem = MarketCalculator.GetUpdateTimeCost(remainingTime);
			this.m_PlayerModule.Consume(0,0,0,costGem);
			this.m_BuildingModule.FinishArmyUpgradeInstantly(laboratoryID);
			this.m_ArmyModule.FinishUpgrade(armyType);
			this.m_PlayerModule.UpgradeArmy(armyType);
			
			this.m_TaskModule.OnUpgradeArmy(armyType, this.m_PlayerModule.Data.GetArmyLevel(armyType));
		}
	}
	
	public void FinishArmyProduced(ArmyType type)
	{
		this.m_PlayerModule.ProducedArmy(type);
		this.m_TaskModule.OnProduceArmy(type);
	}
	#endregion
	
	#region Item
	public ItemLogicObject GetItemObject(ItemIdentity id)
	{
		return this.m_ItemModule.GetItemObject(id);
	}
	
	public ObjectUpgrade<ItemType> GetItemUpgrade(ItemType type)
	{
		return this.m_ItemModule.GetUpgrade(type);
	}
	
	public void FinishProduceItemInstantly(BuildingIdentity factoryID)
	{
	}
	#endregion
	
	#region Mercenary
	public MercenaryLogicData GetMercenaryData(MercenaryIdentity id)
	{
		return this.m_MercenaryModule.GetMercenaryData(id);
	}
	
	public void HireMercenary(BuildingIdentity tavernID, MercenaryType type)
	{
		int no = this.m_PlayerModule.Data.GetMercenaryStartNO(type);
		MercenaryIdentity mercenaryID = new MercenaryIdentity(type,no);
		this.m_PlayerModule.HireMercenary(type);
		BuildingIdentity campID = this.m_BuildingModule.HireMercenary(mercenaryID, tavernID);
		this.m_MercenaryModule.HireMercenary(mercenaryID, campID);
		
		this.m_TaskModule.OnHireMercenary(type);
	}
	
	public void DropMercenary(MercenaryIdentity id)
	{
		this.m_BuildingModule.DropMercenary(id);
		this.m_MercenaryModule.DropMercenary(id);
	}
	#endregion
	
	#region Player Progress
	public int GetArmyLevel(ArmyType type)
	{
		return this.m_PlayerModule.Data.GetArmyLevel(type);
	}
	
	public int GetItemLevel(ItemType type)
	{
		return this.m_PlayerModule.Data.GetItemLevel(type);
	}
	
	public void UpgradeArmy(ArmyType type, BuildingIdentity laboratoryID)
	{
		int currentLevel = this.m_PlayerModule.Data.GetArmyLevel(type);
		CostConfigData cost = ConfigInterface.Instance.ArmyConfigHelper.GetUpgradeCostData(type, currentLevel);
		this.m_PlayerModule.Consume(cost.CostGold, cost.CostFood, cost.CostOil, cost.CostGem);
		this.m_ArmyModule.UpgradeArmy(type, this.m_PlayerModule.Data.GetArmyLevel(type));
		this.m_BuildingModule.UpgradeArmy(type, laboratoryID, currentLevel);
	}
	
	public void CancelUpgradeArmy(ArmyType type, BuildingIdentity laboratoryID)
	{
		int currentLevel = this.m_PlayerModule.Data.GetArmyLevel(type);
		CostConfigData cost = ConfigInterface.Instance.ArmyConfigHelper.GetUpgradeCostData(type, currentLevel);
		this.m_PlayerModule.Receive(cost.CostGold, cost.CostFood, cost.CostOil, cost.CostGem);
		this.m_BuildingModule.CancelArmyUpgrade(laboratoryID);
	}
	
	public void UpgradeItem(ItemType type)
	{
	}
	
	
	private void ArmyUpgradeFinished(ArmyType type)
	{
		this.m_PlayerModule.UpgradeArmy(type);
		this.m_ArmyModule.FinishUpgrade(type);
		
		this.m_TaskModule.OnUpgradeArmy(type, this.m_PlayerModule.Data.GetArmyLevel(type));
	}
	
	private void ItemUpgradeFinished(ItemType type)
	{
		this.m_PlayerModule.UpgradeItem(type);
		this.m_ItemModule.FinishUpgrade(type);
	}
	#endregion
	
	#region Shop
	public void BuyGold(int gold)
	{
		this.m_PlayerModule.BuyGold(gold);
		this.m_ShopModule.BuyGold(gold);
		this.m_BuildingModule.ReCalculateResource();
	}
	
	public void BuyFood(int food)
	{
		this.m_PlayerModule.BuyFood(food);
		this.m_ShopModule.BuyFood(food);
		this.m_BuildingModule.ReCalculateResource();
	}
	
	public void BuyOil(int oil)
	{
		this.m_PlayerModule.BuyOil(oil);
		this.m_ShopModule.BuyOil(oil);
		this.m_BuildingModule.ReCalculateResource();
	}
	
	public BuildingIdentity BuyBuilderHut(int builderNO, TilePosition position)	
	{
		BuildingIdentity builderHutIdentity = new BuildingIdentity(BuildingType.BuilderHut, builderNO);
		this.ConsumeBuildingConstructResource(BuildingType.BuilderHut);
		this.m_BuildingModule.BuyBuilderHut(builderNO, position);
		this.m_ShopModule.BuyBuilderHut(builderNO, position);
		this.RewardBuildingConstructResource(BuildingType.BuilderHut);
		this.m_TaskModule.OnConstructBuilding(BuildingType.BuilderHut);
		return builderHutIdentity;
	}
	
	public BuildingIdentity BuyBuilderHut(TilePosition position)
	{
		BuilderData[] builderList = this.m_BuilderModule.AllBuilders;
		for(int i = 0; i < ConfigInterface.Instance.SystemConfig.MaxBuilderNumber; i ++)
		{
			if(builderList[i] == null)
			{
				return this.BuyBuilderHut(i, position);
			}
		}
		throw new KeyNotFoundException();
	}
	
	public void BuyUpgradeBuilderHut(int builderNO)
	{
		BuildingIdentity id = new BuildingIdentity(BuildingType.BuilderHut, builderNO);
		BuildingLogicData buildingData = this.GetBuildingObject(id);
		this.ConsumeResource(buildingData.UpgradeGold, buildingData.UpgradeFood, buildingData.UpgradeOil, buildingData.UpgradeGem);
		RewardConfigData rewardData = new RewardConfigData() { RewardGold = buildingData.UpgradeRewardGold,
			RewardFood = buildingData.UpgradeRewardFood, RewardOil = buildingData.UpgradeRewardOil,
			RewardGem = buildingData.UpgradeRewardGem, RewardExp = buildingData.UpgradeRewardExp};
		this.m_BuildingModule.BuyBuilderHutUpgrade(builderNO);
		this.m_ShopModule.BuyBuilderHutUpgrade(builderNO);
		this.RewardResource(rewardData.RewardGold, rewardData.RewardFood, rewardData.RewardOil, rewardData.RewardGem, rewardData.RewardExp);
		this.m_TaskModule.OnUpgradeBuilding(BuildingType.BuilderHut, buildingData.Level);
	}
	
	public BuildingIdentity BuyWall(TilePosition position)
	{
		int wallNO = this.m_BuildingModule.GetBuildingNumber(BuildingType.Wall);
		BuildingIdentity wallIdentity = new BuildingIdentity(BuildingType.Wall, wallNO);
		this.ConsumeBuildingConstructResource(BuildingType.Wall);
		this.m_BuildingModule.BuyWall(wallNO, position);
		this.m_ShopModule.BuyWall(wallNO, position);
		this.RewardBuildingConstructResource(BuildingType.Wall);
		this.m_TaskModule.OnConstructBuilding(BuildingType.Wall);
		return wallIdentity;
	}
	
	public void BuyUpgradeWall(int wallNO)
	{
		BuildingIdentity id = new BuildingIdentity(BuildingType.Wall, wallNO);
		BuildingLogicData buildingData = this.GetBuildingObject(id);
		RewardConfigData rewardData = new RewardConfigData() { RewardGold = buildingData.UpgradeRewardGold,
			RewardFood = buildingData.UpgradeRewardFood, RewardOil = buildingData.UpgradeRewardOil,
			RewardGem = buildingData.UpgradeRewardGem, RewardExp = buildingData.UpgradeRewardExp};
		this.ConsumeResource(buildingData.UpgradeGold, buildingData.UpgradeFood, buildingData.UpgradeOil, buildingData.UpgradeGem);
		this.m_BuildingModule.BuyWallUpgrade(wallNO);
		this.m_ShopModule.BuyWallUpgrade(wallNO);
		this.RewardResource(rewardData.RewardGold, rewardData.RewardFood, rewardData.RewardOil, rewardData.RewardGem, rewardData.RewardExp);
		this.m_TaskModule.OnUpgradeBuilding(BuildingType.Wall, buildingData.Level);
	}
	
	public RemovableObjectLogicData BuyRemovableObject(RemovableObjectType type, TilePosition position)
	{
		ProductRemovableObjectConfigData configData = ConfigInterface.Instance.ProductConfigHelper.GetProductRemovableObject(type);
		this.m_PlayerModule.Consume(0,0,0,configData.GemPrice);
		
		bool isReward = false;
		int propsNo = this.m_PropsModule.GetNextPropsNo();
		RemovableObjectLogicData result = this.m_RemovableModule.BuyRemovableObject(type, position, propsNo, ref isReward).LogicData;
		if(!isReward)
		{
			this.m_PropsModule.ReversePropsNo();
		}
		return result;
	}
	#endregion
	
	#region Accelerate
	
	public void AddResourceAccelerate(BuildingIdentity id)
	{
		int costGem = MarketCalculator.GetResourceAccelerateCostGem(id.buildingType, this.GetBuildingObject(id).Level);
		this.m_PlayerModule.Consume(0,0,0,costGem);
		this.m_BuildingModule.AddResourceAccelerate(id);
	}
	
	public void AddArmyAccelerate(BuildingIdentity id)
	{
		int costGem = ConfigInterface.Instance.SystemConfig.ProduceArmyAccelerateCostGem;
		this.m_PlayerModule.Consume(0,0,0,costGem);
		this.m_BuildingModule.AddArmyAccelerate(id);
	}
	
	public void AddItemAccelerate(BuildingIdentity id)
	{
		int costGem = ConfigInterface.Instance.SystemConfig.ProduceItemAccelerateCostGem;
		this.m_PlayerModule.Consume(0,0,0,costGem);
		this.m_BuildingModule.AddItemAccelerate(id);
	}
	#endregion
	
	#region BuildingManager
	public List<BuildingLogicData> GetBuildings(BuildingType type)
	{
		return this.m_BuildingModule.GetBuildings(type);
	}
	
	public List<BuildingLogicData> GetBuildingsForTypes(HashSet<BuildingType> types)
	{
		return this.m_BuildingModule.GetBuildingsForTypes(types);
	}
	
	public List<BuildingLogicData> GetBuildingsExceptTypes(HashSet<BuildingType> types)
	{
		return this.m_BuildingModule.GetBuildingsExceptTypes(types);
	}
	#endregion
	
	#region Match
	public void FindMatch()
	{
		this.m_PlayerModule.FindMatch();
	}
	
	public void DropArmy(ArmyIdentity id)
	{
		this.m_BuildingModule.DropArmy(id);
		this.m_ArmyModule.DropArmy(id);
	}
	
	public void PlunderResource(int gold, int food, int oil)
	{
		this.m_PlayerModule.Plunder(gold, food, oil);
		this.m_BuildingModule.ReCalculateResource();
		
		if(gold > 0)
		{
			this.m_TaskModule.OnPlunder(ResourceType.Gold, gold);
		}
		if(food > 0)
		{
			this.m_TaskModule.OnPlunder(ResourceType.Food, food);
		}
		if(oil > 0)
		{
			this.m_TaskModule.OnPlunder(ResourceType.Oil, oil);
		}
	}
	
	public void DestroyBuilding(BuildingType buildingType)
	{
		this.m_PlayerModule.DestroyBuilding(buildingType);
		this.m_TaskModule.OnDestroyBuilding(buildingType);
	}
	
	public void AddAttackLog(LogData data)
	{
		this.m_PlayerModule.AddAttackLog(data);
	}
	
	public void Revenge()
	{
		this.m_PlayerModule.Revenge();
	}
	
	public void WinHonour(int winValue)
	{
		this.m_PlayerModule.WinHonour(winValue);
		this.m_TaskModule.OnHonourChanged(this.m_PlayerModule.Data.Honour);
	}
	
	public void LoseHonour(int loseValue)
	{
		this.m_PlayerModule.LoseHonour(loseValue);
		this.m_TaskModule.OnHonourChanged(this.m_PlayerModule.Data.Honour);
	}
	
	public void RewardVictoryResource(int rewardGold, int rewardFood, int rewardOil)
	{
		this.m_PlayerModule.Receive(rewardGold, rewardFood, rewardOil, 0);
	}
	#endregion
	
	#region Friend
	public FriendData CurrentFriend
	{
		get { return this.m_CurrentFriend; } 
		set { this.m_CurrentFriend = value; }
	}
	#endregion
	
	#region Props
	public void GenerateProps(PropsType propsType)
	{
		this.m_PropsModule.GenerateProps(propsType);
	}
	
	public List<PropsLogicData> AllProps
	{
		get { return this.m_PropsModule.AllProps; }
	}
	
	public void DestroyProps(int propsNo)
	{
		this.m_PropsModule.DestroyProps(propsNo);
	}
	
	public PropsLogicData GetProps(int propsNo)
	{
		return this.m_PropsModule.GetPropsLogicData(propsNo).Data;
	}
	
	public void UesAuxilaryProps(int propsNo)
	{
		PropsLogicData data = this.m_PropsModule.GetPropsLogicData(propsNo).Data;
		if(data.FunctionConfigData is PropsBuffConfigData)
		{
			this.m_PlayerModule.AddPlayerBuff(propsNo);
		}
		else if(data.FunctionConfigData is PropsShieldConfigData)
		{
			this.m_PlayerModule.AddPlayerShield(propsNo);
		}
		this.m_PropsModule.UseProps(propsNo);
	}
	
	public void AddPropsInBattle(int propsNo)
	{
		this.m_PropsModule.GetPropsLogicData(propsNo).AddInBattle();
	}
	
	public void RemovePropsInBattle(int propsNo)
	{
		this.m_PropsModule.GetPropsLogicData(propsNo).RemoveInBattle();
	}
	
	public void UsePropsInBattle(int propsNo)
	{
		this.m_PropsModule.UseProps(propsNo);
	}
	#endregion
	
	#region DefenseObject
	public List<DefenseObjectLogicData> AllDefenseObjects
	{
		get { return this.m_DefenseModule.AllDefenseObjects; }
	}
	
	public DefenseObjectLogicData AddDefenseObject(int propsNo, TilePosition position)
	{
		DefenseObjectLogicData result = this.m_DefenseModule.AddDefenseObject(propsNo, position);
		this.m_PropsModule.UseProps(propsNo);
		return result;
	}
	
	public void MoveDefenseObject(long defenseObjectID, TilePosition position)
	{
		this.m_DefenseModule.MoveDefenseObject(defenseObjectID, position);
	}
	
	public void DestroyDefenseObject(long defenseObjectID)
	{
		this.m_DefenseModule.DestroyDefenseObject(defenseObjectID);
	}
	#endregion
	
	#region Removable
	public RemovableObjectLogicData GetRemovableData(int removableObjectNo)
	{
		return this.m_RemovableModule.GetRemovableObjectData(removableObjectNo);
	}
	
	public bool GeneratePosition(int removableObjectNo, IMapData mapData)
	{
		return this.m_RemovableModule.GeneratePosition(removableObjectNo, mapData);
	}
	
	public void Remove(int removableObjectNo, int builderNO)
	{
		RemovableObjectLogicObject logicObject = this.m_RemovableModule.GetRemovableObject(removableObjectNo);
		this.ConsumeResource(logicObject.LogicData.GoldCost, logicObject.LogicData.FoodCost, logicObject.LogicData.OilCost,
			logicObject.LogicData.GemCost);
		logicObject.Remove(builderNO);
		this.m_BuilderModule.SendBuilder(builderNO, logicObject.LogicData);
	}
	
	public void CancelRemove(int removableObjectNo)
	{
		RemovableObjectLogicObject logicObject = this.m_RemovableModule.GetRemovableObject(removableObjectNo);
		this.ReturnResource(logicObject.LogicData.GoldCost, logicObject.LogicData.FoodCost, logicObject.LogicData.OilCost,
			logicObject.LogicData.GemCost);
		this.m_BuilderModule.RecycleBuilder(logicObject.LogicData.CurrentAttachedBuilderNO);
		logicObject.CancelRemove();
		
	}
	
	public RewardConfigData FinishRemove(int removableObjectNo)
	{
		RemovableObjectLogicData logicData = this.m_RemovableModule.GetRemovableObjectData(removableObjectNo);
		this.m_RemovableModule.FinishRemove(removableObjectNo);
		this.RewardResource(0, 0, 0, logicData.RewardGem, logicData.RewardExp);
		
		this.m_TaskModule.OnRemoveObject(logicData.ObjectType);
		this.m_PlayerModule.RemoveObject();
		if(logicData.RewardProps.HasValue)
		{
			this.m_PropsModule.GenerateProps(logicData.RewardProps.Value, logicData.RewardPropsType.Value);
		}
		return new RewardConfigData(){ RewardGem = logicData.RewardGem, RewardExp = logicData.RewardExp };
	}
	#endregion
	
	#region Private Methods
	private void ConsumeResource(int gold, int food, int oil, int gem)
	{
		this.m_PlayerModule.Consume(gold, food, oil, gem);
		this.m_BuildingModule.ReCalculateResource();
	}
	
	private void ReturnResource(int gold, int food, int oil, int gem)
	{
		int returnGold = Mathf.FloorToInt(gold * ConfigInterface.Instance.SystemConfig.ResourceReturnRate);
		int returnFood = Mathf.FloorToInt(food * ConfigInterface.Instance.SystemConfig.ResourceReturnRate);
		int returnOil = Mathf.FloorToInt(oil * ConfigInterface.Instance.SystemConfig.ResourceReturnRate);
		int returnGem = Mathf.FloorToInt(gem * ConfigInterface.Instance.SystemConfig.GemReturnRate);
		this.m_PlayerModule.Receive(returnGold, returnFood, returnOil, returnGem);
		this.m_BuildingModule.ReCalculateResource();
	}
	
	private void RewardResource(int gold, int food, int oil, int gem, int exp)
	{
		this.m_PlayerModule.Receive(gold, food, oil, gem);
		this.m_PlayerModule.AddExp(exp);
		this.m_BuildingModule.ReCalculateResource();
	}
		
	private void ConsumeBuildingConstructResource(BuildingType type)
	{
		CostConfigData costData = ConfigInterface.Instance.BuildingConfigHelper.GetUpgradeCostData(type, 0);
		this.ConsumeResource(costData.CostGold, costData.CostFood, costData.CostOil, costData.CostGem);
	}
	
	private void RewardBuildingConstructResource(BuildingType type)
	{
		RewardConfigData rewardData = ConfigInterface.Instance.BuildingConfigHelper.GetUpgradeRewardData(type, 0);
		this.RewardResource(rewardData.RewardGold, rewardData.RewardFood, rewardData.RewardOil, rewardData.RewardGem, rewardData.RewardExp);
	}
	#endregion
	
	#region Purchase
	public void Purchase(ShopItemInformation shopItem)
	{
		this.m_PlayerModule.Receive(0,0,0,shopItem.GemQuantity);
	}
	#endregion
	
	#region Task
	public void AwardTask(Task task)
	{
		int taskID = task.TaskID;
		TaskConfigData configData = ConfigInterface.Instance.TaskConfigHelper.GetTaskData(taskID);
		this.RewardResource(configData.RewardGold, configData.RewardFood, configData.RewardOil, configData.RewardGem, configData.RewardExp);
		
		AwardTaskRequestParameter request = new AwardTaskRequestParameter();
		request.TaskID = taskID;
		request.OperateTick = LogicTimer.Instance.GetServerTick();
		CommunicationUtility.Instance.AwardTask(request);
		
		this.m_TaskModule.AwardTask(task);
	}
	#endregion
	
	#region Newbie
	public void ChangeName(string newName, Component receiver, string methodName)
	{	
		this.m_PlayerModule.ChangeName(newName);
		ChangeNameRequestParameter request = new ChangeNameRequestParameter();
		request.NewName = newName;
		CommunicationUtility.Instance.ChangeName(request, receiver, methodName, true);
	}
	
	
	public void CompleteNewbieGuide()
	{
		this.m_PlayerModule.CompleteNewbieGuide();
		CommunicationUtility.Instance.CompleteNewbieGuide();
	}

	#endregion
	
	#region Properties
	public TaskManager TaskManager { get { return this.m_TaskModule; } }
	
	public PlayerLogicData PlayerData 
	{ 
		get 
		{ 
			if(this.m_PlayerModule == null)
			{
				return null;
			}
			else
			{
				return this.m_PlayerModule.Data; 
			} 
		} 
	}

	public List<BuildingLogicData> AllBuildings { get { return this.m_BuildingModule.AllBuildings; } }
	public List<AchievementBuildingLogicData> AllAchievementBuildings { get { return this.m_AchievementBuildingModule.AllAchievementBuildings; } }
	public List<RemovableObjectLogicData> AllRemovableObjects { get { return this.m_RemovableModule.AllObjects; } }
	
	public int AvailableBuilderNumber { get { return this.m_BuilderModule.AvailableBuilderNumber; } }
	public int IdleBuilderNumber { get { return this.m_BuilderModule.IdleBuilderNumber; } }
	public int BusyBuilderNumber { get { return this.m_BuilderModule.BusyBuilderNumber; } }
	
	public BuilderData[] AllBuilderInformation{ get { return this.m_BuilderModule.AllBuilders; } }
	
	public int CurrentCityHallLevel
	{
		get
		{
			return this.m_BuildingModule.CurrentCityHallLevel;
		}
	}
	
	public int MaxAttackPropsSlot 
	{ 
		get 
		{ 
			return ConfigInterface.Instance.PropsRestrictionConfigHelper.GetPropsRestrictions(this.CurrentCityHallLevel).MaxAttackPropsSlotNumber;
		}
	}
	
	public int MaxDefenseObjectNumber
	{
		get
		{
			return ConfigInterface.Instance.PropsRestrictionConfigHelper.GetPropsRestrictions(this.CurrentCityHallLevel).MaxDefensePropsSlotNumber;
		}
	}
	
	public int MaxAchievementBuildingNumber
	{
		get
		{
			return ConfigInterface.Instance.PropsRestrictionConfigHelper.GetPropsRestrictions(this.CurrentCityHallLevel).MaxAchievementBuildingNumber;
		}
	}
	
	public int AvailableDefenseObjectNumber{ get { return this.m_DefenseModule.AllDefenseObjects.Count; } }
	public int AvailableBattlePropsNumner 
	{
		get 
		{ 
			int result = 0;
			foreach (long propsID in this.m_PropsModule.AllBattleProps) 
			{
				result ++;
			} 
			return result;
		} 
	} 
	
	public int AvailableAchievementBuildingNumber { get { return this.m_AchievementBuildingModule.AllAchievementBuildings.Count; } }
	
	public IEnumerable<int> AvailableBattleProps { get { return this.m_PropsModule.AllBattleProps; } }
	public IEnumerable<BuffLogicData> AllBuffs { get { return this.m_PlayerModule.Buffs; } }
	
    public int GetBuildingCount(BuildingType buildingType)
    {
        return this.m_BuildingModule.GetBuildingNumber(buildingType);
    }
	public int CampsTotalCapacity { get { return this.m_BuildingModule.CampsTotalArmyCapacity; } }
	public int TotalArmyCapacity { get { return this.m_BuildingModule.TotalArmyCapacity; } }
	
	public int CurrentAvailableArmyCapacity { get { return this.m_BuildingModule.CurrentAvailableArmyCapacity; } }
	
	public ArmyType[] CurrentUpgradingArmies { get { return this.m_ArmyModule.CurrentUpgradeArmies; } }
	
	public List<KeyValuePair<ArmyType, List<ArmyIdentity>>> AvailableArmies { get { return this.m_BuildingModule.CurrentAvailableArmies; } }
	public List<KeyValuePair<ItemType, List<ItemIdentity>>> AvailableItems { get { return this.m_BuildingModule.CurrentAvailableItems; } }
	public List<KeyValuePair<MercenaryType, List<MercenaryIdentity>>> AvailableMercenaries { get { return this.m_BuildingModule.CurrentAvailableMercenaries; } }
	#endregion
}
