using UnityEngine;
using System.Collections;
using ConfigUtilities;
using ConfigUtilities.Enums;
using System;

public class RemovableObjectData  
{
	public int RemovableObjectNo { get; set; }
	public RemovableObjectType RemovableObjectType { get; set; }
	public RemovableObjectConfigData ConfigData { get; set; }
	public TilePosition Position { get; set; }
	
	public Nullable<int> BuilderBuildingNO { get; set; }
	public Nullable<float> RemainingWorkload { get; set; }
	
	public int RewardExp { get;set; }
	public int RewardGem { get;set; }
	public Nullable<int> RewardProps { get;set; }
	public Nullable<PropsType> RewardPropsType { get;set; }
}
