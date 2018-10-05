using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class BuildingSceneDirector : ActorDirector 
{
	private static BuildingSceneDirector s_Sigleton;
	
	public static BuildingSceneDirector Instance
	{
		get
		{
			return s_Sigleton;
		}
	}
	
	void Awake()
	{
		s_Sigleton = this;
	}
	
	void Start()
	{
		this.GenerateActors();
	}
	
	protected override int CityHallLevel 
	{
		get 
		{
			return LogicController.Instance.GetBuildingObject(new BuildingIdentity(BuildingType.CityHall, 0)).Level;
		}
	}
	
	protected override void Initialize ()
	{
		this.m_MapData = SceneManager.Instance;
		this.m_SceneHelper = this.GetComponent<BuildingSceneHelper>();
		
		List<BuildingLogicData> buildings = LogicController.Instance.AllBuildings;
		List<RemovableObjectLogicData> objects = LogicController.Instance.AllRemovableObjects;
		
		foreach(RemovableObjectLogicData removableObject in objects)
		{
			if(removableObject.CurrentAttachedBuilderNO >= 0)
			{
				int builderNO = removableObject.CurrentAttachedBuilderNO;
				int builderLevel = LogicController.Instance.GetBuildingObject
					(new BuildingIdentity(BuildingType.BuilderHut, builderNO)).Level;
				this.SendBuilderBuild(builderNO, builderLevel, removableObject, this.m_MapData);
			}
		}
		
		foreach (BuildingLogicData building in buildings) 
		{
			ArmyIdentity[] armies = building.Armies;
			if(armies !=  null)
			{
				foreach (ArmyIdentity army in armies) 
				{
					this.GenerateArmyInCamp(army.armyType, LogicController.Instance.GetArmyLevel(army.armyType), building);
				}
			}
			MercenaryIdentity[] mercenaries = building.Mercenaries;
			if(mercenaries != null)
			{
				foreach(MercenaryIdentity mercenary in mercenaries)
				{
					BuildingIdentity campID = LogicController.Instance.GetMercenaryData(mercenary).CampID;
					BuildingLogicData camp = LogicController.Instance.GetBuildingObject(campID);
					this.GenerateMercenaryInCamp(mercenary.mercenaryType, camp);
				}
			}
			int builderNO = building.CurrentAttachedBuilderNO;
			if(builderNO >= 0)
			{
				int builderLevel = LogicController.Instance.GetBuildingObject
					(new BuildingIdentity(BuildingType.BuilderHut, builderNO)).Level;
				this.SendBuilderBuild(building.CurrentAttachedBuilderNO, builderLevel, building, this.m_MapData);
			}
		}
	}
}
