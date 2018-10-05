using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class ActorPrefabConfig : MonoBehaviour 
{
	private static ActorPrefabConfig s_Sigleton;
	[SerializeField]
	private string[] m_BuilderActorPrefabName;
	[SerializeField]
	private string[] m_VillagerActorPrefabName;
	[SerializeField]
	private string[] m_BerserkerActorPrefabName;
	[SerializeField]
	private string[] m_RangerActorPrefabName;
	[SerializeField]
	private string[] m_MarauderActorPrefabName;
	[SerializeField]
	private string[] m_MTActorPrefabName;
	[SerializeField]
	private string[] m_BombermanActorPrefabName;
	[SerializeField]
	private string m_MercenarySlingerPrefabName;
	[SerializeField]
	private string m_MercenaryHerculesPrefabName;
	[SerializeField]
	private string m_MercenaryKodoPrefabName;
	[SerializeField]
	private string m_MercenaryHerculesIIPrefabName;
	[SerializeField]
	private string m_MercenaryKodoIIPrefabName;
	[SerializeField]
	private string m_MercenaryArsonistPrefabName;
	[SerializeField]
	private string m_MercenaryArsonistIIPrefabName;
	[SerializeField]
	private string m_MercenaryPhalanxSoldierPrefabName;
	[SerializeField]
	private string m_MercenaryCatapultsPrefabName;
	[SerializeField]
	private string m_MercenaryCrazyKodoPrefabName;
	[SerializeField]
	private string m_MercenaryPhalanxSoldierIIPrefabName;
	[SerializeField]
	private string m_MercenaryCatapultsIIPrefabName;
	[SerializeField]
	private string m_MercenaryDemolisherPrefabName;
	
	private const string BUILDER_PREFAB_PREFIX = "Builders/";
	private const string VILLAGER_PREFAB_PREFIX = "Villagers/";
	private const string ARMY_PREFAB_PREFIX = "Armies/";
	private const string MERCENARY_PREFAB_PREFIX = "Mercenaries/";
	
	private Dictionary<ArmyType, string[]> m_ArmyPrefabName;
	private Dictionary<MercenaryType, string> m_MercenaryPrefabName;
	
	void Awake()
	{
		s_Sigleton = this;
		this.m_ArmyPrefabName = new Dictionary<ArmyType, string[]>()
		{
			{ArmyType.Berserker, this.m_BerserkerActorPrefabName},
			{ArmyType.Ranger, this.m_RangerActorPrefabName},
			{ArmyType.Marauder, this.m_MarauderActorPrefabName},
			{ArmyType.MT, this.m_MTActorPrefabName},
			{ArmyType.Bomberman, this.m_BombermanActorPrefabName}
		};
		this.m_MercenaryPrefabName = new Dictionary<MercenaryType, string>()
		{
			{MercenaryType.Slinger, this.m_MercenarySlingerPrefabName},
			{MercenaryType.Hercules, this.m_MercenaryHerculesPrefabName},
			{MercenaryType.Kodo, this.m_MercenaryKodoPrefabName},
			{MercenaryType.HerculesII, this.m_MercenaryHerculesIIPrefabName},
			{MercenaryType.KodoII, this.m_MercenaryKodoIIPrefabName},
			{MercenaryType.Arsonist, this.m_MercenaryArsonistPrefabName},
			{MercenaryType.ArsonistII, this.m_MercenaryArsonistIIPrefabName},
			{MercenaryType.PhalanxSoldier, this.m_MercenaryPhalanxSoldierPrefabName},
			{MercenaryType.Catapults, this.m_MercenaryCatapultsPrefabName},
			{MercenaryType.CrazyKodo, this.m_MercenaryCrazyKodoPrefabName},
			{MercenaryType.PhalanxSoldierII, this.m_MercenaryPhalanxSoldierIIPrefabName},
			{MercenaryType.CatapultsII, this.m_MercenaryCatapultsIIPrefabName},
			{MercenaryType.Demolisher, this.m_MercenaryDemolisherPrefabName}
		};
	}
	
	void Start () 
	{
		GameObject.DontDestroyOnLoad(this.gameObject);
	}
	
	public string GetVillagerActorPrefab(int cityHallLevel)
	{
		return ClientStringConstants.BUILDING_SCENE_RESOURCE_PREFAB_PREFIX_NAME + 
			ClientStringConstants.ACTOR_OBJECT_PREFAB_PREFIX_NAME +
			VILLAGER_PREFAB_PREFIX + this.m_VillagerActorPrefabName[cityHallLevel - 1];
	}
	
	public string GetBuilderActorPrefab(int builderLevel)
	{
		return ClientStringConstants.BUILDING_SCENE_RESOURCE_PREFAB_PREFIX_NAME + 
			ClientStringConstants.ACTOR_OBJECT_PREFAB_PREFIX_NAME +
			BUILDER_PREFAB_PREFIX + this.m_BuilderActorPrefabName[builderLevel - 1];
	}
	
	public string GetArmyActorPrefab(ArmyType armyType, int armyLevel)
	{
		return ClientStringConstants.BUILDING_SCENE_RESOURCE_PREFAB_PREFIX_NAME + 
			ClientStringConstants.ACTOR_OBJECT_PREFAB_PREFIX_NAME +
			ARMY_PREFAB_PREFIX + armyType.ToString() + "/" + this.m_ArmyPrefabName[armyType][armyLevel - 1];
	}
	
	public string GetMercenaryActorPrefab(MercenaryType mercenaryType)
	{
		return ClientStringConstants.BUILDING_SCENE_RESOURCE_PREFAB_PREFIX_NAME + 
			ClientStringConstants.ACTOR_OBJECT_PREFAB_PREFIX_NAME +
			MERCENARY_PREFAB_PREFIX + mercenaryType.ToString() + "/" + this.m_MercenaryPrefabName[mercenaryType]; 
	}
	
	public static ActorPrefabConfig Instance
	{
		get
		{
			return s_Sigleton;
		}
	}
}
