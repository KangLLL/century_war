using UnityEngine;
using System.Collections;
using ConfigUtilities;
using System.Linq;
public class UIWindowBuyTree : UIWindowCommon
{
    [SerializeField] UIBuyTreeModule m_UIBuyTreeModule;
    [SerializeField] UILabel[] m_UILabelText;//0=title;1=gold;2=food;3=gem;4=obstacle count
    public UIBuyTreeModule UIBuyTreeModule { get { return m_UIBuyTreeModule; } }
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
        int plantCount = LogicController.Instance.AllRemovableObjects.Count(a => a.IsCountable);
        m_UILabelText[4].text = string.Format(StringConstants.PROMT_OBSTACLE_COUNT, plantCount, ConfigInterface.Instance.SystemConfig.MaxRemovableObjectNumber);
        m_UILabelText[4].color = plantCount < ConfigInterface.Instance.SystemConfig.MaxRemovableObjectNumber ? Color.white : Color.red;
        this.m_UIBuyTreeModule.SetModulItem();
        SpringPanel springPanel = this.m_UIBuyTreeModule.GetComponent<SpringPanel>();
        DestroyImmediate(springPanel);
    }
}
