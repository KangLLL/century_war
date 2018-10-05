using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;
using CommandConsts;

public class PropsModule  
{
	private Dictionary<int, PropsLogicObject> m_Props;
	private int m_PropsStartNo;
	
	public void InitializeProps(Dictionary<int, PropsData> props, int PropsStartNo)
	{
		this.m_Props = new Dictionary<int, PropsLogicObject>();
		foreach (KeyValuePair<int, PropsData> p in props) 
		{
			this.m_Props.Add(p.Key, new PropsLogicObject(p.Value));
		}
		this.m_PropsStartNo = PropsStartNo;
	}
	
	public void Process()
	{
		foreach (KeyValuePair<int, PropsLogicObject> props in this.m_Props) 
		{
			props.Value.Process();
		}
	}
	
	public PropsLogicObject GetPropsLogicData(int propsNo)
	{
		return this.m_Props[propsNo];
	}
	
	public int PropsCount { get { return this.m_Props.Count; } }
	
	public int GetNextPropsNo()
	{
		return ++this.m_PropsStartNo;
	}
	
	public void ReversePropsNo()
	{
		this.m_PropsStartNo--;
	}
	
	public List<PropsLogicData> AllProps
	{
		get
		{
			List<PropsLogicData> result = new List<PropsLogicData>();
			foreach (KeyValuePair<int, PropsLogicObject> props in this.m_Props) 
			{
				result.Add(props.Value.Data);
			}
			return result;
		}
	}
	
	public void GenerateProps(int propsNo, PropsType propsType)
	{
		PropsData data = new PropsData();
		PropsConfigData configData = ConfigInterface.Instance.PropsConfigHelper.GetPropsData(propsType);
		
		data.PropsNo = propsNo;
		data.RemainingUseTime = configData.MaxUseTimes;
		data.RemainingCD = configData.CD;
		data.PropsConfigData = configData;
		data.PropsType = propsType;
		data.IsInBattle = false;
		
		PropsLogicObject logicObject = new PropsLogicObject(data);
		this.m_Props.Add(propsNo, logicObject);
	}
	
	public void GenerateProps(PropsType propsType)
	{
		this.GenerateProps(++this.m_PropsStartNo, propsType);
	}
	
	public void UseProps(int propsNo)
	{
		PropsLogicObject logicObject = this.m_Props[propsNo];
		logicObject.Use();
		if(logicObject.Data.RemainingUseTime == 0)
		{
			this.m_Props.Remove(propsNo);
		}
	}
	
	public void DestroyProps(int propsNo)
	{
		this.m_Props.Remove(propsNo);
		CommunicationUtility.Instance.DestroyProps(new DestroyPropsRequestParameter() { PropsNo = propsNo });
	}
	
	public List<int> BuildAchievementBuilding(AchievementBuildingType type)
	{
		List<int> result = new List<int>();
		AchievementBuildingConfigData buildingConfigData = ConfigInterface.Instance.AchievementBuildingConfigHelper.GetAchievementBuildingData(type);
		int number = 0;
		foreach (KeyValuePair<int, PropsLogicObject> props in this.m_Props) 
		{
			if(props.Value.Data.PropsType == buildingConfigData.NeedPropsType && props.Value.Data.RemainingCD == 0)
			{
				result.Add(props.Key);
				if(++number == buildingConfigData.NeedPropsNumber)
				{
					break;
				}
			}
		}
		
		foreach(int propsNo in result)
		{
			this.m_Props.Remove(propsNo);
		}
		
		return result;
	}
	
	public List<int> RepairAchievementBuilding(AchievementBuildingLogicData achievementBuilding) 
	{
		List<int> result = new List<int>();
		int maxLife = achievementBuilding.MaxLife;
		int currentLife = achievementBuilding.Life;
		
		foreach (KeyValuePair<int, PropsLogicObject> props in this.m_Props) 
		{
			if(props.Value.Data.PropsType == achievementBuilding.NeedProps && props.Value.Data.RemainingCD == 0)
			{
				result.Add(props.Key);
				if(++currentLife == maxLife)
				{
					break;
				}
			}
		}
		
		foreach(int propsNo in result)
		{
			this.m_Props.Remove(propsNo);
		}
		
		return result;
	}
	
	public IEnumerable<int> AllBattleProps
	{
		get
		{
			foreach (KeyValuePair<int, PropsLogicObject> props in this.m_Props) 
			{
				if(props.Value.Data.IsInBattle)
				{
					yield return props.Key;
				}
			}
		}
	}
}
