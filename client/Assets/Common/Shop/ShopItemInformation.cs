using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities;

public class ShopItemInformation
{
	public void Initialize(Hashtable data)
	{
		this.ProductID = (string)data[ClientStringConstants.JSON_PRODUCT_ID_KEY];
		this.LocaleTitle = (string)data[ClientStringConstants.JSON_PRODUCT_LOCALE_TITLE];
		this.LocaleDescription = (string)data[ClientStringConstants.JSON_PRODUCT_LOCALE_DESCRIPTION];
		this.CurrencySymbol = (string)data[ClientStringConstants.JSON_PRODUCT_LOCALE_CONCURRENCY_SYMBOL];
		this.Price = (string)data[ClientStringConstants.JSON_PRODUCT_LOCALE_PRICE];
		Debug.Log(this.ProductID + "," + this.LocaleTitle + "," + this.LocaleDescription + "," + this.Price + "," + this.CurrencySymbol);
	}
	
	public void Initialize(ProductConfigData configData)
	{
		this.ProductID = configData.ProductID;
		this.LocaleTitle = configData.ProductName;
		this.LocaleDescription = configData.ProductDescription;
		this.Price = configData.Price.ToString();
		this.GemQuantity = configData.GemQuantity;
        this.CurrencySymbol = "¥ ";
		this.IconName = configData.IconName;
	}
	
	public string ProductID { get;set; }
	
	public string LocaleDescription { get;set; }
	public string LocaleTitle { get;set; }
	public string Price { get;set; }
	public string CurrencySymbol { get;set; }
	public int GemQuantity { get;set; }
	public string IconName { get;set; }
}
