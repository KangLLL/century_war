using UnityEngine;
using System.Collections;

public class UIConstructBuilding : UIWindowCommon
{
    [SerializeField] UILabel[] m_UILabelText;//;0=food;1=gold;2=gem;
    // Use this for initialization
    void Awake()
    {
        this.GetTweenComponent();
    }

 
    public override void ShowWindow()
    {
        base.ShowWindowImmediately();
        UIManager.Instance.UIWindowMain.gameObject.SetActive(false);
        this.SetWindow();
    }
    public override void HideWindow()
    {
        base.HideWindowImmediately(true, true);
        UIManager.Instance.UIWindowMain.gameObject.SetActive(true);
    }
    protected override void GetTweenComponent()
    {
        base.GetTweenComponent();
    }
    void SetWindow()
    {
        m_UILabelText[0].text = LogicController.Instance.PlayerData.CurrentStoreFood.ToString();
        m_UILabelText[1].text = LogicController.Instance.PlayerData.CurrentStoreGold.ToString();
        m_UILabelText[2].text = LogicController.Instance.PlayerData.CurrentStoreGem.ToString();
    }

}
