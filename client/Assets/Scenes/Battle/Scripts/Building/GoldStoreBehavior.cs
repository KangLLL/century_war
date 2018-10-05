using UnityEngine;
using System.Collections;

public class GoldStoreBehavior : ResourceStoreBehavior 
{
	protected override float OriginalPercentage 
	{
		get 
		{
			return this.m_Property.OriginalGoldPercentageWithoutPlunder;
		}
	}
	
	protected override int CurrentValue 
	{
		get 
		{
			return this.m_Property.Gold;
		}
	}
	
	protected override int TotalValue 
	{
		get 
		{
			return this.m_Property.OriginalGold;
		}
	}
}
