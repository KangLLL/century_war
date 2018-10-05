using UnityEngine;
using System.Collections;

public class DefenseObjectAI : BattleAI 
{
	public override void Start ()
	{
		this.m_CurrentResponseCount = BattleRandomer.Instance.GetRandomNumber(0, this.ResponseTime);
		DefenseObjectIdleState idleState = new DefenseObjectIdleState(this);
		this.ChangeState(idleState);
	}
}
