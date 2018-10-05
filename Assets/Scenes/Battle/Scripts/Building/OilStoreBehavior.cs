using UnityEngine;
using System.Collections;

public class OilStoreBehavior : ResourceStoreBehavior 
{
	protected override float OriginalPercentage 
	{
		get 
		{
			return this.m_Property.OriginalOilPercentageWithoutPlunder;
		}
	}
	
	protected override int CurrentValue 
	{
		get 
		{
			return this.m_Property.Oil;
		}
	}
	
	protected override int TotalValue 
	{
		get 
		{
			return this.m_Property.OriginalOil;
		}
	}
}
