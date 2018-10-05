using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
using ConfigUtilities;
using System.Collections.Generic;

public class UIWindowUpgradeArmyInfo : UIWindowCommon
{
    [SerializeField] GameObject m_ArmyIconRef;
    //[SerializeField] GameObject[] m_ArmyPrefab;//Army Icon
    [SerializeField] PrefabDictionary m_ArmyTypeDict;
    [SerializeField] UILabel[] m_TextCost; //0=gold;1=food;2=oil;3=gem ;
    [SerializeField] UISprite[] m_UISpriteCostColor;//0=gold;1=food;2=oil;3=gem ;
    [SerializeField] UISprite[] m_UISpriteCostGray;//0=gold;1=food;2=oil;3=gem ;
    [SerializeField] UISprite[] m_UISpriteBtnBkCost;//0 = bk;1 = bk gray  
    [SerializeField] UILabel[] m_UILabel;//Favorite,DamageType,Target,HouseSpace,TrainTime,MoveVelocity,UpgradeTime
    [SerializeField] UILabel m_UILabelTitle;// Name+Level
    [SerializeField] UIUpgradeProgressBar[] m_UIUpgradeProgressBar;//DPS,HP,Food
    public bool IsForbid { get; set; }
    public bool EnableCost { get; set; }
    Vector3 m_IniLocalPosition = new Vector3(42, 0, -4);
    Vector3 m_OffsetLocalPosition = new Vector3(0, 35, 0);
    public ArmyType ArmyType { get; set; }
	// Use this for initialization
    void Awake()
    {
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
        int levelNext = armyLevel + armyConfigDataCurrent.UpgradeStep; //(armyLevel - armyConfigDataCurrent.InitialLevel) / armyConfigDataCurrent.UpgradeStep * armyConfigDataCurrent.UpgradeStep + armyConfigDataCurrent.InitialLevel + armyConfigDataCurrent.UpgradeStep;
        int armyLevelMax = armyConfigDataCurrent.MaxLevel;  
        int armyLevelNext = levelNext > armyLevelMax ? armyLevel : levelNext;
        ArmyConfigData armyConfigDataNext = ConfigInterface.Instance.ArmyConfigHelper.GetArmyData(this.ArmyType, armyLevelNext);
        ArmyConfigData armyConfigDataMax = ConfigInterface.Instance.ArmyConfigHelper.GetArmyData(this.ArmyType, armyLevelMax);
        //Favorite,DamageType,Target,HouseSpace,TrainTime,MoveVelocity
        this.m_UILabel[0].text = ClientSystemConstants.BUILDINGCATEGORY_DICTIONARY[(BuildingCategory)armyConfigDataCurrent.DisplayFavoriteType];
        this.m_UILabel[1].text = ClientSystemConstants.ATTACKTYPE_DICTIONARY[(AttackType)armyConfigDataCurrent.AttackType];//"配置表里没有";//armyConfigData. DamageType
        this.m_UILabel[2].text = ClientSystemConstants.TARGETTYPE_DICTIONARY[(TargetType)armyConfigDataCurrent.TargetType];
        this.m_UILabel[3].text = armyConfigDataCurrent.CapcityCost.ToString();//"配置表里没有";//armyConfigData. RoomSpace
        this.m_UILabel[4].text = SystemFunction.TimeSpanToString(Mathf.CeilToInt((float)armyConfigDataCurrent.ProduceWorkload / ConfigInterface.Instance.SystemConfig.ProduceArmyEfficiency));
        this.m_UILabel[5].text = armyConfigDataCurrent.MoveVelocity.ToString();
        this.m_UILabel[6].text = SystemFunction.TimeSpanToString(Mathf.CeilToInt((float)armyConfigDataCurrent.UpgradeWorkload / ConfigInterface.Instance.SystemConfig.UpgradeArmyEfficiency));

        //this.m_UILabelTitle[0].text = armyConfigDataCurrent.Name;
        //this.m_UILabelTitle[1].text = (armyLevel + 1).ToString();
        this.m_UILabelTitle.text = StringConstants.PROMPT_UPGRADE + armyConfigDataCurrent.Name + StringConstants.PROMPT_TO + StringConstants.LEFT_PARENTHESES + levelNext + StringConstants.RIGHT_PARENTHESES + StringConstants.PROMPT_LV + StringConstants.QUESTION_MARK;

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

        ProgressParam progressParamDps = SystemFunction.CalculateParam((int)(armyConfigDataCurrent.AttackValue / armyConfigDataCurrent.AttackCD), (int)(armyConfigDataNext.AttackValue / armyConfigDataNext.AttackCD), armyConfigDataMax.AttackValue / armyConfigDataMax.AttackCD);
        ProgressParam progressParamFood = SystemFunction.CalculateParam(armyConfigDataCurrent.ProduceCostFood, armyConfigDataNext.ProduceCostFood, armyConfigDataMax.ProduceCostFood);
        ProgressParam progressParamHP = SystemFunction.CalculateParam(armyConfigDataCurrent.MaxHP, armyConfigDataNext.MaxHP, armyConfigDataMax.MaxHP);
 
        m_UIUpgradeProgressBar[0].SetProgressBar(progressParamDps.ProgressCurrent, progressParamDps.Value);
        m_UIUpgradeProgressBar[0].SetUpgradeProgressBar2(progressParamDps.ProgressNext); 
        m_UIUpgradeProgressBar[1].SetProgressBar(progressParamHP.ProgressCurrent, progressParamHP.Value);
        m_UIUpgradeProgressBar[1].SetUpgradeProgressBar2(progressParamHP.ProgressNext);
        m_UIUpgradeProgressBar[2].SetProgressBar(progressParamFood.ProgressCurrent, progressParamFood.Value);
        m_UIUpgradeProgressBar[2].SetUpgradeProgressBar2(progressParamFood.ProgressNext);

        CheckForbid();
        SetCostItemData();
        
    }
    void SetCostItemData()
    {
        ArmyConfigData armyConfigData = ConfigInterface.Instance.ArmyConfigHelper.GetArmyData(this.ArmyType, LogicController.Instance.PlayerData.GetArmyLevel(this.ArmyType));
        int[] costValue = SystemFunction.ConverTObjectToArray<int>(armyConfigData.UpgradeCostGold, armyConfigData.UpgradeCostFood, armyConfigData.UpgradeCostOil, armyConfigData.UpgradeCostGem);
        int[] userHasValue = SystemFunction.ConverTObjectToArray<int>(LogicController.Instance.PlayerData.CurrentStoreGold, LogicController.Instance.PlayerData.CurrentStoreFood, LogicController.Instance.PlayerData.CurrentStoreOil, LogicController.Instance.PlayerData.CurrentStoreGem);
        bool condition = true;
        for (int i = 0, j = 0; i < m_TextCost.Length; i++)
        {
            if (costValue[i] > 0)
            {
                m_TextCost[i].transform.parent.gameObject.SetActive(true);
                m_TextCost[i].text = costValue[i].ToString();
                m_TextCost[i].color = costValue[i] <= userHasValue[i] ? new Color(1, 1, 1, 1) : new Color(1, 0, 0, 1);
                m_UISpriteCostColor[i].alpha = this.IsForbid ? 0 : 1;
                m_UISpriteCostGray[i].alpha = this.IsForbid ? 1 : 0;
                m_TextCost[i].transform.parent.localPosition = m_IniLocalPosition + j * m_OffsetLocalPosition;
                j++;
                if (userHasValue[i] < costValue[i])
                    condition = false;
            }
            else
            {
                m_TextCost[i].transform.parent.gameObject.SetActive(false);
            }
        } 
        m_UISpriteBtnBkCost[0].alpha = this.IsForbid ? 0 : 1;
        m_UISpriteBtnBkCost[1].alpha = this.IsForbid ? 1 : 0;
        this.EnableCost = condition;
    }
    void CheckForbid()
    {
        bool isCurrentUpgradingArmy = false;
        for (int i = 0; i < LogicController.Instance.CurrentUpgradingArmies.Length; i++)
        {
            if (LogicController.Instance.CurrentUpgradingArmies[i] == this.ArmyType)
            {
                isCurrentUpgradingArmy = true;
                break;
            }
        }

        this.IsForbid = base.BuildingLogicData.ArmyUpgrade.HasValue || isCurrentUpgradingArmy;
        
    }
    void UpgradeArmy()
    { 
        ArmyConfigData armyConfigData = ConfigInterface.Instance.ArmyConfigHelper.GetArmyData(this.ArmyType, LogicController.Instance.PlayerData.GetArmyLevel(this.ArmyType));
        Dictionary<CostType, int> costBalance = SystemFunction.UpgradeCostBalance(armyConfigData);
        Dictionary<CostType, int> costTotal = SystemFunction.UpgradeTotalCost(armyConfigData);
        int costGem = armyConfigData.UpgradeCostGem;
        if (costBalance.Count > 0)
        {
            int costBalanceGem = SystemFunction.ResourceConvertToGem(costBalance);
            //int costResourceToGem = SystemFunction.ResourceConvertToGem(costTotal) - costGem;

            string resourceContext = SystemFunction.BuyReusoureContext(costBalance);
            UIManager.Instance.UIWindowCostPrompt.ShowWindow(costBalanceGem, resourceContext, SystemFunction.BuyReusoureTitle(costBalance));
            UIManager.Instance.UIWindowCostPrompt.UnRegistDelegate();
            UIManager.Instance.UIWindowCostPrompt.Click += () =>
            {
                if (SystemFunction.CostUplimitCheck(costTotal[CostType.Gold], costTotal[CostType.Food], costTotal[CostType.Oil]))
                {
                    if (LogicController.Instance.PlayerData.CurrentStoreGem - costBalanceGem/*costResourceToGem*/ < costGem)
                    {
                        print("宝石不足，去商店");
                        UIManager.Instance.UIWindowFocus = null;
                        //UIManager.Instance.UIButtonShopping.GoShopping();
                        UIManager.Instance.UISelectShopMenu.GoShopping();
                    }
                    else
                    {
                        print("宝石换资源!");
                        SystemFunction.BuyResources(costBalance);
                        //有资源后继续升级
                        LogicController.Instance.UpgradeArmy(this.ArmyType, this.BuildingLogicData.BuildingIdentity);
                        UIManager.Instance.UIWindowFocus = null;
                        UIManager.Instance.UIWindowUpgradeArmy.ShowWindow();
                        //UIManager.Instance.UIWindowCostPrompt.WindowEvent += () => UIManager.Instance.UIWindowUpgradeArmy.ShowWindow();
                    }
                }
                else
                    UIErrorMessage.Instance.ErrorMessage(16);
            };
        }
        else
        {
            print("升级士兵");
            LogicController.Instance.UpgradeArmy(this.ArmyType, this.BuildingLogicData.BuildingIdentity);
            UIManager.Instance.UIWindowUpgradeArmy.ShowWindow(); 
        }
    }
    void OnUpgradeArmy()
    {
        if (UIManager.Instance.UIWindowUpgradeArmyInfo.ControlerFocus != null)
            return;
        if (!this.IsForbid)
        { 
            this.HideWindow();
            this.UpgradeArmy();
        }
    }
}
