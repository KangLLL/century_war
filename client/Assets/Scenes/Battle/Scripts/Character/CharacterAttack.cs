using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterAttack : AttackBehavior 
{
	[SerializeField]
	private CharacterAnimationController m_AnimationController;
	[SerializeField]
	private float m_AttackScopeFluctuation;
	
	public override void Start ()
	{
		this.AttackScope +=  BattleRandomer.Instance.GetRondomValue(-this.m_AttackScopeFluctuation, this.m_AttackScopeFluctuation);
		base.Start ();
	}
}
