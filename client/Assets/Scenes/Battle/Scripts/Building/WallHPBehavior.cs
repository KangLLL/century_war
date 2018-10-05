using UnityEngine;
using System.Collections;

public class WallHPBehavior : BuildingHPBehavior 
{
	[SerializeField]
	private GameObject[] m_RandomDeadPrefab;
	
	protected override void OnDead ()
	{
		base.OnDead();
		
		GameObject ruins = BattleObjectCache.Instance.RuinsObjectParent;
		
		int id = BattleRandomer.Instance.GetRandomNumber(0, this.m_RandomDeadPrefab.Length);
		GameObject deadEffect = GameObject.Instantiate(this.m_RandomDeadPrefab[id]) as GameObject;
		deadEffect.transform.position = this.GetComponent<BuildingPropertyBehavior>().AnchorTransform.position;
		deadEffect.transform.parent = ruins.transform;
	}
}
