using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public enum LoginState
{
	Logining,
	Success,
	Fail
}

public enum BuyState
{
	Buying,
	Success,
	Fail
}

public class NdCenter : MonoBehaviour 
{
	private static NdCenter s_Sigleton;
	
	public static NdCenter Instace
	{
		get { return s_Sigleton; }
	}
	
	[SerializeField]
	private int m_AppID;
	[SerializeField]
	private string m_AppKey;
	[SerializeField]
	private bool m_IsDebug;
	[SerializeField]
	private string m_NdID;
	
	private LoginState m_LoginState;
	private BuyState m_BuyState;
	private string m_BuyError;
	private bool m_IsInPlatform;
	
	private bool m_IsFirstInGame;
	
	public LoginState CurrentLoginState { get { return this.m_LoginState; } }
	public BuyState CurrentBuyState { get { return this.m_BuyState; } }
	public string BuyError { get { return this.m_BuyError; } }
	public bool IsInPlatform { get { return this.m_IsInPlatform; } }
	public string NdID 
	{
		get
		{
			if(Application.platform == RuntimePlatform.IPhonePlayer)
			{
				return Bonjour.GetPlayerID().ToString();
			}
			else
			{
				return this.m_NdID;
			}
		}
	}
	
	public int AppID { get { return this.m_AppID; } }
	public string AppKey { get { return this.m_AppKey; } }
	public bool IsDebug { get { return this.m_IsDebug; } }

	void Awake () 
	{
		s_Sigleton = this;
		CommonHelper.PlatformType = PlatformType.Nd;
	}
	
	#region Handler
	public void LoginSuccess()
	{
		this.m_LoginState = LoginState.Success;
	}
	
	public void LoginFail()
	{
		this.m_LoginState = LoginState.Fail;
	}
	
	public void BuySuccess()
	{
		this.m_BuyState = BuyState.Success;
	}
	
	public void BuyFail(string error)
	{
		this.m_BuyState = BuyState.Fail;
		this.m_BuyError = error;
	}
	#endregion
	
	#region Public Methods
	public void Login()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if(Bonjour.IsLogined())
			{
				Bonjour.LoginOut();
			}
			Bonjour.Login();
			this.m_LoginState = LoginState.Logining;
		}
	}
	
	public void Logout()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if(Bonjour.IsLogined())
			{
				Bonjour.Enter91();
				this.m_IsInPlatform = true;
			}
		}
	}
	
	public void LeavePlatform()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			this.m_IsInPlatform = false;
		}
	}
	
	public void Buy(string purchaseID, string productID, string name, double price, double originalPrice, int quantity, string description)
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			this.m_BuyError = null;
			if(Bonjour.IsLogined())
			{
				Bonjour.UniPay(purchaseID, productID, name, price, price, quantity, description);
				this.m_BuyState = BuyState.Buying;
			}
			else
			{
				this.m_BuyState = BuyState.Fail;
				this.m_BuyError = "Not Login yet!";
			}
		}
	}
	#endregion
}
