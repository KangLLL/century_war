using UnityEngine;
using System.Collections;
using System;

public class UIWindowConfirmLogin : UIWindowCommon {
    [SerializeField] UILabel[] m_UILabelText;//0=Title,1=Context
    public event Action Click;
    void Awake()
    {
        base.GetTweenComponent();
    }
    public void ShowWindow(string title, string context)
    {
        this.m_UILabelText[0].text = title;
        this.m_UILabelText[1].text = context; 
        base.ShowWindowImmediatelySimplify();
    }
    public override void HideWindow()
    {
        base.HideWindowImmediatelySimplify();
    }
    //Button message
    void OnMission()
    {

        if (this.Click != null)
        {
            Click();
        }
        this.HideWindow();
        this.UnRegistDelegate(); 
    }
    public void UnRegistDelegate()
    {
        this.Click = null;
    }
}
