using UnityEngine;
using System.Collections;

public class LastingPropsBehavior : AttackPropsBehavior 
{
	public int TotalTimes { get;set; }
	public int IntervalTicks { get;set; }
	
	private int m_CurrentTick;
	private int m_CurrentTimes;
	
	public override void Start()
	{
		base.Start();
		this.m_CurrentTick = 0;
		this.m_CurrentTimes = 0;
	}
	
	void FixedUpdate()
	{
		if(this.IntervalTicks == 0)
		{
			if(this.m_CurrentTick == this.m_PlayAnimationTicks)
			{
				while(this.m_CurrentTimes < this.TotalTimes)
				{
					this.m_CurrentTimes ++;
					this.Effect();
				}
			}
		}
		else
		{
			if((this.m_CurrentTick - this.m_PlayAnimationTicks) % this.IntervalTicks == 0)
			{
				this.m_CurrentTimes ++;
				this.Effect();
			}
		}
		if(this.m_CurrentTimes == this.TotalTimes)
		{
			GameObject.Destroy(this.gameObject);
			this.EffectDisappear();
		}
		else
		{
			this.m_CurrentTick ++;
		}
	}
	
	protected virtual void Effect()
	{
	}
}
