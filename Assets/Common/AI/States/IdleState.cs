using UnityEngine;
using System.Collections;

public class IdleState : AIState 
{
	public IdleState(NewAI aiBehavior) : base(aiBehavior)
	{
	}
	
	public override void Initial ()
	{
		if(this.m_AnimationController != null)
		{
			this.m_AnimationController.PlayIdleAnimation();
		}
	}
	
	public override void AICalculate ()
	{
	}
}
