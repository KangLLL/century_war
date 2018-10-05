using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;

public class IgnoreTargetWeightStrategy : CharacterGCalculator 
{
	public IgnoreTargetWeightStrategy(IMapData mapData, int destinationRow, int destinationColumn)
		:base(mapData, destinationRow, destinationColumn)
	{
	}
	
	protected override void CalculateLightPositions (GameObject target, int targetRow, int targetColumn)
	{
		base.CalculateLightPositions (target, targetRow, targetColumn);
		if(target != null)
		{
			IObstacleInfo obstacleInfo = this.m_MapData.GetObstacleInfoFormActorObstacleMap(targetRow, targetColumn);
			TilePosition position = obstacleInfo.ActorPosition;
			foreach(TilePosition offset in obstacleInfo.ActorObstacleList)
			{
				TilePosition lightPosition = position + offset;
				//Debug.Log(lightPosition.Row + " , " + lightPosition.Column);
				this.m_PreviousTargetLightPositions.Add(lightPosition.GetIndexInt());
			}
		}
	}
}
