using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroupAttackObjectBehavior : AttackObjectBehavior
{
	private int m_DamageScope { get;set; }
	
	public int DamageScope
	{
		get
		{
			return this.m_DamageScope;
		}
		set
		{
			this.m_DamageScope = value;
		}
	}
	
	public override void Start ()
	{
		base.Start ();
	}
	
	protected override void Calculate ()
	{
		TilePosition affectPosition = PositionConvertor.GetActorTileIndexFromWorldPosition(this.DestinationObject.GetDestinationPosition(this.transform.position));
		List<TilePosition> affectedTiles = RoundHelper.FillCircle
			(affectPosition.Column, affectPosition.Row, Mathf.CeilToInt(this.DamageScope / 
				(float)Mathf.Min(ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.height, 
				ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.width)));
		
		foreach(TilePosition tile in affectedTiles)
		{
			if(tile.IsValidActorTilePosition())
			{
				List<GameObject> characters = BattleMapData.Instance.ActorArray[tile.Row, tile.Column];
				foreach(GameObject character in characters)
				{
					CharacterPropertyBehavior property = character.GetComponent<CharacterPropertyBehavior>();
					if(property.CharacterType == CharacterType.Invader)
					{						
						//float distance = Vector2.Distance((Vector2)this.transform.position, (Vector2)character.transform.position);
						
						//float percentage = (1 - distance) / this.m_GroupDamageScope;
						//percentage = Mathf.Max(percentage, 0);
						
						CharacterHPBehavior hpBehavior = character.GetComponent<CharacterHPBehavior>();
						hpBehavior.DecreaseHP(this.Damage, this.AttackCategory);
						
						this.PushCharacter(character);
					}
				}
			}
		}
	}
}
