using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class VisitSceneDirector : ActorDirector
{
	void Start () 
	{
		this.GenerateActors();
	}
	
	protected override int CityHallLevel 
	{
		get 
		{
			return LogicController.Instance.CurrentFriend.GetBuildingData(new BuildingIdentity(BuildingType.CityHall, 0)).Level;
		}
	}
	
	protected override void Initialize ()
	{
		this.m_SceneHelper = LogicController.Instance.CurrentFriend;
		this.m_MapData = SceneManager.Instance;
		List<BuildingLogicData> buildings = LogicController.Instance.CurrentFriend.AllBuildings;
		List<RemovableObjectLogicData> objects = LogicController.Instance.CurrentFriend.AllRemovableObjects;
		
		foreach(RemovableObjectLogicData removableObject in objects)
		{
			if(removableObject.CurrentAttachedBuilderNO >= 0)
			{
				int builderNO = removableObject.CurrentAttachedBuilderNO;
				int builderLevel = LogicController.Instance.CurrentFriend.GetBuildingData
					(new BuildingIdentity(BuildingType.BuilderHut, builderNO)).Level;
				this.SendBuilderBuild(builderNO, builderLevel, removableObject, this.m_MapData);
			}
		}
		
		foreach (BuildingLogicData building in buildings) 
		{
			ArmyIdentity[] armies = building.Armies;
			MercenaryIdentity[] mercenaries = building.Mercenaries;
			if(armies !=  null)
			{
				foreach (ArmyIdentity army in armies) 
				{
					this.GenerateArmyInCamp(army.armyType, LogicController.Instance.CurrentFriend.GetArmy(army).ArmyLevel, building);
				}
			}
			if(mercenaries != null)
			{
				foreach(MercenaryIdentity mercenary in mercenaries)
				{
					this.GenerateMercenaryInCamp(mercenary.mercenaryType, building);
				}
			}
			int builderNO = building.CurrentAttachedBuilderNO;
			if(builderNO >= 0)
			{
				int builderLevel = LogicController.Instance.CurrentFriend.GetBuildingData
					(new BuildingIdentity(BuildingType.BuilderHut, builderNO)).Level;
				this.SendBuilderBuild(building.CurrentAttachedBuilderNO, builderLevel, building, this.m_MapData);
			}
		}
	}
}