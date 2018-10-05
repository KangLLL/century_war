using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities;
using ConfigUtilities.Enums;
using CommandConsts;

public class iOSShopUtility : ShopUtility 
{
	private List<ProductConfigData> m_ProductsData;
	
	public List<ProductConfigData> ProductsData 
	{ 
		get 
		{ 
			return this.m_ProductsData;
		} 
	}
	
	private bool m_IsReceivedConfirmResponse;
	private string m_CurrentConfirmError;
	
	public bool IsReceivedConfirmResponse
	{
		get
		{
			if(this.m_IsReceivedConfirmResponse)
			{
				this.m_IsReceivedConfirmResponse = false;
				return true;
			}
			return false;
		}
	}
	
	public string CurrentConfirmError
	{
		get
		{
			return this.m_CurrentConfirmError;
		}
	}
	
	
	public override bool CanMakePayment 
	{
		get 
		{
			return StoreKitHelper.CanMakePayments();
		}
	}
	
	public override void Initialize ()
	{
		Debug.Log("begin...!");
	
		string unconfirmedProductID = StoreKitHelper.GetUnconfirmedProductID();
		//this.m_IsRequestProducts = false;
		
		if(unconfirmedProductID != null)
		{
			iOSConfirmContext confirmContext = new iOSConfirmContext();
			this.ChangeContext(confirmContext);
			confirmContext.StartProductConfirm(unconfirmedProductID);
			this.State = ShopActionState.Operating;
		}
		else
		{
			this.ChangeContext(new iOSIdleContext());
			this.State = ShopActionState.Idle;
		}
	}
	
	public void RequestProduct()
	{
		if(this.ShopItems == null)
		{
			if(this.m_ProductsData == null)
			{
				this.InitialProductsID();
			}
			
			iOSRequestContext requestContext = new iOSRequestContext();
			this.ChangeContext(requestContext);
			List<string> ids = new List<string>();
			foreach (ProductConfigData p in m_ProductsData)
			{
				ids.Add(p.ProductID);
			}
			requestContext.StartRequest(ids, true);
			this.State = ShopActionState.Operating;
		}
	}
	
	public override void PurchaseProduct (string productID)
	{
		iOSIdleContext idleContext = this.m_CurrentContext as iOSIdleContext;
		if(idleContext != null)
		{
			Debug.Log("real start!");
			this.State = ShopActionState.Operating;
			idleContext.PurchaseProduct(productID);
		}
	}
	
	private void InitialProductsID()
	{
		this.m_ProductsData = ConfigInterface.Instance.ProductConfigHelper.GetProducts(PlatformType.iOS);
	}
	
	private void ReceivedConfirmResponse(Hashtable result)
	{
		if(!string.IsNullOrEmpty(this.m_CurrentConfirmError))
		{
			this.m_CurrentConfirmError = null;
		}
		ConfirmApplePurchaseResponseParameter param = new ConfirmApplePurchaseResponseParameter();
		param.InitialParameterObjectFromHashtable(result);
		if(!string.IsNullOrEmpty(param.ErrorString))
		{
			this.m_CurrentConfirmError = param.ErrorString;
		}
		this.m_IsReceivedConfirmResponse = true;
	}
}
