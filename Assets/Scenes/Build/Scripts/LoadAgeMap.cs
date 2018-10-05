using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class LoadAgeMap : MonoBehaviour 
{	
	private static LoadAgeMap s_Sigleton;
	
	private GameObject m_CurrentMap;
    public GameObject CurrentMap { get { return m_CurrentMap; } }
	public Age CurrentAge { get; private set; }
	
	public static LoadAgeMap Instance
	{
		get { return s_Sigleton; }
	}
	// Use this for initialization
	
	void Awake()
	{
		s_Sigleton = this;
	}
	
	void Ondestroy()
	{
		s_Sigleton = null;
	}
	
	public void SetMap(Age age)
	{
		if(this.m_CurrentMap != null)
		{
			GameObject.Destroy(this.m_CurrentMap);
		}
		this.CurrentAge = age;
		
		string prefabName = ClientConfigConstants.Instance.GetAgeMapName(age);
		this.m_CurrentMap =  GameObject.Instantiate(Resources.Load(prefabName, typeof(GameObject))) as GameObject;
	}
}
