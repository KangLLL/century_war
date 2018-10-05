using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum ShopActionState
{
	Idle,
	Operating,
	Success,
	Fail,
	Cancel
}

public class ShopUtility : MonoBehaviour 
{
	private ReceiverManager m_RequestFailedListeners;
	private ReceiverManager m_RequestSuccessedListeners;
	private ReceiverManager m_PurchaseFailedListeners;
	private ReceiverManager m_PurchaseSuccessedListeners;
	private ReceiverManager m_ContextChangeListeners;
	
	protected ShopContext m_CurrentContext;
	private List<ShopItemInformation> m_ShopItems;
	
	private ShopItemInformation m_PurchasedItem;
	
    public ShopActionState State
	{
		get;set;
	}
	
	public ShopItemInformation PurchasedItem
	{
		get { return this.m_PurchasedItem; }
	}
	
	public virtual bool CanMakePayment
	{
		get { return false; }
		/*
		get { return StoreKitHelper.CanMakePayments(); }
		*/
	}
	
	public virtual string UnconfirmedTransaction
	{
		get { return null; }
	}

    private static ShopUtility s_Singleton;
    
	public static ShopUtility Instance
    {
		get { return s_Singleton; }
    }
	
	public void RegisterRequestFailedListener(Component comp, string methodName)
	{
		ReceiverInformation receiverInfo = new ReceiverInformation() 
		{ Receiver = comp, MethodName = methodName, IsListenOnce = false };
		this.m_RequestFailedListeners.AddReceiver(receiverInfo);
	}
	
	public void RegisterRequestSuccessedListener(Component comp, string methodName)
	{
		ReceiverInformation receiverInfo = new ReceiverInformation() 
		{ Receiver = comp, MethodName = methodName, IsListenOnce = false };
		this.m_RequestSuccessedListeners.AddReceiver(receiverInfo);
	}
	
	public void RegisterPurchaseFailedListener(Component comp, string methodName)
	{
		ReceiverInformation receiverInfo = new ReceiverInformation() 
		{ Receiver = comp, MethodName = methodName, IsListenOnce = false };
		this.m_PurchaseFailedListeners.AddReceiver(receiverInfo);
	}
	
	public void RegisterPurchaseSuccessedListener(Component comp, string methodName)
	{
		ReceiverInformation receiverInfo = new ReceiverInformation() 
		{ Receiver = comp, MethodName = methodName, IsListenOnce = false };
		this.m_PurchaseSuccessedListeners.AddReceiver(receiverInfo);
	}
	
	public void RegisterStateChangeListener(Component comp, string methodName)
	{
		ReceiverInformation receiverInfo = new ReceiverInformation()
		{ Receiver = comp, MethodName = methodName, IsListenOnce = false };
		this.m_ContextChangeListeners.AddReceiver(receiverInfo);
	}
	
	public void OnRequestFailed(string errorString)
	{
		this.m_RequestFailedListeners.Invoke(errorString);
	}
	
	public void OnRequestSuccessed(List<ShopItemInformation> shopItems)
	{
		this.m_ShopItems = shopItems;
		foreach(ShopItemInformation info in this.m_ShopItems)
		{
			Debug.Log(string.Format("product id is : {0},description is :{1},price is :{2}",info.ProductID,info.LocaleDescription,info.Price));
		}
		this.m_RequestSuccessedListeners.Invoke(shopItems);
	}
	
	public void OnPurchaseFailed(PurchaseFailInformation failInformation)
	{
		this.m_PurchaseFailedListeners.Invoke(failInformation);
	}
	
	public void OnPurchaseSuccessed(ShopItemInformation shopItem)
	{
		this.m_PurchaseSuccessedListeners.Invoke(shopItem);
		this.m_PurchasedItem = shopItem;
	}
	
	public void RemoveInvalidHandlers()
	{
		this.m_RequestFailedListeners.RemoveInvalidReceiver();
		this.m_RequestSuccessedListeners.RemoveInvalidReceiver();
		this.m_PurchaseFailedListeners.RemoveInvalidReceiver();
		this.m_PurchaseSuccessedListeners.RemoveInvalidReceiver();
		this.m_ContextChangeListeners.RemoveInvalidReceiver();
	}
	
    void Awake()
    {
        GameObject.DontDestroyOnLoad(this);
		s_Singleton = this;
		this.m_RequestFailedListeners = new ReceiverManager();
		this.m_RequestSuccessedListeners = new ReceiverManager();
		this.m_PurchaseFailedListeners = new ReceiverManager();
		this.m_PurchaseSuccessedListeners = new ReceiverManager();
		this.m_ContextChangeListeners = new ReceiverManager();
    }
	
	protected virtual void Start()
	{
	}
	
	void Update () 
	{
		if(this.m_CurrentContext != null)
		{
			this.m_CurrentContext.Execute();
		}
	}
	
	public void ChangeContext(ShopContext context)
	{
		if(this.m_CurrentContext != null)
		{
			Debug.Log("from context : " + this.m_CurrentContext.GetType().ToString() + " to context: " + context.GetType().ToString());
		}
		else
		{
			Debug.Log("to context :" + context.GetType().ToString());
		}
		
		ContextChangeParameter param = new ContextChangeParameter() { FromContext = this.m_CurrentContext, ToContext = context };
		
		this.m_CurrentContext = context;
		//Debug.Log(context.GetType().ToString());
		context.ShopModule = this;
		this.m_ContextChangeListeners.Invoke(param);
	}
	
	public virtual void Initialize()
	{
	}
	
	public virtual void PurchaseProduct(string productID)
	{	
	}
	
	public List<ShopItemInformation> ShopItems
	{
		get
		{
			return this.m_ShopItems;
		}
	}
	
	public void ClearProducts()
	{
		this.m_ShopItems = null;
	}
}