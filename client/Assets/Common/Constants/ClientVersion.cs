using UnityEngine;
using System.Collections;

public class ClientVersion : MonoBehaviour 
{
	private static ClientVersion s_Sigleton;
	
	public static ClientVersion Instance
	{
		get { return s_Sigleton; }
	}
	
	[SerializeField]
	private string m_Version;
	
	public string Version
	{
		get { return this.m_Version; }
	}
	
	void Awake()
	{
		s_Sigleton = this;
	}
}
