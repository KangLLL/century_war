using UnityEngine;
using System.Collections;

public class NdStart : MonoBehaviour 
{
	private static bool s_IsFirstEnterGame = true;
	private bool m_IsLogin;
	
	void Start () 
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer && s_IsFirstEnterGame)
		{
			Debug.Log("initialize!");
			Bonjour.SetAutoRotation(false);
			Bonjour.SetScreenOrientation(3);
			//Bonjour.ShowToolBar();
			Bonjour.InitializeNdPlatform(NdCenter.Instace.AppID, NdCenter.Instace.AppKey);
			if(NdCenter.Instace.IsDebug)
			{
				Bonjour.DebugMode();
			}
		}
		else if(!s_IsFirstEnterGame)
		{
			this.m_IsLogin = true;
			//this.Initialize();
		}
		else
		{
			s_IsFirstEnterGame = false;
			Application.LoadLevel(ClientStringConstants.INITIAL_SCENE_LEVEL_NAME);
		}
	}
	
	void Update()
	{
		if(this.m_IsLogin)
		{
			if(NdCenter.Instace.CurrentLoginState == LoginState.Success)
			{
				if(s_IsFirstEnterGame)
				{
					s_IsFirstEnterGame = false;
					Application.LoadLevel(ClientStringConstants.INITIAL_SCENE_LEVEL_NAME);
				}
				else
				{
					Application.LoadLevel(ClientStringConstants.LOADING_SCENE_LEVEL_NAME);
				}
			}
			else if (NdCenter.Instace.CurrentLoginState == LoginState.Fail)
			{
				this.Initialize();
			}
		}
	}
	
	public void Initialize()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if(Bonjour.IsLogined())
			{
				Application.LoadLevel(ClientStringConstants.INITIAL_SCENE_LEVEL_NAME);
			}
			else
			{
				NdCenter.Instace.Login();
				this.m_IsLogin = true;
			}
		}
	}
}
