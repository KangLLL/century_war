using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class ArmyAI : NewAI 
{
	public float WalkVelocity { get;set; }
	public IMapData MapData { get;set; }
	
	public void SendArmyToCamp(IBuildingInfo campInfo)
	{
		TilePosition targetPoint = this.FindCampStandablePoint(campInfo);
		ArmyWalkState walkState = new ArmyWalkState(this.MapData, targetPoint, this, campInfo);
		this.ChangeState(walkState);
	}
	
	public void GenerateArmyInCamp(IBuildingInfo campInfo)
	{
		this.transform.position = PositionConvertor.GetWorldPositionFromActorTileIndex(
			this.FindCampStandablePoint(campInfo)) + new Vector3(Random.Range(- ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.width / 2, ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.height / 2), 
			Random.Range(- ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.width / 2, ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.height / 2), 0);
		ArmyIdleState idleState = new ArmyIdleState(this, campInfo);
		this.ChangeState(idleState);
	}
	
	public TilePosition FindCampStandablePoint(IBuildingInfo campInfo)
	{
		int index = Random.Range(0, campInfo.BuildingObstacleList.Count);
		TilePosition buildingObstaclePosition = campInfo.BuildingObstacleList[index];
		
		TilePosition buildingPosition = campInfo.BuildingPosition + buildingObstaclePosition;
		
		TilePosition actorObstaclePosition = PositionConvertor.GetActorTilePositionFromBuildingTilePosition(buildingPosition);
		TilePosition actorOffset = actorObstaclePosition - campInfo.ActorPosition;
		
		while(!actorObstaclePosition.IsValidActorTilePosition() || 
			campInfo.ActorObstacleList.Contains(actorOffset))
		{
			index = Random.Range(0, campInfo.BuildingObstacleList.Count);
			buildingObstaclePosition = campInfo.BuildingObstacleList[index];
			
			buildingPosition = campInfo.BuildingPosition + buildingObstaclePosition;
			
			actorObstaclePosition = PositionConvertor.GetActorTilePositionFromBuildingTilePosition(buildingPosition);
			actorOffset = actorObstaclePosition - campInfo.ActorPosition;
		}
		return actorObstaclePosition;
	}
}
