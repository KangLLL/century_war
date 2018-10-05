using UnityEngine;
using System.Collections;

public class BuilderData 
{
	private BuilderInformation m_Data;
		
	public BuilderData(BuilderInformation data)
	{
		this.m_Data = data;
	}
	
	public IObstacleInfo CurrentWorkTarget { get { return this.m_Data.CurrentWorkTarget; } }
	
	public float RemainingWorkload { get { return this.m_Data.RemainingWorkload; } }
	public int TotalWorkload { get { return this.m_Data.TotalWorkload; } }
	
	public float Efficiency { get { return this.m_Data.Efficiency; } }
	public BuildingIdentity BuilderID { get { return this.m_Data.BuilderID; } }
}
