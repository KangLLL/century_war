using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ConfigUtilities.Enums;
using ConfigUtilities;

public class BuildingConstruct : MonoBehaviour {
  
	// Use this for initialization
    static BuildingConstruct m_Instance;
    static public BuildingConstruct Instance
    {
        get
        {
            return m_Instance;
        }
    }
    void Awake()
    {
        m_Instance = this;
    }
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	

	}
    //public void ConstructBuilding(BuildingLogicObject buildingLogicObject ,bool created) 
    //{ 
    //    GameObject building = Instantiate(Resources.Load(buildingLogicObject.BuildingPrefabName, typeof(GameObject))) as GameObject;
    //    building.transform.parent = SceneManager.Instance.transform;
    //    BuildingBehavior buildingBehavior = building.transform.GetComponent<BuildingBehavior>();
    //    buildingBehavior.BuildingLogicObject = buildingLogicObject;
    //    buildingBehavior.Created = created;
    //}
    //public void ConstructBuilding(BuildingConfigData buildingConfigData, BuildingType buildingType, bool created)
    //{ 
    //    GameObject building = Instantiate(Resources.Load(buildingConfigData.BuildingPrefabName, typeof(GameObject))) as GameObject;
    //    building.transform.parent = SceneManager.Instance.transform;
    //    BuildingBehavior buildingBehavior = building.transform.GetComponent<BuildingBehavior>();
    //    buildingBehavior.BuildingConfigData = buildingConfigData;
    //    buildingBehavior.BuildingType = buildingType;
    //    buildingBehavior.Created = created;
    //    if (!created)
    //        SceneManager.Instance.BuildingBehaviorTemporary = buildingBehavior;
    //}
/*
    public BuildingData BuildingConfigHelper(BuildingType buildingType, int level = 0)
    {
        ConfigUtilities.BuildingConfigData buildingConfigData = ConfigUtilities.ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(buildingType, level);
        BuildingData buildingData = new BuildingData()
        {
            AP = buildingConfigData.AP,
            ApMaxScope = buildingConfigData.ApMaxScope,
            ApMinScope = buildingConfigData.ApMinScope,
            ApSpeed = buildingConfigData.ApSpeed,
            ArmyCapacity = buildingConfigData.ArmyCapacity,
            AttackType = (AttackType)buildingConfigData.AttackType,
            ArmyProduceCapacity = buildingConfigData.ArmyProduceCapacity,
            BuildingPrefabName = buildingConfigData.BuildingPrefabName,
            BuildingType = buildingType,
            CanAttack = buildingConfigData.CanAttack,
            CanClan = buildingConfigData.CanClan,
            CanCollectFood = buildingConfigData.CanProduceFood,
            CanCollectGold = buildingConfigData.CanProduceGold,
            CanCollectOil = buildingConfigData.CanProduceOil,
            CanHelpArmy = buildingConfigData.CanHelpArmy,
            CanProduceArmy = buildingConfigData.CanProduceArmy,
            CanProduceItem = buildingConfigData.CanProduceItem,
            CanStoreFood = buildingConfigData.CanStoreFood,
            CanStoreGold = buildingConfigData.CanStoreGold,
            CanStoreItem = buildingConfigData.CanStoreItem,
            CanStoreOil = buildingConfigData.CanStoreOil,
            CanUpgradeArmy = buildingConfigData.CanUpgradeArmy,
            CanUpgradeItem = buildingConfigData.CanUpgradeItem,
            StoreFoodCapacity = buildingConfigData.StoreFoodCapacity,
            StoreItemCapacity = buildingConfigData.StoreItemCapacity,
            StoreGoldCapacity = buildingConfigData.StoreGoldCapacity,
            StoreOilCapacity = buildingConfigData.StoreOilCapacity,
            Description = buildingConfigData.Description,
            //Favorite = buildingConfigData.Favorite,
            IsMaximumLevel = buildingConfigData.IsMaximumLevel,
            MaximumLevel = buildingConfigData.MaximumLevel, 
            MaxHP = buildingConfigData.MaxHP,
            Name = buildingConfigData.Name,
            PlunderRate = buildingConfigData.PlunderRate,
            ProduceArmyEfficiency = 1,
            ProduceFoodEfficiency = buildingConfigData.ProduceFoodEfficiency,
            ProduceGoldEfficiency = buildingConfigData.ProduceGoldEfficiency,
            ProduceItemEfficiency = 1,
            ProduceOilEfficiency = buildingConfigData.ProduceOilEfficiency,
            TargetType = (TargetType)buildingConfigData.TargetType,
            UpgradeFood = buildingConfigData.UpgradeFood,
            UpgradeGem = buildingConfigData.UpgradeGem,
            UpgradeGold = buildingConfigData.UpgradeGold,
            UpgradeOil = buildingConfigData.UpgradeOil,
            UpgradeRemainingWorkload = buildingConfigData.UpgradeWorkload,
            UpgradeRewardExp = buildingConfigData.UpgradeRewardExp,
            UpgradeRewardFood = buildingConfigData.UpgradeRewardFood,
            UpgradeRewardGem = buildingConfigData.UpgradeRewardGem,
            UpgradeRewardGold = buildingConfigData.UpgradeRewardGold,
            UpgradeRewardOil = buildingConfigData.UpgradeRewardOil,
            UpgradeWorkload = buildingConfigData.UpgradeWorkload,
            ActorWorkEfficiency = 1,//临时数据 
            Level = level,
            InitialLevel = buildingConfigData.InitialLevel,
            //BuildingNO =,
            //BuildingPosition = ,
            //CurrentBuilidngState = ,
            //CurrentStoreArmy = ,
            //CurrentStoreFood = ,
            //CurrentStoreGold =,
            //CurrentStoreItem = ,
            //CurrentStoreOil =,
        };
        buildingData.ActorObstacleList = new List<TilePosition>();
        buildingData.BuildingObstacleList = new List<TilePosition>();
        foreach (ConfigUtilities.Structs.TilePoint tilePoint in buildingConfigData.ActorObstacleList)
        {
            buildingData.ActorObstacleList.Add(new TilePosition(tilePoint.column, tilePoint.row));
        }
        foreach (ConfigUtilities.Structs.TilePoint tilePoint in buildingConfigData.BuildingObstacleList)
        {
            buildingData.BuildingObstacleList.Add(new TilePosition(tilePoint.column, tilePoint.row));
        }
        return buildingData;
    }
 */
}
