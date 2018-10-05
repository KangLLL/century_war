using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
using ConfigUtilities;
using ConfigUtilities.Structs;
using System.Collections.Generic;

public class EditorBuildingBehavior : MonoBehaviour 
{
	private BuildingType m_BuildingType;
	private int m_Level;
	
	public BuildingType BuildingType 
	{ 
		get
		{
			return this.m_BuildingType;
		}
		set
		{
			this.m_BuildingType = value;
		}
	}
	public int Level 
	{ 
		get
		{
			return this.m_Level;
		}
		set
		{
			this.m_Level = value;
			this.m_ConfigData = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(this.BuildingType, this.Level);
			this.m_BuildingObstacle = new List<TilePosition>();
			foreach (TilePoint point in this.m_ConfigData.BuildingObstacleList) 
			{	
				this.m_BuildingObstacle.Add(point.ConvertToTilePosition());
			}
		}
	}
	
	public TilePosition Position { get; set; }
	
	private BuildingConfigData m_ConfigData;
	private List<TilePosition> m_BuildingObstacle;
	
	private TilePosition m_OriginalPosition;
	private TilePosition m_SelectOffset;
	
	private List<EditorBuildingBehavior> m_Selections = new List<EditorBuildingBehavior>();
	
	private const string BUILDING_BACKGROUND_OBJECT_NAME = "BuildingBackground";
	
	void OnPress(bool isPressed)
	{
		if(UICamera.currentTouchID == -1)
		{
			if(isPressed)
			{
				if(Input.GetKey(KeyCode.A))
				{
					this.SelectRow();
				}
				else if(Input.GetKey(KeyCode.B))
				{
					this.SelectColumn();
				}
				else
				{
					this.m_Selections.Clear();
					this.m_Selections.Add(this);
					this.Select(new TilePosition(0,0));
				}
			}
			else
			{
				this.DropDown();
			}
		}
	}
	
	void OnDrag(Vector2 delta)
	{
		if(UICamera.currentTouchID == -1)
		{
			Vector3 worldPosition = EditorConfigInterface.Instance.SceneCamera.ScreenToWorldPoint(Input.mousePosition);
			TilePosition currentPosition = PositionConvertor.GetBuildingTileIndexFromWorldPosition(worldPosition) + this.m_SelectOffset;
			this.Position = currentPosition;
			
			this.gameObject.transform.position = PositionConvertor.GetWorldPositionByBuildingTileIndex(this.Position);
			
			foreach (var b in this.m_Selections) 
			{
				if(b != this)
				{
					b.OnDrag(delta);
				}
			}
		}
	}
	
	void OnClick()
	{
		if(UICamera.currentTouchID == -2)
		{
			if(Input.GetKey(KeyCode.LeftControl))
			{
				this.Degrade();
			}
			else
			{
				this.Upgrade();
			}
		}
		else if(UICamera.currentTouchID == -3)
		{
			this.Delete();
		}
	}
	
	void Start()
	{
		Transform anchor = this.transform.FindChild(ClientStringConstants.BUILDING_ANCHOR_OBJECT_NAME);
		tk2dSpriteAnimator sp = anchor.FindChild(BUILDING_BACKGROUND_OBJECT_NAME).
			GetComponent<tk2dSpriteAnimator>();
		
		//Debug.Log(anchor == null);
		//Debug.Log(sp == null);
		
		BoxCollider c = this.gameObject.AddComponent<BoxCollider>();
		c.center = anchor.localPosition + sp.Sprite.GetBounds().center;
		c.size = sp.Sprite.GetBounds().extents;
			
	}
	
	public void Select(TilePosition offset)
	{
		this.m_OriginalPosition = this.Position;
		this.m_SelectOffset = offset;
		this.Move();
	}
	
	private void SelectRow()
	{
		this.m_Selections.Clear();
		
		int initialColumn = this.Position.Column;
		//int initialRow = this.Position.Row;
		int currentColumn = this.Position.Column;
		int currentRow = this.Position.Row;
		while(currentColumn >=  0 && EditorFactory.Instance.MapData[currentRow, currentColumn] != null)
		{
			EditorBuildingBehavior b = EditorFactory.Instance.MapData[currentRow, currentColumn].
				GetComponent<EditorBuildingBehavior>();
			if(b == null)
			{
				break;
			}
			if(!this.m_Selections.Contains(b))
			{
				this.m_Selections.Add(b);
			}
			currentColumn --;
		}
		currentColumn = initialColumn;
		while(currentColumn < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width && EditorFactory.Instance.MapData[currentRow, currentColumn] != null)
		{
			EditorBuildingBehavior b = EditorFactory.Instance.MapData[currentRow, currentColumn].
				GetComponent<EditorBuildingBehavior>();
			if(b == null)
			{
				break;
			}
			if(!this.m_Selections.Contains(b))
			{
				this.m_Selections.Add(b);
			}
			currentColumn ++;
		}
		
		foreach (EditorBuildingBehavior b in this.m_Selections) 
		{
			b.Select(b.Position - this.Position);
		}
	}
	
	private void SelectColumn()
	{
		this.m_Selections.Clear();
		
		//int initialColumn = this.Position.Column;
		int initialRow = this.Position.Row;
		int currentColumn = this.Position.Column;
		int currentRow = this.Position.Row;
		while(currentRow >=  0 && EditorFactory.Instance.MapData[currentRow, currentColumn] != null)
		{
			EditorBuildingBehavior b = EditorFactory.Instance.MapData[currentRow, currentColumn].
				GetComponent<EditorBuildingBehavior>();
			if(b == null)
			{
				break;
			}
			if(!this.m_Selections.Contains(b))
			{
				this.m_Selections.Add(b);
			}
			currentRow --;
		}
		currentRow = initialRow;
		while(currentRow < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height && EditorFactory.Instance.MapData[currentRow, currentColumn] != null)
		{
			EditorBuildingBehavior b = EditorFactory.Instance.MapData[currentRow, currentColumn].
				GetComponent<EditorBuildingBehavior>();
			if(b == null)
			{
				break;
			}
			if(!this.m_Selections.Contains(b))
			{
				this.m_Selections.Add(b);
			}
			currentRow ++;
		}
		
		foreach (EditorBuildingBehavior b in this.m_Selections) 
		{
			b.Select(b.Position - this.Position);
		}
	}
	
	private void Delete()
	{
		GameObject.Destroy(this.gameObject);
		EditorFactory.Instance.DestroyBuilding(this.Position, this.m_ConfigData);
	}
	
	private void Upgrade()
	{
		int newLevel = this.Level + this.m_ConfigData.UpgradeStep;
		if(newLevel <= this.m_ConfigData.MaximumLevel)
		{
			GameObject.Destroy(this.gameObject);
			EditorFactory.Instance.DestroyBuilding(this.Position, this.m_ConfigData);
			EditorFactory.Instance.ConstructBuilding(this.BuildingType, this.Level + this.m_ConfigData.UpgradeStep, this.Position);
		}
	}
	
	private void Degrade()
	{
		int newLevel = this.Level - this.m_ConfigData.UpgradeStep;
		if(newLevel >= this.m_ConfigData.InitialLevel)
		{
			GameObject.Destroy(this.gameObject);
			EditorFactory.Instance.DestroyBuilding(this.Position, this.m_ConfigData);
			EditorFactory.Instance.ConstructBuilding(this.BuildingType, this.Level - this.m_ConfigData.UpgradeStep, this.Position);
		}
	}
	
	private void Move()
	{
		EditorFactory.Instance.DestroyBuilding(this.Position, this.m_ConfigData);
		foreach (TilePosition offset in m_BuildingObstacle) 
		{
			GameObject cell = GameObject.Instantiate(EditorConfigInterface.Instance.CellPrefab) as GameObject;
			cell.transform.parent = this.transform;
			cell.transform.localPosition = new Vector3(offset.Column * ClientSystemConstants.BUILDING_TILE_MAP_TILE_SIZE.width + ClientSystemConstants.BUILDING_TILE_MAP_TILE_SIZE.width / 2,
				offset.Row * ClientSystemConstants.BUILDING_TILE_MAP_TILE_SIZE.height + ClientSystemConstants.BUILDING_TILE_MAP_TILE_SIZE.height / 2, 0);
		}
	}
	
	private void DropDown()
	{
		GameObject.Destroy(this.gameObject);
		if(EditorFactory.Instance.IsBuildable(this.Position, this.m_ConfigData.BuildingObstacleList))
		{
			EditorFactory.Instance.ConstructBuilding(this.BuildingType, this.Level, this.Position);
		}
		else
		{
			EditorFactory.Instance.ConstructBuilding(this.BuildingType, this.Level, this.m_OriginalPosition);
		}
		foreach (EditorBuildingBehavior b in m_Selections) 
		{
			if(b != this)
			{
				b.DropDown();
			}
		}
	}
}
