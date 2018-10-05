using UnityEngine;
using System.Collections;

public class AttackObjectState 
{
	protected AttackObjectBehavior m_Behavior;
	protected AttackObjectConfig m_Config;
	
	public AttackObjectBehavior Behavior
	{ 
		get
		{
			return this.m_Behavior;
		}
		set
		{
			this.m_Behavior = value;
			this.m_Config = this.m_Behavior.GetComponent<AttackObjectConfig>();
		}
	}
	protected IFinishable m_Criterion;
	
	private bool m_IsFirstExecute = true;
	
	
	public virtual AttackObjectState GetNextState()
	{
		return null;
	}
	
	public virtual void Initialize()
	{
		this.m_Criterion.Reset();
	}
	
	public virtual void ExecuteLogic()
	{
		if(this.m_IsFirstExecute)
		{
			this.Initialize();
			this.m_IsFirstExecute = false;
		}
		
		if(this.m_Criterion.IsFinished())
		{
			this.Behavior.ChangeState(this.GetNextState());
		}
		FrameRelatedCriterion frameRelatedCriterion = this.m_Criterion as FrameRelatedCriterion;
		if(frameRelatedCriterion != null)
		{
			frameRelatedCriterion.Advance();
		}
	}
}
