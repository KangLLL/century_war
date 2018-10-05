using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class FactoryManager<T> where T : struct
{
	private List<BuildingIdentity> m_Factories;
	
	public FactoryManager()
	{
		this.m_Factories = new List<BuildingIdentity>();
	}
	
	public int FactoriesTotalAlreadyCapacity
	{
		get
		{
			int result = 0;
			foreach(BuildingIdentity id in this.m_Factories)
			{
				result += this.GetAlreadyCapacity(LogicController.Instance.GetBuildingObject(id));
			}
			return result;
		}
	}
	
	public void AddFactory(BuildingIdentity factory)
	{
		this.m_Factories.Add(factory);
	}
	
	public Dictionary<BuildingIdentity, T> CurrentFinishProducts
	{
		get
		{
			Dictionary<BuildingIdentity, T> result = new Dictionary<BuildingIdentity, T>();
			foreach (BuildingIdentity id in this.m_Factories) 
			{
				T? finishedProduct = this.GetFinishedHeadProduct(id);
				if(finishedProduct.HasValue)
				{
					result.Add(id, finishedProduct.Value);
				}
			}
			return result;
		}
	}
	
	protected abstract Nullable<T> GetFinishedHeadProduct(BuildingIdentity id);
	
	protected abstract int GetTotalCapacity(BuildingLogicData building);
	protected abstract int GetAlreadyCapacity(BuildingLogicData building);
}
