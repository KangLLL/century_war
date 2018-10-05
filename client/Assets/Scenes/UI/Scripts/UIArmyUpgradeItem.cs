using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;
public class UIArmyUpgradeItem : MonoBehaviour {
	// Use this for initialization
    [SerializeField] ArmyType m_ArmyType;
    [SerializeField] UILabel[] m_TextCost; //0=gold;1=food;2=oil;3=gem ;
    [SerializeField] UILabel[] m_TextValue; //0= Warning; 1 = Level
    [SerializeField] UISprite[] m_UISprite;//0=Lock;1=LevelIcon
    [SerializeField] UIWindowUpgradeArmy m_UIWindowUpgradeArmy;
    [SerializeField] TweenAlpha[] m_TweenAlpha;
    public bool IsLock { get; set; }
    public bool IsForbid { get; set; }
    public bool IsMaxLevel { get; set; }
    public bool EnableCost { get; set; }
    Vector3 m_IniLocalPosition = new Vector3(42, -48, -4);
    Vector3 m_OffsetLocalPosition = new Vector3(0, 35, 0);
    public BuildingLogicData BuildingLogicData { get; set; }
	void Start () 
    {

	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (this.BuildingLogicData != null)
            this.SetItemData();
        this.OnUpgradintArmyState();
	}
    public void SetItemData()
    {
        bool state = !this.MaxLevelCheck() && !this.LockCheck() && !this.ForbidCheck();
        this.ActiveIcon();
        if (state)
            this.SetCostItemData();
        this.SetItemValue();
        
    }
    void SetCostItemData()
    {
        ArmyConfigData armyConfigData = ConfigInterface.Instance.ArmyConfigHelper.GetArmyData(this.m_ArmyType, LogicController.Instance.PlayerData.GetArmyLevel(m_ArmyType));
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
        int armyLevel = LogicController.Instance.PlayerData.GetArmyLevel(m_ArmyType);
        //this.IsForbid = this.BuildingLogicObject.Level <= armyLevel;
        m_TextValue[0].text = this.IsForbid ? (string.Format(StringConstants.PROMPT_REQUIRE_BUILDING_LEVEL, armyLevel + 1, ClientSystemConstants.BUILDING_NAME_DICTIONARY[BuildingType.Barracks])).ToString() : string.Empty;
        m_TextValue[1].text = this.IsMaxLevel ? StringConstants.PROMPT_MAX_LEVEL : StringConstants.PROMPT_LEVEL + armyLevel.ToString();
    }
    bool ForbidCheck()
    {
        
        int armyLevel = LogicController.Instance.PlayerData.GetArmyLevel(m_ArmyType);
        ArmyConfigData armyConfigData = ConfigInterface.Instance.ArmyConfigHelper.GetArmyData(this.m_ArmyType, armyLevel);
        return this.IsForbid = armyLevel + armyConfigData.UpgradeStep > this.BuildingLogicData.Level ;
    }
    bool LockCheck()
    {
        ArmyConfigData armyConfigData = ConfigInterface.Instance.ArmyConfigHelper.GetArmyData(this.m_ArmyType, LogicController.Instance.GetArmyLevel(this.m_ArmyType));
        return this.IsLock = armyConfigData.RequireProduceBuildingLevel > this.BuildingLogicData.Level;
    
    }
    bool MaxLevelCheck()
    {
        int armyLevel = LogicController.Instance.PlayerData.GetArmyLevel(m_ArmyType);
        int maxLevel = ConfigInterface.Instance.ArmyConfigHelper.GetArmyMaxLevel(m_ArmyType);
        return this.IsMaxLevel = armyLevel >= maxLevel;
    }
    void ActiveIcon()
    {
        //m_TextValue; //0= Warning; 1 = Level
        //m_UISprite;//0=Lock;1=LevelIcon
        //m_UISprite[0].alpha = active ? 0 : 1;
        m_UISprite[0].alpha = this.IsLock ? 1 : 0;
        //m_UISprite[1].alpha = this.IsLock ? 0 : 1;
        m_TextValue[0].alpha = this.IsLock ? 0 : 1;
        m_TextValue[1].alpha = this.IsLock ? 0 : 1;
        for (int i = 0; i < m_TextCost.Length; i++)
        {
            m_TextCost[i].transform.parent.gameObject.SetActive(!this.IsLock && !this.IsMaxLevel && !this.IsForbid);
        }
    }
    void OnClick()
    {
        if (UIManager.Instance.UIWindowUpgradeArmy.ControlerFocus != null)
            return;
        if (!this.IsLock)
        {
            if (!this.IsForbid && !this.IsMaxLevel/*&& this.EnableCost*/)
            {
                m_UIWindowUpgradeArmy.HideWindow();
                UIManager.Instance.UIWindowUpgradeArmyInfo.BuildingLogicData = this.BuildingLogicData;
                UIManager.Instance.UIWindowUpgradeArmyInfo.ArmyType = this.m_ArmyType;
                UIManager.Instance.UIWindowUpgradeArmyInfo.ShowWindow();
            }
            else
            {
                if (!this.IsMaxLevel)
                {
                    int armyLevel = LogicController.Instance.GetArmyLevel(this.m_ArmyType);
                    ArmyConfigData armyConfigData = ConfigInterface.Instance.ArmyConfigHelper.GetArmyData(this.m_ArmyType, armyLevel);
                    UIErrorMessage.Instance.ErrorMessage(0, ClientSystemConstants.BUILDING_NAME_DICTIONARY[BuildingType.Barracks], (armyLevel + armyConfigData.UpgradeStep).ToString());
                }
            }
        }
        
    }
    bool IsUpgradingNow()
    {
        for (int i = 0; i < LogicController.Instance.CurrentUpgradingArmies.Length; i++)
        {
            if (LogicController.Instance.CurrentUpgradingArmies[i] == this.m_ArmyType)
                return true;
        }
        return false;
    }
    void OnUpgradintArmyState()
    {
        if (this.IsUpgradingNow())
            this.StartTween();
        else
            this.StopTween();
    }

    //void AddPanel()
    //{
    //    if (GetComponent<UIPanel>() == null)
    //    {
    //        UIPanel uiPanel = this.gameObject.AddComponent<UIPanel>();
    //        uiPanel.Refresh();
    //    }
    //}
    //void AddTweenAlpha()
    //{
    //    if (GetComponent<TweenAlpha>() == null)
    //    {
    //        TweenAlpha tweenAlpha = this.gameObject.AddComponent<TweenAlpha>();
    //        tweenAlpha.method = UITweener.Method.Linear;
    //        tweenAlpha.style = UITweener.Style.PingPong;
    //        tweenAlpha.delay = 0;
    //        tweenAlpha.duration = 1.0f;
    //        tweenAlpha.from = 1.0f;
    //        tweenAlpha.to = 0.5f;
           
    //    }
    //}
    //void AddComponents()
    //{
    //    this.AddPanel();
    //    this.AddTweenAlpha();
    //}
    //void RemovePanel()
    //{
    //    DestroyImmediate(this.gameObject.GetComponent<UIPanel>());
    //}
    //void RemoveTweenAlpha()
    //{
    //    DestroyImmediate(this.gameObject.GetComponent<TweenAlpha>());
    //}
    //public void RemoveComponents()
    //{
    //    this.RemoveTweenAlpha();
    //    this.RemovePanel();
    //}
    void StartTween()
    {
        for (int i = 0; i < m_TweenAlpha.Length; i++)
            m_TweenAlpha[i].enabled = true;
    }
    public void StopTween()
    {
        for (int i = 0; i < m_TweenAlpha.Length; i++)
        {
            m_TweenAlpha[i].alpha = 1;
            m_TweenAlpha[i].enabled = false;
        }
    }
}
