using UnityEngine;
using System.Collections;

public class SimulateTimeTips : TimeTips 
{
	private int m_StartTime;

	void Start () 
	{
		this.m_StartTime = TimeTickRecorder.Instance.CurrentTimeTick;
	}
	
	void Update () 
	{
		int seconds =  Mathf.RoundToInt( (TimeTickRecorder.Instance.CurrentTimeTick - this.m_StartTime) / (float)ClientConfigConstants.Instance.TicksPerSecond);
		this.DisplayTime(seconds);
	}
}
