using UnityEngine;
using System.Collections;

public class BuilderObject  
{
	private BuilderInformation m_Data;
	private BuilderData m_DataObject;
	
	public BuilderData BuilderData { get { return this.m_DataObject; } }
	
	public BuilderObject(BuilderInformation data)
	{
		this.m_Data = data;
		this.m_DataObject = new BuilderData(data);
	}
	
	public void Build(IObstacleInfo target)
	{
		this.m_Data.CurrentWorkTarget = target;
	}
	
	public void BuildOver()
	{
		this.m_Data.CurrentWorkTarget = null;
	}
}
