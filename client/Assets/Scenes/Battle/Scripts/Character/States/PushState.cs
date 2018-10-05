using UnityEngine;
using System.Collections;

public class PushState : AIState 
{
	private int m_PushTicks;
	private int m_Velocity;
	private Vector2 m_PushVelocity;
	private Vector2 m_Direction;
	
	private int m_Ticks;
	private TilePosition m_PreviousPosition;
	
	private float m_PushAttenuateFactor;
	private float m_PushFactor;
	
	public PushState(NewAI aiBehavior, int pushTicks, int pushVelocity, Vector2 direction) : base(aiBehavior)
	{
		this.m_Velocity = pushVelocity;
		this.m_PushTicks = pushTicks;
		if(Mathf.Approximately(direction.magnitude,0.0f))
		{
			this.m_Direction = Vector2.up;
		}
		else
		{
			this.m_Direction = direction;
			this.m_Direction.Normalize();
		}
		
		this.m_PushAttenuateFactor = ((CharacterAI)aiBehavior).PushAttenuateFactor;
		this.m_PushFactor = ((CharacterAI)aiBehavior).PushFactor;
	}
	
	public override void Initial ()
	{
		this.m_PushVelocity = this.m_Velocity * this.m_Direction;
		this.m_PreviousPosition = PositionConvertor.GetActorTileIndexFromWorldPosition(this.m_AIBehavior.transform.position);
		
		this.m_AnimationController.PlayIdleAnimation();
	}
	
	public override void AICalculate ()
	{
		CharacterAI characterAI = (CharacterAI) this.m_AIBehavior;

		if(this.m_Ticks++ == this.m_PushTicks)
		{
			characterAI.SetIdle(false);
		}
		else
		{
			this.m_PushVelocity *=  this.m_PushAttenuateFactor;
			Vector2 v = this.m_PushVelocity * this.m_PushFactor;
			Vector2 newPosition = (Vector2)this.m_AIBehavior.transform.position + v;

			newPosition = PositionConvertor.ClampWorldPositionOfActorTile(newPosition);
			TilePosition currentTile = PositionConvertor.GetActorTileIndexFromWorldPosition(this.m_AIBehavior.transform.position);
			TilePosition newTile = PositionConvertor.GetActorTileIndexFromWorldPosition(new Vector3(newPosition.x, newPosition.y));
			if(BattleMapData.Instance.ActorCanPass(newTile.Row, newTile.Column) && BattleMapData.Instance.ActorCanPass(currentTile.Row, currentTile.Column))
			{
				this.m_AIBehavior.transform.position = new Vector3(newPosition.x, newPosition.y, this.m_AIBehavior.transform.position.z);
				if(newTile != this.m_PreviousPosition)
				{
					BattleMapData.Instance.RefreshInformationWithMoveActor(this.m_AIBehavior.gameObject, this.m_PreviousPosition, newTile);
					this.m_PreviousPosition = newTile;
					if(characterAI.BattleMapData.ActorCanPass(newTile.Row, newTile.Column))
					{
						characterAI.PreviousValidPosition = newTile;
					}
				}
			}
		}
	}
}
