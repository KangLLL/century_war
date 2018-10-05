using UnityEngine;
using System.Collections;
using System;
using System.Text.RegularExpressions;
public class UIWindowAccount : UIWindowCommon {
    [SerializeField] UILabel[] m_UILabel;//0 = title; 1 = input email; 2 = input password;

    public event EventHandler Click;
    void Awake()
    {
        this.GetTweenComponent();
    } 
    public override void HideWindow()
    {
        this.UnRegistDelegate();
        base.HideWindow();
    }
    public void ShowWindow(string title)
    {
        m_UILabel[1].text = title;
        this.ClearInputText();
        base.ShowWindow();
    }
    void UnRegistDelegate()
    {
        this.Click = null;
    }
    protected void OnMission()
    {
        if (this.Click != null)
        {
            Click(this, null);
        }
        //this.HideWindow();
    }
    void ClearInputText()
    {
        m_UILabel[1].text = "";
        m_UILabel[2].text = "";
    }
    public bool CheckUserRegistAccount()
    {
        if (!this.CheckUserInput())
            return false;
        //if (NdAccountUtility.Instance.IsMounted)
        //{
        //    UIErrorMessage.Instance.ErrorMessage(11);
        //    print(StringConstants.ERROR_MESSAGE[11]);
        //    return false;
        //}
        
        return true;
    }
    public bool CheckUserExchangeAccount()
    {
        if (!this.CheckUserInput())
            return false;
        return true;
    }
    bool CheckUserInput()
    {
        string regexEmail = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"; 
        string regexPassword = @"^\S{6,20}$";
        
        if (!Regex.IsMatch(m_UILabel[1].text,regexEmail))
        {
            UIErrorMessage.Instance.ErrorMessage(10);
            print(StringConstants.ERROR_MESSAGE[10]);
            return false;
        }
        if (!Regex.IsMatch(m_UILabel[2].text,regexPassword))
        {
            UIErrorMessage.Instance.ErrorMessage(12);
            print(StringConstants.ERROR_MESSAGE[12]);
            return false;
        }
        return true;
    }
   

}
