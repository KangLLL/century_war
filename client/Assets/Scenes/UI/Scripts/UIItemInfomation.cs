using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class UIItemInfomation : UIItemCommon
{ 
    public bool IsLock { get; set; }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    //public bool SetCostItemData()
    //{
    //    return base.SetCostItemData();
    //}
    //public bool SetItemData()
    //{
    //    return base.SetItemData();
    //}
    public new bool SetLock(bool isLock)
    {
        //print("isLock = " + isLock);
        base.SetLock(isLock);
        return isLock;
    }
}
