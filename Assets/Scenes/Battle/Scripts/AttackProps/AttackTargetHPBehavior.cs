using UnityEngine;
using System.Collections;

public class AttackTargetHPBehavior : HPBehavior 
{
	[SerializeField]
	private AttackTargetBehavior m_TargetBehavior;
	
	protected override void OnDead ()
	{
		base.OnDead ();
		this.m_TargetBehavior.EffectDisappear();
		foreach (GameObject displayObject in this.m_TargetBehavior.DisplayObjects) 
		{
			GameObject.Destroy(displayObject);
		}
	}
	
	public void SetDead()
	{
		this.OnDead();
	}
}
