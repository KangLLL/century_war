using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DefenseObjectBattleBehavior : MonoBehaviour 
{
	[SerializeField]
	private DefenseObjectPropertyBehavior m_Property;
	[SerializeField]
	private GameObject m_DisplayObject;
	[SerializeField]
	private string m_EffectSound;
	
	private int m_Damage;
	private int m_AttackCategory;
	
	private int m_Scope;
	private int m_TriggerTick;
	private int m_TriggerScope;
	private int m_TriggerScopeSqrt;

	private TargetType m_TargetType;
	private List<TilePosition> m_TriggerList;
	private List<TilePosition> m_DamageList;
	
	public DefenseObjectPropertyBehavior Property
	{
		get { return this.m_Property; }
	}
	
	public int Damage
	{
		get { return this.m_Damage; }
		set { this.m_Damage = value; }
	}
	
	public int Scope
	{
		get { return this.m_Scope; }
		set { this.m_Scope = value; }
	}
	
	public int TriggerTick
	{
		get { return this.m_TriggerTick; } 
		set { this.m_TriggerTick = value; }
	}
	
	public int TriggerScope
	{
		get { return this.m_TriggerScope; } 
		set { this.m_TriggerScope = value; }
	}
	
	public int TriggerScopeSqrt
	{
		get { return this.m_TriggerScopeSqrt; }
	}
	
	public List<TilePosition> TriggerList
	{
		get 
		{
			if(this.m_TriggerList == null)
			{
				this.m_TriggerList = this.GetTilePositionList(this.m_TriggerScope);
			}
			return this.m_TriggerList; 
		} 
	}
	
	public List<TilePosition> DamageList
	{
		get
		{
			if(this.m_DamageList == null)
			{
				this.m_DamageList = this.GetTilePositionList(this.m_Scope);
			}
			return this.m_DamageList;
		}
	}
	
	public TargetType TargetType 
	{
		get{ return this.m_TargetType; }
		set{ this.m_TargetType = value; }
	}
	
	public int AttackCategory
	{
		get { return this.m_AttackCategory; }
		set { this.m_AttackCategory = value; }
	}
	
	public string EffectSound { get { return this.m_EffectSound; } }
	
	public virtual void Start()
	{
		this.m_TriggerScopeSqrt = this.m_TriggerScope * this.m_TriggerScope;
	}
	
	private List<TilePosition> GetTilePositionList(int scope)
	{
		TilePosition actorPosition = PositionConvertor.GetActorTileIndexFromWorldPosition
			(this.m_Property.AnchorTransform.position);
		int radius = Mathf.CeilToInt(scope /  (float)Mathf.Min(ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.width ,
			ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.height ));
		return RoundHelper.FillCircle(actorPosition.Column, actorPosition.Row, radius);
	}
	
	public virtual void Effect()
	{
	}
	
	public void DisplayTriggerAnimation()
	{
		this.m_DisplayObject.SetActive(true);
	}
}
