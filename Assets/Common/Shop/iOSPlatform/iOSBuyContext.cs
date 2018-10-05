using UnityEngine;
using System.Collections;

public class iOSBuyContext : ShopContext 
{
	private string m_ProductID;
	
	public iOSBuyContext(string productID)
	{
		this.m_ProductID = productID;
	}
	
	public override void Execute ()
	{
		PurchaseState purchaseState = StoreKitHelper.GetPurchaseState();
		Debug.Log("the state is : " + purchaseState.ToString()); 
		if(purchaseState == PurchaseState.PurchaseSuccess)
		{
			iOSConfirmContext confirmContext = new iOSConfirmContext();
			this.ShopModule.ChangeContext(confirmContext);
			confirmContext.StartProductConfirm(this.m_ProductID);
		}
		else if(purchaseState == PurchaseState.PurchaseFail || purchaseState == PurchaseState.PurchaseCancel)
		{
			iOSIdleContext idleContext = new iOSIdleContext();
			this.ShopModule.ChangeContext(idleContext);
			if(purchaseState == PurchaseState.PurchaseFail)
			{
				this.ShopModule.State = ShopActionState.Fail;
				PurchaseFailInformation failInformation = new PurchaseFailInformation();
				failInformation.Reason = PurchaseFailedReason.Abort;
				failInformation.ErrorDescription = ClientStringConstants.PURCHASE_FAIL_TIPS;
				this.ShopModule.OnPurchaseFailed(failInformation);
			}
			else
			{
				this.ShopModule.State = ShopActionState.Cancel;
				PurchaseFailInformation failInformation = new PurchaseFailInformation();
				failInformation.Reason = PurchaseFailedReason.Cancel;
				failInformation.ErrorDescription = ClientStringConstants.PURCHASE_FAIL_TIPS;
				this.ShopModule.OnPurchaseFailed(failInformation);
			}
		}
	}
}
