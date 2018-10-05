using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;

public abstract class CampManager<T,P> 
{
	protected List<BuildingIdentity> m_Camps;
	
	public CampManager()
	{
		this.m_Camps = new List<BuildingIdentity>();
	}
	
	public int CampsTotalCapacity
	{
		get
		{
			int result = 0;
			foreach(BuildingIdentity id in this.m_Camps)
			{
				result += this.GetTotalCapacity(LogicController.Instance.GetBuildingObject(id));
			}
			return result;
		}	
	}
	
	public int CampsTotalAlreadyCapacity
	{
		get
		{
			int result = 0;
			foreach (BuildingIdentity id in this.m_Camps) 
			{
				result += this.GetAlreadyCapacity(LogicController.Instance.GetBuildingObject(id));
			}
			return result;
		}
	}
	
	public void AddCamp(BuildingIdentity id)
	{
		this.m_Camps.Add(id);
	}
	
	public BuildingIdentity? FindCamp(T type)
	{
		int capacityCost = this.GetCapacityCost(type);
		
		if(capacityCost + this.CampsTotalAlreadyCapacity > this.CampsTotalCapacity)
			return null;
		
		foreach (BuildingIdentity id in this.m_Camps) 
		{
			BuildingLogicData building = LogicController.Instance.GetBuildingObject(id);
			if(this.GetAlreadyCapacity(building) < this.GetTotalCapacity(building))
			{
				return id;
			}
		}
		return null;
	}
	
	protected abstract int GetCapacityCost(T type);
	public abstract Dictionary<T, List<P>> AvailableObjects { get; }
	protected abstract int GetTotalCapacity(BuildingLogicData building);
	protected abstract int GetAlreadyCapacity(BuildingLogicData building);
}
