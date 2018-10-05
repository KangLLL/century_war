using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using CommandConsts;
using ConfigUtilities.Enums;

public abstract class ReplayDataReader : MonoBehaviour 
{
	protected abstract string GetMapInformation();
	protected abstract string GetBattleInformation();
	
	public MatchLogResponseParameter GetReplayData()
	{
		MatchLogResponseParameter result = new MatchLogResponseParameter();
		result.RandomSeed = 0;
		result.RivalInformation = DataConvertor.ConvertJSONToParameter((Hashtable)(JSONHelper.jsonDecode(this.GetMapInformation())));
		
		Hashtable battleInfo = (Hashtable)JSONHelper.jsonDecode(this.GetBattleInformation());
		result.TotalTime = Convert.ToInt32(battleInfo[EditorConfigInterface.Instance.BattleTimeKey]);
		
		result.DropArmyCommands = new List<DropArmyResponseParameter>();
		result.DropMercenaryCommands = new List<DropMercenaryResponseParameter>();
		result.UsePropsCommands = new List<UsePropsResponseParameter>();
		
		if(battleInfo.ContainsKey(EditorConfigInterface.Instance.BattleArmyKey))
		{
			ArrayList dropArmies = (ArrayList)battleInfo[EditorConfigInterface.Instance.BattleArmyKey];
			foreach (var army in dropArmies) 
			{
				DropArmyResponseParameter param = new DropArmyResponseParameter();
				Hashtable a = (Hashtable)army;
				param.ArmyType = (ArmyType)(Convert.ToInt32(a[EditorConfigInterface.Instance.DropTypeKey]));
				param.Level = Convert.ToInt32(a[EditorConfigInterface.Instance.ArmyLevelKey]);
				param.PositionX = float.Parse(a[EditorConfigInterface.Instance.DropPositionXKey].ToString());
				param.PositionY = float.Parse(a[EditorConfigInterface.Instance.DropPositionYKey].ToString());
				param.OperateTime = Convert.ToInt32(a[EditorConfigInterface.Instance.DropTimeKey]);
				result.DropArmyCommands.Add(param);
			}
		}
		if(battleInfo.ContainsKey(EditorConfigInterface.Instance.BattleMercenaryKey))
		{
			ArrayList dropMercenaries = (ArrayList)battleInfo[EditorConfigInterface.Instance.BattleMercenaryKey];
			foreach (var mercenary in dropMercenaries) 
			{
				DropMercenaryResponseParameter param = new DropMercenaryResponseParameter();
				Hashtable a = (Hashtable)mercenary;
				param.MercenaryType = (MercenaryType)(Convert.ToInt32(a[EditorConfigInterface.Instance.DropTypeKey]));
				param.PositionX = float.Parse(a[EditorConfigInterface.Instance.DropPositionXKey].ToString());
				param.PositionY = float.Parse(a[EditorConfigInterface.Instance.DropPositionYKey].ToString());
				param.OperateTime = Convert.ToInt32(a[EditorConfigInterface.Instance.DropTimeKey]);
				result.DropMercenaryCommands.Add(param);
			}
		}
		
		if(battleInfo.ContainsKey(EditorConfigInterface.Instance.BattlePropsKey))
		{
			ArrayList useProps = (ArrayList)battleInfo[EditorConfigInterface.Instance.BattlePropsKey];
			foreach (var props in useProps) 
			{
				UsePropsResponseParameter param = new UsePropsResponseParameter();
				Hashtable p = (Hashtable)props;
				param.PropsType = (PropsType)(Convert.ToInt32(p[EditorConfigInterface.Instance.DropTypeKey]));
				param.PositionX = float.Parse(p[EditorConfigInterface.Instance.DropPositionXKey].ToString());
				param.PositionY = float.Parse(p[EditorConfigInterface.Instance.DropPositionYKey].ToString());
				param.OperateTime = Convert.ToInt32(p[EditorConfigInterface.Instance.DropTimeKey]);
				result.UsePropsCommands.Add(param);
			}
		}
		
		return result;
	}
}
