using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UIWindowEmail :UIWindowCommon {
    [SerializeField] UIEmailModul[] m_UIEmailModul;
    private UIEmailModul m_CurrentUIEmailModul;
    void Awake()
    {
        this.GetTweenComponent();
    }
    public override void HideWindow()
    {
        base.HideWindow();
        UIManager.Instance.UIWindowEmailChildVisit.HideWindow();
        if (this.m_CurrentUIEmailModul != null)
            this.m_CurrentUIEmailModul.HideEmailModul(0.2f);
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
        if (this.m_CurrentUIEmailModul != null)
            if (!this.m_CurrentUIEmailModul.Equals(m_UIEmailModul[0]))
                this.m_CurrentUIEmailModul.HideEmailModul();

        this.m_CurrentUIEmailModul = m_UIEmailModul[0];
        this.m_CurrentUIEmailModul.ShowEmailModul();
        this.m_CurrentUIEmailModul.SetModulData(LogicController.Instance.PlayerData.DefenseLogs);

    }
    //button message
    void OnDefendLog()
    {
        if (!this.m_CurrentUIEmailModul.Equals(m_UIEmailModul[0]))
        {
            
            this.m_CurrentUIEmailModul.HideEmailModul(); 
            this.m_CurrentUIEmailModul = m_UIEmailModul[0];
            this.m_CurrentUIEmailModul.ShowEmailModul();
            this.m_CurrentUIEmailModul.SetModulData(LogicController.Instance.PlayerData.DefenseLogs);
        }
    }
    //button message
    void OnAttackLog()
    {
        if (!this.m_CurrentUIEmailModul.Equals(m_UIEmailModul[1]))
        {
            this.m_CurrentUIEmailModul.HideEmailModul(); 
            this.m_CurrentUIEmailModul = m_UIEmailModul[1];
            this.m_CurrentUIEmailModul.ShowEmailModul();
            this.m_CurrentUIEmailModul.SetModulData(LogicController.Instance.PlayerData.AttackLogs);
        }
    }

}
