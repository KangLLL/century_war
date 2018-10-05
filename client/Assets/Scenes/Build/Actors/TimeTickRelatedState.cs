using UnityEngine;
using System.Collections;

public abstract class TimeTickRelatedState : AIState
{
	protected int m_CurrentFrame;
	
	public TimeTickRelatedState(NewAI aiBehavior) : base(aiBehavior)
	{
	}
	
	public override void AICalculate ()
	{
		if(this.m_CurrentFrame -- == 0)
		{
			this.OnTimeUp();
		}
	}
	
	protected virtual void OnTimeUp()
	{
	}
}
