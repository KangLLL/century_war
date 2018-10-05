using UnityEngine;
using System.Collections;

public class CollectGold : CollectCommon
{
    protected void Collect()
    {
        base.Collect(this.BuildingLogicData.BuildingIdentity, ResourceType.Gold);
    }
    void OnClick()
    {
        if (this.BuildingLogicData != null)
            base.Collect(this.BuildingLogicData.BuildingIdentity, ResourceType.Gold, CollectMethod.Building);
    }

}
