using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities;

public class AttackScopeDamageBehavior : LastingPropsBehavior 
{
	public int Scope { get;set; }
	public int Damage { get;set; }
	public int AttackCategory { get;set; }
	
	private List<GameObject> m_AffectBuildings;
	
	public override void Start () 
	{
		base.Start();
		this.m_AffectBuildings = BattleSceneHelper.Instance.GetNearByBuilding(this.transform.position, this.Scope);
	}
	
	protected override void Effect ()
	{
		List<GameObject> removeBuilding = new List<GameObject>();
		foreach (GameObject building in this.m_AffectBuildings) 
		{
			if(building == null)
			{
				removeBuilding.Add(building);
			}
			else
			{
				BuildingHPBehavior hp = building.GetComponent<BuildingHPBehavior>();
				hp.DecreaseHP(this.Damage, this.AttackCategory);
			}
		}
		
		foreach (GameObject building in removeBuilding) 
		{
			this.m_AffectBuildings.Remove(building);
		}
	}
}
