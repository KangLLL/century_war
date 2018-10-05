using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class VillagerAI : NewAI 
{
	public IMapData MapData { get;set; }
	public ISceneHelper SceneHelper { get;set; }
	
	public void Spawn()
	{
		TilePosition spawnPosition = new TilePosition();
		spawnPosition.RandomActorPosition();
		
		while(!this.MapData.ActorCanPass(spawnPosition.Row, spawnPosition.Column))
		{
			spawnPosition.RandomActorPosition();
		}
		this.FindTargetObject(null, spawnPosition);
	}
	
	public void FindTargetObject(IBuildingInfo buildingInfo, TilePosition startPosition)
	{
		List<IBuildingInfo> buildings = this.SceneHelper.GetAllBuildings();
		
		int index = Random.Range(0, buildings.Count);
		IBuildingInfo targetBuildingInfo = buildings[index];
		if(buildingInfo != null)
		{
			while(buildingInfo == targetBuildingInfo)
			{
				index = Random.Range(0, buildings.Count);
				targetBuildingInfo = buildings[index];
			}
		}
		
		TilePosition targetPoint = BorderPointHelper.FindValidInflateOneBorderPoint(targetBuildingInfo);
		if(startPosition != null)
		{
			this.transform.position = PositionConvertor.GetWorldPositionFromActorTileIndex(startPosition);
		}
		VillagerWalkState walkState = new VillagerWalkState(this.MapData, targetPoint, this, targetBuildingInfo);
		this.ChangeState(walkState);
	}
	
	public void RunAway(HashSet<BuildingType> disappearBuildings)
	{
		if(this.m_CurrentState is VillagerDisappearState)
		{
			GameObject.Destroy(this.gameObject);
		}
		else
		{
			List<IBuildingInfo> buildings = this.SceneHelper.GetBuildingsOfTypes(disappearBuildings);
			int index = Random.Range(0, buildings.Count);
			
			IBuildingInfo disappearBuilding = buildings[index];
			TilePosition targetPosition = BorderPointHelper.FindValidInflateOneBorderPoint(disappearBuilding);
			
			ActorRunAwayState runAwayState = new ActorRunAwayState(this.MapData, targetPosition, this);
			this.ChangeState(runAwayState);
		}
	}
}
