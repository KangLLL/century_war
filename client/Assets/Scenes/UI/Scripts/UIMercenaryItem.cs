using UnityEngine;
using System.Collections;
using ConfigUtilities;
using ConfigUtilities.Enums;
using System.Collections.Generic;

public class UIMercenaryItem : MonoBehaviour {
    [SerializeField] UILabel[] m_TextCost; //0=gold;1=food;2=oil;3=gem ;
    [SerializeField] UILabel[] m_TextValue; //0=Produce count;
    [SerializeField] UISprite m_UISprite;// cd mask;
    public MercenaryProductLogicData MercenaryProductLogicData { get; set; }
    public BuildingLogicData BuildingLogicData { get; set; }
    public MercenaryType MercenaryType { get; set; } 
    public bool IsForbid { get; set; }
    public bool EnableCost { get; set; }
    public bool IsWait { get; set; }
    Vector3 m_IniLocalPosition = new Vector3(42, -48, -4);
    Vector3 m_OffsetLocalPosition = new Vector3(0, 35, 0);
 
	
	// Update is called once per frame
	void Update () 
    {
        this.WaitCheck();
        this.SetProduceCD();
        this.SetItemValue();
	}
    public void SetItemData()
    { 
        this.SetCostItemData();
        this.SetItemValue();
        
         
    }
    bool ForbidCheck()//capacity check 
    {
        return this.IsForbid = LogicController.Instance.CurrentAvailableArmyCapacity + this.MercenaryProductLogicData.CapacityCost > LogicController.Instance.CampsTotalCapacity;
    }
    bool WaitCheck()
    {
        return this.IsWait = this.MercenaryProductLogicData.ReadyNumber == 0;
    }
    void SetCostItemData()
    {
        MercenaryConfigData mercenaryConfigData = ConfigInterface.Instance.MercenaryConfigHelper.GetMercenaryData(this.MercenaryType);
        int[] costValue = SystemFunction.ConverTObjectToArray<int>(mercenaryConfigData.HireCostGold, mercenaryConfigData.HireCostFood, mercenaryConfigData.HireCostOil, mercenaryConfigData.HireCostGem);
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
        m_TextValue[0].text = this.MercenaryProductLogicData.ReadyNumber.ToString();
    }
    void SetProduceCD()
    {
        if (this.IsWait)
        {
            this.m_UISprite.alpha = 1;
            float percentage  = 0;
            if (this.MercenaryProductLogicData.RemainingTime.HasValue)
            {
                percentage = this.MercenaryProductLogicData.RemainingTime.Value / MercenaryProductLogicData.ProduceTime;
            }
            m_UISprite.fillAmount = percentage;
        }
        else
            this.m_UISprite.alpha = 0;
    }
    public void OnClickContinuous()
    {
        if (UIManager.Instance.UIWindowBuyMercenary.ControlerFocus != null)
            return;
        this.ForbidCheck();
        this.WaitCheck();
        AudioController.Play("ButtonClick");


        if (this.IsForbid)
        {
            UIErrorMessage.Instance.ErrorMessage(1);
            return;
        }
        if (this.IsWait)
        {
            UIErrorMessage.Instance.ErrorMessage(22);
            return;
        }
        if (this.EnableCost)
        {
            //this.SetItemData();
            this.HireMercenaryData();
        }
        else
        {
            UIManager.Instance.UIWindowBuyMercenary.HideWindow();
            this.BuyMercenary();
        }
    }
    void BuyMercenary()
    {
        MercenaryConfigData mercenaryConfigData = ConfigInterface.Instance.MercenaryConfigHelper.GetMercenaryData(this.MercenaryType);
        Dictionary<CostType, int> costBalance = SystemFunction.ProduceCostBalance(mercenaryConfigData);
        Dictionary<CostType, int> costTotal = SystemFunction.ProduceTotalCost(mercenaryConfigData);
        int costGem = mercenaryConfigData.HireCostGem;
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
                        this.HireMercenaryData();
                    }
                }
                else
                    UIErrorMessage.Instance.ErrorMessage(16);
            };
        }
        else
        {
            print("买雇佣兵");
            this.HireMercenaryData();
        }
    }
    void HireMercenaryData()
    {
        LogicController.Instance.HireMercenary(this.BuildingLogicData.BuildingIdentity, this.MercenaryType);
        UIManager.Instance.UIWindowBuyMercenary.SetAllMercenaryItem();
    }
    //button message
    void ShowMercenaryInformationWindow()
    {
        if (UIManager.Instance.UIWindowBuyMercenary.ControlerFocus != null)
            return;
        UIManager.Instance.UIWindowBuyMercenary.HideWindow();
        UIManager.Instance.UIWindowMercenaryInfo.MercenaryType = this.MercenaryType;
        UIManager.Instance.UIWindowMercenaryInfo.ShowWindow();
    }
}
