using UnityEngine;
using System.Collections;
using ConfigUtilities;

public class HPBehavior : MonoBehaviour 
{
	[SerializeField]
	protected GameObject m_DeadEffectPrefab1;
	[SerializeField]
	protected GameObject m_DeadEffectPrefab2;
	[SerializeField]
	private HPBarBehavior m_HPBarBehavior;
	[SerializeField]
	private GameObject[] m_DoNotDestoryGameObjects;
	[SerializeField]
	private string m_DeadSound;
	
	protected int m_TotalHP;
	protected int m_CurrentHP;
	private int m_ArmorCategory;
	
	protected bool m_IsInitialized;
	
	public int TotalHP 
	{ 
		get
		{
			return this.m_TotalHP;
		}
		set
		{
			this.m_TotalHP = value; 
		}
	}
	
	public int ArmorCategory
	{
		get
		{
			return this.m_ArmorCategory;
		}
		set
		{
			this.m_ArmorCategory = value;
		}
	}
	
	public int CurrentHP { get { return this.m_CurrentHP; } }
	
	public virtual void Start()
	{
		this.m_CurrentHP = this.TotalHP;
		this.m_IsInitialized = true;
	}
	
	public void DecreaseHP(int damage, int attackCategory)
	{
		if(BattleDirector.Instance == null || !BattleDirector.Instance.IsBattleFinished)
		{
			int result = (int)(damage * 
				ConfigInterface.Instance.AttackRelationshipConfigHelper.GetEffect(attackCategory,this.m_ArmorCategory));
			this.m_CurrentHP -= result;
			this.m_CurrentHP = Mathf.Max(0, this.m_CurrentHP); 
			if(this.m_CurrentHP != 0)
			{
				this.m_HPBarBehavior.DisplayHPBar();
			}
			this.OnHurt(result);
		}
	}
	
	protected virtual void FixedUpdate()
	{
		if(this.CurrentHP == 0 && 
		   (BattleDirector.Instance == null || !BattleDirector.Instance.IsBattleFinished))
		{
			this.OnDead();
		}
	}
	
	protected virtual void OnDead()
	{
		AudioController.Play(this.m_DeadSound);
		
		GameObject ruins = BattleObjectCache.Instance.RuinsObjectParent;
		BuildingBasePropertyBehavior property = this.GetComponent<BuildingBasePropertyBehavior>();
		
		if(this.m_DeadEffectPrefab1 != null)
		{
			GameObject deadEffect = GameObject.Instantiate(this.m_DeadEffectPrefab1) as GameObject;
			deadEffect.transform.position = property == null ? this.transform.position 
				: property.AnchorTransform.position;
			
			deadEffect.transform.parent = ruins.transform;
		}
		if(this.m_DeadEffectPrefab2 != null)
		{
			GameObject deadEffect = GameObject.Instantiate(this.m_DeadEffectPrefab2) as GameObject;
			deadEffect.transform.position = property == null ? this.transform.position 
				: property.AnchorTransform.position;
			
			deadEffect.transform.parent = ruins.transform;
		}
		
		foreach(GameObject go in this.m_DoNotDestoryGameObjects)
		{
			if(go.transform.IsChildOf(this.transform))
			{
				go.transform.parent = ruins.transform;
			}
		}
		
		GameObject.DestroyObject(this.gameObject);
	}
	
	protected virtual void OnHurt(int damage)
	{
	}
}
