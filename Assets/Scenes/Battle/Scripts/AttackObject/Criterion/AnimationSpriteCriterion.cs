using UnityEngine;
using System.Collections;

public class AnimationSpriteCriterion : IFinishable 
{
	private tk2dSpriteAnimator m_SpriteAnimator;
	private string m_AnimationName;
	
	public AnimationSpriteCriterion(tk2dSpriteAnimator sprite, string animationName)
	{
		this.m_SpriteAnimator = sprite;
		this.m_AnimationName = animationName;
	}
	
	public void Reset()
	{
		this.m_SpriteAnimator.StopAndResetFrame();
		this.m_SpriteAnimator.Play(this.m_AnimationName);
	}
	
	public bool IsFinished()
	{
		return !this.m_SpriteAnimator.IsPlaying(this.m_AnimationName);
	}
}