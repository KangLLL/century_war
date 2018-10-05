using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;

public class MercenaryModule  
{
	private Dictionary<MercenaryType, Dictionary<int ,MercenaryLogicData>> m_Mercenaries;
	
	public MercenaryModule()
	{
		this.m_Mercenaries = new Dictionary<MercenaryType, Dictionary<int, MercenaryLogicData>>();
	}
	
	public void InitializeMercenaries(Dictionary<MercenaryIdentity, MercenaryData> mercenaries)
	{
		foreach (KeyValuePair<MercenaryIdentity, MercenaryData> mercenary in mercenaries) 
		{
			if(!this.m_Mercenaries.ContainsKey(mercenary.Key.mercenaryType))
			{
				this.m_Mercenaries.Add(mercenary.Key.mercenaryType, new Dictionary<int, MercenaryLogicData>());
			}
			
			this.m_Mercenaries[mercenary.Key.mercenaryType].Add(mercenary.Key.mercenaryNO, new MercenaryLogicData(mercenary.Value));
		}
	}
	
	public MercenaryLogicData GetMercenaryData(MercenaryIdentity id)
	{
		return this.m_Mercenaries[id.mercenaryType][id.mercenaryNO];
	}
	
	public void HireMercenary(MercenaryIdentity id, BuildingIdentity campID)
	{
		MercenaryData data = new MercenaryData();
		data.CampID = campID;
		data.ConfigData = ConfigInterface.Instance.MercenaryConfigHelper.GetMercenaryData(id.mercenaryType);
		MercenaryLogicData logicData = new MercenaryLogicData(data);
		if(!this.m_Mercenaries.ContainsKey(id.mercenaryType))
		{
			this.m_Mercenaries.Add(id.mercenaryType, new Dictionary<int, MercenaryLogicData>());
		}
		this.m_Mercenaries[id.mercenaryType].Add(id.mercenaryNO, logicData);
	}
	
	public void DropMercenary(MercenaryIdentity id)
	{
		this.m_Mercenaries[id.mercenaryType].Remove(id.mercenaryNO);
		if(this.m_Mercenaries[id.mercenaryType].Count == 0)
		{
			this.m_Mercenaries.Remove(id.mercenaryType);
		}
	}
}
