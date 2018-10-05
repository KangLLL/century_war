using UnityEngine;
using System.Collections;
using System;
using ConfigUtilities.Enums;

[Serializable]
public class DropArmySerializableInformation  
{
	[SerializeField]
	private ArmyType m_ArmyType;
	[SerializeField]
	private int m_ArmyLevel;
	
	public ArmyType ArmyType { get { return this.m_ArmyType; } }
	public int ArmyLevel { get { return this.m_ArmyLevel; } }
}
