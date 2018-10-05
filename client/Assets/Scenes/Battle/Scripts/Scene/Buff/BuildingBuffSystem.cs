using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities;
using CommandConsts;

public class BuildingBuffSystem : MonoBehaviour 
{
	private static BuildingBuffSystem s_Sigleton;
	
	public static BuildingBuffSystem Instance
	{
		get { return s_Sigleton; }
	}
	
	private List<PropsBuffConfigData> m_Buffs;
	private Dictionary<BuildingCategory, List<PropsBuffConfigData>> m_CategoryBuffs;
	
	void Awake()
	{
		this.m_Buffs = new List<PropsBuffConfigData>();
		this.m_CategoryBuffs = new Dictionary<BuildingCategory, List<PropsBuffConfigData>>();
		s_Sigleton = this;
	}
	
	void OnDestroy()
	{
		s_Sigleton = null;
	}
	
	public void InitialBuff(List<BattleBuffParameter> buffs)
	{
		foreach (BattleBuffParameter buff in buffs) 
		{
			PropsBuffConfigData configData = ConfigInterface.Instance.PropsConfigHelper.GetPropsData(buff.RelatedPropsType).
				FunctionConfigData as PropsBuffConfigData;
			this.m_Buffs.Add(configData);
			
			BuildingCategory relatedCategory = (BuildingCategory)configData.RelatedBuildingCategory;
			
			if(relatedCategory == BuildingCategory.Any)
			{
				this.AddBuff(BuildingCategory.None, configData);
				this.AddBuff(BuildingCategory.Resource, configData);
				this.AddBuff(BuildingCategory.Defense, configData);
			}
			else
			{
				this.AddBuff(relatedCategory, configData);
			}
		}
	}
	
	private void AddBuff(BuildingCategory category, PropsBuffConfigData data)
	{
		if(!this.m_CategoryBuffs.ContainsKey(category))
		{
			this.m_CategoryBuffs.Add(category, new List<PropsBuffConfigData>());
		}
		this.m_CategoryBuffs[category].Add(data);
	}
	
	public void ClearBuff()
	{
		this.m_Buffs.Clear();
		this.m_CategoryBuffs.Clear();
	}
	
	public List<PropsBuffConfigData> AllBuffsData
	{
		get { return this.m_Buffs; }
	}
	
	public List<BuildingBuff> GetBuffs(BuildingCategory category)
	{
		List<BuildingBuff> result = new List<BuildingBuff>();
		if(this.m_CategoryBuffs.ContainsKey(category))
		{
			foreach(PropsBuffConfigData buffData in this.m_CategoryBuffs[category])
			{
				result.Add(this.ConstructBuff(buffData));
			}
		}
		return result;
	}
	
	private BuildingBuff ConstructBuff(PropsBuffConfigData configData)
	{
		PropsBuffType type = (PropsBuffType)configData.RelatedBuffType;
		switch(type)
		{
			case PropsBuffType.HP:
			{
				BuildingHPBuff hpBuff = new BuildingHPBuff(configData.Effect);
				return hpBuff;
			}
			case PropsBuffType.Attack:
			{
				BuildingAttackValueBuff attackBuff = new BuildingAttackValueBuff(configData.Effect);
				return attackBuff;
			}
			case PropsBuffType.AttackSpeed:
			{
				BuildingAttackSpeedBuff attackSpeedBuff = new BuildingAttackSpeedBuff(configData.Effect);
				return attackSpeedBuff;
			}
		}
		return null;
	}
	
}
