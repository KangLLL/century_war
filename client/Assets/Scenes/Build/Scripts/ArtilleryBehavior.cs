using UnityEngine;
using System.Collections;

public class ArtilleryBehavior : BuildingCommon
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
}
