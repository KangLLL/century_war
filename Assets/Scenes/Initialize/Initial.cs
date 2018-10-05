using UnityEngine;
using System.Collections;

public class Initial : MonoBehaviour 
{
	private const string APP_NAME = "AocServer";
	private const string API_NAME = "api";
	private const string CONTROLLER_NAME = "ServerSite";
	
	[SerializeField]
	private bool m_IsDevelop;
	[SerializeField]
	private string m_GetServerIP;
	[SerializeField]
	private string m_DevelopServerIP;
	
	private WWW m_WWW;
	private bool m_IsGot;
	// Use this for initialization
	void Start () 
	{	
		if(this.m_IsDevelop)
		{
			Debug.Log(this.m_DevelopServerIP);  
			CommonHelper.IsDevelop = true;
			CommunicationUtility.Instance.ConnectToServer(this.m_DevelopServerIP);
			Application.LoadLevel(ClientStringConstants.LOADING_SCENE_LEVEL_NAME);
		}
		else
		{
			this.StartCoroutine("GetServerIP");
		}
	}
	
	IEnumerator GetServerIP()
	{
		string url = string.Format("http://{0}/{1}/{2}/{3}/{4}", this.m_GetServerIP, APP_NAME, API_NAME, CONTROLLER_NAME, ClientVersion.Instance.Version);
		Debug.Log(url);
		this.m_IsGot = false;
		this.m_WWW = new WWW(url);
		yield return this.m_WWW;	
		this.m_IsGot = true;
	}
	
	void Update()
	{
		if(this.m_IsGot && this.m_WWW != null)
		{
			if(string.IsNullOrEmpty(this.m_WWW.error))
			{
				Debug.Log(this.m_WWW.text);
				CommunicationUtility.Instance.ConnectToServer(this.m_WWW.text);
				Application.LoadLevel(ClientStringConstants.LOADING_SCENE_LEVEL_NAME);
			}
			else
			{
				Debug.Log(this.m_WWW.error);
				this.StartCoroutine("GetServerIP");
			}
		}
	}
	
	
}
