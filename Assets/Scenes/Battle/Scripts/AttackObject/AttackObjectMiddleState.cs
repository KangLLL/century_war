using UnityEngine;
using System.Collections;

public class AttackObjectMiddleState : AttackObjectState 
{	
	private Vector3 m_MoveVector;
	private Vector3 m_CurrentPosition;
    private CurveCalculator m_CurveCalculator;
	
	public override void Initialize ()
	{
		/*
		this.m_Criterion = new AnimationCriterion(this.Behavior.MoveAnimation);
		
		this.Behavior.MoveAnimation.clip.ClearCurves();
		
		Vector3 sourcePosition = this.Behavior.SourceObject.transform.position;
		Vector3 destinationPosition = this.Behavior.DestinationObject.transform.position;
		
		float distance = Vector2.Distance(sourcePosition, destinationPosition);
		
		float moveTime = distance / this.Behavior.Velocity;
		
		AnimationCurve moveXCurve = AnimationCurve.Linear(0, sourcePosition.x, moveTime, destinationPosition.x);
		AnimationCurve moveYCurve = AnimationCurve.Linear(0, sourcePosition.y, moveTime, destinationPosition.y);
		AnimationCurve moveZCurve = AnimationCurve.Linear(0, sourcePosition.z, moveTime, destinationPosition.z);
		
		this.Behavior.MoveAnimation.clip.SetCurve("",typeof(Transform),"localPosition.x",moveXCurve);
		this.Behavior.MoveAnimation.clip.SetCurve("",typeof(Transform),"localPosition.y",moveYCurve);
		this.Behavior.MoveAnimation.clip.SetCurve("",typeof(Transform),"localPosition.z",moveZCurve);
		*/
		
		Vector3 sourcePosition = this.Behavior.transform.position;
		Vector3 destinationPosition = this.Behavior.DestinationObject.GetDestinationPosition(this.m_Behavior.transform.position);
		
		float distance = Vector2.Distance(new Vector2(destinationPosition.x,destinationPosition.y),new Vector2(sourcePosition.x,sourcePosition.y));
		int middleFrames = Mathf.CeilToInt(distance / this.Behavior.Velocity);
		this.m_MoveVector = (destinationPosition - this.Behavior.transform.position) / middleFrames;


		this.m_Criterion = new FrameRelatedCriterion(middleFrames);
		if(this.m_Config.SpriteAnimator.GetClipByName(AnimationNameConstants.MIDDLE) != null)
		{
			this.m_Config.SpriteAnimator.Play(AnimationNameConstants.MIDDLE);
		}
		else
		{
			CharacterDirection direction = DirectionHelper.GetDirectionFormVector(destinationPosition - this.Behavior.transform.position);
			
			switch(direction)
			{
				case CharacterDirection.Up:
				{
					this.m_Config.SpriteAnimator.Play(AnimationNameConstants.MIDDLE_UP);
				}
				break;
				case CharacterDirection.Down:
				{
					this.m_Config.SpriteAnimator.Play(AnimationNameConstants.MIDDLE_DOWN);
				}
				break;
				case CharacterDirection.Left:
				{
					this.m_Config.SpriteAnimator.Play(AnimationNameConstants.MIDDLE_LEFT);
				}
				break;
				case CharacterDirection.Right:
				{
					this.m_Config.SpriteAnimator.Play(AnimationNameConstants.MIDDLE_RIGHT);
				}
				break;
				case CharacterDirection.LeftUp:
				{
					this.m_Config.SpriteAnimator.Play(AnimationNameConstants.MIDDLE_LEFT_UP);
				}
				break;
				case CharacterDirection.LeftDown:
				{
					this.m_Config.SpriteAnimator.Play(AnimationNameConstants.MIDDLE_LEFT_DOWN);
				}
				break;
				case CharacterDirection.RightUp:
				{
					this.m_Config.SpriteAnimator.Play(AnimationNameConstants.MIDDLE_RIGHT_UP);
				}
				break;
				case CharacterDirection.RightDown:
				{
					this.m_Config.SpriteAnimator.Play(AnimationNameConstants.MIDDLE_RIGHT_DOWN);
				}
				break;
			}
		}
		
		this.m_CurveCalculator = new CurveCalculator(this.m_Config.CurveG, middleFrames);
		this.m_CurrentPosition = this.Behavior.transform.position;
		base.Initialize ();
	}
	
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
	
	public override void ExecuteLogic ()
	{	
		base.ExecuteLogic();
		
		this.m_CurveCalculator.Process();
		this.m_CurrentPosition += this.m_MoveVector;
		this.Behavior.transform.position = this.m_CurrentPosition + this.m_CurveCalculator.HeightRelatedVector;
	}
}
