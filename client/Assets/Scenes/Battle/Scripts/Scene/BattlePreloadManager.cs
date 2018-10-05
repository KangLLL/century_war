using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;

public class BattlePreloadManager : MonoBehaviour 
{
	private Dictionary<ArmyType, Dictionary<int, GameObject>> m_PreloadArmyPrefab = new Dictionary<ArmyType, Dictionary<int, GameObject>>();
	private Dictionary<MercenaryType, GameObject> m_PreloadMercenaryPrefab = new Dictionary<MercenaryType, GameObject>();
	private Dictionary<PropsType, GameObject> m_PreloadPropsPrefab = new Dictionary<PropsType, GameObject>();
	
	
	public void Preload(Dictionary<ArmyType, int> armies, List<MercenaryType> mercenaries, List<PropsType> props)
	{
		foreach (KeyValuePair<ArmyType, int> army in armies) 
		{
			this.PreloadArmy(army.Key, army.Value);
		}
		foreach (MercenaryType mercenaryType in mercenaries) 
		{
			this.PreloadMercenary(mercenaryType);
		}
		foreach (PropsType propsType in props) 
		{
			this.PreloadProps(propsType);
		}
	}
	
	private void PreloadArmy(ArmyType armyType, int level)
	{
		if(!this.m_PreloadArmyPrefab.ContainsKey(armyType) || !this.m_PreloadArmyPrefab[armyType].ContainsKey(level))
		{
			ArmyConfigData configData = ConfigInterface.Instance.ArmyConfigHelper.GetArmyData(armyType, level);
			string prefabPath = string.Format("{0}{1}{2}",ClientStringConstants.BATTLE_SCENE_RESOURCE_PREFAB_PREFIX_NAME,
				ClientStringConstants.ARMY_OBJECT_PREFAB_PREFIX_NAME, configData.PrefabName);
			GameObject armyPrefab = Resources.Load(prefabPath) as GameObject;
			if(!this.m_PreloadArmyPrefab.ContainsKey(armyType))
			{
				this.m_PreloadArmyPrefab.Add(armyType, new Dictionary<int, GameObject>());
			}
			
			if(!this.m_PreloadArmyPrefab[armyType].ContainsKey(level))
			{
				this.m_PreloadArmyPrefab[armyType].Add(level, armyPrefab);
			}
		}
	}
	
	private void PreloadMercenary(MercenaryType mercenaryType)
	{
		if(!this.m_PreloadMercenaryPrefab.ContainsKey(mercenaryType))
		{
			MercenaryConfigData configData = ConfigInterface.Instance.MercenaryConfigHelper.GetMercenaryData(mercenaryType);
			string prefabPath = string.Format("{0}{1}{2}",ClientStringConstants.BATTLE_SCENE_RESOURCE_PREFAB_PREFIX_NAME,
				ClientStringConstants.MERCENARY_OBJECT_PREFAB_PREFIX_NAME, configData.PrefabName);
			GameObject mercenaryPrefab = Resources.Load(prefabPath) as GameObject;
			this.m_PreloadMercenaryPrefab.Add(mercenaryType, mercenaryPrefab);
			
			KodoHPBehavior hp = mercenaryPrefab.GetComponent<KodoHPBehavior>();
			if(hp != null)
			{
				foreach(DropArmySerializableInformation army in hp.DropArmies)
				{
					this.PreloadArmy(army.ArmyType, army.ArmyLevel);
				}
				foreach(MercenaryType mercenary in hp.DropMercenaries)
				{
					this.PreloadMercenary(mercenary);
				}
			}
		}
	}
	
	private void PreloadProps(PropsType propsType)
	{
		object functionData = ConfigInterface.Instance.PropsConfigHelper.GetPropsData(propsType).FunctionConfigData;
			
		if(functionData is PropsScopeConfigData || functionData is PropsArmyConfigData || functionData is PropsMercenaryConfigData || 
			functionData is PropsTargetConfigData)
		{
			AttackPropsConfigWrapper configWrapper = new AttackPropsConfigWrapper(functionData);
			string prefabPath = string.Format("{0}{1}{2}",ClientStringConstants.BATTLE_SCENE_RESOURCE_PREFAB_PREFIX_NAME,
				ClientStringConstants.ATTACK_PROPS_PREFAB_PREFIX_NAME, configWrapper.PrefabName);
			GameObject propsPrefab = Resources.Load(prefabPath) as GameObject;
			if(!this.m_PreloadPropsPrefab.ContainsKey(propsType))
			{
				this.m_PreloadPropsPrefab.Add(propsType, propsPrefab);
			}
			
			if(functionData is PropsArmyConfigData)
			{
				this.PreloadArmy(((PropsArmyConfigData)functionData).ArmyType, ((PropsArmyConfigData)functionData).Level);
			}
			if(functionData is PropsMercenaryConfigData)
			{
				this.PreloadMercenary(((PropsMercenaryConfigData)functionData).MercenaryType);
			}
		}
	}
	
	public GameObject GetArmyPrefab(ArmyType armyType, int level)
	{
		GameObject result = this.m_PreloadArmyPrefab[armyType][level];
		return result;
	}
	
	public GameObject GetMercenaryPrefab(MercenaryType mercenaryType)
	{
		GameObject result = this.m_PreloadMercenaryPrefab[mercenaryType];
		return result;
	}
	
	public GameObject GetPropsPrefab(PropsType propsType)
	{
		GameObject result = this.m_PreloadPropsPrefab[propsType];
		return result;
	}
}
