using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;
using CommonUtilities;
using System;

public class UIItemBuilder : MonoBehaviour {
    [SerializeField] GameObject[] m_BuilderButton;//0=idle,1=busy,2=disable
    [SerializeField] UILabel[] m_UILabelTextIdle;//0 = textLevel /*textTitle */, 1 = textTime
    [SerializeField] UILabel[] m_UILabelTextBusy;//0 = textTime, 1 = textRemainingTime, 2 = textLevel
    [SerializeField] UIWindowCommon m_UIWindowCommon;
    //public BuilderInformation BuilderInformation { get; set; }
    public BuilderData BuilderData { get; set; }
    public BuildingLogicData BuildingLogicData { get; set; }
    public BuildingConfigData BuildingConfigData { get; set; }
    public RemovableObjectLogicData RemovableObjectLogicData { get; set; }
    public BuilderState BuilderState { get; set; }
    public BuilderMenuType BuilderMenuType { get; set; }
    public BuildingBehavior BuildingBehavior { get; set; }
    public int BuilderNo { get; set; }
	// Use this for initialization 
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        OnSetItemDataBusy();
	}
 
    public void SetItemDataIdle()
    {
        m_BuilderButton[0].gameObject.SetActive(true);
        m_BuilderButton[1].gameObject.SetActive(false);
        m_BuilderButton[2].gameObject.SetActive(false);
        if (this.BuilderMenuType != global::BuilderMenuType.RemoveObject) 
            m_UILabelTextIdle[1].text = SystemFunction.TimeSpanToString(Mathf.CeilToInt(this.BuildingConfigData.UpgradeWorkload / (float)this.BuilderData.Efficiency));
        else 
            m_UILabelTextIdle[1].text = SystemFunction.TimeSpanToString(Mathf.CeilToInt(this.RemovableObjectLogicData.RemoveWorkload /*/ (float)this.BuilderData.Efficiency*/));
        m_UILabelTextIdle[0].text = StringConstants.PROMPT_LEVEL + LogicController.Instance.GetBuildingObject(this.BuilderData.BuilderID).Level.ToString();
    }
    public void SetItemDataBusy()
    {
        m_BuilderButton[0].gameObject.SetActive(false);
        m_BuilderButton[1].gameObject.SetActive(true);
        m_BuilderButton[2].gameObject.SetActive(false);
        //m_UILabelTextBusy[0].text = SystemFunction.TimeSpanToString(Mathf.CeilToInt(this.BuilderData.TotalWorkload / this.BuilderData.Efficiency));
        if (this.BuilderMenuType != global::BuilderMenuType.RemoveObject)
            m_UILabelTextBusy[0].text = SystemFunction.TimeSpanToString(Mathf.CeilToInt(this.BuildingConfigData.UpgradeWorkload / (float)this.BuilderData.Efficiency));
        else
            m_UILabelTextBusy[0].text = SystemFunction.TimeSpanToString(Mathf.CeilToInt(this.RemovableObjectLogicData.RemoveWorkload/* / (float)this.BuilderData.Efficiency*/));
        //print("this.BuilderData.RemainingWorkload  ===" + this.BuilderData.RemainingWorkload.ToString());
        //print("Mathf.CeilToInt(this.BuilderData.RemainingWorkload)  ===" + Mathf.CeilToInt(this.BuilderData.RemainingWorkload).ToString());
        m_UILabelTextBusy[1].text = SystemFunction.TimeSpanToString(Mathf.CeilToInt(this.BuilderData.RemainingWorkload /*/ this.BuilderData.Efficiency*/));
        m_UILabelTextBusy[2].text = StringConstants.PROMPT_LEVEL + LogicController.Instance.GetBuildingObject(this.BuilderData.BuilderID).Level.ToString();
    } 
    void OnSetItemDataBusy()
    {
        if (this.BuilderData != null)
        {
            if (this.BuilderData.CurrentWorkTarget != null)
            {
                if (this.BuilderData.CurrentWorkTarget is IRemovableObjectInfo/* ||  ((BuildingLogicData)this.BuilderData.CurrentWorkTarget).CurrentBuilidngState == BuildingEditorState.Update*/)
                    m_UILabelTextBusy[1].text = SystemFunction.TimeSpanToString(Mathf.CeilToInt(this.BuilderData.RemainingWorkload /*/ this.BuilderData.Efficiency*/));
                else
                    if (this.BuilderData.CurrentWorkTarget is IBuildingInfo)
                        m_UILabelTextBusy[1].text = SystemFunction.TimeSpanToString(Mathf.CeilToInt(this.BuilderData.RemainingWorkload / this.BuilderData.Efficiency));
                    
            }
            else
            {
                if (this.BuilderState == global::BuilderState.Busy)
                {
                    this.BuilderState = global::BuilderState.Idle;
                    this.SetItemDataIdle();
                }
            }
        }
    }
    public void SetItemDataDisable()
    {
        m_BuilderButton[0].gameObject.SetActive(false);
        m_BuilderButton[1].gameObject.SetActive(false);
        m_BuilderButton[2].gameObject.SetActive(true);
    }
   
    public void OnClick()
    {
        if (!this.enabled)
            return;
        if (m_UIWindowCommon.ControlerFocus != null   /* || m_UIWindowCommon.ControlerFocus != this.gameObject*/)
            return;
        else
        {
            if (this.BuilderData == null || !(this.BuilderData.CurrentWorkTarget is RemovableObjectLogicData))
                m_UIWindowCommon.ControlerFocus = this.gameObject;
        }
        //m_UIWindowCommon.HideWindow();
        switch (this.BuilderState)
        {
            case BuilderState.Idle:
                switch (this.BuilderMenuType)
                {
                    case BuilderMenuType.Construct:
                        this.ConstructBuilding();
                        break;
                    case BuilderMenuType.Upgrade:
                        this.UpgradeBuilding();
                        break;
                    case BuilderMenuType.RemoveObject:
                        this.RemoveObject();
                        break;
                }
                break;
            case BuilderState.Busy:
                ImmediatelyUpgrade();
                break;
            case BuilderState.Disable:
               ShowBuyBuilderHutWindow();
                print("����δ����");
               break;
        }
    }
    void UpgradeBuilding()
    {
        this.OnMissionEvent(SystemFunction.UpgradeCostBalance(this.BuildingLogicData), SystemFunction.UpgradeTotalCost(this.BuildingLogicData), this.BuildingLogicData.UpgradeGem, this.UpgradeBuildingDelegate);
    }
    void ConstructBuilding()
    {
        this.OnMissionEvent(SystemFunction.UpgradeCosBalance(this.BuildingConfigData), SystemFunction.UpgradeTotalCost(this.BuildingConfigData), this.BuildingConfigData.UpgradeGem, this.ConstructBuildingDelegate);
    }
    void RemoveObject()
    {
        this.OnMissionEvent(SystemFunction.UpgradeCostBalance(this.RemovableObjectLogicData), SystemFunction.UpgradeTotalCost(this.RemovableObjectLogicData), this.RemovableObjectLogicData.GemCost, this.RemoveObjectDelegate);
    }
    void RemoveObjectDelegate()
    {
        AudioController.Play("BuildingConstruct");
        LogicController.Instance.Remove(this.RemovableObjectLogicData.RemovableObjectNo, this.BuilderNo);
    }
    void ConstructBuildingDelegate()
    {
         this.BuildingBehavior.BuildingCommon.BuyBuilding(this.BuilderNo);
    }
    void UpgradeBuildingDelegate()
    {
        if (this.BuildingLogicData.BuildingType != BuildingType.Wall)
        {
            AudioController.Play("BuildingConstruct");
            LogicController.Instance.UpgradeBuilding(this.BuildingLogicData.BuildingIdentity, this.BuilderNo);
        }
        else
            this.BuildingBehavior.BuildingCommon.OnUpgradeBuilding();
    }
    void OnMissionEvent(Dictionary<CostType, int> costBalance, Dictionary<CostType, int> costTotal, int costGem, Action function)
    {
        if (costBalance.Count > 0)
        {
            int costBalanceGem = SystemFunction.ResourceConvertToGem(costBalance);
            //int costResourceToGem = SystemFunction.ResourceConvertToGem(costTotal) - costGem;

            string resourceContext = SystemFunction.BuyReusoureContext(costBalance);
            m_UIWindowCommon.HideWindow();
            UIManager.Instance.UIWindowCostPrompt.ShowWindow(costBalanceGem, resourceContext, SystemFunction.BuyReusoureTitle(costBalance));
            UIManager.Instance.UIWindowCostPrompt.UnRegistDelegate();
            UIManager.Instance.UIWindowCostPrompt.Click += () =>
            {
                if (SystemFunction.CostUplimitCheck(costTotal[CostType.Gold], costTotal[CostType.Food], costTotal[CostType.Oil]))
                {
                    if (LogicController.Instance.PlayerData.CurrentStoreGem - costBalanceGem/*costResourceToGem*/ < costGem)
                    {
                        print("宝石不足，去商店");
                        //print("LogicController.Instance.PlayerData.CurrentStoreGem = " + LogicController.Instance.PlayerData.CurrentStoreGem);
                        //print("costResourceToGem = " + costResourceToGem);
                        //print("costGem = " + costGem);
                        //print("costBalanceGem =" + costBalanceGem);
                        //print("costTotal[gold] = " + costTotal[CostType.Gold]);
                        //print("costTotal[food] = " + costTotal[CostType.Food]);
                        //print("costTotal[oil] = " + costTotal[CostType.Oil]);
                        //print("costTotal[gem] = " + costTotal[CostType.Gem]);
                        UIManager.Instance.UIWindowFocus = null;
                        //UIManager.Instance.UIButtonShopping.GoShopping();
                        UIManager.Instance.UISelectShopMenu.GoShopping();
                    }
                    else
                    {
                        print("宝石换资源!");
                        SystemFunction.BuyResources(costBalance);
                        function.Invoke();
                    }
                }
                else
                    UIErrorMessage.Instance.ErrorMessage(16);
            };
        }
        else
        {
            function.Invoke();
            m_UIWindowCommon.HideWindow();
        }
    }
    void ShowBuyBuilderHutWindow()
    {
        //m_UIWindowCommon.HideWindow();
        m_UIWindowCommon.HideWindowImmediately();
            switch (this.BuilderMenuType)
            {
                case global::BuilderMenuType.Construct:
                    Destroy(this.BuildingBehavior.gameObject); 
                    break;
                case global::BuilderMenuType.Upgrade:
                    break;
                case global::BuilderMenuType.RemoveObject:
                    break;
            }
            SceneManager.Instance.DestroyBuildingBorder();
            UIManager.Instance.UIWindowBuyBuilding.BuilerNo = this.BuilderNo;
            UIManager.Instance.UIWindowBuyBuilding.ShowWindow(UIMenuType.Composite, BuildingType.BuilderHut);
    }
    void ImmediatelyUpgrade()
    {
        if (this.BuilderData.CurrentWorkTarget is IBuildingInfo)
        { 
            int costGem = MarketCalculator.GetUpdateTimeCost(Mathf.CeilToInt((float)this.BuilderData.RemainingWorkload / this.BuilderData.Efficiency));
            string name = this.BuilderData.CurrentWorkTarget is IBuildingInfo ? ((BuildingLogicData)this.BuilderData.CurrentWorkTarget).Name :
                ((RemovableObjectLogicData)this.BuilderData.CurrentWorkTarget).Name;
            string costContext = string.Format(StringConstants.PROMPT_GEM_COST, costGem, StringConstants.COIN_GEM, name, StringConstants.PROMPT_UPGRADE + StringConstants.QUESTION_MARK);
            m_UIWindowCommon.HideWindow();
            UIManager.Instance.UIWindowCostPrompt.ShowWindow(costGem, costContext,StringConstants.PROMPT_FINISH_INSTANTLY);
            UIManager.Instance.UIWindowCostPrompt.UnRegistDelegate();
            UIManager.Instance.UIWindowCostPrompt.Click += () =>
            {
                if (LogicController.Instance.PlayerData.CurrentStoreGem < costGem)
                {
                    print("No gem");
                    UIManager.Instance.UIWindowFocus = null;
                    //UIManager.Instance.UIButtonShopping.GoShopping();
                    UIManager.Instance.UISelectShopMenu.GoShopping();
                }
                else
                {
                    if (this.BuilderData.CurrentWorkTarget != null && this.BuilderData.CurrentWorkTarget is BuildingLogicData)
                    {
                        BuildingLogicData buildingLogicData = (BuildingLogicData)this.BuilderData.CurrentWorkTarget;
                        GameObject building = SceneManager.Instance.GetBuildingObjectFromBuildingObstacleMap(buildingLogicData.BuildingPosition.Row, buildingLogicData.BuildingPosition.Column);
                        this.CreateUpgradeFX(buildingLogicData);
                        LogicController.Instance.FinishBuildingUpgradeInstantly(buildingLogicData.BuildingIdentity);
                        SceneManager.Instance.ConstructBuilding(buildingLogicData, true); 
                        
                        Destroy(building);
                    }
                    switch (this.BuilderMenuType)
                    {
                        case global::BuilderMenuType.Construct:
                            this.ConstructBuilding();
                            break;
                        case global::BuilderMenuType.Upgrade:
                            this.UpgradeBuilding();
                            break;
                        case global::BuilderMenuType.RemoveObject:
                            this.RemoveObject();
                            break;
                    }
                }
            };
        }
        else
        {
            UIErrorMessage.Instance.ErrorMessage(7);
            print("Builder is busy");
        }
    }
    void CreateUpgradeFX(BuildingLogicData buildingLogicData)
    {
        BuildingBehavior buildingBehavior = SceneManager.Instance.GetBuildingBehaviorFromObstacleMap(buildingLogicData.BuildingPosition.Row, buildingLogicData.BuildingPosition.Column);
        if (buildingBehavior != null)
        {
            SceneManager.Instance.CreateUpgradeFX(buildingBehavior);
        }
    }
}
public enum BuilderState
{
    Idle,
    Busy,
    Disable
}
public enum CostType
{
    Gold,
    Food,
    Oil,
    Gem
}
public enum BuilderMenuType
{
    Construct,
    Upgrade,
    RemoveObject
}
