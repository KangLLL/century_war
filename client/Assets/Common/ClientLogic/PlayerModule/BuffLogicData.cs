using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class BuffLogicData
{
	private BuffData m_Data;
	
	public BuffLogicData(BuffData data)
	{
		this.m_Data = data;
	}
	
	public int RemainingSeconds { get { return Mathf.CeilToInt(this.m_Data.RemainingCD); } }
	public PropsType PropsType { get { return this.m_Data.RelatedPropsType; } }
	public BuffCategory BuffCategory { get { return (BuffCategory)this.m_Data.BuffConfigData.BuffCategory; } }
	public PropsBuffType BuffType { get { return (PropsBuffType)this.m_Data.BuffConfigData.RelatedBuffType; } }
	public BuildingCategory BuffBuildingCategory { get { return (BuildingCategory)this.m_Data.BuffConfigData.RelatedBuildingCategory; } }
	public float Effect { get { return this.m_Data.BuffConfigData.Effect; } }
	public string PrefabName { get { return this.m_Data.BuffConfigData.PrefabName; } }
}
