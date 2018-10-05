using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LastingEffectBehavior : DefenseObjectBattleBehavior 
{
	private int m_IntervalTicks;
	private int m_TotalTimes;
	
	private int m_CurrentTimes;
	
	[SerializeField]
	private GameObject m_BombEffect;
	
	public int IntervalTicks
	{
		get { return this.m_IntervalTicks; }
		set { this.m_IntervalTicks = value; }
	}
	
	public int TotalTimes
	{
		get { return this.m_TotalTimes; }
		set { this.m_TotalTimes = value; }
	}
	
	public int CurrentTimes { get { return this.m_CurrentTimes; } }
	
	public override void Start ()
	{
		base.Start ();
		this.m_CurrentTimes = 0;
	}
	
	public override void Effect ()
	{
		if(this.m_CurrentTimes < this.m_TotalTimes)
		{
			if(!this.m_BombEffect.activeSelf)
			{
				this.m_BombEffect.SetActive(true);
			}
			
			List<GameObject> targets = BattleSceneHelper.Instance.GetActors(this.DamageList, this.TargetType);
			foreach (GameObject target in targets) 
			{
				HPBehavior hpBehavior = target.GetComponent<HPBehavior>();
				hpBehavior.DecreaseHP(this.Damage, this.AttackCategory);
			}
			this.m_CurrentTimes ++;
			if(this.m_CurrentTimes == this.m_TotalTimes)
			{
				GameObject.Destroy(this.gameObject);
			}
		}
	}
}
