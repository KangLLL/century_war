using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommandConsts;

public class DefenseObjectIdleState : AIState
{
	private DefenseObjectBattleBehavior m_DefenseObjectBattleBehavior;
	
	public DefenseObjectIdleState(NewAI aiBehavior) : base(aiBehavior)
	{
	}
	
	public override void Initial ()
	{
		this.m_DefenseObjectBattleBehavior = this.m_AIBehavior.GetComponent<DefenseObjectBattleBehavior>();
	}
	
	public override void AICalculate ()
	{
		if(this.m_AIBehavior.CanTakeResponse)
		{
			List<GameObject> targets = BattleSceneHelper.Instance.GetActors(this.m_DefenseObjectBattleBehavior.TriggerList, 
				this.m_DefenseObjectBattleBehavior.TargetType);
			foreach (GameObject target in targets) 
			{
				CharacterPropertyBehavior characterProperty = target.GetComponent<CharacterPropertyBehavior>();
				if(characterProperty.CharacterType == CharacterType.Invader)
				{
					float distanceSqrt = Vector2.SqrMagnitude((Vector2)this.m_DefenseObjectBattleBehavior.Property.AnchorTransform.position - 
						(Vector2)target.transform.position);
					if(distanceSqrt <= this.m_DefenseObjectBattleBehavior.TriggerScopeSqrt)
					{
						this.m_DefenseObjectBattleBehavior.DisplayTriggerAnimation();
						if(Application.loadedLevelName.Equals(ClientStringConstants.BATTLE_SCENE_LEVEL_NAME))
						{
							DefenseObjectPropertyBehavior property = this.m_AIBehavior.GetComponent<DefenseObjectPropertyBehavior>();
							CommunicationUtility.Instance.TriggerDefenseObject(new TriggerDefenseObjectRequestParameter()
							{ DefenseObjectID = property.DefenseObjectID, OperateTime = TimeTickRecorder.Instance.CurrentTimeTick - BattleDirector.Instance.MatchStartTick });
						}
						
						if(this.m_DefenseObjectBattleBehavior.TriggerTick == 0)
						{
							this.m_DefenseObjectBattleBehavior.Effect();
			
							LastingEffectBehavior lastingEffect = this.m_DefenseObjectBattleBehavior as LastingEffectBehavior;
							if(lastingEffect != null && lastingEffect.CurrentTimes != lastingEffect.TotalTimes)
							{
								DefenseObjectLastingState lastingState = new DefenseObjectLastingState(this.m_AIBehavior, lastingEffect);
								this.m_AIBehavior.ChangeState(lastingState);
							}
							AudioController.Play(this.m_DefenseObjectBattleBehavior.EffectSound);
						}
						else
						{
							DefenseObjectTriggerState triggerState = new DefenseObjectTriggerState(this.m_AIBehavior);
							this.m_AIBehavior.ChangeState(triggerState);
						}
						break;
					}
				}
			}
		}
	}
}
