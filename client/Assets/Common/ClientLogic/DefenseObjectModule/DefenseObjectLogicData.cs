using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DefenseObjectLogicData  
{
	private DefenseObjectData m_Data;
	
	public DefenseObjectLogicData(DefenseObjectData data)
	{
		this.m_Data = data;
	}
	
	public long DefenseObjectID { get { return this.m_Data.DefenseObjectID; } }
	public string Name { get { return this.m_Data.Name; } }
	
	public TilePosition Position { get { return this.m_Data.Position; } }
	
	public int TriggerScope { get { return this.m_Data.ConfigData.TriggerScope; } }
	public int Scope { get { return this.m_Data.ConfigData.Scope; } }
	
	public List<TilePosition> BuildingObstacleList
	{
		get
		{
			return this.m_Data.ConfigData.BuildingObstacle;
		}
	}
	
	public string PrefabName { get { return this.m_Data.ConfigData.PrefabName; } }
}
