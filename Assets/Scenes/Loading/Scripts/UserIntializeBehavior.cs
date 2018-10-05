using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommandConsts;
using ConfigUtilities;
using ConfigUtilities.Structs;
using ConfigUtilities.Enums;

public class UserIntializeBehavior : MonoBehaviour 
{
    [SerializeField]
    private UISlider m_ProgressBar;
	[SerializeField]
	private WaitingBehavior m_WaitingBehavior;

	private bool m_IsUserLogined;
	
	public void StartInitialize()
	{
		LoginRequestParameter parameter = new LoginRequestParameter();
		if(CommonHelper.PlatformType == PlatformType.Nd)
		{
			parameter.NdID = NdCenter.Instace.NdID;
		}
		else if(CommonHelper.PlatformType == PlatformType.iOS)
		{
			parameter.AccountID = iOSCenter.Instance.AccountID;
		}
		
		CommunicationUtility.Instance.GetUserData(this, "ReceivedUserData", false, parameter);
		this.StartCoroutine("InitialUserData");
	}
	
	IEnumerator InitialUserData()
	{
		while(!this.m_IsUserLogined)
		{
			yield return null;
		}
		ClientNotification.Instance.SendToken();
        this.m_ProgressBar.sliderValue = 1.0f;
		if(Time.timeScale != 0)
		{
			if(LogicController.Instance.PlayerData.IsRegisterSuccessful)
			{
				Application.LoadLevel(ClientStringConstants.BUILDING_SCENE_LEVEL_NAME);
			}
			else
			{
				Application.LoadLevel(ClientStringConstants.CG_SCENE_LEVEL_NAME);
			}
			ArmyLevelObserver.Instance.StartObserve();
			TaskStatusObserver.Instance.StartObserve();
		}
	}
	
	private void ReceivedUserData(Hashtable result)
	{
		Debug.Log("received");  
		
		LoginResponseParameter response = new LoginResponseParameter();
		response.InitialParameterObjectFromHashtable(result);
		if(response.FailType.HasValue)
		{
			Debug.Log(response.FailType.ToString());
			if(response.FailType == LoginFailType.BeAttacked)
			{
				this.m_ProgressBar.gameObject.SetActive(false);
				this.m_WaitingBehavior.Show();
			}
		}
		else
		{	
			LoginSuccessResponseParameter paramter = response.Response;
			
			if(!string.IsNullOrEmpty(paramter.PlayerAccountID))
			{
				if(CommonHelper.PlatformType == PlatformType.Nd)
				{
					NdAccountUtility.Instance.InitialAccount(paramter.PlayerAccountID);
				}
			}
			
			PlayerConfigData playerConfigData = ConfigUtilities.ConfigInterface.Instance.PlayerConfigHelper.GetPlayerData(paramter.PlayerLevel);  
			
			UserData userData = new UserData();
			userData.PlayerID = paramter.PlayerID;
			userData.Name = paramter.PlayerName;
			userData.Level = paramter.PlayerLevel;
			userData.Exp = paramter.PlayerExp;
			userData.ConfigData = playerConfigData;
			userData.Honour = paramter.PlayerHonour;
			userData.CurrentStoreGold = paramter.PlayerGold;
			userData.CurrentStoreFood = paramter.PlayerFood;
			userData.CurrentStoreOil = paramter.PlayerOil;
			userData.CurrentStoreGem = paramter.PlayerGem;
			userData.RemainingCD = paramter.PlayerShieldRemainSecond;
			userData.PlunderTotalGold = paramter.PlunderTotalGold;
			userData.PlunderTotalFood = paramter.PlunderTotalFood;
			userData.PlunderTotalOil = paramter.PlunderTotalOil;
			userData.RemoveTotalObject = paramter.RemoveTotalObject;
			userData.IsRegisterSuccessful = paramter.IsRegistered;
			userData.IsNewbie = paramter.IsNewbie;
			userData.DestoryBuildings = new Dictionary<BuildingType, int>();
		    for(int i = 0; i < (int)BuildingType.Length; i ++)
			{
				BuildingType type = (BuildingType)i;
				if(paramter.DestroyBuildingDict != null && paramter.DestroyBuildingDict.ContainsKey(type))
				{
					userData.DestoryBuildings.Add(type, paramter.DestroyBuildingDict[type]);
				}
				else
				{
					userData.DestoryBuildings.Add(type, 0);
				}
			}
			userData.ProduceArmies = new Dictionary<ArmyType, int>();
			for(int i = 0; i < (int)ArmyType.Length; i ++)
			{
				ArmyType type = (ArmyType)i;
				if(paramter.ProduceArmyDict != null && paramter.ProduceArmyDict.ContainsKey(type))
				{
					userData.ProduceArmies.Add(type, paramter.ProduceArmyDict[type]);
				}
				else
				{
					userData.ProduceArmies.Add(type, 0);
				}
			}
			
			userData.ArmyProgress = new Dictionary<ArmyType, ProgressInformation>();
			foreach (ArmyProgressParameter param in paramter.ArmiesProgress) 
			{
				ProgressInformation progressInfo = new ProgressInformation()
				{ Level = param.ArmyLevel, StartNo = param.ArmyStartNO };
				userData.ArmyProgress.Add(param.ArmyType, progressInfo);
			}
			
			userData.ItemProgress = new Dictionary<ItemType, ProgressInformation>();
			foreach (ItemProgressParameter param in paramter.ItemsProgress) 
			{
				ProgressInformation progressInfo = new ProgressInformation()
				{ Level = param.ItemLevel, StartNo = param.ItemStartNO };
				userData.ItemProgress.Add(param.ItemType, progressInfo);
			}
			
			userData.MercenaryProgress = new Dictionary<MercenaryType, int>();
			foreach (MercenaryProgressParameter param in paramter.MercenaryProgress) 
			{
				userData.MercenaryProgress.Add(param.MercenaryType, param.MercenaryStartNO);
			}
			
			userData.AttackLogs = new List<LogData>();
			foreach(MatchLogParameter param in paramter.AttackLog)
			{
				userData.AttackLogs.Add(this.GenerateLogData(param));
			}
			
			userData.DefenseLogs = new List<LogData>();
			foreach(MatchLogParameter param in paramter.DefenseLog)
			{
				userData.DefenseLogs.Add(this.GenerateLogData(param));
			}
			
			userData.PlayerBuffs = new List<BuffData>();
			foreach (PlayerBuffParameter param in paramter.Buffs) 
			{
				BuffData buffData = new BuffData();
				buffData.RelatedPropsType = param.RelatedProps;
				buffData.RemainingCD = param.RemainingTime;
				buffData.BuffConfigData = ConfigInterface.Instance.PropsConfigHelper.GetPropsData(param.RelatedProps).FunctionConfigData as PropsBuffConfigData;
				userData.PlayerBuffs.Add(buffData);
			}
			
			List<BuildingData> buildings = new List<BuildingData>();
			Dictionary<int, BuildingData>  builderHuts = new Dictionary<int, BuildingData>();
			
			List<ArmyData> armies = new List<ArmyData>();
			List<ItemData> items = new List<ItemData>();
			List<ObjectUpgrade<ArmyType>> armyUpgrades = new List<ObjectUpgrade<ArmyType>>();
			List<ObjectUpgrade<ItemType>> itemUpgrades = new List<ObjectUpgrade<ItemType>>();
			Dictionary<MercenaryIdentity, MercenaryData> mercenaries = new Dictionary<MercenaryIdentity, MercenaryData>();
			foreach (BuildingParameter param in paramter.Buildings) 
			{
				BuildingData data = this.GenerateBuildingData(param, armies, items, mercenaries, armyUpgrades, itemUpgrades, userData);
				buildings.Add(data);
				if(data.BuildingID.buildingType == BuildingType.BuilderHut)
				{
					builderHuts.Add(data.BuildingID.buildingNO, data);
				}
			}
			
			for (int i = 0; i < buildings.Count; i++) 
			{
				BuildingData buildingData = buildings[i];
				BuildingParameter param = paramter.Buildings[i];
				if(param.BuilderBuildingNO.HasValue)
				{
					/*
					int builderLevel = builderHuts[param.BuilderBuildingNO.Value].Level;
					BuilderConfigData builderData = ConfigInterface.Instance.BuilderConfigHelper.GetBuilderData(builderLevel);
					buildingData.ActorWorkEfficiency = builderData.BuildEfficiency;
					*/
					buildingData.BuilderBuildingNO = param.BuilderBuildingNO.Value;
				}
				
				if(buildingData.ConfigData.ProduceGoldEfficiency == 0 && buildingData.Level > 0)
				{
					userData.GoldMaxCapacity += buildingData.ConfigData.StoreGoldCapacity;
				}
				if(buildingData.ConfigData.ProduceFoodEfficiency == 0 && buildingData.Level > 0)
				{
					userData.FoodMaxCapacity += buildingData.ConfigData.StoreFoodCapacity;
				}
				if(buildingData.ConfigData.ProduceOilEfficiency == 0 && buildingData.Level > 0)
				{
					userData.OilMaxCapacity += buildingData.ConfigData.StoreOilCapacity;
				}
				if(buildingData.ConfigData.CanStoreProps)
				{
					userData.PropsMaxCapacity += buildingData.ConfigData.StorePropsCapacity;
				}
			}
			
			foreach (BuildingData buildingData in buildings) 
			{
				if(buildingData.ConfigData.StoreGoldCapacity > 0 && buildingData.ConfigData.ProduceGoldEfficiency == 0)
				{
					buildingData.CurrentStoreGold = userData.CurrentStoreGold * buildingData.ConfigData.StoreGoldCapacity / userData.GoldMaxCapacity; 
				}
				if(buildingData.ConfigData.StoreFoodCapacity > 0 && buildingData.ConfigData.ProduceFoodEfficiency == 0)
				{
					buildingData.CurrentStoreFood = userData.CurrentStoreFood * buildingData.ConfigData.StoreFoodCapacity / userData.FoodMaxCapacity;
				}
				if(buildingData.ConfigData.StoreOilCapacity > 0 && buildingData.ConfigData.ProduceOilEfficiency == 0)
				{
					buildingData.CurrentStoreOil = userData.CurrentStoreOil * buildingData.ConfigData.StoreOilCapacity / userData.OilMaxCapacity;
				}
			}
			
			List<RemovableObjectData> removableObjects = new List<RemovableObjectData>();
			foreach(RemovableObjectParameter param in paramter.RemovableObjects)
			{
				removableObjects.Add(this.GenerateRemovableData(param));
			}
			
			List<TaskInformation> tasks = new List<TaskInformation>();
			if(paramter.OpenedTasks != null)
			{
				foreach (OpeningTaskParameter task in paramter.OpenedTasks) 
				{
					TaskInformation taskInfo = new TaskInformation();
					taskInfo.TaskID = task.TaskID;
					taskInfo.Status = TaskStatus.Opened;
					taskInfo.RemainingSeconds = task.RemainingSeconds;
					if(task.ConditionProgresses != null)
					{
						taskInfo.ConditionProgresses = new Dictionary<int, TaskProgressInformation>();
						foreach(ConditionProgressParameter conditionProgress in task.ConditionProgresses)
						{
							taskInfo.ConditionProgresses.Add(conditionProgress.ConditionID, new TaskProgressInformation()
							{ StartValue = conditionProgress.StartValue });
						}
					}
					tasks.Add(taskInfo);
				}
			}
			if(paramter.CompletedTasks != null)
			{
				foreach (int taskID in paramter.CompletedTasks) 
				{
					TaskInformation taskInfo = new TaskInformation();
					taskInfo.TaskID = taskID;
					taskInfo.Status = TaskStatus.Completed;
					tasks.Add(taskInfo);
				}
			}
			
			Dictionary<int, PropsData> props = new Dictionary<int, PropsData>();
			foreach (PropsParameter p in paramter.Props)
			{
				PropsData data = new PropsData();
				data.RemainingCD = p.RemainingCD.HasValue ? p.RemainingCD.Value : 0;
				data.RemainingUseTime = p.RemainingUseTimes;
				data.PropsType = p.PropsType;
				data.PropsConfigData = ConfigInterface.Instance.PropsConfigHelper.GetPropsData(p.PropsType);
				data.PropsNo = p.PropsNo;
				data.IsInBattle = p.IsInBattle;
				props.Add(p.PropsNo, data);
			}
			
			List<DefenseObjectData> defenseObjects = new List<DefenseObjectData>();
			foreach (DefenseObjectParameter d in paramter.DefenseObjects) 
			{
				DefenseObjectData data = new DefenseObjectData();
				data.Position = new TilePosition(d.PoistionColumn, d.PositionRow);
				data.DefenseObjectID = d.DefenseObjectID;
				data.Name = ConfigInterface.Instance.PropsConfigHelper.GetPropsData(d.RelatedProps).Name;
				data.ConfigData = new DefenseObjectConfigWrapper(ConfigInterface.Instance.PropsConfigHelper.GetPropsData(d.RelatedProps).FunctionConfigData);
				defenseObjects.Add(data);
			}
			
			List<AchievementBuildingData> achievementBuildings = new List<AchievementBuildingData>();
			foreach (AchievementBuildingParameter a in paramter.AchievementBuildings) 
			{
				AchievementBuildingData data = new AchievementBuildingData();
				data.AchievementBuildingType = a.AchievementBuildingType;
				data.BuildingNo = a.AchievementBuildingNo;
				data.BuildingPosition = new TilePosition(a.PositionColumn, a.PositionRow);
				data.Life = a.Life;
				data.ConfigData = ConfigInterface.Instance.AchievementBuildingConfigHelper.GetAchievementBuildingData(a.AchievementBuildingType);
				achievementBuildings.Add(data);
			}
			
			LogicController.Instance.Initialize(userData, buildings, armies, items, armyUpgrades, itemUpgrades, removableObjects, tasks, mercenaries, props, defenseObjects,
				paramter.RemovableObjectStartNo, paramter.PropsStartNo,achievementBuildings,paramter.AchievementBuildingStartNo);
			LogicTimer.Instance.InitialTimer(paramter.ServerTick);
			this.m_IsUserLogined = true;
		}
	}

	private RemovableObjectData GenerateRemovableData(RemovableObjectParameter param)
	{
		RemovableObjectData removableData = new RemovableObjectData();
		RemovableObjectConfigData configData = ConfigInterface.Instance.RemovableConfigHelper.GetRemovableObjectData(param.RemovableObjectTypeID);
		if(param.PositionColumn.HasValue && param.PositionRow.HasValue)
		{
			removableData.Position = new TilePosition(param.PositionColumn.Value, param.PositionRow.Value);
		}
		removableData.ConfigData = configData;
		removableData.BuilderBuildingNO = param.BuilderBuildingNO;
		removableData.RemainingWorkload = param.RemainWorkload;
		removableData.RemovableObjectNo = param.RemovableObjectNo;
		removableData.RemovableObjectType = param.RemovableObjectTypeID;
		if(param.RewardExp.HasValue)
		{
			removableData.RewardExp = param.RewardExp.Value;
			removableData.RewardGem = param.RewardGem.Value;
			removableData.RewardProps = param.RewardProps;
			removableData.RewardPropsType = param.RewardPropsType;
		}
		return removableData;
	}
	
	private LogData GenerateLogData(MatchLogParameter param)
	{
		LogData logData = new LogData();
		logData.IsDestroyCityHall = param.IsDestroyCityHall;
		logData.DestroyBuildingPercentage = param.DestroyPercentage;
		logData.MatchID = param.MatchID;
		logData.RivalID = param.RivalID;
		logData.RivalName = param.RivalName;
		logData.RivalHonour = param.RivalHonour;
		logData.RivalLevel = param.RivalLevel;
		logData.PlunderGold = param.PlunderGold;
		logData.PlumderFood = param.PlunderFood;
		logData.PlunderOil = param.PlunderOil;
		logData.PlunderHonour = param.PlunderHonour;
		logData.PlunderProps = param.PlunderProps;
		logData.PropsThropy = param.PropsTrophyList;
		logData.ElapsedTime = param.ElapsedSecond;
		logData.CanRevenge = param.CanRevenge;
		logData.ArmyInfos = new Dictionary<ArmyType, DropArmyInfo>();
		foreach(DropArmyParameter armyParam in param.ArmyList)	
		{
			logData.ArmyInfos.Add(armyParam.ArmyType, new DropArmyInfo(){ Quantity = armyParam.Number, Level = armyParam.ArmyLevel} );
		}
		logData.MercenaryInfos = new Dictionary<MercenaryType, int>();
		foreach (DropMercenaryParameter mercenaryParam in param.MercenaryList) 
		{
			logData.MercenaryInfos.Add(mercenaryParam.MercenaryType, mercenaryParam.Number);
		}
		logData.PropsInfos = new List<KeyValuePair<PropsType, int>>();
		foreach (UsePropsParameter propsParam in param.PropsList) 
		{
			logData.PropsInfos.Add(new KeyValuePair<PropsType, int>(propsParam.PropsType, propsParam.Number));
		}
		logData.Version = param.Version;
		return logData;
	}
	
	private ArmyData GenerateArmyData(ArmyProductParameter param, UserData userData)
	{
		ArmyData armyData = new ArmyData();
		armyData.ArmyID = new ArmyIdentity(param.ArmyType, param.ArmyNO);
		armyData.ProduceRemainingWorkload = param.RemainingWorkload.HasValue ? param.RemainingWorkload.Value : 0;
		armyData.ConfigData = ConfigInterface.Instance.ArmyConfigHelper.
			GetArmyData(param.ArmyType, userData.ArmyProgress[param.ArmyType].Level);
		return armyData;
	}
	
	private ArmyData GenerateArmyData(ArmyParameter param, UserData userData, BuildingIdentity campID)
	{
		ArmyData armyData = new ArmyData();
		armyData.ArmyID = new ArmyIdentity(param.ArmyType, param.ArmyNO);
		armyData.ProduceRemainingWorkload = 0;
		armyData.CampID = campID;
		armyData.ConfigData = ConfigInterface.Instance.ArmyConfigHelper.
			GetArmyData(param.ArmyType, userData.ArmyProgress[param.ArmyType].Level);
		return armyData;
	}
	
	private ItemData GenerateItemData(ItemProductParameter param, UserData userData)
	{
		ItemData itemData = new ItemData();
		itemData.ItemID = new ItemIdentity(param.ItemType, param.ItemNO);
		itemData.ProduceRemainingWorkload = param.RemainingWorkload.HasValue ? param.RemainingWorkload.Value : 0;
		itemData.ConfigData = null;//ConfigInterface.Instance.ItemConfigHelper.
			//GetItemData(param.ItemType, userData.ItemProgress[param.ItemType].Level);
		return itemData;
	}
	
	private ItemData GenerateItemData(ItemParameter param, UserData userData)
	{
		ItemData itemData = new ItemData();
		itemData.ItemID = new ItemIdentity(param.ItemType, param.ItemNO);
		itemData.ProduceRemainingWorkload = 0;
		itemData.ConfigData = null;//ConfigInterface.Instance.ItemConfigHelper.
			//GetItemData(param.ItemType, userData.ItemProgress[param.ItemType].Level);
		return itemData;
	}
	
	private MercenaryData GenerateMercenaryData(MercenaryParameter param, BuildingIdentity campID)
	{
		MercenaryData mercenaryData = new MercenaryData();
		mercenaryData.CampID = campID;
		mercenaryData.ConfigData = ConfigInterface.Instance.MercenaryConfigHelper.GetMercenaryData(param.MercenaryType);
		return mercenaryData;
	}
	
	private BuildingData GenerateBuildingData(BuildingParameter param, List<ArmyData> armies, List<ItemData> items, Dictionary<MercenaryIdentity, MercenaryData> mercenaries,
		List<ObjectUpgrade<ArmyType>> armyUpgrades, List<ObjectUpgrade<ItemType>> itemUpgrades, UserData userData)
	{
		BuildingData result = new BuildingData();
		BuildingConfigData configData = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData
			(param.BuildingTypeID, param.Level);
		result.ConfigData = configData;
		
		BuildingIdentity id = new BuildingIdentity();
		id.buildingNO = param.BuildingNO;
		id.buildingType = param.BuildingTypeID;
		result.BuildingID = id;
		
		result.BuildingPosition = new TilePosition(param.PositionColumn, param.PositionRow);
		result.Level = param.Level;
		result.UpgradeRemainingWorkload = param.RemainWorkload;
		
		if(param.LastCollectGoldTick.HasValue)
		{
			result.LastCollectedGoldTick = param.LastCollectGoldTick.Value;
		}
		if(param.CollectedGold.HasValue)
		{
			result.CollectedGold = param.CollectedGold.Value;
		}
		if(param.LastCollectFoodTick.HasValue)
		{
			result.LastCollectedFoodTick = param.LastCollectFoodTick.Value;
		}
		if(param.CollectedFood.HasValue)
		{
			result.CollectedFood = param.CollectedFood.Value;
		}
		if(param.LastCollectOilTick.HasValue)
		{
			result.LastCollectedOilTick = param.LastCollectOilTick.Value;
		}
		if(param.CollectedOil.HasValue)
		{
			result.CollectedOil = param.CollectedOil.Value;
		}
		
		if(param.ArmyProducts != null)
		{
			result.ProduceArmy = new List<KeyValuePair<ArmyType,List<ArmyIdentity>>>();
			ArmyType? previousType = null;
			foreach (ArmyProductParameter product in param.ArmyProducts) 
			{
				if(previousType.HasValue && previousType.Value == product.ArmyType)
				{
					result.ProduceArmy[result.ProduceArmy.Count - 1].Value.Add(new ArmyIdentity(product.ArmyType, product.ArmyNO));
				}
				else
				{
					List<ArmyIdentity> newList = new List<ArmyIdentity>(){new ArmyIdentity(product.ArmyType, product.ArmyNO)};
					result.ProduceArmy.Add(new KeyValuePair<ArmyType,List<ArmyIdentity>>(product.ArmyType, newList));
				}
				previousType = product.ArmyType;
				armies.Add(this.GenerateArmyData(product, userData));
			}
		}
		
		if(param.Armies != null)
		{
			result.AvailableArmy = new List<ArmyIdentity>();
			foreach (ArmyParameter army in param.Armies) 
			{
				result.AvailableArmy.Add(new ArmyIdentity(army.ArmyType, army.ArmyNO));
				armies.Add(this.GenerateArmyData(army, userData, id));
			}
		}
		if(param.ArmyUpgrade != null)
		{
			result.ArmyUpgrade = param.ArmyUpgrade.ArmyType;
			ObjectUpgrade<ArmyType> upgrade = new ObjectUpgrade<ArmyType>(param.ArmyUpgrade.ArmyType, param.ArmyUpgrade.RemainingWorkload);
			armyUpgrades.Add(upgrade);
		
		}
		
		
		if(param.ItemProducts != null)
		{
			result.ProduceItem = new List<KeyValuePair<ItemType,List<ItemIdentity>>>();
			
			ItemType? previousType = null;
			foreach (ItemProductParameter product in param.ItemProducts) 
			{
				if(previousType.HasValue && previousType.Value == product.ItemType)
				{
					result.ProduceItem[result.ProduceItem.Count - 1].Value.Add(new ItemIdentity(product.ItemType, product.ItemNO));
				}
				else
				{
					List<ItemIdentity> newList = new List<ItemIdentity>(){new ItemIdentity(product.ItemType, product.ItemNO)};
					result.ProduceItem.Add(new KeyValuePair<ItemType,List<ItemIdentity>>(product.ItemType, newList));
				}
				previousType = product.ItemType;
				items.Add(this.GenerateItemData(product, userData));
			}
		}
		
		
		if(param.Items != null)
		{
			result.AvailableItem = new List<ItemIdentity>();
			foreach (ItemParameter item in param.Items) 
			{
				result.AvailableItem.Add(new ItemIdentity(item.ItemType, item.ItemNO));
				items.Add(this.GenerateItemData(item, userData));
			}
		}
		
		if(param.ItemUpgrade != null)
		{
			result.ItemUpgrade = param.ItemUpgrade.ItemType;
			ObjectUpgrade<ItemType> upgrade = new ObjectUpgrade<ItemType>(param.ItemUpgrade.ItemType, param.ItemUpgrade.RemainingWorkload);
			itemUpgrades.Add(upgrade);
		}
		
		if(param.Mercenaries != null)
		{
			result.AvailableMercenary = new List<MercenaryIdentity>();
			foreach(MercenaryParameter mercenary in param.Mercenaries)
			{
				result.AvailableMercenary.Add(new MercenaryIdentity(mercenary.MercenaryType, mercenary.MercenaryNO));
				mercenaries.Add(new MercenaryIdentity(mercenary.MercenaryType, mercenary.MercenaryNO), this.GenerateMercenaryData(mercenary, id));
			}
		}
		
		if(param.MercenaryProducts != null)
		{
			Dictionary<MercenaryType, MercenaryProductData> dict = new Dictionary<MercenaryType, MercenaryProductData>();
			foreach (MercenaryProductParameter mercenaryProduct in param.MercenaryProducts) 
			{
				MercenaryProductData data = new MercenaryProductData();
				data.ReadyNumber = mercenaryProduct.Number;
				data.RemainingTime = mercenaryProduct.RemainingSecond;
				data.ConfigData = ConfigInterface.Instance.MercenaryConfigHelper.GetMercenaryData(mercenaryProduct.MercenaryType);
				dict.Add(mercenaryProduct.MercenaryType, data);
			}
			result.ProduceMercenary = new MercenaryProductCollectionLogicObject(dict);
		}
		
		if(param.ResourceAccelerate != null)
		{
			result.RemainResourceAccelerateTime = param.ResourceAccelerate.RemainingTime;
		}
		
		if(param.ArmyAccelerate != null)
		{
			result.RemainArmyAccelerateTime = param.ArmyAccelerate.RemainingTime;
		}
		
		if(param.ItemAccelerate != null)
		{
			result.RemainItemAccelerateTime = param.ItemAccelerate.RemainingTime;
		}
		
		return result;
	}

    void Start()
    {
        Application.targetFrameRate = 30;
    }

    void Update()
    {
		if(this.m_ProgressBar.gameObject.activeInHierarchy)
		{
	        if (m_ProgressBar.sliderValue < 0.3f)
	        {
	            m_ProgressBar.sliderValue += 0.01f;
	        }
			else if (m_ProgressBar.sliderValue < 0.6f)
	        {
	            m_ProgressBar.sliderValue += 0.002f;
	        }
			else if (m_ProgressBar.sliderValue < 0.65f)
	        {
	            m_ProgressBar.sliderValue += 0.0004f;
	        }
			else if (m_ProgressBar.sliderValue < 0.75f)
	        {
	            m_ProgressBar.sliderValue += 0.005f;
	        }
			else if (m_ProgressBar.sliderValue < 0.8f)
	        {
	            m_ProgressBar.sliderValue += 0.0015f;
	        }
			else if (m_ProgressBar.sliderValue < 0.92f)
	        {
	            m_ProgressBar.sliderValue += 0.0005f;
	        }
			else if (m_ProgressBar.sliderValue < 0.97f)
	        {
	            m_ProgressBar.sliderValue += 0.0003f;
	        }
		}
    }
}
