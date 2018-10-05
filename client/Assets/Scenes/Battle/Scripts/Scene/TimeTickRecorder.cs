using UnityEngine;
using System.Collections;

public class TimeTickRecorder : MonoBehaviour 
{
	private static TimeTickRecorder s_Sigleton;
	private int m_TimeTick;
	private bool m_IsPause;

	public static TimeTickRecorder Instance
	{
		get
		{
			return s_Sigleton;
		}
	}
	
	public int CurrentTimeTick
	{
		get
		{
			return this.m_TimeTick;
		}
	}
	
	void Awake()
	{
		s_Sigleton = this;
	}
	
	public void PauseTimeTick()
	{
		this.m_IsPause = true;
	}
	
	public void ResumeTimeTick()
	{
		this.m_IsPause = false;
	}
	
	void Start () 
	{
		this.m_TimeTick = 0;
	}
	
	void FixedUpdate()
	{
		if(!this.m_IsPause)
		{
			this.m_TimeTick ++;
		}
	}
}
