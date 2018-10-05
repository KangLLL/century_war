using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridFactory : MonoBehaviour 
{
	[SerializeField]
	private GameObject m_GridPrefab;
	
	[SerializeField]
	private int m_DisplayCount;
	[SerializeField]
	private Transform m_ParentNode;
	
	private int m_CurrentDisplayCount;
	
	private List<tk2dSprite> m_GridSpriteList;
	
	void Awake()
	{
		this.m_GridSpriteList = new List<tk2dSprite>();
	}
	
	void Update()
	{
		if(this.m_CurrentDisplayCount > 0)
		{
			if(Application.loadedLevelName.Equals(ClientStringConstants.BATTLE_REPLAY_LEVEL_NAME) || 
				(BattleDirector.Instance != null && BattleDirector.Instance.IsBattleStart))
			{
				this.m_CurrentDisplayCount --;
			}
			float percentage = (float)this.m_CurrentDisplayCount / this.m_DisplayCount;
			foreach(tk2dSprite grid in this.m_GridSpriteList)
			{
				if(percentage == 0)
				{
					grid.enabled = false;
				}
				else if(!grid.enabled)
				{
					grid.enabled = true;
				}
				grid.color = new Color(grid.color.r, 0, 0, percentage);
			}
		}
	}
	
	public void ConstructGird()
	{
		this.m_GridSpriteList = new List<tk2dSprite>();
		for(int i = 0; i < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height; i ++)
		{
			for(int j = 0; j < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width; j ++)
			{
				if(BattleMapData.Instance.GridArray[i,j] != GridType.Out)
				{
					float depth = this.m_GridPrefab.transform.position.z;
					GameObject gridObject = (GameObject)ObjectPoolController.InstantiateWithoutPool(this.m_GridPrefab);//GameObject.Instantiate(this.m_GridPrefab) as GameObject;
					Vector3 position = PositionConvertor.GetWorldPositionFromBuildingTileIndex
						(new TilePosition(j,i));
					gridObject.transform.position = new Vector3(position.x, position.y, depth);
					gridObject.transform.parent = this.m_ParentNode;
					tk2dSprite gridSprite = gridObject.GetComponentInChildren<tk2dSprite>();
					gridSprite.spriteId = gridSprite.GetSpriteIdByName(BattleMapData.Instance.GridArray[i, j].ToString());
					this.m_GridSpriteList.Add(gridSprite); 
				}
			}
		}
		
		this.DisplayGrid();
	}
	
	public void DisplayGrid()
	{
		this.m_CurrentDisplayCount = this.m_DisplayCount;
	}
	
	public void Clear()
	{
		for(int i = this.m_GridSpriteList.Count - 1; i >= 0; i --)
		{
			tk2dSprite grid = this.m_GridSpriteList[i];
			GameObject.DestroyImmediate(grid.gameObject);
		}
		this.m_GridSpriteList = new List<tk2dSprite>();
	}
	
}
