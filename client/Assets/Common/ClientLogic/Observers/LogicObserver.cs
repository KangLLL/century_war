using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogicObserver<T> : MonoBehaviour 
{
	protected Queue<T> m_NotificationQueue;
	
	public virtual void Start () 
	{
		this.m_NotificationQueue = new Queue<T>();
	}

	public virtual void StartObserve()
	{
		this.m_NotificationQueue.Clear();
	}

	public List<T> PumpNotifications()
	{
		List<T> result = new List<T>();
		while(this.m_NotificationQueue.Count > 0)
		{
			result.Add(this.m_NotificationQueue.Dequeue());
		}
		return result;
	}
}
