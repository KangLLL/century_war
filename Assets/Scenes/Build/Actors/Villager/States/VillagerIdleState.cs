using UnityEngine;
using System.Collections;

public class VillagerIdleState : TimeTickRelatedState 
{
	private IBuildingInfo m_TargetInfo;
	private ActorConfig m_ActorConfig;
	private bool m_IsTargetHover;
	
	public VillagerIdleState(NewAI aiBehavior, IBuildingInfo targetInfo, bool isTargetHover) : base(aiBehavior)
	{
		this.m_TargetInfo = targetInfo;
		this.m_ActorConfig = ActorPrefabConfig.Instance.GetComponent<ActorConfig>();
		this.m_IsTargetHover = isTargetHover;
	}
	
	public override void Initial ()
	{
		this.m_CurrentFrame = this.m_ActorConfig.VillagerIdleFrame;
		TilePosition buildingFirstPosition = this.m_TargetInfo.BuildingPosition + this.m_TargetInfo.BuildingObstacleList[0];
	    if(!this.m_IsTargetHover)
		{
			GameObject target = ((VillagerAI)this.m_AIBehavior).MapData.
				GetBuildingObjectFromBuildingObstacleMap(buildingFirstPosition.Row, buildingFirstPosition.Column);
			Vector3 targetPosition = target.transform.FindChild(ClientStringConstants.BUILDING_ANCHOR_OBJECT_NAME).position;
			this.m_AnimationController.PlayIdleAnimation(targetPosition);
		}
		else
		{
			this.m_AnimationController.PlayIdleAnimation();
		}
	}
	
	protected override void OnTimeUp ()
	{
		((VillagerAI)this.m_AIBehavior).FindTargetObject(this.m_TargetInfo, null);
	}
}
