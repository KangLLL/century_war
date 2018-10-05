using UnityEngine;
using System.Collections;

public class FoodStoreBehavior : ResourceStoreBehavior 
{
	protected override float OriginalPercentage 
	{
		get 
		{
			return this.m_Property.OriginalFoodPercentageWithoutPlunder;
		}
	}
	
	protected override int CurrentValue 
	{
		get 
		{
			return this.m_Property.Food;
		}
	}
	
	protected override int TotalValue 
	{
		get 
		{
			return this.m_Property.OriginalFood;
		}
	}
}
