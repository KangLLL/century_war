using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
using ConfigUtilities;
using ConfigUtilities.Structs;
using System.Collections.Generic;

public class EditorFactory : MonoBehaviour 
{
	private static EditorFactory s_Sigleton;
	
	[SerializeField]
	private Transform m_SceneCamare;
	[SerializeField]
	private Transform m_SceneParent;
	private GameObject[,] m_MapData;
	
	public static EditorFactory Instance
	{
		get { return s_Sigleton; }
	}
	
	public GameObject[,] MapData
	{
		get { return this.m_MapData; } 
	}
	
	void Awake()
	{
		s_Sigleton = this;
	}
	
	void Start()
	{
		this.m_MapData = new GameObject[ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height, 
			ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width];
	}
	
	#region Public Methods
	public void ConstructBuilding(BuildingType type, int level)
	{
		BuildingConfigData configData = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(type, level);
		TilePosition centerPosition = PositionConvertor.GetBuildingTileIndexFromWorldPosition(this.m_SceneCamare.position);
		TilePosition position = this.FindValidBuildingPosition(centerPosition, configData.BuildingObstacleList);
		if(position != null)
		{
			this.ConstructBuilding(type, level, position);
		}
	}
	 
	public void ConstructBuilding(BuildingType type, int level, TilePosition position)
	{
		BuildingConfigData configData = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(type, level);
	
		string prefabName = string.Format("{0}{1}{2}",ClientStringConstants.BATTLE_SCENE_RESOURCE_PREFAB_PREFIX_NAME,
				ClientStringConstants.BUILDING_OBJECT_PREFAB_PREFIX_NAME, configData.BuildingPrefabName);
		GameObject buildingPrefab = Resources.Load(prefabName) as GameObject;
		GameObject newBuilding = GameObject.Instantiate(buildingPrefab) as GameObject;
		GameObject.DestroyImmediate(newBuilding.GetComponent<BuildingAI>());
		GameObject.DestroyImmediate(newBuilding.GetComponent<BuildingHPBehavior>());
		
		ResourceStoreBehavior storeBehavior = newBuilding.GetComponent<ResourceStoreBehavior>();
		if(storeBehavior != null)
		{
			GameObject.DestroyImmediate(storeBehavior);
		}
		WallUtility wall = newBuilding.GetComponent<WallUtility>();
		if(wall != null)
		{
			GameObject.DestroyImmediate(wall);
			newBuilding.AddComponent<EditorWallUtility>();
		}
		
		EditorBuildingBehavior buildingBehavior = newBuilding.AddComponent<EditorBuildingBehavior>();
		buildingBehavior.Position = position;
		buildingBehavior.BuildingType = type;
		buildingBehavior.Level = level;
		
		newBuilding.transform.position = PositionConvertor.GetWorldPositionByBuildingTileIndex(position);
		newBuilding.transform.parent = this.m_SceneParent;
		this.PopulateMapData(position, configData.BuildingObstacleList, newBuilding);
	}
	
	public void ConstructDefenseObject(PropsType type)
	{
		PropsDefenseScopeConfigData scopeConfigData = ConfigInterface.Instance.PropsConfigHelper.GetPropsData(type).FunctionConfigData as PropsDefenseScopeConfigData;
		PropsDefenseScopeLastingConfigData lastingConfigData = ConfigInterface.Instance.PropsConfigHelper.GetPropsData(type).FunctionConfigData as PropsDefenseScopeLastingConfigData;
		
		List<TilePoint> buildingObstacleList = scopeConfigData != null ? scopeConfigData.BuildingObstacleList : lastingConfigData.BuildingObstacleList;
		TilePosition centerPosition = PositionConvertor.GetBuildingTileIndexFromWorldPosition(this.m_SceneCamare.position);
		TilePosition position = this.FindValidBuildingPosition(centerPosition, buildingObstacleList);
		if(position != null)
		{
			this.ConstructDefenseObject(type, position);
		}
	}
	
	public void ConstructDefenseObject(PropsType type, TilePosition position)
	{
		PropsDefenseScopeConfigData scopeConfigData = ConfigInterface.Instance.PropsConfigHelper.GetPropsData(type).FunctionConfigData as PropsDefenseScopeConfigData;
		PropsDefenseScopeLastingConfigData lastingConfigData = ConfigInterface.Instance.PropsConfigHelper.GetPropsData(type).FunctionConfigData as PropsDefenseScopeLastingConfigData;
	
		string p = scopeConfigData != null ? scopeConfigData.PrefabName : lastingConfigData.PrefabName;
		List<TilePoint> buildingObstacleList = scopeConfigData != null ? scopeConfigData.BuildingObstacleList : lastingConfigData.BuildingObstacleList;
		
		string prefabName = string.Format("{0}{1}{2}",ClientStringConstants.BATTLE_SCENE_RESOURCE_PREFAB_PREFIX_NAME,
				ClientStringConstants.DEFENSE_OBJECT_PREFAB_PREFIX_NAME, p);
		GameObject objectPrefab = Resources.Load(prefabName) as GameObject;
		GameObject newObject = GameObject.Instantiate(objectPrefab) as GameObject;
		
		GameObject.DestroyImmediate(newObject.GetComponent<DefenseObjectAI>());
		
		EditorDefenseObjectBehavior objectBehavior = newObject.AddComponent<EditorDefenseObjectBehavior>();
		objectBehavior.Position = position;
		objectBehavior.PropsType = type;
		
		
		newObject.transform.position = PositionConvertor.GetWorldPositionByBuildingTileIndex(position);
		newObject.transform.parent = this.m_SceneParent;
		this.PopulateMapData(position, buildingObstacleList, newObject);
	}
	
	
	public void ConstructAchievementBuilding(AchievementBuildingType type)
	{
		AchievementBuildingConfigData configData = ConfigInterface.Instance.AchievementBuildingConfigHelper.GetAchievementBuildingData(type);
		TilePosition centerPosition = PositionConvertor.GetBuildingTileIndexFromWorldPosition(this.m_SceneCamare.position);
		TilePosition position = this.FindValidBuildingPosition(centerPosition, configData.BuildingObstacleList);
		if(position != null)
		{
			this.ConstructAchievementBuilding(type, position);
		}
	}
	
	public void ConstructAchievementBuilding(AchievementBuildingType type, TilePosition position)
	{
		AchievementBuildingConfigData configData = ConfigInterface.Instance.AchievementBuildingConfigHelper.GetAchievementBuildingData(type);
	
		string prefabName = string.Format("{0}{1}{2}",ClientStringConstants.BATTLE_SCENE_RESOURCE_PREFAB_PREFIX_NAME,
				ClientStringConstants.ACHIEVEMENT_BULIDING_PREFAB_PREFIX_NAME, configData.PrefabName);
		GameObject objectPrefab = Resources.Load(prefabName) as GameObject;
		GameObject newObject = GameObject.Instantiate(objectPrefab) as GameObject;
		
		GameObject.DestroyImmediate(newObject.GetComponent<AchievementBuildingHPBehavior>());
		
		EditorAchievementBuildingBehavior objectBehavior = newObject.AddComponent<EditorAchievementBuildingBehavior>();
		objectBehavior.Position = position;
		objectBehavior.AchievementBuildingType = type;
	
		newObject.transform.position = PositionConvertor.GetWorldPositionByBuildingTileIndex(position);
		newObject.transform.parent = this.m_SceneParent;
		this.PopulateMapData(position, configData.BuildingObstacleList, newObject);
	}
	
	public void ConstructRemovableObject(RemovableObjectType type)
	{
		RemovableObjectConfigData configData = ConfigInterface.Instance.RemovableConfigHelper.GetRemovableObjectData(type);
		TilePosition centerPosition = PositionConvertor.GetBuildingTileIndexFromWorldPosition(this.m_SceneCamare.position);
		TilePosition position = this.FindValidBuildingPosition(centerPosition, configData.BuildingObstacleList);
		if(position != null)
		{
			this.ConstructRemovableObject(type, position);
		}
	}
	
	public void ConstructRemovableObject(RemovableObjectType type, TilePosition position)
	{
		RemovableObjectConfigData configData = ConfigInterface.Instance.RemovableConfigHelper.GetRemovableObjectData(type);
	
		string prefabName = string.Format("{0}{1}{2}",ClientStringConstants.BATTLE_SCENE_RESOURCE_PREFAB_PREFIX_NAME,
				ClientStringConstants.REMOVABLE_OBJECT_PREFAB_PREFIX_NAME, configData.PrefabName);
		GameObject objectPrefab = Resources.Load(prefabName) as GameObject;
		GameObject newObject = GameObject.Instantiate(objectPrefab) as GameObject;
		
		EditorRemovableObjectBehavior objectBehavior = newObject.AddComponent<EditorRemovableObjectBehavior>();
		objectBehavior.Position = position;
		objectBehavior.RemovableObjectType = type;
	
		newObject.transform.position = PositionConvertor.GetWorldPositionByBuildingTileIndex(position);
		newObject.transform.parent = this.m_SceneParent;
		this.PopulateMapData(position, configData.BuildingObstacleList, newObject);
	}
	
	public void DestroyBuilding(TilePosition position, BuildingConfigData configData)
	{	
		this.PopulateMapData(position, configData.BuildingObstacleList, null);
	}
	
	public void DestroyAchievementBuilding(TilePosition position, AchievementBuildingConfigData configData)
	{
		this.PopulateMapData(position, configData.BuildingObstacleList, null);
	}
	
	public void DestroyRemovableObject(TilePosition position, RemovableObjectConfigData configData)
	{
		this.PopulateMapData(position, configData.BuildingObstacleList, null);
	}
	
	public void DestroyDefenseObject(TilePosition position, PropsConfigData configData)
	{
		PropsDefenseScopeConfigData scopeConfigData = configData.FunctionConfigData as PropsDefenseScopeConfigData;
		PropsDefenseScopeLastingConfigData lastingConfigData = configData.FunctionConfigData as PropsDefenseScopeLastingConfigData;
		
		List<TilePoint> buildingObstacleList = scopeConfigData != null ? scopeConfigData.BuildingObstacleList : lastingConfigData.BuildingObstacleList;
		this.PopulateMapData(position, buildingObstacleList, null);
	}
	#endregion
	
	#region Private Methods
	private void PopulateMapData(TilePosition position, List<TilePoint> obstacleList, GameObject go)
	{
		foreach (TilePoint point in obstacleList) 
		{
			TilePosition offset = point.ConvertToTilePosition();
			TilePosition temp = position + offset;
			this.m_MapData[temp.Row, temp.Column] = go;
		}
	}
	
	private TilePosition FindValidBuildingPosition(TilePosition startPosition, List<TilePoint> obstacleList)
	{
		List<TilePosition> checkList = new List<TilePosition>();
		List<TilePosition> addList = new List<TilePosition>();
		checkList.Add(startPosition);
		
		int currentLayer = 0;
		
		while(checkList.Count > 0)
		{
			currentLayer ++;
			addList.Clear();
			foreach (TilePosition item in checkList) 
			{
				if(this.IsBuildable(item, obstacleList))
				{
					return item;
				}
				
				TilePosition left = new TilePosition(item.Column - 1, item.Row);
				TilePosition right = new TilePosition(item.Column + 1, item.Row);
				TilePosition top = new TilePosition(item.Column, item.Row + 1);
				TilePosition bottom = new TilePosition(item.Column, item.Row - 1);
				
				if(this.IsNeedCheck(left,startPosition,currentLayer))
				{
					addList.Add(left);
				}
				if(this.IsNeedCheck(right,startPosition,currentLayer))
				{
					addList.Add(right);
				}
				if(this.IsNeedCheck(top,startPosition,currentLayer))
				{
					addList.Add(top);
				}
				if(this.IsNeedCheck(bottom,startPosition,currentLayer))
				{
					addList.Add(bottom);
				}
			}
			checkList.Clear();
			checkList.AddRange(addList);
		}
		
		return null;
	}
	
	private bool IsNeedCheck(TilePosition position, TilePosition startPosition, int currentLayer)
	{
		if(!position.IsValidBuildingTilePosition())
		{
			return false;
		}
		int distance = Mathf.Abs(position.Row - startPosition.Row) + Mathf.Abs(position.Column - startPosition.Column);
		return distance ==  currentLayer;
	}
	
	public bool IsBuildable(TilePosition position, List<TilePoint> obstacleList)
	{
		foreach (TilePoint point in obstacleList) 
		{
			TilePosition offset = point.ConvertToTilePosition();
			TilePosition temp = position + offset;
			if(!temp.IsValidBuildingTilePosition() || this.m_MapData[temp.Row, temp.Column])
			{
				return false;
			}
		}
		return true;
	}
	
	public bool IsBuildable(TilePosition position, List<TilePosition> obstacleList)
	{
		foreach (TilePosition offset in obstacleList) 
		{
			TilePosition temp = position + offset;
			if(!temp.IsValidBuildingTilePosition() || this.m_MapData[temp.Row, temp.Column])
			{
				return false;
			}
		}
		return true;
	}
	#endregion
}
