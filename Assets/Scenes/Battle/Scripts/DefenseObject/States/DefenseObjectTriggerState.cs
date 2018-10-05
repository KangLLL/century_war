using UnityEngine;
using System.Collections;
using CommandConsts;

public class DefenseObjectTriggerState : AIState 
{
	private DefenseObjectBattleBehavior m_DefenseObjectBattleBehavior;
	private int m_CurrentTriggerCount;
	
	public DefenseObjectTriggerState(NewAI aiBehavior) : base(aiBehavior)
	{
	}
	
	public override void Initial ()
	{
		this.m_DefenseObjectBattleBehavior = this.m_AIBehavior.GetComponent<DefenseObjectBattleBehavior>();
		this.m_CurrentTriggerCount = this.m_DefenseObjectBattleBehavior.TriggerTick;
		
		AudioController.Play(this.m_DefenseObjectBattleBehavior.EffectSound);
	}
	
	public override void AICalculate ()
	{
		if(--this.m_CurrentTriggerCount == 0)
		{
			this.m_DefenseObjectBattleBehavior.Effect();
			
			LastingEffectBehavior lastingEffect = this.m_DefenseObjectBattleBehavior as LastingEffectBehavior;
			if(lastingEffect != null && lastingEffect.CurrentTimes != lastingEffect.TotalTimes)
			{
				DefenseObjectLastingState lastingState = new DefenseObjectLastingState(this.m_AIBehavior, lastingEffect);
				this.m_AIBehavior.ChangeState(lastingState);
			}
		}
	}
	
}
