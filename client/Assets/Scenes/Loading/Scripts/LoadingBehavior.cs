using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using CommonUtilities;
using CommandConsts;
using ConfigUtilities;

public class LoadingBehavior : MonoBehaviour 
{
	[SerializeField]
	private UserIntializeBehavior m_UserIntializer;
	[SerializeField]
	private GameObject m_ProgressBar;
    [SerializeField]
    private UIWindowUserLogin m_UIWindowUserLogin;
	private const string CONFIG_PATH = "ConfigTable.zip";
	
	private byte[] m_ServerConfigTableMD5;
	private string m_ServerVersion;
	private string m_UpdateUrl;
	
	private bool m_IsOpenUrl;
	private WWW m_wwwConfigTable;
	
	private bool IsGetServerMD5
	{
		get
		{
			return this.m_ServerConfigTableMD5 != null;
		}
	}
	
	public bool IsGetServerVersion
	{
		get
		{
			return !string.IsNullOrEmpty(this.m_ServerVersion);
		}
	}
	
	// Use this for initialization
	void Start () 
	{
		Time.timeScale = 1;
		LogicController.Instance.Destory();
		this.StartCoroutine("Initialize");
        //AudioController.PlayMusic("Loading");
		this.m_ProgressBar.SetActive(false);
	}
	
	void Update()
	{
		if(!string.IsNullOrEmpty(this.m_ServerVersion) && !this.m_ServerVersion.Equals(ClientVersion.Instance.Version) && 
			!AlertView.AlertViewIsShown && !this.m_IsOpenUrl)
		{
			this.m_IsOpenUrl = true;
			this.m_ProgressBar.SetActive(false);
			Application.OpenURL(this.m_UpdateUrl);
			Application.Quit();
		}
	}
	
	IEnumerator Initialize()
	{
		CommunicationUtility.Instance.DisconnectToServer();
		CommunicationUtility.Instance.ConnectToServer();
		
		while(!CommunicationUtility.Instance.IsConnectedToServer)
		{
			yield return null;
		}
		VersionRequestParameter request = new VersionRequestParameter();
		request.PlatformType = CommonHelper.PlatformType;
		CommunicationUtility.Instance.GetVersion(this,"ReceivedVersion",true,request);   
		while(!this.IsGetServerVersion)
		{
			yield return null;
		}
		if(CommonHelper.PlatformType == ConfigUtilities.Enums.PlatformType.iOS)
		{
			if(iOSCenter.Instance.AccountID < 0)
			{
                this.m_UIWindowUserLogin.ShowWindow();
			}
			while(iOSCenter.Instance.AccountID < 0)
			{
				yield return null;
			}
		}
		AudioController.PlayMusic("Loading");
		this.m_ProgressBar.SetActive(true);
		CommunicationUtility.Instance.GetConfigTableMD5(this,"ReceivedConfigTableMD5",true);
		while(!this.IsGetServerMD5)
		{
			yield return null;
		}
		this.LoadConfigTable();
		byte[] localMD5 = null;
		if(this.m_wwwConfigTable != null)
		{
			while(!this.m_wwwConfigTable.isDone)
			{
				yield return null;
			}
			MemoryStream ms = new MemoryStream(this.m_wwwConfigTable.bytes);
			localMD5 = MD5.Create().ComputeHash(ms);
			ms.Close();
		}
		if(localMD5 == null || !localMD5.IsEqualByteArray(this.m_ServerConfigTableMD5))
		{
			string configUrl = string.Format(DataResource.CONFIG_TABLE_URL, ClientVersion.Instance.Version);
			this.m_wwwConfigTable = new WWW(configUrl);
			yield return this.m_wwwConfigTable;
		}
		
		FileStream fileStream = new FileStream(Application.persistentDataPath + "/" + CONFIG_PATH, FileMode.Create);
		BinaryWriter writer = new BinaryWriter(fileStream);
		writer.Write(this.m_wwwConfigTable.bytes);
		writer.Close();
		
		MemoryStream compressedStream = new MemoryStream(this.m_wwwConfigTable.bytes);
		MemoryStream uncompressedStream = new MemoryStream();
		CompressionUtility.DecompressStream(compressedStream, uncompressedStream);
		
		BinaryFormatter bft = new BinaryFormatter();
		DataResource.Resource = (DataSet)bft.Deserialize(uncompressedStream);
		/*
		print(DataResource.Resource.Tables.Count);
		print(DataResource.Resource.Tables[0].Rows.Count);
		print(DataResource.Resource.Tables[1].Rows.Count);
		print(DataResource.Resource.Tables[2].Rows.Count);
		*/
		
		compressedStream.Close();
		uncompressedStream.Close();
		
		if(CommonHelper.PlatformType == ConfigUtilities.Enums.PlatformType.iOS && 
			Application.platform == RuntimePlatform.IPhonePlayer)
		{
			((iOSShopUtility)(iOSShopUtility.Instance)).RequestProduct();
		}
		
		this.m_UserIntializer.StartInitialize();
	}
	
	private void ReceivedConfigTableMD5(Hashtable result)
	{
		ConfigTableMD5ResponseParameter parameter = new ConfigTableMD5ResponseParameter();
		parameter.InitialParameterObjectFromHashtable(result);
		this.m_ServerConfigTableMD5 = parameter.MD5;
		//print("received");
	}
	
	private void ReceivedVersion(Hashtable result)
	{
		VersionResponseParameter parameter = new VersionResponseParameter();
		parameter.InitialParameterObjectFromHashtable(result);
		this.m_ServerVersion = parameter.CurrentVersion;
		this.m_UpdateUrl = parameter.UpdateUrl;
		if(!ClientVersion.Instance.Version.Equals(parameter.CurrentVersion))
		{
			this.StopCoroutine("Initialize");
			if(Application.platform == RuntimePlatform.IPhonePlayer)
			{
				AlertView.ShowAlertView(ClientStringConstants.VERSION_ERROR_TITLE, ClientStringConstants.VERSION_ERROR_DESCRIPTION,
					ClientStringConstants.VERSION_ERROR_OK_BUTTON_TITLE);
			}
			else
			{
				Debug.Log(ClientStringConstants.VERSION_ERROR_DESCRIPTION);
				Debug.Log(this.m_UpdateUrl);
			}
		}
	}
	
	private void LoadConfigTable()
	{
		string configTablePath = Application.persistentDataPath + "/" + CONFIG_PATH;
		
		if(File.Exists(configTablePath))
		{
			this.m_wwwConfigTable = new WWW("file:///" + configTablePath);
		}
	}
}
