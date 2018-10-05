using UnityEngine;
using System.Collections;
using ConfigUtilities;
using ConfigUtilities.Enums;

public class UpgradeArmyCondition : Condition
{
    private ArmyType m_ArmyType;
    private int m_Level;
	
    public UpgradeArmyCondition(TaskConditionConfigData conditionConfigData, Task task, int conditionID, int startValue, int currentValue)
        : base(conditionConfigData, task, conditionID, startValue, currentValue) 
    {
        this.m_ArmyType = (ArmyType)conditionConfigData.Value1;
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
	
	public override void OnUpgradeArmy (ArmyType armyType, int newLevel)
	{
		if(armyType == this.m_ArmyType)
		{
			this.CurrentValue = newLevel;
			this.IsComplete = (this.Progress >= this.m_Level);
		}
	}
}
