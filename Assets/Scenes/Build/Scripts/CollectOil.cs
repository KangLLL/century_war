using UnityEngine;
using System.Collections;

public class CollectOil : CollectCommon
{
    /*
	// Use this for initialization
	void Start () {
        //base.CurrentStore = base.BuildingData.CurrentStoreOil;
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Collect(base.BuildingData.StoreOilCapacity, base.BuildingData.ProduceOilEfficiency);
	}
    protected override void Collect(int capacity, int efficiency)
    {
        //base.Collect(capacity, efficiency);
        //base.BuildingData.CurrentStoreOil = base.CurrentStore;
    }
    protected override void GetCollect()
    {
        //if (base.IsFull)
        //{
        //    base.GetCollect();
        //    base.BuildingData.CurrentStoreOil = 0;
        //}
    }
    protected override void UIGetCollect()
    {
        //if (base.EnableCollect)
        //{
        //    base.UIGetCollect();
        //    base.BuildingData.CurrentStoreOil = 0;
        //}

    }
    */
    protected void Collect()
    {
        base.Collect(this.BuildingLogicData.BuildingIdentity, ResourceType.Oil);
    }
    void OnClick()
    {
        if (this.BuildingLogicData != null)
            base.Collect(this.BuildingLogicData.BuildingIdentity, ResourceType.Oil, CollectMethod.Building);
    } 
  
}
