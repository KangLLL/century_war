using UnityEngine;
using System.Collections;
using System;

public class UIWindowCostPrompt : UIWindowCommon {
    [SerializeField] protected UILabel[] m_UILabelText;//0=Title,1=Context,2=CostValue
    //public delegate void MissionDelegate();
    //public MissionDelegate MissionCost;
    public event Action Click;
    int m_CostGem;
    string m_CostContext;
	// Use this for initialization
     void Awake()
     {
        this.GetTweenComponent();
	}

	void Update()
	{
		if(UIManager.Instance.UIWindowFocus == null && this.m_IsShow)
		{
			UIManager.Instance.UIWindowFocus = this.gameObject;
		}
	}
	 
    protected override void GetTweenComponent()
    {
        base.GetTweenComponent();
    }
    public override void HideWindow()
    {
        this.UnRegistDelegate();
        base.HideWindow();
    }
    public void ShowWindow(int costGem,string costContext,string title = null)
    {
        this.m_CostGem = costGem;
        this.m_CostContext = costContext;
        SetWindowItem(title);
        base.ShowWindow();
    }
    public void ShowWindow(int costGem, string costContext,Vector3 to, string title = null)
    {
        this.m_CostGem = costGem;
        this.m_CostContext = costContext;
        SetWindowItem(title);
        base.ShowWindow(to);
    }
    void SetWindowItem(string title)
    {
        m_UILabelText[0].text = title == null ? StringConstants.PROMPT_REQUEST_GOLD : title;
        m_UILabelText[1].text = m_CostContext;
        m_UILabelText[2].text = m_CostGem.ToString();
        m_UILabelText[2].color = LogicController.Instance.PlayerData.CurrentStoreGem < this.m_CostGem ? Color.red : Color.white;
    }
 
    protected void OnMission()
    {
        
        if (this.Click != null)
        {
            Click();
        }
        base.HideWindow();
        this.UnRegistDelegate(); 
    }
    public void UnRegistDelegate()
    {
        this.Click = null;
    }
}
