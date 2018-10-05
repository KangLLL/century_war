using UnityEngine;
using System.Collections;
using System;
using ConfigUtilities.Enums;
using ConfigUtilities;

public class MercenaryProductData  
{
	public int ReadyNumber { get;set; }
	public Nullable<float> RemainingTime { get;set; }
	public MercenaryConfigData ConfigData { get;set; }
}
