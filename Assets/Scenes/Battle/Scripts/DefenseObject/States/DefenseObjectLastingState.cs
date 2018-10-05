using UnityEngine;
using System.Collections;

public class DefenseObjectLastingState :  AIState
{
	private LastingEffectBehavior m_LastingEffectBehavior;
	private int m_CurrentTriggerCount;
	
	public DefenseObjectLastingState(NewAI aiBehavior, LastingEffectBehavior lastingEffectBehavior) : base(aiBehavior)
	{
		this.m_LastingEffectBehavior = lastingEffectBehavior;
	}
	
	public override void Initial ()
	{
		this.m_CurrentTriggerCount = this.m_LastingEffectBehavior.IntervalTicks;
	}
	
	public override void AICalculate ()
	{
		if(--this.m_CurrentTriggerCount == 0)
		{
			this.m_LastingEffectBehavior.Effect();
			
			if(this.m_LastingEffectBehavior.CurrentTimes != this.m_LastingEffectBehavior.TotalTimes)
			{
				DefenseObjectLastingState lastingState = new DefenseObjectLastingState(this.m_AIBehavior, this.m_LastingEffectBehavior);
				this.m_AIBehavior.ChangeState(lastingState);
			}
		}
	}
}
