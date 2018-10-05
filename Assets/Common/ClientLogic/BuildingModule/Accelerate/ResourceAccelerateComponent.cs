using UnityEngine;
using System.Collections;
using ConfigUtilities;

public class ResourceAccelerateComponent : AccelerateLogicComponent 
{
	public override float Effect 
	{
		get 
		{
			return ConfigInterface.Instance.SystemConfig.ProduceResourceAccelerateScale;
		}
	}
	
	protected override bool IsFinish 
	{
		get 
		{
			return this.m_BuildingData.RemainResourceAccelerateTime == 0;
		}
	}
	
	protected override float Advance (float elapsedSeconds)
	{
		float noAccelerateTime = elapsedSeconds - this.m_BuildingData.RemainResourceAccelerateTime.Value;
		this.m_BuildingData.RemainResourceAccelerateTime = Mathf.Max(0, this.m_BuildingData.RemainResourceAccelerateTime.Value - elapsedSeconds);
		return noAccelerateTime;
	}
}
