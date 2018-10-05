using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public struct ItemIdentity 
{
	public ItemType itemType;
	public int itemNO;
	
	public ItemIdentity(ItemType type, int NO)
	{
		this.itemType= type;
		this.itemNO = NO;
	}
	
	public override bool Equals (object obj)
	{
		if(obj == null)
		{
			return false;
		}
		else if(obj is ItemIdentity)
		{
			return ((ItemIdentity)obj).itemType == this.itemType && ((ItemIdentity)obj).itemNO == this.itemNO;
		}
		else
		{
			return false;
		}
	}
	
	public override int GetHashCode ()
	{
		return this.itemNO << 6 | (int)this.itemType;
	}
	
	public static bool operator == (ItemIdentity i1, ItemIdentity i2)
	{
		return i1.Equals(i2);
	}
	
	public static bool operator != (ItemIdentity i1, ItemIdentity i2)
	{
		return !(i1.Equals(i2));
	}
}
