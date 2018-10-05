using UnityEngine;
using System.Collections;
using ConfigUtilities;
using ConfigUtilities.Enums;
using System.Collections.Generic;

public class UIWindowArmyInformation : UIWindowCommon {
    [SerializeField] GameObject m_ArmyIconRef;
    //[SerializeField] GameObject[] m_ArmyPrefab;//Army Icon
    [SerializeField] PrefabDictionary m_ArmyTypeDict;
    [SerializeField] UILabel[] m_UILabel;//Favorite,DamageType,Target,HouseSpace,TrainTime,MoveVelocity,UpgradeTime
    [SerializeField] UILabel[] m_UILabelTitle;// Name,Level
    [SerializeField] UIUpgradeProgressBar[] m_UIUpgradeProgressBar;//DPS,HP,Gold,Food,Oil,Gem 
    [SerializeField] Vector3 INITIAL_LOCAL_POSITION = new Vector3(-10, 200, -3);
    [SerializeField] Vector3 OFFSET_LOCAL_POSITION = new Vector3(0, -50, 0);
    public ArmyType ArmyType { get; set; }
	// Use this for initialization

	void Awake () {
        this.GetTweenComponent();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    protected override void GetTweenComponent()
    {
        base.GetTweenComponent();
    }
    public override void HideWindow()
    {
        base.HideWindow();
    }
    public override void ShowWindow()
    {
        SetWindowItemData();
        base.ShowWindow();
    }
    void SetWindowItemData()
    {
        int armyLevel = LogicController.Instance.GetArmyLevel(this.ArmyType);
        ArmyConfigData armyConfigDataCurrent = ConfigInterface.Instance.ArmyConfigHelper.GetArmyData(this.ArmyType, armyLevel);
        int armyLevelMax = armyConfigDataCurrent.MaxLevel;
        ArmyConfigData armyConfigDataMax = ConfigInterface.Instance.ArmyConfigHelper.GetArmyData(this.ArmyType, armyLevelMax);
        //Favorite,DamageType,Target,HouseSpace,TrainTime,MoveVelocity
        this.m_UILabel[0].text = ClientSystemConstants.BUILDINGCATEGORY_DICTIONARY[(BuildingCategory)armyConfigDataCurrent.DisplayFavoriteType];
        this.m_UILabel[1].text = ClientSystemConstants.ATTACKTYPE_DICTIONARY[(AttackType)armyConfigDataCurrent.AttackType];
        this.m_UILabel[2].text = ClientSystemConstants.TARGETTYPE_DICTIONARY[(TargetType)armyConfigDataCurrent.TargetType];
        this.m_UILabel[3].text = armyConfigDataCurrent.CapcityCost.ToString();
        this.m_UILabel[4].text = SystemFunction.TimeSpanToString(Mathf.CeilToInt((float)armyConfigDataCurrent.ProduceWorkload / ConfigInterface.Instance.SystemConfig.ProduceArmyEfficiency));
        this.m_UILabel[5].text = armyConfigDataCurrent.MoveVelocity.ToString();
        this.m_UILabel[6].text = armyConfigDataCurrent.Description;
        this.m_UILabelTitle[0].text = armyConfigDataCurrent.Name + StringConstants.LEFT_PARENTHESES + armyLevel + StringConstants.RIGHT_PARENTHESES + StringConstants.PROMPT_LV;

        while (this.m_ArmyIconRef.transform.childCount > 0)
        {
            Transform tran = this.m_ArmyIconRef.transform.GetChild(0);
            tran.parent = null;
            DestroyImmediate(tran.gameObject);
        }
        GameObject go = Instantiate(m_ArmyTypeDict[this.ArmyType.ToString()]) as GameObject;
        //GameObject go = Instantiate(m_ArmyPrefab[(int)this.ArmyType].gameObject) as GameObject;
        go.transform.parent = this.m_ArmyIconRef.transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = this.m_ArmyIconRef.transform.localScale;

        float currentDps = SystemFunction.Division(armyConfigDataCurrent.AttackValue, armyConfigDataCurrent.AttackCD);
        float maxDps = SystemFunction.Division(armyConfigDataMax.AttackValue, armyConfigDataMax.AttackCD);

        ProgressParam paramDps = new ProgressParam() { ProgressCurrent = currentDps, ProgressNext = maxDps };
        ProgressParam paramHp = new ProgressParam() { ProgressCurrent = SystemFunction.Division(currentDps, maxDps), ProgressNext = armyConfigDataCurrent.MaxHP };
        ProgressParam paramCostGold = new ProgressParam() { ProgressCurrent = SystemFunction.Division(armyConfigDataCurrent.ProduceCostGold, armyConfigDataMax.ProduceCostGold), ProgressNext = armyConfigDataCurrent.ProduceCostGold };
        ProgressParam paramCostFood = new ProgressParam() { ProgressCurrent = SystemFunction.Division(armyConfigDataCurrent.ProduceCostFood, armyConfigDataMax.ProduceCostFood), ProgressNext = armyConfigDataCurrent.ProduceCostFood };
        ProgressParam paramCostOil = new ProgressParam() { ProgressCurrent = SystemFunction.Division(armyConfigDataCurrent.ProduceCostOil, armyConfigDataMax.ProduceCostOil), ProgressNext = armyConfigDataCurrent.ProduceCostOil };
        ProgressParam paramCostGem = new ProgressParam() { ProgressCurrent = SystemFunction.Division(armyConfigDataCurrent.ProduceCostGem, armyConfigDataMax.ProduceCostGem), ProgressNext = armyConfigDataCurrent.ProduceCostGem };

        List<ProgressParam> progressParamArray = new List<ProgressParam>(SystemFunction.ConverTObjectToArray<ProgressParam>(paramDps, paramHp));
        List<UIUpgradeProgressBar> progressBarArray = new List<UIUpgradeProgressBar>(SystemFunction.ConverTObjectToArray<UIUpgradeProgressBar>(m_UIUpgradeProgressBar[0], m_UIUpgradeProgressBar[1]));
        if (armyConfigDataCurrent.ProduceCostGold > 0)
        {
            progressParamArray.Add(paramCostGold);
            progressBarArray.Add(m_UIUpgradeProgressBar[2]);
        }
        if (armyConfigDataCurrent.ProduceCostFood > 0)
        {
            progressParamArray.Add(paramCostFood);
            progressBarArray.Add(m_UIUpgradeProgressBar[3]);
        }
        if (armyConfigDataCurrent.ProduceCostOil> 0)
        {
            progressParamArray.Add(paramCostOil);
            progressBarArray.Add(m_UIUpgradeProgressBar[4]);
        }
        if (armyConfigDataCurrent.ProduceCostGem> 0)
        {
            progressParamArray.Add(paramCostGem);
            progressBarArray.Add(m_UIUpgradeProgressBar[5]);
        }
        for (int i = 0, count = m_UIUpgradeProgressBar.Length; i < count; i++)
            m_UIUpgradeProgressBar[i].gameObject.SetActive(false);
        for (int i = 0, count = progressBarArray.Count; i < count; i++)
        {
            progressBarArray[i].gameObject.SetActive(true);
            progressBarArray[i].transform.localPosition = INITIAL_LOCAL_POSITION + OFFSET_LOCAL_POSITION * i;
            progressBarArray[i].SetProgressBar(progressParamArray[i].ProgressCurrent, progressParamArray[i].ProgressNext);
        }
    }
}
