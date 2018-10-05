using UnityEngine;
using System.Collections;
using ConfigUtilities;

public class ItemAccelerateComponent : AccelerateLogicComponent 
{
	public override float Effect 
	{
		get 
		{
			return ConfigInterface.Instance.SystemConfig.ProduceItemAccelerateScale;
		}
	}
	
	protected override bool IsFinish 
	{
		get 
		{
			return this.m_BuildingData.RemainItemAccelerateTime == 0;
		}
	}
	
	protected override float Advance (float elapsedSeconds)
	{
		float noAccelerateTime = elapsedSeconds - this.m_BuildingData.RemainItemAccelerateTime.Value;
		this.m_BuildingData.RemainItemAccelerateTime = Mathf.Max(0,this.m_BuildingData.RemainItemAccelerateTime.Value - elapsedSeconds);
		return noAccelerateTime;
	}
}
