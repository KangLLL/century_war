using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InvaderWalkState : CharacterWalkState 
{
	private GameObject m_Target;
	
	public InvaderWalkState(IMapData mapData, TilePosition targetPosition, NewAI aiBehavior, GameObject target) 
		: base(mapData, targetPosition, aiBehavior)
	{
		this.m_Target = target;
		this.m_StateName = "InvaderWalk";
		this.m_Values = new string[2];
	}
	
	protected override IGCalculator FindPathStrategy 
	{
		get 
		{
			//IgnoreTargetAndAttackScopeWeightStrategy strategy = new IgnoreTargetAndAttackScopeWeightStrategy(this.CharacterAI.BattleMapData, 
			//	this.m_TargetPosition.Row, this.m_TargetPosition.Column, this.CharacterAI.AttackBehavior.AttackScope);
			//this.CharacterAI.DrawLightPoints(strategy.m_PreviousTargetLightPositions);
			//return strategy;
			
			return new IgnoreTargetWeightStrategy(this.CharacterAI.BattleMapData, this.m_TargetPosition.Row, this.m_TargetPosition.Column);
		}
	}
	
	protected override void FindPath ()
	{
		List<TilePosition> aStarPath;
		List<TilePosition> linePath = AStarPathFinder.CalculatePathTile(this.FindPathStrategy, this.CharacterAI.BattleMapData.ActorObstacleArray,
			this.m_PreviousPosition, this.m_TargetPosition, out aStarPath);
		
		//this.CharacterAI.DrawPath(aStarPath);
		/*
		Debug.Log("AStar Path Length is:" + aStarPath.Count);
		foreach(TilePosition at in aStarPath)
		{
			Debug.Log("row:" + at.Row + " ,column:" + at.Column);
		}
		Debug.Log("Line Path Length is:" + linePath.Count);
		foreach (TilePosition lt in linePath) 
		{
			Debug.Log("row:" + lt.Row + " ,column:" + lt.Column);
		}
		*/
		this.SetPath(linePath);
		if(linePath.Count == 0)
		{
			this.OnTargetReached();
		}
		else
		{
			TilePosition targetTilePosition = linePath[linePath.Count - 1];
			
			if(this.m_MapData.GetBulidingObjectFromActorObstacleMap(targetTilePosition.Row, targetTilePosition.Column) != 
				this.m_Target)
			{
				Vector2 reachablePosition = (Vector2)PositionConvertor.GetWorldPositionFromActorTileIndex(linePath[linePath.Count - 2]);
				TilePosition targetTile = aStarPath[aStarPath.Count - 1];
				for(int i = 1; i < aStarPath.Count; i++)
				{
					if(this.m_MapData.GetBulidingObjectFromActorObstacleMap(aStarPath[i].Row, aStarPath[i].Column) == this.m_Target)
					{
						targetTile = aStarPath[i];
						break;
					}
				}
				Vector2 targetPosition = (Vector2)PositionConvertor.GetWorldPositionFromActorTileIndex(targetTile);
				
				if(Vector2.SqrMagnitude(targetPosition - reachablePosition) <= this.CharacterAI.AttackBehavior.AttackScopeSqr)
				{
					this.m_TargetPosition = targetTile;
					this.ShowTarget();
					return;
				}
			}
			
			this.m_TargetPosition = targetTilePosition;
			this.m_Target = this.CharacterAI.BattleMapData.GetBulidingObjectFromActorObstacleMap
				(this.m_TargetPosition.Row,this.m_TargetPosition.Column);
			this.ShowTarget();
		}
	}
	
	public override void AICalculate ()
	{
		float distanceSqr = Vector2.SqrMagnitude((Vector2)PositionConvertor.GetWorldPositionFromActorTileIndex
			(this.m_TargetPosition) - (Vector2)this.m_AIBehavior.transform.position);
		if(this.m_Target == null)
		{
			InvaderIdleState idleState = new InvaderIdleState(this.m_AIBehavior, false);
			this.m_AIBehavior.ChangeState(idleState);
		}
		else
		{
			if(distanceSqr <= this.CharacterAI.AttackBehavior.AttackScopeSqr)
			{
				BuildingBasePropertyBehavior property = this.m_Target.GetComponent<BuildingBasePropertyBehavior>();
				AttackState attackState;
				if(property == null)
				{
					attackState = new InvaderAttackState(this.m_AIBehavior, new AITargetObject(this.m_Target, this.m_Target.transform.position), this.CharacterAI.AttackBehavior);
				}
				else
				{
					attackState = new InvaderAttackState(this.m_AIBehavior,
						new AITargetObject(this.m_Target, (Vector2)(PositionConvertor.GetWorldPositionFromActorTileIndex(this.m_TargetPosition))), this.CharacterAI.AttackBehavior);
				}
				this.m_AIBehavior.ChangeState(attackState);
			}
			else
			{
				base.AICalculate();
			}
		}
	}
	
	protected override void OnTargetReached ()
	{
		InvaderIdleState idleState = new InvaderIdleState(this.m_AIBehavior, false);
		this.m_AIBehavior.ChangeState(idleState);
	}
	
	protected override void OnPositionTileChanged (TilePosition oldPosition, TilePosition newPosition)
	{
		this.CharacterAI.BattleMapData.RefreshInformationWithMoveActor(this.m_AIBehavior.gameObject, oldPosition, newPosition);
		base.OnPositionTileChanged(oldPosition, newPosition);
	}
	
	private void ShowTarget()
	{
		if(BattleEffectConfig.Instance.TargetEffectPrefab != null && this.CharacterAI.IsShowTarget)
		{
			BuildingBasePropertyBehavior property = this.m_Target.GetComponent<BuildingBasePropertyBehavior>();
			GameObject targetEffect = GameObject.Instantiate(BattleEffectConfig.Instance.TargetEffectPrefab) as GameObject;
			Vector3 offset = targetEffect.transform.position;
			
			Vector3 targetEffectPosition = property == null ? this.m_Target.transform.position :
				property.AnchorTransform.position;
			targetEffect.transform.position = targetEffectPosition + offset;
			targetEffect.transform.parent = BattleObjectCache.Instance.EffectObjectParent.transform;
		}
	}
}
