using UnityEngine;
using System.Collections;
using ConfigUtilities;
using ConfigUtilities.Enums;
using System.Collections.Generic;
public class UIBuildingInformationModulProgress : UIWindowItemCommon
{
	[SerializeField] UILabel m_UILabelTile;
	//[SerializeField]
	//UILabel m_UILabelName;
	[SerializeField] tk2dSprite m_Tk2dSpriteIcon;
	[SerializeField] UIUpgradeProgressBar[] m_UIUpgradeProgressBar;//Food,Gold,HP,Army,DPS,Gold PR,Food PR,PropsStorageCapacity,Efficiency,Attack,Defense,Special,Live
	[SerializeField] Vector3 INITIAL_LOCAL_POSITION = new Vector3(-10, 200, -3);
	[SerializeField] Vector3 OFFSET_LOCAL_POSITION = new Vector3(0, -30, 0);
	bool m_IsBuildingLogicData = false;
	// Use this for initialization
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{
		this.OnProduceResource();
		this.OnProduceArmy();
		//this.SetWindowItem();
	}
	
	public override void SetWindowItem()
	{
		this.m_IsBuildingLogicData = true;
		m_UILabelTile.text = base.BuildingLogicData.Name + StringConstants.LEFT_PARENTHESES + base.BuildingLogicData.Level.ToString() + StringConstants.RIGHT_PARENTHESES + StringConstants.PROMPT_LV ;
		//m_UILabelName.text = base.BuildingLogicData.Name;
		BuildingConfigData buildingConfigDataCurrent = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(base.BuildingLogicData.BuildingIdentity.buildingType, base.BuildingLogicData.Level);
		
		GameObject building = Resources.Load(ClientStringConstants.BUILDING_SCENE_RESOURCE_PREFAB_PREFIX_NAME + ClientStringConstants.BUILDING_OBJECT_PREFAB_PREFIX_NAME + buildingConfigDataCurrent.BuildingPrefabName, typeof(GameObject)) as GameObject;
		tk2dSprite tk2dSpriteBuilding = building.transform.FindChild(ClientSystemConstants.BACKGROUND_NAME).GetComponent<tk2dSprite>();
		m_Tk2dSpriteIcon.SetSprite(tk2dSpriteBuilding.Collection, tk2dSpriteBuilding.spriteId);
		
		#region progress
		for (int i = 0, count = m_UIUpgradeProgressBar.Length; i < count; i++)
		{
			m_UIUpgradeProgressBar[i].gameObject.SetActive(false);
		}
		
		UIUpgradeProgressBar[] progressBarArray = new UIUpgradeProgressBar[0];
		ProgressParam[] progressParamArray = new ProgressParam[0]; 
		//Food,Gold,HP,Army,DPS,Gold PR,Food PR,Spell,Efficiency
		ProgressParam progressParamFood = CalculateParam(base.BuildingLogicData.CurrentStoreFood, buildingConfigDataCurrent.StoreFoodCapacity);
		ProgressParam progressParamGold = CalculateParam(base.BuildingLogicData.CurrentStoreGold, buildingConfigDataCurrent.StoreGoldCapacity);
		ProgressParam progressParamHP = CalculateParam(buildingConfigDataCurrent.MaxHP, buildingConfigDataCurrent.MaxHP);


        ProgressParam progressParamArmy = (SceneManager.Instance.SceneMode == SceneMode.SceneBuild) ? CalculateParam(LogicController.Instance.CurrentAvailableArmyCapacity, LogicController.Instance.CampsTotalCapacity) : CalculateParam(LogicController.Instance.CurrentFriend.AlreadyArmyCapacity, LogicController.Instance.CurrentFriend.TotalCampCapacity);
 
        ProgressParam progressParamDps = CalculateParam(buildingConfigDataCurrent.AttackValue / base.BuildingLogicData.AttackCD, buildingConfigDataCurrent.AttackValue / buildingConfigDataCurrent.AttackCD);
		ProgressParam progressParamGoldPR = CalculateParam(buildingConfigDataCurrent.ProduceGoldEfficiency * 3600, buildingConfigDataCurrent.ProduceGoldEfficiency * 3600);
		ProgressParam progressParamFoodPR = CalculateParam(buildingConfigDataCurrent.ProduceFoodEfficiency * 3600, buildingConfigDataCurrent.ProduceFoodEfficiency * 3600);
		
		ProgressParam progressParamAttackProps = (SceneManager.Instance.SceneMode == SceneMode.SceneBuild) ? CalculateParam(LogicController.Instance.AvailableBattlePropsNumner, LogicController.Instance.MaxAttackPropsSlot) : CalculateParam(0, LogicController.Instance.MaxAttackPropsSlot);
		ProgressParam progressParamDefenseProps = (SceneManager.Instance.SceneMode == SceneMode.SceneBuild) ? CalculateParam(LogicController.Instance.AvailableDefenseObjectNumber, LogicController.Instance.MaxDefenseObjectNumber) : CalculateParam(0, LogicController.Instance.MaxDefenseObjectNumber);
		ProgressParam progressParamSpecialProps = (SceneManager.Instance.SceneMode == SceneMode.SceneBuild) ? CalculateParam(LogicController.Instance.AvailableAchievementBuildingNumber, LogicController.Instance.MaxAchievementBuildingNumber) : CalculateParam(0, LogicController.Instance.MaxAchievementBuildingNumber);
		
		
		ProgressParam progressParamPropsStorageCapacity = (SceneManager.Instance.SceneMode == SceneMode.SceneBuild) ? CalculateParam(LogicController.Instance.AllProps.Count, buildingConfigDataCurrent.StorePropsCapacity) : CalculateParam(0, buildingConfigDataCurrent.StorePropsCapacity);
        
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
		case BuildingType.BuilderHut://Efficiency,HP
			progressBarArray = SystemFunction.ConverTObjectToArray<UIUpgradeProgressBar>(m_UIUpgradeProgressBar[8], m_UIUpgradeProgressBar[2]);
			BuilderConfigData builderConfigDataCurrent = ConfigInterface.Instance.BuilderConfigHelper.GetBuilderData(base.BuildingLogicData.Level); 
			ProgressParam progressParamEfficiency = CalculateParam(builderConfigDataCurrent.BuildEfficiency, builderConfigDataCurrent.BuildEfficiency);
			progressParamArray = SystemFunction.ConverTObjectToArray<ProgressParam>(progressParamEfficiency, progressParamHP);
			break;
		case BuildingType.CityHall://Food,Gold,HP,Attack,Defense,Special
			progressBarArray = SystemFunction.ConverTObjectToArray<UIUpgradeProgressBar>(m_UIUpgradeProgressBar[0], m_UIUpgradeProgressBar[1], m_UIUpgradeProgressBar[2], m_UIUpgradeProgressBar[9], m_UIUpgradeProgressBar[10], m_UIUpgradeProgressBar[11]);
			progressParamArray = SystemFunction.ConverTObjectToArray<ProgressParam>(progressParamFood, progressParamGold, progressParamHP, progressParamAttackProps, progressParamDefenseProps, progressParamSpecialProps);
			break;
			/*
            case BuildingType.ClanCastle://Army,HP
                progressBarArray = ConvertObjectToArray(m_UIUpgradeProgressBar[3], m_UIUpgradeProgressBar[2]);
                progressParamArray = ConvertParamToArray(progressParamArmy, progressParamHP);
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
                progressBarArray = ConvertObjectToArray(m_UIUpgradeProgressBar[4], m_UIUpgradeProgressBar[2]);
                progressParamArray = ConvertParamToArray(progressParamDps, progressParamHP);
                break;
            case BuildingType.Mortar://Dps,HP
                progressBarArray = ConvertObjectToArray(m_UIUpgradeProgressBar[4], m_UIUpgradeProgressBar[2]);
                progressParamArray = ConvertParamToArray(progressParamDps, progressParamHP);
                break;
             */
		case BuildingType.Tavern://HP
			progressBarArray = SystemFunction.ConverTObjectToArray<UIUpgradeProgressBar>(m_UIUpgradeProgressBar[2]);
			progressParamArray = SystemFunction.ConverTObjectToArray<ProgressParam>(progressParamHP);
			break;
		case BuildingType.Wall://HP
			progressBarArray = SystemFunction.ConverTObjectToArray<UIUpgradeProgressBar>(m_UIUpgradeProgressBar[2]);
			progressParamArray = SystemFunction.ConverTObjectToArray<ProgressParam>(progressParamHP);
			break;
		case BuildingType.PropsStorage://HP,Capacity
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
			//progressBarArray[i].SetUpgradeProgressBar2(progressParamArray[i].ProgressNext);
		}
		#endregion
	}
	void OnProduceArmy()
	{
		if (!this.m_IsBuildingLogicData)
			return;
		if (base.BuildingLogicData.BuildingIdentity.buildingType == BuildingType.ArmyCamp)
		{
            ProgressParam progressParamArmy = (SceneManager.Instance.SceneMode == SceneMode.SceneBuild) ? CalculateParam(LogicController.Instance.CurrentAvailableArmyCapacity, LogicController.Instance.CampsTotalCapacity) : CalculateParam(LogicController.Instance.CurrentFriend.AlreadyArmyCapacity, LogicController.Instance.CurrentFriend.TotalCampCapacity);
			m_UIUpgradeProgressBar[3].SetProgressBar(progressParamArmy.ProgressCurrent, progressParamArmy.Value);
		}
	}
	void OnProduceResource()
	{
		if (!this.m_IsBuildingLogicData)
			return;
		if (base.BuildingLogicData.BuildingIdentity.buildingType == BuildingType.Farm || base.BuildingLogicData.BuildingIdentity.buildingType == BuildingType.GoldMine)
		{
			BuildingConfigData buildingConfigDataCurrent = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(base.BuildingLogicData.BuildingIdentity.buildingType, base.BuildingLogicData.Level);
			UIUpgradeProgressBar[] progressBarArray = new UIUpgradeProgressBar[0];
			ProgressParam[] progressParamArray = new ProgressParam[0];
			ProgressParam progressParamFood = CalculateParam(base.BuildingLogicData.CurrentStoreFood, buildingConfigDataCurrent.StoreFoodCapacity);
			ProgressParam progressParamGold = CalculateParam(base.BuildingLogicData.CurrentStoreGold, buildingConfigDataCurrent.StoreGoldCapacity);
			switch (base.BuildingLogicData.BuildingIdentity.buildingType)
			{
			case BuildingType.Farm:
				progressBarArray = SystemFunction.ConverTObjectToArray<UIUpgradeProgressBar>(m_UIUpgradeProgressBar[0]);
				progressParamArray = SystemFunction.ConverTObjectToArray<ProgressParam>(progressParamFood);
				break;
			case BuildingType.GoldMine:
				progressBarArray = SystemFunction.ConverTObjectToArray<UIUpgradeProgressBar>(m_UIUpgradeProgressBar[1]);
				progressParamArray = SystemFunction.ConverTObjectToArray<ProgressParam>(progressParamGold);
				break;
			}
			for (int i = 0, count = progressBarArray.Length; i < count; i++)
			{
				progressBarArray[i].gameObject.SetActive(true);
				progressBarArray[i].transform.localPosition = INITIAL_LOCAL_POSITION + OFFSET_LOCAL_POSITION * i;
				progressBarArray[i].SetProgressBar(progressParamArray[i].ProgressCurrent, progressParamArray[i].Value);
			}
		}
	}
	//UIUpgradeProgressBar[] ConvertObjectToArray(params UIUpgradeProgressBar[] param)
	//{
	//    return param;
	//}
	//ProgressParam[] ConvertParamToArray(params ProgressParam[] param)
	//{
	//    return param;
	//}
	ProgressParam CalculateParam(float currentValue, float maxValue)
	{
		ProgressParam progressParam = new ProgressParam()
		{
			ProgressCurrent = maxValue == 0 ? 0 : (float)currentValue / maxValue,
			Value = maxValue == 0 ? "0" : Mathf.RoundToInt(currentValue) + "/" + Mathf.RoundToInt(maxValue)
		};
		return progressParam;
	}
	public override void SetWindowItemAchievementBuilding()
	{
		this.m_IsBuildingLogicData = false;
		m_UILabelTile.text = base.AchievementBuildingLogicData.Name;
		GameObject building = Resources.Load(ClientStringConstants.BUILDING_SCENE_RESOURCE_PREFAB_PREFIX_NAME + ClientStringConstants.ACHIEVEMENT_BULIDING_PREFAB_PREFIX_NAME + base.AchievementBuildingLogicData.PrefabName, typeof(GameObject)) as GameObject;
		tk2dSprite tk2dSpriteBuilding = building.transform.FindChild(ClientSystemConstants.BACKGROUND_NAME).GetComponent<tk2dSprite>();
		m_Tk2dSpriteIcon.SetSprite(tk2dSpriteBuilding.Collection, tk2dSpriteBuilding.spriteId);
		for (int i = 0, count = m_UIUpgradeProgressBar.Length; i < count; i++)
		{
			m_UIUpgradeProgressBar[i].gameObject.SetActive(false);
		}
		UIUpgradeProgressBar[] progressBarArray = new UIUpgradeProgressBar[0];
		ProgressParam[] progressParamArray = new ProgressParam[0];
		ProgressParam progressParamHP = CalculateParam(base.AchievementBuildingLogicData.MaxHP, base.AchievementBuildingLogicData.MaxHP);
		ProgressParam progressParamLive = CalculateParam(base.AchievementBuildingLogicData.Life, base.AchievementBuildingLogicData.MaxLife);
		//HP ,LIVE
		progressBarArray = SystemFunction.ConverTObjectToArray<UIUpgradeProgressBar>(m_UIUpgradeProgressBar[2], m_UIUpgradeProgressBar[12]);
		progressParamArray = SystemFunction.ConverTObjectToArray<ProgressParam>(progressParamHP, progressParamLive);
		for (int i = 0, count = progressBarArray.Length; i < count; i++)
		{
			progressBarArray[i].gameObject.SetActive(true);
			progressBarArray[i].transform.localPosition = INITIAL_LOCAL_POSITION + OFFSET_LOCAL_POSITION * i;
			progressBarArray[i].SetProgressBar(progressParamArray[i].ProgressCurrent, progressParamArray[i].Value); 
		}
	}
}
 