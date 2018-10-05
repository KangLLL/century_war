using UnityEngine;
using System.Collections;

public class DisconnectOKButton : AntiMutiClickButtonBehavior 
{
	[SerializeField]
	private DisconnectDialog m_Dialog;

	protected override void Click ()
	{
		if(this.m_Dialog.DisconnectType == DisconnectType.CannotConnect)
		{
			Application.Quit();
		}
		else
		{
			Application.LoadLevel(ClientStringConstants.LOADING_SCENE_LEVEL_NAME);
		}
	}
}
