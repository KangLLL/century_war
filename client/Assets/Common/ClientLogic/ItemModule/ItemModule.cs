using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;

public class ItemModule
{
	private Dictionary<ItemType, Dictionary<int ,ItemLogicObject>> m_Items;
	private Dictionary<ItemType, ObjectUpgrade<ItemType>> m_Upgrades;
	
	public ItemModule()
	{
		this.m_Items = new Dictionary<ItemType, Dictionary<int, ItemLogicObject>>();
		this.m_Upgrades = new Dictionary<ItemType, ObjectUpgrade<ItemType>>();
	}
	
	public void InitializeItem(List<ItemData> items, List<ObjectUpgrade<ItemType>> upgrades)
	{
		foreach (ItemData item in items) 
		{
			if(!this.m_Items.ContainsKey(item.ItemID.itemType))
			{
				this.m_Items.Add(item.ItemID.itemType, new Dictionary<int, ItemLogicObject>());
			}
			
			this.m_Items[item.ItemID.itemType].Add(item.ItemID.itemNO, new ItemLogicObject(item));
		}
		
		foreach (ObjectUpgrade<ItemType> upgrade in upgrades) 
		{
			this.m_Upgrades.Add(upgrade.Identity, upgrade);
		}
	}
	
	public ItemIdentity ProduceItem(ItemType type, int level, int NO)
	{
		ItemData itemData = new ItemData();
		itemData.ConfigData = null;//ConfigInterface.Instance.ItemConfigHelper.GetItemData(type, level);
		itemData.ItemID = new ItemIdentity(type, NO);
		itemData.ProduceRemainingWorkload = itemData.ConfigData.ProduceWorkload;
		if(!this.m_Items.ContainsKey(itemData.ItemID.itemType))
		{
			this.m_Items.Add(itemData.ItemID.itemType, new Dictionary<int, ItemLogicObject>());
		}
		
		this.m_Items[itemData.ItemID.itemType].Add(itemData.ItemID.itemNO, new ItemLogicObject(itemData));
		return itemData.ItemID;
	}
	
	public void UpgradeItem(ItemType type, int currentLevel)
	{
		int workload = 1;//ConfigInterface.Instance.ItemConfigHelper.GetUpgradeWorkload(type, currentLevel);
		ObjectUpgrade<ItemType> upgrade = new ObjectUpgrade<ItemType>(type, workload);
		this.m_Upgrades.Add(type, upgrade);
	}
	
	public void FinishUpgrade(ItemType type)
	{
		this.m_Upgrades.Remove(type);
	}
	
	public ItemLogicObject GetItemObject(ItemIdentity id)
	{
		return this.m_Items[id.itemType][id.itemNO];
	}
	
	public ObjectUpgrade<ItemType> GetUpgrade(ItemType type)
	{
		return this.m_Upgrades[type];
	}
}
