using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RequestingContext : ShopContext 
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
					this.ShopModule.State = ShopActionState.Idle;
					IdleContext idleContext = new IdleContext();
					this.ShopModule.ChangeContext(idleContext);
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
		IdleContext idleContext = new IdleContext();
		this.ShopModule.ChangeContext(idleContext);
		this.ShopModule.OnRequestFailed(string.Empty);
	}
	
	private List<ShopItemInformation> ConstructShopItemInformation()
	{
		string products = StoreKitHelper.GetValidProducts();
		Debug.Log(products);
		
		if(products == null)
		{
			return null;
		}
		
		ArrayList productList = new ArrayList();//= Json.jsonDecode(products) as ArrayList;
		if(productList == null)
		{
			return null;
		}
		
		List<ShopItemInformation> results  = new List<ShopItemInformation>();
		foreach(object product in productList)
		{
			Hashtable info = product as Hashtable;
			if(info == null)
			{
				return null;
			}
			ShopItemInformation itemInformation = new ShopItemInformation();
			itemInformation.Initialize(info);
			results.Add(itemInformation);
 		}
		
		/*
		results.Sort((x, y) => 
					{
						if(float.Parse(x.Price) < float.Parse(y.Price))
						{
							return -1;
						}
						else if(float.Parse(x.Price) > float.Parse(y.Price))
						{
							return 1;
						}
						return 0;
					});
		*/
		return results;
	}
}
