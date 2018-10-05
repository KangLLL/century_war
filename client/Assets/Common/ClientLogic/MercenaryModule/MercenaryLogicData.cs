using UnityEngine;
using System.Collections;

public class MercenaryLogicData 
{
	private MercenaryData m_Data;
	
	public MercenaryLogicData(MercenaryData data)
	{
		this.m_Data = data;
	}
	
	public BuildingIdentity CampID { get { return this.m_Data.CampID; } }
	
	public string Name { get { return this.m_Data.ConfigData.Name; } }
	public string Description { get { return this.m_Data.ConfigData.Description; } }
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
}
