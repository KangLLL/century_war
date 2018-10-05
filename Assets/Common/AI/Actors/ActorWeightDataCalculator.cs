using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class ActorWeightDataCalculator : MonoBehaviour 
{
	private IMapData m_MapData; 
	
	private GameObject m_PreviousCalculateTarget;
	protected List<int> m_PreviousTargetLightPositions;
	
	public IMapData MapData
	{
		get { return this.m_MapData; }
		set { this.m_MapData = value; }
	}
	
	void Awake()
	{
		this.m_PreviousTargetLightPositions = new List<int>();
	}
	
	protected virtual void CalculateLightPositions(GameObject target)
	{
		this.m_PreviousTargetLightPositions.Clear();
	}
	
	public int GetWeight(int row, int column, int destinationRow, int destinationColumn) 
	{
		GameObject target = this.m_MapData.GetBuildingObjectFromBuildingObstacleMap(destinationRow, destinationColumn);
		if(!this.m_MapData.ActorCanPass(destinationRow, destinationColumn))
		{
			if(target != this.m_PreviousCalculateTarget)
			{
				this.CalculateLightPositions(target);
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
		
		return this.m_PreviousTargetLightPositions.Contains(column + row << 16);
	}
	
	protected virtual int GetWeightAccordingToObstacle(IObstacleInfo obstacle)
	{
		return 4;
	}
}
