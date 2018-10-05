using UnityEngine;
using System.Collections;

public class LockScreen : MonoBehaviour {

    private static LockScreen s_Sigleton;

    public static LockScreen Instance
    {
        get
        {
            return s_Sigleton;
        }
    }

    void Awake()
    {
        s_Sigleton = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void EnableInput()
    {
        if (Application.platform != RuntimePlatform.IPhonePlayer &&
            Application.platform != RuntimePlatform.Android)
        {
            UICamera.eventHandler.useMouse = true;
        }
        UICamera.eventHandler.useTouch = true;
    }

    public void DisableInput()
    {
        if (Application.platform != RuntimePlatform.IPhonePlayer &&
            Application.platform != RuntimePlatform.Android)
        {
            UICamera.eventHandler.useMouse = false;
        }
        UICamera.eventHandler.useTouch = false;
    }
	
	public bool Inputable
	{
		get 
		{ 
			
	        if (Application.platform != RuntimePlatform.IPhonePlayer &&
	            Application.platform != RuntimePlatform.Android)
	        {
	            return UICamera.eventHandler.useMouse;
	        }
			return UICamera.eventHandler.useTouch;
		}
	}
}
