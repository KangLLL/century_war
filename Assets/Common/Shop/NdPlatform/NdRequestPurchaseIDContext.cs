using UnityEngine;
using System.Collections;

public class NdRequestPurchaseIDContext : ShopContext 
{
	
	
	public override void Execute ()
	{
		NdShopUtility ndShopUtility = (NdShopUtility)this.ShopModule;
		
		string purchaseID = ndShopUtility.CurrentPurchaseID;
		if(!string.IsNullOrEmpty(purchaseID))
		{
			Debug.Log("purchase id is :" +  purchaseID);
			
			NdBuyContext buyContext = new NdBuyContext();
			this.ShopModule.ChangeContext(buyContext);
			string productID = PlayerPrefs.GetString(NdShopUtility.UNCONFIRMED_PRODUCT_ID_KEY);
			buyContext.Buy(purchaseID, productID);
		}
	}
}
