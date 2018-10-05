using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using ConfigUtilities.Enums;
using ConfigUtilities;

public class ArmyInitialize : MonoBehaviour 
{
	[SerializeField]
	private List<SerializableArmy> m_Armies;
	[SerializeField]
	private List<SerializableMercenary> m_Mercenaries;
	[SerializeField]
	private List<SerializableProps> m_Props;
	
	
	[SerializeField]
	private BattlePreloadManager m_PreloadManager;
	
	void Awake () 
	{
		List<KeyValuePair<ArmyType, List<ArmyIdentity>>> armies = new List<KeyValuePair<ArmyType, List<ArmyIdentity>>>();
		
		foreach(SerializableArmy army in this.m_Armies)
		{
			List<ArmyIdentity> a = new List<ArmyIdentity>();
			for(int j = 0; j < army.count; j ++)
			{
				a.Add(new ArmyIdentity(army.type,j));
			}
			armies.Add(new KeyValuePair<ArmyType, List<ArmyIdentity>>(army.type,a));
		}
		
		List<MercenaryType> mercenaryTypes = new List<MercenaryType>();
		List<KeyValuePair<MercenaryType, List<MercenaryIdentity>>> mercenaries = new List<KeyValuePair<MercenaryType, List<MercenaryIdentity>>>();
		foreach(SerializableMercenary mercenary in this.m_Mercenaries)
		{
			List<MercenaryIdentity> m = new List<MercenaryIdentity>();
			for(int j = 0; j < mercenary.count; j ++)
			{
				m.Add(new MercenaryIdentity(mercenary.type,j));
			}
			mercenaries.Add(new KeyValuePair<MercenaryType, List<MercenaryIdentity>>(mercenary.type,m));
			mercenaryTypes.Add(mercenary.type);
		}
		
		List<KeyValuePair<PropsType, List<int>>> props = new List<KeyValuePair<PropsType, List<int>>>();
		int propsNo = 0;
		foreach (SerializableProps p in this.m_Props)
		{
			PropsConfigData configData = ConfigInterface.Instance.PropsConfigHelper.GetPropsData(p.type);
			if((PropsCategory)configData.Category == PropsCategory.Attack)
			{
				for(int j = 0; j < p.count; j ++)
				{
					List<int> propsNos = new List<int>();
					props.Add(new KeyValuePair<PropsType,List<int>>(p.type, propsNos));
					for(int i = 0; i < configData.MaxUseTimes; i ++)
					{
						propsNos.Add(propsNo);
					}
					propsNo ++;
				}
			}
		}
		
		ArmyMenuPopulator.Instance.AvailableArmies = armies;
		ArmyMenuPopulator.Instance.ArmyLevel = new Dictionary<ArmyType, int>();
		
		foreach (SerializableArmy army in m_Armies) 
		{
			ArmyMenuPopulator.Instance.ArmyLevel.Add(army.type, army.level);
		}
		ArmyMenuPopulator.Instance.AvailableMercenaries = mercenaries;
		ArmyMenuPopulator.Instance.AvailableProps = props;
		
		
		List<PropsType> propsTypes = new List<PropsType>();
		foreach (var p in props) 
		{
			if(!propsTypes.Contains(p.Key))
			{
				propsTypes.Add(p.Key);
			}
		}
		this.m_PreloadManager.Preload(ArmyMenuPopulator.Instance.ArmyLevel, mercenaryTypes, propsTypes);
		
	}
	
	[Serializable]
	private class SerializableArmy
	{
		public ArmyType type;
		public int level;
		public int count;
	}
	
	[Serializable]
	private class SerializableMercenary
	{
		public MercenaryType type;
		public int count;
	}
	
	[Serializable]
	private class SerializableProps
	{
		public PropsType type;
		public int count;
	}
}
