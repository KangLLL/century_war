using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using CommandConsts;
using ConfigUtilities;
using ConfigUtilities.Structs;

public class ObstacleFactory : MonoBehaviour 
{
	[SerializeField]
	private BattleSceneHelper m_SceneHelper;
	[SerializeField]
	private Transform m_ParentNode;
	[SerializeField]
	private Transform m_BulletParent;
	
	private int m_CurrentRivalCityHallLevel;
	
	public int CurrentRivalCityHallLevle
	{
		get { return this.m_CurrentRivalCityHallLevel; }
	}
	
	public Age CurrentAge
	{
		get { return CommonUtilities.CommonUtilities.GetAgeFromCityHallLevel(this.m_CurrentRivalCityHallLevel); }
	}
	
	public void ConstructBuilding(FindRivalResponseParameter response)
	{
		foreach(BattleRemovableObjectParameter removableObject in response.Objects) 
		{
			this.ConstructRemovableObject(removableObject);
		}

		foreach (BattleDefenseObjectParameter defenseObject in response.DefenseObjects) 
		{
			this.ConstructDefenseObject(defenseObject);
		}
	
		if(response.Buffs != null)
		{
			BuildingBuffSystem.Instance.ClearBuff();
			BuildingBuffSystem.Instance.InitialBuff(response.Buffs);
		}
		
		bool isReplay = Application.loadedLevelName == ClientStringConstants.BATTLE_REPLAY_LEVEL_NAME;
		Dictionary<BuildingType, int> NOGenerater = new Dictionary<BuildingType, int>();
		if(isReplay)
		{
			for(int i = 0; i < (int)BuildingType.Length; i ++)
			{
				NOGenerater.Add((BuildingType)i, 0);
			}
		}
		Age age = Age.Prehistoric;
		bool isInitialAge = false;
		List<BuildingSurfaceBehavior> notInitialSurfaces = new List<BuildingSurfaceBehavior>();
		
		foreach(BattleBuildingParameter building in response.Buildings)
		{
			BuildingConfigData configData = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(building.BuildingType, building.Level);
			string prefabName = string.Format("{0}{1}{2}",ClientStringConstants.BATTLE_SCENE_RESOURCE_PREFAB_PREFIX_NAME,
				ClientStringConstants.BUILDING_OBJECT_PREFAB_PREFIX_NAME, configData.BuildingPrefabName);
			
			GameObject buildingPrefab = Resources.Load(prefabName) as GameObject;
			GameObject buildingObject = GameObject.Instantiate(buildingPrefab) as GameObject;
			
			buildingObject.transform.position = PositionConvertor.GetWorldPositionFromBuildingTileIndex
				(new TilePosition(building.PositionColumn, building.PositionRow));
			buildingObject.transform.parent = this.m_ParentNode;
			
			BuildingPropertyBehavior property = buildingObject.GetComponent<BuildingPropertyBehavior>();
			property.BuildingType = building.BuildingType;
			
			if(building.BuildingType == BuildingType.CityHall)
			{
				this.m_CurrentRivalCityHallLevel = building.Level;
				age = CommonUtilities.CommonUtilities.GetAgeFromCityHallLevel(building.Level);
				isInitialAge = true;
			}
			
			if(isReplay)
			{
				property.BuildingNO = NOGenerater[building.BuildingType];
				NOGenerater[building.BuildingType] ++;
			}
			else
			{
				property.BuildingNO = building.BuildingNO;
			}
			property.BuildingCategory = (BuildingCategory)configData.Category;
			property.Level = building.Level;
			
			property.BuildingPosition = new TilePosition(building.PositionColumn, building.PositionRow);
			property.BuildingObstacleList = new List<TilePosition>();
			property.ActorObstacleList = new List<TilePosition>();
			foreach(TilePoint buildingObstacle in configData.BuildingObstacleList)
			{
				property.BuildingObstacleList.Add(buildingObstacle.ConvertToTilePosition());
			}
			foreach (TilePoint actorObstacle in configData.ActorObstacleList) 
			{
				property.ActorObstacleList.Add(actorObstacle.ConvertToTilePosition());
			}
			
			//propert
			property.PlunderRate = configData.PlunderRate;
			property.GoldCapacity = configData.StoreGoldCapacity;
			property.FoodCapacity = configData.StoreFoodCapacity;
			property.OilCapacity = configData.StoreOilCapacity;
			
			property.Buffs = BuildingBuffSystem.Instance.GetBuffs(property.BuildingCategory);
			
			if(building.CurrentGold.HasValue || building.CurrentFood.HasValue || building.CurrentOil.HasValue)
			{
				property.Gold = building.CurrentGold.HasValue ? building.CurrentGold.Value : 0;
				property.Food = building.CurrentFood.HasValue ? building.CurrentFood.Value : 0;
				property.Oil = building.CurrentOil.HasValue ? building.CurrentOil.Value : 0;
				
				property.Gold = Mathf.RoundToInt(property.Gold * property.PlunderRate);
				property.Food = Mathf.RoundToInt(property.Food * property.PlunderRate);
				property.Oil = Mathf.RoundToInt(property.Oil * property.PlunderRate);
				property.OriginalGold = property.Gold;
				property.OriginalFood = property.Food;
				property.OriginalOil = property.Oil;
				
				this.m_SceneHelper.AddProduceResourceBuilding(property);
			}
			else
			{
				if((configData.CanStoreGold && !configData.CanProduceGold && building.Level != 0) || 
					(configData.CanStoreFood && !configData.CanProduceFood && building.Level != 0) || 
					(configData.CanStoreOil && !configData.CanProduceOil && building.Level != 0))
				{
					this.m_SceneHelper.AddResourceBuilding(buildingObject, 
						new CapacityConfigData(){ GoldCapacity = configData.StoreGoldCapacity,
						FoodCapacity = configData.StoreFoodCapacity, OilCapacity = configData.StoreOilCapacity });
				}
			}
			
			BuildingHPBehavior hp = buildingObject.GetComponent<BuildingHPBehavior>();
			hp.TotalHP = Mathf.Max(1, configData.MaxHP + property.BuffHPEffect);
			hp.SceneHelper = this.m_SceneHelper;
			hp.ArmorCategory = configData.ArmorCategory;
			
			if(building.BuilderLevel.HasValue)
			{
				BattleObstacleUpgradingInfo info = new BattleObstacleUpgradingInfo() 
					{ AttachedBuilderLevel = building.BuilderLevel.Value, ObstacleProperty = property };
				this.m_SceneHelper.AddUpgradingObstacle(info);
			}
			if(building.IsUpgrading)
			{
				this.ConstructFacility(buildingObject, property);
			}
			
			BuildingSurfaceBehavior surface = buildingObject.GetComponent<BuildingSurfaceBehavior>();
			if(surface != null)
			{
				if(isInitialAge)
				{
					surface.SetSurface(age, building.BuildingType);
				}
				else
				{
					notInitialSurfaces.Add(surface);
				}
			}
			
			BuildingAI buildingAI = buildingObject.GetComponent<BuildingAI>();
			if(configData.CanAttack && !building.IsUpgrading)
			{
				buildingAI.enabled = false;
			    AttackBehavior attackBehavior = null;
				if(configData.ApMinScope > 0)
				{
					attackBehavior = buildingObject.AddComponent<RingAttackBehavior>();
					((RingAttackBehavior)attackBehavior).BlindScope = configData.ApMinScope;
				}
				else
				{
					attackBehavior = buildingObject.AddComponent<AttackBehavior>();
				}
				int cd = Mathf.FloorToInt(configData.AttackCD * ClientConfigConstants.Instance.TicksPerSecond);
				attackBehavior.AttackCD = Mathf.Max(1, cd - property.BuffAttackSpeedEffect);
				attackBehavior.AttackScope = configData.ApMaxScope;
				attackBehavior.AttackValue = Mathf.Max(1, configData.AttackValue + property.BuffAttackValueEffect);
				attackBehavior.BulletFlySpeed = configData.AttackMiddleSpeed;
				attackBehavior.AttackType = (AttackType)configData.AttackType;
				attackBehavior.AttackCategory = configData.AttackCategory;
				attackBehavior.TargetType = (TargetType)configData.TargetType;
				attackBehavior.DamageScope = configData.DamageScope;
				attackBehavior.PushTicks = configData.DamagePushTicks;
				attackBehavior.PushVelocity = configData.DamagePushVelocity;
				attackBehavior.BulletParent = this.m_BulletParent;
				
				buildingAI.AttackBehavior = attackBehavior;
				BuildingIdleState idleState = new BuildingIdleState(buildingAI, true);
				buildingAI.ChangeState(idleState);
				buildingAI.SceneHelper = BattleSceneHelper.Instance;
			}
			else
			{
				buildingAI.DetachSelf();
			}
			
			this.m_SceneHelper.ConstructBuilding(buildingObject);
		}
		
		foreach (BuildingSurfaceBehavior surface in notInitialSurfaces) 
		{
			surface.SetSurface(age, surface.GetComponent<BuildingPropertyBehavior>().BuildingType);
		}
		
		this.m_SceneHelper.DistributeResource(response.TotalGold, response.TotalFood, response.TotalOil);
		
		
		foreach (BattleAchievementBuildingParameter achievementBuilding in response.AchievementBuildings) 
		{
			this.ConstructAchievementBuilding(achievementBuilding, age);
		}
	}
	
	private void ConstructAchievementBuilding(BattleAchievementBuildingParameter param, Age age)
	{
		AchievementBuildingConfigData configData = ConfigInterface.Instance.AchievementBuildingConfigHelper.GetAchievementBuildingData(param.AchievementBuildingType);
		string prefabName = string.Format("{0}{1}{2}",ClientStringConstants.BATTLE_SCENE_RESOURCE_PREFAB_PREFIX_NAME,
			ClientStringConstants.ACHIEVEMENT_BULIDING_PREFAB_PREFIX_NAME, configData.PrefabName);
			
		GameObject buildingPrefab = Resources.Load(prefabName) as GameObject;
		GameObject buildingObject = GameObject.Instantiate(buildingPrefab) as GameObject;
			
		buildingObject.transform.position = PositionConvertor.GetWorldPositionFromBuildingTileIndex
			(new TilePosition(param.PositionColumn, param.PositionRow));
		buildingObject.transform.parent = this.m_ParentNode;
			
		AchievementBuildingPropertyBehavior property = buildingObject.GetComponent<AchievementBuildingPropertyBehavior>();
		property.AchievementBuildingType = param.AchievementBuildingType;
		property.BuildingNO = param.AchievementBuildingNo;
		property.IsDropProps = param.IsDropProps;
		
		property.BuildingCategory = (BuildingCategory)configData.Category;
		property.BuildingPosition = new TilePosition(param.PositionColumn, param.PositionRow);
		property.BuildingObstacleList = new List<TilePosition>();
		property.ActorObstacleList = new List<TilePosition>();
		foreach(TilePoint buildingObstacle in configData.BuildingObstacleList)
		{
			property.BuildingObstacleList.Add(buildingObstacle.ConvertToTilePosition());
		}
		foreach (TilePoint actorObstacle in configData.ActorObstacleList) 
		{
			property.ActorObstacleList.Add(actorObstacle.ConvertToTilePosition());
		}
		
		property.Buffs = BuildingBuffSystem.Instance.GetBuffs(property.BuildingCategory);
			
		AchievementBuildingHPBehavior hp = buildingObject.GetComponent<AchievementBuildingHPBehavior>();
		hp.TotalHP = Mathf.Max(1, configData.MaxHP + property.BuffHPEffect);
		hp.SceneHelper = this.m_SceneHelper;
		hp.ArmorCategory = configData.ArmorCategory;
			
		BuildingSurfaceBehavior surface = buildingObject.GetComponent<BuildingSurfaceBehavior>();
		if(surface != null)
		{
			surface.SetSurface(age, param.AchievementBuildingType);
		}
		this.m_SceneHelper.ConstructAchievementBuilding(buildingObject);
	}
	
	private void ConstructRemovableObject(BattleRemovableObjectParameter param)
	{
		RemovableObjectConfigData configData = ConfigInterface.Instance.RemovableConfigHelper.GetRemovableObjectData(param.ObjectType);
		GameObject objectPrefab = Resources.Load( ClientStringConstants.BATTLE_SCENE_RESOURCE_PREFAB_PREFIX_NAME + 
			ClientStringConstants.REMOVABLE_OBJECT_PREFAB_PREFIX_NAME + configData.PrefabName) as GameObject;
		
		GameObject removableObject = GameObject.Instantiate(objectPrefab) as GameObject;
		removableObject.transform.position = PositionConvertor.GetWorldPositionFromBuildingTileIndex
				(new TilePosition(param.PositionColumn, param.PositionRow));
		
		RemovableObjectPropertyBehavior property = removableObject.GetComponent<RemovableObjectPropertyBehavior>();
		
		property.BuildingPosition = new TilePosition(param.PositionColumn, param.PositionRow);
		property.ObjectType = param.ObjectType;
		property.ActorObstacleList = new List<TilePosition>();
		property.BuildingObstacleList = new List<TilePosition>();
		foreach (TilePoint point in configData.ActorObstacleList) 
		{
			property.ActorObstacleList.Add(point.ConvertToTilePosition());
		}
		foreach (TilePoint point in configData.BuildingObstacleList) 
		{
			property.BuildingObstacleList.Add(point.ConvertToTilePosition());
		}
		removableObject.transform.parent = this.m_ParentNode;
		
		if(param.BuilderLevel.HasValue)
		{
			BattleObstacleUpgradingInfo info = new BattleObstacleUpgradingInfo() 
					{ AttachedBuilderLevel = param.BuilderLevel.Value, ObstacleProperty = property };
			this.m_SceneHelper.AddUpgradingObstacle(info);
		}
		
		this.m_SceneHelper.ConstructRemovableObject(removableObject);
	}
	
	private void ConstructDefenseObject(BattleDefenseObjectParameter  param)
	{
		PropsConfigData configData = ConfigInterface.Instance.PropsConfigHelper.GetPropsData(param.PropsType);
		PropsDefenseScopeConfigData defenseData = configData.FunctionConfigData as PropsDefenseScopeConfigData;
		PropsDefenseScopeLastingConfigData lastingData = configData.FunctionConfigData as PropsDefenseScopeLastingConfigData;
		DefenseObjectConfigWrapper configWrapper = new DefenseObjectConfigWrapper(configData.FunctionConfigData);
		
		GameObject defenseObjectPrefab = Resources.Load( ClientStringConstants.BATTLE_SCENE_RESOURCE_PREFAB_PREFIX_NAME + 
			ClientStringConstants.DEFENSE_OBJECT_PREFAB_PREFIX_NAME + configWrapper.PrefabName) as GameObject; 
		
		GameObject defenseObject = GameObject.Instantiate(defenseObjectPrefab) as GameObject;
		defenseObject.transform.position = PositionConvertor.GetWorldPositionFromBuildingTileIndex
				(new TilePosition(param.PositionColumn, param.PositionRow));
		
		DefenseObjectAI ai = defenseObject.GetComponent<DefenseObjectAI>();
		ai.enabled = false;
		
		DefenseObjectPropertyBehavior property = defenseObject.GetComponent<DefenseObjectPropertyBehavior>();
		property.DefenseObjectID = param.DefenseObjectID;
		property.BuildingPosition = new TilePosition(param.PositionColumn, param.PositionRow);
		property.ActorObstacleList = new List<TilePosition>();
		property.BuildingObstacleList = new List<TilePosition>();
		
		if(defenseData != null)
		{
			BombBehavior bomb = defenseObject.GetComponent<BombBehavior>();
			bomb.AttackCategory = defenseData.AttackCategory;
			bomb.Damage = defenseData.Damage;
			bomb.PushVelocity = defenseData.PushVelocity;
			bomb.PushTicks = defenseData.PushTicks;
			bomb.Scope = defenseData.Scope;
			bomb.TriggerScope = defenseData.TriggerScope;
			bomb.TriggerTick = defenseData.TriggerTicks;
			bomb.TargetType = (TargetType)defenseData.TargetType;
			bomb.ParentNode = this.m_BulletParent;
		}
		else if(lastingData != null)
		{
			LastingEffectBehavior lasting = defenseObject.GetComponent<LastingEffectBehavior>();
			lasting.AttackCategory = lastingData.AttackCategory;
			lasting.Damage = lastingData.Damage;
			lasting.IntervalTicks = lastingData.IntervalTicks;
			lasting.TotalTimes = lastingData.TotalTimes;
			lasting.Scope = lastingData.Scope;
			lasting.TriggerScope = lastingData.TriggerScope;
			lasting.TriggerTick = lastingData.TriggerTicks;
			lasting.TargetType = (TargetType)lastingData.TargetType;
			//lasting.ParentNode = this.m_ParentNode;
		}
		
		defenseObject.transform.parent = this.m_ParentNode;
		
		this.m_SceneHelper.ConstructDefenseObject(defenseObject);
	}
	
	private void ConstructFacility(GameObject buildingObject, BuildingPropertyBehavior property)
	{
		string prefabPath = ClientStringConstants.BATTLE_SCENE_RESOURCE_PREFAB_PREFIX_NAME +
			ClientStringConstants.BUILDING_OBJECT_PREFAB_PREFIX_NAME + ClientStringConstants.FACILITIES_OBJECT_PREFAB_PREFIX_NAME + property.FacilityPrefabPath;
		GameObject prefab = Resources.Load(prefabPath) as GameObject;
		GameObject facilities = GameObject.Instantiate(prefab) as GameObject;
		facilities.SetActive(true);
		Vector3 localPosition = facilities.transform.position;
		facilities.transform.parent = buildingObject.GetComponent<BuildingPropertyBehavior>().AnchorTransform;
		facilities.transform.localPosition = localPosition;
	}
}
