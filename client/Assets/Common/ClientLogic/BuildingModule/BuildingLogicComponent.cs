using UnityEngine;
using System.Collections;

public class BuildingLogicComponent : LogicComponent 
{
	protected BuildingData m_BuildingData;
	
	public virtual void Initial(BuildingData data)
	{
		this.m_BuildingData = data;
	}
	
	public virtual void RefreshData()
	{
	}
}
