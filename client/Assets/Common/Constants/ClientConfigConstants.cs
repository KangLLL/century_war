using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
using System.Collections.Generic;

public class ClientConfigConstants : MonoBehaviour 
{
	[SerializeField]
	private int m_TicksPerSecond;
	[SerializeField]
	private int m_HPBarDisplayCount;
	[SerializeField]
	private int m_HPBarFadeOutCount;
	[SerializeField]
	private int m_HPBarBlinkFrequencyCount;
	[SerializeField]
	private float m_HPBarBlinkPercentage;
	[SerializeField]
	private float m_RunAwayAccelerateScale;
	[SerializeField]
	private int[] m_ReplayValidScale;
	[SerializeField]
	private int[] m_BattleProgressStep;
	[SerializeField]
	private float m_Store20Criterion;
	[SerializeField]
	private float m_Store40Criterion;
	[SerializeField]
	private float m_Store60Criterion;
	[SerializeField]
	private float m_Store80Criterion;
	[SerializeField]
	private float m_Plunder200Criterion;
	[SerializeField]
	private float m_Plunder400Criterion;
	[SerializeField]
	private float m_Plunder600Criterion;
	[SerializeField]
	private float m_Plunder800Criterion;
	[SerializeField]
	private int m_LevelsPerAge;
    [SerializeField]
    private float m_ProduceRateHourPercentage;
    public float ProduceRateHourPercentage { get { return m_ProduceRateHourPercentage; } }
    [SerializeField]
    private int m_HourToSecond;
    public int HourToSecond { get { return m_HourToSecond; } }
    [SerializeField]
    private string[] m_AgeMapNameArray;
	[SerializeField]
	private string[] m_AgeNameArray;
	[SerializeField]
	private string[] m_CityHallSurfacePrefabName;
	[SerializeField]
	private string[] m_BarracksSurfacePrefabName;
	[SerializeField]
	private string[] m_ArmyCampSurfacePrefabName;
	[SerializeField]
	private string[] m_FortressSurfacePrefabName;
	[SerializeField]
	private string[] m_GoldMineSurfacePrefabName;
	[SerializeField]
	private string[] m_FarmSurfacePrefabName;
	[SerializeField]
	private string[] m_GoldStorageSurfacePrefabName;
	[SerializeField]
	private string[] m_FoodStorageSurfacePrefabName;
	[SerializeField]
	private string[] m_BuilderHutSurfacePrefabName;
	[SerializeField]
	private string[] m_WallSurfacePrefabName;
	[SerializeField]
	private string[] m_DefenseTowerSurfacePrefabName;
	[SerializeField]
	private string[] m_TavernSurfacePrefabName;
	[SerializeField]
	private string[] m_PropsStorageSurfacePrefabName;
	[SerializeField]
	private string[] m_ArtillerySurfacePrefabName;
	[SerializeField]
	private string m_SurfacePrefabPrefix;
    [SerializeField]
    private string[] m_AncientTotemSurfacePrefabName;
	[SerializeField]
    private string[] m_BerserkerStatueSurfacePrefabName;
	[SerializeField]
	private string[] m_RangerStatueSurfacePrefabName;
	[SerializeField]
	private string[] m_MarauderStatueSurfacePrefabName;
	[SerializeField]
	private string[] m_MTStatueSurfacePrefabName;
 
	[SerializeField]
	private Dictionary<BuildingType, string[]> m_SurfacePrefabDict;
    [SerializeField]
    private Dictionary<AchievementBuildingType, string[]> m_SurfaceAchievPrefabDict;
	[SerializeField]
    private float m_ProgressBarInterval;
    [SerializeField]
    private int m_UserNameLennth;
	[SerializeField]
	private string[] m_TipsInfos;
	
	[SerializeField]
	private int m_MercenarySlingerOrder;
	[SerializeField]
	private int m_MercenaryHerculesOrder;
	[SerializeField]
	private int m_MercenaryKodoOrder;
	[SerializeField]
	private int m_MercenaryHerculesIIOrder;
	[SerializeField]
	private int m_MercenaryKodoIIOrder;
	[SerializeField]
	private int m_MercenaryArsonistOrder;
	[SerializeField]
	private int m_MercenaryArsonistIIOrder;
	[SerializeField]
	private int m_MercenaryPhalanxSoldierOrder;
	[SerializeField]
	private int m_MercenaryCatapultsOrder;
	[SerializeField]
	private int m_MercenaryCrazyKodoOrder;
	[SerializeField]
	private int m_MercenaryPhalanxSoldierIIOrder;
	[SerializeField]
	private int m_MercenaryCatapultsIIOrder;
	[SerializeField]
	private int m_MercenaryDemolisherOrder;
	
	private Dictionary<MercenaryType, int> m_MercenaryOrders = new Dictionary<MercenaryType, int>();
	
    public int UserNameLennth
    {
        get { return this.m_UserNameLennth; }
    }
    public float ProgressBarInterval
    {
        get { return this.m_ProgressBarInterval; }
    }
	public int TicksPerSecond
	{
		get { return this.m_TicksPerSecond; }
	}
	
	public int HPBarDisplayCount
	{
		get { return this.m_HPBarDisplayCount; }
	}
	
	public int HPBarFadeOutCount
	{
		get { return this.m_HPBarFadeOutCount; } 
	}
	
	public int HPBarBlinkFrequencyCount
	{
		get { return this.m_HPBarBlinkFrequencyCount; } 
	}
	
	public float HPBarBlinkPercentage
	{
		get { return this.m_HPBarBlinkPercentage; } 
	}
	
	public float RunAwayAccelerateScale
	{
		get { return this.m_RunAwayAccelerateScale; }
	}
	
	public int[] ReplayValidScale
	{
		get { return this.m_ReplayValidScale; } 
	}
	
	public int[] BattleProgressStep
	{
		get { return this.m_BattleProgressStep; }
	}
	
	public float Store20Criterion
	{
		get { return this.m_Store20Criterion; }
	}
	public float Store40Criterion
	{
		get { return this.m_Store40Criterion; }
	}
	public float Store60Criterion
	{
		get { return this.m_Store60Criterion; }
	}
	public float Store80Criterion
	{
		get { return this.m_Store80Criterion; }
	}
	public float Plunder200Criterion
	{
		get { return this.m_Plunder200Criterion; } 
	}
	public float Plunder400Criterion
	{
		get { return this.m_Plunder400Criterion; } 
	}
	public float Plunder600Criterion
	{
		get { return this.m_Plunder600Criterion; } 
	}
	public float Plunder800Criterion
	{
		get { return this.m_Plunder800Criterion; } 
	}
	public string[] TipsInfos
	{
		get { return this.m_TipsInfos; }
	}
	
	private static ClientConfigConstants s_Sigleton;
	
	public static ClientConfigConstants Instance	
	{
		get { return s_Sigleton; }
	}
	
	public string GetSurfacePrefabName(BuildingType type, Age age)
	{
		return string.IsNullOrEmpty(this.m_SurfacePrefabPrefix) ? this.m_SurfacePrefabDict[type][(int)age] :
			this.m_SurfacePrefabPrefix + "/" + this.m_SurfacePrefabDict[type][(int)age];
	}
    public string GetSurfacePrefabName(AchievementBuildingType type, Age age)
    {
        return string.IsNullOrEmpty(this.m_SurfacePrefabPrefix) ? this.m_SurfaceAchievPrefabDict[type][(int)age] :
            this.m_SurfacePrefabPrefix + "/" + this.m_SurfaceAchievPrefabDict[type][(int)age];
    }
	public string GetAgeName(Age age)
	{
		return this.m_AgeNameArray[(int)age];
	}
	
	public string GetAgeMapName(Age age)
	{
		return this.m_AgeMapNameArray[(int)age];
	}
	
	public int GetMercenaryOrder(MercenaryType mercenaryType)
	{
		return this.m_MercenaryOrders[mercenaryType];
	}
	
	// Use this for initialization
	void Awake () 
	{
		s_Sigleton = this;
		GameObject.DontDestroyOnLoad(this.gameObject);
		this.m_SurfacePrefabDict = new Dictionary<BuildingType, string[]>();
        this.m_SurfaceAchievPrefabDict = new Dictionary<AchievementBuildingType, string[]>();
		this.m_SurfacePrefabDict.Add(BuildingType.CityHall, this.m_CityHallSurfacePrefabName);
		this.m_SurfacePrefabDict.Add(BuildingType.Barracks, this.m_BarracksSurfacePrefabName);
		this.m_SurfacePrefabDict.Add(BuildingType.ArmyCamp, this.m_ArmyCampSurfacePrefabName);
		this.m_SurfacePrefabDict.Add(BuildingType.Fortress, this.m_FortressSurfacePrefabName);
		this.m_SurfacePrefabDict.Add(BuildingType.GoldMine, this.m_GoldMineSurfacePrefabName);
		this.m_SurfacePrefabDict.Add(BuildingType.Farm, this.m_FarmSurfacePrefabName);
		this.m_SurfacePrefabDict.Add(BuildingType.GoldStorage, this.m_GoldStorageSurfacePrefabName);
		this.m_SurfacePrefabDict.Add(BuildingType.FoodStorage, this.m_FoodStorageSurfacePrefabName);
		this.m_SurfacePrefabDict.Add(BuildingType.BuilderHut, this.m_BuilderHutSurfacePrefabName);
		this.m_SurfacePrefabDict.Add(BuildingType.DefenseTower, this.m_DefenseTowerSurfacePrefabName);
		this.m_SurfacePrefabDict.Add(BuildingType.Tavern, this.m_TavernSurfacePrefabName);
		this.m_SurfacePrefabDict.Add(BuildingType.PropsStorage, this.m_PropsStorageSurfacePrefabName);
		this.m_SurfacePrefabDict.Add(BuildingType.Artillery, this.m_ArtillerySurfacePrefabName);
		
		this.m_MercenaryOrders.Add(MercenaryType.Arsonist, this.m_MercenaryArsonistOrder);
		this.m_MercenaryOrders.Add(MercenaryType.ArsonistII, this.m_MercenaryArsonistIIOrder);
		this.m_MercenaryOrders.Add(MercenaryType.Catapults, this.m_MercenaryCatapultsOrder);
		this.m_MercenaryOrders.Add(MercenaryType.Hercules, this.m_MercenaryHerculesOrder);
		this.m_MercenaryOrders.Add(MercenaryType.HerculesII, this.m_MercenaryHerculesIIOrder);
		this.m_MercenaryOrders.Add(MercenaryType.Kodo, this.m_MercenaryKodoOrder);
		this.m_MercenaryOrders.Add(MercenaryType.KodoII, this.m_MercenaryKodoIIOrder);
		this.m_MercenaryOrders.Add(MercenaryType.PhalanxSoldier, this.m_MercenaryPhalanxSoldierOrder);
		this.m_MercenaryOrders.Add(MercenaryType.Slinger, this.m_MercenarySlingerOrder);
		this.m_MercenaryOrders.Add(MercenaryType.CrazyKodo, this.m_MercenaryCrazyKodoOrder);
		this.m_MercenaryOrders.Add(MercenaryType.PhalanxSoldierII, this.m_MercenaryPhalanxSoldierIIOrder);
		this.m_MercenaryOrders.Add(MercenaryType.CatapultsII, this.m_MercenaryCatapultsIIOrder);
		this.m_MercenaryOrders.Add(MercenaryType.Demolisher, this.m_MercenaryDemolisherOrder);

        this.m_SurfaceAchievPrefabDict.Add(AchievementBuildingType.AncientTotem, this.m_AncientTotemSurfacePrefabName);
 		this.m_SurfaceAchievPrefabDict.Add(AchievementBuildingType.BerserkerStatue, this.m_BerserkerStatueSurfacePrefabName);
		this.m_SurfaceAchievPrefabDict.Add(AchievementBuildingType.RangerStatue, this.m_RangerStatueSurfacePrefabName);
		this.m_SurfaceAchievPrefabDict.Add(AchievementBuildingType.MarauderStatue, this.m_MarauderStatueSurfacePrefabName);
		this.m_SurfaceAchievPrefabDict.Add(AchievementBuildingType.MTStatue, this.m_MTStatueSurfacePrefabName);
	}
	
}
