using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public struct MercenaryIdentity
{
	public MercenaryType mercenaryType;
	public int mercenaryNO;
	
	public MercenaryIdentity(MercenaryType type, int NO)
	{
		this.mercenaryType = type;
		this.mercenaryNO = NO;
	}
	
	public override bool Equals (object obj)
	{
		if(obj == null)
		{
			return false;
		}
		else if(obj is MercenaryIdentity)
		{
			return ((MercenaryIdentity)obj).mercenaryType == this.mercenaryType && ((MercenaryIdentity)obj).mercenaryNO == this.mercenaryNO;
		}
		else
		{
			return false;
		}
	}
	
	public override int GetHashCode ()
	{
		return this.mercenaryNO << 6 | (int)this.mercenaryType;
	}

	public static bool operator == (MercenaryIdentity a1, MercenaryIdentity a2)
	{
		return a1.Equals(a2);
	}
	
	public static bool operator != (MercenaryIdentity a1, MercenaryIdentity a2)
	{
		return !(a1.Equals(a2));
	}
}
