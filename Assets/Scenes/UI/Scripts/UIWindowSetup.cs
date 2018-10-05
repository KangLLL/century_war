using UnityEngine;
using System.Collections;

using ConfigUtilities.Enums;

public class UIWindowSetup : UIWindowCommon {
    //[SerializeField] GameObject m_RegistButton;
    [SerializeField] UILabel m_UILabelAccount;
	
	private bool m_IsLogout;
	
    void Awake()
    {
        this.GetTweenComponent();
    } 
    public override void HideWindow()
    {

        base.HideWindow();
    }
    public override void ShowWindow()
    {
        this.SetWindowItem();
        base.ShowWindow();
    }
    protected override void GetTweenComponent()
    {
        base.GetTweenComponent();
    }
    /*
    void SetWindowItem()
    {

        if (NdAccountUtility.Instance.IsMounted)
        {
            this.m_RegistButton.SetActive(false);
            this.m_UILabelAccount.gameObject.SetActive(true);
            this.m_UILabelAccount.text = StringConstants.PROMPT_RECORD_ALREADY_BOUNDED + '\n' + NdAccountUtility.Instance.MountedAccount;
        }
        else
        {
            this.m_RegistButton.SetActive(true);
            this.m_UILabelAccount.gameObject.SetActive(false);

        }
       
    }
    */
    void SetWindowItem()
    {
        switch (CommonHelper.PlatformType)
        {
            case PlatformType.iOS:
                this.m_UILabelAccount.text = StringConstants.TITLE_EXCHANGE_ACCOUNT;
                break;
            case PlatformType.Nd:
                this.m_UILabelAccount.text = StringConstants.PROMPT_91_ACCOUNT;
                break;
        }
    }
    //Button message
    void OnSwitchAccount()
    {

        switch (CommonHelper.PlatformType)
        {
            case PlatformType.iOS:
                if (base.ControlerFocus != null)
                    return;
                else
                    base.ControlerFocus = this.gameObject;
                this.HideWindow(); 
                UIManager.Instance.UIWindowConfirmPrompt.UnRegistDelegate();
                UIManager.Instance.UIWindowConfirmPrompt.Click += () =>
                {
                    PlayerPrefs.DeleteKey("Password");
                    //PlayerPrefs.DeleteKey("Account");
                    iOSCenter.Instance.AccountID = -1;
					CommunicationUtility.Instance.LogoutAccount();
                    Application.LoadLevel(ClientStringConstants.LOADING_SCENE_LEVEL_NAME);
                    
                };
                UIManager.Instance.UIWindowConfirmPrompt.ShowWindow(StringConstants.PROMT, StringConstants.PROMT_EXCHANGE_ACCOUNT_PROMT2, false);
                break;
            case PlatformType.Nd:
                //this.HideWindow();
				LockScreen.Instance.DisableInput();
                NdAccountUtility.Instance.RegisterLogoutFailReceiver(this, "OnLogoutFailReceiver");
				NdAccountUtility.Instance.RegisterLogoutSuccessReceiver(this, "OnLogoutSuccessReceiver");
                NdAccountUtility.Instance.Logout();
               
                break;
        }

    }
    //Button message
    void OnRegistAccount()
    {
       
        switch (CommonHelper.PlatformType)
        {
            case PlatformType.iOS:
                UIManager.Instance.UIWindowAccount.Click += (sender, e) => { if ((sender as UIWindowAccount).CheckUserRegistAccount()) NdAccountUtility.Instance.Mount(); };
                NdAccountUtility.Instance.RegisterMountSuccessReceiver(this, "OnRegisterMountSuccessReceiver");
                NdAccountUtility.Instance.RegisterMountFailReceiver(this, "OnRegisterMountFailReceiver");
                UIManager.Instance.UIWindowAccount.ShowWindow(StringConstants.TITLE_REGIST_ACCOUNT);
                this.HideWindow();
                break;
            case PlatformType.Nd:
                NdAccountUtility.Instance.RegisterMountSuccessReceiver(this, "OnRegisterMountSuccessReceiver");
                NdAccountUtility.Instance.RegisterMountFailReceiver(this, "OnRegisterMountFailReceiver");
                NdAccountUtility.Instance.Mount();
                break;
        }

    }
	
	void OnLogoutFailReceiver(object info)
	{
		LockScreen.Instance.EnableInput();
	}
	
	void OnLogoutSuccessReceiver(object info)
	{
		AlertView.ShowAlertView(ClientStringConstants.LOG_OUT_DIALOG_TITLE , ClientStringConstants.LOG_OUT_DIALOG_DESCRIPTION,
			ClientStringConstants.LOG_OUT_DIALOG_OK_BUTTON_TITLE);
		this.m_IsLogout = true;
	}
	
	void Update()
	{
		if(this.m_IsLogout)
		{
			if(!AlertView.AlertViewIsShown)
			{
				Application.Quit();
			}
		}
	}
	
    void OnRegisterSwitchFailReceiver(object mountFailType)
    {
        switch ((SwitchFailType)mountFailType)
        {
            case SwitchFailType.AccountIsNotMounted:
                UIErrorMessage.Instance.ErrorMessage(11);
                break;
            case SwitchFailType.NotLogin:
                UIErrorMessage.Instance.ErrorMessage(13);
                break;
        }
        print(((SwitchFailType)mountFailType).ToString());
    }
    void OnRegisterMountSuccessReceiver()
    {
        this.HideWindow();
        UIManager.Instance.UIWindowConfirmPrompt.ShowWindow(StringConstants.PROMT, string.Format(StringConstants.PROMPT_REGIST_SUCCESS, NdAccountUtility.Instance.MountedAccount,true));
    }
    void OnRegisterMountFailReceiver(object mountFailType)
    {
        switch ((MountFailType)mountFailType)
        {
            case MountFailType.AccountIsAlreadyMounted:
                UIErrorMessage.Instance.ErrorMessage(11);
                break;
            case MountFailType.NotLogin:
                UIErrorMessage.Instance.ErrorMessage(13);
                break;
            case MountFailType.PlayerIsAlreadyMounted:
                break;
        }
        print(((MountFailType)mountFailType).ToString());
    }
 
}
