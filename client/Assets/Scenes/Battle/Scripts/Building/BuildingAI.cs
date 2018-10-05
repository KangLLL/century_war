using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingAI : BattleAI 
{
	[SerializeField]
	private GameObject[] m_ActorObjects;
	private AttackBehavior m_AttackBehavior;
	private BattleSceneHelper m_SceneHelper; 
	
	private BuildingPropertyBehavior m_Property;
	
	public AttackBehavior AttackBehavior 
	{
		get
		{
			return this.m_AttackBehavior;
		} 
		set
		{
			this.m_AttackBehavior = value;
		}
	}
	
	public BattleSceneHelper SceneHelper
	{
		get
		{
			return this.m_SceneHelper;
		}
		set
		{
			this.m_SceneHelper = value;
		}
	}
	
	public BuildingPropertyBehavior Property
	{
		get { return this.m_Property; }
	}
	
	void Awake()
	{
		this.m_Property = this.GetComponent<BuildingPropertyBehavior>();
	}
	
	public void SetIdle(bool isResponseInstantly)
	{
		BuildingIdleState idleState = new BuildingIdleState(this, isResponseInstantly);
		this.ChangeState(idleState);
	}
	
	public void SetTarget(GameObject target, Vector2 targetPosition)
	{
		AITargetObject targetObject = new AITargetObject(target, targetPosition);
		BuildingAttackState attackState = new BuildingAttackState(this, targetObject, this.m_AttackBehavior);
		this.ChangeState(attackState);
	}
	
	public void DetachSelf()
	{
		foreach (GameObject actor in this.m_ActorObjects) 
		{
			GameObject.Destroy(actor);
		}
		GameObject.DestroyImmediate(this);
	}
}
