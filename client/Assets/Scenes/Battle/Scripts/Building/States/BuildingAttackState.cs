using UnityEngine;
using System.Collections;

public class BuildingAttackState : AttackState 
{
	public BuildingAttackState(NewAI aiBehavior, AITargetObject target, AttackBehavior attackBehavior)
		: base(aiBehavior, target, attackBehavior)
	{
	}
	
	protected override Vector2 SelfPosition 
	{
		get 
		{
			return (Vector2)(((BuildingAI)this.m_AIBehavior).Property.AnchorTransform.position);
		}
	}
	
	protected override void OnTargetLost ()
	{
		BuildingIdleState idleState = new BuildingIdleState(this.m_AIBehavior, false);
		this.m_AIBehavior.ChangeState(idleState);
	}
	
}
