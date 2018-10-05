using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class BattleSceneDirector : ActorDirector 
{
	[SerializeField]
	private BattleSceneHelper m_BattleSceneHelper;
	
	protected override int CityHallLevel 
	{
		get 
		{
			if(this.m_BattleSceneHelper.GetBuildings(BuildingType.CityHall).Count == 0)
			{
				return 1;
			}
			return this.m_BattleSceneHelper.GetBuildings(BuildingType.CityHall)[0].Level;
		}
	}
	
	protected override void Initialize ()
	{
		this.m_MapData = BattleMapData.Instance;
		this.m_SceneHelper = this.m_BattleSceneHelper;
		
		int builderNO = 0;
		foreach (BattleObstacleUpgradingInfo upgradingInfo in this.m_BattleSceneHelper.UpgradingBuildings) 
		{
			this.SendBuilderBuild(builderNO ++, upgradingInfo.AttachedBuilderLevel, upgradingInfo.ObstacleProperty, this.m_MapData);
		}
		base.Initialize ();
	}
	
	public void ClearAllActors()
	{
		if(this.m_Villagers != null)
		{
			foreach(GameObject villager in this.m_Villagers)
			{
				GameObject.DestroyImmediate(villager);
			}
			this.m_Villagers.Clear();
		}
		if(this.m_Builders !=  null)
		{
			foreach (int builderNO in this.m_Builders.Keys) 
			{
				GameObject.DestroyImmediate(this.m_Builders[builderNO]);
			}
			this.m_Builders.Clear();
		}
	}
	
	public void SendRunAway()
	{
		List<IBuildingInfo> builderHuts = this.m_BattleSceneHelper.GetBuildings(BuildingType.BuilderHut);
		BuildingType[] disappearBuildings = ActorPrefabConfig.Instance.GetComponent<ActorConfig>().VillagerDisappearBuildingTypes;
		HashSet<BuildingType> buildings = new HashSet<BuildingType>();
		foreach (BuildingType building in disappearBuildings) 
		{
			buildings.Add(building);
		}
		
		foreach (KeyValuePair<int, GameObject> builder in this.m_Builders) 
		{
			BuilderAI builderAI = builder.Value.GetComponent<BuilderAI>();
			builderAI.RunAway(builderHuts);
		}
		foreach (GameObject villager in this.m_Villagers) 
		{
			VillagerAI villagerAI = villager.GetComponent<VillagerAI>();
			villagerAI.RunAway(buildings);
		}
	}
}
