using UnityEngine;
using System.Collections;

public class ArmyLogicObject : LogicObject, IProduciable<ArmyIdentity> 
{
	private ArmyData m_Data;
	private ArmyLogicData m_ArmyData;
	
	public ArmyLogicObject(ArmyData data)
	{
		this.m_Data = data;
		this.m_ArmyData = new ArmyLogicData(data);
	}
	
	public bool Produce(float efficiency, float seconds, out float remainingSeconds)
	{
		float remainingTime = this.m_Data.ProduceRemainingWorkload / efficiency;
		
		if(seconds >= remainingTime)
		{
			this.m_Data.ProduceRemainingWorkload = 0;
			remainingSeconds = seconds - remainingTime;
			return true;
		}
		else
		{
			this.m_Data.ProduceRemainingWorkload -= efficiency * seconds;
			remainingSeconds = 0;
			return false;
		}
	}
	
	public void FloorOutput ()
	{
		this.m_Data.ProduceRemainingWorkload = Mathf.CeilToInt(this.m_Data.ProduceRemainingWorkload);
	}
	
	public void Reset()
	{
		this.m_Data.ProduceRemainingWorkload = ConfigUtilities.ConfigInterface.Instance.ArmyConfigHelper.GetProduceWorkload
			(this.m_Data.ArmyID.armyType, LogicController.Instance.PlayerData.GetArmyLevel(this.m_Data.ArmyID.armyType));
	}
	
	public ArmyIdentity Identity { get { return this.m_Data.ArmyID; } }
	public float LogicProduceRemainingWorkload{ get { return this.m_Data.ProduceRemainingWorkload; } }
	public ArmyLogicData ArmyLogicData { get { return this.m_ArmyData; } } 
	
	public void AddArmyToCamp(BuildingIdentity campID)
	{
		this.m_Data.CampID = campID;
	}
}
