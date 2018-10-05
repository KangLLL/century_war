using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public interface IAchievementBuildingInfo : IObstacleInfo 
{
	AchievementBuildingType AchievementBuildingType { get; }
}
