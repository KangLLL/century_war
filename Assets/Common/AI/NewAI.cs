using UnityEngine;
using System.Collections;

public class NewAI : MonoBehaviour 
{
	protected AIState m_CurrentState;
	
	[SerializeField]
	private int m_ResponseTime;
	
	protected int m_CurrentResponseCount;
	
	private int m_GlobalAttackCD;
	
	public int ResponseTime
	{
		get
		{	
			return this.m_ResponseTime;
		}
	}
	
	public bool CanTakeResponse
	{
		get
		{
			return this.m_CurrentResponseCount == this.ResponseTime;
		}
	}
	
	public bool CanAttack
	{
		get
		{
			return this.m_GlobalAttackCD == 0;
		}
	}
	
	public virtual void Start()
	{
		this.m_CurrentResponseCount = CommonHelper.GetRandomNumber(0, this.ResponseTime);
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
		if(this.m_GlobalAttackCD > 0)
		{
			this.m_GlobalAttackCD --;
		}
		this.m_CurrentState.AICalculate();
	}
	
	public void ChangeState(AIState newState)
	{
		this.m_CurrentState = newState;
		newState.Initial();
	}
	
	public void ResetAttackCD(int newCD)
	{
		this.m_GlobalAttackCD = newCD;
	}
}
