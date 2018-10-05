using UnityEngine;
using System.Collections;

public class CrazyKodoWalkState : AIState 
{
	private GameObject m_TargetBuilding;
	private Vector2 m_TargetPosition;
	
	private float m_MoveVelocity;
	private Vector2 m_MoveVector;
	
	//private int m_TotalTicks;
	//private int m_MoveTicks;
	
	private TilePosition m_PreviousPosition;
	
	public CharacterAI CharacterAI
	{
		get { return (CharacterAI)this.m_AIBehavior; } 
	}
	
	public CrazyKodoWalkState(NewAI aiBehavior, GameObject target, Vector2 targetPostion) : base(aiBehavior)
	{
		this.m_TargetBuilding = target;
		this.m_TargetPosition = targetPostion;
		
		Vector2 deltaVector = this.m_TargetPosition - (Vector2)this.m_AIBehavior.transform.position;
		
		this.m_MoveVelocity = this.CharacterAI.PropertyBehavior.MoveVelocity;
		
		//this.m_TotalTicks = Mathf.CeilToInt(deltaVector.magnitude / this.m_MoveVelocity);
		//this.m_MoveTicks = 0;
		this.m_MoveVector = this.m_MoveVelocity * deltaVector.normalized;
		
		this.m_StateName = "Walk";
	}
	
	public override void Initial ()
	{
		this.m_AnimationController.PlayWalkAnimation(this.m_MoveVector);
		this.ShowTarget();
		this.m_PreviousPosition = PositionConvertor.GetActorTileIndexFromWorldPosition(this.m_AIBehavior.transform.position);
	}
	
	public override void AICalculate ()
	{
		/*
		if(this.m_TargetBuilding != null)
		{
			if(++this.m_MoveTicks == this.m_TotalTicks)
			{
				this.m_AIBehavior.transform.position = new Vector3(this.m_TargetPosition.x, this.m_TargetPosition.y, this.m_TargetPosition.y);
				TilePosition currentPosition = PositionConvertor.GetActorTileIndexFromWorldPosition(this.m_AIBehavior.transform.position);
				if(this.m_PreviousPosition != currentPosition)
				{
					BattleMapData.Instance.RefreshInformationWithMoveActor(this.m_AIBehavior.gameObject, this.m_PreviousPosition, currentPosition);
				}
				this.Bomb(this.m_TargetBuilding);
			}
			else
			{
			*/
				this.m_AIBehavior.transform.position = new Vector3(this.m_AIBehavior.transform.position.x + this.m_MoveVector.x,
					this.m_AIBehavior.transform.position.y + this.m_MoveVector.y,
					this.m_AIBehavior.transform.position.y + this.m_MoveVector.y);
				
				TilePosition currentPosition = PositionConvertor.GetActorTileIndexFromWorldPosition(this.m_AIBehavior.transform.position);
				if(this.m_PreviousPosition != currentPosition)
				{
					if(currentPosition.IsValidActorTilePosition())
			{
					BattleMapData.Instance.RefreshInformationWithMoveActor(this.m_AIBehavior.gameObject, this.m_PreviousPosition, currentPosition);
					if(!BattleMapData.Instance.ActorCanPass(currentPosition.Row, currentPosition.Column))
					{
						this.Bomb(BattleMapData.Instance.GetBulidingObjectFromActorObstacleMap(currentPosition.Row, currentPosition.Column));
					}
			}
			else
			{
				BattleSceneHelper.Instance.DestroyActor(this.m_AIBehavior.gameObject, this.m_PreviousPosition);
				GameObject.Destroy(this.m_AIBehavior.gameObject);
				if(Application.loadedLevelName == ClientStringConstants.BATTLE_SCENE_LEVEL_NAME)
		{
			CharacterPropertyBehavior property = this.CharacterAI.PropertyBehavior;
			if(property.CharacterType == CharacterType.Invader)
			{
				if(BattleSceneHelper.Instance.TotalInvaderCount == 0 && 
					BattleRecorder.Instance.DropArmyCount == ArmyMenuPopulator.Instance.TotalArmyCount &&
					BattleRecorder.Instance.DropMercenaryCount == ArmyMenuPopulator.Instance.TotalMercenaryCount)
				{
					BattleDirector.Instance.EndMatch();
				}
			}
		}
			}
				}
				this.m_PreviousPosition = currentPosition;
		
		
		
		/*
			}
		}
		else
		{
			this.CharacterAI.SetIdle(false);
		}
		*/
	}
	
	private void Bomb(GameObject target)
	{
		BuildingHPBehavior targetHP = target.GetComponent<BuildingHPBehavior>();
		if(targetHP != null)
		{
			targetHP.DecreaseHP(this.CharacterAI.AttackBehavior.AttackValue, this.CharacterAI.AttackBehavior.AttackCategory);
		}
		KodoHPBehavior hp = this.m_AIBehavior.GetComponent<KodoHPBehavior>();
		hp.Bomb();
	}
	
	private void ShowTarget()
	{
		if(BattleEffectConfig.Instance.TargetEffectPrefab != null && this.CharacterAI.IsShowTarget)
		{
			BuildingBasePropertyBehavior property = this.m_TargetBuilding.GetComponent<BuildingBasePropertyBehavior>();
			GameObject targetEffect = GameObject.Instantiate(BattleEffectConfig.Instance.TargetEffectPrefab) as GameObject;
			Vector3 offset = targetEffect.transform.position;
			
			Vector3 targetEffectPosition = property == null ? this.m_TargetBuilding.transform.position :
				property.AnchorTransform.position;
			targetEffect.transform.position = targetEffectPosition + offset;
			targetEffect.transform.parent = BattleObjectCache.Instance.EffectObjectParent.transform;
		}
	}
	
}
