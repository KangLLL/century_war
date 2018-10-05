using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;

public class MercenaryProductLogicObject 
{
	private MercenaryType m_Type;
	private MercenaryProductLogicData m_LogicData;
	private MercenaryProductData m_Data;
	
	//private long m_LastTick;
	
	public MercenaryProductLogicData Data { get { return this.m_LogicData; } }
	public MercenaryType Type { get { return this.m_Type; } }
	
	public MercenaryProductLogicObject(MercenaryType type, MercenaryProductData data)
	{
		this.m_Type = type;
		this.m_Data = data;
		this.m_LogicData = new MercenaryProductLogicData(data);
	}
	
	public void Produce(float elapsedSecond)
	{
		if(this.m_Data.RemainingTime.HasValue)
		{
			float remaining = this.m_Data.RemainingTime.Value;
			while(elapsedSecond >= remaining && this.m_Data.ReadyNumber < this.m_LogicData.MaxProduceNumber)
			{
				this.m_Data.ReadyNumber ++;
				this.m_Data.RemainingTime = this.m_LogicData.ProduceTime;
				elapsedSecond -= remaining;
				remaining = this.m_LogicData.ProduceTime;
			}
			if(this.m_Data.ReadyNumber == this.m_LogicData.MaxProduceNumber)
			{
				this.m_Data.RemainingTime = null;
			}
			else
			{
				this.m_Data.RemainingTime -= elapsedSecond;
			}
			
			/*
			Debug.Log("remaining:" + this.m_Data.RemainingTime.Value + " , elapsed:" + elapsedSecond + ", current:" + LogicTimer.Instance.CurrentTime);
			
			if(this.m_LastTick != 0)
			{
				System.TimeSpan ts = new System.TimeSpan(System.DateTime.Now.Ticks - this.m_LastTick);
				Debug.Log("the seconds is:" + ts.TotalSeconds);
			}
			this.m_LastTick = System.DateTime.Now.Ticks;
			*/
		}
	}
	
	public void HireMercenary()
	{
		this.m_Data.ReadyNumber --;
		if(!this.m_Data.RemainingTime.HasValue)
		{
			this.m_Data.RemainingTime = this.m_LogicData.ProduceTime;
		}
	}
}
