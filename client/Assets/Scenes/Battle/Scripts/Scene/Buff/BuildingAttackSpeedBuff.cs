using UnityEngine;
using System.Collections;

public class BuildingAttackSpeedBuff : BuildingBuff 
{
	private int m_Effect;
	
	public BuildingAttackSpeedBuff(int effect)
	{
		this.m_Effect = effect;
	}
	
	public override int HPEffect 
	{
		get 
		{
			return this.m_Effect;
		}
	}
}
