using UnityEngine;
using System.Collections;

public class CurveCalculator
{
	private float G;
    private float m_Height;
    private float m_Time;
    private float m_V0;
	
	private int m_TotalFrames;
	private int m_CurrentFrame;
	
	public float Height
	{
		get 
		{
			return this.m_Height;
		}  
	}
	
	public Vector3 HeightRelatedVector
	{
		get
		{
			return new Vector3(0, this.m_Height, 0);
		}
	}
	
	public CurveCalculator(float g, int totalFrames)
	{
		this.G = g;
		this.m_V0 = -totalFrames * 0.03333333f / 2 * G;
		this.m_TotalFrames = totalFrames;
	}
	
	public void Initialize()
	{
	}
	
	public void Process()
	{
		if(this.m_CurrentFrame >= this.m_TotalFrames)
		{
			this.m_Height = 0;
		}
		else
		{
			this.m_Time += 0.03333333f;
	        this.m_Height = this.m_V0 * this.m_Time + this.G / 2 * this.m_Time * this.m_Time;
		}
		this.m_CurrentFrame ++;
	}
	
	
}
