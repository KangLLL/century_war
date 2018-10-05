using UnityEngine;
using System.Collections;

public class AttackObjectEndState : EndState 
{ 
	public override void Initialize ()
	{
		//this.m_Criterion = new AnimationSpriteCriterion(this.Behavior.AnimatedSprite, AnimationNameConstants.END);
		AudioController.Play(this.m_Config.Sound);
		
		this.m_Criterion = new InfinityCriterion();
		this.m_Config.SpriteAnimator.Play(AnimationNameConstants.END);
		Vector2 destinationPosition = this.Behavior.DestinationObject.GetDestinationPosition(this.m_Behavior.transform.position);
		this.Behavior.transform.position = new Vector3(destinationPosition.x, destinationPosition.y, 0);
		this.m_Config.SpriteAnimator.AnimationCompleted += OnExplosionFinished;
		base.Initialize ();
	}
	
	private void OnExplosionFinished(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clip)
	{
		if(this.m_Config.SpriteAnimator == sprite)
		{
			this.Behavior.ChangeState(this.GetNextState());
		}	
	}
}
