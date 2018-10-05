using UnityEngine;
using System.Collections;

public class InfinityCriterion : IFinishable  
{
	public void Reset()
	{
	}
	
	public bool IsFinished()
	{
		return false;
	}
}
