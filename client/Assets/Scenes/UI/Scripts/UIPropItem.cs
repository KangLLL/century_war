using UnityEngine;
using System.Collections;
using ConfigUtilities;

public class UIPropItem : MonoBehaviour {
    [SerializeField] UILabel[] m_UILabel;//0=remaingUseTimes;1=tips
    [SerializeField] UISprite[] m_UISprite;//0=new icon;1 = fighting icon ;2= warning background ;3 = propIcon ;4 = Button  main background
 
    [SerializeField] GameObject m_PropIconParent;
    private PropsLogicData m_PropsLogicData;
    
    bool m_EnableRuntime = true;
    void OnClick()
    {
        if (UIManager.Instance.UIWindowPropInfo.ControlerFocus != this.gameObject)
        {
            this.ShowPropInfoWindow();
            UIManager.Instance.UIWindowPropInfo.ControlerFocus = this.gameObject;
        }
        else
        {
            if (UIManager.Instance.UIWindowPropInfo.gameObject.activeSelf || this.m_PropsLogicData == null)
                UIManager.Instance.UIWindowPropInfo.HideWindow();
            else
                this.ShowPropInfoWindow();
        }
    }
    void OnDrag()
    {
        UIManager.Instance.UIWindowPropInfo.HideWindow();
    }
    void Update()
    {
        this.OnTimer();
    }
    public void SetItemData(PropsLogicData propsLogicData)
    {
        this.SetBackground(propsLogicData);

        if (propsLogicData == null)
        {
            m_PropIconParent.SetActive(false);
            return;
        }
        else
            m_PropIconParent.SetActive(true);
        this.m_PropsLogicData = propsLogicData;
        this.m_UILabel[0].text = "X" + this.m_PropsLogicData.RemainingUseTime.ToString();
        this.m_UILabel[1].text = this.m_PropsLogicData.RemainingCD > 0 ? SystemFunction.TimeSpanToString(this.m_PropsLogicData.RemainingCD) :
                                 LogicController.Instance.PlayerData.Level < this.m_PropsLogicData.RequireLevel ? string.Format(StringConstants.PROMT_REQUIRE_LEVEL, this.m_PropsLogicData.RequireLevel.ToString()) : "";
        this.m_UISprite[0].gameObject.SetActive(!PlayerPrefs.HasKey(LogicController.Instance.PlayerData.PlayerID.ToString() + ":PropID:" + this.m_PropsLogicData.PropsNo.ToString()));
        this.m_UISprite[1].gameObject.SetActive(this.m_PropsLogicData.IsInBattle);

        this.m_UISprite[2].gameObject.SetActive(!(this.m_PropsLogicData.RemainingCD <= 0 && LogicController.Instance.PlayerData.Level >= this.m_PropsLogicData.RequireLevel));
        this.m_UISprite[3].spriteName = this.m_PropsLogicData.PrefabName;
        this.m_UISprite[3].MakePixelPerfect();
        //print("SetItemData");
    }
    void SetBackground(PropsLogicData propsLogicData)
    {
        if (propsLogicData == null)
            m_UISprite[4].spriteName = ClientSystemConstants.PROPS_QUALITY_BG_NAME[-1];
        else
            m_UISprite[4].spriteName = ClientSystemConstants.PROPS_QUALITY_BG_NAME[(int)propsLogicData.Quality];
    }
    void OnTimer()
    {
        if (this.m_EnableRuntime)
        {
            if (this.m_PropsLogicData != null && this.m_PropsLogicData.RemainingCD > 0)
                this.m_UILabel[1].text = SystemFunction.TimeSpanToString(this.m_PropsLogicData.RemainingCD);
            else
            {
                this.SetItemData(this.m_PropsLogicData);
                this.m_EnableRuntime = false;
            }
        }
    }
    void OnEnable()
    {
        this.m_EnableRuntime = true;
    }
    void ShowPropInfoWindow()
    {
        if (this.m_PropsLogicData == null)
            return;
        UIManager.Instance.UIWindowPropInfo.UnRegistDelegate();
        UIManager.Instance.UIWindowPropInfo.ClickUse += this.OnUseProp; 
        UIManager.Instance.UIWindowPropInfo.ClickDestroy += this.OnDestroyProp;
        UIManager.Instance.UIWindowPropInfo.ListenButton += this.OnSetUseButton;

        string qualityColor = ClientSystemConstants.PROPS_QUALITY_COLOR[m_PropsLogicData.Quality];
        string name = qualityColor + m_PropsLogicData.Name;
        string category = qualityColor + ClientSystemConstants.PROPS_CATEGORY[m_PropsLogicData.Category];
        string quality = qualityColor + ClientSystemConstants.PROPS_QUALITY[m_PropsLogicData.Quality];
        string description = ClientSystemConstants.PROPS_DESCRIPTION + m_PropsLogicData.Description;
        UIManager.Instance.UIWindowPropInfo.SetWindowPositon(this.transform.position, this.transform.position.y > UIManager.Instance.UIWindowPropsStorage.VewWindowPosition.y ? Side.Bottom : Side.Top);
        UIManager.Instance.UIWindowPropInfo.SetWindowStyle(ClientSystemConstants.PROPS_QUALITY_WIN_NAME[m_PropsLogicData.Quality]);
        
        UIManager.Instance.UIWindowPropInfo.ShowWindow(name,category,quality,description);
        //UIManager.Instance.UIWindowPropInfo.SetUseButton(this.CheckUseCondition(false));
        this.OnSetUseButton();
    }
    void OnSetUseButton()
    {
        bool condition = this.CheckUseCondition(false);
        switch (m_PropsLogicData.Category)
        {
            case PropsCategory.Attack:
                if (!this.m_PropsLogicData.IsInBattle)
                    UIManager.Instance.UIWindowPropInfo.SetUseButton(condition,StringConstants.PROMT_FIGHT_PROP);
                else
                    UIManager.Instance.UIWindowPropInfo.SetUseButton(true, StringConstants.PROMT_CANCEL_FIGHT_PROP);
                break;
            case PropsCategory.Auxiliary:
                UIManager.Instance.UIWindowPropInfo.SetUseButton(condition, StringConstants.PROMT_USE_PROP);
                break;
            case PropsCategory.Defense:
                UIManager.Instance.UIWindowPropInfo.SetUseButton(condition, StringConstants.PROMT_PLACE_PROP);
                break;
            case PropsCategory.Special:
                UIManager.Instance.UIWindowPropInfo.SetUseButton(condition, StringConstants.PROMT_USE_PROP, m_PropsLogicData.FunctionConfigData != null);
                break;
        }
    }
    void OnUseProp()
    { 
        switch (m_PropsLogicData.Category)
        {
            case PropsCategory.Attack:
                if (!this.m_PropsLogicData.IsInBattle)
                {
                    if (!this.CheckUseCondition(true))
                        return;
                    LogicController.Instance.AddPropsInBattle(this.m_PropsLogicData.PropsNo);
                }
                else
                    LogicController.Instance.RemovePropsInBattle(this.m_PropsLogicData.PropsNo);

                if (this.m_PropsLogicData.RemainingUseTime <= 0) 
                    UIManager.Instance.UIWindowPropsStorage.SetCurrentWindowItem(); 
                else
                {
                    this.m_UILabel[0].text = "X" + this.m_PropsLogicData.RemainingUseTime.ToString();
                    this.m_UISprite[1].gameObject.SetActive(this.m_PropsLogicData.IsInBattle);
                }
                break;
            case PropsCategory.Defense:
                if (!this.CheckUseCondition(true))
                    return;
                SceneManager.Instance.DestroyTemporaryBuildingBehavior();
                SceneManager.Instance.UnSelectBuilding();
                SceneManager.Instance.ConstructDefenseProp(new DefenseObjectConfigWrapper(ConfigInterface.Instance.PropsConfigHelper.GetPropsData(this.m_PropsLogicData.PropsType).FunctionConfigData), this.m_PropsLogicData, false);
                UIManager.Instance.UIWindowPropsStorage.HideWindow();
                break;
            case PropsCategory.Auxiliary:
                if (!this.CheckUseCondition(true))
                    return;
                LogicController.Instance.UesAuxilaryProps(this.m_PropsLogicData.PropsNo);
                UIManager.Instance.UIWindowPropsStorage.HideWindow();
                break;
            case PropsCategory.Special:
                break;
        }
        
    }
    void OnDestroyProp()
    { 
        UIManager.Instance.UIWindowPropDestroy.UnRegistDelegate();
        UIManager.Instance.UIWindowPropDestroy.Click += () =>
        { 
            LogicController.Instance.DestroyProps(this.m_PropsLogicData.PropsNo);
            UIManager.Instance.UIWindowPropsStorage.SetCurrentWindowItem();
        }; 
        UIManager.Instance.UIWindowPropDestroy.ShowWindow(StringConstants.PROMT_DESTROY_PROP, string.Format(StringConstants.PROMT_DESTROY_PROP_CONTEXT, ClientSystemConstants.PROPS_QUALITY_COLOR[m_PropsLogicData.Quality] + m_PropsLogicData.Name + "[-]"));
    }
    bool CheckUseCondition(bool warning)
    {
        bool condition = true;
        if (!(this.m_PropsLogicData.RemainingCD <= 0))
        {
            if (warning)
                UIErrorMessage.Instance.ErrorMessage(29);
            return false;
        }

        if (!(LogicController.Instance.PlayerData.Level >= this.m_PropsLogicData.RequireLevel))
        {
            if (warning)
                UIErrorMessage.Instance.ErrorMessage(28);
            return false;
        }
        switch (this.m_PropsLogicData.Category)
        {
            case PropsCategory.Attack:
                if(!(LogicController.Instance.AvailableBattlePropsNumner < LogicController.Instance.MaxAttackPropsSlot))
                {
                    if (warning)
                        UIErrorMessage.Instance.ErrorMessage(31);
                    return false;
                }
                break;
            case PropsCategory.Auxiliary:
                break;
            case PropsCategory.Defense:
                if (!(LogicController.Instance.AvailableDefenseObjectNumber < LogicController.Instance.MaxDefenseObjectNumber))
                {
                    if (warning)
                        UIErrorMessage.Instance.ErrorMessage(28);
                    return false;
                }
                break;
            case PropsCategory.Special:
                break;
        }

        return condition;
    }
}

