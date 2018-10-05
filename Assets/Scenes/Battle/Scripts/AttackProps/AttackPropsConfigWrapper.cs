using UnityEngine;
using System.Collections;
using ConfigUtilities;

public class AttackPropsConfigWrapper  
{
	public string PrefabName { get; private set; }
	
	public AttackPropsConfigWrapper(object configData)
	{
		if(configData is PropsScopeConfigData)
		{
			this.PrefabName = ((PropsScopeConfigData)configData).PrefabName;
		}
		else if(configData is PropsArmyConfigData)
		{
			this.PrefabName = ((PropsArmyConfigData)configData).PrefabName;
		}
		else if(configData is PropsMercenaryConfigData)
		{
			this.PrefabName = ((PropsMercenaryConfigData)configData).PrefabName;
		}
		else if(configData is PropsTargetConfigData)
		{
			this.PrefabName = ((PropsTargetConfigData)configData).PrefabName;
		}
	}
}
