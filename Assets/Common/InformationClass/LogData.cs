using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using System;

public class LogData  
{
	public long MatchID { get; set; }
	public long RivalID { get; set; }
	public string RivalName { get; set; }
	public int RivalHonour { get; set; }
	public int RivalLevel { get; set; }
	public int PlunderGold { get; set; }
	public int PlumderFood { get; set; }
	public int PlunderOil { get; set; }
	public int PlunderHonour { get; set; }
	public Nullable<PropsType> PlunderProps { get;set; }
	public List<PropsType> PropsThropy { get; set; }
	
	public bool IsDestroyCityHall { get; set; }
	public int DestroyBuildingPercentage { get; set; }
	private int m_RankStar = -1;
	public int RankStar 
	{ 
		get 
		{ 
			if(this.m_RankStar < 0)
			{
				this.m_RankStar = this.DestroyBuildingPercentage < ClientConfigConstants.Instance.BattleProgressStep[0] ? 0 :
					this.DestroyBuildingPercentage < ClientConfigConstants.Instance.BattleProgressStep[1] ? 1 : 2;
				if(this.IsDestroyCityHall)
					this.m_RankStar ++;
			}
			return this.m_RankStar;
		} 
	} 
	public long ElapsedTime { get; set; }
	public Dictionary<ArmyType, DropArmyInfo> ArmyInfos { get; set; }
	public Dictionary<MercenaryType, int> MercenaryInfos { get; set; }
	public List<KeyValuePair<PropsType, int>> PropsInfos { get; set; }
	
	public bool IsReplayable { get { return ClientVersion.Instance.Version == this.Version; } } 
	public bool CanRevenge { get; set; }
	public string Version { get; set; }
}
