using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
using ConfigUtilities;

public class ConstructBuildingCondition : Condition
{
    private BuildingType m_BuildingType;
    private int m_Count;
	
    public ConstructBuildingCondition(TaskConditionConfigData conditionConfigData, Task task, int conditionID, int startValue, int currentValue)
        : base(conditionConfigData, task, conditionID, startValue, currentValue) 
    {
        this.m_BuildingType = (BuildingType)conditionConfigData.Value1;
        this.m_Count = conditionConfigData.Value2;
		this.IsComplete = (this.Progress >= this.m_Count);
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
				int displayProgress = Mathf.Min(this.Progress, this.m_Count);
				displayProgress = Mathf.Max(displayProgress, 0);
				string secondLine = string.Format("（{0}/{1}）", displayProgress, this.m_Count);
				return firstLine + secondLine;
			}
		}
	}
	
	public override void OnConstructBuilding (BuildingType buildingType)
	{	
		if (buildingType == this.m_BuildingType)
		{
			this.CurrentValue ++;
			this.IsComplete = (this.Progress >= this.m_Count);
		}
	}
}
