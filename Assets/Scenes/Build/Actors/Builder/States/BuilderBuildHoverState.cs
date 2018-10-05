using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuilderBuildHoverState : MovableTargetWalkState 
{
	
	public BuilderBuildHoverState(IObstacleInfo targetInfo, IMapData mapData, TilePosition targetPosition, NewAI aiBehavior)
		: base(mapData, targetPosition, aiBehavior, targetInfo)
	{
		ActorConfig config = ActorPrefabConfig.Instance.GetComponent<ActorConfig>();
		this.WalkVelocity =  config.BuilderMoveVelocity;
	}
	
	protected override IGCalculator FindPathStrategy 
	{
		get 
		{
			return new IgnoreTargetPositionWeightStrategy(this.m_MapData, this.m_TargetPosition.Row, this.m_TargetPosition.Column);
		}
	}
	
	protected override void FindPath ()
	{
		this.m_TargetObjectPosition = this.m_TargetInfo.ActorPosition;
		
		List<TilePosition> aStarPath = AStarPathFinder.CalculateAStarPahtTile
			(this.FindPathStrategy, this.m_PreviousPosition, this.m_TargetPosition);
		
		this.m_LinePath.Clear();
		
		for(int i = 1; i < aStarPath.Count; i ++)
		{
			TilePosition astarPoint = aStarPath[i];
			this.m_LinePath.Enqueue(astarPoint);
		}
	}
	
	protected override void OnTargetReached ()
	{
		BuilderBuildState buildState = new BuilderBuildState(this.m_AIBehavior, this.m_TargetInfo, this.m_MapData, this.m_TargetPosition);//this.m_AIBehavior, this.m_TargetInfo, this.m_TargetPosition);
		this.m_AIBehavior.ChangeState(buildState);
	}
	
	protected override bool OnTargetMove ()
	{
		BuilderWalkState walkState = new BuilderWalkState(this.m_TargetInfo, this.m_MapData, this.m_TargetInfo.ActorPosition, this.m_AIBehavior);//this.m_TargetInfo, currentPosition, this.m_AIBehavior);
		this.m_AIBehavior.ChangeState(walkState);
		return false;
	}
}
