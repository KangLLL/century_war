using UnityEngine;
using System.Collections;

public class BattleEffectConfig : MonoBehaviour 
{
	[SerializeField]
	private GameObject m_TargetEffectPrefab;
	
	public GameObject TargetEffectPrefab
	{
		get { return this.m_TargetEffectPrefab; }
	}
	
	/*
	[SerializeField]
	private GameObject[] m_PlunderGoldPrefab;
	[SerializeField]
	private GameObject[] m_PlunderFoodPrefab;
	[SerializeField]
	private GameObject[] m_PlunderOilPrefab;
	
	public GameObject[] PlunderGoldPrefab
	{
		get { return this.m_PlunderGoldPrefab; }
	}
	
	public GameObject[] PlunderFoodPrefab
	{
		get { return this.m_PlunderFoodPrefab; } 
	}
	
	public GameObject[] PlunderOilPrefab
	{
		get { return this.m_PlunderOilPrefab; } 
	}
	*/
	
	
	private static BattleEffectConfig s_Sigleton;
	
	public static BattleEffectConfig Instance
	{
		get { return s_Sigleton; }
	}

	void Awake () 
	{
		s_Sigleton = this;
	}

	void OnDestroy()
	{
		s_Sigleton = null;
	}
	
}
