using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class ArmyButtonInformation : DropButtonInformation<ArmyType, ArmyIdentity> 
{
	[SerializeField]
	private UILabel m_LevelLabel;
	
	public int ArmyLevel { get; set; }
	
	public override void Start () 
	{
		base.Start();
		this.m_LevelLabel.text = StringConstants.PROMPT_LEVEL + this.ArmyLevel.ToString();
	}
}
