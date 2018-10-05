using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class KodoPathFindStrage : IgnoreTargetWeightStrategy 
{
	public KodoPathFindStrage(IMapData mapData, int destinationRow, int destinationColumn) 
		: base(mapData, destinationRow, destinationColumn)
	{
	}
	
	protected override int GetWeightAccordingToObstacle (IObstacleInfo obstacle)
	{
		if(obstacle is IBuildingInfo)
		{
			if(((IBuildingInfo)obstacle).BuildingType == BuildingType.Wall)
			{
				return 10;
			}
		}
		
		return base.GetWeightAccordingToObstacle (obstacle);
	}
}
