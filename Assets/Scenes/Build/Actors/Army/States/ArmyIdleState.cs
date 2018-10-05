using UnityEngine;
using System.Collections;

public class ArmyIdleState : TimeTickRelatedState 
{
	private IBuildingInfo m_CampInfo;
	
	private TilePosition m_CampPosition;
	 
	public ArmyIdleState(NewAI aiBehavior, IBuildingInfo campInfo) : base(aiBehavior)
	{
		this.m_CampInfo = campInfo;
		ActorConfig config = ActorPrefabConfig.Instance.GetComponent<ActorConfig>();
		this.m_CurrentFrame = Random.Range(config.ArmyIdleMinFrame, config.ArmyIdleMaxFrame + 1);
		this.m_CampPosition = this.m_CampInfo.BuildingPosition;
	}
	
	public override void Initial ()
	{
		this.m_AnimationController.PlayIdleAnimation();
	}
	
	protected override void OnTimeUp ()
	{
		TilePosition targetPosition = ((ArmyAI)this.m_AIBehavior).FindCampStandablePoint(this.m_CampInfo);
		if(this.m_CampPosition.Equals(this.m_CampInfo.BuildingPosition))
		{
			ArmyHoverState hoverState = new ArmyHoverState(((ArmyAI)this.m_AIBehavior).MapData, 
				targetPosition, this.m_AIBehavior, this.m_CampInfo);
			this.m_AIBehavior.ChangeState(hoverState);
		}
		else
		{
			ArmyWalkState walkState = new ArmyWalkState(((ArmyAI)this.m_AIBehavior).MapData, targetPosition,
				this.m_AIBehavior, this.m_CampInfo);
			this.m_AIBehavior.ChangeState(walkState);
		}
	}
}
