using UnityEngine;
using System.Collections;
using ConfigUtilities;

public class ArmyAccelerateComponent : AccelerateLogicComponent 
{
	public override float Effect 
	{
		get 
		{
			return ConfigInterface.Instance.SystemConfig.ProduceArmyAccelerateScale;
		}
	}
	
	protected override bool IsFinish 
	{
		get 
		{
			return this.m_BuildingData.RemainArmyAccelerateTime == 0;
		}
	}
	
	protected override float Advance (float elapsedSeconds)
	{
		float noAccelerateTime = elapsedSeconds - this.m_BuildingData.RemainArmyAccelerateTime.Value;
		this.m_BuildingData.RemainArmyAccelerateTime = Mathf.Max(0, this.m_BuildingData.RemainArmyAccelerateTime.Value - elapsedSeconds);
		return noAccelerateTime;
	}
}
