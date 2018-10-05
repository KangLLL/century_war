using UnityEngine;
using System.Collections;

public class AnimationCriterion : IFinishable 
{
	private Animation m_Animation;
	
	public AnimationCriterion(Animation animation)
	{
		this.m_Animation = animation;
	}
	
	public void Reset()
	{
		this.m_Animation.Stop();
		this.m_Animation.Play();
	}
	
	public bool IsFinished()
	{
		return !this.m_Animation.isPlaying;
	}
}
