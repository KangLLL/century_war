using UnityEngine;
using System.Collections;

public class AttackState : AIState 
{
	protected AITargetObject m_Target;
	protected AttackBehavior m_AttackBehavior;
	
	public AttackState(NewAI aiBehavior, AITargetObject target, AttackBehavior attackBehavior) : base(aiBehavior)
	{
		this.m_Target = target;
		this.m_AttackBehavior = attackBehavior;
	}
	
	protected virtual Vector2 SelfPosition
	{
		get 
		{ 
			BuildingBasePropertyBehavior property = this.m_AIBehavior.GetComponent<BuildingBasePropertyBehavior>();
			if(property != null)
			{
				return property.AnchorTransform.position;
			}
			else
			{
				return this.m_AIBehavior.transform.position; 
			}
			
		}
	}
	
	public override void Initial ()
	{
		if(!this.m_AIBehavior.CanAttack)
		{
			if(this.m_AnimationController != null)
			{
				this.m_AnimationController.PlayIdleAnimation(this.m_Target.TargetPosition, this.SelfPosition);
			}
		}
		//this.m_CurrentFrame = this.m_AIBehavior.GlobalAttackCD;
		//this.m_CurrentFrame = (int)(this.m_AttackBehavior.AttackCD * ClientConfigConstants.Instance.TicksPerSecond);
		//this.Attack();
	}
	
	public override void AICalculate ()
	{
		if(this.m_Target.Target == null || this.IsTargetLose())
		{
			this.OnTargetLost();
		}
		else if(this.m_AIBehavior.CanAttack)
		{
			this.Attack();
		}
	}
	
	protected bool IsTargetLose()
	{
		float disanceSqr = Vector2.SqrMagnitude(this.m_Target.TargetPosition - this.SelfPosition);
		if(this.m_AttackBehavior is RingAttackBehavior)
		{
			float blindDistanceSqr = ((RingAttackBehavior)this.m_AttackBehavior).BlindScopeSqr;
			return disanceSqr <= blindDistanceSqr || disanceSqr > this.m_AttackBehavior.AttackScopeSqr;
		}
		else
		{
			return disanceSqr > this.m_AttackBehavior.AttackScopeSqr;
		}
	}
	
	protected virtual void OnTargetLost()
	{
	}
	
	private void Attack()
	{
		this.m_AIBehavior.ResetAttackCD(this.m_AttackBehavior.AttackCD);

		if(this.m_AnimationController != null)
		{
			this.m_AnimationController.PlayAttackAnimation(this.m_Target.TargetPosition, this.SelfPosition);
			this.m_AnimationController.PlayInstantly();
		}
		this.m_AttackBehavior.Fire(this.m_Target.Target, this.m_Target.TargetPosition);
	}
}
