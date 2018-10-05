using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class RecordUserCommand<T>  
{
	private UserCommand<T> m_ConstructCommand;
	private int m_DroppedFrame;
	
	public UserCommand<T> ConstructCommand
	{
		get { return this.m_ConstructCommand; }
		set { this.m_ConstructCommand = value; }
	}
	
	public int DroppedFrame
	{
		get { return this.m_DroppedFrame; } 
		set { this.m_DroppedFrame = value; }
	}
}
