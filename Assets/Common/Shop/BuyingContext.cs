using UnityEngine;
using System.Collections;

public class BuyingContext : ShopContext 
{
	public string ProductID
	{
		get;set;
	}
	
	public override void Execute ()
	{
		PurchaseState purchaseState = StoreKitHelper.GetPurchaseState();
		Debug.Log("the state is : " + purchaseState.ToString()); 
		/*
		if(purchaseState == PurchaseState.PurchasePurchasing)
		{
			NetWorkDialog.Instance.HideConnectingWithoutManager();
		}
		*/
		if(purchaseState == PurchaseState.PurchaseSuccess)
		{
			ConfirmingContext confirmingContext = new ConfirmingContext()
			{ProductID = this.ProductID};
			this.ShopModule.ChangeContext(confirmingContext);
		}
		else if(purchaseState == PurchaseState.PurchaseFail || purchaseState == PurchaseState.PurchaseCancel)
		{
			IdleContext idleContext = new IdleContext();
			this.ShopModule.ChangeContext(idleContext);
			if(purchaseState == PurchaseState.PurchaseFail)
			{
				this.ShopModule.State = ShopActionState.Fail;
				//this.ShopModule.OnPurchaseFailed(PurchaseFailedReason.Abort);
			}
			else
			{
				this.ShopModule.State = ShopActionState.Cancel;
				//this.ShopModule.OnPurchaseFailed(PurchaseFailedReason.Cancel);
			}
		}
	}
}
