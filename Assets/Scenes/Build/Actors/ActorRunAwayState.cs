using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActorRunAwayState : WalkState 
{
	public ActorRunAwayState(IMapData mapData, TilePosition targetPosition, NewAI aiBehavior) 
		: base(mapData, targetPosition, aiBehavior)
	{
	}
	
	public override void Initial ()
	{
		this.m_AnimationController.SetAdvanceScale(ClientConfigConstants.Instance.RunAwayAccelerateScale);
		this.WalkVelocity = ActorPrefabConfig.Instance.GetComponent<ActorConfig>().VillagerMoveVelocity * 
			ClientConfigConstants.Instance.RunAwayAccelerateScale;
		base.Initial();
	}
	
	protected override IGCalculator FindPathStrategy 
	{
		get
		{
			return new IgnoreTargetWeightStrategy(this.m_MapData, this.m_TargetPosition.Row, this.m_TargetPosition.Column);
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
	
	protected override void OnTargetReached ()
	{
		GameObject.Destroy(this.m_AIBehavior.gameObject);
	}
}
