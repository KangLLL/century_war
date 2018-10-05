using UnityEngine;
using System.Collections;

public class NdIdleContext : ShopContext 
{
	public void PurchaseProduct(string productID)
	{
		Debug.Log("start buying!");
		
		PlayerPrefs.SetString(NdShopUtility.UNCONFIRMED_PRODUCT_ID_KEY, productID);
		PlayerPrefs.Save();
		CommunicationUtility.Instance.GetPurchaseID(this.ShopModule, "ReceivedPurchaseID", true);
		NdRequestPurchaseIDContext requestContext = new NdRequestPurchaseIDContext();
		this.ShopModule.ChangeContext(requestContext);
	}
}
