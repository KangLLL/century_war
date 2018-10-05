using UnityEngine;
using System.Collections;

public class IgnoreTargetSourceWeightStrategy : IgnoreTargetWeightStrategy 
{
	private int m_SourceRow;
	private int m_SourceColumn;
	
	public IgnoreTargetSourceWeightStrategy(IMapData mapData, int destinationRow, int destinationColumn, int sourceRow, int sourceColumn)
		: base(mapData, destinationRow, destinationColumn)
	{
		this.m_SourceRow = sourceRow;
		this.m_SourceColumn = sourceColumn;
	}
	
	protected override void CalculateLightPositions (GameObject target, int targetRow, int targetColumn)
	{
		base.CalculateLightPositions (target, targetRow, targetColumn);
		
		IObstacleInfo obstacleInfo = this.m_MapData.GetObstacleInfoFormActorObstacleMap(this.m_SourceRow, this.m_SourceColumn);
		if(obstacleInfo != null)
		{
			TilePosition position = obstacleInfo.ActorPosition;
			foreach(TilePosition offset in obstacleInfo.ActorObstacleList)
			{
				TilePosition lightPosition = position + offset;
				this.m_PreviousTargetLightPositions.Add(lightPosition.GetIndexInt());
			}
		}
	}
}
