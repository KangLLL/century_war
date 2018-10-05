using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class VillagerWalkState : MovableTargetWalkState
{
	private TilePosition m_Offset;
	private ActorConfig m_ActorConfig;
	
	public VillagerWalkState(IMapData mapData, TilePosition targetPosition, NewAI aiBehavior, IBuildingInfo targetInfo) : 
		base(mapData, targetPosition, aiBehavior, targetInfo)
	{
		this.m_ActorConfig = ActorPrefabConfig.Instance.GetComponent<ActorConfig>();
		this.WalkVelocity =  this.m_ActorConfig.VillagerMoveVelocity;
		this.m_Offset = targetPosition - targetInfo.ActorPosition;
	}
	
	protected override IGCalculator FindPathStrategy 
	{
		get 
		{
			return new IgnoreTargetPositionWeightStrategy(this.m_MapData, 
				this.m_TargetPosition.Row, this.m_TargetPosition.Column);
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
	
	protected override bool OnTargetMove ()
	{
		this.m_TargetPosition = this.m_TargetInfo.ActorPosition + this.m_Offset;
		while(!this.m_TargetPosition.IsValidActorTilePosition())
		{
			this.m_TargetPosition = BorderPointHelper.FindValidInflateOneBorderPoint(this.m_TargetInfo);
			this.m_Offset = this.m_TargetPosition - this.m_TargetInfo.ActorPosition;
		}
		this.FindPath();
		return true;
	}
	
	protected override void OnTargetReached ()
	{		
		bool isTargetHover = (this.m_TargetInfo as IBuildingInfo).IsBuildingHover(((VillagerAI)this.m_AIBehavior).MapData);
		
		bool canDisappear = false;
		foreach(BuildingType type in this.m_ActorConfig.VillagerDisappearBuildingTypes)
		{
			if(type == ((IBuildingInfo)this.m_TargetInfo).BuildingType)
			{
				canDisappear = true;
				break;
			}
		}
		int max = canDisappear && !isTargetHover ? (int)VillagerState.Disappear : (int)VillagerState.Idle;
		
		VillagerState toState = (VillagerState)(Random.Range(0, max + 1));
		if(toState == VillagerState.Idle)
		{
			VillagerIdleState idleState = new VillagerIdleState(this.m_AIBehavior, ((IBuildingInfo)this.m_TargetInfo), isTargetHover);
			this.m_AIBehavior.ChangeState(idleState);
		}
		else
		{
			VillagerDisappearState disappearState = new VillagerDisappearState(this.m_AIBehavior);
			this.m_AIBehavior.ChangeState(disappearState);
		}
	}
	
	private enum VillagerState
	{
		Idle,
		Disappear
	}
}
