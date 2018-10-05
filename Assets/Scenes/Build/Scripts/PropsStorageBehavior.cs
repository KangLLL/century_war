using UnityEngine;
using System.Collections;

public class PropsStorageBehavior : BuildingCommon
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
    void ShowWindowPropsStorage()
    {
        UIManager.Instance.UIWindowPropsStorage.BuildingLogicData = base.BuildingLogicData;
        UIManager.Instance.UIWindowPropsStorage.ShowWindow();
    }
}
