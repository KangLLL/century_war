using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class TestLogicArmyUpgrade : MonoBehaviour 
{
	void OnClick()
	{
		BuildingIdentity id = new BuildingIdentity(BuildingType.Barracks, 0);
		LogicController.Instance.UpgradeArmy(ArmyType.Berserker, id);
	}
}
