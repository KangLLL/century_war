using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstaclePropertyBehavior : MonoBehaviour, IObstacleInfo
{
	private TilePosition m_BuildingPosition;
	private List<TilePosition> m_BuildingObstacleList;
	private List<TilePosition> m_ActorObstacleList;
	
	public TilePosition BuildingPosition 
	{ 
		get
		{
			return this.m_BuildingPosition;
		}
		set
		{
			this.m_BuildingPosition = value;
		}
	}
	
	public TilePosition ActorPosition
	{
		get
		{
			return PositionConvertor.GetActorTilePositionFromBuildingTilePosition(this.m_BuildingPosition); 
		}
	}
	
	public List<TilePosition> BuildingObstacleList 
	{ 
		get
		{
			return this.m_BuildingObstacleList;
		}
		set
		{
			this.m_BuildingObstacleList = value;
		}
	}
	public List<TilePosition> ActorObstacleList
	{ 
		get
		{
			return this.m_ActorObstacleList;
		}
		set
		{
			this.m_ActorObstacleList = value;
		}
	}
}
