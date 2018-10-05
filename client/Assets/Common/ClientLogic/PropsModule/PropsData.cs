using UnityEngine;
using System.Collections;
using ConfigUtilities;
using ConfigUtilities.Enums;

public class PropsData : ICD
{
	public int PropsNo { get;set; }
	public float RemainingCD { get;set; }
	public int RemainingUseTime { get;set; }
	public PropsType PropsType { get;set; }
	public bool IsInBattle { get;set; }
	
	public PropsConfigData PropsConfigData { get;set; }
}
