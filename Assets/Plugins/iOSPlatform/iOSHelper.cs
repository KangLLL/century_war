using UnityEngine;
using System.Collections;
using System.Security.Cryptography;

public static class iOSHelper
{
    public static string GetDeviceID()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
			byte[] deviceID = System.Text.Encoding.Default.GetBytes(_GetDeviceID());
			byte[] md5ID = MD5.Create().ComputeHash(deviceID);
			
            return System.Convert.ToBase64String(md5ID);
        }
        else
        {
            return "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
        }
    }
	
	public static string GetFileFullPath(string filename)
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return _GetFullPath(filename);
		}
		return null;
	}
	
	public static void SetCopyBoardString(string newString)
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_SetCopyBoard(newString);
		}
	}
	
	public static void ClearApplicationIconBadge()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_ClearApplicationIconBadge();
		}
	}
	
	public static bool IsNativeControllAdded
	{
		get
		{
			if(Application.platform == RuntimePlatform.IPhonePlayer)
			{
				return _GetGameViewChildCount() > 0;
			}
			else
			{
				return false;
			}
		}
	}

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern string _GetDeviceID();
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern string _GetFullPath(string filename);
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
	private static extern void _SetCopyBoard(string newString);
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _ClearApplicationIconBadge();
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
	private static extern int _GetGameViewChildCount();
}