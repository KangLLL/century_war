using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class BuildingPropertyBehavior : BuildingBasePropertyBehavior , IBuildingInfo
{
	private BuildingType m_BuildingType;
	private int m_Level;
	
	private int m_Gold;
	private int m_Food;
	private int m_Oil;
	
	private int m_OriginalGold;
	private int m_OriginalFood;
	private int m_OriginalOil;
	
	private int m_GoldCapacity;
	private int m_FoodCapacity;
	private int m_OilCapacity;
	
	private float m_OriginalGoldPercentageWithoutPlunderRate;
	private float m_OriginalFoodPercentageWithoutPlunderRate;
	private float m_OriginalOilPercentageWithoutPlunderRate;
	
	private float m_PlunderRate;
	
	public BuildingType BuildingType 
	{ 
		get
		{
			return this.m_BuildingType;
		}
		set
		{
			this.m_BuildingType = value;
		}
	}
	
	public int Level 
	{ 
		get
		{
			return this.m_Level;
		}
		set
		{
			this.m_Level = value;
		}
	}
	
	public int Gold 
	{ 
		get
		{
			return this.m_Gold;
		}
		set
		{
			this.m_Gold = value;
		} 
	}
	
	public int Food 
	{ 
		get
		{
			return this.m_Food;
		}
		set
		{
			this.m_Food = value;
		}
	}
	
	public int Oil 
	{ 
		get
		{
			return this.m_Oil;
		}
		set
		{
			this.m_Oil = value;
		}
	}
	
	public int OriginalGold
	{
		get
		{
			return this.m_OriginalGold;
		}
		set
		{
			this.m_OriginalGold = value;
		}
	}
	
	public int OriginalFood
	{
		get
		{
			return this.m_OriginalFood;
		}
		set
		{
			this.m_OriginalFood = value;
		}
	}
	
	public int OriginalOil
	{
		get
		{
			return this.m_OriginalOil;
		}
		set
		{
			this.m_OriginalOil = value;
		}
	}
	
	public int GoldCapacity
	{
		get
		{
			return this.m_GoldCapacity;
		}
		set
		{
			this.m_GoldCapacity = value;
		}
	}
	
	public int FoodCapacity
	{
		get
		{
			return this.m_FoodCapacity;
		}
		set
		{
			this.m_FoodCapacity = value;
		}
	}
	
	public int OilCapacity
	{
		get
		{
			return this.m_OilCapacity;
		}
		set
		{
			this.m_OilCapacity = value;
		}
	}
	
	public float PlunderRate 
	{ 
		get
		{
			return this.m_PlunderRate;
		}
		set
		{
			this.m_PlunderRate = value;
		}
	}
	
	public float OriginalGoldPercentageWithoutPlunder
	{
		get
		{
			return this.m_OriginalGoldPercentageWithoutPlunderRate;
		}
		set
		{
			this.m_OriginalGoldPercentageWithoutPlunderRate = value;
		}
	}
	
	public float OriginalFoodPercentageWithoutPlunder
	{
		get
		{
			return this.m_OriginalFoodPercentageWithoutPlunderRate;
		}
		set
		{
			this.m_OriginalFoodPercentageWithoutPlunderRate = value;
		}
	}
	
	public float OriginalOilPercentageWithoutPlunder
	{
		get
		{
			return this.m_OriginalOilPercentageWithoutPlunderRate;
		}
		set
		{
			this.m_OriginalOilPercentageWithoutPlunderRate = value;
		}
	}
}
