using UnityEngine;
using System.Collections;

public class TavernActorBehavior : MonoBehaviour 
{
	[SerializeField]
	private int m_IdleMaxTicks;
	[SerializeField]
	private int m_IdleMinTicks;
	[SerializeField]
	private int m_WaitMaxTicks;
	[SerializeField]
	private int m_WaitMinTicks;
	[SerializeField]
	private tk2dSpriteAnimator m_SpriteAnimator;
	
	private bool m_IsIdle;
	private int m_CurrentTick;
	
	private const string IDLE_ANIMATION_NAME = AnimationNameConstants.IDLE_RIGHT_DOWN;
    private const string IDLE_ANIMATION_NAME_A = AnimationNameConstants.IDLE_RIGHT_DOWN + "A";
	
	void Start()
	{
		this.ChangeToWait();
	}
	
	void Update () 
	{
		if(this.m_CurrentTick -- == 0)
		{
			if(this.m_IsIdle)
			{
				this.ChangeToWait();
			}
			else
			{
				this.ChangeToIdle();
			}
		}
	}
	
	private void ChangeToWait()
	{
		this.m_CurrentTick = Random.Range(this.m_WaitMinTicks, this.m_WaitMaxTicks);
        this.m_SpriteAnimator.Play(IDLE_ANIMATION_NAME_A);
		this.m_SpriteAnimator.Play();
		this.m_IsIdle = false;
	}
	
	private void ChangeToIdle()
	{
		this.m_CurrentTick = Random.Range(this.m_IdleMinTicks, this.m_IdleMaxTicks);
		this.m_SpriteAnimator.Play(IDLE_ANIMATION_NAME);
		this.m_SpriteAnimator.Play();
		this.m_IsIdle = true;
	}
}
