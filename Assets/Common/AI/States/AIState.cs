using UnityEngine;
using System.Collections;
using System;

[Serializable]
public abstract class AIState 
{
	protected NewAI m_AIBehavior;
	protected CharacterAnimationController m_AnimationController;
	
	protected string m_StateName;
	protected string[] m_Values;
	
	public AIState(NewAI aiBehavior)
	{
		this.m_AIBehavior = aiBehavior;
		this.m_AnimationController = this.m_AIBehavior.GetComponent<CharacterAnimationController>();
	}
	
	public abstract void AICalculate();
	
	public abstract void Initial();
}
