using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackBehavior : MonoBehaviour 
{	
	private AttackConfig m_AttackConfig;
	
	private int m_AttackValue;
	private int m_AttackCD;
	private float m_AttackScope;
	private float m_BulletFlySpeed;
	private AttackType m_AttackType;
	private int m_AttackCategory;
	private TargetType m_TargetType;
	
	private int m_DamageScope;
	private int m_PushTicks;
	private int m_PushVelocity;
	
	private Transform m_BulletParent;
	
	public int AttackValue 
	{ 
		get
		{
			return this.m_AttackValue;
		}
		set
		{
			this.m_AttackValue = value;
		}
	}
	public int AttackCD 
	{ 
		get
		{
			return this.m_AttackCD;
		}
		set
		{
			this.m_AttackCD = value;
		}
	}
	public float AttackScope 
	{  
		get
		{
			return this.m_AttackScope;
		}
		set
		{
			this.m_AttackScope = value;
		}
	}
	
	public AttackType AttackType
	{
		get
		{
			return this.m_AttackType;
		}
		set
		{
			this.m_AttackType = value;
		}
	}
	
	public TargetType TargetType
	{
		get
		{
			return this.m_TargetType;
		}
		set
		{
			this.m_TargetType = value;
		}
	}
	
	public int AttackCategory
	{
		get
		{
			return this.m_AttackCategory;
		}
		set
		{
			this.m_AttackCategory = value;
		}
	}
	
	public float BulletFlySpeed
	{
		get
		{
			return this.m_BulletFlySpeed;
		}
		set
		{
			this.m_BulletFlySpeed = value;
		}
	}
	
	public int DamageScope
	{
		get
		{
			return this.m_DamageScope;
		}
		set
		{
			this.m_DamageScope = value;
		}
	}
	
	public int PushTicks
	{
		get
		{
			return this.m_PushTicks;
		}
		set
		{
			this.m_PushTicks = value;
		}
	}
	
	public int PushVelocity
	{
		get	
		{
			return this.m_PushVelocity;
		}
		set
		{
			this.m_PushVelocity = value;
		}
	}
	
	private float m_AttackScopeSqr;
	private List<TilePosition> m_AttackScopeArray = new List<TilePosition>();
	
	public float AttackScopeSqr
	{
		get
		{
			return this.m_AttackScopeSqr;
		}
	}
	
	public List<TilePosition> AttackScopeArray
	{
		get
		{
			return this.m_AttackScopeArray;
		}
	}

	public virtual void Start()
	{
		this.m_AttackConfig = this.GetComponent<AttackConfig>();
		this.m_AttackScopeSqr = this.AttackScope * this.AttackScope;
		this.m_AttackScopeArray = RoundHelper.FillCircle(0,0,Mathf.CeilToInt(this.AttackScope / 
			Mathf.Min(ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.width, 
			ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.height)));
	}
	
	public Transform BulletParent 
	{ 
		get 
		{ 
			return this.m_BulletParent; 
		} 
		set
		{
			this.m_BulletParent = value;
		}
	}
	
	public virtual void Fire(GameObject target, Vector2 targetPosition)
	{
		AudioController.Play(this.m_AttackConfig.AttackSound);
		
		GameObject attackObject = GameObject.Instantiate(this.m_AttackConfig.AttackObjectPrefab) as GameObject;
		AttackObjectBehavior behavior = null;
		if(this.AttackType == AttackType.Single)
		{
			behavior = attackObject.AddComponent<AttackObjectBehavior>();
			
		}
		else
		{
			behavior = attackObject.AddComponent<GroupAttackObjectBehavior>();
		}
		
		BuildingBasePropertyBehavior property = this.GetComponent<BuildingBasePropertyBehavior>();
		Vector2 deltaPosition = property == null ? targetPosition - (Vector2)transform.position : 
			targetPosition - (Vector2)property.AnchorTransform.position;
		CharacterDirection direction = DirectionHelper.GetDirectionFormVector(deltaPosition);
		
		Vector2 offset = Vector2.zero;
		
		switch(direction)
		{
			case CharacterDirection.Up:
			{
				offset = this.m_AttackConfig.AttackUpOffset;
			}
			break;
			case CharacterDirection.Down:
			{
				offset = this.m_AttackConfig.AttackDownOffset;
			}
			break;
			case CharacterDirection.Left:
			{
				offset = this.m_AttackConfig.AttackLeftOffset;
			}
			break;
			case CharacterDirection.Right:
			{
				offset = this.m_AttackConfig.AttackRightOffset;
			}
			break;
			case CharacterDirection.LeftUp:
			{
				offset = this.m_AttackConfig.AttackLeftUpOffset;
			}
			break;
			case CharacterDirection.LeftDown:
			{
				offset = this.m_AttackConfig.AttackLeftDownOffset;
			}
			break;
			case CharacterDirection.RightUp:
			{
				offset = this.m_AttackConfig.AttackRightUpOffset;
			}
			break;
			case CharacterDirection.RightDown:
			{
				offset = this.m_AttackConfig.AttackRightDownOffset;
			}
			break;
		}
		
		Vector2 firePosition = property == null ? (Vector2)this.transform.position : 
			(Vector2)property.AnchorTransform.position;
		
		attackObject.transform.position = new Vector3((firePosition + offset).x, (firePosition + offset).y, 0);
		behavior.SourceObject = gameObject;
		behavior.SetDestinationObject(target, targetPosition, firePosition);
		if(behavior is GroupAttackObjectBehavior)
		{
			((GroupAttackObjectBehavior)behavior).DamageScope = this.m_DamageScope;
		}
		
		behavior.Velocity = this.m_BulletFlySpeed;
		behavior.Damage = this.AttackValue;
		behavior.AttackCategory = this.m_AttackCategory;
		behavior.PushTicks = this.m_PushTicks;
		behavior.PushVelocity = this.m_PushVelocity;
		
		if(this.BulletParent != null)
		{
			attackObject.transform.parent = this.BulletParent;
		}
		
		for(int i = 0; i < this.m_AttackConfig.FireEffectPrefabs.Count; i ++) 
		{
			GameObject prefab = this.m_AttackConfig.FireEffectPrefabs[i];
			offset = this.m_AttackConfig.FireEffectOffsets[i].GetOffset(direction);
			GameObject fireEffect = GameObject.Instantiate(prefab) as GameObject;
			fireEffect.transform.position = property == null ? this.transform.position + new Vector3(offset.x, offset.y, 0) :
				property.AnchorTransform.position + new Vector3(offset.x, offset.y, 0);
			fireEffect.transform.parent = BattleObjectCache.Instance.EffectObjectParent.transform;
		}
	}
}
