using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IObstacleInfo  
{
	TilePosition BuildingPosition { get; }
	TilePosition ActorPosition { get; }
	List<TilePosition> BuildingObstacleList { get; }
	List<TilePosition> ActorObstacleList { get; }
}
