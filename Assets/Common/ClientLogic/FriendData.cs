using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommandConsts;
using ConfigUtilities.Enums;
using ConfigUtilities;

public class FriendData : ISceneHelper
{
	private Dictionary<BuildingType, Dictionary<int, BuildingLogicObject>> m_LogicObjects;
	private Dictionary<ArmyType, Dictionary<int, FriendArmyParameter>> m_ArmyObjects;
	private Dictionary<MercenaryType, int> m_Mercenaries;
	private Dictionary<ItemType, Dictionary<int, FriendItemParameter>> m_ItemObjects;
	
	private List<BuffLogicData> m_Buffs;
	private List<PropsLogicData> m_Props;
	
	private Dictionary<long, RemovableObjectLogicData> m_RemovableObjects;
	private Dictionary<int, AchievementBuildingLogicData> m_AchievementBuildings;

    private PlayerLogicData m_FriendData;

	private int m_TotalCampCapacity;
	private int m_AlreadyArmyCapacity;

	private Dictionary<ArmyType, int> m_ArmyLevels;

    public PlayerLogicData PlayerData
    {
        get { return m_FriendData; }
       
    }
	private ResourceManager m_ResourceManager;
	
	private List<BuildingLogicData> m_CachedBuildings;
	
	public void ProcessLogic()
	{
		foreach (KeyValuePair<BuildingType, Dictionary<int, BuildingLogicObject>> building in this.m_LogicObjects) 
		{
			foreach (KeyValuePair<int, BuildingLogicObject> b in building.Value) 
			{
				b.Value.Process();
			}
		}
	}
	
	public void InitialWithResponseData(FriendResponseParameter parameter, string friendName)
	{
		UserData userData = new UserData();
		userData.Name = friendName;
		userData.Honour = parameter.Honour;
		userData.Level = parameter.Level;
		userData.CurrentStoreGold = parameter.PlayerGold;
		userData.CurrentStoreFood = parameter.PlayerFood;
		userData.CurrentStoreOil = parameter.PlayerOil;
		userData.ConfigData = ConfigInterface.Instance.PlayerConfigHelper.GetPlayerData(parameter.Level);
		
		this.m_FriendData = new PlayerLogicData(userData);
		this.m_LogicObjects = new Dictionary<BuildingType, Dictionary<int, BuildingLogicObject>>();
		this.m_ArmyObjects = new Dictionary<ArmyType, Dictionary<int, FriendArmyParameter>>();
		this.m_Mercenaries = new Dictionary<MercenaryType, int>();
		this.m_ItemObjects = new Dictionary<ItemType, Dictionary<int, FriendItemParameter>>();
		this.m_AchievementBuildings = new Dictionary<int, AchievementBuildingLogicData>();
		this.m_Buffs = new List<BuffLogicData>();
		this.m_Props = new List<PropsLogicData>();
		this.m_ResourceManager = new ResourceManager();

		this.m_ArmyLevels = new Dictionary<ArmyType, int>();
		
		Dictionary<BuildingType, Dictionary<int, BuildingData>> datas = 
			new Dictionary<BuildingType, Dictionary<int, BuildingData>>();
		
		this.m_RemovableObjects = new Dictionary<long, RemovableObjectLogicData>();
		
		int objectID = 0;

		foreach(FriendObjectParameter param in parameter.Objects)
		{
			RemovableObjectData data = new RemovableObjectData();
			data.BuilderBuildingNO = param.BuilderBuildingNO;
			data.Position = new TilePosition(param.PositionColumn, param.PositionRow);
			
			if(param.IsRemoving)
			{
				if(param.BuilderBuildingNO.HasValue)
				{
					data.RemainingWorkload = 0;
				}
				else
				{
					data.RemainingWorkload  = 1;
				}
			}
			data.RemovableObjectNo = objectID;
			data.RemovableObjectType = param.ObjectType;
			data.ConfigData = ConfigInterface.Instance.RemovableConfigHelper.GetRemovableObjectData(param.ObjectType);
			this.m_RemovableObjects.Add(objectID, new RemovableObjectLogicData(data));
			objectID ++;
		}
		
		int buildingNo = 0;
		foreach(FriendAchievementBuildingParameter param in parameter.AchievementBuildings)
		{
			AchievementBuildingData data = new AchievementBuildingData();
			data.AchievementBuildingType = param.AchievementBuildingType;
			data.BuildingNo = buildingNo ++;
			data.BuildingPosition = new TilePosition(param.PositionColumn, param.PositionRow);
			data.Life = param.Life;
			data.ConfigData = ConfigInterface.Instance.AchievementBuildingConfigHelper.GetAchievementBuildingData(param.AchievementBuildingType);
			this.m_AchievementBuildings.Add(data.BuildingNo, new AchievementBuildingLogicData(data));
		}
		
		foreach (FriendBuffParameter buff in parameter.Buffs) 
		{
			BuffData b = new BuffData();
			b.RelatedPropsType = buff.PropsType;
			b.RemainingCD = 10;
			b.BuffConfigData = ConfigInterface.Instance.PropsConfigHelper.GetPropsData(buff.PropsType).FunctionConfigData as PropsBuffConfigData;
			BuffLogicData data = new BuffLogicData(b);
			
			this.m_Buffs.Add(data);
		}
		
		int tempPropsID = 0;
		foreach(FriendPropsParameter props in parameter.Props)
		{
			PropsData p = new PropsData();
			PropsConfigData propsConfigData = ConfigInterface.Instance.PropsConfigHelper.GetPropsData(props.PropsType);
			p.PropsConfigData = propsConfigData;
			p.PropsNo = tempPropsID ++;
			p.PropsType = props.PropsType;
			p.RemainingCD = 10;
			p.RemainingUseTime = propsConfigData.MaxUseTimes;
			PropsLogicData data = new PropsLogicData(p);
			
			this.m_Props.Add(data);
		}
		
		foreach (FriendBuildingParameter param in parameter.Buildings) 
		{
			BuildingConfigData configData = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(param.BuildingTypeID, param.Level);
			
			BuildingData data = new BuildingData();
			data.BuilderBuildingNO = param.BuilderBuildingNO;
			data.BuildingID = new BuildingIdentity(param.BuildingTypeID, param.BuildingNO);
			data.BuildingPosition = new TilePosition(param.PositionColumn, param.PositionRow);
			data.Level = param.Level;
			if(param.IsUpgrading)
			{
				if(param.BuilderBuildingNO.HasValue)
				{
					data.UpgradeRemainingWorkload = 1;
				}
				else
				{
					data.UpgradeRemainingWorkload = 0;
				}
			}
			if(param.IsResourceAccelerate)
			{
				data.RemainResourceAccelerateTime = 1;
			}
			if(param.IsArmyAccelerate)
			{
				data.RemainArmyAccelerateTime = 1;
			}
			if(param.IsItemAccelerate)
			{
				data.RemainItemAccelerateTime = 1;
			}
			
			data.ConfigData = configData;
			
			if(param.CurrentStoreGold.HasValue)
			{
				data.CurrentStoreGold = param.CurrentStoreGold.Value;
			}
			else if(configData.CanStoreGold)
			{
				this.m_ResourceManager.AddStorage(ResourceType.Gold, data.BuildingID, configData.StoreGoldCapacity);
			}
			if(param.CurrentStoreFood.HasValue)
			{
				data.CurrentStoreFood = param.CurrentStoreFood.Value;
			}
			else if(configData.CanStoreFood)
			{
				this.m_ResourceManager.AddStorage(ResourceType.Food, data.BuildingID, configData.StoreFoodCapacity);
			}
			if(param.CurrentStoreOil.HasValue)
			{
				data.CurrentStoreOil = param.CurrentStoreOil.Value;
			}
			else if(configData.CanStoreOil)
			{
				this.m_ResourceManager.AddStorage(ResourceType.Oil, data.BuildingID, configData.StoreOilCapacity);
			}
			
			if(!this.m_LogicObjects.ContainsKey(param.BuildingTypeID))
			{
				this.m_LogicObjects.Add(param.BuildingTypeID, new Dictionary<int, BuildingLogicObject>());
			}
			if(!datas.ContainsKey(param.BuildingTypeID))
			{
				datas.Add(param.BuildingTypeID, new Dictionary<int, BuildingData>());
			}
			this.m_LogicObjects[param.BuildingTypeID].Add(param.BuildingNO, 
				new BuildingLogicObject(data, (int)BuildingFunction.ProduceResource, false));
			datas[param.BuildingTypeID].Add(param.BuildingNO, data);
			
			data.AvailableMercenary = new List<MercenaryIdentity>();
			if(param.Mercenaries != null)
			{
				foreach (KeyValuePair<MercenaryType, int> mercenary in param.Mercenaries) 
				{
					MercenaryConfigData mercenaryConfigData = ConfigInterface.Instance.MercenaryConfigHelper.GetMercenaryData(mercenary.Key);
					this.m_AlreadyArmyCapacity += mercenaryConfigData.CapcityCost * mercenary.Value;

					for(int i = 0; i < mercenary.Value; i ++)
					{
						data.AvailableMercenary.Add(new MercenaryIdentity(mercenary.Key, i));
						if(!this.m_Mercenaries.ContainsKey(mercenary.Key))
						{
							this.m_Mercenaries.Add(mercenary.Key, 0);
						}
						this.m_Mercenaries[mercenary.Key] ++;
					}
				}
			}
			
			data.AvailableArmy = new List<ArmyIdentity>();
			if(param.Armies != null)
			{
				foreach (FriendArmyParameter army in param.Armies) 
				{
					if(!this.m_ArmyLevels.ContainsKey(army.ArmyType))
					{
						this.m_ArmyLevels.Add(army.ArmyType, army.ArmyLevel);
					}

					ArmyType type = army.ArmyType;
					ArmyConfigData armyConfigData = ConfigInterface.Instance.ArmyConfigHelper.GetArmyData(army.ArmyType, army.ArmyLevel);
					this.m_AlreadyArmyCapacity += armyConfigData.CapcityCost;
					if(!this.m_ArmyObjects.ContainsKey(type))
					{
						this.m_ArmyObjects.Add(type, new Dictionary<int, FriendArmyParameter>());
					}
					ArmyIdentity armyID = new ArmyIdentity(type, this.m_ArmyObjects[type].Count);
					data.AvailableArmy.Add(armyID);
					this.m_ArmyObjects[type].Add(armyID.armyNO, army);
				}
			}
			data.AvailableItem = new List<ItemIdentity>();
			if(param.Items != null)
			{
				foreach (FriendItemParameter item in param.Items) 
				{
					ItemType type = item.ItemType;
					if(!this.m_ItemObjects.ContainsKey(type))
					{
						this.m_ItemObjects.Add(type, new Dictionary<int, FriendItemParameter>());
					}
					ItemIdentity itemID = new ItemIdentity(type, this.m_ItemObjects[type].Count);
					data.AvailableItem.Add(itemID);
					this.m_ItemObjects[type].Add(itemID.itemNO, item);
				}
			}

			this.m_TotalCampCapacity += configData.ArmyCapacity;
		}
		
		Dictionary<ResourceType, Dictionary<BuildingIdentity, int>> result = this.m_ResourceManager.CalculateStorage
			(userData.CurrentStoreGold, userData.CurrentStoreFood, userData.CurrentStoreOil);
		foreach(KeyValuePair<ResourceType, Dictionary<BuildingIdentity, int>> resource in result)
		{
			foreach(KeyValuePair<BuildingIdentity, int> r in resource.Value)
			{
				switch(resource.Key)
				{
					case ResourceType.Gold:
					{
						datas[r.Key.buildingType][r.Key.buildingNO].CurrentStoreGold = r.Value;
					}
					break;
					case ResourceType.Food:
					{
						datas[r.Key.buildingType][r.Key.buildingNO].CurrentStoreFood = r.Value;
					}
					break;
					case ResourceType.Oil:
					{
						datas[r.Key.buildingType][r.Key.buildingNO].CurrentStoreOil = r.Value;
					}
					break;
				}
			}
		}
	}
	
	public BuildingLogicData GetBuildingData(BuildingIdentity id)
	{
		return this.m_LogicObjects[id.buildingType][id.buildingNO].BuildingLogicData;
	}
	
	public FriendArmyParameter GetArmy(ArmyIdentity id)
	{
		return this.m_ArmyObjects[id.armyType][id.armyNO];
	}
	
	public FriendItemParameter GetItem(ItemIdentity id)
	{
		return this.m_ItemObjects[id.itemType][id.itemNO];
	}
	
	public List<BuildingLogicData> AllBuildings
	{
		get
		{
			if(this.m_CachedBuildings  == null)
			{
				this.m_CachedBuildings = new List<BuildingLogicData>();
				foreach (KeyValuePair<BuildingType, Dictionary<int, BuildingLogicObject>> building in this.m_LogicObjects) 
				{
					foreach(KeyValuePair<int, BuildingLogicObject> b in building.Value)
					{
						this.m_CachedBuildings.Add(b.Value.BuildingLogicData);
					}
				}
			}
			
			return this.m_CachedBuildings;
		}
	}
	
	public List<AchievementBuildingLogicData> AllAchievementBuildings
	{
		get
		{
			return new List<AchievementBuildingLogicData>(this.m_AchievementBuildings.Values);
		}
	}
	
	public List<RemovableObjectLogicData> AllRemovableObjects
	{
		get
		{
			return new List<RemovableObjectLogicData>(this.m_RemovableObjects.Values);
		}
	}
	
	public IEnumerable<BuffLogicData> AllBuffs 
	{ 
		get 
		{ 
			return this.m_Buffs;
		} 
	}
	
	public IEnumerable<PropsLogicData> AllPlunderableProps
	{
		get
		{
			return this.m_Props;
		}
	}

	public int TotalCampCapacity { get { return this.m_TotalCampCapacity; } }
	public int AlreadyArmyCapacity { get { return this.m_AlreadyArmyCapacity; } }
	public Dictionary<ArmyType, int> TotalArmies 
	{
		get
		{
			Dictionary<ArmyType, int> result = new Dictionary<ArmyType, int>();
			foreach (var army in this.m_ArmyObjects) 
			{
				result.Add(army.Key, army.Value.Count);
			}
			return result;
		}
	}

	public Dictionary<MercenaryType, int> TotalMercenaries
	{
		get
		{
			Dictionary<MercenaryType, int> result = new Dictionary<MercenaryType, int>();
			foreach (var mercenary in this.m_Mercenaries) 
			{
				result.Add(mercenary.Key, mercenary.Value);
			}
			return result;
		}
	}

	public int GetArmyLevel(ArmyType type)
	{
		return this.m_ArmyLevels[type];
	}

	#region ISceneHelper implementation
	public List<IBuildingInfo> GetBuildings (BuildingType type)
	{
		List<IBuildingInfo> result = new List<IBuildingInfo>();
		foreach (KeyValuePair<int, BuildingLogicObject> item in m_LogicObjects[type])
		{
			result.Add(item.Value.BuildingLogicData);
		}
		return result;
	}

	public List<IBuildingInfo> GetBuildingsExceptTypes (HashSet<BuildingType> types)
	{
		List<IBuildingInfo> result = new List<IBuildingInfo>();
		foreach (KeyValuePair<BuildingType, Dictionary<int, BuildingLogicObject>> building in m_LogicObjects) 
		{
			if(!types.Contains(building.Key))
			{
				foreach (KeyValuePair<int, BuildingLogicObject> item in building.Value) 
				{
					result.Add(item.Value.BuildingLogicData);
				}
			}
		}
		return result;
	}

	public List<IBuildingInfo> GetBuildingsOfTypes (HashSet<BuildingType> types)
	{
		List<IBuildingInfo> result = new List<IBuildingInfo>();
		foreach (KeyValuePair<BuildingType, Dictionary<int, BuildingLogicObject>> building in m_LogicObjects) 
		{
			if(types.Contains(building.Key))
			{
				foreach (KeyValuePair<int, BuildingLogicObject> item in building.Value) 
				{
					result.Add(item.Value.BuildingLogicData);
				}
			}
		}
		return result;
	}

	public List<IBuildingInfo> GetAllBuildings ()
	{
		return this.m_CachedBuildings.ToOtherList<IBuildingInfo, BuildingLogicData>();
	}
	#endregion
}
