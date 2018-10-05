using UnityEngine;
using System.Collections;
using CommonUtilities;

public class ProductFacade<T> : IOrderable
{
	public IProduciable<T> Product { get;set; }
	public int OrderSecond { get;set; }
	public float RemainingSeond { get;set; }

	#region IOrderable implementation
	public long GetOrderIndex ()
	{
		return this.OrderSecond;
	}
	#endregion
}
