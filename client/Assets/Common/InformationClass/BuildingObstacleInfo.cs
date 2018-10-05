using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class BuildingObstacleInfo
{
	private static Dictionary<BuildingType, List<TilePosition>> s_BuildingObstacleInfoDict;
	private static Dictionary<BuildingType, List<TilePosition>> s_ActorObstacleInfoDict;
	
	static BuildingObstacleInfo()
	{
		s_BuildingObstacleInfoDict = new Dictionary<BuildingType, List<TilePosition>>()
		{
			{BuildingType.CityHall, new List<TilePosition>(){new TilePosition(-2,-2), new TilePosition(-1,-2), new TilePosition(0,-2), new TilePosition(1,-2), new TilePosition(2,-2),
													new TilePosition(-2,-1), new TilePosition(-1,-1), new TilePosition(0,-1), new TilePosition(1,-1), new TilePosition(2, -1),
													new TilePosition(-2,0), new TilePosition(-1,0), new TilePosition(0,0), new TilePosition(1,0), new TilePosition(2,0),
													new TilePosition(-2,1), new TilePosition(-1,1), new TilePosition(0,1), new TilePosition(1,1), new TilePosition(2,1),
													new TilePosition(-2,2), new TilePosition(-1,2), new TilePosition(0,2), new TilePosition(1,2), new TilePosition(2,2)}},
			{BuildingType.Fortress, new List<TilePosition>(){new TilePosition(-1,-1), new TilePosition(0,-1), new TilePosition(1,-1),
													new TilePosition(-1,0),	new TilePosition(0,0), new TilePosition(1,0), 
													new TilePosition(-1,1), new TilePosition(0,1), new TilePosition(1,1)}}
		};
		
		s_ActorObstacleInfoDict = new Dictionary<BuildingType, List<TilePosition>>()
		{
			{BuildingType.CityHall, new List<TilePosition>(){new TilePosition(-1,-1), new TilePosition(0,-1), new TilePosition(1,-1),
													new TilePosition(-1,0), new TilePosition(0,0), new TilePosition(1,0),
													new TilePosition(-1,1), new TilePosition(0,1), new TilePosition(1,1)}},
			{BuildingType.Fortress, new List<TilePosition>(){new TilePosition(-1,-1), new TilePosition(0,-1), new TilePosition(1,-1),
													new TilePosition(-1,0),	new TilePosition(0,0), new TilePosition(1,0), 
													new TilePosition(-1,1), new TilePosition(0,1), new TilePosition(1,1)}}
		};
	}
	
	public static List<TilePosition> GetBuildingBuildingObstacleInfoList(BuildingType type)
	{
		return s_BuildingObstacleInfoDict[type];
	}
	
	public static List<TilePosition> GetBuildingActorObstacleInfoList(BuildingType type)
	{
		return s_ActorObstacleInfoDict[type];
	}
}
