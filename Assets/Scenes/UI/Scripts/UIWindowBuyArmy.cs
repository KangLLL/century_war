using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
public class UIWindowBuyArmy : UIWindowCommon {
    //[SerializeField] UIGrid m_UIGridProduce;
    [SerializeField] UIArmyItemInfomation[] m_UIArmyItemInfomation;
    //[SerializeField] UIGrid m_UIGridProduceIcon;
    //[SerializeField] UIExpProgressBar[] m_ArmyIcon;//ArmyIcon Bar
    [SerializeField] UILabel[] m_UILabel;//0=Current Bar count; 1=Total ArmyClamp count
    [SerializeField] UIArmyQueueModul m_UIArmyQueueModul;
    bool m_EnableProduceQuene = false;
	// Use this for initialization
    void Awake()
    {
        this.GetTweenComponent();
	}
	void Update () {
        OnSetItemData();
		this.SetWindowItemData();
	}
    public override void ShowWindow()
    {
        this.m_EnableProduceQuene = true; 
        m_UIArmyQueueModul.BuildingLogicData = base.BuildingLogicData;
        //SetWindowItemData();
        
        base.ShowWindow();
        //UIManager.Instance.UICamera.allowMultiTouch = true;
        
    }
    public override void HideWindow()
    {
        base.HideWindow();
        this.m_EnableProduceQuene = false;
        m_UIArmyQueueModul.EnableProduceQuene = this.m_EnableProduceQuene;
        //UIManager.Instance.UICamera.allowMultiTouch = false;
    }
    protected override void GetTweenComponent()
    {
        
        base.GetTweenComponent();
    }
    void ShowWindows(object param)
    { 
        this.ShowWindow();
    }
 
    public void SetWindowItemData()
    {
        //m_UILabel[0].text = base.BuildingLogicObject.AlreadyProduceArmyCapacity + " / " + base.BuildingLogicObject.ArmyProduceCapacity;
        if (LogicController.Instance.TotalArmyCapacity > LogicController.Instance.CampsTotalCapacity)
            m_UILabel[1].text = "[ff0000]" + LogicController.Instance.TotalArmyCapacity + "[-]" + " / " + LogicController.Instance.CampsTotalCapacity;
        else
            m_UILabel[1].text = LogicController.Instance.TotalArmyCapacity + " / " + LogicController.Instance.CampsTotalCapacity;
        for (int i = 0; i < m_UIArmyItemInfomation.Length; i++)
        {
            m_UIArmyItemInfomation[i].BuildingLogicData = base.BuildingLogicData;
            m_UIArmyItemInfomation[i].SetItemData();
        }
        m_UIArmyQueueModul.EnableProduceQuene = this.m_EnableProduceQuene;
        m_UIArmyQueueModul.ActiveQueue();
        m_UIArmyQueueModul.SetImmediatelyBtnState(LogicController.Instance.TotalArmyCapacity <= LogicController.Instance.CampsTotalCapacity);
    }
    void OnSetItemData()
    {
        m_UILabel[0].text = base.BuildingLogicData.AlreadyProduceArmyCapacity + " / " + base.BuildingLogicData.ArmyProduceCapacity; 
    }
    public void BuyArmy(ArmyType armyType)
    {
        LogicController.Instance.ProduceArmy(armyType, base.BuildingLogicData.BuildingIdentity);
        this.SetWindowItemData();
    }
}
