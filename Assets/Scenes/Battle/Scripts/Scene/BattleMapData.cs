using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using System;

public class BattleMapData : MonoBehaviour, IMapData
{
	private static BattleMapData s_Sigleton;
	
	private GameObject[,] m_ActorObstacleArray;
	private GameObject[,] m_BuildingObstacleArray;
	
	private List<GameObject>[,] m_ActorArray;
	
	private GridType[,] m_GridArray;
	private bool[,] m_CharacterForbiddenArray;
	
	public static BattleMapData Instance
	{
		get
		{
			return s_Sigleton;
		}
	}
	
	public GameObject[,] BuildingArray
	{
		get
		{
			return this.m_BuildingObstacleArray;
		}
	}
	
	public List<GameObject>[,] ActorArray
	{
		get
		{
			return this.m_ActorArray;
		}
	}
	
	public GridType[,] GridArray
	{
		get
		{
			return this.m_GridArray;
		}
	}
	
	public bool[,] CharacterForbiddenArray
	{
		get
		{
			return this.m_CharacterForbiddenArray;
		}
	}
	
	void Awake()
	{
		s_Sigleton = this;
	}
	
	void OnDestroy()
	{
		s_Sigleton = null;
	}
	
	public void InitialMapData()
	{
		this.m_ActorObstacleArray = new GameObject[ClientSystemConstants.ACTOR_TILE_MAP_SIZE.height, 
			ClientSystemConstants.ACTOR_TILE_MAP_SIZE.width];
		this.m_BuildingObstacleArray = new GameObject[ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height, 
			ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width];
		
		this.m_ActorArray = new List<GameObject>[ClientSystemConstants.ACTOR_TILE_MAP_SIZE.height, 
			ClientSystemConstants.ACTOR_TILE_MAP_SIZE.width];
		this.m_GridArray = new GridType[ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height,
			ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width];
		
		this.m_CharacterForbiddenArray = new bool[ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height, 
			ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width];
		
		for(int i = 0; i < ClientSystemConstants.ACTOR_TILE_MAP_SIZE.height; i ++)
		{
			for(int j = 0; j < ClientSystemConstants.ACTOR_TILE_MAP_SIZE.width; j ++)
			{
				this.m_ActorArray[i,j] = new List<GameObject>();
			}
		}
	}
	
	public void RefreshInformationWithConstructObstacle(GameObject obstacle)
	{
		this.RefreshInformationWithObstacle(obstacle, true);
	}
	
	public void RefreshInformationWithDestroyObstacle(GameObject obstacle)
	{
		this.RefreshInformationWithObstacle(obstacle, false);
		/*
		foreach(GameObject invader in this.m_InvaderList)
		{
			invader.SendMessage("SetIdle", false);
		}
		*/
	}
	
	public void RefreshInformationWithConstructActor(GameObject actor, TilePosition position)
	{
		this.RefreshInformationWithActor(actor, null, position);
	}
	
	public void RefreshInformationWithDestroyActor(GameObject actor, TilePosition position)
	{
		this.RefreshInformationWithActor(actor, position, null);
	}
	
	public void RefreshInformationWithMoveActor(GameObject actor, TilePosition oldPosition, TilePosition newPosition)
	{
		this.RefreshInformationWithActor(actor, oldPosition, newPosition);
	}
	
	private void RefreshInformationWithActor(GameObject actor, TilePosition oldPosition, TilePosition newPosition)
	{
		if(oldPosition != null)
		{
			this.m_ActorArray[oldPosition.Row, oldPosition.Column].Remove(actor);
		}
		if(newPosition != null)
		{
			this.m_ActorArray[newPosition.Row, newPosition.Column].Add(actor);
		}
	}
	
	private void RefreshInformationWithObstacle(GameObject obstacle, bool isAdded)
	{
		ObstaclePropertyBehavior property = obstacle.GetComponent<ObstaclePropertyBehavior>();
		if(property != null)
		{
			TilePosition buildingPosition = property.BuildingPosition;
			TilePosition actorPosition = property.ActorPosition;
			
			foreach(TilePosition position in property.BuildingObstacleList)
			{
				TilePosition p = buildingPosition + position;
				this.m_BuildingObstacleArray[p.Row, p.Column] = isAdded ? obstacle : null;
			}
			foreach(TilePosition position in property.ActorObstacleList)
			{
				TilePosition p = actorPosition + position;
				this.m_ActorObstacleArray[p.Row, p.Column] = isAdded ? obstacle : null;
			}
			
			if(!isAdded)
			{
			    BuildingPropertyBehavior buildingProperty = property as BuildingPropertyBehavior;
				if(buildingProperty != null && buildingProperty.BuildingType == BuildingType.Wall)
				{
					this.DestroyInflateObstacleOfWall(buildingProperty);
				}
			}
		}
	}
	
	private void InflateBuildingArray()
	{
		for(int i = 0; i < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height; i ++)
		{
			for(int j = 0; j < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width; j ++)
			{
				this.m_CharacterForbiddenArray[i,j] = this.IsHaveBuildingNearBy(i,j);
			}
		}
	}
	
	private bool IsHaveBuildingNearBy(int row, int column)
	{
		if(this.m_BuildingObstacleArray[row,column] != null)
		{
			return true;
		}
		
		int bottom = row - 1;
		int top = row + 1;
		int left = column - 1;
		int right = column + 1;
		
		if(bottom.IsValidBuildingRow())
		{
			if(this.m_BuildingObstacleArray[bottom, column] != null && 
				this.m_BuildingObstacleArray[bottom, column].GetComponent<BuildingBasePropertyBehavior>() != null)
			{
				return true;
			}
			if(left.IsValidBuildingColumn())
			{
				if(this.m_BuildingObstacleArray[bottom, left] != null &&
					this.m_BuildingObstacleArray[bottom, left].GetComponent<BuildingBasePropertyBehavior>() != null)
				{
					return true;
				}
			}
			if(right.IsValidBuildingColumn())
			{
				if(this.m_BuildingObstacleArray[bottom, right] != null &&
					this.m_BuildingObstacleArray[bottom, right].GetComponent<BuildingBasePropertyBehavior>() != null)
				{
					return true;
				}
			}
		}
		if(top.IsValidBuildingRow())
		{
			if(this.m_BuildingObstacleArray[top, column] != null &&
				this.m_BuildingObstacleArray[top, column].GetComponent<BuildingBasePropertyBehavior>() != null)
			{
				return true;
			}
			if(left.IsValidBuildingColumn())
			{
				if(this.m_BuildingObstacleArray[top, left] != null &&
					this.m_BuildingObstacleArray[top, left].GetComponent<BuildingBasePropertyBehavior>() != null)
				{
					return true;
				}
			}
			if(right.IsValidBuildingColumn())
			{
				if(this.m_BuildingObstacleArray[top, right] != null &&
					this.m_BuildingObstacleArray[top, right].GetComponent<BuildingBasePropertyBehavior>() != null)
				{
					return true;
				}
			}
		}
		if(left.IsValidBuildingColumn())
		{
			if(this.m_BuildingObstacleArray[row, left] != null &&
				this.m_BuildingObstacleArray[row, left].GetComponent<BuildingBasePropertyBehavior>() != null)
			{
				return true;
			}
		}
		if(right.IsValidBuildingColumn())
		{
			if(this.m_BuildingObstacleArray[row, right] != null &&
				this.m_BuildingObstacleArray[row, right].GetComponent<BuildingBasePropertyBehavior>() != null)
			{
				return true;
			}
		}
		return false;
	}
	
	public void ConstructGridArray()
	{
		this.InflateBuildingArray();
		for(int i = 0; i < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height; i ++)
		{
			for(int j = 0; j < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width; j ++)
			{
				if(this.m_CharacterForbiddenArray[i,j])
				{
					int result = 0;
					int left = j - 1;
					int right = j + 1;
					int up = i + 1;
					int bottom = i - 1;
					if(!up.IsValidBuildingRow() || !this.m_CharacterForbiddenArray[up, j])
					{
						result |= (int)CommonDirection.Up;
					}
					if(!bottom.IsValidBuildingRow() || !this.m_CharacterForbiddenArray[bottom, j])
					{
						result |= (int)CommonDirection.Down;
					}
					if(!left.IsValidBuildingColumn() || !this.m_CharacterForbiddenArray[i,left])
					{
						result |= (int)CommonDirection.Left;
					}
					if(!right.IsValidBuildingColumn() || !this.m_CharacterForbiddenArray[i, right] )
					{
						result |= (int)CommonDirection.Right;
					}
					//result = result ^ 0x0000000F;
					this.m_GridArray[i,j] = result.ConvertToGridType();
				}
				else
				{
					this.m_GridArray[i,j] = GridType.Out;
				}
			}
		}
	}

	#region IMapData implementation
	public bool[,] ActorObstacleArray
	{
		get
		{
			int height = this.m_ActorObstacleArray.GetLength(0);
			int width = this.m_ActorObstacleArray.GetLength(1);
			
			bool[,] result = new bool[height,width];
			for(int i = 0; i < height; i ++)
			{
				for(int j = 0; j < width; j ++)
				{
					result[i,j] = this.m_ActorObstacleArray[i,j] != null;
				}
			}
			return result;
		}
	}
	
	public bool ActorCanPass (int row, int column)
	{
		return this.m_ActorObstacleArray[row, column] == null;
	}

	public IObstacleInfo GetObstacleInfoFormActorObstacleMap (int row, int column)
	{
		if(this.m_ActorObstacleArray[row, column] != null)
		{
			return this.m_ActorObstacleArray[row, column].GetComponent<ObstaclePropertyBehavior>(); 
		}
		return null;
	}
	
	public GameObject GetBulidingObjectFromActorObstacleMap(int row, int column)
	{
		return this.m_ActorObstacleArray[row, column];
	}

	public GameObject GetBuildingObjectFromBuildingObstacleMap(int row, int column)
	{
		return this.m_BuildingObstacleArray[row, column];
	}
	
	public void InflateUpActorObstacleOfWall(BuildingPropertyBehavior wall)
	{
		TilePosition p = wall.GetBuildingFirstActorPosition();
		int row = p.Row + 1;
		int column = p.Column;
		if(row.IsValidActorRow())
		{
			this.m_ActorObstacleArray[row,column] = wall.gameObject;
		}
	}
	
	public void InflateRightObstacleOfWall(BuildingPropertyBehavior wall)
	{
		TilePosition p = wall.GetBuildingFirstActorPosition();
		int row = p.Row;
		int column = p.Column + 1;
		if(column.IsValidActorColumn())
		{
			this.m_ActorObstacleArray[row,column] = wall.gameObject;
		}
	}
	
	public void DestroyInflateObstacleOfWall(BuildingPropertyBehavior wall)
	{
		TilePosition p = wall.ActorPosition + wall.ActorObstacleList[0];
		int upRow = p.Row + 1;
		int upColumn = p.Column;
		
		if(upRow.IsValidActorRow() && this.m_ActorObstacleArray[upRow, upColumn] == wall.gameObject)
		{
			this.m_ActorObstacleArray[upRow,upColumn] = null;
		}
		
		int rightRow = p.Row;
		int rightColumn = p.Column + 1;
		
		if(rightColumn.IsValidActorColumn() && this.m_ActorObstacleArray[rightRow, rightColumn] == wall.gameObject)
		{
			this.m_ActorObstacleArray[rightRow, rightColumn] = null;
		}
		
		int downRow = p.Row - 1;
		int downColumn = p.Column;
		
		if(downRow.IsValidActorRow())
		{
			GameObject downObject  = this.m_ActorObstacleArray[downRow, downColumn];
			if(downObject != null)
			{
				BuildingPropertyBehavior buildingProperty = downObject.GetComponent<BuildingPropertyBehavior>();
				if(buildingProperty != null && buildingProperty.BuildingType == BuildingType.Wall)
				{
					this.m_ActorObstacleArray[downRow, downColumn] = null;
					downObject.GetComponent<WallUtility>().HideUpObject();
				}
			}
		}
		
		int leftRow = p.Row;
		int leftColumn = p.Column - 1;
		
		if(leftColumn.IsValidActorColumn())
		{
			GameObject leftObject = this.m_ActorObstacleArray[leftRow, leftColumn];
			if(leftObject != null)
			{
				BuildingPropertyBehavior buildingProperty = leftObject.GetComponent<BuildingPropertyBehavior>();
				if(buildingProperty != null && buildingProperty.BuildingType == BuildingType.Wall)
				{
					this.m_ActorObstacleArray[leftRow, leftColumn] = null;
					leftObject.GetComponent<WallUtility>().HideRightObject();
				}
			}
		}
	}
	#endregion
	
	
	void OnDrawGizmos()
    {
		if(this.m_BuildingObstacleArray != null && this.m_ActorObstacleArray != null)
		{
	        int column = ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width;
	        int row = ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height;
	        for (int i = 0; i < column; i++)
	        {
	            for (int j = 0; j < row; j++)
	            {
	                if (this.m_BuildingObstacleArray[j,i] != null)
	                {
	                    TilePosition tp = new TilePosition(i, j);
	                    Gizmos.color = Color.blue;
	                    Gizmos.DrawSphere(PositionConvertor.GetWorldPositionByBuildingTileIndex(tp), 8);
	                }
	            }
	        }
	        int columnActor = ClientSystemConstants.ACTOR_TILE_MAP_SIZE.width;
	        int rowActor = ClientSystemConstants.ACTOR_TILE_MAP_SIZE.height;
	        for (int i = 0; i < columnActor; i++)
	        {
	            for (int j = 0; j < rowActor; j++)
	            {
	                if (this.m_ActorObstacleArray[j,i] != null)
	                {
	                    TilePosition tp = new TilePosition(i, j);
	                    Gizmos.color = Color.magenta;
	                    Gizmos.DrawSphere(PositionConvertor.GetWorldPositionByActorTileIndex(tp), 4);
	                }
	            }
	        }
		}
    }
}