using UnityEngine;
using System.Collections;

public class ClientNotification : MonoBehaviour 
{	
	private bool m_TokenSent = false;
	private bool m_SendToken = false;
	
	private static ClientNotification s_Sigleton;
	
	public static ClientNotification Instance
	{
		get { return s_Sigleton; }
	}
	
	void Awake()
	{
		s_Sigleton = this;
		this.DontDestroyOnLoad(this.gameObject);
	}
	
	void Start () 
	{
        this.OnRegistRemoteNotification();
	}
	
	void OnDestroy()
	{
		s_Sigleton = null;
	}
	
	public void SendToken()
	{
		this.m_SendToken = true;
	}
	
	// Update is called once per frame
	void Update () 
    {
        this.OnWaitingForDeviceToken();
	}
    void OnRegistRemoteNotification()
    {
        NotificationServices.RegisterForRemoteNotificationTypes(RemoteNotificationType.Alert | RemoteNotificationType.Badge | RemoteNotificationType.Sound);
    }
    void OnWaitingForDeviceToken()
    {
		if(this.m_SendToken)
		{
			if(CommunicationUtility.Instance.IsConnectedToServer)
			{
		        if (!this.m_TokenSent)
		        {
		            byte[] token = NotificationServices.deviceToken;
		            if (token != null)
		            {
		                // send token to a provider
		                string hexToken = System.BitConverter.ToString(token).Replace("-","");
		                print("hexToken =" + hexToken);
		                CommunicationUtility.Instance.SetDeviceToken(new CommandConsts.DeviceTokenRequestParameter() { DeviceToken = hexToken });
		                this.m_TokenSent = true;
						this.m_SendToken = false;
		            }
		        }
			}
	    }
	}
}
