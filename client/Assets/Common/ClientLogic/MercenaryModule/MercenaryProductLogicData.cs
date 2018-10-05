using UnityEngine;
using System.Collections;
using System;

public class MercenaryProductLogicData  
{
	private MercenaryProductData m_Data;
	
	public MercenaryProductLogicData(MercenaryProductData data)
	{
		this.m_Data = data;
	}
	
	public int ReadyNumber { get { return this.m_Data.ReadyNumber; } }
	public Nullable<float> RemainingTime { get { return this.m_Data.RemainingTime; } }
	public int ProduceTime { get { return this.m_Data.ConfigData.ProduceTime; } }
	public int MaxProduceNumber { get { return this.m_Data.ConfigData.MaxProduceNumber; } }
	
	public int CapacityCost { get { return this.m_Data.ConfigData.CapcityCost; } }
	public float Velocity { get { return this.m_Data.ConfigData.MoveVelocity; } }
	public int AttackValue { get { return this.m_Data.ConfigData.AttackValue; } }
	public float AttackCD { get { return this.m_Data.ConfigData.AttackCD; } }
	public int AttackScope { get { return this.m_Data.ConfigData.AttackScope; } }
	public AttackType AttackType { get { return (AttackType)this.m_Data.ConfigData.AttackType; } }
	public TargetType TargetType { get { return (TargetType)this.m_Data.ConfigData.TargetType; } }
	public float AttackMiddleSpeed { get { return this.m_Data.ConfigData.AttackMiddleSpeed; } }
	public int DamageScope { get { return this.m_Data.ConfigData.DamageScope; } }
    public BuildingCategory FavoriteBuilding { get { return (BuildingCategory)this.m_Data.ConfigData.FavoriteType; } }
	public int HireGold { get { return this.m_Data.ConfigData.HireCostGold; } }
	public int HireFood { get { return this.m_Data.ConfigData.HireCostFood; } }
	public int HireOil { get { return this.m_Data.ConfigData.HireCostOil; } }
	public int HireGem { get { return this.m_Data.ConfigData.HireCostGem; } }
}
