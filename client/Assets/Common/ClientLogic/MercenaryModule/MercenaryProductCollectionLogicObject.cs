using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;

public class MercenaryProductCollectionLogicObject 
{
	private Dictionary<MercenaryType, MercenaryProductLogicObject> m_Products;
	
	public MercenaryProductCollectionLogicObject(Dictionary<MercenaryType, MercenaryProductData> data)
	{
		this.m_Products = new Dictionary<MercenaryType, MercenaryProductLogicObject>();
		
		foreach (KeyValuePair<MercenaryType, MercenaryProductData> product in data) 
		{
			this.m_Products.Add(product.Key, new MercenaryProductLogicObject(product.Key, product.Value));
		}
	}
	
	public Dictionary<MercenaryType, MercenaryProductLogicObject> Products { get { return this.m_Products; } }
	
	public void ReloadNewProduct(List<MercenaryType> newTypes)
	{
		List<MercenaryType> deleteType = new List<MercenaryType>();
		foreach (MercenaryType type in this.m_Products.Keys) 
		{
			if(!newTypes.Contains(type))
			{
				deleteType.Add(type);
			}
		}
		
		foreach (MercenaryType type in newTypes) 
		{
			if(!this.m_Products.ContainsKey(type))
			{
				MercenaryConfigData configData = ConfigInterface.Instance.MercenaryConfigHelper.GetMercenaryData(type);
				MercenaryProductData productData = new MercenaryProductData();
				productData.ConfigData = configData;
				productData.ReadyNumber = configData.MaxProduceNumber;
				
				this.m_Products.Add(type, new MercenaryProductLogicObject(type, productData));
			}
		}
		
		foreach (MercenaryType type in deleteType) 
		{
			this.m_Products.Remove(type);
		}
	}
	
	public void HireMercenary(MercenaryType type)
	{
		this.m_Products[type].HireMercenary();
	}
	
	public MercenaryProductLogicObject GetProductLogicObject(MercenaryType type)
	{
		return this.m_Products[type];
	}
}
