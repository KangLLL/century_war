using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class UIWindowUpgradeArmy : UIWindowCommon {
 
    
    [SerializeField] UIWindowItemCommon[] m_UIWindowItemCommon;//0 = descriptin modul ,1 = upgrade army modul ,2 = select army modul
    [SerializeField] UIArmyUpgradeItem[] m_UIArmyUpgradeItem;
    void Awake()
    {
        this.GetTweenComponent();
    }
    protected override void GetTweenComponent()
    {
        base.GetTweenComponent();
    }
    public override void HideWindow()
    {
        this.StopAllArmyItemTween();
        ((UIArmyUpragdeModul)m_UIWindowItemCommon[1]).EnableUprgade = false;
        base.HideWindow();
    }
    public override void ShowWindow()
    {
        SetWindowItemData();
        base.ShowWindow();
    }
    public void SetWindowItemData()
    {
        //m_UILabel[0].text = StringConstants.PROMPT_SELECT_ARMY; //base.BuildingLogicObject.AlreadyProduceArmyCapacity + " / " + base.BuildingLogicObject.ArmyProduceCapacity;
        //m_UILabel[1].text = (base.BuildingLogicObject.AlreadyProduceArmyCapacity + LogicController.Instance.TotalArmyCapacity) + " / " + LogicController.Instance.CampsTotalCapacity;

        for (int i = 0; i < m_UIWindowItemCommon.Length; i++)
        {
            m_UIWindowItemCommon[i].BuildingLogicData = base.BuildingLogicData;
        }
        if (base.BuildingLogicData.ArmyUpgrade.HasValue)
        {
            m_UIWindowItemCommon[0].gameObject.SetActive(false);
            m_UIWindowItemCommon[1].gameObject.SetActive(true);
            m_UIWindowItemCommon[1].SetWindowItem();
            ((UIArmyUpragdeModul)m_UIWindowItemCommon[1]).EnableUprgade = true; 
        }
        else
        {
            m_UIWindowItemCommon[0].gameObject.SetActive(true);
            m_UIWindowItemCommon[1].gameObject.SetActive(false); 
        }
        m_UIWindowItemCommon[2].SetWindowItem();

    }

    void StopAllArmyItemTween()
    {
        for (int i = 0; i < m_UIArmyUpgradeItem.Length; i++)
            m_UIArmyUpgradeItem[i].StopTween();
    }
}
