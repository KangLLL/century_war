using UnityEngine;
using System.Collections;

using CommandConsts;

public class NdConfirmContext : ShopContext
{
	private bool m_IsConfirm;
	
	public override void Execute ()
	{
		if(!m_IsConfirm)
		{	
			string purchaseID = PlayerPrefs.GetString(NdShopUtility.UNCONFIRMED_PURCHASE_ID_KEY);
			string productID = PlayerPrefs.GetString(NdShopUtility.UNCONFIRMED_PRODUCT_ID_KEY);
			ConfirmNdPurchaseRequestParameter request = new ConfirmNdPurchaseRequestParameter();
			request.PurchaseID = purchaseID;
			request.ProductID = productID;
			CommunicationUtility.Instance.ConfirmNdPurchase(request, this.ShopModule, "ReceivedConfirmResponse", true);
			
			this.m_IsConfirm = true;
		}
		else
		{
			NdShopUtility ndShopUtility = (NdShopUtility)this.ShopModule;
			if(ndShopUtility.IsReceivedConfirmResponse)
			{
				NdIdleContext context = new NdIdleContext();
				this.ShopModule.ChangeContext(context);
				
			    if(string.IsNullOrEmpty(ndShopUtility.CurrentConfirmError))
				{
					this.ShopModule.State = ShopActionState.Success;
					string productID = PlayerPrefs.GetString(NdShopUtility.UNCONFIRMED_PRODUCT_ID_KEY);
					ShopItemInformation shopItem = ndShopUtility.ProductsDict[productID];
					this.ShopModule.OnPurchaseSuccessed(shopItem);
				}
				else
				{
					this.ShopModule.State = ShopActionState.Fail;
					this.ShopModule.OnPurchaseFailed(new PurchaseFailInformation() 
					{ Reason = PurchaseFailedReason.ComfirmFail, ErrorDescription = ndShopUtility.CurrentConfirmError });
				}
				PlayerPrefs.DeleteKey(NdShopUtility.UNCONFIRMED_PURCHASE_ID_KEY);
				PlayerPrefs.DeleteKey(NdShopUtility.UNCONFIRMED_PRODUCT_ID_KEY);
				PlayerPrefs.Save();
			}
		}
	}
}
