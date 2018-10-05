using UnityEngine;
using System.Collections;

public class ArmyLogicData
{
	private ArmyData m_Data;
	
	public ArmyLogicData(ArmyData data)
	{
		this.m_Data = data;
	}
	
    public ArmyCategory ArmyCategory{ get { return (ArmyCategory)this.m_Data.ConfigData.Category; } }
	public int CapacityCost { get { return this.m_Data.ConfigData.CapcityCost; } }
	public string Name { get { return this.m_Data.ConfigData.Name; } }
    public string Description { get { return this.m_Data.ConfigData.Description; } }
	public int Level { get { return LogicController.Instance.GetArmyLevel(this.m_Data.ArmyID.armyType); } }
	public float Velocity { get { return this.m_Data.ConfigData.MoveVelocity; } }
	public int AttackValue { get { return this.m_Data.ConfigData.AttackValue; } }
	public float AttackCD { get { return this.m_Data.ConfigData.AttackCD; } }
	public int AttackScope { get { return this.m_Data.ConfigData.AttackScope; } }
	public AttackType AttackType { get { return (AttackType)this.m_Data.ConfigData.AttackType; } }
	public TargetType TargetType { get { return (TargetType)this.m_Data.ConfigData.TargetType; } }
	public float AttackMiddleSpeed { get { return this.m_Data.ConfigData.AttackMiddleSpeed; } }
	public int DamageScope { get { return this.m_Data.ConfigData.DamageScope; } }
    public BuildingCategory DisplayFavoriteBuilding { get { return (BuildingCategory)this.m_Data.ConfigData.DisplayFavoriteType; } }
	
	public int ProduceRemainingWorkload { get { return Mathf.CeilToInt(this.m_Data.ProduceRemainingWorkload); } }
	public float LogicProduceRemainingWorkload { get { return this.m_Data.ProduceRemainingWorkload; } }
	public ArmyIdentity Identity { get { return this.m_Data.ArmyID; } }
	public int ProduceTotalWorkload { get { return this.m_Data.ConfigData.ProduceWorkload; } }
	public int UpgradeStep { get { return this.m_Data.ConfigData.UpgradeStep; } }
	
	public BuildingIdentity CampID { get { return this.m_Data.CampID; } }
}
