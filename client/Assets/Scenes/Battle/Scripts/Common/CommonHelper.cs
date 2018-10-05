using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class CommonHelper 
{
	private static System.Random r = new System.Random();
	
	public static PlatformType PlatformType { get;set; }
	public static bool IsDevelop { get;set; }   
	
	public static int GetRandomNumber(int min, int max)
	{
		return r.Next(min,max);
	}
	
	
	public static int GetCurrentSecond()
	{
		return (int)Time.realtimeSinceStartup;
	}
	
	public static int GetIndexFormRowColumn(int row, int column)
	{
		return column + row << 16;
	}
}
