using UnityEngine;
using System.Collections;
using System;

public class AttackObjectBehavior : MonoBehaviour 
{
	private GameObject m_Source;
	private IAttackObjectTarget m_Destination;
	
	private AttackObjectState m_CurrentState;
	
	private int m_Damage;
	private AttackObjectConfig m_Config;
	
	private float m_Velocity;
	private float m_VelocitySqr;
	private int m_AttackCategory;
	
	private GameObject m_TargetObject;
	private Vector2 m_TargetPosition;
	private Vector2 m_FirePosition;
	
	private int m_PushTicks;
	private int m_PushVelocity;
	
	public int Damage 
	{ 
		protected get
		{
			return this.m_Damage;
		}
		set
		{
			this.m_Damage = value;
		}
	}
	
	public GameObject SourceObject
	{
		get
		{
			return this.m_Source;
		}
		set
		{
			this.m_Source = value;
		}
	}
	
	public IAttackObjectTarget DestinationObject
	{
		get
		{
			return this.m_Destination;
		}
	}
	
	public float Velocity
	{
		get
		{
			return this.m_Velocity;
		}
		set
		{
			this.m_Velocity = value;
			this.m_VelocitySqr = value * value;
		}
	}
	
	public float VelocitySqr
	{
		get
		{
			return this.m_VelocitySqr;
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
	
	public void ChangeState(AttackObjectState newState)
	{
		if(newState != null)
		{
			newState.Behavior = this;
		}
		else
		{
			GameObject.DestroyObject(gameObject);
		}
		
		if(newState is EndState || 
			(newState == null && !(this.m_CurrentState is EndState)))
		{
			this.Calculate();
		}
		this.m_CurrentState = newState;
	}
	
	// Use this for initialization
	public virtual void Start () 
	{
		this.m_Config = this.GetComponent<AttackObjectConfig>();
		
		if(this.m_Config.PathType == AttackObjectPathType.TargetObject)
		{
			this.m_Destination = new TargetObject(this.m_TargetObject, this.m_TargetPosition, this.m_FirePosition);
		}
		else
		{
			this.m_Destination = new DestinationTarget(this.m_TargetPosition);
		}
		
		Vector2 destinationPosition = this.m_Destination.GetDestinationPosition(this.transform.position);
		Vector2 delta = destinationPosition - (Vector2)this.transform.position;
		
		if(this.m_Config.SpriteAnimator.GetClipByName(AnimationNameConstants.MIDDLE) != null)
		{
			this.transform.rotation = Quaternion.FromToRotation(Vector3.right, 
				new Vector3(delta.x, delta.y,0));
		}
		
		if(this.m_Config.SpriteAnimator.GetClipByName(AnimationNameConstants.START) != null)
		{
			this.ChangeState(new AttackObjectStartState());
		}
		else if(this.m_Config.SpriteAnimator.GetClipByName(AnimationNameConstants.MIDDLE) != null ||
			this.m_Config.SpriteAnimator.GetClipByName(AnimationNameConstants.MIDDLE_UP) != null)
		{
			if(this.m_Destination is TargetObject && !((TargetObject)this.m_Destination).Target.IsStaticTarget())
			{
				this.ChangeState(new AttackObjectTraceMiddleState());
			}
			else
			{
				this.ChangeState(new AttackObjectMiddleState());
			}
		}
		else if(this.m_Config.EndPrefab != null)
		{
			this.ChangeState(new AttackObjectPrefabEndState());
		}
		else if(this.m_Config.SpriteAnimator.GetClipByName(AnimationNameConstants.END) != null)
		{
			this.ChangeState(new AttackObjectEndState());
		}
		else
		{
			GameObject.DestroyObject(gameObject);
			this.Calculate();
		}
	}
	
	void FixedUpdate()
	{
		this.m_CurrentState.ExecuteLogic();
	}
	
	public void SetDestinationObject(GameObject target, Vector2 targetPosition, Vector2 firePosition)
	{
		this.m_TargetObject = target;
		this.m_TargetPosition = targetPosition;
		this.m_FirePosition = firePosition;
	}
	
	protected virtual void Calculate()
	{
		if(this.m_Destination is TargetObject && ((TargetObject)this.m_Destination).Target != null)
		{
			HPBehavior hpBehavior = ((TargetObject)this.m_Destination).Target.GetComponent<HPBehavior>();
			hpBehavior.DecreaseHP(this.Damage, this.m_AttackCategory);
			
			this.PushCharacter(((TargetObject)this.m_Destination).Target);
		}
	}
	
	protected void PushCharacter(GameObject character)
	{
		if(this.m_PushTicks > 0 && this.m_PushVelocity > 0)
		{
			CharacterAI aiBehavior = character.GetComponent<CharacterAI>();
			if(aiBehavior != null)
			{
				aiBehavior.SetPush(this.m_PushTicks, this.m_PushVelocity, this.transform.position);
			}
		}
	}
	
	void OnDestroy()
	{
		this.m_TargetObject = null;
	}
}
