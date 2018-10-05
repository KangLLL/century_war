using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class ObjectUpgrade<T> : IProduciable<T>
{
	private T m_ObjectType;
	
	private float m_UpgradeRemainingWorkload;
	private int m_UpgradeEfficiency;
	
	public T Identity { get { return this.m_ObjectType; } }
	
	public ObjectUpgrade(T type, int remainingWorkload)
	{
		this.m_ObjectType = type;
		this.m_UpgradeRemainingWorkload = remainingWorkload;
	}

	#region IProduciable implementation
	public bool Produce (float efficiency, float seconds, out float remainingSeconds)
	{
		float workload = efficiency * seconds;
		this.m_UpgradeRemainingWorkload -= workload;
		this.m_UpgradeRemainingWorkload = Mathf.Max(0, this.m_UpgradeRemainingWorkload);
		remainingSeconds = 0;
		return this.m_UpgradeRemainingWorkload.IsZero();
	}

	public int ProduceRemainingWorkload 
	{
		get 
		{
			return Mathf.CeilToInt(this.m_UpgradeRemainingWorkload);
		}
	}
	
	public void FloorOutput ()
	{
		this.m_UpgradeRemainingWorkload = Mathf.CeilToInt(this.m_UpgradeRemainingWorkload);
	}
	
	public float LogicProduceRemainingWorkload
	{
		get
		{
			return this.m_UpgradeRemainingWorkload;
		}
	}
	
	public void Reset()
	{
	}
	#endregion
}
