using UnityEngine;
using System.Collections;
using System;
using ConfigUtilities;

public class DefenseObjectData  
{
	public long DefenseObjectID { get;set; }
	public TilePosition Position { get;set; }
	public string Name { get;set; }
	
	public DefenseObjectConfigWrapper ConfigData { get;set; }
}
