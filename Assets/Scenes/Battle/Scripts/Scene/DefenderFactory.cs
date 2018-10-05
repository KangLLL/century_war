using UnityEngine;
using System.Collections;

public class DefenderFactory : MonoBehaviour 
{
	[SerializeField]
	private GameObject m_DefenderPrefab;
	[SerializeField]
	private BattleMapData m_MapData;
	[SerializeField]
	private Transform m_ParentNode;
	
	public void ConstructDefender()
	{
		GameObject defender = GameObject.Instantiate(this.m_DefenderPrefab) as GameObject;
		defender.transform.position = transform.position;
		
		defender.transform.parent = this.m_ParentNode;
		
		/*
		DefenderBehavior behavior = defender.GetComponent<DefenderBehavior>();
		behavior.MapData = this.m_MapData;
		
		
		ArmyWeightDataCalculator weightCalculator = defender.GetComponent<ArmyWeightDataCalculator>();
		weightCalculator.MapData = this.m_MapData;
		
		CharacterWalk walk = defender.GetComponent<CharacterWalk>();
		walk.MapData = this.m_MapData;
		*/
		
		//CharacterHPBehavior hpBehavior = defender.GetComponent<CharacterHPBehavior>();
		
		CharacterPropertyBehavior property = defender.GetComponent<CharacterPropertyBehavior>();
		property.CharacterType = CharacterType.Defender;
		
		BattleSceneHelper.Instance.ConstructActor
			(defender, PositionConvertor.GetActorTileIndexFromWorldPosition(defender.transform.position));
	}
}
