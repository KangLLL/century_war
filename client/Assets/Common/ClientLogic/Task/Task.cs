using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;
using System;
using UnityEngine;

public class Task 
{
	private TaskManager m_Manager;
    private List<Condition> m_ConditionList;
	public int TaskID { get; private set; }
	public TaskConfigData TaskConfigData { get; private set; }
	public TaskStatus Status { get; private set; }
	public Nullable<int> RemainingSeconds 
	{ 
		get
		{
			if(this.m_RemainingSeconds.HasValue)
			{
				return Mathf.CeilToInt(this.m_RemainingSeconds.Value);
			}
			else
			{
				return null;
			}
		}
	}
		
	private Nullable<float> m_RemainingSeconds;
	
	public string Description 
	{
		get
		{
			string[] conditionDescriptions = new string[this.m_ConditionList.Count];
			for(int i = 0; i < conditionDescriptions.Length; i ++)
			{
				conditionDescriptions[i] = this.m_ConditionList[i].Description;
			}
			string result = string.Join("\n\n",conditionDescriptions);
			return result;
		}
	}

	private Condition GenerateCondition(TaskConditionConfigData data, int conditionID, TaskProgressInformation progress)
	{
		Condition condition = null;
		switch (data.ConditionType)
    	{
    	    case TaskConditionType.UpgradeBuildingCondition:
    	        condition = new UpgradeBuildingCondition(data, this, conditionID, progress.StartValue, progress.CurrentValue);
    	        break; 
    	    case TaskConditionType.ConstructBuildingCondition:
    	        condition = new ConstructBuildingCondition(data, this, conditionID, progress.StartValue, progress.CurrentValue);
    	        break;
    	    case TaskConditionType.ProduceArmyCondition:
    	        condition = new ProduceArmyCondition(data, this, conditionID, progress.StartValue, progress.CurrentValue);
    	        break;
    	    case TaskConditionType.UpgradeArmyCondition:
    	        condition = new UpgradeArmyCondition(data, this, conditionID, progress.StartValue, progress.CurrentValue);
    	        break;
    	    case TaskConditionType.HonourCondition:
    	        condition = new HonourCondition(data, this, conditionID, progress.StartValue, progress.CurrentValue);
    	        break;
    	    case TaskConditionType.PlunderCondition:
    	        condition = new PlunderCondition(data, this, conditionID, progress.StartValue, progress.CurrentValue);
    	        break;
    	    case TaskConditionType.RemoveObjectCondition:
    	        condition = new RemoveObjectCondition(data, this, conditionID, progress.StartValue, progress.CurrentValue);
    	        break;
    	    case TaskConditionType.DestroyBuildingCondition:
    	        condition = new DestroyBuildingCondition(data, this, conditionID, progress.StartValue, progress.CurrentValue);
    	        break;
			case TaskConditionType.HireMercenaryCondition:
				condition = new HireMercenaryCondition(data,this,conditionID,progress.StartValue,progress.CurrentValue);
				break;
    	}
		return condition;
	}
	
	public void Process(float elapsedSecond)
	{
		if(this.Status == TaskStatus.Opened && this.m_RemainingSeconds.HasValue)
		{
			if(this.m_RemainingSeconds.Value <= elapsedSecond)
			{
				float remainingSecond = Mathf.Max(0, elapsedSecond - this.m_RemainingSeconds.Value);
				this.m_RemainingSeconds = 0;
				this.m_Manager.TimeOutTask(this, remainingSecond);
			}
			else
			{
				this.m_RemainingSeconds = this.m_RemainingSeconds.Value - elapsedSecond;
			}
		}
	}

    public Task(TaskInformation taskInformation, TaskManager manager)
    {
		this.m_Manager = manager;
		
		this.TaskID = taskInformation.TaskID;
		this.m_RemainingSeconds = taskInformation.RemainingSeconds;
		this.TaskConfigData = ConfigInterface.Instance.TaskConfigHelper.GetTaskData(this.TaskID);
		this.m_ConditionList = new List<Condition>();
		if(taskInformation.Status == TaskStatus.Completed)
		{
			foreach (KeyValuePair<int,TaskConditionConfigData> tcd in this.TaskConfigData.Conditions) 
			{
				this.m_ConditionList.Add(this.GenerateCondition(tcd.Value,tcd.Key,new TaskProgressInformation()));
			}
			this.Status = TaskStatus.Completed;
		}
		else
		{
			if(this.m_RemainingSeconds.HasValue && Mathf.Approximately(0.0f, this.m_RemainingSeconds.Value))	
			{
				this.m_Manager.TimeOutTask(this, 0);
			}
			else
			{
				bool isCompleted = true;
		        foreach (KeyValuePair<int,TaskConditionConfigData> tcd in this.TaskConfigData.Conditions)
		        {
					Condition condition = this.GenerateCondition(tcd.Value,tcd.Key,taskInformation.ConditionProgresses[tcd.Key]);
					this.m_ConditionList.Add(condition);
					if(!condition.IsComplete)
					{
						isCompleted = false;
					}		
		        }
				if(isCompleted)
				{
					this.CompleteTask();
				}
			}
		}
    }

    public virtual void OnUpgradeBuilding(BuildingType buildingType, int level)
    {
		bool isCompleted = true;
		foreach (Condition condition in m_ConditionList) 
		{
			condition.OnUpgradeBuilding(buildingType, level);
			if(!condition.IsComplete)
			{
				isCompleted = false;
			}
		}
		if(isCompleted)
		{
			this.CompleteTask();
		}
    }
	
    public virtual void OnConstructBuilding(BuildingType buildingType)
    {
		bool isCompleted = true;
		foreach (Condition condition in m_ConditionList) 
		{
			condition.OnConstructBuilding(buildingType);
			if(!condition.IsComplete)
			{
				isCompleted = false;
			}
		}
		if(isCompleted)
		{
			this.CompleteTask();
		}
    }
	
    public virtual void OnProduceArmy(ArmyType armyType)
    {
		bool isCompleted = true;
		foreach (Condition condition in m_ConditionList) 
		{
			condition.OnProduceArmy(armyType);
			if(!condition.IsComplete)
			{
				isCompleted = false;
			}
		}
		if(isCompleted)
		{
			this.CompleteTask();
		}
    }

    public virtual void OnUpgradeArmy(ArmyType armyType, int level)
    {
		bool isCompleted = true;
		foreach (Condition condition in m_ConditionList) 
		{
			condition.OnUpgradeArmy(armyType, level);
			if(!condition.IsComplete)
			{
				isCompleted = false;
			}
		}
		if(isCompleted)
		{
			this.CompleteTask();
		}
    }
	
    public virtual void OnHonourChanged(int currentHonour)
    {
		bool isCompleted = true;
		foreach (Condition condition in m_ConditionList) 
		{
			condition.OnHonourChanged(currentHonour);
			if(!condition.IsComplete)
			{
				isCompleted = false;
			}
		}
		if(isCompleted)
		{
			this.CompleteTask();
		}
    }
	
    public virtual void OnPlunder(ResourceType resourceType, int number)
	{
		bool isCompleted = true;
		foreach (Condition condition in m_ConditionList) 
		{
			condition.OnPlunderResource(resourceType, number);
			if(!condition.IsComplete)
			{
				isCompleted = false;
			}
		}
		if(isCompleted)
		{
			this.CompleteTask();
		}
    }
	
    public virtual void OnRemoveObject(RemovableObjectType objectType)
    {
		bool isCompleted = true;
		foreach (Condition condition in m_ConditionList) 
		{
			condition.OnRemoveObject(objectType);
			if(!condition.IsComplete)
			{
				isCompleted = false;
			}
		}
		if(isCompleted)
		{
			this.CompleteTask();
		}
    }
	
    public virtual void OnDestroyBuilding(BuildingType buildingType)
    {
		bool isCompleted = true;
		foreach (Condition condition in m_ConditionList) 
		{
			condition.OnDestroyBuilding(buildingType);
			if(!condition.IsComplete)
			{
				isCompleted = false;
			}
		}
		if(isCompleted)
		{
			this.CompleteTask();
		}
    }
	
	public virtual void OnHireMercenary(MercenaryType mercenaryType)
	{
		bool isCompleted = true;
		foreach (Condition condition in m_ConditionList) 
		{
			condition.OnHireMercenary(mercenaryType);
			if(!condition.IsComplete)
			{
				isCompleted = false;
			}
		}
		if(isCompleted)
		{
			this.CompleteTask();
		}
	}
	
	private void CompleteTask()
	{
		this.Status = TaskStatus.Completed;
		this.m_Manager.CompleteTask(this);
	}
	
	public void OpenChildrenTask()
	{
		foreach (int childID in this.TaskConfigData.Children) 
		{
			TaskInformation info = new TaskInformation();
			info.Status = TaskStatus.Opened;
			info.TaskID = childID;
			info.ConditionProgresses = new Dictionary<int, TaskProgressInformation>();
			TaskConfigData configData = ConfigInterface.Instance.TaskConfigHelper.GetTaskData(childID);
			if(configData.ValidSeconds > 0)
			{
				info.RemainingSeconds = configData.ValidSeconds;
			}
			foreach (KeyValuePair<int, TaskConditionConfigData> condition in configData.Conditions) 
			{
				if(!condition.Value.IsGlobal)
				{
					info.ConditionProgresses.Add(condition.Key, new TaskProgressInformation(){ 
						StartValue = TaskProgressFactory.GetCurrentValueFromConfig(condition.Value) });
				}
			}
			TaskProgressFactory.PopulateTaskInformation(info);
			Task childTask = new Task(info, this.m_Manager);
			LogicController.Instance.TaskManager.AddTask(childTask);
		}
	}
}
