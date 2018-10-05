using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using CommandConsts;
using ConfigUtilities.Enums;

public class CGReader : ReplayDataReader 
{	
	[SerializeField]
	private TextAsset m_BattleRecord;
	[SerializeField]
	private TextAsset m_MapRecord;
	
	protected override string GetMapInformation ()
	{
		return this.m_MapRecord.text;
	}
	
	protected override string GetBattleInformation ()
	{
		return this.m_BattleRecord.text;
	}
}
