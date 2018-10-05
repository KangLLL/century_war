using UnityEngine;
using System.Collections;

public class ProduceArmy : ProduceCommon
{

	// Use this for initialization
	void Start () {
	
	}
	
	
    void ShowWindowProduceArmy()
    {
        UIManager.Instance.UIWindowBuyArmy.BuildingLogicData = base.BuildingLogicData;
        UIManager.Instance.UIWindowBuyArmy.ShowWindow();
    }
}
