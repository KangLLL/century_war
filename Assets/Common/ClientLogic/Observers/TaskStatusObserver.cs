using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskStatusObserver : LogicObserver<TaskCompleteNotification> 
{
	private List<int> m_AlreadyNotifyTasks;

	private static TaskStatusObserver s_Sigleton;
	
	public static TaskStatusObserver Instance
	{
		get { return s_Sigleton; }
	}
	
	void Awake()
	{
		s_Sigleton = this;
	}
	
	void OnDestroy()
	{
		s_Sigleton = null;
	}
	
	public override void Start () 
	{
		this.m_AlreadyNotifyTasks = new List<int>();
		base.Start();
	}

	void Update () 
	{
		if(LogicController.Instance.TaskManager != null)
		{
			foreach (Task t in LogicController.Instance.TaskManager.TaskList) 
			{
				if(t.Status == TaskStatus.Completed && !this.m_AlreadyNotifyTasks.Contains(t.TaskID))
				{
					this.m_NotificationQueue.Enqueue(new TaskCompleteNotification() { Task = t });
					this.m_AlreadyNotifyTasks.Add(t.TaskID);
				}
			}
		}
	}

	public override void StartObserve()
	{
		base.StartObserve();

		this.m_AlreadyNotifyTasks.Clear();
		foreach (Task t in LogicController.Instance.TaskManager.TaskList) 
		{
			if(t.Status == TaskStatus.Completed)
			{
				this.m_AlreadyNotifyTasks.Add(t.TaskID);
			}
		}
	}
}
