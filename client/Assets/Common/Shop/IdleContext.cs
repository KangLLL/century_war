using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IdleContext : ShopContext 
{
	public void PurchaseProduct(string id)
	{       
        StoreKitHelper.PurchaseProduct(id, 1);
        Debug.Log("PurchaseProduct!--->productID:" + id);
		this.ShopModule.State = ShopActionState.Operating;
		BuyingContext buyingContext = new BuyingContext()
		{ProductID = id};
		if(StoreKitHelper.IsProductAvailable(id))
		{
			this.ShopModule.ChangeContext(buyingContext);
		}
		else
		{
			RequestingContext requestingContext = new RequestingContext(){SuccessorContext = buyingContext};
			this.ShopModule.ChangeContext(requestingContext);
			requestingContext.StartRequest(new List<string>(){id}, false);
		}
    }
}
