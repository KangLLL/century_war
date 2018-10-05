using UnityEngine;
using System.Collections;

public abstract class BuildingProduceLogicComponent : BuildingLogicComponent
{
	private float m_PreviousSecond;
	private bool m_PreviousBlockingState;
	private AccelerateLogicComponent m_Accelerate;
	
	protected bool IsBlocking{ get { return this.BlockLogic(); } }
	
	public AccelerateLogicComponent Accelerate 
	{ 
		get
		{
			return this.m_Accelerate;
		}
		set
		{
			this.m_Accelerate = value;
			this.FloorOutput();
		}
	}
	
	public override void Process ()
	{
		base.Process ();
		if(!this.IsBlocking)
		{
			float currentSecond = LogicTimer.Instance.CurrentTime;
			if(this.m_PreviousBlockingState)
			{
				this.m_PreviousSecond = currentSecond;
			}
			if(this.m_PreviousSecond < currentSecond)
			{
				float elapsedSecond = currentSecond - this.m_PreviousSecond;
				this.m_PreviousSecond = currentSecond;
				this.ProduceAdvance(elapsedSecond);
			}
		}
		else
		{
			this.FloorOutput();
		}
		this.m_PreviousBlockingState = this.IsBlocking;
	}
	
	public override void Initial (BuildingData data)
	{
		base.Initial (data);
		this.m_PreviousSecond = LogicTimer.Instance.CurrentTime;
	}
	
	protected void Reset()
	{
		this.m_PreviousSecond = LogicTimer.Instance.CurrentTime;
	}
	
	protected virtual void ProduceAdvance(float elapsedSeconds)
	{
	}
	
	protected virtual void FloorOutput()
	{
	}
	
	public void ProduceAdvanceTo(float time)
	{
		if(!this.IsBlocking)
		{
			float elpasedSeconds = time - this.m_PreviousSecond;
			this.m_PreviousSecond = time;
			this.ProduceAdvance(elpasedSeconds);
		}
	}
	
	protected virtual bool BlockLogic()
	{
		return this.m_BuildingData.UpgradeRemainingWorkload.HasValue;
	}
}
