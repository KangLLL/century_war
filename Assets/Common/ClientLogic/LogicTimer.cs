using UnityEngine;
using System.Collections;

public class LogicTimer  
{
	private static LogicTimer s_Sigleton;
	private float m_CurrentTime;

	private float m_StartTime;
	private long m_ServerTick;

	public static LogicTimer Instance
	{
		get
		{
			if(s_Sigleton == null)
			{
				s_Sigleton = new LogicTimer();
			}
			return s_Sigleton;
		}
	}

	public void InitialTimer(long serverTick)
	{
		this.m_StartTime = Time.realtimeSinceStartup;
		this.m_ServerTick = serverTick;
	}

	public long GetServerTick()
	{
		return this.GetServerTick(0);
	}

	public long GetServerTick(float remainingTime)
	{
		remainingTime = Mathf.Max(remainingTime, 0);
		double elapsedTime = ((double)this.CurrentTime) - remainingTime - this.m_StartTime;
		long result = (long)(System.Math.Floor(elapsedTime * System.TimeSpan.TicksPerSecond));
		return this.m_ServerTick + result;
	}
	
	public float CurrentTime
	{
		get
		{
			return this.m_CurrentTime;
		}
	}
	
	public void Process()
	{
		this.m_CurrentTime = Time.realtimeSinceStartup;
	}
}
