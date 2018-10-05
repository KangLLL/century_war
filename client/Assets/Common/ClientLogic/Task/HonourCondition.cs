using UnityEngine;
using System.Collections;
using ConfigUtilities;

public class HonourCondition : Condition
{
    private int m_Honour;
	
    public HonourCondition(TaskConditionConfigData conditionConfigData, Task task, int conditionID, int startValue, int currentValue)
        : base(conditionConfigData, task, conditionID, startValue, currentValue) 
    {
        this.m_Honour = conditionConfigData.Value1;
		this.IsComplete = (this.Progress >= this.m_Honour);
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
				int displayProgress = Mathf.Min(this.Progress, this.m_Honour);
				displayProgress = Mathf.Max(displayProgress, 0);
				string secondLine = string.Format("（{0}/{1}）", displayProgress, this.m_Honour);
				return firstLine + secondLine;
			}
		}
	}
	
	public override void OnHonourChanged (int currentHonour)
	{
		this.CurrentValue = currentHonour;
		this.IsComplete = (this.Progress >= this.m_Honour);
	}
}
