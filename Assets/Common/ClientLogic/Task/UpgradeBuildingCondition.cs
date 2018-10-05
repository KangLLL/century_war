using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;

public class UpgradeBuildingCondition : Condition
{
    private BuildingType m_BuildingType;
    private int m_Level;
	
    public UpgradeBuildingCondition(TaskConditionConfigData conditionConfigData, Task task, int conditionID, int startValue, int currentValue)
        : base(conditionConfigData, task, conditionID, startValue, currentValue) 
    {
        this.m_BuildingType = (BuildingType)conditionConfigData.Value1;
        this.m_Level = conditionConfigData.Value2;
		this.IsComplete = (this.Progress >= this.m_Level);
    }
	
	public override string Description 
	{
		get 
		{
            string firstLine = this.ConditionConfigData.Description;
			if(!this.ConditionConfigData.IsShowProgress || this.Task.Status == TaskStatus.Completed)
			{
				return firstLine;
			}
			else
			{
				int displayProgress = Mathf.Min(this.Progress, this.m_Level);
				displayProgress = Mathf.Max(displayProgress, 0);
				string secondLine = string.Format("（{0}/{1}）", displayProgress, this.m_Level);
				return firstLine + secondLine;
			}
		}
	}
	
	public override void OnUpgradeBuilding (BuildingType buildingType, int newLevel)
	{
		if(buildingType == this.m_BuildingType)
		{
			if(newLevel > this.CurrentValue)
			{
				this.CurrentValue = newLevel;
				this.IsComplete = (this.Progress >= this.m_Level);
			}
		}
	}
}
