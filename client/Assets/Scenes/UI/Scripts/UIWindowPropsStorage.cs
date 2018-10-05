using UnityEngine;
using System.Collections;

public class UIWindowPropsStorage : UIWindowCommon
{
    [SerializeField] UIPropsStorageModul[] m_UIPropsStorageModul;//All,Attack,Defend,Subsidiary,Special
    [SerializeField] GameObject m_ViewWindow;
    public Vector3 VewWindowPosition { get { return m_ViewWindow.transform.position; } }
    
    private UIPropsStorageModul m_CurrentUIPropsStorageModul;

    void Awake()
    {
        this.GetTweenComponent();
    }
    void Start()
    {

    }
    public override void HideWindow()
    {
        this.ClearNewProps();
        base.HideWindow();
        UIManager.Instance.UIWindowPropInfo.HideWindow();
        UIManager.Instance.UIWindowPropDestroy.HideWindow();
        if (this.m_CurrentUIPropsStorageModul != null)
            this.m_CurrentUIPropsStorageModul.HidePropsModul(0.2f);
    }
    public override void ShowWindow()
    {
        base.ShowWindow();
        this.SetWindowItem();
    }
    protected override void GetTweenComponent()
    {
        base.GetTweenComponent();
    }
    void SetWindowItem()
    {
        if (this.m_CurrentUIPropsStorageModul != null)
            if (!this.m_CurrentUIPropsStorageModul.Equals(m_UIPropsStorageModul[0]))
                this.m_CurrentUIPropsStorageModul.HidePropsModul();

        this.m_CurrentUIPropsStorageModul = m_UIPropsStorageModul[0];
        this.m_CurrentUIPropsStorageModul.ShowPropsModul();
        this.m_CurrentUIPropsStorageModul.SetModulData();
    }
    public void SetCurrentWindowItem()
    {
        if (this.m_CurrentUIPropsStorageModul != null)
        {
            this.m_CurrentUIPropsStorageModul.ShowPropsModul();
            this.m_CurrentUIPropsStorageModul.SetModulData();
        }
    }
    void ClearNewProps()
    {
        foreach (PropsLogicData prop in LogicController.Instance.AllProps)
        {
            if (!PlayerPrefs.HasKey(LogicController.Instance.PlayerData.PlayerID.ToString() + ":PropID:" + prop.PropsNo.ToString()))
                PlayerPrefs.SetString(LogicController.Instance.PlayerData.PlayerID.ToString() + ":PropID:" + prop.PropsNo.ToString(), prop.PropsNo.ToString());
        }
    }
    //button message
    void OnAllModul()
    {
        if (!this.m_CurrentUIPropsStorageModul.Equals(m_UIPropsStorageModul[0]))
        {
            this.m_CurrentUIPropsStorageModul.HidePropsModul();
            this.m_CurrentUIPropsStorageModul = m_UIPropsStorageModul[0];
            this.m_CurrentUIPropsStorageModul.ShowPropsModul();
            this.m_CurrentUIPropsStorageModul.SetModulData();
        }
    }
    //button message
    void OnAttackModul()
    {
        if (!this.m_CurrentUIPropsStorageModul.Equals(m_UIPropsStorageModul[1]))
        {
            this.m_CurrentUIPropsStorageModul.HidePropsModul();
            this.m_CurrentUIPropsStorageModul = m_UIPropsStorageModul[1];
            this.m_CurrentUIPropsStorageModul.ShowPropsModul();
            this.m_CurrentUIPropsStorageModul.SetModulData();
        }
    }
    //button message
    void OnDefendModul()
    {
        if (!this.m_CurrentUIPropsStorageModul.Equals(m_UIPropsStorageModul[2]))
        {
            this.m_CurrentUIPropsStorageModul.HidePropsModul();
            this.m_CurrentUIPropsStorageModul = m_UIPropsStorageModul[2];
            this.m_CurrentUIPropsStorageModul.ShowPropsModul();
            this.m_CurrentUIPropsStorageModul.SetModulData();
        }
    }
    //button message
    void OnSubsidiaryModul()
    {
        if (!this.m_CurrentUIPropsStorageModul.Equals(m_UIPropsStorageModul[3]))
        {
            this.m_CurrentUIPropsStorageModul.HidePropsModul();
            this.m_CurrentUIPropsStorageModul = m_UIPropsStorageModul[3];
            this.m_CurrentUIPropsStorageModul.ShowPropsModul();
            this.m_CurrentUIPropsStorageModul.SetModulData();
        }
    }
    //button message
    void OnSpecialModul()
    {
        if (!this.m_CurrentUIPropsStorageModul.Equals(m_UIPropsStorageModul[4]))
        {
            this.m_CurrentUIPropsStorageModul.HidePropsModul();
            this.m_CurrentUIPropsStorageModul = m_UIPropsStorageModul[4];
            this.m_CurrentUIPropsStorageModul.ShowPropsModul();
            this.m_CurrentUIPropsStorageModul.SetModulData();
        }
    }

 
}
