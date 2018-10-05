using UnityEngine;
using System.Collections;

public class BuildingAttackValueBuff : BuildingBuff 
{
	private int m_Effect;
	
	public BuildingAttackValueBuff(int effect)
	{
		this.m_Effect = effect;
	}
	
	public override int AttackValueEffect 
	{
		get 
		{
			return this.m_Effect;
		}
	}
}
