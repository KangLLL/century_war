using UnityEngine;
using System.Collections;
using System;

using ConfigUtilities;
using ConfigUtilities.Enums;

public abstract class AccelerateLogicComponent : BuildingLogicComponent 
{
	public event Action<float> AccelerateFinish;
	
	private float m_PreviousSecond;
	private bool m_PreviousBlockState;
	
	public abstract float Effect { get; }
	protected abstract bool IsFinish { get; }
	
	protected abstract float Advance(float elapsedSeconds);
	
	public override void Initial (BuildingData data)
	{
		base.Initial (data);
		this.m_PreviousSecond = LogicTimer.Instance.CurrentTime;
		this.m_PreviousBlockState = false;
	}
	
	public override void Process ()
	{
		bool isUpgrade = this.m_BuildingData.UpgradeRemainingWorkload.HasValue;
		if(!isUpgrade)
		{
			if(this.m_PreviousBlockState)
			{
				this.m_PreviousSecond = LogicTimer.Instance.CurrentTime;
			}
			float currentSecond = LogicTimer.Instance.CurrentTime;
			if(this.m_PreviousSecond != currentSecond)
			{
				float elapsedSeconds = currentSecond - this.m_PreviousSecond;
				this.m_PreviousSecond = currentSecond;
				
				float noAccelerateTime = this.Advance(elapsedSeconds);
				if(this.IsFinish)
				{
					if(this.AccelerateFinish != null)
					{
						this.AccelerateFinish(noAccelerateTime);
					}
					this.LogicObject.RemoveComponent(this);
				}
			}
		}
		this.m_PreviousBlockState = isUpgrade;
	}
}
