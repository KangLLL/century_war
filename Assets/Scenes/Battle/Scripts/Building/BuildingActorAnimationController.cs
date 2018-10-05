using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingActorAnimationController : MonoBehaviour 
{
	[SerializeField]
	private tk2dSpriteAnimator m_SpriteAnimator;
	
	private Dictionary<CharacterDirection, string> m_AnimationAttackDict;

	// Use this for initialization
	void Start () 
	{	
		this.m_AnimationAttackDict = new Dictionary<CharacterDirection, string>(){
			{CharacterDirection.Up, AnimationNameConstants.ATTACK_UP},
			{CharacterDirection.Down, AnimationNameConstants.ATTACK_DOWN},
			{CharacterDirection.Left, AnimationNameConstants.ATTACK_LEFT},
			{CharacterDirection.Right, AnimationNameConstants.ATTACK_RIGHT},
			{CharacterDirection.LeftUp, AnimationNameConstants.ATTACK_LEFT_UP},
			{CharacterDirection.LeftDown, AnimationNameConstants.ATTACK_LEFT_DOWN},
			{CharacterDirection.RightUp, AnimationNameConstants.ATTACK_RIGHT_UP},
			{CharacterDirection.RightDown, AnimationNameConstants.ATTACK_RIGHT_DOWN}
		};
	}
	
	public void PlayAttackAnimation(Vector3 targetPosition)
	{
		Vector2 deltaVector = (Vector2)(targetPosition - this.transform.position);
		CharacterDirection direction = DirectionHelper.GetDirectionFormVector(deltaVector);
		this.m_SpriteAnimator.Play(this.m_AnimationAttackDict[direction]);
	}
}
