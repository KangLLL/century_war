using UnityEngine;
using System.Collections;

public enum PurchaseFailedReason
{
	Cancel,
	Abort,
	ComfirmFail,
}

public class PurchaseFailInformation  
{
	public PurchaseFailedReason Reason { get;set; }
	public string ErrorDescription { get;set; }
}
