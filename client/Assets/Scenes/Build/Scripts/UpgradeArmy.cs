using UnityEngine;
using System.Collections;

public class UpgradeArmy : UpgradeCommon {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void ShowWindowUpgradeArmy()
    {
        UIManager.Instance.UIWindowUpgradeArmy.BuildingLogicData = base.BuildingLogicObject;
        UIManager.Instance.UIWindowUpgradeArmy.ShowWindow();
    }
}
