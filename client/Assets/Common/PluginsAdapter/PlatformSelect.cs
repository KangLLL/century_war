using UnityEngine;
using System.Collections;

using ConfigUtilities.Enums;

public class PlatformSelect : MonoBehaviour 
{
	[SerializeField]
	private PlatformType m_Platform;
	
	// Use this for initialization
	void Start () 
	{
		GameObject.DontDestroyOnLoad(this.gameObject);
		if(Application.platform == RuntimePlatform.IPhonePlayer && this.m_Platform == PlatformType.Nd)
		{
			this.gameObject.AddComponent<NdCenter>();
		}
		else
		{
			Application.LoadLevel(ClientStringConstants.LOADING_SCENE_LEVEL_NAME);
		}
	}
}
