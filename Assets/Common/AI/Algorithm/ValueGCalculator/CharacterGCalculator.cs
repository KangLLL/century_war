using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class CharacterGCalculator : IGCalculator 
{
	private int m_DestinationRow;
	private int m_DestinationColumn;
	
	protected IMapData m_MapData; 
	
	private GameObject m_PreviousCalculateTarget;
	protected List<int> m_PreviousTargetLightPositions;
	
	protected virtual void CalculateLightPositions(GameObject target, int targetRow, int targetColumn)
	{
		this.m_PreviousTargetLightPositions.Clear();
	}
	
	private int GetWeight(int row, int column, int destinationRow, int destinationColumn) 
	{
		GameObject target = this.m_MapData.GetBulidingObjectFromActorObstacleMap(destinationRow, destinationColumn);
		if(!this.m_MapData.ActorCanPass(destinationRow, destinationColumn))
		{
			if(target != this.m_PreviousCalculateTarget)
			{
				this.CalculateLightPositions(target, destinationRow, destinationColumn);
				this.m_PreviousCalculateTarget = target;
			}
			if(this.IsInLightArray(row, column))
			{
				return 1;
			}
		}
		IObstacleInfo obstacleInfo = this.m_MapData.GetObstacleInfoFormActorObstacleMap(row, column);
		int weight = obstacleInfo == null ? 1 : this.GetWeightAccordingToObstacle(obstacleInfo);
		return weight;
	}
	
	protected virtual bool IsInLightArray(int row, int column)
	{
		if(this.m_MapData.GetBulidingObjectFromActorObstacleMap(row, column) == null)
		{
			return false;
		}
		
		return this.m_PreviousTargetLightPositions.Contains(column + (row << 16));
	}
	
	protected virtual int GetWeightAccordingToObstacle(IObstacleInfo obstacle)
	{
		if(obstacle is IRemovableObjectInfo)
		{
			return 1000;
		}
		else
		{
			return 20;
		}
	}
	
	public CharacterGCalculator(IMapData mapData, int destinationRow, int destinationColumn)
	{
		this.m_DestinationRow = destinationRow;
		this.m_DestinationColumn = destinationColumn;
		this.m_MapData = mapData;
		this.m_PreviousTargetLightPositions = new List<int>();
	}
	
	public int GetGValue(int row, int column)
	{
		return this.GetWeight(row, column, this.m_DestinationRow, this.m_DestinationColumn);
	}
}
