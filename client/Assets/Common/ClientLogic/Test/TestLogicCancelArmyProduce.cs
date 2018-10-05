using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class TestLogicCancelArmyProduce : MonoBehaviour 
{
	void OnClick()
	{
		//ArmyIdentity id = new ArmyIdentity(ArmyType.Berserker, 1);
		BuildingIdentity id = new BuildingIdentity(BuildingType.Barracks, 0);
		LogicController.Instance.CancelProduceArmy(ArmyType.Berserker, id);
	}
}
