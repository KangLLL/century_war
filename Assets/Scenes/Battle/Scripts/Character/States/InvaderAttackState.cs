using UnityEngine;
using System.Collections;

public class InvaderAttackState : AttackState 
{
	public InvaderAttackState(NewAI aiBehavior, AITargetObject target, AttackBehavior attackBehavior)
		: base(aiBehavior, target, attackBehavior)
	{
		this.m_StateName = "InvaderAttack";
	}
	
	protected override void OnTargetLost ()
	{
		InvaderIdleState idleState = new InvaderIdleState(this.m_AIBehavior, false);
		this.m_AIBehavior.ChangeState(idleState);
	}
}
