using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public abstract class ActorDirector : MonoBehaviour 
{
	protected List<GameObject> m_Villagers;
	protected Dictionary<int, GameObject> m_Builders;
	protected Dictionary<ArmyType, List<GameObject>> m_Armies;
	protected Dictionary<MercenaryType, List<GameObject>> m_Mercenaries;
	
	protected IMapData m_MapData;
	protected ISceneHelper m_SceneHelper;
	
	public void GenerateActors () 
	{
		this.m_Villagers = new List<GameObject>();
		this.m_Builders = new Dictionary<int, GameObject>();
		this.m_Armies = new Dictionary<ArmyType, List<GameObject>>();
		this.m_Mercenaries = new Dictionary<MercenaryType, List<GameObject>>();
		
		ActorConfig config = ActorPrefabConfig.Instance.GetComponent<ActorConfig>();
		int cityHallLevel = this.CityHallLevel;
		int villagerCount = config.CityHallLimit[cityHallLevel - 1];
		
		this.Initialize();
		
		for(int i = 0; i < villagerCount; i ++)
		{
			this.m_Villagers.Add(this.GenerateVillager(cityHallLevel));
		}
	}
	
	protected virtual void Initialize()
	{
	}
	
	protected abstract int CityHallLevel { get; }
	
	#region Villager
	private GameObject GenerateVillager(int cityHallLevel)
	{
		string prefabPath = ActorPrefabConfig.Instance.GetVillagerActorPrefab(cityHallLevel);
		GameObject villagerPrefab = Resources.Load(prefabPath) as GameObject;
		GameObject villager = GameObject.Instantiate(villagerPrefab) as GameObject;
		VillagerAI villagerAI = villager.GetComponent<VillagerAI>();
		villagerAI.MapData = this.m_MapData;
		villagerAI.SceneHelper = this.m_SceneHelper;
		villagerAI.Spawn();
		return villager;
	}
	#endregion
	#region Builder
	public void SendBuilderToBuild(int builderNO, int builderLevel, TilePosition builderHutPosition,
		IObstacleInfo targetInfo, IMapData mapData)
	{
		
		GameObject builder = null;
		if(this.m_Builders.ContainsKey(builderNO))
		{
			builder = this.m_Builders[builderNO]; 
			builder.SetActive(true);
		}
		else
		{
			builder = this.GenerateBuilder(builderNO, builderLevel);
			this.m_Builders.Add(builderNO, builder);
			builder.transform.position = PositionConvertor.GetWorldPositionFromBuildingTileIndex(builderHutPosition);
		}
	
		BuilderAI builderAI = builder.GetComponent<BuilderAI>();
		builderAI.MapData = mapData;
		builderAI.BuilderNO = builderNO;
		builderAI.Build(targetInfo);
	}
	
	public void SendBuilderBuild(int builderNO, int builderLevel, IObstacleInfo targetInfo, IMapData mapData)
	{
		GameObject builder = null;
		if(this.m_Builders.ContainsKey(builderNO))
		{
			builder = this.m_Builders[builderNO]; 
			builder.SetActive(true);
		}
		else
		{
			builder = this.GenerateBuilder(builderNO, builderLevel);
			this.m_Builders.Add(builderNO, builder);
		}
		
		
		TilePosition buildPoint = BorderPointHelper.FindValidInflateOneBorderPoint(targetInfo);
		builder.transform.position = PositionConvertor.GetWorldPositionFromActorTileIndex(buildPoint);
		
		BuilderAI builderAI = builder.GetComponent<BuilderAI>();
		builderAI.MapData = mapData;
		builderAI.BuilderNO = builderNO;
		builderAI.Build(targetInfo, buildPoint);
	}
	
	private GameObject GenerateBuilder(int builderNO, int builderLevel)
	{
		string prefabPath = ActorPrefabConfig.Instance.GetBuilderActorPrefab(builderLevel);
		GameObject builderPrefab = Resources.Load(prefabPath) as GameObject;
		GameObject builder = GameObject.Instantiate(builderPrefab) as GameObject;
		return builder;
	}
	
	
	public void SendBuilderReturn(int builderNO)
	{
		BuilderAI builder = this.m_Builders[builderNO].GetComponent<BuilderAI>();
		builder.FinishBuild();
	}
	
	public void RecycleBuilder(int builderNO)
	{
		this.m_Builders[builderNO].SetActive(false);
	}
	#endregion
	
	#region Army
	public void SendArmyToCamp(ArmyType armyType, int level, IBuildingInfo campInfo, IBuildingInfo factoryInfo)
	{
		GameObject army = this.GenerateArmy(armyType, level);
		TilePosition initialPoint = BorderPointHelper.FindValidInflateOneBorderPoint(factoryInfo);
		army.transform.position = PositionConvertor.GetWorldPositionFromActorTileIndex(initialPoint);
		
		ArmyAI ai = army.GetComponent<ArmyAI>();
		ai.SendArmyToCamp(campInfo);
	}
	
	public void GenerateArmyInCamp(ArmyType armyType, int level, IBuildingInfo campInfo)
	{
		GameObject army = this.GenerateArmy(armyType, level);
		ArmyAI ai = army.GetComponent<ArmyAI>();
		ai.GenerateArmyInCamp(campInfo);
	}
	
	private GameObject GenerateArmy(ArmyType armyType, int level)
	{
		string prefabPath = ActorPrefabConfig.Instance.GetArmyActorPrefab(armyType, level);
		GameObject armyPrefab = Resources.Load(prefabPath) as GameObject;
		GameObject army = GameObject.Instantiate(armyPrefab) as GameObject;
		
		if(!this.m_Armies.ContainsKey(armyType))
		{
			this.m_Armies.Add(armyType, new List<GameObject>());
		}
		this.m_Armies[armyType].Add(army);
		
		ActorConfig config = ActorPrefabConfig.Instance.GetComponent<ActorConfig>();

		ArmyAI armyAI = army.GetComponent<ArmyAI>();
		armyAI.WalkVelocity = config.ArmyMoveVelocity[armyType];
		armyAI.MapData = this.m_MapData;
		return army;
	}
	#endregion
	
	#region Mercenary
	public void SendMercenaryToCamp(MercenaryType mercenaryType, IBuildingInfo campInfo, IBuildingInfo factoryInfo)
	{
		GameObject mercenary = this.GenerateMercenary(mercenaryType);
		TilePosition initialPoint = BorderPointHelper.FindValidInflateOneBorderPoint(factoryInfo);
		mercenary.transform.position = PositionConvertor.GetWorldPositionFromActorTileIndex(initialPoint);
		
		ArmyAI ai = mercenary.GetComponent<ArmyAI>();
		ai.SendArmyToCamp(campInfo);
	}
	
	public void GenerateMercenaryInCamp(MercenaryType mercenaryType, IBuildingInfo campInfo)
	{
		GameObject mercenary = this.GenerateMercenary(mercenaryType);
		ArmyAI ai = mercenary.GetComponent<ArmyAI>();
		ai.GenerateArmyInCamp(campInfo);
	}
	
	private GameObject GenerateMercenary(MercenaryType mercenaryType)
	{
		string prefabPath = ActorPrefabConfig.Instance.GetMercenaryActorPrefab(mercenaryType);
		GameObject mercenaryPrefab = Resources.Load(prefabPath) as GameObject;
		GameObject mercenary = GameObject.Instantiate(mercenaryPrefab) as GameObject;
		
		if(!this.m_Mercenaries.ContainsKey(mercenaryType))
		{
			this.m_Mercenaries.Add(mercenaryType, new List<GameObject>());
		}
		this.m_Mercenaries[mercenaryType].Add(mercenary);
		
		ActorConfig config = ActorPrefabConfig.Instance.GetComponent<ActorConfig>();
		
		ArmyAI armyAI = mercenary.GetComponent<ArmyAI>();
		armyAI.WalkVelocity = config.MercenaryMoveVelocity[mercenaryType];
		armyAI.MapData = this.m_MapData;
		return mercenary;
	}
	#endregion
}
