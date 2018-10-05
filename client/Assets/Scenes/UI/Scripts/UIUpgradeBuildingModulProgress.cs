using UnityEngine;
using System.Collections;
using ConfigUtilities;
using ConfigUtilities.Enums;
public class UIUpgradeBuildingModulProgress : UIWindowItemCommon
{
    [SerializeField] UILabel m_UILabelTile;
    //[SerializeField] UILabel m_UILabelName;
    [SerializeField] tk2dSprite m_Tk2dSpriteIcon;
    [SerializeField] UIUpgradeProgressBar[] m_UIUpgradeProgressBar;//Food,Gold,HP,Army,DPS,Gold PR,Food PR,PropsStorageCapacity,Efficiency,Attack,Defense,Special
    [SerializeField] Vector3 INITIAL_LOCAL_POSITION = new Vector3(-10, 200, -3);
    [SerializeField] Vector3 OFFSET_LOCAL_POSITION = new Vector3(0, -30, 0);

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public override void SetWindowItem()
    { 
        int buildingLevel = base.BuildingLogicData.Level;
        int buildingLevelMax = base.BuildingLogicData.MaximumLevel;
        int levelNext = (base.BuildingLogicData.Level - base.BuildingLogicData.InitialLevel) / base.BuildingLogicData.UpgradeStep * base.BuildingLogicData.UpgradeStep + base.BuildingLogicData.InitialLevel + base.BuildingLogicData.UpgradeStep;
        int buildingLevenNext = levelNext > buildingLevelMax ? base.BuildingLogicData.Level : levelNext;
        m_UILabelTile.text = base.BuildingLogicData.Name + StringConstants.PROMPT_UPGRADE + StringConstants.PROMPT_TO + StringConstants.LEFT_PARENTHESES +  levelNext + StringConstants.RIGHT_PARENTHESES + StringConstants.PROMPT_LV + StringConstants.QUESTION_MARK;
        //m_UILabelName.text = base.BuildingLogicData.Name;
        BuildingConfigData buildingConfigDataCurrent = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(base.BuildingLogicData.BuildingIdentity.buildingType, buildingLevel);
        BuildingConfigData buildingConfigDataNext = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(base.BuildingLogicData.BuildingIdentity.buildingType, buildingLevenNext);
        BuildingConfigData buildingConfigDataMax = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(base.BuildingLogicData.BuildingIdentity.buildingType, buildingLevelMax);
        GameObject building = Resources.Load(ClientStringConstants.BUILDING_SCENE_RESOURCE_PREFAB_PREFIX_NAME + ClientStringConstants.BUILDING_OBJECT_PREFAB_PREFIX_NAME + buildingConfigDataNext.BuildingPrefabName, typeof(GameObject)) as GameObject;
   
        tk2dSprite tk2dSpriteBuilding = building.transform.FindChild(ClientSystemConstants.BACKGROUND_NAME).GetComponent<tk2dSprite>();
        m_Tk2dSpriteIcon.SetSprite(tk2dSpriteBuilding.Collection, tk2dSpriteBuilding.spriteId);
        
        #region progress
        for (int i = 0, count = m_UIUpgradeProgressBar.Length; i < count; i++)
        {
            m_UIUpgradeProgressBar[i].gameObject.SetActive(false);
        }
        UIUpgradeProgressBar[] progressBarArray = new UIUpgradeProgressBar[0];
        ProgressParam[] progressParamArray = new ProgressParam[0];

        //Food,Gold,HP,Army,DPS,Gold PR,Food PR,Spell,Efficiency, Attack,Defense,Special
        ProgressParam progressParamFood = SystemFunction.CalculateParam(buildingConfigDataCurrent.StoreFoodCapacity, buildingConfigDataNext.StoreFoodCapacity, buildingConfigDataMax.StoreFoodCapacity);
        ProgressParam progressParamGold = SystemFunction.CalculateParam(buildingConfigDataCurrent.StoreGoldCapacity, buildingConfigDataNext.StoreGoldCapacity, buildingConfigDataMax.StoreGoldCapacity);
        ProgressParam progressParamHP = SystemFunction.CalculateParam(buildingConfigDataCurrent.MaxHP, buildingConfigDataNext.MaxHP, buildingConfigDataMax.MaxHP);
        ProgressParam progressParamArmy = SystemFunction.CalculateParam(buildingConfigDataCurrent.ArmyCapacity, buildingConfigDataNext.ArmyCapacity, buildingConfigDataMax.ArmyCapacity);
        ProgressParam progressParamDps = SystemFunction.CalculateParam(buildingConfigDataCurrent.AttackValue / buildingConfigDataCurrent.AttackCD, buildingConfigDataNext.AttackValue / buildingConfigDataNext.AttackCD, buildingConfigDataMax.AttackValue / buildingConfigDataMax.AttackCD);
        ProgressParam progressParamGoldPR = SystemFunction.CalculateParam(buildingConfigDataCurrent.ProduceGoldEfficiency * 3600, buildingConfigDataNext.ProduceGoldEfficiency * 3600, buildingConfigDataMax.ProduceGoldEfficiency * 3600);
        ProgressParam progressParamFoodPR = SystemFunction.CalculateParam(buildingConfigDataCurrent.ProduceFoodEfficiency * 3600, buildingConfigDataNext.ProduceFoodEfficiency * 3600, buildingConfigDataMax.ProduceFoodEfficiency * 3600);

        bool isCityHall = base.BuildingLogicData.BuildingType == BuildingType.CityHall;
        ProgressParam progressParamAttackProps = isCityHall ? SystemFunction.CalculateParam(LogicController.Instance.MaxAttackPropsSlot, ConfigInterface.Instance.PropsRestrictionConfigHelper.GetPropsRestrictions(LogicController.Instance.CurrentCityHallLevel + LogicController.Instance.GetBuildingObject(new BuildingIdentity() { buildingNO = 0, buildingType = BuildingType.CityHall }).UpgradeStep).MaxAttackPropsSlotNumber, 1) : null;
        ProgressParam progressParamDefenseProps = isCityHall ? SystemFunction.CalculateParam(LogicController.Instance.MaxDefenseObjectNumber, ConfigInterface.Instance.PropsRestrictionConfigHelper.GetPropsRestrictions(LogicController.Instance.CurrentCityHallLevel + LogicController.Instance.GetBuildingObject(new BuildingIdentity() { buildingNO = 0, buildingType = BuildingType.CityHall }).UpgradeStep).MaxDefensePropsSlotNumber, 1) : null;
        ProgressParam progressParamSpecialProps = isCityHall ? SystemFunction.CalculateParam(LogicController.Instance.MaxAchievementBuildingNumber, ConfigInterface.Instance.PropsRestrictionConfigHelper.GetPropsRestrictions(LogicController.Instance.CurrentCityHallLevel + LogicController.Instance.GetBuildingObject(new BuildingIdentity() { buildingNO = 0, buildingType = BuildingType.CityHall }).UpgradeStep).MaxAchievementBuildingNumber, 1) : null;
      
        ProgressParam progressParamPropsStorageCapacity = SystemFunction.CalculateParam(buildingConfigDataCurrent.StorePropsCapacity, buildingConfigDataNext.StorePropsCapacity, buildingConfigDataMax.StorePropsCapacity);

        //ProgressParam progressParamSpell = SystemFunction.CalculateParam(buildingConfigDataCurrent.StoreItemCapacity, buildingConfigDataNext.StoreItemCapacity, buildingConfigDataMax.StoreItemCapacity);
      
        switch (base.BuildingLogicData.BuildingIdentity.buildingType)
        {
            case BuildingType.ArmyCamp://Army,HP   
                progressBarArray = SystemFunction.ConverTObjectToArray<UIUpgradeProgressBar>(m_UIUpgradeProgressBar[3], m_UIUpgradeProgressBar[2]);
                progressParamArray = SystemFunction.ConverTObjectToArray<ProgressParam>(progressParamArmy, progressParamHP);
                break;
            case BuildingType.Barracks://HP
                progressBarArray = SystemFunction.ConverTObjectToArray<UIUpgradeProgressBar>(m_UIUpgradeProgressBar[2]);
                progressParamArray = SystemFunction.ConverTObjectToArray<ProgressParam>(progressParamHP);
                break;
            case BuildingType.BuilderHut://Food,Efficiency
                progressBarArray = SystemFunction.ConverTObjectToArray<UIUpgradeProgressBar>(m_UIUpgradeProgressBar[0], m_UIUpgradeProgressBar[8]);
                BuilderConfigData builderConfigDataCurrent = ConfigInterface.Instance.BuilderConfigHelper.GetBuilderData(base.BuildingLogicData.Level);
                BuilderConfigData builderConfigDataNext = ConfigInterface.Instance.BuilderConfigHelper.GetBuilderData(base.BuildingLogicData.Level + 1);
                BuilderConfigData builderConfigDataMax = ConfigInterface.Instance.BuilderConfigHelper.GetBuilderData(base.BuildingLogicData.MaximumLevel);
                ProgressParam progressParamEfficiency = SystemFunction.CalculateParam(builderConfigDataCurrent.BuildEfficiency, builderConfigDataNext.BuildEfficiency, builderConfigDataMax.BuildEfficiency);
                progressParamArray = SystemFunction.ConverTObjectToArray<ProgressParam>(progressParamFood, progressParamEfficiency);
                break;
            case BuildingType.CityHall://Food,Gold,HP,Attack,Defense,Special
                progressBarArray = SystemFunction.ConverTObjectToArray<UIUpgradeProgressBar>(m_UIUpgradeProgressBar[0], m_UIUpgradeProgressBar[1], m_UIUpgradeProgressBar[2], m_UIUpgradeProgressBar[9], m_UIUpgradeProgressBar[10], m_UIUpgradeProgressBar[11]);
                progressParamArray = SystemFunction.ConverTObjectToArray<ProgressParam>(progressParamFood, progressParamGold, progressParamHP, progressParamAttackProps, progressParamDefenseProps, progressParamSpecialProps);
                break;
			/*
            case BuildingType.ClanCastle://Army,HP
                progressBarArray = SystemFunction.ConvertObjectToArray(m_UIUpgradeProgressBar[3], m_UIUpgradeProgressBar[2]);
                progressParamArray = SystemFunction.ConvertParamToArray(progressParamArmy, progressParamHP);
                break;
                */
            case BuildingType.DefenseTower://Dps,HP
                progressBarArray = SystemFunction.ConverTObjectToArray<UIUpgradeProgressBar>(m_UIUpgradeProgressBar[4], m_UIUpgradeProgressBar[2]);
                progressParamArray = SystemFunction.ConverTObjectToArray<ProgressParam>(progressParamDps, progressParamHP);
                break;
            case BuildingType.Farm://Food,Food RP,HP
                progressBarArray = SystemFunction.ConverTObjectToArray<UIUpgradeProgressBar>(m_UIUpgradeProgressBar[0], m_UIUpgradeProgressBar[6], m_UIUpgradeProgressBar[2]);
                progressParamArray = SystemFunction.ConverTObjectToArray<ProgressParam>(progressParamFood, progressParamFoodPR, progressParamHP);
                break;
            case BuildingType.FoodStorage://Food,HP
                progressBarArray = SystemFunction.ConverTObjectToArray<UIUpgradeProgressBar>(m_UIUpgradeProgressBar[0], m_UIUpgradeProgressBar[2]);
                progressParamArray = SystemFunction.ConverTObjectToArray<ProgressParam>(progressParamFood, progressParamHP);
                break;
            case BuildingType.Fortress://Dps,HP
                progressBarArray = SystemFunction.ConverTObjectToArray<UIUpgradeProgressBar>(m_UIUpgradeProgressBar[4], m_UIUpgradeProgressBar[2]);
                progressParamArray = SystemFunction.ConverTObjectToArray<ProgressParam>(progressParamDps, progressParamHP);
                break;
            case BuildingType.GoldMine://Gold,Gold PR,HP
                progressBarArray = SystemFunction.ConverTObjectToArray<UIUpgradeProgressBar>(m_UIUpgradeProgressBar[1], m_UIUpgradeProgressBar[5], m_UIUpgradeProgressBar[2]);
                progressParamArray = SystemFunction.ConverTObjectToArray<ProgressParam>(progressParamGold, progressParamGoldPR, progressParamHP);
                break;
            case BuildingType.GoldStorage://Gold,HP
                progressBarArray = SystemFunction.ConverTObjectToArray<UIUpgradeProgressBar>(m_UIUpgradeProgressBar[1], m_UIUpgradeProgressBar[2]);
                progressParamArray = SystemFunction.ConverTObjectToArray<ProgressParam>(progressParamGold, progressParamHP);
                break;
			/*
            case BuildingType.MagicTower://DPS,HP
                progressBarArray = SystemFunction.ConvertObjectToArray(m_UIUpgradeProgressBar[4], m_UIUpgradeProgressBar[2]);
                progressParamArray = SystemFunction.ConvertParamToArray(progressParamDps, progressParamHP);
                break;
            case BuildingType.Mortar://Dps,HP
                progressBarArray = SystemFunction.ConvertObjectToArray(m_UIUpgradeProgressBar[4], m_UIUpgradeProgressBar[2]);
                progressParamArray = SystemFunction.ConvertParamToArray(progressParamDps, progressParamHP);
                break;
             */
            case BuildingType.Wall://HP
                progressBarArray = SystemFunction.ConverTObjectToArray<UIUpgradeProgressBar>(m_UIUpgradeProgressBar[2]);
                progressParamArray = SystemFunction.ConverTObjectToArray<ProgressParam>(progressParamHP);
                break;
            case BuildingType.Tavern://HP
                progressBarArray = SystemFunction.ConverTObjectToArray<UIUpgradeProgressBar>(m_UIUpgradeProgressBar[2]);
                progressParamArray = SystemFunction.ConverTObjectToArray<ProgressParam>(progressParamHP);
                break;
            case BuildingType.PropsStorage://HP ,Capacity
                progressBarArray = SystemFunction.ConverTObjectToArray<UIUpgradeProgressBar>(m_UIUpgradeProgressBar[2], m_UIUpgradeProgressBar[7]);
                progressParamArray = SystemFunction.ConverTObjectToArray<ProgressParam>(progressParamHP, progressParamPropsStorageCapacity);
                break;
            case BuildingType.Artillery://Dps,HP
                progressBarArray = SystemFunction.ConverTObjectToArray<UIUpgradeProgressBar>(m_UIUpgradeProgressBar[4], m_UIUpgradeProgressBar[2]);
                progressParamArray = SystemFunction.ConverTObjectToArray<ProgressParam>(progressParamDps, progressParamHP);
                break;

        }
        for (int i = 0, count = progressBarArray.Length; i < count; i++)
        {
           progressBarArray[i].gameObject.SetActive(true);
           progressBarArray[i].transform.localPosition = INITIAL_LOCAL_POSITION + OFFSET_LOCAL_POSITION * i;
           progressBarArray[i].SetProgressBar(progressParamArray[i].ProgressCurrent, progressParamArray[i].Value);
           progressBarArray[i].SetUpgradeProgressBar2(progressParamArray[i].ProgressNext);
        }
        #endregion
    }

}
public class ProgressParam
{
    public float ProgressCurrent { get; set; }
    public float ProgressNext { get; set; }
    public string Value { get; set; }
}
