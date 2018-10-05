using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LogicObject 
{
	private List<LogicComponent> m_Components = new List<LogicComponent>();
	private List<LogicComponent> m_RemoveComponents = new List<LogicComponent>();
	
	/*
	public List<LogicComponent> Components
	{
		get
		{
			return m_Components;
		}
	}
	*/
	
	public void AddComponent(LogicComponent comp, int order)
	{
		comp.LogicObject = this;
		this.m_Components.Insert(order, comp);
	}
	
	public void AddComponent(LogicComponent comp)
	{
		comp.LogicObject = this;
		this.m_Components.Add(comp);
	}
	
	public void RemoveComponent(LogicComponent comp)
	{
		this.m_RemoveComponents.Add(comp);
	}
	
	public virtual void Process()
	{
		foreach(LogicComponent component in this.m_RemoveComponents)
		{
			this.m_Components.Remove(component);
		}
		this.m_RemoveComponents.Clear();
		foreach(LogicComponent component in this.m_Components)
		{
			component.Process();
		}
	}
}
