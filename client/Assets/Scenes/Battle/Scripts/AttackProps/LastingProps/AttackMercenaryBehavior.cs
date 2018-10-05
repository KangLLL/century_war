using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class AttackMercenaryBehavior : LastingPropsBehavior 
{
	public int Number { get;set; }
	public MercenaryType MercenaryType { get;set; }
	public CharacterFactory CharacterFactory { get;set; }
	
	protected override void Effect ()
	{
		for(int i = 0; i < this.Number; i ++)
		{
			this.CharacterFactory.ConstructMercenary(this.MercenaryType, this.transform.position,false);
		}
	}
}
