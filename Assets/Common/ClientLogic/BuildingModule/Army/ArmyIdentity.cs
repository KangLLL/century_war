using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public struct ArmyIdentity  
{
	public ArmyType armyType;
	public int armyNO;
	
	public ArmyIdentity(ArmyType type, int NO)
	{
		this.armyType = type;
		this.armyNO = NO;
	}
	
	public override bool Equals (object obj)
	{
		if(obj == null)
		{
			return false;
		}
		else if(obj is ArmyIdentity)
		{
			return ((ArmyIdentity)obj).armyType == this.armyType && ((ArmyIdentity)obj).armyNO == this.armyNO;
		}
		else
		{
			return false;
		}
	}
	
	public override int GetHashCode ()
	{
		return this.armyNO << 6 | (int)this.armyType;
	}
	
	public static bool operator == (ArmyIdentity a1, ArmyIdentity a2)
	{
		return a1.Equals(a2);
	}
	
	public static bool operator != (ArmyIdentity a1, ArmyIdentity a2)
	{
		return !(a1.Equals(a2));
	}
}
