using UnityEngine;
using System.Collections;
using CommandConsts;

public class PropsLogicObject : LogicObject 
{
	private PropsLogicData m_LogicData;
	private PropsData m_Data;
	
	private CDComponent m_CDComponent;
	
	public PropsLogicData Data { get { return this.m_LogicData; } }
	
	public PropsLogicObject(PropsData data)
	{
		this.m_Data = data;
		this.m_LogicData = new PropsLogicData(data);
		
		if(this.m_Data.RemainingCD != 0)
		{
			this.m_CDComponent = new CDComponent(data, LogicTimer.Instance.CurrentTime);
			this.AddComponent(this.m_CDComponent);
			this.m_CDComponent.CDFinished += CDFinished;
		}
	}
	
	public void Use()
	{
		this.m_Data.RemainingUseTime --;
	}
	
	public void AddInBattle()
	{
		this.m_Data.IsInBattle = true;
		
		OperateAttackPropsRequestParameter request = new OperateAttackPropsRequestParameter();
		request.PropsNo = this.m_Data.PropsNo;
		CommunicationUtility.Instance.AddPropsInBattle(request);
	}
	
	public void RemoveInBattle()
	{
		this.m_Data.IsInBattle = false;
		
		OperateAttackPropsRequestParameter request = new OperateAttackPropsRequestParameter();
		request.PropsNo = this.m_Data.PropsNo;
		CommunicationUtility.Instance.RemovePropsInBattle(request);
	}
	
	public override void Process ()
	{
		if(this.m_CDComponent != null)
		{
			this.m_CDComponent.ProduceTo(LogicTimer.Instance.CurrentTime);
		}
		base.Process ();
	}
	
	private void CDFinished(ICD propsData)
	{
		this.RemoveComponent(this.m_CDComponent);
		this.m_CDComponent = null;
	}
}
