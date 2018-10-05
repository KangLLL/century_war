using UnityEngine;
using System.Collections;

public class TestLogicCancel : MonoBehaviour 
{

	public void OnClick()
	{
		BuildingIdentity id = new BuildingIdentity();
		id.buildingType = ConfigUtilities.Enums.BuildingType.GoldMine;
		id.buildingNO = 0;
		LogicController.Instance.CancelBuildingConstruct(id);
		Debug.Log("################################!");
	}
}
