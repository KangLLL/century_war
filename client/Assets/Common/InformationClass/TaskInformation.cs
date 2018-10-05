using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum TaskStatus
{
	Opened,
	Completed
}

public class TaskProgressInformation
{
	public int StartValue { get;set; }
	public int CurrentValue { get;set; }
}

public class TaskInformation  
{
	public int TaskID { get;set; }
	public Dictionary<int, TaskProgressInformation> ConditionProgresses { get;set; } 
	public Nullable<float> RemainingSeconds { get;set; }
	public TaskStatus Status { get;set; }
}
