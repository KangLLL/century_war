using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BorderGennerate : MonoBehaviour { 
    [SerializeField] private GameObject m_GridPrefab;
    [SerializeField] float delay = 1;
    [SerializeField] float duration = 2;
    int m_TimeTick;
    GridType[,] m_GridTypeMapData;
    public BorderType BuildingBorderType
    { get; set; }
    List<tk2dSprite> m_GridSpriteList = new List<tk2dSprite>();
    Color m_From = new Color(0, 1, 0, 0.5f);
    Color m_To = new Color(1, 0, 0, 0);
	// Use this for initialization
	void Start () {
       m_GridTypeMapData = new GridType[ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height, 
			ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width];
       ConstructGridArray();
       CreateGrid();
       DestroySelf();
	}
	
	// Update is called once per frame
    void Update()
    {
        OnFade();
	}
     
    void CreateGrid()
    {
        for (int i = 0; i < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height; i++)
        {
            for (int j = 0; j < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width; j++)
            {
                if (this.m_GridTypeMapData[i, j] != GridType.Out)
                {
					GameObject gridObject = ObjectPoolController.Instantiate(this.m_GridPrefab);//GameObject.Instantiate(this.m_GridPrefab) as GameObject;
                    Vector3 position = PositionConvertor.GetWorldPositionFromBuildingTileIndex(new TilePosition(j, i));
                    float depth = gridObject.transform.position.z;
                    gridObject.transform.position = new Vector3(position.x, position.y, depth);
                    tk2dSprite gridSprite = gridObject.GetComponentInChildren<tk2dSprite>();
                    gridSprite.spriteId = gridSprite.GetSpriteIdByName(this.m_GridTypeMapData[i, j].ToString());
                    gridObject.transform.parent = this.transform;
                    switch (BuildingBorderType)
                    {
                        case BorderType.BuildingBorder:
							gridSprite.color = new Color(1,1,1,0.5f);
                            break;
                        case BorderType.BuildingOutlineBorder:
                            gridSprite.color = m_From;
                            m_GridSpriteList.Add(gridSprite);
                            break;
                    }
                }
            }
        }
    }
 
    void ConstructGridArray()
    {
        bool[,] buildingOwnedArray = new bool[ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height,
			ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width];
        switch(BuildingBorderType)
        {
            case BorderType.BuildingBorder:
                buildingOwnedArray = SceneManager.Instance.BuildingMapData;
                break;
            case BorderType.BuildingOutlineBorder:
                buildingOwnedArray = GetBuildingOutline();
                break;
        }
        for (int i = 0; i < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height; i++)
        {
            for (int j = 0; j < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width; j++)
            {
                if (!buildingOwnedArray[i, j])
                {
                    int result = 0;
                    int left = j - 1;
                    int right = j + 1;
                    int up = i + 1;
                    int bottom = i - 1;
                    if (!up.IsValidBuildingRow() || buildingOwnedArray[up, j])
                    {
						result |= (int)CommonDirection.Up;
                    }
                    if (!bottom.IsValidBuildingRow() || buildingOwnedArray[bottom, j])
                    {
                        result |= (int)CommonDirection.Down;
                    }
                    if (!left.IsValidBuildingColumn() || buildingOwnedArray[i, left])
                    {
                        result |= (int)CommonDirection.Left;
                    }
                    if (!right.IsValidBuildingColumn() || buildingOwnedArray[i, right])
                    {
                         result |= (int)CommonDirection.Right;
                    }
                    ///result = result ^ 0x0000000F;
                    this.m_GridTypeMapData[i, j] = result.ConvertToGridType();
                }
                else
                {
                    this.m_GridTypeMapData[i, j] = GridType.Out;
                }
            }
        }
    }
    bool[,] GetBuildingOutline()
    {
        bool[,] buildingOutlineTileData = new bool[ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height,
			ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width];
        for (int i = 0; i < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height; i++)
        {
            for (int j = 0; j < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width; j++)
            {
                buildingOutlineTileData[i, j] = this.AnalyseOutline(i, j);
            }
        }
        return buildingOutlineTileData;
    }
    bool AnalyseOutline(int row, int column)
    {
        BuildingBehavior buildingBehavior = SceneManager.Instance.GetBuildingBehaviorBaseFromObstacleMap(row, column);
        if (buildingBehavior != null)
            return false;
            //return buildingBehavior.GetType().ToString() == "DefenseObjectBehavior" || buildingBehavior.GetType().ToString() == "RemovableObjectBehavior"; 

        int bottom = row - 1;
        int top = row + 1;
        int left = column - 1;
        int right = column + 1;

        if (bottom.IsValidBuildingRow())
        {
            buildingBehavior = SceneManager.Instance.GetBuildingBehaviorBaseFromObstacleMap(bottom, column);
            if (buildingBehavior != null)
            {
                return false;
            }
            if (left.IsValidBuildingColumn())
            {
                buildingBehavior = SceneManager.Instance.GetBuildingBehaviorBaseFromObstacleMap(bottom, left);
                if (buildingBehavior != null)
                {
                    return false;
                }
            }
            if (right.IsValidBuildingColumn())
            {
                buildingBehavior = SceneManager.Instance.GetBuildingBehaviorBaseFromObstacleMap(bottom, right);
                if (buildingBehavior != null)
                {
                    return false;
                }
            }
        }
        if (top.IsValidBuildingRow())
        {
            buildingBehavior = SceneManager.Instance.GetBuildingBehaviorBaseFromObstacleMap(top, column);
            if (buildingBehavior != null)
            {
                return false;
            }
            if (left.IsValidBuildingColumn())
            {
                buildingBehavior = SceneManager.Instance.GetBuildingBehaviorBaseFromObstacleMap(top, left);
                if (buildingBehavior != null)
                {
                    return false;
                }
            }
            if (right.IsValidBuildingColumn())
            {
                buildingBehavior = SceneManager.Instance.GetBuildingBehaviorBaseFromObstacleMap(top, right);
                if (buildingBehavior != null)
                {
                    return false;
                }
            }
        }
        if (left.IsValidBuildingColumn())
        {
            buildingBehavior = SceneManager.Instance.GetBuildingBehaviorBaseFromObstacleMap(row, left);
            if (buildingBehavior != null)
            {
                return false;
            }
        }
        if (right.IsValidBuildingColumn())
        {
            buildingBehavior = SceneManager.Instance.GetBuildingBehaviorBaseFromObstacleMap(row, right);
            if (buildingBehavior != null)
            {
                return false;
            }
        }
        return true;
    }
    void OnFade()
    {
        if (BuildingBorderType == BorderType.BuildingOutlineBorder)
        {
            m_TimeTick++;
            float seconds = (float)m_TimeTick / ClientConfigConstants.Instance.TicksPerSecond;
            if (seconds> delay)
            {
                if (seconds - delay < duration)
                {
                    float factor = (seconds - delay) / duration;
                    foreach (tk2dSprite tk in m_GridSpriteList)
                    {
                       tk.color = Color.Lerp(m_From, m_To, factor);
                    }
                }
				else
				{
					List<Transform> children = new List<Transform>();
					for(int i = this.transform.childCount - 1; i >=0; i --)
					{
						children.Add(this.transform.GetChild(i));
					}
					foreach (var t in children) {
						t.parent = null;
						ObjectPoolController.Destroy(t.gameObject);
					}
					
					Destroy(this.gameObject);
				}
            }
        }
    }
    void DestroySelf()
    {
        
    }
}
public enum BorderType
{
    BuildingBorder,
    BuildingOutlineBorder
}
