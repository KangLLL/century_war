using UnityEngine;
using System.Collections;

public class IgnoreTargetPositionWeightStrategy : CharacterGCalculator 
{
	public IgnoreTargetPositionWeightStrategy(IMapData mapData, int destinationRow, int destinationColumn) : 
		base(mapData, destinationRow, destinationColumn)
	{
	}
	
	protected override void CalculateLightPositions (GameObject target, int targetRow, int targetColumn)
	{
		base.CalculateLightPositions (target, targetRow, targetColumn);
		if(target != null)
		{
			this.m_PreviousTargetLightPositions.Add(CommonHelper.GetIndexFormRowColumn(targetRow, targetColumn));
		}
	}
}
