using UnityEngine;
using System.Collections;

public class BuildingHPBuff : BuildingBuff 
{
	private int m_Effect;
	
	public BuildingHPBuff(int effect)
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
