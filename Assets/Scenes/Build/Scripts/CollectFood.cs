using UnityEngine;
using System.Collections;

public class CollectFood : CollectCommon
{
    protected void Collect()
    {
        base.Collect(this.BuildingLogicData.BuildingIdentity, ResourceType.Food);
    }
    void OnClick()
    {
        if (this.BuildingLogicData != null)
            base.Collect(this.BuildingLogicData.BuildingIdentity, ResourceType.Food, CollectMethod.Building);
    } 
}
