using UnityEngine;
using System.Collections;
using System;
public class DefenseTowerBehavior : BuildingCommon
{
 
    void Update()
    {
        this.ShowProgress();
        base.OnReadyForUpgradeFx();
        //this.PlayIdleAnimation();
    }
 
    protected override void ShowProgress()
    {
        base.ShowProgress();
    }
 
 
}
