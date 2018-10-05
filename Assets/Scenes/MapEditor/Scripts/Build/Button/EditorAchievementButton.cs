using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class EditorAchievementButton : MonoBehaviour 
{
	void OnClick()
	{
		EditorFactory.Instance.ConstructAchievementBuilding(AchievementBuildingType.AncientTotem);
	}
}
