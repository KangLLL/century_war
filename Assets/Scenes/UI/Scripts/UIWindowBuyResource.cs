using UnityEngine;
using System.Collections;
using ConfigUtilities;
using System.Linq;
public class UIWindowBuyResource : UIWindowCommon
{
    [SerializeField] UIBuyResourceModule m_UIBuyResourceModule;
    [SerializeField] UILabel[] m_UILabelText;//0=title;1=gold;2=food;3=gem;

    void Awake()
    {
        this.GetTweenComponent();
    }
    public override void ShowWindow()
    {
        UIManager.Instance.UIWindowMain.gameObject.SetActive(false);
        base.ShowWindowImmediately();
        this.SetWindowModul();
    }
    public override void HideWindow()
    {
        base.HideWindowImmediately(true, true);
        UIManager.Instance.UIWindowMain.gameObject.SetActive(true);
        UIManager.Instance.UIWindowBuyTreeChild.HideWindow();
    }
    void SetWindowModul()
    {
        //m_UILabelText[0].text = ClientSystemConstants.UIMENU_TYPE_DICTIONARY[uiMenuType];
        m_UILabelText[1].text = LogicController.Instance.PlayerData.CurrentStoreGold.ToString();
        m_UILabelText[2].text = LogicController.Instance.PlayerData.CurrentStoreFood.ToString();
        m_UILabelText[3].text = LogicController.Instance.PlayerData.CurrentStoreGem.ToString();

        this.m_UIBuyResourceModule.SetModulItem();
        SpringPanel springPanel = this.m_UIBuyResourceModule.GetComponent<SpringPanel>();
        DestroyImmediate(springPanel);
    }
}
