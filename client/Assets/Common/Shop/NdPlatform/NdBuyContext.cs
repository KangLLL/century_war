using UnityEngine;
using System.Collections;

public class NdBuyContext : ShopContext 
{
	public void Buy(string purchaseID, string productID)
	{
		Debug.Log("buying!");
		NdShopUtility shopUtility = (NdShopUtility)this.ShopModule;
		PlayerPrefs.SetString(NdShopUtility.UNCONFIRMED_PURCHASE_ID_KEY, purchaseID);
		PlayerPrefs.Save();
		ShopItemInformation item = shopUtility.ProductsDict[productID];
		NdCenter.Instace.Buy(purchaseID, productID, item.LocaleTitle, double.Parse(item.Price), double.Parse(item.Price), 1, "");
	}
	
	public override void Execute ()
	{
		if(NdCenter.Instace.CurrentBuyState == BuyState.Success)
		{
			NdConfirmContext confirmContext = new NdConfirmContext();
			this.ShopModule.ChangeContext(confirmContext);
		}
		else if(NdCenter.Instace.CurrentBuyState == BuyState.Fail)
		{
			PlayerPrefs.DeleteKey(NdShopUtility.UNCONFIRMED_PRODUCT_ID_KEY);
			PlayerPrefs.DeleteKey(NdShopUtility.UNCONFIRMED_PURCHASE_ID_KEY);
			PlayerPrefs.Save();
			
			NdIdleContext context = new NdIdleContext();
			this.ShopModule.ChangeContext(context);
			this.ShopModule.State = ShopActionState.Fail;
			
			this.ShopModule.OnPurchaseFailed(new PurchaseFailInformation()
			{ Reason = PurchaseFailedReason.Cancel, ErrorDescription = NdCenter.Instace.BuyError });
		}
	}
}
