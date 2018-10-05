using UnityEngine;
using System.Collections;

public static class MathHelper
{
	private const float EPSINON = 1E-05f;
	
	public static bool IsZero(this float a)
	{
		return Mathf.Abs(a) < EPSINON;
	}
	
	public static bool IsPositive(this float a)
	{
		return a >= EPSINON;
	}
	
	public static bool IsNegative(this float a)
	{
		return a <= -EPSINON; 
	}
}
