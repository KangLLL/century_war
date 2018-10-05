using UnityEngine;
using System.Collections;
using ConfigUtilities;

public class ItemData 
{
	public ItemIdentity ItemID { get; set; }
	public ItemConfigData ConfigData { get; set; }
	
	public float ProduceRemainingWorkload { get; set; }
}
