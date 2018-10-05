using UnityEngine;
using System.Collections;
using System;

public class CDComponent : LogicComponent 
{
	private float m_PreviousTime;
	private ICD m_CD;
	
	public event Action<ICD> CDFinished;
	
	public CDComponent(ICD CDdata, float startTime)
	{
		this.m_PreviousTime = startTime;
		this.m_CD = CDdata;
	}
	
	public void ProduceTo(float time)
	{
		float elapsedTime = time - this.m_PreviousTime;
		this.m_PreviousTime = time;
		this.m_CD.RemainingCD -= elapsedTime;
		this.m_CD.RemainingCD = Mathf.Max(0, this.m_CD.RemainingCD);
		if(this.m_CD.RemainingCD == 0)
		{
			if(this.CDFinished != null)
			{
				this.CDFinished(this.m_CD);
			}
		}
	}
}
