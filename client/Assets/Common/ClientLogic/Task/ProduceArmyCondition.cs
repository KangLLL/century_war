using UnityEngine;
using System.Collections;
using ConfigUtilities;
using ConfigUtilities.Enums;

public class ProduceArmyCondition : Condition
{
    private ArmyType m_ArmyType;
    private int m_Count;
    public ProduceArmyCondition(TaskConditionConfigData conditionConfigData, Task task, int conditionID, int startValue, int currentValue)
        : base(conditionConfigData, task, conditionID, startValue, currentValue) 
    {
        this.m_ArmyType = (ArmyType)conditionConfigData.Value1;
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
	
	public override void OnProduceArmy (ArmyType armyType)
	{
		if(armyType == this.m_ArmyType)
		{
			this.CurrentValue ++;
			this.IsComplete = (this.Progress >= this.m_Count);
		}
	}
}
