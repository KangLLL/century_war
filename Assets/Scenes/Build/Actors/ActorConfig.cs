using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class ActorConfig : MonoBehaviour 
{
	[SerializeField]
	private int m_BuildAnimationFrame;
	[SerializeField]
	private float m_BuilderMoveVelocity;
	[SerializeField]
	private float m_VillagerMoveVelocity;
	[SerializeField]
	private float m_BerserkerMoveVelocity;
	[SerializeField]
	private float m_RangerMoveVelocity;
	[SerializeField]
	private float m_MarauderMoveVelocity;
	[SerializeField]
	private float m_MTMoveVelocity;
	[SerializeField]
	private float m_BombermanMoveVelocity;
	[SerializeField]
	private float m_MercenarySlingerMoveVelocity;
	[SerializeField]
	private float m_MercenaryHerculesMoveVelocity;
	[SerializeField]
	private float m_MercenaryKodoMoveVelocity;
	[SerializeField]
	private float m_MercenaryHerculesIIMoveVelocity;
	[SerializeField]
	private float m_MercenaryKodoIIMoveVelocity;
	[SerializeField]
	private float m_MercenaryArsonistMoveVelocity;
	[SerializeField]
	private float m_MercenaryArsonistIIMoveVelocity;
	[SerializeField]
	private float m_MercenaryPhalanxSoldierMoveVelocity;
	[SerializeField]
	private float m_MercenaryCatapultsMoveVelocity;
	[SerializeField]
	private float m_MercenaryCrazyKodoMoveVelocity;
	[SerializeField]
	private float m_MercenaryPhalanxSoldierIIMoveVelocity;
	[SerializeField]
	private float m_MercenaryCatapultsIIMoveVelocity;
	[SerializeField]
	private float m_MercenaryDemolisherMoveVelocity;
	
	private Dictionary<ArmyType, float> m_ArmyMoveVelocity;
	private Dictionary<MercenaryType, float> m_MercenaryMoveVelocity;
	
	[SerializeField]
	private int m_VillagerIdleFrame;
	[SerializeField]
	private int m_VillagerDisappearMinFrame;
	[SerializeField]
	private int m_VillagerDisappearMaxFrame;
	[SerializeField]
	private BuildingType[] m_VillagerAppearBuildingTypes;
	[SerializeField]
	private BuildingType[] m_VillagerDisappearBuildingTypes;
	[SerializeField]
	private int[] m_CityHallLimit;
	[SerializeField]
	private int m_ArmyIdleMinFrame;
	[SerializeField]
	private int m_ArmyIdleMaxFrame;
	
	public int BuildAnimationFrame { get { return this.m_BuildAnimationFrame; } }
	public float BuilderMoveVelocity { get { return this.m_BuilderMoveVelocity; } }
	public float VillagerMoveVelocity { get { return this.m_VillagerMoveVelocity; } }
	public Dictionary<ArmyType, float> ArmyMoveVelocity { get { return this.m_ArmyMoveVelocity; } }
	public Dictionary<MercenaryType, float> MercenaryMoveVelocity { get { return this.m_MercenaryMoveVelocity; } }
	public int VillagerIdleFrame { get { return this.m_VillagerIdleFrame; } }
	public int VillagerDisappearMinFrame { get { return this.m_VillagerDisappearMinFrame; } }
	public int VillagerDisappearMaxFrame { get { return this.m_VillagerDisappearMaxFrame; } }
	public BuildingType[] VillagerAppearBuildingTypes { get { return this.m_VillagerAppearBuildingTypes; } }
	public BuildingType[] VillagerDisappearBuildingTypes { get { return this.m_VillagerDisappearBuildingTypes; } }
	public int[] CityHallLimit { get { return this.m_CityHallLimit; } }
	public int ArmyIdleMinFrame { get { return this.m_ArmyIdleMinFrame; } }
	public int ArmyIdleMaxFrame { get { return this.m_ArmyIdleMaxFrame; } }
	
	void Start()
	{
		this.m_ArmyMoveVelocity = new Dictionary<ArmyType, float>(){
			{ArmyType.Berserker, this.m_BerserkerMoveVelocity},
			{ArmyType.Ranger, this.m_RangerMoveVelocity}, 
			{ArmyType.Marauder, this.m_MarauderMoveVelocity},
			{ArmyType.MT, this.m_MTMoveVelocity},
			{ArmyType.Bomberman, this.m_BombermanMoveVelocity}
		};
		this.m_MercenaryMoveVelocity =new Dictionary<MercenaryType, float>(){
			{MercenaryType.Slinger, this.m_MercenarySlingerMoveVelocity},
			{MercenaryType.Hercules, this.m_MercenaryHerculesMoveVelocity},
			{MercenaryType.Kodo, this.m_MercenaryKodoMoveVelocity},
			{MercenaryType.HerculesII, this.m_MercenaryHerculesIIMoveVelocity},
			{MercenaryType.KodoII, this.m_MercenaryKodoIIMoveVelocity},
			{MercenaryType.Arsonist, this.m_MercenaryArsonistMoveVelocity},
			{MercenaryType.ArsonistII, this.m_MercenaryArsonistIIMoveVelocity},
			{MercenaryType.PhalanxSoldier, this.m_MercenaryPhalanxSoldierMoveVelocity},
			{MercenaryType.Catapults, this.m_MercenaryCatapultsMoveVelocity},
			{MercenaryType.CrazyKodo, this.m_MercenaryCrazyKodoMoveVelocity},
			{MercenaryType.PhalanxSoldierII, this.m_MercenaryPhalanxSoldierIIMoveVelocity},
			{MercenaryType.CatapultsII, this.m_MercenaryCatapultsIIMoveVelocity},
			{MercenaryType.Demolisher, this.m_MercenaryDemolisherMoveVelocity}
		};
	}
}
