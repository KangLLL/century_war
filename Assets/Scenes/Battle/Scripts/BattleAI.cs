using UnityEngine;
using System.Collections;

public class BattleAI : NewAI 
{
	public override void Start ()
	{
		this.m_CurrentResponseCount = BattleRandomer.Instance.GetRandomNumber(0, this.ResponseTime);
	}
}
