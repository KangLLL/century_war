using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommonUtilities;
using ConfigUtilities;

public class iOSRequestContext : ShopContext 
{
	public void StartRequest(List<string> productsId, bool isNeedRefresh)
	{	
		StoreKitHelper.RequestProducts(productsId, isNeedRefresh);
	}
	
	public override void Execute ()
	{
		if(StoreKitHelper.GetRequestState() == RequestProductState.RequestSuccess)
		{
			List<ShopItemInformation> products = this.ConstructShopItemInformation();
			if(products == null)
			{
				this.OnRequestFail();
			}
			else
			{
				if(this.SuccessorContext != null)
				{
					this.ShopModule.ChangeContext(this.SuccessorContext);
				}
				else
				{
					this.ShopModule.ChangeContext(new iOSIdleContext());
					this.ShopModule.State = ShopActionState.Idle;
				}
				this.ShopModule.OnRequestSuccessed(products);
			}
		}
		else if(StoreKitHelper.GetRequestState() == RequestProductState.RequestFail)
		{
			this.OnRequestFail();
		}
	}
	
	private void OnRequestFail()
	{
		this.ShopModule.State = ShopActionState.Fail;
		iOSIdleContext idleContext = new iOSIdleContext();
		this.ShopModule.ChangeContext(idleContext);
		this.ShopModule.OnRequestFailed(ClientStringConstants.REQUEST_FAIL_TIPS);
	}
	
	private List<ShopItemInformation> ConstructShopItemInformation()
	{
		string products = StoreKitHelper.GetValidProducts();
		Debug.Log(products);
		
		if(products == null)
		{
			return null;
		}
		
		ArrayList productList =  JsonUtility.jsonDecode(products) as ArrayList;
		if(productList == null)
		{
			return null;
		}
		
		List<ShopItemInformation> temp  = new List<ShopItemInformation>();
		foreach(object product in productList)
		{
			Hashtable info = product as Hashtable;
			if(info == null)
			{
				return null;
			}
			ShopItemInformation itemInformation = new ShopItemInformation();
			itemInformation.Initialize(info);
			temp.Add(itemInformation);
 		}
		
		List<ShopItemInformation> result = new List<ShopItemInformation>();
		List<ProductConfigData> productsData = ((iOSShopUtility)this.ShopModule).ProductsData;
		
		foreach (ProductConfigData p in productsData) 
		{
			foreach (ShopItemInformation info in temp) 
			{
				if(p.ProductID.Equals(info.ProductID))
				{
					info.GemQuantity = p.GemQuantity;
					info.IconName = p.IconName;
					result.Add(info);
					break;
				}
			}
		}
		
		return result;
	}
}
