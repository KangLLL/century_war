using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class Bonjour {

	/* Interface to native implementation */
	/* for NdPlatform
	[DllImport ("__Internal")]
	private static extern void U3dInitializeNdPlatform (int appID, string appKey);
	[DllImport ("__Internal")]
	private static extern int U3dNdLogin (int nFlag);
	[DllImport ("__Internal")]
	private static extern int U3dNdSetDebugMode (int nFlag);
	[DllImport ("__Internal")]
	private static extern int U3dNdCheckUpdate (int nFlag);
	[DllImport ("__Internal")]
	private static extern int U3dNdUniPayForCoin (string cooOrderSerial,int needPayCoins,string payDescription);
	[DllImport ("__Internal")]
	private static extern int U3dNdShowToolBar ();
	[DllImport ("__Internal")]
	private static extern int U3dNdHideToolBar ();
	[DllImport ("__Internal")]
	private static extern void U3dNdSetAutoRotation(bool bFlag);
	[DllImport ("__Internal")]
	private static extern int U3dNdSetScreenOrientation (int orientation);//orientation 0:竖屏 1:横屏朝左 2:竖屏朝下 3:横屏朝右;
	[DllImport ("__Internal")]
	private static extern int U3dNdLogout (int nFlag);//nFlag 标识（按位标识）0,表示注销；1，表示注销，并清除自动登录;
	[DllImport ("__Internal")]
	private static extern bool U3dNdIsLogined ();
	[DllImport ("__Internal")]
	private static extern bool U3dNdEnterPlatform (int nFlag);
	[DllImport ("__Internal")]
	private static extern long U3dNdGetUin();
	[DllImport("__Internal")]
	private static extern int U3dNdUniPay(string cooOrderSerial, string productId, string productName, 
		double productPrice, double productOriginalPrice, int productCount, string payDescription);
	
	// Starts lookup for some bonjour registered service inside specified domain
	
	public static void InitializeNdPlatform(int appID, string appKey)
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			U3dInitializeNdPlatform(appID, appKey);
		}
	}
	
	
	public static void Login()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			U3dNdLogin(0);	
		}
	}
	
	public static void DebugMode()
	{
		if (Application.platform != RuntimePlatform.WindowsEditor&&Application.platform != RuntimePlatform.OSXEditor&&Application.platform != RuntimePlatform.Android)
			U3dNdSetDebugMode(0);			
	}
	public static void PayForCoin(string cooOrderSerial,int needPayCoins,string payDescription)
	{
			U3dNdUniPayForCoin(cooOrderSerial,needPayCoins,payDescription);			
	}
	
	public static void ShowToolBar()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			U3dNdShowToolBar();
		}
	}
	
	public static void HideToolBar()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			U3dNdHideToolBar();
		}
	}
	
	public static void SetAutoRotation(bool isAutoRotate)
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			U3dNdSetAutoRotation(isAutoRotate);
		}
	}
	
	public static void SetScreenOrientation(int orientation)
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			U3dNdSetScreenOrientation(orientation);
		}
	}
	public static void LoginOut()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			U3dNdLogout(1);	
		}
	}
	public static bool IsLogined()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return U3dNdIsLogined();
		}
		else
		{
			return false;
		}
	}
	public static void Enter91()
	{
			U3dNdEnterPlatform(0);
	}
	public static bool CheckUpdate()
	{
		if (Application.platform != RuntimePlatform.WindowsEditor&&Application.platform != RuntimePlatform.OSXEditor&&Application.platform != RuntimePlatform.Android){
			U3dNdCheckUpdate(0);
			return false;
		}else{
			return true;
		}
	}
	public static long GetPlayerID()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return U3dNdGetUin();
		}
		return -1;
	}
	
	public static void UniPay(string cooOrderSerial, string productId, string productName, 
		double productPrice, double productOriginalPrice, int productCount, string payDescription)
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			 U3dNdUniPay(cooOrderSerial, productId, productName, productPrice, productOriginalPrice, productCount, payDescription);
		}
	}
	
	*/
	
	
	public static void InitializeNdPlatform(int appID, string appKey)
	{
	}
	
	public static void Login()
	{
	}
	
	public static void DebugMode()
	{			
	}
	public static void PayForCoin(string cooOrderSerial,int needPayCoins,string payDescription)
	{	
	}
	
	public static void ShowToolBar()
	{
	}
	
	public static void HideToolBar()
	{
	}
	
	public static void SetAutoRotation(bool isAutoRotate)
	{
	}
	
	public static void SetScreenOrientation(int orientation)
	{
	}
	public static void LoginOut()
	{
	}
	public static bool IsLogined()
	{
		return false;
	}
	public static void Enter91()
	{
	}
	
	public static bool CheckUpdate()
	{
		return false;
	}
	public static long GetPlayerID()
	{
		return -1;
	}
	
	public static void UniPay(string cooOrderSerial, string productId, string productName, 
		double productPrice, double productOriginalPrice, int productCount, string payDescription)
	{
	}
}
