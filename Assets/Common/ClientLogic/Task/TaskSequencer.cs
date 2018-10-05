using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TaskSequencer  
{
	private List<Task> m_Tasks;
	private bool m_IsNeedSequence;
	
	private List<Task> m_OrderedTask;
	private TaskComparer m_Comparer;
	
	public TaskSequencer(List<Task> tasks)
	{
		this.m_Tasks = tasks;
		this.m_IsNeedSequence = true;
		this.m_Comparer = new TaskComparer();
	}
	
	public List<Task> SequencedTask
	{
		get
		{
			if(this.m_IsNeedSequence)
			{
				Task[] originalTask = new Task[this.m_Tasks.Count];
				this.m_Tasks.CopyTo(originalTask);
				this.m_OrderedTask = new List<Task>(originalTask);
				this.m_OrderedTask.Sort(this.m_Comparer);
				this.m_IsNeedSequence = false;
			}
			return this.m_OrderedTask;
		}
	}
	
	public void ReSequenceTask()
	{
		this.m_IsNeedSequence = true;
	}
	
	private class TaskComparer : IComparer<Task>
	{
	   public int Compare(Task x, Task y)
	   {
	      if (x == null)
	      {
	         if (y == null)
	         {
	            return 0;
	         }
	         else
	         {
	            return -1;
	         }
	      }
	      else
	      {
	         if (y == null)
	         {
	            return 1;
	         }
	         else
	         {
	            int retval =  y.Status.CompareTo(x.Status);
	            if (retval != 0)
				{
	               return retval;
	            }
	            else
	            {
	               return x.TaskID.CompareTo(y.TaskID);
	            }
	         }
	      }
	   }
	}
}
