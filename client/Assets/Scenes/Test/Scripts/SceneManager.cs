using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums; 
using ConfigUtilities;
using System;
public class SceneManager : MonoBehaviour,IMapData{
    [SerializeField]
    SceneMode m_SceneMode;
    public SceneMode SceneMode { get { return m_SceneMode; } }
    public BuildingBehavior BuildingBehaviorTemporary { get; set; }
	IPickableObject m_PickableObjectCurrentSelect;
    public IPickableObject PickableObjectCurrentSelect { get{ return m_PickableObjectCurrentSelect;} set{ m_PickableObjectCurrentSelect = value; if(value == null) m_MouseOrTouchDictionaryGloble.Clear();} }
    private Dictionary<BuildingType, List<BuildingBehavior>> BuildingBehaviorDictionary { get; set; }
    public Dictionary<BuildingType, List<BuildingLogicData>> BuildingLogicDataDictionary { get; set; }
    public bool[,] BuildingMapData { get; set; }
    //public bool[,] ActorMapData { get; set; }
    public GameObject[,] BuildingGameObjectData { get; set; }
    public GameObject[,] ActorGameObjectData { get; set; }
    [SerializeField] GameObject m_BuildingBorderPrefab;
    [SerializeField] GameObject[] m_BuldingFx;//0 =building ready for update; 1 = building upgrade; 2 =building accelerate ; 3 = dropdown building smoke;
                                              //4 = collect gold1 ; 5 = collect gold4;6 = collect gold3;7 = collect gold4;8 = collect gold5
                                              //9 = collect food1 ; 10 = collect food2;11 = collect food3;12 = collect food4;13 = collect food5
    [SerializeField] GameObject[] m_AwardFx;//0 = gold; 1 = food; 2 = oil ; 3 = gem ;4 = exp;
    [SerializeField] GameObject[] m_AwardProp;//0 = Excellent, 1=Sophisticated, 2=Epic, 3=Legend
    [SerializeField] Color[] m_AwardFxColor;//0 = gold; 1 = food; 2 = oil ; 3 = gem ;4 = exp;
    [SerializeField] GameObject m_AwardText;
    [SerializeField] Vector2 m_AwardTextOffset = new Vector2(0, 0);
	[SerializeField] GameObject m_BorderPrefab;
    GameObject m_BuildingBorderCopy;
    public GameObject AgeMap { get; set; }
    
    #region Wall Globel Function
    List<WallBehavior> CurrentSelectedWallList { get; set; } 
    List<WallBehavior> m_SelectedAllWallList = new List<WallBehavior>();
    public bool EnableCreateWallContinuation { get; set; }
    public TilePosition LastWallTilePosition { get; set; } 
    public WallSelectedMode WallSelectedMode { get; set; }
    public List<WallBehavior> SelectedAllWallList { get { return m_SelectedAllWallList; } set { m_SelectedAllWallList = value; } }
    List<WallBehavior> m_WallListLeft = new List<WallBehavior>(); 
    public List<WallBehavior> WallListLeft { get { return m_WallListLeft; } set { m_WallListLeft = value; } } 
    List<WallBehavior> m_WallListRight = new List<WallBehavior>();
    public List<WallBehavior> WallListRight { get { return m_WallListRight; } set { m_WallListRight = value; } }
    List<WallBehavior> m_WallListTop = new List<WallBehavior>();
    public List<WallBehavior> WallListTop{ get { return m_WallListTop; } set { m_WallListTop = value; } } 
    List<WallBehavior> m_WallListBottom = new List<WallBehavior>(); 
    public List<WallBehavior> WallListBottom{ get { return m_WallListBottom; } set { m_WallListBottom = value; } }
    public bool GetAllBuildingEnableState()
    {
        for (int i = 0; i < SceneManager.Instance.SelectedAllWallList.Count; i++) { if (!SceneManager.Instance.SelectedAllWallList[i].EnableCreate) return false; }
        return true;
    }
    public bool GetAllBuildingBuildState()
    {
        for (int i = 0; i < SceneManager.Instance.SelectedAllWallList.Count; i++) { if (!SceneManager.Instance.SelectedAllWallList[i].IsBuild)return false; }
        return true;
    }
    #endregion
    static SceneManager m_Instance;
    static public SceneManager Instance
    {
        get
        {
            return m_Instance;
        }
    } 
	Dictionary<int, UICamera.MouseOrTouch> m_MouseOrTouchDictionaryGloble = new Dictionary<int, UICamera.MouseOrTouch>();
	public Dictionary<int, UICamera.MouseOrTouch> MouseOrTouchDictionaryGloble { get { return m_MouseOrTouchDictionaryGloble; } set { m_MouseOrTouchDictionaryGloble = value; } }
    void Awake()
    {
        m_Instance = this;
        this.BuildingBehaviorDictionary = new Dictionary<BuildingType, List<BuildingBehavior>>();
        this.BuildingLogicDataDictionary = new Dictionary<BuildingType, List<BuildingLogicData>>();
        InitialMapData();
        GenerateScene(); 
    }
    void Start()
    {
        Resources.UnloadUnusedAssets();
		ObjectPoolController.Preload(this.m_BorderPrefab);
    }
	public void OnPressGloble(bool isPressed)
	{
	    if (isPressed)
        {
            if (!m_MouseOrTouchDictionaryGloble.ContainsKey(UICamera.currentTouchID))
                m_MouseOrTouchDictionaryGloble.Add(UICamera.currentTouchID, UICamera.currentTouch);
        }
        else
            if (m_MouseOrTouchDictionaryGloble.ContainsKey(UICamera.currentTouchID))
                m_MouseOrTouchDictionaryGloble.Remove(UICamera.currentTouchID);

	}
    public void CreateBuildingBorder(BorderType bodrerType = BorderType.BuildingBorder)
    {
        if (m_BuildingBorderCopy != null)
        {
            DestroyBuildingBorder();
        }
        m_BuildingBorderCopy = GameObject.Instantiate(m_BuildingBorderPrefab) as GameObject;
        m_BuildingBorderCopy.name = "Border parent";
        m_BuildingBorderCopy.GetComponent<BorderGennerate>().BuildingBorderType = bodrerType;
    }
    public void DestroyBuildingBorder()
    {
		if(this.m_BuildingBorderCopy != null)
		{
			List<Transform> children = new List<Transform>();
			for(int i = this.m_BuildingBorderCopy.transform.childCount - 1; i >=0; i --)
			{
				children.Add(this.m_BuildingBorderCopy.transform.GetChild(i));
			}
			foreach (var t in children) {
				t.parent = null;
				ObjectPoolController.Destroy(t.gameObject);
			}
	        DestroyImmediate(m_BuildingBorderCopy);
		}
    }

    void InitialMapData()
    {
        int column = ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width;//ClientSystemConstants.MAP_TILE_WIDTH;
        int row = ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height;//ClientSystemConstants.MAP_TILE_HEIGHT;
        int columnActor = ClientSystemConstants.ACTOR_TILE_MAP_SIZE.width;
        int rowActor = ClientSystemConstants.ACTOR_TILE_MAP_SIZE.height;
        this.BuildingMapData = new bool[row, column];
        //this.ActorMapData = new bool[rowActor, columnActor];
        this.BuildingGameObjectData = new GameObject[row, column];
        this.ActorGameObjectData = new GameObject[rowActor, columnActor];
        for (int i = 0; i < column; i++)
        {
            for (int j = 0; j < row; j++)
            {
               this.BuildingMapData[j, i] = true;
            }
        }
        //for (int i = 0; i < columnActor; i++)
        //{
        //    for (int j = 0; j < rowActor; j++)
        //    {
        //        this.ActorMapData[j,i] = true;
        //    }
        //}

    }
    public void ClearAllWallList()
    {
        this.m_WallListLeft.Clear();
        this.m_WallListRight.Clear();
        this.m_WallListTop.Clear();
        this.m_WallListBottom.Clear();
        this.m_SelectedAllWallList.Clear();
    }
    public int WallListColumnCount
    {
        get { return this.m_WallListLeft.Count + this.m_WallListRight.Count; }
    }
    public int WallListRowCount
    {
        get{ return this.m_WallListTop.Count + this.m_WallListBottom.Count;}
    }
    
    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            int column = ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width;
            int row = ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height;
            for (int i = 0; i < column; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    if (SceneManager.Instance.BuildingMapData[j, i] == false)
                    {
                        TilePosition tp = new TilePosition(i, j);
                        Gizmos.color = Color.blue;
                        //Gizmos.DrawSphere(PositionConvertor.GetWorldPositionByBuildingTileIndex(tp), 8);
                        Gizmos.DrawWireCube(PositionConvertor.GetWorldPositionByBuildingTileIndex(tp)+ new Vector3(ClientSystemConstants.BUILDING_TILE_MAP_TILE_SIZE.width>>1, ClientSystemConstants.BUILDING_TILE_MAP_TILE_SIZE.height>>1), new Vector3(40, 40, 40));
                    }
                }
            }
            int columnActor = ClientSystemConstants.ACTOR_TILE_MAP_SIZE.width;
            int rowActor = ClientSystemConstants.ACTOR_TILE_MAP_SIZE.height;
            for (int i = 0; i < columnActor; i++)
            {
                for (int j = 0; j < rowActor; j++)
                {
                    //if (SceneManager.Instance.ActorMapData[j, i] == false)
                    if(this.ActorCanPass(j,i) == false)
                    {
                        TilePosition tp = new TilePosition(i, j);
                        Gizmos.color = Color.magenta;
                        Gizmos.DrawSphere(PositionConvertor.GetWorldPositionByActorTileIndex(tp), 5);
                        //Gizmos.DrawWireCube(PositionConvertor.GetWorldPositionByActorTileIndex(tp) + new Vector3(ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.width >> 1, ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.height >> 1), new Vector3(20, 20, 20));

                    }
                }
            }
        }
    }

    void GenerateScene()
    {
        List<BuildingLogicData> allBuildings = new List<BuildingLogicData>();
        List<RemovableObjectLogicData> allRemovableObjects = new List<RemovableObjectLogicData>();
        List<DefenseObjectLogicData> allDefenseObjectLogicData = new List<DefenseObjectLogicData>();
        List<AchievementBuildingLogicData> allAchievementBuildingLogicData = new List<AchievementBuildingLogicData>();
		Age age = Age.Prehistoric;
		
        switch (this.m_SceneMode)
        {
            case SceneMode.SceneBuild:
                allBuildings = LogicController.Instance.AllBuildings;
                allRemovableObjects = LogicController.Instance.AllRemovableObjects;
                allDefenseObjectLogicData = LogicController.Instance.AllDefenseObjects;
                allAchievementBuildingLogicData = LogicController.Instance.AllAchievementBuildings;
				age = CommonUtilities.CommonUtilities.GetAgeFromCityHallLevel(LogicController.Instance.GetBuildingObject(new BuildingIdentity(BuildingType.CityHall,0)).Level);
                break;
            case SceneMode.SceneVisit:
                allBuildings = LogicController.Instance.CurrentFriend.AllBuildings;
                allRemovableObjects = LogicController.Instance.CurrentFriend.AllRemovableObjects;
                allAchievementBuildingLogicData = LogicController.Instance.CurrentFriend.AllAchievementBuildings;
				age = CommonUtilities.CommonUtilities.GetAgeFromCityHallLevel(LogicController.Instance.CurrentFriend.GetBuildingData(new BuildingIdentity(BuildingType.CityHall, 0)).Level);
                break;
        }
		
		LoadAgeMap.Instance.SetMap(age);
        LoadSceneMusic.Instance.SetSceneMusic(age);
        for (int i = 0, count = allBuildings.Count; i < count; i++)
        {
            this.ConstructBuilding(allBuildings[i], true);
        }

        for (int i = 0; i < allRemovableObjects.Count; i++)
        {
            this.ConstructObstacle(allRemovableObjects[i]);
        }
        for (int i = 0; i < allDefenseObjectLogicData.Count; i++)
        {
            this.ConstructDefenseProp(allDefenseObjectLogicData[i],true);
        }
        for (int i = 0; i < allAchievementBuildingLogicData.Count; i++)
        {
            this.ConstructAchievementBuilding(allAchievementBuildingLogicData[i]);
        }
        this.AgeMap = LoadAgeMap.Instance.CurrentMap;

    }
 
    public void AddBuildingBehavior(BuildingBehavior item)
    {
       BuildingType type = item.BuildingType;
       if (BuildingBehaviorDictionary.ContainsKey(type))
       {
           BuildingBehaviorDictionary[type].Add(item);
       }
       else
       {
           List<BuildingBehavior> itemList = new List<BuildingBehavior>();
           itemList.Add(item);
           BuildingBehaviorDictionary.Add(type, itemList);
       }
    }
    public void RemoveBuildingBehavior(BuildingBehavior item)
    {
        BuildingType type = item.BuildingType;
        if (BuildingBehaviorDictionary.ContainsKey(type))
        {
            if (BuildingBehaviorDictionary[type].Contains(item))
            {
                BuildingBehaviorDictionary[type].Remove(item);
                Destroy(item.gameObject);
            }
        }
    }
    public void ConstructObstacle(RemovableObjectLogicData removableObjectLogicData)
    {
        GameObject obstacle = Instantiate(Resources.Load(ClientStringConstants.BUILDING_SCENE_RESOURCE_PREFAB_PREFIX_NAME + ClientStringConstants.REMOVABLE_OBJECT_PREFAB_PREFIX_NAME + removableObjectLogicData.PrefabName, typeof(GameObject))) as GameObject;
        obstacle.transform.parent = SceneManager.Instance.transform;
        RemovableObjectBehavior obstacleBehavior = obstacle.transform.GetComponent<RemovableObjectBehavior>();
        obstacleBehavior.RemovableObjectLogicData = removableObjectLogicData;
        obstacleBehavior.RemovableObjectType = removableObjectLogicData.ObjectType;
        obstacleBehavior.Created = true;
    }
    public void ConstructObstacle(RemovableObjectConfigData removableObjectConfigData, RemovableObjectType removableObjectType ,ProductRemovableObjectConfigData productRemovableObjectConfigData)
    {
        GameObject obstacle = Instantiate(Resources.Load(ClientStringConstants.BUILDING_SCENE_RESOURCE_PREFAB_PREFIX_NAME + ClientStringConstants.REMOVABLE_OBJECT_PREFAB_PREFIX_NAME + removableObjectConfigData.PrefabName, typeof(GameObject))) as GameObject;
        obstacle.transform.parent = SceneManager.Instance.transform;
        RemovableObjectBehavior obstacleBehavior = obstacle.transform.GetComponent<RemovableObjectBehavior>();
        obstacleBehavior.RemovableObjectConfigData = removableObjectConfigData;
        obstacleBehavior.RemovableObjectType = removableObjectType;
        obstacleBehavior.ProductRemovableObjectConfigData = productRemovableObjectConfigData;
        SceneManager.Instance.BuildingBehaviorTemporary = obstacleBehavior;
        obstacleBehavior.Created = false;
    }
    public BuildingBehavior ConstructBuilding(BuildingLogicData buildingLogicData, bool created)
    {
        GameObject building = Instantiate(Resources.Load(ClientStringConstants.BUILDING_SCENE_RESOURCE_PREFAB_PREFIX_NAME + ClientStringConstants.BUILDING_OBJECT_PREFAB_PREFIX_NAME + buildingLogicData.BuildingPrefabName, typeof(GameObject))) as GameObject;
        
        building.transform.parent = SceneManager.Instance.transform;
        BuildingBehavior buildingBehavior = building.transform.GetComponent<BuildingBehavior>();
        buildingBehavior.BuildingLogicData = buildingLogicData;
        buildingBehavior.BuildingConfigData = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(buildingLogicData.BuildingIdentity.buildingType, buildingLogicData.Level);
        buildingBehavior.BuildingType = buildingLogicData.BuildingIdentity.buildingType;
        buildingBehavior.Created = created;
		
		
		BuildingSurfaceBehavior surface = building.GetComponent<BuildingSurfaceBehavior>();
		if(surface != null)
		{
			surface.SetSurface(LoadAgeMap.Instance.CurrentAge, buildingLogicData.BuildingType);
		}
        return buildingBehavior;
    }
    public void ConstructBuilding(BuildingConfigData buildingConfigData, BuildingType buildingType, bool created)
    {
        GameObject building = Instantiate(Resources.Load(ClientStringConstants.BUILDING_SCENE_RESOURCE_PREFAB_PREFIX_NAME + ClientStringConstants.BUILDING_OBJECT_PREFAB_PREFIX_NAME + buildingConfigData.BuildingPrefabName, typeof(GameObject))) as GameObject;
        building.transform.parent = SceneManager.Instance.transform;
       
        BuildingBehavior buildingBehavior = building.transform.GetComponent<BuildingBehavior>();
        buildingBehavior.BuildingConfigData = buildingConfigData;
        buildingBehavior.BuildingType = buildingType;
        buildingBehavior.Created = created;
        if (!created)
            SceneManager.Instance.BuildingBehaviorTemporary = buildingBehavior;
		
		BuildingSurfaceBehavior surface = building.GetComponent<BuildingSurfaceBehavior>();
		if(surface != null)
		{
			surface.SetSurface(LoadAgeMap.Instance.CurrentAge, buildingType);
		}
    }
    public void ConstructDefenseProp(DefenseObjectConfigWrapper defenseObjectConfigWrapper, PropsLogicData propsLogicData, bool created)
    {
        GameObject prop = Instantiate(Resources.Load(ClientStringConstants.BUILDING_SCENE_RESOURCE_PREFAB_PREFIX_NAME + ClientStringConstants.DEFENSE_OBJECT_PREFAB_PREFIX_NAME + defenseObjectConfigWrapper.PrefabName, typeof(GameObject))) as GameObject;
        prop.transform.parent = SceneManager.Instance.transform;
        DefenseObjectBehavior defenseObjectBehavior = prop.transform.GetComponent<DefenseObjectBehavior>();
        defenseObjectBehavior.DefenseObjectConfigData = defenseObjectConfigWrapper;
        defenseObjectBehavior.PropsLogicData = propsLogicData;
        //defenseObjectBehavior.PropsType = propsType;
        defenseObjectBehavior.Created = created;
        if (!created)
            SceneManager.Instance.BuildingBehaviorTemporary = defenseObjectBehavior;
    }
    public BuildingBehavior ConstructDefenseProp(DefenseObjectLogicData defenseObjectLogicData, bool created)
    {
        GameObject prop = Instantiate(Resources.Load(ClientStringConstants.BUILDING_SCENE_RESOURCE_PREFAB_PREFIX_NAME + ClientStringConstants.DEFENSE_OBJECT_PREFAB_PREFIX_NAME + defenseObjectLogicData.PrefabName, typeof(GameObject))) as GameObject;
        prop.transform.parent = SceneManager.Instance.transform;
        DefenseObjectBehavior defenseObjectBehavior = prop.transform.GetComponent<DefenseObjectBehavior>();
        defenseObjectBehavior.DefenseObjectLogicData = defenseObjectLogicData;
        //defenseObjectBehavior.PropsDefenseScopeConfigData = propsDefenseScopeConfigData;
        //defenseObjectBehavior.PropsType = propsType;defenseObjectLogicData.
        defenseObjectBehavior.Created = created;
        return defenseObjectBehavior;
    }
    public void ConstructAchievementBuilding(AchievementBuildingConfigData achievementBuildingConfigData, AchievementBuildingType achievementBuildingType)
    {
        GameObject building = Instantiate(Resources.Load(ClientStringConstants.BUILDING_SCENE_RESOURCE_PREFAB_PREFIX_NAME + ClientStringConstants.ACHIEVEMENT_BULIDING_PREFAB_PREFIX_NAME + achievementBuildingConfigData.PrefabName, typeof(GameObject))) as GameObject;
        building.transform.parent = SceneManager.Instance.transform;
        AchievementBuildingBehavior achievementBuildingBehavior = building.transform.GetComponent<AchievementBuildingBehavior>();
        achievementBuildingBehavior.AchievementBuildingConfigData = achievementBuildingConfigData;
        achievementBuildingBehavior.AchievementBuildingType = achievementBuildingType;
        achievementBuildingBehavior.Created = false; 
        SceneManager.Instance.BuildingBehaviorTemporary = achievementBuildingBehavior;

        BuildingSurfaceBehavior surface = building.GetComponent<BuildingSurfaceBehavior>();
        if (surface != null)
        {
            surface.SetSurface(LoadAgeMap.Instance.CurrentAge, achievementBuildingType);
        }
    }
    public void ConstructAchievementBuilding(AchievementBuildingLogicData achievementBuildingLogicData)
    {
        GameObject building = Instantiate(Resources.Load(ClientStringConstants.BUILDING_SCENE_RESOURCE_PREFAB_PREFIX_NAME + ClientStringConstants.ACHIEVEMENT_BULIDING_PREFAB_PREFIX_NAME + achievementBuildingLogicData.PrefabName, typeof(GameObject))) as GameObject;
        building.transform.parent = SceneManager.Instance.transform;
        AchievementBuildingBehavior achievementBuildingBehavior = building.transform.GetComponent<AchievementBuildingBehavior>();
        achievementBuildingBehavior.AchievementBuildingLogicData = achievementBuildingLogicData;
        achievementBuildingBehavior.AchievementBuildingType = achievementBuildingLogicData.AchievementBuildingType;
        achievementBuildingBehavior.Created = true;
        BuildingSurfaceBehavior surface = building.GetComponent<BuildingSurfaceBehavior>();
        if (surface != null)
        {
            surface.SetSurface(LoadAgeMap.Instance.CurrentAge, achievementBuildingLogicData.AchievementBuildingType);
        }
    }
    public void DestroyTemporaryBuildingBehavior()
    {
        if (this.BuildingBehaviorTemporary != null)
        {
            Destroy(this.BuildingBehaviorTemporary.gameObject);
        }
    }
    public void UnSelectBuilding()
    {
        if (SceneManager.Instance.PickableObjectCurrentSelect != null)
        {
            SceneManager.Instance.PickableObjectCurrentSelect.OnUnSelect(true);
            SceneManager.Instance.PickableObjectCurrentSelect = null;
        }
    }
    public bool[,] ActorObstacleArray
    {
        get
        {
            int height = this.ActorGameObjectData.GetLength(0);
            int width = this.ActorGameObjectData.GetLength(1);
            bool[,] result = new bool[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    result[i, j] = this.ActorGameObjectData[i, j] != null;
                }
            }
            return result;

        }
    }

    public bool ActorCanPass(int row, int column)
    {
        return this.ActorGameObjectData[row, column] == null;
    }



    public GameObject GetBuildingObjectFromBuildingObstacleMap(int row, int column)
    {
        if (row >= 0 && column >= 0 && row < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height && column < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width)
            return this.BuildingGameObjectData[row, column];
        return null;
    }


    public GameObject GetBulidingObjectFromActorObstacleMap(int row, int column)
    {
        if (row >= 0 && column >= 0 && row < ClientSystemConstants.ACTOR_TILE_MAP_SIZE.height && column < ClientSystemConstants.ACTOR_TILE_MAP_SIZE.height)
            return this.ActorGameObjectData[row, column];
        return null;
    }

	public IObstacleInfo GetObstacleInfoFormActorObstacleMap(int row, int column)
    {
        if (row >= 0 && column >= 0 && row < ClientSystemConstants.ACTOR_TILE_MAP_SIZE.height && column < ClientSystemConstants.ACTOR_TILE_MAP_SIZE.height)
            if (this.ActorGameObjectData[row, column] != null)
		{
			GameObject go = this.ActorGameObjectData[row, column];
			BuildingBehavior behavior = go.GetComponent<BuildingBehavior>();
			if(behavior != null)
			{
				return behavior.BuildingLogicData;
			}
			else
			{
				return go.GetComponent<RemovableObjectBehavior>().RemovableObjectLogicData; 
			}
		}
        return null;
    }
    public BuildingBehavior GetBuildingBehaviorFromObstacleMap(int row, int column)
    {
        if (row >= 0 && column >= 0 && row < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height && column < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width)
            if (this.BuildingGameObjectData[row, column] != null) 
                return this.BuildingGameObjectData[row, column].GetComponent < BuildingBehavior>();
        return null;
    }
    public BuildingBehavior GetBuildingBehaviorBaseFromObstacleMap(int row, int column)
    {
        if (row >= 0 && column >= 0 && row < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height && column < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width)
            if (this.BuildingGameObjectData[row, column] != null)
            {
                BuildingBehavior buildingBehavior = this.BuildingGameObjectData[row, column].GetComponent<BuildingBehavior>();
                string typeString = buildingBehavior.GetType().ToString();
                if (typeString == "BuildingBehavior" || typeString == "AchievementBuildingBehavior" || typeString == "WallBehavior")
                    return buildingBehavior;
                else
                    return null;
            }
        return null;
    }
	/*
    public IBuildingInfo GetBuildingInfoFormActorObstacleMap(int row, int column)
    {
        if (row >= 0 && column >= 0 && row < ClientSystemConstants.ACTOR_TILE_MAP_SIZE.height && column < ClientSystemConstants.ACTOR_TILE_MAP_SIZE.height)
            if (this.ActorGameObjectData[row, column] != null)
                return this.ActorGameObjectData[row, column].GetComponent<BuildingBehavior>().BuildingLogicData;
        return null;
    }
    */
    public IObstacleInfo GetBuildingInfoFormBuildingObstacleMap(int row, int column)
    { 
        if (row >= 0 && column >= 0 && row < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height && column < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width)
            if (this.BuildingGameObjectData[row, column] != null)
            {
                GameObject go = this.BuildingGameObjectData[row, column];
                BuildingBehavior behavior = go.GetComponent<BuildingBehavior>();
                if (behavior != null)
                {
                    return behavior.BuildingLogicData;
                }
                else
                {
                    return go.GetComponent<RemovableObjectBehavior>().RemovableObjectLogicData;
                }
            }
        return null;
    }
   
    #region Find building obstacle from reference point

    public bool FindObstacle(BuildingObstacleDirection wallDirection, TilePosition refTilePosition, int index)
    {
        TilePosition tilePositionNext = null;
        switch (wallDirection)
        {
            case BuildingObstacleDirection.Top:
                tilePositionNext = new TilePosition(refTilePosition.Column, refTilePosition.Row + index);
                break;
            case BuildingObstacleDirection.Bottom:
                tilePositionNext = new TilePosition(refTilePosition.Column, refTilePosition.Row - index);
                break;
            case BuildingObstacleDirection.Left:
                tilePositionNext = new TilePosition(refTilePosition.Column - index, refTilePosition.Row);
                break;
            case BuildingObstacleDirection.Right:
                tilePositionNext = new TilePosition(refTilePosition.Column + index, refTilePosition.Row);
                break;
        }
        //if (tilePositionNext.Row < 0 || tilePositionNext.Column < 0 || tilePositionNext.Row >= ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height || tilePositionNext.Column >= ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width)
        if (!tilePositionNext.IsValidBuildingTilePosition() || tilePositionNext.IsEdgeBuildingTilePosition())
            return false;
        return SceneManager.Instance.BuildingMapData[tilePositionNext.Row, tilePositionNext.Column];
    }

    public TilePosition FindBuildNextPoint()
    {
        TilePosition tilePosition = null;
        if (SceneManager.Instance.LastWallTilePosition != null)
        {
            BuildingObstacleDirection wallDirection = BuildingObstacleDirection.None;
            if (!FindObstacle(BuildingObstacleDirection.Top, SceneManager.Instance.LastWallTilePosition, 1))
                wallDirection |= BuildingObstacleDirection.Top;
            if (!FindObstacle(BuildingObstacleDirection.Bottom, SceneManager.Instance.LastWallTilePosition, 1))
                wallDirection |= BuildingObstacleDirection.Bottom;
            if (!FindObstacle(BuildingObstacleDirection.Left, SceneManager.Instance.LastWallTilePosition, 1))
                wallDirection |= BuildingObstacleDirection.Left;
            if (!FindObstacle(BuildingObstacleDirection.Right, SceneManager.Instance.LastWallTilePosition, 1))
                wallDirection |= BuildingObstacleDirection.Right;
            //Right  ¡ú
            if (wallDirection == BuildingObstacleDirection.Left || wallDirection == BuildingObstacleDirection.TopLeft || wallDirection == BuildingObstacleDirection.TopBottom || wallDirection == BuildingObstacleDirection.TopLeftBottom || wallDirection == BuildingObstacleDirection.LeftBottom || wallDirection == BuildingObstacleDirection.All || wallDirection == BuildingObstacleDirection.None)
                tilePosition = new TilePosition(SceneManager.Instance.LastWallTilePosition.Column + 1, SceneManager.Instance.LastWallTilePosition.Row);
            //Top ¡ü
            if (wallDirection == BuildingObstacleDirection.Bottom || wallDirection == BuildingObstacleDirection.RightBottomLeft)
                tilePosition = new TilePosition(SceneManager.Instance.LastWallTilePosition.Column, SceneManager.Instance.LastWallTilePosition.Row + 1);
            //Bottom ¡ý
            if (wallDirection == BuildingObstacleDirection.Top || wallDirection == BuildingObstacleDirection.RightTopLeft || wallDirection == BuildingObstacleDirection.RightLeft)
                tilePosition = new TilePosition(SceneManager.Instance.LastWallTilePosition.Column, SceneManager.Instance.LastWallTilePosition.Row - 1);
            //Left ¡û
            if (wallDirection == BuildingObstacleDirection.Right || wallDirection == BuildingObstacleDirection.RightTop || wallDirection == BuildingObstacleDirection.TopRightBottom || wallDirection == BuildingObstacleDirection.RightBottom)
                tilePosition = new TilePosition(SceneManager.Instance.LastWallTilePosition.Column - 1, SceneManager.Instance.LastWallTilePosition.Row);
        }
        return tilePosition;
    }
    #endregion

    #region Bulidng Fx
    public GameObject CreateFX(Transform refObject,FxType fxType,bool isChild = true)
    {
        GameObject goFX = Instantiate(m_BuldingFx[(int)fxType]) as GameObject;
        if (isChild)
        {
            goFX.transform.parent = refObject;
            goFX.transform.localPosition = Vector3.zero;
        }
        else 
            goFX.gameObject.transform.position = refObject.transform.position;
        return goFX;
    }
    public GameObject CreateReadyForUpgradeFX(BuildingBehavior buildingBehavior)
    {
        AudioController.Play("BuildingLevelUp");
        return this.CreateFX(buildingBehavior.BuildingAnchor, (FxType)Enum.Parse(typeof(FxType), Enum.GetName(typeof(ReadyForUpdateFxType), ((ReadyForUpdateFxType)((int)Enum.Parse(typeof(BuildingTypeMapToFX), Enum.GetName(typeof(BuildingType), buildingBehavior.BuildingType)))))));  
    }
    public void CreateUpgradeFX(BuildingBehavior buildingBehavior)
    {
        BuildingLogicData bld = buildingBehavior.BuildingLogicData;
        this.CreateAwardFX(buildingBehavior.BuildingAnchor, bld.UpgradeRewardGold, bld.UpgradeRewardFood, bld.UpgradeRewardOil, bld.UpgradeRewardGem, bld.UpgradeRewardExp);
        AudioController.Play("BuildingLevelUp");
        print("CreateUpgradeFX=========================");
        this.CreateFX(buildingBehavior.BuildingAnchor, (FxType)Enum.Parse(typeof(FxType), Enum.GetName(typeof(UpgradeFxType), ((UpgradeFxType)((int)Enum.Parse(typeof(BuildingTypeMapToFX), Enum.GetName(typeof(BuildingType), buildingBehavior.BuildingType)))))),false);
        //this.CreateAwardFxText(buildingBehavior.BuildingAnchor, bld.UpgradeRewardExp.ToString(), this.m_AwardFxColor[4]);
    }
    public void CreateObstacleUpgradeFX(RemovableObjectBehavior removableObjectBehavior)
    {
        AudioController.Play("BuildingLevelUp");
        this.CreateFX(removableObjectBehavior.BuildingAnchor, (FxType)Enum.Parse(typeof(FxType), Enum.GetName(typeof(UpgradeFxType), ((UpgradeFxType)((int)Enum.Parse(typeof(RemovableObjectTypeMapToFX), Enum.GetName(typeof(RemovableObjectType), removableObjectBehavior.RemovableObjectLogicData.ObjectType)))))), false);
    }
    public void CreateCollectFX(BuildingBehavior buildingBehavior, ResourceType type, int collected, BuildingLogicData buildingLogicData)
    {
        switch (type)
        {
            case ResourceType.Gold:
                {
                    int percentage = SystemFunction.GetCollectPercentageRange(collected, buildingLogicData, ResourceType.Gold);
                    this.CreateFX(buildingBehavior.BuildingAnchor, (FxType)Enum.Parse(typeof(FxType), Enum.GetName(typeof(CollectGoldFxType), (CollectGoldFxType)percentage)));
                    AudioController.Play("GoldCollect");
                    this.CreateAwardFxText(buildingBehavior.BuildingAnchor, collected.ToString(), this.m_AwardFxColor[0]);
                }
                break;
            case ResourceType.Food:
                {
                    int percentage = SystemFunction.GetCollectPercentageRange(collected, buildingLogicData, ResourceType.Food);
                    this.CreateFX(buildingBehavior.BuildingAnchor, (FxType)Enum.Parse(typeof(FxType), Enum.GetName(typeof(CollectFoodFxType), (CollectFoodFxType)percentage)));
                    AudioController.Play("FoodCollect");
                    this.CreateAwardFxText(buildingBehavior.BuildingAnchor, collected.ToString(), this.m_AwardFxColor[1]);
                }
                break;
            case ResourceType.Oil:
                {
                    //int percentage = SystemFunction.GetCollectPercentage(buildingLogicData, ResourceType.Oil);
                    //this.CreateFX(buildingBehavior.BuildingAnchor, (FxType)Enum.Parse(typeof(FxType), Enum.GetName(typeof(CollectOilFxType), (CollectOilFxType)percentage)));
                    //AudioController.Play("OilCollect");
                }
                break;
        }
    }
    public GameObject CreateAccelerateFX(BuildingBehavior buildingBehavior)
    {
        AudioController.Play("BuildingBoost");
        return this.CreateFX(buildingBehavior.BuildingAnchor, (FxType)Enum.Parse(typeof(FxType), Enum.GetName(typeof(AccelerateFxType), ((AccelerateFxType)((int)Enum.Parse(typeof(BuildingTypeMapToFX), Enum.GetName(typeof(BuildingType), buildingBehavior.BuildingType)))))));  
    }
    public GameObject CreateReadyForRemoveFX(RemovableObjectBehavior removableObjectBehavior)
    {
        AudioController.Play("BuildingLevelUp");
        return this.CreateFX(removableObjectBehavior.BuildingAnchor, (FxType)Enum.Parse(typeof(FxType), Enum.GetName(typeof(ReadyForUpdateFxType), ((ReadyForUpdateFxType)((int)Enum.Parse(typeof(RemovableObjectTypeMapToFX), Enum.GetName(typeof(RemovableObjectType), removableObjectBehavior.RemovableObjectLogicData.ObjectType)))))));
    }
    public void CreateSmokeFX(BuildingBehavior buildingBehavior)
    {
        AudioController.Play("BuildingDrop");
        this.CreateFX(buildingBehavior.BuildingAnchor, (FxType)Enum.Parse(typeof(FxType), Enum.GetName(typeof(SmokeFxType), ((SmokeFxType)((int)Enum.Parse(typeof(BuildingTypeMapToFX), Enum.GetName(typeof(BuildingType), buildingBehavior.BuildingType)))))));  
    }
    public void CreateSmokeFX(AchievementBuildingBehavior achievementBuildingBehavior)
    {
        AudioController.Play("BuildingDrop");
        this.CreateFX(achievementBuildingBehavior.BuildingAnchor, (FxType)Enum.Parse(typeof(FxType), Enum.GetName(typeof(SmokeFxType), ((SmokeFxType)((int)Enum.Parse(typeof(AchievementBuildingTypeMapToFX), Enum.GetName(typeof(AchievementBuildingType), achievementBuildingBehavior.AchievementBuildingType)))))));  
    }
    public void CreateSmokeFX(RemovableObjectBehavior removableObjectBehavior)
    {
        AudioController.Play("BuildingDrop");
        //this.CreateFX(removableObjectBehavior.BuildingAnchor, (FxType)Enum.Parse(typeof(FxType), Enum.GetName(typeof(SmokeFxType), ((SmokeFxType)((int)Enum.Parse(typeof(RemovableObjectTypeMapToFX), Enum.GetName(typeof(RemovableObjectType), removableObjectBehavior.RemovableObjectLogicData.ObjectType)))))));  
    }
    #endregion

    #region  Award Fx
    public void CreateAwardFX(Transform refObject, params int[] reward)
    {
        for (int i = 0; i < reward.Length; i++)
        {
            if (reward[i] > 0)
            {
                if (i == 3)//gem
                {
                    for (int j = 0; j < reward[i]; j++)
                    {
                        GameObject goFX = Instantiate(m_AwardFx[i]) as GameObject;
                        goFX.gameObject.transform.position = refObject.transform.position;
                    }
                }
                else//gold food oil exp
                {
                    GameObject goFX = Instantiate(m_AwardFx[i]) as GameObject;
                    goFX.gameObject.transform.position = refObject.transform.position;
                }
            }
        }
    }
    public void CreatePropFX(Transform refObject, PropsConfigData propsConfigData)
    {
        GameObject goFX = Instantiate(m_AwardProp[propsConfigData.Quality]) as GameObject;
        goFX.gameObject.transform.position = refObject.transform.position;
    }
    void CreateAwardFxText(Transform refObject, string text, Color color)
    {
        GameObject goFX = Instantiate(this.m_AwardText) as GameObject;
        Vector3 position = Vector3.zero;
        position.x = refObject.position.x + m_AwardTextOffset.x;
        position.y = refObject.position.y + m_AwardTextOffset.y;
        position.z = -200;
        goFX.gameObject.transform.position = position;
        goFX.GetComponent<AwardTextBehaviour>().SetText(text, color);
    }
    #endregion

}
public enum SceneMode
{
    SceneBuild,
    SceneVisit
}
