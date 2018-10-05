using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class BombermanIdleState : CharacterIdleState 
{	
	private List<Transform> m_Targets;
	private List<GameObject> m_TargetsObjects;
	
	private int m_SearchIndex;
	
	private CharacterAI CharacterAI
	{
		get
		{
			return (CharacterAI)this.m_AIBehavior;
		}
	}
	
	public BombermanIdleState(NewAI aiBehavior, bool isFindInstantly) : base(aiBehavior, isFindInstantly)
	{
		this.m_StateName = "BombermanIdle";
	}
	
	public override void AICalculate ()
	{	
		if(this.m_Targets == null)
		{
			if(this.m_IsFindInstantly || this.m_AIBehavior.CanTakeResponse)
			{
				this.m_TargetsObjects = new List<GameObject>();
				this.m_Targets = this.CharacterAI.BattleSceneHelper.GetBuildingsTransformOfCategory(this.CharacterAI.FavoriteCategory);
				if(this.m_Targets.Count == 0)
				{
					this.m_Targets = this.CharacterAI.BattleSceneHelper.GetBuildingsTransformOfCategory(BuildingCategory.Any);
				}
				
				this.m_SearchIndex = 0;
				
				this.m_Targets.Sort((x, y) => {
					float distanceX = Vector2.SqrMagnitude((Vector2)(x.position - this.m_AIBehavior.transform.position));
					float distanceY = Vector2.SqrMagnitude((Vector2)(y.position - this.m_AIBehavior.transform.position));
					return distanceX.CompareTo(distanceY);
				}
				);
				
				foreach (Transform item in this.m_Targets) 
				{
					this.m_TargetsObjects.Add(item.parent.gameObject);
				}
				
				Debug.Log("Total count:" + this.m_Targets.Count);
			}
		}
		else if(this.m_SearchIndex == this.m_Targets.Count)
		{
			this.SetNoWallBuildingToTarget();
		}
		else
		{
			GameObject go = this.m_TargetsObjects[this.m_SearchIndex];
			while(this.m_SearchIndex < this.m_Targets.Count && go == null)
			{
				this.m_SearchIndex ++;
				if(this.m_SearchIndex < this.m_Targets.Count)
				{
					go = this.m_TargetsObjects[this.m_SearchIndex];
				}
			}
			if(this.m_SearchIndex == this.m_Targets.Count)
			{
				this.SetNoWallBuildingToTarget();
			}
			else
			{
				GameObject target = this.m_TargetsObjects[this.m_SearchIndex ++];
				
				
				if(target != null)
				{
					BuildingBasePropertyBehavior property = target.GetComponent<BuildingBasePropertyBehavior>();
					if(property != null)
					{
						TilePosition destination = property.ActorPosition + property.ActorObstacleList[0];
						IgnoreTargetAndAttackScopeWeightStrategy findPathStrategy = new IgnoreTargetAndAttackScopeWeightStrategy(
				 			this.CharacterAI.BattleMapData, destination.Row, destination.Column, this.CharacterAI.AttackBehavior.AttackScope);
						//KodoPathFindStrage findPathStrategy = new KodoPathFindStrage(
						//	this.CharacterAI.BattleMapData, destination.Row, destination.Column);
						List<TilePosition> aStarPath;
						List<TilePosition> linePath = AStarPathFinder.CalculatePathTile(findPathStrategy, this.CharacterAI.BattleMapData.ActorObstacleArray, 
							this.m_CurrentPosition, destination, out aStarPath);
				
						TilePosition endPoint = linePath[linePath.Count - 1];
						GameObject targetBuilding = this.CharacterAI.BattleMapData.GetBulidingObjectFromActorObstacleMap(endPoint.Row,endPoint.Column);
						
						if(targetBuilding != null)
						{
							property = targetBuilding.GetComponent<BuildingPropertyBehavior>();
							if(property != null && ((BuildingPropertyBehavior)property).BuildingType == BuildingType.Wall)
							{
								if(BattleEffectConfig.Instance.TargetEffectPrefab != null && this.CharacterAI.IsShowTarget)
								{
									GameObject targetEffect = GameObject.Instantiate(BattleEffectConfig.Instance.TargetEffectPrefab) as GameObject;
									Vector3 offset = targetEffect.transform.position;
									
									Vector3 targetEffectPosition = property.AnchorTransform.position;
									targetEffect.transform.position = targetEffectPosition + offset;
									targetEffect.transform.parent = BattleObjectCache.Instance.EffectObjectParent.transform;
								}
								
								BombermanWalkState walkState = new BombermanWalkState(this.CharacterAI.BattleMapData, endPoint, this.m_AIBehavior, targetBuilding);
								walkState.SetPath(linePath);
								this.m_AIBehavior.ChangeState(walkState);
							}
						}
					}
				}
			}
		}
	}
	
	
	private void SetNoWallBuildingToTarget()
	{
		foreach (GameObject item in this.m_TargetsObjects) 
		{
			if(item != null)
			{
				BuildingBasePropertyBehavior property = item.GetComponent<BuildingBasePropertyBehavior>();
				TilePosition destination = property.GetBuildingFirstActorPosition();
				IgnoreTargetAndAttackScopeWeightStrategy findPathStrategy = new IgnoreTargetAndAttackScopeWeightStrategy(
					this.CharacterAI.BattleMapData, destination.Row, destination.Column, this.CharacterAI.AttackBehavior.AttackScope);
				//KodoPathFindStrage findPathStrategy = new KodoPathFindStrage(
				//	this.CharacterAI.BattleMapData, destination.Row, destination.Column);
				
				List<TilePosition> aStarPath;
				List<TilePosition> linePath = AStarPathFinder.CalculatePathTile(findPathStrategy, this.CharacterAI.BattleMapData.ActorObstacleArray, 
					this.m_CurrentPosition, destination, out aStarPath);
				
				TilePosition endPoint = linePath[linePath.Count - 1];
				GameObject targetBuilding = this.CharacterAI.BattleMapData.GetBulidingObjectFromActorObstacleMap(endPoint.Row,endPoint.Column);
				if(targetBuilding != null)
				{
					property = targetBuilding.GetComponent<BuildingBasePropertyBehavior>();
					if(property != null)
					{
						if(BattleEffectConfig.Instance.TargetEffectPrefab != null && this.CharacterAI.IsShowTarget)
						{
							GameObject targetEffect = GameObject.Instantiate(BattleEffectConfig.Instance.TargetEffectPrefab) as GameObject;
							Vector3 offset = targetEffect.transform.position;
							
							Vector3 targetEffectPosition = property.AnchorTransform.position;
							targetEffect.transform.position = targetEffectPosition + offset;
							targetEffect.transform.parent = BattleObjectCache.Instance.EffectObjectParent.transform;
						}
								
						BombermanWalkState walkState = new BombermanWalkState(this.CharacterAI.BattleMapData, endPoint, this.m_AIBehavior, targetBuilding);
						walkState.SetPath(linePath);
						this.m_AIBehavior.ChangeState(walkState);
						break;
					}
				}
			}
		}
	}
}
