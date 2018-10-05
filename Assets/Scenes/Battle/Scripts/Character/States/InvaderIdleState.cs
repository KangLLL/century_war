using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class InvaderIdleState : CharacterIdleState 
{
	public InvaderIdleState(NewAI aiBehavior, bool isFindInstantly) : base(aiBehavior, isFindInstantly)
	{
		this.m_StateName = "InvaderIdle";
	}
	
	public override void AICalculate ()
	{
		if(this.m_IsFindInstantly || this.m_AIBehavior.CanTakeResponse)
		{
			this.FindTarget();
		}
	}
	
	private void FindTarget()
	{	
		TilePosition currentPosition = PositionConvertor.GetActorTileIndexFromWorldPosition(this.m_AIBehavior.transform.position);
		if(!this.FindDefender(currentPosition))
		{
			GameObject target = null;
			target = this.CharacterAI.BattleSceneHelper.GetNearestBuildingOfCategory(this.m_AIBehavior.transform.position, this.CharacterAI.FavoriteCategory);
			if(target != null)
			{
				BuildingBasePropertyBehavior property = target.GetComponent<BuildingBasePropertyBehavior>();
				int rID = BattleRandomer.Instance.GetRandomNumber(0, property.ActorObstacleList.Count);
				TilePosition tp = property.ActorPosition + property.ActorObstacleList[rID];
				

				InvaderWalkState invaderWalkState = new InvaderWalkState(this.CharacterAI.BattleMapData, 
					tp, this.m_AIBehavior, target);
				this.m_AIBehavior.ChangeState(invaderWalkState);
			}
		}
	}
	
	private bool FindDefender(TilePosition currentPosition)
	{
		foreach(TilePosition offset in this.CharacterAI.AttackBehavior.AttackScopeArray)
		{
			TilePosition positon = currentPosition + offset;
			
			bool isNeedContinue = true;
			if(positon.IsValidActorTilePosition())
			{
				List<GameObject> actors = this.CharacterAI.BattleMapData.ActorArray[positon.Row, positon.Column];
				
				foreach(GameObject actor in actors)
				{
					CharacterPropertyBehavior property = actor.GetComponent<CharacterPropertyBehavior>();
					if(property.CharacterType == CharacterType.Defender)
					{
						isNeedContinue = false;
						if(Vector2.SqrMagnitude((Vector2)(this.m_AIBehavior.transform.position - actor.transform.position))
							<= this.CharacterAI.AttackBehavior.AttackScopeSqr)
						{	
							AttackState attackState = new AttackState(this.m_AIBehavior, new AITargetObject(actor, (Vector2)actor.transform.position), this.CharacterAI.AttackBehavior);
							this.m_AIBehavior.ChangeState(attackState);
							return true;
						}
					}
				}
			}
			if(!isNeedContinue)
			{
				break;
			}
		}
		return false;
	}
	
	
}
