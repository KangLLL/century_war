using UnityEngine;
using System.Collections;
using ConfigUtilities;

public class PlunderCondition : Condition
{
    private ResourceType m_ResourceType;
    private int m_Count;
    public PlunderCondition(TaskConditionConfigData conditionConfigData, Task task, int conditionID, int startValue, int currentValue)
        : base(conditionConfigData, task, conditionID, startValue, currentValue) 
    {
        this.m_ResourceType = (ResourceType)conditionConfigData.Value1;
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
	
	public override void OnPlunderResource (ResourceType resourceType, int number)
	{
		if(resourceType == this.m_ResourceType)
		{
			this.CurrentValue += number;
			this.IsComplete = (this.Progress >= this.m_Count);
		}
	}
}
