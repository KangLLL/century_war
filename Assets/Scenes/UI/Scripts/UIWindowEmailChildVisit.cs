using UnityEngine;
using System.Collections;
using System;
public class UIWindowEmailChildVisit : UIWindowCommon
{
    [SerializeField] UILabel m_UILabelName;
    public Action MissionEvent1;//visit rival or friend
    public Action MissionEvent2;//view clan
	// Use this for initialization
	void Start () {
	
	}

    //void OnClick()
    //{
    //    this.HideWindow();
    //}
    public void ShowWindow(string name)
    {
        this.gameObject.SetActive(true);
        this.SetWindowItem(name);
    }
    public override void HideWindow()
    {
        this.UnRegistDelegate();
        this.gameObject.SetActive(false);
    }
    protected void OnMission1()
    {
        if (this.MissionEvent1 != null)
        {
            MissionEvent1();
        }
        this.HideWindow();
    }
    protected void OnMission2()
    {
        if (this.MissionEvent2 != null)
        {
            MissionEvent2();
        }
        this.HideWindow();
    }
    public void UnRegistDelegate()
    {
        this.MissionEvent1 = null;
        this.MissionEvent2 = null;
    }
    public void SetWindowItem(string name)
    {
        m_UILabelName.text = name;
    }
}
