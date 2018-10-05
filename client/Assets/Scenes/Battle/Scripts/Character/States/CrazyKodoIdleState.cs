using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrazyKodoIdleState : IdleState 
{
	private bool m_IsFindInstantly;
	
	public CrazyKodoIdleState(NewAI aiBehavior, bool isFindInstantly) : base(aiBehavior)
	{
		this.m_IsFindInstantly = isFindInstantly;
		
		this.m_StateName = "Idle";
	}
	
	public override void AICalculate ()
	{
		if(this.m_IsFindInstantly || this.m_AIBehavior.CanTakeResponse)
		{
			CharacterAI ai = (CharacterAI)this.m_AIBehavior;
			
			TilePosition currentPosition = PositionConvertor.GetActorTileIndexFromWorldPosition(this.m_AIBehavior.transform.position);
			if(!BattleMapData.Instance.ActorCanPass(currentPosition.Row, currentPosition.Column))
			{
				GameObject go = BattleMapData.Instance.GetBulidingObjectFromActorObstacleMap(currentPosition.Row, currentPosition.Column);
				BuildingHPBehavior targetHP = go.GetComponent<BuildingHPBehavior>();
				if(targetHP != null)
				{
					CharacterAttack attack = ai.AttackBehavior;
					targetHP.DecreaseHP(attack.AttackValue, attack.AttackCategory);
				}
				
				KodoHPBehavior hp = this.m_AIBehavior.GetComponent<KodoHPBehavior>();
				hp.Bomb();
			}
			else
			{
				List<GameObject> targets = BattleSceneHelper.Instance.GetBuildingsOfCategory(ai.FavoriteCategory);
				if(targets.Count > 0)
				{
					int i = BattleRandomer.Instance.GetRandomNumber(0, targets.Count);
					GameObject targetGo = targets[i];
					BuildingBasePropertyBehavior targetProperty = targetGo.GetComponent<BuildingBasePropertyBehavior>();
					i = BattleRandomer.Instance.GetRandomNumber(0, targetProperty.ActorObstacleList.Count);
					TilePosition targetTile = targetProperty.ActorPosition + targetProperty.ActorObstacleList[i];
					Vector2 targetPosition = PositionConvertor.GetWorldPositionFromActorTileIndex(targetTile);
					
					CrazyKodoWalkState walkState = new CrazyKodoWalkState(this.m_AIBehavior, targetGo, targetPosition);
					this.m_AIBehavior.ChangeState(walkState);
				}
			}
		}
	}
}
