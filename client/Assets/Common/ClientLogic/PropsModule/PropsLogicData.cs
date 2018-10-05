using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class PropsLogicData  
{
	private PropsData m_Data;
	
	public PropsLogicData(PropsData data)
	{
		this.m_Data = data;
	}
	
	public int PropsNo { get { return this.m_Data.PropsNo; } }
	public int RemainingCD { get { return Mathf.CeilToInt(this.m_Data.RemainingCD); } }
	public int RemainingUseTime { get { return this.m_Data.RemainingUseTime; } }
	public PropsType PropsType { get { return this.m_Data.PropsType; } }
	
	public string Name { get { return this.m_Data.PropsConfigData.Name; } }
	public string Description { get { return this.m_Data.PropsConfigData.Description; } }
	public PropsCategory Category { get { return (PropsCategory)this.m_Data.PropsConfigData.Category; } }
	public PropsQuality Quality { get { return (PropsQuality)this.m_Data.PropsConfigData.Quality; } }
	public int RequireLevel { get { return this.m_Data.PropsConfigData.RequireLevel; } }
	public int MaxUseTimes { get { return this.m_Data.PropsConfigData.MaxUseTimes; } }
	public int CD { get { return this.m_Data.PropsConfigData.CD; } }
	public bool IsInBattle { get { return this.m_Data.IsInBattle; } }
	public string PrefabName { get { return this.m_Data.PropsConfigData.PrefabName; } }
	public object FunctionConfigData { get { return this.m_Data.PropsConfigData.FunctionConfigData; } } 
}
