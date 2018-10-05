using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using CommandConsts;
using ConfigUtilities;
using ConfigUtilities.Enums;

public class NdShopUtility : ShopUtility 
{
	public const string UNCONFIRMED_PRODUCT_ID_KEY = "UnconfirmedProduct";
	public const string UNCONFIRMED_PURCHASE_ID_KEY = "UnconfirmedPurchase";
	
	private string m_CurrentPurchaseID;
	
	private bool m_IsReceivedConfirmResponse;
	private string m_CurrentConfirmError;
	
	private Dictionary<string, ShopItemInformation> m_ProductsDict;
	
	public Dictionary<string, ShopItemInformation> ProductsDict
	{
		get { return this.m_ProductsDict; }
	}
	
	public string CurrentPurchaseID
	{
		get 
		{
			if(string.IsNullOrEmpty(this.m_CurrentPurchaseID))
			{
				return null;
			}
			else
			{
				string result = this.m_CurrentPurchaseID;
				this.m_CurrentPurchaseID = null;
				return result;
			}
		}
	}
	
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
			return Bonjour.IsLogined();
		}
	}
	
	public override void Initialize ()
	{
		if(this.m_ProductsDict == null) 
		{
			this.InitialProductItems();
		}
		
		string unconfirmedPurchaseID = PlayerPrefs.GetString(UNCONFIRMED_PURCHASE_ID_KEY, string.Empty);
		
		if(string.IsNullOrEmpty(unconfirmedPurchaseID))
		{
			string unconfirmedProductID = PlayerPrefs.GetString(UNCONFIRMED_PRODUCT_ID_KEY, string.Empty);
			if(string.IsNullOrEmpty(unconfirmedProductID))
			{
				NdIdleContext idleContext = new NdIdleContext();
				this.ChangeContext(idleContext);
				this.State = ShopActionState.Idle;
			}
			else
			{
				this.m_CurrentPurchaseID = unconfirmedProductID;
				NdRequestPurchaseIDContext requestContext = new NdRequestPurchaseIDContext();
				this.ChangeContext(requestContext);
				this.State = ShopActionState.Operating;
			}
		}
		else
		{
			NdConfirmContext confirmContext = new NdConfirmContext();
			this.ChangeContext(confirmContext);
			this.State = ShopActionState.Operating;
		}
	}
	
	public override void PurchaseProduct (string productID)
	{
		Debug.Log(productID);
		Debug.Log(this.m_CurrentContext);
		
		NdIdleContext idleContext = this.m_CurrentContext as NdIdleContext;
		if(idleContext != null)
		{
			Debug.Log("real start!");
			this.State = ShopActionState.Operating;
			idleContext.PurchaseProduct(productID);
		}
	}
	
	private void InitialProductItems()
	{
		this.m_ProductsDict = new Dictionary<string, ShopItemInformation>();
		List<ProductConfigData> products = ConfigInterface.Instance.ProductConfigHelper.GetProducts(PlatformType.Nd);

		List<ShopItemInformation> result = new List<ShopItemInformation>();
		for(int i = 0; i < products.Count; i ++)
		{
			ShopItemInformation info = new ShopItemInformation();
			info.Initialize(products[i]);
			
			this.m_ProductsDict.Add(info.ProductID, info);
			result.Add(info);
		}
		this.OnRequestSuccessed(result);
	}
	
	private void ReceivedPurchaseID(Hashtable result)
	{
		GeneratePurchaseResponseParameter param = new GeneratePurchaseResponseParameter();
		param.InitialParameterObjectFromHashtable(result);
		this.m_CurrentPurchaseID = param.PurchaseID;
	}
	
	private void ReceivedConfirmResponse(Hashtable result)
	{
		if(!string.IsNullOrEmpty(this.m_CurrentConfirmError))
		{
			this.m_CurrentConfirmError = null;
		}
		ConfirmNdPurchaseResponseParameter param = new ConfirmNdPurchaseResponseParameter();
		param.InitialParameterObjectFromHashtable(result);
		if(!string.IsNullOrEmpty(param.ErrorString))
		{
			this.m_CurrentConfirmError = param.ErrorString;
		}
		this.m_IsReceivedConfirmResponse = true;
	}
}
