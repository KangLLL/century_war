using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class AchievementBuildingPropertyBehavior : BuildingBasePropertyBehavior, IAchievementBuildingInfo
{
	private AchievementBuildingType m_AchievementBuildingType;
	
	public AchievementBuildingType AchievementBuildingType
	{
		get
		{
			return this.m_AchievementBuildingType;
		}
		set
		{
			this.m_AchievementBuildingType = value;
		}
	}
	
	private bool m_IsDropProps;
	
	public bool IsDropProps
	{
		get 
		{
			return this.m_IsDropProps;
		}
		set
		{
			this.m_IsDropProps = value;
		}
	}
}
