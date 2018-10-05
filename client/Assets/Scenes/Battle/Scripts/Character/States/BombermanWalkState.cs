using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombermanWalkState : CharacterWalkState 
{
	private GameObject m_Target;
	private KodoHPBehavior m_HPBehavior;
	
	public BombermanWalkState(IMapData mapData, TilePosition targetPosition, NewAI aiBehavior, GameObject target) 
		: base(mapData, targetPosition, aiBehavior)
	{
		this.m_Target = target;
		this.m_HPBehavior = this.m_AIBehavior.gameObject.GetComponent<KodoHPBehavior>();
		this.m_StateName = "BombermanWalk";
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
			BombermanIdleState idleState = new BombermanIdleState(this.m_AIBehavior, false);
			this.m_AIBehavior.ChangeState(idleState);
		}
		else
		{
			float distanceSqr = Vector2.SqrMagnitude((Vector2)PositionConvertor.GetWorldPositionFromActorTileIndex
				(this.m_TargetPosition) - (Vector2)this.m_AIBehavior.transform.position);
			if(distanceSqr <= this.CharacterAI.AttackBehavior.AttackScopeSqr)
			{
				this.m_HPBehavior.Bomb();
				List<GameObject> relatedBuildings = new List<GameObject>();
				if(this.CharacterAI.AttackBehavior.DamageScope > 0)
				{
					relatedBuildings = BattleSceneHelper.Instance.GetNearByBuilding(this.m_AIBehavior.transform.position, this.CharacterAI.AttackBehavior.DamageScope);
				}
				else
				{
					relatedBuildings.Add(this.m_Target);
				}
				foreach (GameObject building in relatedBuildings) 
				{
					BuildingHPBehavior targetHP = building.GetComponent<BuildingHPBehavior>();
					targetHP.DecreaseHP(this.CharacterAI.AttackBehavior.AttackValue, this.CharacterAI.AttackBehavior.AttackCategory);
				}
			}
			else
			{
				base.AICalculate();
			}
		}
	}
	
	protected override void OnTargetReached ()
	{
		BombermanIdleState idleState = new BombermanIdleState(this.m_AIBehavior, false);
		this.m_AIBehavior.ChangeState(idleState);
	}
	
	protected override void OnPositionTileChanged (TilePosition oldPosition, TilePosition newPosition)
	{
		this.CharacterAI.BattleMapData.RefreshInformationWithMoveActor(this.m_AIBehavior.gameObject, oldPosition, newPosition);
		base.OnPositionTileChanged(oldPosition, newPosition);
	}
}
