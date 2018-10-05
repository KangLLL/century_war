using UnityEngine;
using System.Collections;

public class BattleObjectCache : MonoBehaviour 
{
	[SerializeField]
	private GameObject m_EffectObjectParent;
	[SerializeField]
	private GameObject m_RuinsObjectParent;
	
	private static BattleObjectCache s_Sigleton;
	
	public static BattleObjectCache Instance
	{
		get { return s_Sigleton; }
	}
	
	void Awake()
	{
		s_Sigleton = this;
	}
	
	void OnDestroy()
	{
		s_Sigleton = null;
	}
	
	public GameObject EffectObjectParent
	{
		get { return this.m_EffectObjectParent; }
	}
	
	public GameObject RuinsObjectParent
	{
		get { return this.m_RuinsObjectParent; }
	}
	
}
