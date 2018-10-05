using UnityEngine;
using System.Collections;
using CommandConsts;
using System.Security.Cryptography;
using System;
using System.Text;


public class UIWindowUserLogin : UIWindowCommon {
    [SerializeField] UILabel m_UILabelEmail;
    [SerializeField] UILabel m_UILabelPassword;
    [SerializeField] UICheckbox m_UICheckboxAutomaticLogin;
    //[SerializeField] UICheckbox m_UICheckboxPassword;
    [SerializeField] UIWindowUserRegist m_UIWindowUserRegist;
    [SerializeField] GameObject m_Progress;
    [SerializeField] UIInput m_UIInputEmail;
    [SerializeField] UIInput m_UIInputPassword;
    event Action LoginFail;
    void Awake()
    {
        base.GetTweenComponent();
        this.LoadCheckboxAutomaticLogin();
    }
    public override void ShowWindow()
    {
        this.AutomaticLogin();
    }
    public void ShowWindow(bool automaticLogin)
    {
        if (automaticLogin)
            this.ShowWindow();
        else
        {
            this.LoadAccount();
            this.LoadPassword();
            //this.LoadCheckboxAutomaticLogin();
            base.ShowWindowImmediatelySimplify();
        }
    }
    public override void HideWindow()
    {
        base.HideWindowImmediatelySimplify();
    }
    public void SetUserInput(string email,string password)
    {
        this.m_UIInputEmail.text = email;
        //this.m_UILabelPassword.text = password;
        //this.m_UIInputPassword.selected = true;
        this.m_UIInputPassword.text = string.Empty;
        //this.m_UIInputPassword.selected = false;
    }
    //Button message
    bool OnUserLogin()
    { 
        if (!SystemFunction.RegexEmail(m_UIInputEmail.text))
        {
            UIErrorMessage.Instance.ErrorMessage(10);
            return false;
        } 
        if (!SystemFunction.RegexPassword(m_UIInputPassword.text))
        {
            UIErrorMessage.Instance.ErrorMessage(34);
            return false;
        } 
        byte[] md5ByteArray = MD5.Create().ComputeHash(Encoding.Default.GetBytes(this.m_UIInputPassword.text), 0, this.m_UIInputPassword.text.Length);
        string result = BitConverter.ToString(md5ByteArray).Replace("-", "");
        CommunicationUtility.Instance.LoginAccount(new LoginAccountRequestParameter() { AccountName = this.m_UIInputEmail.text, AccountPassword = result }, this, "OnReceiveLogin", true);
        LockScreen.Instance.DisableInput();
        this.m_Progress.SetActive(true);
        return true;
    }
    //Button message
    void OnUserGotoRegist()
    {
        this.HideWindow();
        this.m_UIWindowUserRegist.ShowWindow();
    }
    void OnReceiveLogin(Hashtable hashtable)
    {
        LoginAccountResponseParameter response = new LoginAccountResponseParameter();
        response.InitialParameterObjectFromHashtable(hashtable);
        if (!response.FailType.HasValue)
		{
            this.SaveAccount();
            this.SavePassword();
            this.SaveCheckboxAutomaticLogin();
            iOSCenter.Instance.AccountID = response.AccountID.Value;
            this.HideWindow();
        }
        else
        {
            switch (response.FailType.Value)
            {
                case LoginAccountFailType.AccountIsNotExist:
                    UIErrorMessage.Instance.ErrorMessage(32);
                    break;
                case LoginAccountFailType.PasswordIsIncorrect:
                    UIErrorMessage.Instance.ErrorMessage(32);
                    break;
            }
            LockScreen.Instance.EnableInput();
            this.m_Progress.SetActive(false);
            if (this.LoginFail != null)
                this.LoginFail();
        }
    }
    void AutomaticLogin()
    {
        if (this.LoadAccount() && this.LoadCheckboxAutomaticLogin() && this.LoadPassword() && this.OnUserLogin())
        {
            this.LoginFail = null;
            this.LoginFail += () => ShowWindowImmediatelySimplify();
        }
        else
            base.ShowWindowImmediatelySimplify();
    }
    bool LoadAccount()
    {
        return (this.m_UIInputEmail.text = PlayerPrefs.GetString("Account", string.Empty)) != string.Empty;
    }
    bool LoadPassword()
	{
        return (this.m_UIInputPassword.text = PlayerPrefs.GetString("Password", string.Empty)) != string.Empty;
    }
    bool LoadCheckboxAutomaticLogin()
    { 
        this.m_UICheckboxAutomaticLogin.isChecked = PlayerPrefs.GetInt("AutomaticLogin", 0) == 1;
        return this.m_UICheckboxAutomaticLogin.isChecked;
    }
    void SaveAccount()
    {
        //if (this.m_UICheckboxAutomaticLogin.isChecked)
            PlayerPrefs.SetString("Account", this.m_UIInputEmail.text);
        //else
        //    PlayerPrefs.DeleteKey("Account");
    }
    void SavePassword()
    {
        if (this.m_UICheckboxAutomaticLogin.isChecked)
            PlayerPrefs.SetString("Password", this.m_UIInputPassword.text);
        else
            PlayerPrefs.DeleteKey("Password");
    }
    void SaveCheckboxAutomaticLogin()
    {
        if (this.m_UICheckboxAutomaticLogin.isChecked)
            PlayerPrefs.SetInt("AutomaticLogin", 1);
        else
            PlayerPrefs.DeleteKey("AutomaticLogin"); 
    }
    //IOS input done event
    void OnSubmitAccount(string text)
    {
        m_UILabelEmail.text = SystemFunction.ReplaceEmoji(text);
        m_UIInputPassword.selected = true; 
       
    }
    //IOS input done event
    void OnSubmitPassword(string text)
    {
        this.OnUserLogin();
    }
}
