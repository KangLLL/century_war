using UnityEngine;
using System.Collections;

public class TestLogicUpgradeFinish : MonoBehaviour 
{
	public void OnClick()
	{
		BuildingIdentity id = new BuildingIdentity();
		id.buildingType = ConfigUtilities.Enums.BuildingType.GoldMine;
		id.buildingNO = 0;
		LogicController.Instance.FinishBuildingUpgrade(id);
		Debug.Log("################################!");
	}
}
