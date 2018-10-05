using UnityEngine;
using System.Collections;
using CommandConsts;

public class iOSConfirmContext : ShopContext 
{
	private string m_ProductID;
	private string m_TransactionID;
	private string m_Receipt;
	
	private const string NO_RECEIPT_ERROR_DESCRIPTION = "no receipt";
	
	public void StartProductConfirm(string productID)
	{
		this.m_ProductID = productID;
		this.m_TransactionID = StoreKitHelper.GetTransactionID(productID, 1);
		if(string.IsNullOrEmpty(this.m_TransactionID))
		{
			this.Fail();
		}
		else
		{
			this.StartTransactionConfirm(this.m_TransactionID);
		}
	}
	
	private void StartTransactionConfirm(string transactionID)
	{
		this.m_TransactionID = transactionID;
		
		this.m_Receipt = StoreKitHelper.GetTransactionReceipt(this.m_TransactionID);
		if(string.IsNullOrEmpty(this.m_Receipt))
		{
			this.Fail();
		}
		else
		{
			this.SubmitReceipt();
		}
	}

	private void SubmitReceipt()
	{
		ConfirmApplePurchaseRequestParameter request = new ConfirmApplePurchaseRequestParameter();
		request.Receipt = this.m_Receipt;
		CommunicationUtility.Instance.ConfirmApplePurchase(request, this.ShopModule, "ReceivedConfirmResponse", true);
	}
	
	private void Fail()
	{
		this.ShopModule.State = ShopActionState.Fail;
		PurchaseFailInformation failInformation = new PurchaseFailInformation();
		failInformation.Reason = PurchaseFailedReason.ComfirmFail;
		failInformation.ErrorDescription = NO_RECEIPT_ERROR_DESCRIPTION;
		
		this.ShopModule.OnPurchaseFailed(failInformation);
		this.ChangeToNextContext(false);
	}
	
	private void ChangeToNextContext(bool isSuccessful)
	{
		this.ShopModule.State = isSuccessful ? ShopActionState.Success : ShopActionState.Fail;
		this.ShopModule.ChangeContext(new iOSIdleContext());
		
		StoreKitHelper.ComfirmTransaction(this.m_TransactionID);
	}
	
	public override void Execute ()
	{
		iOSShopUtility shopUtility = (iOSShopUtility)this.ShopModule;
		if(shopUtility.IsReceivedConfirmResponse)
		{
		    if(string.IsNullOrEmpty(shopUtility.CurrentConfirmError))
			{		
				this.ChangeToNextContext(true);
				foreach (ShopItemInformation product in this.ShopModule.ShopItems) 
				{
					if(this.m_ProductID.Equals(product.ProductID))
					{
						this.ShopModule.OnPurchaseSuccessed(product);
						break;
					}
				}
			}
			else
			{
				this.ChangeToNextContext(false);
				PurchaseFailInformation failInformation = new PurchaseFailInformation();
				failInformation.Reason = PurchaseFailedReason.ComfirmFail;
				failInformation.ErrorDescription = shopUtility.CurrentConfirmError;
				this.ShopModule.OnPurchaseFailed(failInformation);
			}
		}
	}
}
