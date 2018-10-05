using UnityEngine;
using System.Collections;
using System;
using ConfigUtilities;

public class RemoveLogicComponent : LogicComponent 
{
	public event Action<float> RemoveFinish;
	private RemovableObjectData m_Data;
	
	public RemoveLogicComponent(RemovableObjectData data)
	{
		this.m_Data = data;
		this.m_PreviousSecond = LogicTimer.Instance.CurrentTime;
	}
		
	private float m_PreviousSecond;
	
	private int m_PreviousBuilderLevel;
	private float m_PreviousEfficiency;
	
	public override void Process ()
	{
		if(this.m_Data.RemainingWorkload > 0)
		{
			float currentSecond = LogicTimer.Instance.CurrentTime;
			if(this.m_PreviousSecond != currentSecond)
			{
				float elapsedSecond = currentSecond - this.m_PreviousSecond;
				this.m_PreviousSecond = currentSecond;
				BuildingIdentity builerHutIdentity = new BuildingIdentity();
				builerHutIdentity.buildingType = ConfigUtilities.Enums.BuildingType.BuilderHut;
				builerHutIdentity.buildingNO = this.m_Data.BuilderBuildingNO.Value;
				this.m_PreviousEfficiency = 1;
				/*
				int builderLevel = LogicController.Instance.GetBuildingObject(builerHutIdentity).Level;
				if(builderLevel != this.m_PreviousBuilderLevel)
				{
					this.m_PreviousBuilderLevel = builderLevel;
					this.m_PreviousEfficiency = ConfigInterface.Instance.BuilderConfigHelper.GetBuilderData(builderLevel).BuildEfficiency;
				}
				*/
				float previousRemainingWorkload = this.m_Data.RemainingWorkload.Value;
				this.m_Data.RemainingWorkload = Mathf.Max(0, 
					this.m_Data.RemainingWorkload.Value - this.m_PreviousEfficiency * elapsedSecond);
				
				if(this.m_Data.RemainingWorkload == 0)
				{
					if(this.RemoveFinish != null)
					{
						this.RemoveFinish(elapsedSecond - previousRemainingWorkload / this.m_PreviousEfficiency);
					}
				}
			}
		}
	}
}
