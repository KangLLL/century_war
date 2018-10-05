using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public struct BuildingIdentity 
{
	public BuildingType buildingType;
	public int buildingNO;
	
	public BuildingIdentity(BuildingType type, int NO)
	{
		this.buildingType = type;
		this.buildingNO = NO;
	}
	
	public override bool Equals (object obj)
	{
		if(obj == null)
		{
			return false;
		}
		else if(obj is BuildingIdentity)
		{
			return ((BuildingIdentity)obj).buildingType == this.buildingType && ((BuildingIdentity)obj).buildingNO == this.buildingNO;
		}
		else
		{
			return false;
		}
	}
	
	public override int GetHashCode ()
	{
		return this.buildingNO << 6 | (int)this.buildingType;
	}
	
	public static bool operator == (BuildingIdentity b1, BuildingIdentity b2)
	{
		return b1.Equals(b2);
	}
	
	public static bool operator != (BuildingIdentity b1, BuildingIdentity b2)
	{
		return !(b1.Equals(b2));
	}
}
