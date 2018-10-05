using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommandConsts;
using System;
using ConfigUtilities;
using ConfigUtilities.Enums;

public class RemovableObjectLogicObject : LogicObject 
{
	public event Action<int> RemoveTimeUp;
	
	private RemovableObjectLogicData m_LogicData;
	private RemovableObjectData m_Data;
	
	private static Dictionary<RemovableObjectType, List<KeyValuePair<PropsType, int>>> s_GeneratePropsDict = new Dictionary<RemovableObjectType, List<KeyValuePair<PropsType, int>>>();
	
	private RemoveLogicComponent m_RemoveComponent;
	
	public RemovableObjectLogicObject(RemovableObjectData data)
	{
		this.m_Data = data;
		this.m_LogicData = new RemovableObjectLogicData(data);
		
		if(data.RemainingWorkload.HasValue)
		{
			this.AddRemoveComponent ();
		}
	}
	
	public RemovableObjectLogicData LogicData
	{
		get { return this.m_LogicData; }
	}
	
	private void AddRemoveComponent ()
	{
		this.m_RemoveComponent  = new RemoveLogicComponent(this.m_Data);
		this.AddComponent(this.m_RemoveComponent);
		this.m_RemoveComponent.RemoveFinish += ObjectRemoveTimeUp;
	}
	
	public bool GenerateRewardData(int propsNo)
	{
		this.m_Data.RewardExp = CommonHelper.GetRandomNumber(this.m_Data.ConfigData.MinRewardExp, this.m_Data.ConfigData.MaxRewardExp + 1);
		this.m_Data.RewardGem = CommonHelper.GetRandomNumber(this.m_Data.ConfigData.MinRewardGem, this.m_Data.ConfigData.MaxRewardGem + 1);
		
		if(!s_GeneratePropsDict.ContainsKey(this.m_Data.RemovableObjectType))
		{
			s_GeneratePropsDict.Add(this.m_Data.RemovableObjectType, new List<KeyValuePair<PropsType, int>>());
			int lastRate = 0;
			foreach (KeyValuePair<PropsType,int> rate in this.m_Data.ConfigData.GeneratePropsRate) 
			{
				int newRate = lastRate + rate.Value;
				s_GeneratePropsDict[this.m_Data.RemovableObjectType].Add(new KeyValuePair<PropsType, int>(rate.Key, newRate));
				lastRate = newRate;
			}
		}
		
		this.m_Data.RewardPropsType = this.GetRandomPropsType();
		if(this.m_Data.RewardPropsType.HasValue)
		{
			this.m_Data.RewardProps = propsNo;
			return true;
		}
		return false;
	}
	
	 public Nullable<PropsType> GetRandomPropsType()
        {
            if (!s_GeneratePropsDict.ContainsKey(this.m_Data.RemovableObjectType))
            {
                return null;
            }
            else
            {
                List<KeyValuePair<PropsType, int>> rates = s_GeneratePropsDict[this.m_Data.RemovableObjectType];
                int r = CommonHelper.GetRandomNumber(0,100);
                for (int i = 0; i < rates.Count; i++)
                {
                    if (r < rates[i].Value)
                    {
                        return rates[i].Key;
                    }
                }
                return null;
            }
        }
	
	public void GeneratePosition(TilePosition position)
	{
		this.m_Data.Position = position;
		
		GenerateRemovableObjectRequestParameter request = new GenerateRemovableObjectRequestParameter();
		request.RemovableObjectNo = this.m_Data.RemovableObjectNo;
		request.PositionRow = position.Row;
		request.PositionColumn = position.Column;
		request.OperateTick = LogicTimer.Instance.GetServerTick();
		CommunicationUtility.Instance.GenerateRemovableObject(request);
	}
	
	public void Remove(int builderNO)
	{
		this.m_Data.BuilderBuildingNO = builderNO;
		this.m_Data.RemainingWorkload = this.m_Data.ConfigData.RemoveWorkload;
		
		this.AddRemoveComponent();
		RemoveObjectRequestParameter request = new RemoveObjectRequestParameter();
		request.RemovableObjectNo = this.m_Data.RemovableObjectNo;
		request.BuilderBuildingNO = builderNO;
		request.OperateTick = LogicTimer.Instance.GetServerTick();
		CommunicationUtility.Instance.RemoveObject(request);
	}
	
	public void CancelRemove()
	{
		this.RemoveComponent(this.m_RemoveComponent);
		this.m_RemoveComponent = null;
		this.m_Data.BuilderBuildingNO = null;
		this.m_Data.RemainingWorkload = null;
		CancelRemoveRequestParameter request = new CancelRemoveRequestParameter();
		request.RemovableObjectNo = this.m_Data.RemovableObjectNo;
		CommunicationUtility.Instance.CancelRemoveObject(request);
	}
	
	private void ObjectRemoveTimeUp(float remainingSeconds)
	{
		if(this.RemoveTimeUp != null)
		{
			this.RemoveTimeUp(this.m_Data.RemovableObjectNo);
		}
		this.m_RemoveComponent = null;
		this.m_Data.BuilderBuildingNO = null;
		RemoveTimeUpRequestParameter request = new RemoveTimeUpRequestParameter();
		request.RemovableObjectNo = this.m_Data.RemovableObjectNo;
		request.OperateTick = LogicTimer.Instance.GetServerTick(remainingSeconds);
		CommunicationUtility.Instance.TimeUpRemoveOject(request);
	}
	
	public void FinishRemove()
	{
		FinishRemoveObjectRequestParameter request = new FinishRemoveObjectRequestParameter();
		request.RemovableObjectNo = this.m_Data.RemovableObjectNo;
		request.OperateTick = LogicTimer.Instance.GetServerTick();
		CommunicationUtility.Instance.FinishRemoveObject(request);
	}
}
