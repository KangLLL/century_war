using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class VillagerDisappearState : TimeTickRelatedState 
{
	private ActorConfig m_ActorConfig;
	private HashSet<BuildingType> m_AppearBuildings;
	
	public VillagerDisappearState(NewAI aiBehavior) : base(aiBehavior)
	{
		this.m_ActorConfig = ActorPrefabConfig.Instance.GetComponent<ActorConfig>();
		
		this.m_AppearBuildings = new HashSet<BuildingType>();
		foreach(BuildingType building in this.m_ActorConfig.VillagerAppearBuildingTypes)
		{
			this.m_AppearBuildings.Add(building);
		}
	}
	
	public override void Initial ()
	{
		this.m_CurrentFrame = Random.Range(this.m_ActorConfig.VillagerDisappearMinFrame, this.m_ActorConfig.VillagerDisappearMaxFrame + 1);
		this.m_AnimationController.SetHide();
	}
	
	protected override void OnTimeUp ()
	{
		this.Appear();
		((VillagerAI)this.m_AIBehavior).FindTargetObject(null, null);
	}
	
	private void Appear()
	{
		List<IBuildingInfo> buildings = ((VillagerAI)this.m_AIBehavior).SceneHelper.GetBuildingsOfTypes(this.m_AppearBuildings);
		int index = Random.Range(0, buildings.Count);
		IBuildingInfo appearBuilding = buildings[index];
		while(appearBuilding.IsBuildingHover(((VillagerAI)this.m_AIBehavior).MapData))
		{
			index = Random.Range(0, buildings.Count);
			appearBuilding = buildings[index];
		}
		
		
		List<TilePosition> borders = BorderPointHelper.GetBorder(appearBuilding);
		index = Random.Range(0, borders.Count);
		TilePosition appearPoint = appearBuilding.ActorPosition + borders[index];
		while(!appearPoint.IsValidActorTilePosition())
		{
			index = Random.Range(0, borders.Count);
		 	appearPoint = appearBuilding.BuildingPosition + borders[index];
		}
		
		this.m_AIBehavior.transform.position = PositionConvertor.GetWorldPositionFromActorTileIndex(appearPoint);
		this.m_AnimationController.SetVisible(); 
	}
}
