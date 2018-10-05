using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class IgnoreTargetAndAttackScopeWeightStrategy : CharacterGCalculator
{
	private float m_AttackScope;
	
	public IgnoreTargetAndAttackScopeWeightStrategy(IMapData mapData, int destinationRow, int destinationColumn, float attackScope)
		: base( mapData, destinationRow, destinationColumn)
	{
		this.m_AttackScope = attackScope;
	}
	
	protected override void CalculateLightPositions (GameObject target, int targetRow, int targetColumn)
	{
		base.CalculateLightPositions (target, targetRow, targetColumn);
		IObstacleInfo property = this.m_MapData.GetObstacleInfoFormActorObstacleMap(targetRow, targetColumn);
		
		int radius = Mathf.FloorToInt(m_AttackScope / 
			Mathf.Min(ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.width, 
			ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.height));
		radius = Mathf.Max(0, radius - 1);
		foreach(TilePosition offset in property.ActorObstacleList)
		{
			TilePosition targetPosition = property.ActorPosition + offset;
			
			List<TilePosition> destinationTiles = RoundHelper.FillCircle(targetPosition.Column, targetPosition.Row, radius);
			foreach(TilePosition lightPosition in destinationTiles)
			{
				if(lightPosition.IsValidActorTilePosition())
				{
					if(!this.m_PreviousTargetLightPositions.Contains(lightPosition.Column + (lightPosition.Row << 16)))
					{
						this.m_PreviousTargetLightPositions.Add(lightPosition.Column + (lightPosition.Row << 16));
					}
				}
			}
		}
	}
	
	
	protected override bool IsInLightArray(int row, int column)
	{
		GameObject relatedObject = this.m_MapData.GetBulidingObjectFromActorObstacleMap(row, column);
		if(relatedObject == null)
		{
			return false;
		}
		IObstacleInfo property = this.m_MapData.GetObstacleInfoFormActorObstacleMap(row, column);
		TilePosition buildingPosition = property.ActorPosition;
		
		foreach(TilePosition offset in property.ActorObstacleList)
		{
			TilePosition position = buildingPosition + offset;
			if(position.IsValidActorTilePosition()
				&& !this.m_PreviousTargetLightPositions.Contains(position.Column + (position.Row << 16)))
			{
				return false;
			}
		}
		return true;
	}
}
