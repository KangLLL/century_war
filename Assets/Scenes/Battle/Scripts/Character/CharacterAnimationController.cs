using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterAnimationController : MonoBehaviour 
{
	[SerializeField]
	private tk2dSpriteAnimator m_SpriteAnimator;
	[SerializeField]
	private tk2dSprite m_ShadowSprite;
	
	private float m_FPSScale;
	
	private string m_PreviousAnimationName;
	private string m_AnimationName;
	
	private bool m_IsPlayInstantly;
	
	private CharacterDirection m_Direction;
	
	private static Dictionary<CharacterDirection, string> s_AnimationWalkDict;
	private static Dictionary<CharacterDirection, string> s_AnimationAttackDict;
	private static Dictionary<CharacterDirection, List<string>> s_AnimationBuildDict;
	private static Dictionary<CharacterDirection, string> s_AnimationIdleDict;
		

	// Use this for initialization
	void Awake () 
	{	
		s_AnimationWalkDict = new Dictionary<CharacterDirection, string>(){
			{CharacterDirection.Up, AnimationNameConstants.MOVE_UP},
			{CharacterDirection.Down, AnimationNameConstants.MOVE_DOWN},
			{CharacterDirection.Left, AnimationNameConstants.MOVE_LEFT},
			{CharacterDirection.Right, AnimationNameConstants.MOVE_RIGHT},
			{CharacterDirection.LeftUp, AnimationNameConstants.MOVE_LEFT_UP},
			{CharacterDirection.LeftDown, AnimationNameConstants.MOVE_LEFT_DOWN},
			{CharacterDirection.RightUp, AnimationNameConstants.MOVE_RIGHT_UP},
			{CharacterDirection.RightDown, AnimationNameConstants.MOVE_RIGHT_DOWN}
		};
		s_AnimationAttackDict = new Dictionary<CharacterDirection, string>(){
			{CharacterDirection.Up, AnimationNameConstants.ATTACK_UP},
			{CharacterDirection.Down, AnimationNameConstants.ATTACK_DOWN},
			{CharacterDirection.Left, AnimationNameConstants.ATTACK_LEFT},
			{CharacterDirection.Right, AnimationNameConstants.ATTACK_RIGHT},
			{CharacterDirection.LeftUp, AnimationNameConstants.ATTACK_LEFT_UP},
			{CharacterDirection.LeftDown, AnimationNameConstants.ATTACK_LEFT_DOWN},
			{CharacterDirection.RightUp, AnimationNameConstants.ATTACK_RIGHT_UP},
			{CharacterDirection.RightDown, AnimationNameConstants.ATTACK_RIGHT_DOWN}
		};
		s_AnimationBuildDict = new Dictionary<CharacterDirection, List<string>>(){
			{CharacterDirection.Up, new List<string> { AnimationNameConstants.BUILD_UP_A, AnimationNameConstants.BUILD_UP_B }},
			{CharacterDirection.Down, new List<string> { AnimationNameConstants.BUILD_DOWN_A, AnimationNameConstants.BUILD_DOWN_B }},
			{CharacterDirection.Left, new List<string> { AnimationNameConstants.BUILD_LEFT_A, AnimationNameConstants.BUILD_LEFT_B }},
			{CharacterDirection.Right, new List<string> { AnimationNameConstants.BUILD_RIGHT_A, AnimationNameConstants.BUILD_RIGHT_B }},
			{CharacterDirection.LeftUp, new List<string> { AnimationNameConstants.BUILD_LEFT_UP_A, AnimationNameConstants.BUILD_LEFT_UP_B }},
			{CharacterDirection.LeftDown, new List<string> { AnimationNameConstants.BUILD_LEFT_DOWN_A, AnimationNameConstants.BUILD_LEFT_DOWN_B }},
			{CharacterDirection.RightUp, new List<string> { AnimationNameConstants.BUILD_RIGHT_UP_A, AnimationNameConstants.BUILD_RIGHT_UP_B }},
			{CharacterDirection.RightDown, new List<string> { AnimationNameConstants.BUILD_RIGHT_DOWN_A, AnimationNameConstants.BUILD_RIGHT_DOWN_B }}
		};
		s_AnimationIdleDict = new Dictionary<CharacterDirection, string>() {
			{CharacterDirection.Up, AnimationNameConstants.IDLE_UP},
			{CharacterDirection.Down, AnimationNameConstants.IDLE_DOWN},
			{CharacterDirection.Left, AnimationNameConstants.IDLE_LEFT},
			{CharacterDirection.Right, AnimationNameConstants.IDLE_RIGHT},
			{CharacterDirection.LeftUp, AnimationNameConstants.IDLE_LEFT_UP},
			{CharacterDirection.LeftDown, AnimationNameConstants.IDLE_LEFT_DOWN},
			{CharacterDirection.RightUp, AnimationNameConstants.IDLE_RIGHT_UP},
			{CharacterDirection.RightDown, AnimationNameConstants.IDLE_RIGHT_DOWN}
		};
		
		this.m_FPSScale = 1.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if((this.m_AnimationName != this.m_PreviousAnimationName || this.m_IsPlayInstantly) && this.m_SpriteAnimator != null)
		{
			this.m_SpriteAnimator.Play(this.m_AnimationName);
			this.m_SpriteAnimator.ClipFps = this.m_SpriteAnimator.ClipFps * this.m_FPSScale;
			this.m_IsPlayInstantly = false;
		}
		this.m_PreviousAnimationName = this.m_AnimationName;
	}
	
	public void PlayInstantly()
	{
		this.m_IsPlayInstantly = true;
	}
	
	public void PlayWalkAnimation(Vector2 moveVector)
	{
		this.CalculateDirection(moveVector);
		this.m_AnimationName = s_AnimationWalkDict[this.m_Direction];
	}
	
	public void PlayAttackAnimation(Vector3 targetPosition, Vector3 sourcePosition)
	{
		this.CalculateDirection((Vector2)(targetPosition - sourcePosition));
		this.m_AnimationName = s_AnimationAttackDict[this.m_Direction];
	}
	
	public void PlayBuildAnimation(Vector3 targetPosition)
	{
		this.CalculateDirection((Vector2)(targetPosition - this.transform.position));
		List<string> animationsName = s_AnimationBuildDict[this.m_Direction];
		
		this.m_AnimationName = animationsName[Random.Range(0, animationsName.Count)];
	}
	
	public void PlayIdleAnimation(Vector3 targetPosition)
	{
		this.CalculateDirection((Vector2)(targetPosition - this.transform.position));
		this.PlayIdleAnimation();
	}
	
	public void PlayIdleAnimation(Vector3 targetPosition, Vector3 sourcePosition)
	{
		this.CalculateDirection((Vector2)(targetPosition - sourcePosition));
		this.PlayIdleAnimation();
	}
	
	public void PlayIdleAnimation()
	{
		if(this.m_Direction == CharacterDirection.None)
		{
			this.m_AnimationName = AnimationNameConstants.IDLE;
		}
		else
		{
			this.m_AnimationName = s_AnimationIdleDict[this.m_Direction];
		}
	}
	
	private void CalculateDirection(Vector2 dletaVector)
	{		
		this.m_Direction = DirectionHelper.GetDirectionFormVector(dletaVector);
	}
	
	public void SetVisible()
	{
		this.m_SpriteAnimator.renderer.enabled = true;
		if(this.m_ShadowSprite != null)
		{
			this.m_ShadowSprite.renderer.enabled = true;
		}
	}
	
	public void SetHide()
	{
		this.m_SpriteAnimator.renderer.enabled = false;
		if(this.m_ShadowSprite != null)
		{
			this.m_ShadowSprite.renderer.enabled = false;
		}
	}
	
	public void SetAdvanceScale(float scale)
	{
		this.m_FPSScale = scale; 
	}
}
