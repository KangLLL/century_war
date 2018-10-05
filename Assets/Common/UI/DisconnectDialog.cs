using UnityEngine;
using System.Collections;

public enum DisconnectType
{
	Normal,
	BackFromBackground,
	CannotConnect
}


public class DisconnectDialog : MonoBehaviour 
{	
	[SerializeField]
	private UILabel m_Label;

	public DisconnectType DisconnectType { get;set; }

	// Use this for initialization
	void Start () 
	{
		Time.timeScale = 0.0f;
		LockScreen.Instance.EnableInput();

		switch(this.DisconnectType)
		{
			case DisconnectType.Normal:
			{
				this.m_Label.text = ClientStringConstants.DISCONNECT_DIALOG_NORMAL_TIPS;
			}
			break;
			case DisconnectType.BackFromBackground:
			{
				this.m_Label.text = ClientStringConstants.DISCONNECT_DIALOG_FROM_BACKGROUND_TIPS;
			}
			break;
			case DisconnectType.CannotConnect:
			{
				this.m_Label.text = ClientStringConstants.DISCONNECT_DIALOG_CAN_NOT_CONNECT_TIPS;
			}
			break;
		}
	}
}
