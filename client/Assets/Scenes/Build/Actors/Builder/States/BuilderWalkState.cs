using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuilderWalkState : MovableTargetWalkState 
{
	public BuilderWalkState(IObstacleInfo targetInfo, IMapData mapData, TilePosition targetPosition,  NewAI aiBehavior) 
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
		
		List<TilePosition> border = BorderPointHelper.GetInflateOneBorder(this.m_TargetInfo);
		if(border.Contains(this.m_PreviousPosition - this.m_TargetObjectPosition))
		{
			this.m_TargetPosition = this.m_PreviousPosition;
			this.OnTargetReached();
		}
		else
		{
			List<TilePosition> aStarPath = AStarPathFinder.CalculateAStarPahtTile
				(this.FindPathStrategy, this.m_PreviousPosition, this.m_TargetPosition);
			
			this.m_LinePath.Clear();
			int destinationIndex = aStarPath.Count - 1;
			for(int i = destinationIndex - 1; i >= 0 ; i --)
			{
				TilePosition position = aStarPath[i];
				if(border.Contains(position - this.m_TargetObjectPosition))
				{
					destinationIndex = i;
					break;
				}
			}
			this.m_TargetPosition = aStarPath[destinationIndex];
			for(int i = 1; i <= destinationIndex; i ++)
			{
				TilePosition astarPoint = aStarPath[i];
				this.m_LinePath.Enqueue(astarPoint);
			}
		}
	}
	
	protected override void OnTargetReached ()
	{
		BuilderBuildState buildState = new BuilderBuildState(this.m_AIBehavior, this.m_TargetInfo, this.m_MapData, this.m_TargetPosition);//this.m_AIBehavior, this.m_TargetInfo, this.m_TargetPosition);
		this.m_AIBehavior.ChangeState(buildState);
	}
	
	protected override bool OnTargetMove ()
	{
		this.m_TargetObjectPosition = this.m_TargetInfo.ActorPosition;
		this.m_TargetPosition = this.m_TargetObjectPosition;
		this.FindPath();
		return true;
	}
}
