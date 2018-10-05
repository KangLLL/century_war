using UnityEngine;
using System.Collections;
using CommandConsts;

public enum AccountOperationType
{
	Mount,
	Switch,
	Logout
}

public class NdAccountUtility : AccountUtility 
{
	private static NdAccountUtility s_Sigleton;
	
	private bool m_IsLogin;
	private bool m_IsLogout;
	private AccountOperationType m_CurrrentOperation;
	
	public static NdAccountUtility Instance
	{
		get
		{
			return s_Sigleton;
		}
	}
	
	public override void Awake ()
	{
		base.Awake ();
		s_Sigleton = this;
	}
	
	void Update()
	{
		if(this.m_IsLogin)
		{
			if(NdCenter.Instace.CurrentLoginState == LoginState.Success)
			{
				if(this.m_CurrrentOperation == AccountOperationType.Mount)
				{
					this.SendMountRequest();
				}
				if(this.m_CurrrentOperation == AccountOperationType.Switch)
				{
					this.SendSwitchRequest();
				}
				this.m_IsLogin = false;
			}
			else if(NdCenter.Instace.CurrentLoginState == LoginState.Fail)
			{
				if(this.m_CurrrentOperation == AccountOperationType.Mount)
				{
					this.m_MountFailListener.Invoke(MountFailType.NotLogin);
				}
				if(this.m_CurrrentOperation == AccountOperationType.Switch)
				{
					this.m_SwitchFailListener.Invoke(SwitchFailType.NotLogin);
				}
				this.m_IsLogin = false;
			}
		}
		else if(this.m_IsLogout)
		{
			if(!NdCenter.Instace.IsInPlatform)
			{
				this.m_LogoutFailListener.Invoke(true);
				this.m_IsLogout = false;
			}
			if(NdCenter.Instace.CurrentLoginState == LoginState.Fail)
			{
				this.m_LogoutSuccessListener.Invoke(true);
				this.m_IsLogout = false;
			}
		}
	}
	
	public void Mount()
	{
		this.m_CurrrentOperation = AccountOperationType.Mount;
		if(Bonjour.IsLogined())
		{
			this.SendMountRequest();
		}
		else
		{
			NdCenter.Instace.Login();
			this.m_IsLogin = true;
		}
	}
	
	public void Switch()
	{
		this.m_CurrrentOperation = AccountOperationType.Switch;
		if(Bonjour.IsLogined())
		{
			this.SendSwitchRequest();
		}
		else
		{
			NdCenter.Instace.Login();
			this.m_IsLogin = true;
		}
	}
	
	public void Logout()
	{
		this.m_CurrrentOperation = AccountOperationType.Logout;
		this.m_IsLogout = true;
		NdCenter.Instace.Logout();
	}
	
	private void SendMountRequest()
	{
		string uid = Bonjour.GetPlayerID().ToString();
		MountAccountRequestParameter request = new MountAccountRequestParameter();
		request.AccountID = uid;
		CommunicationUtility.Instance.MountAccount(request, this, "OnMountResponse", true);
	}
	
	private void SendSwitchRequest()
	{
		string uid = Bonjour.GetPlayerID().ToString();
		SwitchAccountRequestParameter request = new SwitchAccountRequestParameter();
		request.AccountID = uid;
		CommunicationUtility.Instance.SwitchAccount(request, this, "OnSwitchResponse", true);
	}
	
	private void OnMountResponse(Hashtable response)
	{
		MountAccountResponseParameter param = new MountAccountResponseParameter();
		param.InitialParameterObjectFromHashtable(response);
		if(param.FailType.HasValue)
		{
			this.m_MountFailListener.Invoke((MountFailType)((int)param.FailType.Value));
		}
		else
		{
			string uid = Bonjour.GetPlayerID().ToString();
			this.MountAccount(uid);
		}
	}
	
	private void OnSwitchResponse(Hashtable response)
	{
		SwitchAccountResponseParameter param = new SwitchAccountResponseParameter();
		param.InitialParameterObjectFromHashtable(response);
		
		if(param.FailType.HasValue)
		{
			this.m_SwitchFailListener.Invoke((SwitchFailType)((int)param.FailType.Value));
		}
		else
		{
			Application.LoadLevel(ClientStringConstants.LOADING_SCENE_LEVEL_NAME);
		}
	}
}
