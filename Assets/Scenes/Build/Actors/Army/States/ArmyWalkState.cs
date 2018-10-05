using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArmyWalkState : MovableTargetWalkState 
{
	public ArmyWalkState(IMapData mapData, TilePosition targetPosition, NewAI aiBehavior, IBuildingInfo targetInfo) :
		base(mapData, targetPosition, aiBehavior, targetInfo)
	{
		this.WalkVelocity = ((ArmyAI)aiBehavior).WalkVelocity;
	}
	
	public override void Initial ()
	{
		this.m_TargetOffset = new Vector2(Random.Range(- ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.width / 2, 
			ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.width / 2),
			Random.Range(- ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.height / 2, 
			ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.height / 2));
		base.Initial();
	}
	
	protected override IGCalculator FindPathStrategy 
	{
		get 
		{
			return new IgnoreTargetWeightStrategy(this.m_MapData, 
				this.m_TargetInfo.ActorPosition.Row, this.m_TargetInfo.ActorPosition.Column);
		}
	}
	
	protected override void FindPath ()
	{
		List<TilePosition> aStarPath = AStarPathFinder.CalculateAStarPahtTile
				(this.FindPathStrategy, this.m_PreviousPosition, this.m_TargetPosition);
		
		this.m_LinePath.Clear();
		for(int i = 1; i < aStarPath.Count; i ++)
		{
			TilePosition astarPoint = aStarPath[i];
			this.m_LinePath.Enqueue(astarPoint);
		}
	}
	
	protected override bool OnTargetMove ()
	{
		this.m_TargetObjectPosition = this.m_TargetInfo.ActorPosition;
		this.m_TargetPosition = ((ArmyAI)this.m_AIBehavior).FindCampStandablePoint(this.m_TargetInfo as IBuildingInfo);
		this.FindPath();
		return true;
	}
	
	protected override void OnTargetReached ()
	{
		ArmyIdleState idleState = new ArmyIdleState(this.m_AIBehavior, this.m_TargetInfo as IBuildingInfo);
		this.m_AIBehavior.ChangeState(idleState);
	}
}
