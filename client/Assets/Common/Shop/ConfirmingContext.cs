using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConfirmingContext : ShopContext 
{
	private string transactionID;
	private string receipt;
	
	public string TransactionID
	{
		set
		{
			this.transactionID = value;
			this.receipt = StoreKitHelper.GetTransactionReceipt(this.transactionID);
			if(this.receipt == null)
			{
				this.Success(null);
			}
			else
			{
				this.SubmitReceipt();
			}
		}
	}
	
	public string ProductID
	{	
		set
		{
			this.transactionID = StoreKitHelper.GetTransactionID(value, 1);
			if(this.transactionID == null)
			{
				this.Success(null);
			}
			else
			{
				this.receipt = StoreKitHelper.GetTransactionReceipt(this.transactionID);
				if(this.receipt == null)
				{
					this.Success(null);
				}
				else
				{
					this.SubmitReceipt();
				}
			}
		}
	}

	private void SubmitReceipt()
	{
        if (this.ShopModule == null)
        {//由于异步调用 ShopModule 可能还未来得及赋值
            this.ShopModule = ShopUtility.Instance;
        }
		//HttpEvent.Instance.ShopBuyStone(OnSubmitSuccess,this.ShopModule,this.receipt);
	}
	
	/*
	private void OnSubmitSuccess(object sender, ShopBuyStoneArguments arg)
	{
		if(string.IsNullOrEmpty(arg.ResponseBaseInfo1.Error))
		{
			if(arg.ResponseBaseInfo1.ResponseCode.Equals("0"))
			{
				this.Success(StoreKitHelper.ComfirmTransaction(this.transactionID));
			}
			else
			{
				this.Success(StoreKitHelper.ComfirmTransaction(this.transactionID));
			}
		}
		else
		{
			this.Success(null);
		}
	}
	*/
	
	private void Success(string comfirmedProduct)
	{
		this.ShopModule.State = comfirmedProduct == null ? ShopActionState.Idle : ShopActionState.Success;
		if(this.SuccessorContext == null)
		{
			this.ShopModule.ChangeContext(new IdleContext());
		}
		else
		{
			this.ShopModule.State = ShopActionState.Operating;
			FetchContext fetchContext = this.SuccessorContext as FetchContext;
			this.ShopModule.ChangeContext(this.SuccessorContext);
			if(fetchContext != null)
			{
				fetchContext.StartFetch();
			}
		}
		if(comfirmedProduct == null)
		{
			return;
		}
		ShopItemInformation info = new ShopItemInformation();
		//info.Initialize(Json.jsonDecode(comfirmedProduct) as Hashtable);
		this.ShopModule.OnPurchaseSuccessed(info);
	}
}
