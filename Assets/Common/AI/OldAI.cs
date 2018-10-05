using UnityEngine;
using System.Collections;

public class OldAI : MonoBehaviour 
{
	protected TilePosition m_TargetTilePosition;
	protected Vector3 m_TargetPosition;
	protected GameObject m_Target;
	
	[SerializeField]
	private int m_ResponseTime;
	
	protected int m_CurrentResponseCount;
	
	public TilePosition CurrentTargetPosition 
	{ 
		get
		{
			return this.m_TargetTilePosition;
		}
	}
	
	public GameObject CurrentTarget 
	{ 
		get
		{
			return this.m_Target;
		}
	}
	
	public Vector3 TargetPosition
	{
		get
		{
			return this.m_TargetPosition;
		}
	}
	
	public int ResponseTime
	{
		get
		{	
			return this.m_ResponseTime;
		}
	}
	
	public bool IsResponseable
	{
		get
		{
			return this.m_CurrentResponseCount == this.ResponseTime;
		}
	}
	
	public virtual void Start()
	{
		//this.m_CurrentResponseCount = CommonHelper.GetRandomNumber(0, this.ResponseTime);
	}
	
	public virtual void FixedUpdate()
	{
		if(this.m_CurrentResponseCount > 0)
		{
			this.m_CurrentResponseCount --;
		}
		else
		{
			this.m_CurrentResponseCount = this.ResponseTime;
		}
	}
}
