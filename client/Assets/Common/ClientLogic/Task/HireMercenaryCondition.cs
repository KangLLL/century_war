using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
using ConfigUtilities;

public class HireMercenaryCondition : Condition 
{
    private MercenaryType m_MercenaryType;
    private int m_Count;
	
    public HireMercenaryCondition(TaskConditionConfigData conditionConfigData, Task task, int conditionID, int startValue, int currentValue)
        : base(conditionConfigData, task, conditionID, startValue, currentValue) 
    {
        this.m_MercenaryType = (MercenaryType)conditionConfigData.Value1;
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
	
	public override void OnHireMercenary (MercenaryType mercenaryType)
	{
		if(mercenaryType == this.m_MercenaryType)
		{
			this.CurrentValue ++;
			this.IsComplete = (this.Progress >= this.m_Count);
		}
	}
}
