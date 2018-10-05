using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;

public class BattleSceneHelper : MonoBehaviour, ISceneHelper
{
	private static BattleSceneHelper s_Sigleton;

	private Dictionary<BuildingType, List<GameObject>> m_Buildings;
	private Dictionary<BuildingCategory, List<GameObject>> m_BuildingsCategoryDict;
	
	private List<BattleObstacleUpgradingInfo> m_UpgradingObstacles;
	private Dictionary<GameObject, Transform> m_CachedTransformDict;
	
	private Dictionary<ResourceType, Dictionary<BuildingIdentity, GameObject>> m_ResouceBuildingDict;
	private ResourceManager m_ResourceManager;

	private int m_TotalGold;
	private int m_TotalFood;
	private int m_TotalOil;
	
	private int m_TotalSummaryBuildingCount;
	
	private List<GameObject> m_BuildingList;
	private List<GameObject> m_RemovableObjectList;
	private List<GameObject> m_DefenseObjectList;
	private List<GameObject> m_InvaderList;
	private List<GameObject> m_AttackableBuildingList;
	private List<GameObject> m_AchievementBuildingList;
	
	public static BattleSceneHelper Instance
	{
		get
		{
			return s_Sigleton;
		}
	}
	
	public int TotalGold
	{
		get
		{
			return this.m_TotalGold;
		}
	}
	
	public int TotalFood
	{
		get
		{
			return this.m_TotalFood;
		}
	}
	
	public int TotalOil
	{
		get
		{
			return this.m_TotalOil;
		}
	}
	
	public int TotalSummaryBuildingCount
	{
		get
		{
			return this.m_TotalSummaryBuildingCount;
		}
	}
	
	public int TotalInvaderCount
	{
		get
		{
			return this.m_InvaderList.Count;
		}
	}
	
	void Awake()
	{
		s_Sigleton = this;
		
		this.m_Buildings = new Dictionary<BuildingType, List<GameObject>>();
		this.m_BuildingList = new List<GameObject>();
		this.m_AchievementBuildingList = new List<GameObject>();
		this.m_RemovableObjectList = new List<GameObject>();
		this.m_DefenseObjectList = new List<GameObject>();
		this.m_UpgradingObstacles = new List<BattleObstacleUpgradingInfo>();
		this.m_ResouceBuildingDict = new Dictionary<ResourceType, Dictionary<BuildingIdentity, GameObject>>();
		this.m_ResourceManager = new ResourceManager();
		this.m_BuildingsCategoryDict = new Dictionary<BuildingCategory, List<GameObject>>();
		this.m_CachedTransformDict = new Dictionary<GameObject, Transform>();
		this.m_InvaderList = new List<GameObject>();
	}
	
	void OnDestroy()
	{
		s_Sigleton = null;
	}
	
	public List<BattleObstacleUpgradingInfo> UpgradingBuildings
	{
		get { return this.m_UpgradingObstacles; }
	}
	
	#region SceneHelper
	public List<GameObject> GetNearByBuilding(Vector3 position, int scope)
	{
		List<GameObject> result = new List<GameObject>();
		int distanceSqrt = scope * scope;
	
		TilePosition tilePosition = PositionConvertor.GetActorTileIndexFromWorldPosition(position);
	    int radius = Mathf.CeilToInt(scope / (float)Mathf.Min(ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.width,
			ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.height));
		List<TilePosition> affectedTiles = RoundHelper.FillCircle(tilePosition.Column, tilePosition.Row, radius);
		
		foreach (TilePosition tile in affectedTiles) 
		{
			if(tile.IsValidActorTilePosition())
			{
				Vector2 p = (Vector2)PositionConvertor.GetWorldPositionFromActorTileIndex(tile);
				Vector2  dis = p - (Vector2)position;
				if(dis.sqrMagnitude <= distanceSqrt)
				{
					GameObject building = BattleMapData.Instance.GetBulidingObjectFromActorObstacleMap(tile.Row, tile.Column);
					if(building != null && !result.Contains(building) && building.GetComponent<BuildingHPBehavior>() != null)
					{
						result.Add(building);
					}
				}
			}
		}
		return result;
	}
	
	public GameObject GetNearestBuilding(Vector3 characterPosition)
	{
		float nearestDistanceSqr = 100000000;
		GameObject result = null;
		foreach(GameObject building in this.m_BuildingList)
		{
			Vector3 buildingPosition = this.m_CachedTransformDict[building].position;
			float distanceSqr = Vector2.SqrMagnitude((Vector2)(buildingPosition - characterPosition));
			if(distanceSqr < nearestDistanceSqr)
			{
				nearestDistanceSqr = distanceSqr;
				result = building;
			}
		}
		return result;
	}
	
	public GameObject GetNearestBuildingExcludeType(Vector3 characterPosition, BuildingType buildingType)
	{
		float nearestDistanceSqr = 100000000;
		GameObject result = null;
		
		foreach (KeyValuePair<BuildingCategory, List<GameObject>> building in this.m_BuildingsCategoryDict) 
		{
			foreach (GameObject b in building.Value) 
			{
				BuildingPropertyBehavior property = b.GetComponent<BuildingPropertyBehavior>();
				if(property == null || property.BuildingType != buildingType)
				{
					Vector3 buildingPosition = this.m_CachedTransformDict[b].position;
					float distanceSqr = Vector2.SqrMagnitude(buildingPosition - characterPosition);
					if(distanceSqr < nearestDistanceSqr)
					{
						nearestDistanceSqr = distanceSqr;
						result = b;
					}
				}
			}
		}
		return result;
	}
	
	public GameObject GetNearestBuildingOfType(Vector3 characterPosition, BuildingType buildingType)
	{
		float nearestDistanceSqr = 100000000;
		GameObject result = null;
		if(this.m_Buildings.ContainsKey(buildingType))
		{
			foreach(GameObject building in this.m_Buildings[buildingType])
			{
				Vector3 buildingPosition = this.m_CachedTransformDict[building].position;
				float distanceSqr = Vector2.SqrMagnitude(buildingPosition - characterPosition);
				if(distanceSqr < nearestDistanceSqr)
				{
					nearestDistanceSqr = distanceSqr;
					result = building;
				}
			}
		}
		return result;
	}
	
	public List<Transform> GetBuildingsTransformOfCategory(BuildingCategory buildingCategory)
	{
		List<Transform> result = new List<Transform>();
		List<GameObject> buildings = this.GetBuildingsOfCategory(buildingCategory);
		foreach (GameObject building in buildings) 
		{
			result.Add(this.m_CachedTransformDict[building]);
		}
		return result;
	}
	
	public List<GameObject> GetBuildingsOfCategory(BuildingCategory buildingCategory)
	{
		List<GameObject> result = new List<GameObject>();
		if(buildingCategory == BuildingCategory.Any)
		{
			foreach(KeyValuePair<BuildingType, List<GameObject>> item in this.m_Buildings)
			{
				if(item.Key != BuildingType.Wall)
				{
					result.AddRange(item.Value);
				}
			}
		}
		else
		{
			if(this.m_BuildingsCategoryDict.ContainsKey(buildingCategory))
			{
				result.AddRange(this.m_BuildingsCategoryDict[buildingCategory]);
			}
		}
		return result;
	}
	
	public GameObject GetNearestBuildingOfCategory(Vector3 characterPosition, BuildingCategory buildingCategory)
	{
		if(buildingCategory == BuildingCategory.Any || !this.m_BuildingsCategoryDict.ContainsKey(buildingCategory))
		{
			return this.GetNearestBuildingExcludeType(characterPosition, BuildingType.Wall);
		}
		else
		{
			float nearestDistanceSqr = 100000000;
			GameObject result = null;
			foreach(GameObject building in this.m_BuildingsCategoryDict[buildingCategory])
			{
				Vector3 buildingPosition = this.m_CachedTransformDict[building].position;
				float distanceSqr = Vector2.SqrMagnitude(buildingPosition - characterPosition);
				if(distanceSqr < nearestDistanceSqr)
				{
					nearestDistanceSqr = distanceSqr;
					result = building;
				}
			}
			return result;
		}
	}
	
	public GameObject GetBuilding(TilePosition tilePosition)
	{
		return BattleMapData.Instance.BuildingArray[tilePosition.Row, tilePosition.Column];
	}
	
	private List<GameObject> GetActors(List<TilePosition> tiles)
	{
		List<GameObject> result = new List<GameObject>();
		
		foreach(TilePosition tile in tiles)
		{
			if(tile.IsValidActorTilePosition())
			{
				result.AddRange(BattleMapData.Instance.ActorArray[tile.Row, tile.Column]);
			}
		}
		return result;
	}
	
	public List<GameObject> GetActors(List<TilePosition> tiles, TargetType type)
	{
		if(type == TargetType.AirGround)
		{
			return this.GetActors(tiles);
		}
		else
		{
			List<GameObject> result = new List<GameObject>();
			
			foreach(TilePosition tile in tiles)
			{
				if(tile.IsValidActorTilePosition())
				{
					List<GameObject> characters = BattleMapData.Instance.ActorArray[tile.Row, tile.Column];
					foreach (GameObject character in characters) 
					{
						CharacterPropertyBehavior property = character.GetComponent<CharacterPropertyBehavior>();
						if(property.Type == type)
						{
							result.Add(character);
						}
					}
				}
			}
			return result;
		}
	}
	
	public void ConstructActor(GameObject actor, TilePosition position)
	{
		BattleMapData.Instance.RefreshInformationWithConstructActor(actor, position);
		if(actor.GetComponent<CharacterPropertyBehavior>().CharacterType == CharacterType.Invader)
		{
			this.m_InvaderList.Add(actor);
		}
	}
	
	public void DestroyActor(GameObject actor, TilePosition position)
	{
		BattleMapData.Instance.RefreshInformationWithDestroyActor(actor, position);
		if(actor.GetComponent<CharacterPropertyBehavior>().CharacterType == CharacterType.Invader)
		{
			this.m_InvaderList.Remove(actor);
		}
	}
	
	public void DestroyAllInvaders()
	{	
		if(BattleDirector.Instance == null)
		{
			foreach (GameObject invader in this.m_InvaderList) 
			{
				CharacterHPBehavior hpBehavior = invader.GetComponent<CharacterHPBehavior>();
				BattleMapData.Instance.RefreshInformationWithDestroyActor(invader, 
					PositionConvertor.GetActorTileIndexFromWorldPosition(invader.transform.position));
				hpBehavior.SetDead();
			}
			this.m_InvaderList.Clear();
		}
		else
		{
			foreach (GameObject invader in this.m_InvaderList) 
			{
				invader.GetComponent<NewAI>().enabled = false;
			}
			this.StartCoroutine("CharacterDestroy");
		}
	}
	
	IEnumerator CharacterDestroy()
	{
		for (int i = this.m_InvaderList.Count - 1; i >= 0; i --) 
		{
			GameObject invader = this.m_InvaderList[i];
			if(invader != null)
			{
				CharacterHPBehavior hpBehavior = invader.GetComponent<CharacterHPBehavior>();
				BattleMapData.Instance.RefreshInformationWithDestroyActor(invader, 
					PositionConvertor.GetActorTileIndexFromWorldPosition(invader.transform.position));
				hpBehavior.SetDead();
				yield return new WaitForSeconds(0.2f);
			}
		}
		this.m_InvaderList.Clear();
	}
	#endregion
	
	#region Destroy
	public void DestroyAchievementBuilding(GameObject achievementBuilding)
	{
		AchievementBuildingPropertyBehavior property = achievementBuilding.GetComponent<AchievementBuildingPropertyBehavior>();
		
		this.m_AchievementBuildingList.Remove(achievementBuilding);
		this.m_BuildingsCategoryDict[property.BuildingCategory].Remove(achievementBuilding);
		if(this.m_BuildingsCategoryDict[property.BuildingCategory].Count == 0)
		{
			this.m_BuildingsCategoryDict.Remove(property.BuildingCategory);
		}
		this.m_CachedTransformDict.Remove(achievementBuilding);
		
		BattleMapData.Instance.RefreshInformationWithDestroyObstacle(achievementBuilding);
		
		if(BattleDirector.Instance != null)
		{
			if(BattleRecorder.Instance.DestroyBuildingCount == this.m_TotalSummaryBuildingCount && 
				this.m_AchievementBuildingList.Count == 0)
			{
				BattleDirector.Instance.EndMatch();
			}
		}
	}
	
	public void DestroyBuilding(GameObject building)
	{
		BuildingPropertyBehavior property = building.GetComponent<BuildingPropertyBehavior>();
	    this.m_Buildings[property.BuildingType].Remove(building);
		this.m_BuildingList.Remove(building);
		if(this.m_Buildings[property.BuildingType].Count == 0)
		{
			this.m_Buildings.Remove(property.BuildingType);
		}
		this.m_BuildingsCategoryDict[property.BuildingCategory].Remove(building);
		if(this.m_BuildingsCategoryDict[property.BuildingCategory].Count == 0)
		{
			this.m_BuildingsCategoryDict.Remove(property.BuildingCategory);
		}
		this.m_CachedTransformDict.Remove(building);
		BattleMapData.Instance.RefreshInformationWithDestroyObstacle(building);
		
		if(BattleDirector.Instance != null)
		{
			if(BattleRecorder.Instance.DestroyBuildingCount == this.m_TotalSummaryBuildingCount && 
				this.m_AchievementBuildingList.Count == 0)
			{
				BattleDirector.Instance.EndMatch();
			}
		}
	}
	
	public void DestroyObject(GameObject removableObject)
	{
		this.m_RemovableObjectList.Remove(removableObject);
		BattleMapData.Instance.RefreshInformationWithDestroyObstacle(removableObject);
	}
	
	public void DestroyDefenseObject(GameObject defenseObject)
	{
		this.m_DefenseObjectList.Remove(defenseObject);
		BattleMapData.Instance.RefreshInformationWithDestroyObstacle(defenseObject);
	}
	
	#endregion
	
	#region Initial
	public void ConstructAchievementBuilding(GameObject achievementBuilding)
	{
		AchievementBuildingPropertyBehavior property = achievementBuilding.GetComponent<AchievementBuildingPropertyBehavior>();
		
		this.m_AchievementBuildingList.Add(achievementBuilding);
		if(!this.m_BuildingsCategoryDict.ContainsKey(property.BuildingCategory))
		{
			this.m_BuildingsCategoryDict[property.BuildingCategory] = new List<GameObject>();
		}
		this.m_BuildingsCategoryDict[property.BuildingCategory].Add(achievementBuilding);
		
		this.m_CachedTransformDict.Add(achievementBuilding, property.AnchorTransform);
		BattleMapData.Instance.RefreshInformationWithConstructObstacle(achievementBuilding);
	}
	
	public void ConstructBuilding(GameObject building)
	{
		BuildingPropertyBehavior property = building.GetComponent<BuildingPropertyBehavior>();
		if(!this.m_Buildings.ContainsKey(property.BuildingType))
		{
			this.m_Buildings[property.BuildingType] = new List<GameObject>();
		}
		this.m_Buildings[property.BuildingType].Add(building);
		this.m_BuildingList.Add(building);
		if(!this.m_BuildingsCategoryDict.ContainsKey(property.BuildingCategory))
		{
			this.m_BuildingsCategoryDict[property.BuildingCategory] = new List<GameObject>();
		}
		this.m_BuildingsCategoryDict[property.BuildingCategory].Add(building);
		
		if(property.BuildingType != BuildingType.Wall)
		{
			this.m_TotalSummaryBuildingCount ++;
		}
		if(building.GetComponent<BuildingAI>() != null)
		{
			this.m_AttackableBuildingList.Add(building);
		}
		
		this.m_CachedTransformDict.Add(building, property.AnchorTransform);
		BattleMapData.Instance.RefreshInformationWithConstructObstacle(building);
	}
	
	public void ConstructDefenseObject(GameObject defenseObject)
	{
		this.m_DefenseObjectList.Add(defenseObject);
		//BattleMapData.Instance.RefreshInformationWithConstructObstacle(defenseObject);
	}
	
	public void ConstructRemovableObject(GameObject removableObject)
	{
		this.m_RemovableObjectList.Add(removableObject);
		BattleMapData.Instance.RefreshInformationWithConstructObstacle(removableObject);
	}
	
	public void ClearObject()
	{
		BattleMapData.Instance.InitialMapData();
		foreach(KeyValuePair<BuildingType, List<GameObject>> entry in this.m_Buildings)
		{
			List<GameObject> buildings = entry.Value;
			for(int i = buildings.Count - 1; i >= 0; i --)
			{
				GameObject building = buildings[i];
				GameObject.DestroyImmediate(building);
			}
		}
		for(int i = this.m_RemovableObjectList.Count - 1; i >= 0; i--)
		{
			GameObject removableObject = this.m_RemovableObjectList[i];
			GameObject.DestroyImmediate(removableObject);
		}
		for(int i = this.m_DefenseObjectList.Count - 1; i >= 0; i --)
		{
			GameObject defenseObject = this.m_DefenseObjectList[i];
			GameObject.DestroyImmediate(defenseObject);
		}
		for(int i = this.m_AchievementBuildingList.Count - 1; i >= 0; i--)
		{
			GameObject achievementBuilding = this.m_AchievementBuildingList[i];
			GameObject.DestroyImmediate(achievementBuilding);
		}
		
		this.m_Buildings = new Dictionary<BuildingType, List<GameObject>>();
		this.m_BuildingList = new List<GameObject>();
		this.m_CachedTransformDict = new Dictionary<GameObject, Transform>();
		this.m_BuildingsCategoryDict = new Dictionary<BuildingCategory, List<GameObject>>();
		this.m_RemovableObjectList = new List<GameObject>();
		this.m_DefenseObjectList = new List<GameObject>();
		this.m_AchievementBuildingList = new List<GameObject>();
		this.m_UpgradingObstacles = new List<BattleObstacleUpgradingInfo>();
		this.m_ResouceBuildingDict = new Dictionary<ResourceType, Dictionary<BuildingIdentity, GameObject>>(){
			{ResourceType.Gold, new Dictionary<BuildingIdentity, GameObject>()},
			{ResourceType.Food, new Dictionary<BuildingIdentity, GameObject>()},
			{ResourceType.Oil, new Dictionary<BuildingIdentity, GameObject>()}
		};
		this.m_ResourceManager = new ResourceManager();
		this.m_AttackableBuildingList = new List<GameObject>();
		this.m_TotalGold = this.m_TotalFood = this.m_TotalOil = 0;
		this.m_TotalSummaryBuildingCount = 0;
		
		GameObject ruins = BattleObjectCache.Instance.RuinsObjectParent;
		for(int i = ruins.transform.childCount -  1; i >= 0; i --)
		{
			GameObject.Destroy(ruins.transform.GetChild(i).gameObject);
		}
		GameObject effects = BattleObjectCache.Instance.EffectObjectParent;
		for(int i = effects.transform.childCount - 1; i >= 0; i --)
		{
			GameObject.Destroy(effects.transform.GetChild(i).gameObject);
		}
	}
	
	public void EnableBuildingAI()
	{
		foreach (GameObject building in m_AttackableBuildingList) 
		{
			BuildingAI ai = building.GetComponent<BuildingAI>();
			ai.enabled = true;
		}
		foreach (GameObject defenseObject in this.m_DefenseObjectList) 
		{
			DefenseObjectAI ai = defenseObject.GetComponent<DefenseObjectAI>();
			ai.enabled = true;
		}
	}
	
	public void AddUpgradingObstacle(BattleObstacleUpgradingInfo info)
	{
		this.m_UpgradingObstacles.Add(info);
	}
	
	public void AddResourceBuilding(GameObject building, CapacityConfigData capacity)
	{
		BuildingPropertyBehavior property = building.GetComponent<BuildingPropertyBehavior>();
		BuildingIdentity id = new BuildingIdentity(property.BuildingType, property.BuildingNO);
		if(capacity.GoldCapacity > 0)
		{
			this.m_ResourceManager.AddStorage(ResourceType.Gold, 
				id, capacity.GoldCapacity);
			this.m_ResouceBuildingDict[ResourceType.Gold].Add(id, building);
		}
		if(capacity.FoodCapacity > 0)
		{
			this.m_ResourceManager.AddStorage(ResourceType.Food, 
				id, capacity.FoodCapacity);
			this.m_ResouceBuildingDict[ResourceType.Food].Add(id, building);
		}
		if(capacity.OilCapacity > 0)
		{
			this.m_ResourceManager.AddStorage(ResourceType.Oil, 
				id, capacity.OilCapacity);
			this.m_ResouceBuildingDict[ResourceType.Oil].Add(id, building);
		}
	}
	
	public void AddProduceResourceBuilding(BuildingPropertyBehavior property)
	{
		this.m_TotalGold += property.Gold;
		this.m_TotalFood += property.Food;
		this.m_TotalOil += property.Oil;
	}
	
	public void DistributeResource(int totalGold, int totalFood, int totalOil)
	{
		Dictionary<ResourceType, Dictionary<BuildingIdentity, int>> result = 
			this.m_ResourceManager.CalculateStorage(totalGold, totalFood, totalOil);
		
		foreach (KeyValuePair<ResourceType, Dictionary<BuildingIdentity, GameObject>> building in m_ResouceBuildingDict) 
		{
			foreach (KeyValuePair<BuildingIdentity, GameObject> b in building.Value) 
			{
				BuildingPropertyBehavior property = b.Value.GetComponent<BuildingPropertyBehavior>();
				switch(building.Key)
				{
					case ResourceType.Gold:
					{
						int gold = result[building.Key][b.Key];
						property.OriginalGoldPercentageWithoutPlunder = gold / (float)property.GoldCapacity;
						property.Gold = Mathf.RoundToInt(gold * property.PlunderRate);
						property.OriginalGold = property.Gold;
						this.m_TotalGold += property.Gold;
					}
					break;
					case ResourceType.Food:
					{
						int food = result[building.Key][b.Key];
						property.OriginalFoodPercentageWithoutPlunder = food / (float)property.FoodCapacity;
						property.Food = Mathf.RoundToInt(food* property.PlunderRate);
						property.OriginalFood = property.Food;
						this.m_TotalFood += property.Food;
					}
					break;
					case ResourceType.Oil:
					{
						int oil = result[building.Key][b.Key];
						property.OriginalOilPercentageWithoutPlunder = oil / (float)property.OilCapacity;
						property.Oil = Mathf.RoundToInt(oil * property.PlunderRate);
						property.OriginalOil = property.Oil;
						this.m_TotalOil += property.Oil;
					}
					break;
				}
			}
		}
		
		this.m_ResouceBuildingDict.Clear();
	}
	#endregion
	
	#region ISceneHelper implementation
	public List<IBuildingInfo> GetBuildings (BuildingType type)
	{
		List<IBuildingInfo> result = new List<IBuildingInfo>();
		if(this.m_Buildings.ContainsKey(type))
		{
			foreach(GameObject building in this.m_Buildings[type])
			{
				result.Add(building.GetComponent<BuildingPropertyBehavior>());
			}
		}
		return result;
	}

	public List<IBuildingInfo> GetBuildingsExceptTypes (HashSet<BuildingType> types)
	{
		HashSet<BuildingType> validTypes = new HashSet<BuildingType>();
		for(int i = 0; i < (int)BuildingType.Length; i ++)
		{
			if(!types.Contains((BuildingType)i))
			{
				validTypes.Add((BuildingType)i);
			}
		}
		return this.GetBuildingsOfTypes(validTypes);
	}

	public List<IBuildingInfo> GetBuildingsOfTypes (HashSet<BuildingType> types)
	{
		List<IBuildingInfo> result = new List<IBuildingInfo>();
		foreach(BuildingType type in types)
		{
			result.AddRange(this.GetBuildings(type));
		}
		return result;
	}

	public List<IBuildingInfo> GetAllBuildings ()
	{
		List<IBuildingInfo> result = new List<IBuildingInfo>();
		foreach(BuildingType type in this.m_Buildings.Keys)
		{
			result.AddRange(this.GetBuildings(type));
		}
		return result;
	}
	#endregion
	
	#region Plunder
	public void Plunder(int gold, int food, int oil)
	{
		this.m_TotalGold -= gold;
		this.m_TotalFood -= food;
		this.m_TotalOil -= oil;
	}
	#endregion
}
