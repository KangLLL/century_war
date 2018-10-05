using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuilderReturnState : MovableTargetWalkState 
{
	public BuilderReturnState(IMapData mapData, TilePosition targetPosition, NewAI aiBehavior, IBuildingInfo targetInfo) 
		: base(mapData, targetPosition, aiBehavior, targetInfo)
	{
		ActorConfig config = ActorPrefabConfig.Instance.GetComponent<ActorConfig>();
		this.WalkVelocity =  config.BuilderMoveVelocity;
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
	
	protected override IGCalculator FindPathStrategy 
	{
		get 
		{
			return new IgnoreTargetWeightStrategy(this.m_MapData, this.m_TargetPosition.Row, this.m_TargetPosition.Column);
		}
	}
	
	protected override void OnTargetReached ()
	{
		int builderNO = ((BuilderAI)this.m_AIBehavior).BuilderNO;
		BuildingSceneDirector.Instance.RecycleBuilder(builderNO);
	}
	
	protected override bool OnTargetMove ()
	{
		this.m_TargetObjectPosition = this.m_TargetInfo.ActorPosition;
		this.m_TargetPosition = BorderPointHelper.FindValidInflateOneBorderPoint(this.m_TargetInfo);
		this.FindPath();
		return true;
	}
}
