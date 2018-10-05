using UnityEngine;
using System.Collections;

public class AttackObjectTraceMiddleState : AttackObjectState 
{
	private Vector3 m_TargetPosition;
	private Vector3 m_CurrentPosition;
	private CurveCalculator m_CurveCalculator;
	
	public override AttackObjectState GetNextState ()
	{
		if(this.m_Config.EndPrefab != null)
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
	
	public override void Initialize ()
	{	
		this.m_Config.SpriteAnimator.Play(AnimationNameConstants.MIDDLE);
		this.m_CurrentPosition = this.Behavior.transform.position;
		this.m_TargetPosition = this.Behavior.DestinationObject.GetDestinationPosition(this.m_CurrentPosition);
		this.m_Criterion = new InfinityCriterion();
		
		int frames = Mathf.CeilToInt(Vector2.Distance(((Vector2)this.m_TargetPosition),((Vector2)this.m_CurrentPosition)) / this.Behavior.Velocity);
		this.m_CurveCalculator = new CurveCalculator(this.m_Config.CurveG, frames);
	}
	
	public override void ExecuteLogic ()
	{	
		base.ExecuteLogic();
		Vector3 targetPosition = this.m_TargetPosition;
		if(this.Behavior.DestinationObject != null)
		{
			targetPosition = this.Behavior.DestinationObject.GetDestinationPosition(this.m_CurrentPosition);
		}
		
		Vector2 delta = ((Vector2)targetPosition) - ((Vector2)this.m_CurrentPosition);
		float percentage = this.Behavior.Velocity / 
			Vector2.Distance(targetPosition, this.m_CurrentPosition);
		
		if(percentage >= 1)
		{
			this.m_CurrentPosition = new Vector2(targetPosition.x, 
				targetPosition.y);
			this.Behavior.ChangeState(this.GetNextState());
		}
		else
		{
			Vector2 moveDistance = delta * percentage;
			this.m_CurrentPosition += new Vector3(moveDistance.x, moveDistance.y, 0);
		}
		
		this.m_CurveCalculator.Process();
		this.Behavior.transform.position = this.m_CurrentPosition + this.m_CurveCalculator.HeightRelatedVector;
		this.Behavior.transform.rotation = Quaternion.FromToRotation(Vector3.right, new Vector3(delta.x, delta.y,0));
	}
}
