using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class AttackArmyBehavior : LastingPropsBehavior 
{
	public int Number { get;set; }
	public ArmyType ArmyType { get;set; }
	public int ArmyLevel { get;set; }
	public CharacterFactory CharacterFactory { get;set; }
	
	protected override void Effect ()
	{
		for(int i = 0; i < this.Number; i ++)
		{
			this.CharacterFactory.ConstructArmy(this.ArmyType, this.ArmyLevel, this.transform.position,false);
		}
	}
}
