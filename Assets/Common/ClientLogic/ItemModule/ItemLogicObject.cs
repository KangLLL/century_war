using UnityEngine;
using System.Collections;

public class ItemLogicObject : LogicObject, IProduciable<ItemIdentity>
{
	private ItemData m_Data;
	
	public ItemLogicObject(ItemData data)
	{
		this.m_Data = data;
	}
	
	public bool Produce(float efficiency, float seconds, out float remainingSeconds)
	{
		float remainingTime = this.m_Data.ProduceRemainingWorkload / efficiency;
		if(seconds >= remainingTime)
		{
			this.m_Data.ProduceRemainingWorkload = 0;
			remainingSeconds = seconds - remainingTime;
			return true;
		}
		else
		{
			this.m_Data.ProduceRemainingWorkload -= efficiency * seconds;
			remainingSeconds = 0;
			return false;
		}
	}
	
	public void FloorOutput ()
	{
		this.m_Data.ProduceRemainingWorkload = Mathf.CeilToInt(this.m_Data.ProduceRemainingWorkload);
	}
	
	public void Reset ()
	{
		this.m_Data.ProduceRemainingWorkload = this.m_Data.ConfigData.ProduceWorkload;
	}
	
	public int ProduceRemainingWorkload { get { return Mathf.CeilToInt(this.m_Data.ProduceRemainingWorkload); } }
	public float LogicProduceRemainingWorkload { get { return this.m_Data.ProduceRemainingWorkload; } }
	public ItemIdentity Identity { get { return this.m_Data.ItemID; } }
	public int ProduceTotalWorkload { get { return this.m_Data.ConfigData.ProduceWorkload; } } 
}
