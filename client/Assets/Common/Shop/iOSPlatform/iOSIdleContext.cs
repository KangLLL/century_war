using UnityEngine;
using System.Collections;

public class iOSIdleContext : ShopContext 
{
	public void PurchaseProduct(string productID)
	{
        StoreKitHelper.PurchaseProduct(productID, 1);
        Debug.Log("PurchaseProduct!--->productID:" + productID);
		this.ShopModule.State = ShopActionState.Operating;
		iOSBuyContext buyContext = new iOSBuyContext(productID);
		this.ShopModule.ChangeContext(buyContext);
	}
}
