using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;

public class UIArmyItemInfomation : MonoBehaviour
{ 
    [SerializeField] ArmyType m_ArmyType;
    [SerializeField] UILabel[] m_TextCost; //0=gold;1=food;2=oil;3=gem ;
    [SerializeField] UILabel[] m_TextValue; //0=Produce count; 1 = Level
    [SerializeField] UISprite[] m_UISprite;//0=Lock;1=Forbid ;LevelIcon

    [SerializeField] UIWindowBuyArmy m_UIWindowBuyArmy;

    public BuildingLogicData BuildingLogicData { get; set; }
    public bool IsLock { get; set; }
    public bool IsForbid { get; set; }
    public bool EnableCost { get; set; }
    Vector3 m_IniLocalPosition = new Vector3(42, -48, -4);
    Vector3 m_OffsetLocalPosition = new Vector3(0, 35, 0);
	// Use this for initialization
 
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {
        if (this.BuildingLogicData != null)
        {
            this.ForbidCheck();
            this.SetForbidIcon();
        }

    }
    public void SetItemData()
    {   
        this.LockCheck();
        this.ForbidCheck();
        this.ActiveIcon();
        if (!this.IsLock)
        {
            this.SetCostItemData();
            this.SetItemValue();
        }
    }
    void SetCostItemData()
    {
        ArmyConfigData armyConfigData = ConfigInterface.Instance.ArmyConfigHelper.GetArmyData(this.m_ArmyType,LogicController.Instance.PlayerData.GetArmyLevel(m_ArmyType) );
        int[] costValue = SystemFunction.ConverTObjectToArray<int>(armyConfigData.ProduceCostGold, armyConfigData.ProduceCostFood, armyConfigData.ProduceCostOil, armyConfigData.ProduceCostGem);
        int[] userHasValue = SystemFunction.ConverTObjectToArray<int>(LogicController.Instance.PlayerData.CurrentStoreGold, LogicController.Instance.PlayerData.CurrentStoreFood, LogicController.Instance.PlayerData.CurrentStoreOil, LogicController.Instance.PlayerData.CurrentStoreGem);
        bool condition = true;
        for (int i = 0, j = 0; i < m_TextCost.Length; i++)
        {
            if (costValue[i] > 0)
            {
                m_TextCost[i].transform.parent.gameObject.SetActive(true);
                m_TextCost[i].text = costValue[i].ToString();
                m_TextCost[i].color = costValue[i] <= userHasValue[i] ? new Color(1, 1, 1, 1) : new Color(1, 0, 0, 1);
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
        this.EnableCost = condition;
    }
    void SetItemValue()
    {
        m_TextValue[0].text = "";
        m_TextValue[1].text = StringConstants.PROMPT_LEVEL + LogicController.Instance.PlayerData.GetArmyLevel(m_ArmyType).ToString();
        if (this.BuildingLogicData.ArmyProducts != null && this.BuildingLogicData.ArmyProducts.Count >0)
        {
            for (int i = 0, count = this.BuildingLogicData.ArmyProducts.Count; i < count; i++)
            {
                if (this.BuildingLogicData.ArmyProducts[i].Key == this.m_ArmyType)
                {
                    m_TextValue[0].text = "X" + this.BuildingLogicData.ArmyProducts[i].Value.Count;
                }
            }
        }

        
    }
    bool LockCheck()
    { 
        ArmyConfigData armyConfigData = ConfigInterface.Instance.ArmyConfigHelper.GetArmyData(this.m_ArmyType, LogicController.Instance.GetArmyLevel(this.m_ArmyType));
        return this.IsLock = armyConfigData.RequireProduceBuildingLevel > this.BuildingLogicData.Level;
        //SetLockIcon();
    }
    bool ForbidCheck()//capacity check
    {
        int populationCount = ConfigInterface.Instance.ArmyConfigHelper.GetArmyCapacityCost(this.m_ArmyType);
        return this.IsForbid = this.BuildingLogicData.AlreadyProduceArmyCapacity + populationCount > this.BuildingLogicData.ArmyProduceCapacity; 
        //SetForbidIcon();
       
    }
    
    void SetLockIcon()
    {
        m_UISprite[0].alpha = this.IsLock ? 1 : 0;
    }
    void SetForbidIcon()
    {
        m_UISprite[1].alpha = this.IsLock ? 0 : this.IsForbid ? 1 : 0;
    }
    void ActiveIcon()
    {
        //0=Lock;1=Forbid ;LevelIcon
        m_UISprite[0].alpha = this.IsLock ? 1 : 0;
        m_UISprite[1].alpha = this.IsLock ? 0 : this.IsForbid ? 1 : 0;
        //m_UISprite[2].alpha = this.IsLock ? 0 : 1;
        m_TextValue[0].alpha = this.IsLock ? 0 : 1;
        m_TextValue[1].alpha = this.IsLock ? 0 : 1;
        for (int i = 0; i < m_TextCost.Length; i++)
        {
            m_TextCost[i].transform.parent.gameObject.SetActive(!this.IsLock && !this.IsForbid);
        }
    }

    public void OnClickContinuous()
    {
        AudioController.Play("ButtonClick");
        this.SetCostItemData();
        this.ForbidCheck();
        if (UIManager.Instance.UIWindowBuyArmy.ControlerFocus != null)
            return;
        if (!this.NewbieGuideCondition())
            return;
        if (this.IsLock)
        {
            ArmyConfigData armyConfigData = ConfigInterface.Instance.ArmyConfigHelper.GetArmyData(this.m_ArmyType, LogicController.Instance.GetArmyLevel(this.m_ArmyType));
            UIErrorMessage.Instance.ErrorMessage(15, armyConfigData.Name, ClientSystemConstants.BUILDING_NAME_DICTIONARY[BuildingType.Barracks], armyConfigData.RequireProduceBuildingLevel.ToString());
            return;
        }
        if (!this.IsLock && !IsForbid && this.EnableCost)
        { 
            m_UIWindowBuyArmy.BuyArmy(m_ArmyType);
            // this.SetItemData();
            return;
        }

        if (!this.EnableCost && !this.IsForbid)
        {
            m_UIWindowBuyArmy.HideWindow();
            this.BuyArmy();
        }
    }
    bool NewbieGuideCondition()
    {
        if (LogicController.Instance.PlayerData.IsNewbie)
        {
            if (LogicController.Instance.TotalArmyCapacity < LogicController.Instance.CampsTotalCapacity)
                return true;
            else
                return false;
        }
        return true;
    }
    void ShowArmyInformationWindow()
    {
        if (UIManager.Instance.UIWindowBuyArmy.ControlerFocus != null)
            return;
        m_UIWindowBuyArmy.HideWindow();
        UIManager.Instance.UIWindowArmyInformation.ArmyType = this.m_ArmyType;
        UIManager.Instance.UIWindowArmyInformation.ShowWindow();
    }

    void BuyArmy()
    {
        ArmyConfigData armyConfigData = ConfigInterface.Instance.ArmyConfigHelper.GetArmyData(this.m_ArmyType, LogicController.Instance.PlayerData.GetArmyLevel(this.m_ArmyType));
        Dictionary<CostType, int> costBalance = SystemFunction.ProduceCostBalance(armyConfigData);
        Dictionary<CostType, int> costTotal = SystemFunction.ProduceTotalCost(armyConfigData);
        int costGem = armyConfigData.ProduceCostGem; 
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
                        UIManager.Instance.UIWindowFocus = null;
                        //UIManager.Instance.UIButtonShopping.GoShopping();
                        UIManager.Instance.UISelectShopMenu.GoShopping();
                        print("宝石不足，去商店");
                    }
                    else
                    {
                        print("宝石换资源!");
                        SystemFunction.BuyResources(costBalance);
                        //有资源后继续买兵
                        m_UIWindowBuyArmy.BuyArmy(m_ArmyType);
                    }
                }
                else
                    UIErrorMessage.Instance.ErrorMessage(16);
            };
        }
        else
        {
            print("买士兵");
            m_UIWindowBuyArmy.BuyArmy(m_ArmyType);
        }
    }
}
