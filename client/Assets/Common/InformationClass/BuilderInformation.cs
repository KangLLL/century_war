using UnityEngine;
using System.Collections;
using ConfigUtilities;

public class BuilderInformation
{
	private BuildingIdentity m_BuilderID;
	public IObstacleInfo CurrentWorkTarget { get; set; }
	
	public BuilderInformation(int builderNO)
	{
		this.m_BuilderID = new BuildingIdentity(ConfigUtilities.Enums.BuildingType.BuilderHut, builderNO);
	}
	
	public BuildingIdentity BuilderID { get { return this.m_BuilderID; } }
	
	public int TotalWorkload
	{
		get
		{
			if(this.CurrentWorkTarget == null)
			{
				return 0;
			}
			else
			{
				if(this.CurrentWorkTarget is IBuildingInfo)
				{
					BuildingLogicData data = (BuildingLogicData)this.CurrentWorkTarget;
					return data.UpgradeWorkload;
				}
				else
				{
					RemovableObjectLogicData data = (RemovableObjectLogicData)this.CurrentWorkTarget;
					return data.RemoveWorkload;
				}
			}
		}
	}
	
	public float RemainingWorkload
	{
		get
		{
			if(this.CurrentWorkTarget == null)
			{
				return 0;
			}
			else
			{
				if(this.CurrentWorkTarget is IBuildingInfo)
				{
					BuildingLogicData data = (BuildingLogicData)this.CurrentWorkTarget;
					return data.UpgradeRemainingWorkload;
				}
				else
				{
					RemovableObjectLogicData data = (RemovableObjectLogicData)this.CurrentWorkTarget;
					return data.RemoveRemainingWorkload;
				}
			}
		}
	}
	
	public float Efficiency
	{
		get
		{
            int level = LogicController.Instance.GetBuildingObject(this.m_BuilderID).Level;
			return ConfigInterface.Instance.BuilderConfigHelper.GetBuilderData(level).BuildEfficiency;
		}
	}
}
