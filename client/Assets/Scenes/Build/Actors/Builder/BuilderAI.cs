using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class BuilderAI : NewAI 
{
	public IMapData MapData { get;set; }
	public int BuilderNO { get;set; }
	
	public void Build(IObstacleInfo targetInfo)
	{
		BuilderWalkState walkState = new BuilderWalkState(targetInfo, this.MapData, targetInfo.ActorPosition, this);//targetData, targetData.BuildingPosition, this);
		this.ChangeState(walkState);
	}
	
	public void Build(IObstacleInfo targetInfo, TilePosition buildPosition)
	{
		BuilderBuildState buildState = new BuilderBuildState(this, targetInfo, this.MapData, buildPosition);
		this.ChangeState(buildState);
	}
	
	public void FinishBuild()
	{
		BuildingLogicData logicData = LogicController.Instance.GetBuildingObject(
			new BuildingIdentity(BuildingType.BuilderHut, this.BuilderNO));
		TilePosition targetPosition = BorderPointHelper.FindValidInflateOneBorderPoint(logicData);
		
		BuilderReturnState returnState = new BuilderReturnState(this.MapData, targetPosition, this, logicData);
		this.ChangeState(returnState);
	}
	
	public void RunAway(List<IBuildingInfo> builderHuts)
	{
		IBuildingInfo targetInfo = builderHuts[this.BuilderNO];
		TilePosition targetPoint = BorderPointHelper.FindValidInflateOneBorderPoint(targetInfo);
		
		ActorRunAwayState runAwayState = new ActorRunAwayState(this.MapData, targetPoint, this);
		this.ChangeState(runAwayState);
	}
}
