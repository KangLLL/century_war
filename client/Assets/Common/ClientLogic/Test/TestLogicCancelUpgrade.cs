using UnityEngine;
using System.Collections;

public class TestLogicCancelUpgrade : MonoBehaviour 
{

	public void OnClick()
	{
		BuildingIdentity id = new BuildingIdentity();
		id.buildingType = ConfigUtilities.Enums.BuildingType.GoldMine;
		id.buildingNO = 0;
		LogicController.Instance.CancelBuildingUpgrade(id);
		Debug.Log("################################!");
	}
}
