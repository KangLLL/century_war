using UnityEngine;
using System.Collections;

public abstract class MovableTargetWalkState : WalkState 
{
	protected IObstacleInfo m_TargetInfo;
	protected TilePosition m_TargetObjectPosition;
	
	public MovableTargetWalkState(IMapData mapData, TilePosition targetPosition, NewAI aiBehavior, IObstacleInfo targetInfo) 
		: base(mapData, targetPosition, aiBehavior)
	{
		this.m_TargetInfo = targetInfo;
		this.m_TargetObjectPosition = targetInfo.ActorPosition;
	}
	
	public override void AICalculate ()
	{
		if(!this.m_TargetObjectPosition.Equals(this.m_TargetInfo.ActorPosition))
		{
			if(this.OnTargetMove())
			{
				base.AICalculate();
			}
		}
		else
		{
			base.AICalculate();
		}
	}
	
	protected virtual bool OnTargetMove()
	{
		return true;
	}
	
}
