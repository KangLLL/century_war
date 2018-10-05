using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KodoWalkState : WalkState 
{
	private CharacterAI CharacterAI
	{
		get
		{
			return (CharacterAI)this.m_AIBehavior;
		}
	}
	
	private GameObject m_Target;
	private KodoHPBehavior m_HPBehavior;
	//private BuildingBasePropertyBehavior m_TargetProperty;
	
	public KodoWalkState(IMapData mapData, TilePosition targetPosition, NewAI aiBehavior, GameObject target) 
		: base(mapData, targetPosition, aiBehavior)
	{
		this.m_Target = target;
		this.WalkVelocity = this.CharacterAI.PropertyBehavior.MoveVelocity;
		this.m_HPBehavior = this.m_AIBehavior.gameObject.GetComponent<KodoHPBehavior>();
		//this.m_TargetProperty = this.m_Target.GetComponent<BuildingBasePropertyBehavior>();
		this.m_StateName = "KodoWalk";
	}
	
	protected override IGCalculator FindPathStrategy 
	{
		get 
		{
			return null;
		}
	}
	
	public override void AICalculate ()
	{
		if(this.m_Target == null)
		{
			KodoIdleState idleState = new KodoIdleState(this.m_AIBehavior, false);
			this.m_AIBehavior.ChangeState(idleState);
		}
		else
		{
			float distanceSqr = Vector2.SqrMagnitude((Vector2)PositionConvertor.GetWorldPositionFromActorTileIndex
				(this.m_TargetPosition) - (Vector2)this.m_AIBehavior.transform.position);
			if(distanceSqr <= this.CharacterAI.AttackBehavior.AttackScopeSqr)
			{
				this.m_HPBehavior.Bomb();
				BuildingHPBehavior targetHP = this.m_Target.transform.GetComponent<BuildingHPBehavior>();
				targetHP.DecreaseHP(this.CharacterAI.AttackBehavior.AttackValue, this.CharacterAI.AttackBehavior.AttackCategory);
			}
			else
			{
				base.AICalculate();
			}
		}
	}
	
	protected override void OnTargetReached ()
	{
		KodoIdleState idleState = new KodoIdleState(this.m_AIBehavior, false);
		this.m_AIBehavior.ChangeState(idleState);
	}
	
	protected override void OnPositionTileChanged (TilePosition oldPosition, TilePosition newPosition)
	{
		this.CharacterAI.BattleMapData.RefreshInformationWithMoveActor(this.m_AIBehavior.gameObject, oldPosition, newPosition);
		base.OnPositionTileChanged(oldPosition, newPosition);
	}
}
