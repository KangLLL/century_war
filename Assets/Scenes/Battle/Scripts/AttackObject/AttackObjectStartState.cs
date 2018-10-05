using UnityEngine;
using System.Collections;

public class AttackObjectStartState : AttackObjectState
{
	public override void Initialize ()
	{
		//this.m_Criterion = new AnimationSpriteCriterion(this.Behavior.AnimatedSprite, AnimationNameConstants.START);
		this.m_Criterion = new FrameRelatedCriterion(this.m_Config.StartFrames);
		this.m_Config.SpriteAnimator.Play(AnimationNameConstants.START);
		base.Initialize ();
	}
	
	public override void ExecuteLogic ()
	{
		base.ExecuteLogic();
		if((this.m_Behavior.DestinationObject is TargetObject  && ((TargetObject)this.m_Behavior.DestinationObject).Target == null) 
			|| this.m_Behavior.SourceObject == null)
		{
			GameObject.DestroyObject(this.m_Behavior.gameObject);
		}
	}
	
	public override AttackObjectState GetNextState ()
	{
		if(this.m_Config.SpriteAnimator.GetClipByName(AnimationNameConstants.MIDDLE) != null || 
			this.m_Config.SpriteAnimator.GetClipByName(AnimationNameConstants.MIDDLE_UP) != null)
		{	
			
			if(this.m_Behavior.DestinationObject is TargetObject && !((TargetObject)this.m_Behavior.DestinationObject).Target.IsStaticTarget())
			{
				return new AttackObjectTraceMiddleState();
			}
			else
			{
				return new AttackObjectMiddleState();
			}
		}
		else if(this.m_Config.EndPrefab != null)
		{
			AttackObjectPrefabEndState endState = new AttackObjectPrefabEndState();
			return endState;
		}
		else if(this.m_Config.SpriteAnimator.GetClipByName(AnimationNameConstants.END) != null)
		{
			AttackObjectEndState endState = new AttackObjectEndState();
			return endState;
		}
		return null;
	}
}
