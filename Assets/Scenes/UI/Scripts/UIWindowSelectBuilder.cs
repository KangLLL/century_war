using UnityEngine;
using System.Collections;
using ConfigUtilities;
using ConfigUtilities.Enums;
public class UIWindowSelectBuilder : UIWindowCommon {
    [SerializeField] UIItemBuilder[] m_UIItemBuilder;
    public BuildingConfigData BuildingConfigData { get; set; }
    public BuildingBehavior BuildingBehavior { get; set; }
    public RemovableObjectLogicData RemovableObjectLogicData { get; set; }
    public BuilderMenuType BuilderMenuType { get; set; }
	// Use this for initialization
    void Awake()
    {
        this.GetTweenComponent();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    protected override void GetTweenComponent()
    {
        base.GetTweenComponent();
    }
    public override void HideWindow()
    {
        base.HideWindow();
        if (this.BuilderMenuType != global::BuilderMenuType.RemoveObject)
            this.BuildingBehavior.ActiveButton(true);
    }
    public override void ShowWindow()
    {
        base.ShowWindow();
        SetBuilderItemData();
    }
    void SetBuilderItemData()
    {
        //int builderCount = ConfigInterface.Instance.SystemConfig.MaxBuilderNumber; //LogicController.Instance.AllBuilderInformation.Count;
        for (int i = 0, count = m_UIItemBuilder.Length; i < count; i++)
        {
            m_UIItemBuilder[i].BuildingLogicData = base.BuildingLogicData;
            m_UIItemBuilder[i].BuildingConfigData = this.BuildingConfigData;
            m_UIItemBuilder[i].BuilderMenuType = this.BuilderMenuType;//BuilderMenuType.Construct;
            m_UIItemBuilder[i].BuildingBehavior = this.BuildingBehavior;
            m_UIItemBuilder[i].RemovableObjectLogicData = this.RemovableObjectLogicData;
            BuilderData builderData = LogicController.Instance.AllBuilderInformation[i];
            //if (i <= builderCount - 1)
            if (builderData == null)//disable
            {
                m_UIItemBuilder[i].SetItemDataDisable();
                m_UIItemBuilder[i].BuilderNo = i;
                m_UIItemBuilder[i].BuilderState = BuilderState.Disable;
            }
            else
            {
                if (builderData.CurrentWorkTarget == null)//builder idle
                {
                    m_UIItemBuilder[i].BuilderData = builderData;
                    m_UIItemBuilder[i].BuilderNo = i;
                    m_UIItemBuilder[i].BuilderState = BuilderState.Idle;
                    m_UIItemBuilder[i].SetItemDataIdle();
                }
                else//buider busy
                {
                    m_UIItemBuilder[i].BuilderNo = i;
                    m_UIItemBuilder[i].BuilderData = builderData;
                    m_UIItemBuilder[i].BuilderState = BuilderState.Busy; 
                    m_UIItemBuilder[i].SetItemDataBusy();
                }
            }
        
        }
    }
}
