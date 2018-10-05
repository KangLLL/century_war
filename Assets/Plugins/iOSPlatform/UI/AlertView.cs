using UnityEngine;
using System.Collections;

public class AlertView 
{
	public static bool AlertViewIsShown
	{
		get
		{	
			if(Application.platform == RuntimePlatform.IPhonePlayer)
			{
				return _AlertViewIsShown();
			}
			return false;
		}
	}
	
	public static void ShowAlertView(string title, string description, string cancelTitle)
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			_ShowAlertView(title,description,cancelTitle);
		}
	}
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern bool _AlertViewIsShown();
	[System.Runtime.InteropServices.DllImport("__Internal")]
	private static extern void _ShowAlertView(string title, string description, string cancelTitle);
}
