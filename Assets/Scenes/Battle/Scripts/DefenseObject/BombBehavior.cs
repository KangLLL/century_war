using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombBehavior : DefenseObjectBattleBehavior 
{
	[SerializeField]
	private GameObject m_BombPrefab;
	
	public Transform ParentNode { get;set; }
	
	private int m_PushVelocity;
	private int m_PushTicks;
	
	public int PushVelocity
	{
		get { return this.m_PushVelocity; }
		set { this.m_PushVelocity = value; }
	}
	
	public int PushTicks
	{
		get { return this.m_PushTicks; }
		set { this.m_PushTicks = value; }
	}
	
	public override void Effect ()
	{
		List<GameObject> targets = BattleSceneHelper.Instance.GetActors(this.DamageList, this.TargetType);
		foreach (GameObject target in targets) 
		{
			HPBehavior hpBehavior = target.GetComponent<HPBehavior>();
			hpBehavior.DecreaseHP(this.Damage, this.AttackCategory);
			
			CharacterAI ai = target.GetComponent<CharacterAI>();
			ai.SetPush(this.m_PushTicks, this.m_PushVelocity, this.Property.AnchorTransform.position);
		}
		
		if(this.m_BombPrefab != null)
		{
			GameObject bomb = GameObject.Instantiate(this.m_BombPrefab) as GameObject;
			bomb.transform.position = this.Property.AnchorTransform.position;
			bomb.transform.parent = this.ParentNode;
		}
		GameObject.Destroy(this.gameObject);
	}
}
