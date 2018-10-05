using UnityEngine;
using System.Collections;
using CommandConsts;
using System.Security.Cryptography;
using System.Text;
using System;
public class UIWindowUserRegist : UIWindowCommon
{
    [SerializeField] UILabel m_UILabelEmail;
    [SerializeField] UILabel m_UILabelPassword;
    [SerializeField] UILabel m_UILabelConformPassword;
    [SerializeField] UIWindowUserLogin m_UIWindowUserLogin;
    [SerializeField] UIWindowConfirmLogin m_UIWindowConfirmLogin;
    [SerializeField] GameObject m_Progress;
    [SerializeField] UIInput m_UIInputEmail;
    [SerializeField] UIInput m_UIInputPassword;
    [SerializeField] UIInput m_UIInputPasswordConform;
    void Awake()
    {
        base.GetTweenComponent();
    }
    public override void ShowWindow()
    {
        this.ClearInput();
        base.ShowWindowImmediatelySimplify();
    }
    public override void HideWindow()
    {
        base.HideWindowImmediatelySimplify();
    }
    //Button message
    void OnUserRegist()
    {
        if (!SystemFunction.RegexEmail(m_UIInputEmail.text))
        {
            UIErrorMessage.Instance.ErrorMessage(10);
            return;
        }
        if (!SystemFunction.RegexPassword(m_UIInputPassword.text))
        {
            UIErrorMessage.Instance.ErrorMessage(34);
            return;
        }
        if (!m_UIInputPassword.text.Equals(m_UIInputPasswordConform.text))
        {
            UIErrorMessage.Instance.ErrorMessage(33);
            return;
        }
        byte[] md5ByteArray = MD5.Create().ComputeHash(Encoding.Default.GetBytes(this.m_UIInputPassword.text), 0, this.m_UIInputPassword.text.Length);
 
        string result = BitConverter.ToString(md5ByteArray).Replace("-","");
        print("result =" + result);
        CommunicationUtility.Instance.RegisterAccount(new RegisterAccountRequestParameter() { AccountName = this.m_UIInputEmail.text, AccountPassword = result }, this, "OnReceivRegist", true);
        LockScreen.Instance.DisableInput();
        this.m_Progress.SetActive(true);
    }
    //Button message
    void OnUserCanelRegist()
    {
        this.HideWindow();
        this.m_UIWindowUserLogin.ShowWindow(false);

    }
    void OnReceivRegist(Hashtable hashtable)
    {
        LockScreen.Instance.EnableInput();
        this.m_Progress.SetActive(false);
        RegisterAccountResponseParameter response = new RegisterAccountResponseParameter();
        response.InitialParameterObjectFromHashtable(hashtable);
        if (response.FailType.HasValue)
        {
            switch (response.FailType.Value)
            {
                case RegisterAccountFailType.AccountNameIsAlreadyRegistered:
                    UIErrorMessage.Instance.ErrorMessage(11);
                    return;
            }
        }
        this.HideWindow();
        this.m_UIWindowConfirmLogin.ShowWindow(StringConstants.PROMT,  m_UILabelEmail.text);
        this.m_UIWindowConfirmLogin.Click += () =>
        {
            this.m_UIWindowUserLogin.ShowWindow(false);
            this.m_UIWindowUserLogin.SetUserInput(this.m_UIInputEmail.text, this.m_UIInputPassword.text);
        };

    }
    void ClearInput()
    {
        m_UIInputEmail.text = "";
        m_UIInputPassword.text = "";
        m_UIInputPasswordConform.text = "";
    }

    void OnSubmitAccount(string text)
    {
        m_UIInputEmail.text = SystemFunction.ReplaceEmoji(text);
        //m_UIInputEmail.selected = false;
        m_UIInputPassword.selected = true;
    }
    void OnSubmitRegistPassword(string text)
    {
        //m_UILabelPassword.text = SystemFunction.ReplaceEmoji(text);
        //m_UIInputPassword.selected = false;
        m_UIInputPasswordConform.selected = true;
    }
    void OnSubmitRegistPasswordConform(string text)
    {
        //m_UILabelConformPassword.text = SystemFunction.ReplaceEmoji(text);
        this.OnUserRegist();
    }
}
