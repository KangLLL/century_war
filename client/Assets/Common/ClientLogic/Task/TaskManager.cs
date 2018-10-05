using System.Collections;
using System.Collections.Generic;
using ConfigUtilities;
using ConfigUtilities.Enums;
using System.Linq;
using CommandConsts;

public class TaskManager
{
    List<Task> m_TaskList;
    public List<Task> TaskList { get { return this.m_Sequencer.SequencedTask; } }
	
	private List<Task> m_NewOpenTask;
	private List<Task> m_RemoveTask;
	private TaskSequencer m_Sequencer;
	private float m_PreviousSecond;
	
	public TaskManager()
	{
		this.m_TaskList = new List<Task>();
		this.m_NewOpenTask = new List<Task>();
		this.m_RemoveTask = new List<Task>();
		
		this.m_Sequencer = new TaskSequencer(this.m_TaskList);
	}
	
	public void Process()
	{
		float elapsedSecond = LogicTimer.Instance.CurrentTime - this.m_PreviousSecond;
		this.m_PreviousSecond = LogicTimer.Instance.CurrentTime;
		foreach (Task task in this.m_TaskList) 
		{
			task.Process(elapsedSecond);
		}
		
		this.RemoveTimeOutTask();
		this.AddNewTask();
		this.m_Sequencer.ReSequenceTask();
	}
	
	public void InitialTask(List<TaskInformation> taskInformationList)
    {
		foreach (TaskInformation ti in taskInformationList)
		{
			Task task = new Task(ti, this);
			m_TaskList.Add(task);
		}
		this.m_PreviousSecond = LogicTimer.Instance.CurrentTime;
		
		this.RemoveTimeOutTask();
		this.AddNewTask();
		this.m_Sequencer.ReSequenceTask();
    }
	
	public void OnUpgradeBuilding(BuildingType buildingType, int level)
    {
		foreach (Task task in this.m_TaskList) 
		{
			if(task.Status == TaskStatus.Opened)
			{
				task.OnUpgradeBuilding(buildingType, level);
			}
		}
    }
	
   	public void OnConstructBuilding(BuildingType buildingType)
    {
		foreach (Task task in this.m_TaskList) 
		{
			if(task.Status == TaskStatus.Opened)
			{
				task.OnConstructBuilding(buildingType);
			}
		}
    }
	
    public void OnProduceArmy(ArmyType armyType)
    {
		foreach (Task task in this.m_TaskList) 
		{
			if(task.Status == TaskStatus.Opened)
			{
				task.OnProduceArmy(armyType);
			}
		}
    }
	
    public void OnUpgradeArmy(ArmyType armyType, int level)
    {
		foreach (Task task in this.m_TaskList) 
		{
			if(task.Status == TaskStatus.Opened)
			{
				task.OnUpgradeArmy(armyType, level);
			}
		}
    }
	
    public void OnHonourChanged(int currentHonour)
    {
		foreach (Task task in this.m_TaskList) 
		{
			if(task.Status == TaskStatus.Opened)
			{
				task.OnHonourChanged(currentHonour);
			}
		}
    }
	
    public void OnPlunder(ResourceType resourceType, int number)
    {
		foreach (Task task in this.m_TaskList) 
		{
			if(task.Status == TaskStatus.Opened)
			{
				task.OnPlunder(resourceType, number);
			}
		}
    }
	
    public void OnRemoveObject(RemovableObjectType objectType)
    {
		foreach (Task task in this.m_TaskList) 
		{
			if(task.Status == TaskStatus.Opened)
			{
				task.OnRemoveObject(objectType);
			}
		}
    }
	
    public void OnDestroyBuilding(BuildingType buildingType)
    {
		foreach (Task task in this.m_TaskList) 
		{
			if(task.Status == TaskStatus.Opened)
			{
				task.OnDestroyBuilding(buildingType);
			}
		}
    } 
	
	public void OnHireMercenary(MercenaryType mercenaryType)
	{
		foreach (Task task in this.m_TaskList) 
		{
			if(task.Status == TaskStatus.Opened)
			{
				task.OnHireMercenary(mercenaryType);
			}
		}
	}
	
	public void TimeOutTask(Task task, float remainingSeond)
	{
		task.OpenChildrenTask();
		this.RemoveTask(task);
		
		TimeOutTaskRequestParameter request = new TimeOutTaskRequestParameter();
		request.TaskID = task.TaskID;
		request.OperateTick = LogicTimer.Instance.GetServerTick(remainingSeond);
		CommunicationUtility.Instance.TimeOutTask(request);
	}
	
	public void CompleteTask(Task task)
	{
		CompleteTaskRequestParameter request = new CompleteTaskRequestParameter();
		request.TaskID = task.TaskID;
		CommunicationUtility.Instance.CompleteTask(request);
		
		this.m_Sequencer.ReSequenceTask();
	}
	
	public void AwardTask(Task task)
	{
		task.OpenChildrenTask();
		this.m_TaskList.Remove(task);
		this.AddNewTask();
		this.m_Sequencer.ReSequenceTask();
	}
	
	public void AddTask(Task task)
	{
		this.m_NewOpenTask.Add(task);
		this.m_Sequencer.ReSequenceTask();
	}
	
	public void RemoveTask(Task task)
	{
		this.m_RemoveTask.Add(task);
		this.m_Sequencer.ReSequenceTask();
	}
	
	private void AddNewTask()
	{
		for(int i = 0; i < this.m_NewOpenTask.Count; i ++)
		{
			this.m_TaskList.Add(this.m_NewOpenTask[i]);
		}
		this.m_NewOpenTask.Clear();
	}
	
	private void RemoveTimeOutTask()
	{
		for(int i = 0; i < this.m_RemoveTask.Count; i ++)
		{
			this.m_TaskList.Remove(this.m_RemoveTask[i]);
		}
		this.m_RemoveTask.Clear();
	}
	
}
