using UnityEngine;
using System.Collections;

public class TavernBehavior : BuildingCommon
{
    void Update()
    {
        this.ShowProgress();
        base.OnReadyForUpgradeFx();
    } 
    protected override void ShowProgress()
    {
        base.ShowProgress();
    }
    void ShowWindowTavern()
    {
        print("ShowWindowTavern");
        UIManager.Instance.UIWindowBuyMercenary.BuildingLogicData = base.BuildingLogicData;
        UIManager.Instance.UIWindowBuyMercenary.ShowWindow();
    }
}
