using UnityEngine;
using System.Collections;

public class FrameRelatedCriterion : IFinishable 
{
	private int m_CurrntFrame;
	private int m_TotalFrame;
	
	public FrameRelatedCriterion(int totalFrame)
	{
		this.m_TotalFrame = totalFrame;
		this.m_CurrntFrame = 0;
	}
	
	public void Reset()
	{
		this.m_CurrntFrame = 0;
	}
	
	public void Advance()
	{
		this.m_CurrntFrame ++;
	}
	
	public bool IsFinished()
	{
		return this.m_CurrntFrame == this.m_TotalFrame;
	}
}
