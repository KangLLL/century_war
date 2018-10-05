using UnityEngine;
using System.Collections;
using System;
using ConfigUtilities.Enums;
using ConfigUtilities;

public class ArmyData 
{
	public ArmyIdentity ArmyID { get; set; }
	public ArmyConfigData ConfigData { get; set; }
	public BuildingIdentity CampID { get; set; }
	
	public float ProduceRemainingWorkload { get; set; }
}
