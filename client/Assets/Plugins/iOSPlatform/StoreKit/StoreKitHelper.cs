using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class StoreKitHelper 
{
	public static void RequestProducts(List<string> products, bool isNeedRefresh)
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			string identifiers = string.Empty;
			foreach (string product in products) 
			{
				if(!identifiers.Equals(string.Empty))
				{
					identifiers += " ";
				}
				identifiers += product;
			}
			_RequestProducts(identifiers, isNeedRefresh);
		}
	}
	
	public static RequestProductState GetRequestState()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return (RequestProductState)_GetRequestState();
		}
		return RequestProductState.RequestFail;
	}
	
	public static string GetUnconfirmedTransactionID()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return _GetUnconfirmedTransactionID();
		}
		return null;
	}
	
	public static string GetUnconfirmedProductID()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return _GetUnconfirmedProductID();
		}
		return null;
	}
	
	public static bool IsProductAvailable(string productID)
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return _IsProductAvailable(productID);
		}
		return false;
	}
	
	public static bool CanMakePayments()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return _CanMakePayments();
		}
		return false;
	}
	
	public static void PurchaseProduct(string productID, int quantity)
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_PurchaseProduct(productID, quantity);
		}
	}
	
	public static PurchaseState GetPurchaseState()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return (PurchaseState)_GetPurchaseState();
		}
		return PurchaseState.PurchaseFail;
	}

	public static string GetTransactionID(string productID, int quantity)
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return _GetTransactionID(productID, quantity);
		}
		return null;
	}
	
	public static string GetTransactionReceipt(string transactionID)
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return _GetTransactionReceipt(transactionID);
		}
		return null;
	}
	
	public static int ValidateTransactionReceipt(string transactionID, string receipt, bool isTest)
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return _ValidateTransactionReceipt(transactionID, receipt, isTest);
		}
		return 0;
	}
	
	public static ValidationState GetTransactionValidationState(int validationID)
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return (ValidationState)_GetTransactionValidationState(validationID);
		}
		return ValidationState.ValidateFail;
	}
	
	public static string ComfirmTransaction(string transactionID)
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return _ComfirmTransaction(transactionID);
		}
		return null;
	}
	
	public static string GetValidProducts()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return _GetValidProductsInformation();
		}
		return null;
	}
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
	private static extern bool _CanMakePayments();
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _PurchaseProduct(string productID, int quantity);
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
	private static extern int _GetPurchaseState();
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern string _GetTransactionID(string productID, int quantity);
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern string _GetTransactionReceipt(string transactionID);
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern int _ValidateTransactionReceipt(string transactionID, string receipt, bool isTest);
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern int _GetTransactionValidationState(int validationID);
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern string _ComfirmTransaction(string transactionID);
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _RequestProducts(string products, bool isNeedRefresh);
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern int  _GetRequestState();
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern string _GetUnconfirmedTransactionID();
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
	private static extern string _GetUnconfirmedProductID();
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern bool _IsProductAvailable(string productID);
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern string _GetValidProductsInformation();
}
