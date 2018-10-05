using UnityEngine;
using System.Collections;

public class ShopContext  
{
	public ShopUtility ShopModule
	{get;set;}
	public ShopContext SuccessorContext
	{get;set;}
	
	public virtual void Execute()
	{
	}
}
