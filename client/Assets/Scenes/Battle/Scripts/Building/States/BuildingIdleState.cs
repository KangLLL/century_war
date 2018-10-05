using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingIdleState : IdleState 
{
	private BuildingAI BuildingAI { get { return (BuildingAI)this.m_AIBehavior; } }
	private List<TilePosition> m_AttackScopeList;
	private bool m_IsFindInstantly;
	
	public BuildingIdleState(NewAI aiBehavior, bool isFindInstantly) : base(aiBehavior)
	{
		this.m_IsFindInstantly = isFindInstantly;
	}
	
	public override void AICalculate ()
	{
		if(this.m_AttackScopeList == null)
		{
			if(this.BuildingAI.AttackBehavior.AttackScopeArray.Count > 0)
			{
				this.m_AttackScopeList = new List<TilePosition>();
				TilePosition actorPosition = PositionConvertor.GetActorTileIndexFromWorldPosition(
					this.BuildingAI.Property.AnchorTransform.position);
				foreach(TilePosition offset in this.BuildingAI.AttackBehavior.AttackScopeArray)
				{
					this.m_AttackScopeList.Add(actorPosition + offset);
				}
			}
		}
		if(this.m_AttackScopeList != null)
		{
			if(this.m_IsFindInstantly || this.m_AIBehavior.CanTakeResponse)
			{
				this.FindTarget();
			}
		}
		
	}
	
	private void FindTarget()
	{	
		List<GameObject> actorsInScope = this.BuildingAI.SceneHelper.GetActors(this.m_AttackScopeList, this.BuildingAI.AttackBehavior.TargetType);
		
		Vector3 anchorPosition = this.BuildingAI.Property.AnchorTransform.position;
		
		foreach(GameObject actor in actorsInScope)
		{
			Vector2 delta = (Vector2)(actor.transform.position - anchorPosition);
			float vectorMag = Vector2.SqrMagnitude(delta);
			if(vectorMag <= this.BuildingAI.AttackBehavior.AttackScopeSqr)
			{
				RingAttackBehavior ringBehavior = this.BuildingAI.AttackBehavior as RingAttackBehavior;
				if(ringBehavior != null)
				{
					if(vectorMag <= ringBehavior.BlindScopeSqr)
					{
						continue;
					}
				}
				
				BuildingAttackState attackState = new BuildingAttackState(this.m_AIBehavior, 
					new AITargetObject(actor, (Vector2)actor.transform.position), 
					this.BuildingAI.AttackBehavior);
				
				this.m_AIBehavior.ChangeState(attackState);
				return;
			}
		}
	}
}
