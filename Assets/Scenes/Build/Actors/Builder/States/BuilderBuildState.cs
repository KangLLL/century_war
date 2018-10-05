using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuilderBuildState : TimeTickRelatedState 
{
	private TilePosition m_TargetPosition;
	private IObstacleInfo m_TargetInfo;
	
	private TilePosition m_BuildPosition;
	private IMapData m_MapData;
	
	public BuilderBuildState(NewAI aiBehavior, IObstacleInfo targetInfo, IMapData mapData, TilePosition buildPosition) : base(aiBehavior)
	{
		this.m_TargetInfo = targetInfo;
		this.m_TargetPosition = this.m_TargetInfo.BuildingPosition;
		this.m_BuildPosition = buildPosition;
		this.m_MapData = mapData;
	}
	
	public override void Initial ()
	{
		ActorConfig config = ActorPrefabConfig.Instance.GetComponent<ActorConfig>();
		this.m_CurrentFrame = config.BuildAnimationFrame;
		
		if(SceneManager.Instance != null)
		{
			GameObject go = SceneManager.Instance.GetBuildingObjectFromBuildingObstacleMap(this.m_TargetInfo.BuildingPosition.Row,
				this.m_TargetInfo.BuildingPosition.Column);
			if(go != null)
			{
				Transform anchorPosition = go.transform.FindChild(ClientStringConstants.BUILDING_ANCHOR_OBJECT_NAME);
				if(anchorPosition != null)
				{
					this.m_AnimationController.PlayBuildAnimation(anchorPosition.position);
				}
				else
				{
					this.m_AnimationController.PlayBuildAnimation(PositionConvertor.GetWorldPositionFromActorTileIndex(this.m_TargetInfo.ActorPosition));
				}
			}
		}
		else
		{
			BuildingBasePropertyBehavior property = this.m_TargetInfo as BuildingBasePropertyBehavior;
			if(property != null)
			{
				this.m_AnimationController.PlayBuildAnimation(property.AnchorTransform.position);
			}
			else
			{
				this.m_AnimationController.PlayBuildAnimation(PositionConvertor.GetWorldPositionFromActorTileIndex(this.m_TargetInfo.ActorPosition));
			}
		}
	}
	
	public override void AICalculate ()
	{
		if(!this.m_TargetInfo.BuildingPosition.Equals(this.m_TargetPosition))
		{
			BuilderWalkState walkState = new BuilderWalkState(this.m_TargetInfo, this.m_MapData, this.m_TargetInfo.ActorPosition, this.m_AIBehavior);
			this.m_AIBehavior.ChangeState(walkState);
		}
		else
		{
			base.AICalculate();
		}
	}
	
	protected override void OnTimeUp ()
	{
		TilePosition newBuildPosition = BorderPointHelper.FindValidInflateOneBorderPoint(this.m_TargetInfo);
		
		if(newBuildPosition.Equals(this.m_BuildPosition))
		{
			this.Initial();
		}
		else
		{
			BuilderBuildHoverState hoverState = new BuilderBuildHoverState(this.m_TargetInfo, this.m_MapData, newBuildPosition, this.m_AIBehavior);//this.m_TargetInfo, newBuildPosition, this.m_AIBehavior);
			this.m_AIBehavior.ChangeState(hoverState);
		}
	}
}
