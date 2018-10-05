using UnityEngine;
using System.Collections;

public class TestLogicCollect : MonoBehaviour 
{
	void Start()
	{
		print(LogicController.Instance.PlayerData.CurrentStoreGold);
		print(LogicController.Instance.PlayerData.GoldMaxCapacity);
	}
	
	void OnClick()
	{
		BuildingIdentity id = new BuildingIdentity();
		id.buildingType = ConfigUtilities.Enums.BuildingType.GoldMine;
		id.buildingNO = 0;
		//LogicController.Instance.Collect(id,ResourceType.Gold, 200);
	}
}
