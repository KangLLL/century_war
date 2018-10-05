using UnityEngine;
using System.Collections;
using System;
using ConfigUtilities.Enums;

public interface IMapData 
{
	bool[,] ActorObstacleArray { get; }
	bool ActorCanPass(int row, int column);
	IObstacleInfo GetObstacleInfoFormActorObstacleMap(int row, int column);
	
	GameObject GetBulidingObjectFromActorObstacleMap(int row, int column);
	GameObject GetBuildingObjectFromBuildingObstacleMap(int row, int column);
}
