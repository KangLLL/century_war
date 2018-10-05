using UnityEngine;
using System.Collections;

public abstract class CharacterWalkState : WalkState 
{
	protected CharacterAI CharacterAI
	{
		get
		{
			return (CharacterAI)this.m_AIBehavior;
		}
	}

	public CharacterWalkState(IMapData mapData, TilePosition targetPosition, NewAI aiBehavior) 
		: base(mapData, targetPosition, aiBehavior)
	{
		this.WalkVelocity = this.CharacterAI.PropertyBehavior.MoveVelocity;
	}

	protected override void OnPositionTileChanged (TilePosition oldPosition, TilePosition newPosition)
	{
		if(this.CharacterAI.BattleMapData.ActorCanPass(newPosition.Row, newPosition.Column))
		{
			this.CharacterAI.PreviousValidPosition = newPosition;
		}
		base.OnPositionTileChanged (oldPosition, newPosition);
	}
}
