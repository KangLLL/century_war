using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingBasePropertyBehavior : ObstaclePropertyBehavior 
{
	[SerializeField]
	private string m_BuildingFacilityPrefabPath;
	
	private Transform m_CachedAnchorTransform;
	
	private int m_BuildingNO;
	private BuildingCategory m_BuildingCategory;
	
	private List<BuildingBuff> m_Buffs;
	
	public int BuildingNO
	{
		get
		{
			return this.m_BuildingNO;
		}
		set
		{
			this.m_BuildingNO = value;
		}
	}
	
	public BuildingCategory BuildingCategory
	{
		get
		{
			return this.m_BuildingCategory;
		}
		set
		{
			this.m_BuildingCategory = value;
		}
	}
	
	public string FacilityPrefabPath
	{
		get
		{
			return this.m_BuildingFacilityPrefabPath;
		}
	}
	
	public Transform AnchorTransform
	{
		get
		{
			if(this.m_CachedAnchorTransform == null)
			{
				this.m_CachedAnchorTransform = this.transform.FindChild(ClientStringConstants.BUILDING_ANCHOR_OBJECT_NAME);
			}
			return this.m_CachedAnchorTransform;
		}
	}
	
	public List<BuildingBuff> Buffs
	{
		get
		{
			return this.m_Buffs;
		}
		set
		{
			this.m_Buffs = value;
		}
	}
	
	public int BuffHPEffect
	{
		get 
		{
			int effect = 0;
			foreach (BuildingBuff buff in this.m_Buffs) 
			{
				effect += buff.HPEffect;
			}
			return effect;
		}
	}
	
	public int BuffAttackValueEffect
	{
		get
		{
			int effect = 0;
			foreach (BuildingBuff buff in this.m_Buffs) 
			{
				effect += buff.AttackValueEffect;
			}
			return effect;
		}
	}
	
	public int BuffAttackSpeedEffect
	{
		get
		{
			int effect = 0;
			foreach (BuildingBuff buff in this.m_Buffs) 
			{
				effect += buff.AttackSpeedEffect;
			}
			return effect;
		}
	}
}
