using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
using ConfigUtilities;

public class BuffData : ICD
{
	public PropsType RelatedPropsType { get; set; }
	public float RemainingCD { get; set; }
	
	public PropsBuffConfigData BuffConfigData { get; set; }
}
