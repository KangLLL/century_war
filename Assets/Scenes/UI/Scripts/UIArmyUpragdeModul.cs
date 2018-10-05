using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
using CommandConsts;
using ConfigUtilities;
using CommonUtilities;
public class UIArmyUpragdeModul : UIWindowItemCommon
{
    [SerializeField] GameObject m_ArmyIconRef;
    //[SerializeField] GameObject[] m_ArmyPrefab;//Army Icon
    [SerializeField] PrefabDictionary m_ArmyTypeDict;
    [SerializeField] UILabel[] m_UILabel;//0 = name & level,1 = remaingTime; 2 = CostGem
    public bool EnableUprgade { get; set; }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        OnUprade();
	}

    void OnUprade()
    {
        if (!this.EnableUprgade)
            return;
        if (base.BuildingLogicData != null)
        {
            if (base.BuildingLogicData.ArmyUpgrade.HasValue)
            {
                //ArmyType armyType = base.BuildingLogicData.ArmyUpgrade.Value;
                int remainingTime = Mathf.CeilToInt((float)base.BuildingLogicData.ArmyUpgradeRemainWorkload / base.BuildingLogicData.ArmyUpgradeEfficiency);
                m_UILabel[1].text = SystemFunction.TimeSpanToString(remainingTime);
                int gemCost = MarketCalculator.GetUpdateTimeCost(remainingTime);
                m_UILabel[2].text = gemCost.ToString();
                m_UILabel[2].color = LogicController.Instance.PlayerData.CurrentStoreGem < gemCost ? Color.red : Color.white;
            }
        }
        this.ActiveComponent();
    }
    public void ActiveComponent()
    {
        if (base.BuildingLogicData.ArmyUpgrade.HasValue)
        {
            this.gameObject.SetActive(true); 
        }
        else
        {
            print("=====UnActiveComponent====");
            UIManager.Instance.UIWindowUpgradeArmy.SetWindowItemData();
            this.gameObject.SetActive(false);
        }
    }
    void OnImmediatelyArmyUpgrade()
    {
        //this.BuildingLogicObject.ArmyProducts
        if (UIManager.Instance.UIWindowUpgradeArmy.ControlerFocus != null)
            return;
        else
            UIManager.Instance.UIWindowUpgradeArmy.ControlerFocus = this.gameObject;
        if (base.BuildingLogicData.ArmyUpgrade.HasValue)
        {
            int remainingTime = this.BuildingLogicData.ArmyUpgradeRemainingTime;
            int gemCost = MarketCalculator.GetUpdateTimeCost(remainingTime);
            UIManager.Instance.UIWindowUpgradeArmy.HideWindow();
            UIManager.Instance.UIWindowCostPrompt.ShowWindow(gemCost, string.Format(StringConstants.PROMPT_GEM_COST, gemCost, StringConstants.COIN_GEM, StringConstants.PROMPT_ARMY_TYPE, StringConstants.PROMPT_UPGRADE), StringConstants.PROMPT_FINISH_INSTANTLY);
        
            UIManager.Instance.UIWindowCostPrompt.UnRegistDelegate();
            UIManager.Instance.UIWindowCostPrompt.UnRegistWindowEvent();
            UIManager.Instance.UIWindowCostPrompt.Click += () =>
            {
                if (LogicController.Instance.PlayerData.CurrentStoreGem < gemCost)
                {
                    UIManager.Instance.UIWindowFocus = null;
                    //UIManager.Instance.UIButtonShopping.GoShopping();
                    UIManager.Instance.UISelectShopMenu.GoShopping();
                    print("宝石不足，去商店");
                }
                else
                {
                    print("立即完成士兵升级!");
                    LogicController.Instance.FinishUpgradeArmyInstantly(this.BuildingLogicData.BuildingIdentity);
                    UIManager.Instance.UIWindowFocus = null;
                    //UIManager.Instance.UIWindowUpgradeArmy.ShowWindow();
                    UIManager.Instance.UIWindowCostPrompt.WindowCloseEvent += () => UIManager.Instance.UIWindowUpgradeArmy.ShowWindow();
                }
            };
        }
    }
    public override void SetWindowItem()
    {
        if (base.BuildingLogicData.ArmyUpgrade.HasValue)
        {
            ArmyType armyType = base.BuildingLogicData.ArmyUpgrade.Value;
            int level = LogicController.Instance.GetArmyLevel(armyType) + 1;
            m_UILabel[0].text = ConfigInterface.Instance.ArmyConfigHelper.GetArmyData(armyType, level).Name + StringConstants.LEFT_PARENTHESES + level + StringConstants.PROMPT_LV + StringConstants.RIGHT_PARENTHESES;

            while (this.m_ArmyIconRef.transform.childCount > 0)
            {
                Transform tran = this.m_ArmyIconRef.transform.GetChild(0);
                tran.parent = null;
                DestroyImmediate(tran.gameObject);
            }

            GameObject go = Instantiate(m_ArmyTypeDict[armyType.ToString()]) as GameObject;
            //GameObject go = Instantiate(m_ArmyPrefab[(int)armyType].gameObject) as GameObject;
            go.transform.parent = this.m_ArmyIconRef.transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = this.m_ArmyIconRef.transform.localScale;
        }
    }
   
}
