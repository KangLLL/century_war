using UnityEngine;
using System.Collections;

public class CityHallBehavior :BuildingCommon {
	// Use this for initialization
    //void Start () {
    //    this.CreateFacility();
    //    this.CreateComponent();
    //    this.Initial();
    //}
	
	// Update is called once per frame
	void Update () {
        this.ShowProgress();
        base.OnReadyForUpgradeFx();
	}
    //protected override void Initial()
    //{
    //    base.Initial();
    //}
    protected override void ShowProgress()
    {
        base.ShowProgress(); 
    }
    //protected override void CreateComponent()
    //{
    //    base.CreateComponent();
    //}
    //protected override void ActiveFacility(object prama)
    //{
    //    base.ActiveFacility(prama);
    //}
    //protected override void CreateFacility()
    //{
    //    base.CreateFacility();
    //}
 
    //protected override void ShowWindowBuildingInfomation()
    //{
    //    base.ShowWindowBuildingInfomation();
    //}
}
