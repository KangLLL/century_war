using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public interface IBuildingInfo : IObstacleInfo
{
	BuildingType BuildingType { get; }
	int Level { get; }
}
