using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class EditorAchievementBuildingBehavior : EditorObjectBehavior 
{
	public AchievementBuildingType AchievementBuildingType { get; set; }

	void Start()
	{
		this.m_Function = new AchievementBuildingFunction() { Position = this.Position, ObjectType = this.AchievementBuildingType, OperatorBehavior = this };
		this.m_Function.Initial();
	}
}
