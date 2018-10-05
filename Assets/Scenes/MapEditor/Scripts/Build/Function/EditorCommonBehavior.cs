using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class EditorCommonBehavior<T,P> : IOperateFunction
{
	public T ObjectType { get; set; }
	public TilePosition Position { get; set; }
	public MonoBehaviour OperatorBehavior { get;set; }
	public List<TilePosition> BuildingObstacle { get { return this.m_BuildingObstacle; } }
	public TilePosition OriginalPosition
	{
		get
		{
			return this.m_OriginalPosition;
		}
		set
		{
			this.m_OriginalPosition = value;
		}
	}
	public P ConfigData
	{
		get { return this.m_ConfigData; } 
	}
	
	private P m_ConfigData;
	private List<TilePosition> m_BuildingObstacle;
	
	private TilePosition m_OriginalPosition;
	
	private const string BUILDING_BACKGROUND_OBJECT_NAME = "BuildingBackground";
	private const string BACKGROUND_OBJECT_NAME = "Background";
	private const string BACKGROUND_ANCHOR_OBJECT_NAME = "BackgroundAnchor";
	
	public void Initial()
	{
		this.m_ConfigData = this.GetConfigData();
		this.m_BuildingObstacle = this.GetBuildingObstacleInfo(this.ObjectType);
		
		Transform anchor = this.OperatorBehavior.transform.FindChild(ClientStringConstants.BUILDING_ANCHOR_OBJECT_NAME);
		if(anchor == null)
		{
			anchor = this.OperatorBehavior.transform.FindChild(BACKGROUND_ANCHOR_OBJECT_NAME);
		}
		Transform bg = anchor.FindChild(BUILDING_BACKGROUND_OBJECT_NAME);
		if(bg == null)
		{
			bg = anchor.FindChild(BACKGROUND_OBJECT_NAME);
			bg.gameObject.SetActive(true);
			GameObject.DestroyImmediate(bg.GetComponent<DefenseObjectFX>());
		}
		
		tk2dSpriteAnimator sp = bg.GetComponent<tk2dSpriteAnimator>();
	
		BoxCollider c = this.OperatorBehavior.gameObject.AddComponent<BoxCollider>();
		c.center = sp != null ? anchor.localPosition + sp.Sprite.GetBounds().center :
			Vector3.zero;
		c.size = sp != null ? sp.Sprite.GetBounds().extents :
			new Vector3(100,100,0);
	}
	
	public virtual void Delete()
	{	
	}
	protected virtual void Construct(T type, TilePosition position)
	{
	}
	protected abstract P GetConfigData();
	protected abstract List<TilePosition> GetBuildingObstacleInfo(T type);
	protected abstract T GetTypeFromIndex(int index);
	protected abstract int GetIndexFromType(T type);
	
	protected virtual bool IsValidType(T type) { return true; }
	protected abstract T StartType { get; } 
	
	public void ChangeType()
	{
		GameObject.Destroy(this.OperatorBehavior.gameObject);
		this.Delete();
		
		T originalType = this.ObjectType;
		T currentType = this.ObjectType;
		
		T nextType = this.StartType;
		foreach (var type in Enum.GetValues(typeof(T))) 
		{
			if(this.IsValidType((T)type))
			{
				if(this.GetIndexFromType((T)type) > this.GetIndexFromType(currentType))
				{
					nextType = (T)type;
					break;
				}
			}
		}
		
		currentType = nextType;
		List<TilePosition> buildingObstacle = this.GetBuildingObstacleInfo(nextType);
		while(!EditorFactory.Instance.IsBuildable(this.Position, buildingObstacle) && !nextType.Equals(originalType))
		{
			nextType = this.StartType;
			foreach (var type in Enum.GetValues(typeof(T))) 
			{
				if(this.IsValidType((T)type))
				{
					if(this.GetIndexFromType((T)type) > this.GetIndexFromType(currentType))
					{
						nextType = (T)type;
						break;
					}
				}
			}
		    buildingObstacle = this.GetBuildingObstacleInfo(nextType);
		}
		
		Debug.Log(nextType);
		
		if(!nextType.Equals(originalType))
		{
			this.Construct(nextType, this.Position);
		}
		else
		{
			this.Construct(originalType, this.Position);
		}
	}
	
	public void DropDown()
	{
		GameObject.Destroy(this.OperatorBehavior.gameObject);
		if(EditorFactory.Instance.IsBuildable(this.Position, this.GetBuildingObstacleInfo(this.ObjectType)))
		{
			this.Construct(this.ObjectType, this.Position);
		}
		else
		{
			this.Construct(this.ObjectType, this.m_OriginalPosition);
		}
	}
	
	public void Move()
	{
		this.OriginalPosition = this.Position;
		this.Delete();
		foreach (TilePosition offset in this.BuildingObstacle) 
		{
			GameObject cell = GameObject.Instantiate(EditorConfigInterface.Instance.CellPrefab) as GameObject;
			cell.transform.parent = this.OperatorBehavior.transform;
			cell.transform.localPosition = new Vector3(offset.Column * ClientSystemConstants.BUILDING_TILE_MAP_TILE_SIZE.width + ClientSystemConstants.BUILDING_TILE_MAP_TILE_SIZE.width / 2,
				offset.Row * ClientSystemConstants.BUILDING_TILE_MAP_TILE_SIZE.height + ClientSystemConstants.BUILDING_TILE_MAP_TILE_SIZE.height / 2, 0);
		}
	}
	
	public void Drag()
	{
		Vector3 worldPosition = EditorConfigInterface.Instance.SceneCamera.ScreenToWorldPoint(Input.mousePosition);
		TilePosition currentPosition = PositionConvertor.GetBuildingTileIndexFromWorldPosition(worldPosition);
		this.Position = currentPosition;
		this.OperatorBehavior.transform.position = PositionConvertor.GetWorldPositionByBuildingTileIndex(this.Position);
	}
}
