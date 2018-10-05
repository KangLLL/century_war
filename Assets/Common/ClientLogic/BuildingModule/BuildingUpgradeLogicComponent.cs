using UnityEngine;
using System.Collections;
using System;

using ConfigUtilities;

public class BuildingUpgradeLogicComponent : BuildingLogicComponent 
{
	public event Action<float> UpgradeTimeUp;
		
	private float m_PreviousSecond;
	
	private int m_PreviousBuilderLevel;
	private float m_PreviousEfficiency;
	
	private bool m_PreviousIsUpgrading;
	
	public bool IsUpgrading
	{
		get
		{
			return this.m_BuildingData.BuilderBuildingNO.HasValue;
		}
	}
	
	public override void Initial (BuildingData data)
	{
		base.Initial (data);
		this.m_PreviousSecond = LogicTimer.Instance.CurrentTime;
	}
	
	public override void Process ()
	{
		base.Process ();
		if(this.IsUpgrading && this.m_BuildingData.UpgradeRemainingWorkload > 0)
		{
			float currentSecond = LogicTimer.Instance.CurrentTime;
			if(!this.m_PreviousIsUpgrading)
			{
				this.m_PreviousSecond = currentSecond;
			}
			if(this.m_PreviousSecond != currentSecond)
			{
				float elapsedSecond = currentSecond - this.m_PreviousSecond;
				this.m_PreviousSecond = currentSecond;
				BuildingIdentity builerHutIdentity = new BuildingIdentity();
				builerHutIdentity.buildingType = ConfigUtilities.Enums.BuildingType.BuilderHut;
				builerHutIdentity.buildingNO = this.m_BuildingData.BuilderBuildingNO.Value;
				int builderLevel = LogicController.Instance.GetBuildingObject(builerHutIdentity).Level;
				if(builderLevel != this.m_PreviousBuilderLevel)
				{
					this.m_PreviousBuilderLevel = builderLevel;
					this.m_PreviousEfficiency = ConfigInterface.Instance.BuilderConfigHelper.GetBuilderData(builderLevel).BuildEfficiency;
				}

				float previousWorkload = this.m_BuildingData.UpgradeRemainingWorkload.Value;
				this.m_BuildingData.UpgradeRemainingWorkload = Mathf.Max(0, 
					this.m_BuildingData.UpgradeRemainingWorkload.Value - this.m_PreviousEfficiency * elapsedSecond);
				//Debug.Log(this.m_BuildingData.UpgradeRemainingWorkload);
				if(this.m_BuildingData.UpgradeRemainingWorkload == 0)
				{
					if(this.UpgradeTimeUp != null)
					{
						this.UpgradeTimeUp(elapsedSecond - previousWorkload / this.m_PreviousEfficiency);
					}
				}
			}
		}
		this.m_PreviousIsUpgrading = this.IsUpgrading;
	}
	
	/*
	public void StartUpgrade(int builderEfficiency)
	{
		if(this.IsUpgrading)
		{
			return;
		}
		else
		{
			this.m_BuildingData.ActorWorkEfficiency = builderEfficiency;
			this.m_CurrentTick = 0;
		}
	}
	*/
}
