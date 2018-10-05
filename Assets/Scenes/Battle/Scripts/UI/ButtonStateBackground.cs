using UnityEngine;
using System.Collections;

public enum ButtonState
{
	Normal,
	Disable
}

public class ButtonStateBackground : MonoBehaviour 
{
	[SerializeField]
	private ButtonState m_InitialState;
	[SerializeField]
	private UISprite m_NormalSprite;
	[SerializeField]
	private UISprite m_DisableSprite;
	
	void Start()
	{
		switch(this.m_InitialState)
		{
			case ButtonState.Normal :
			{
				this.SetNormalSprite();
			}
			break;
		case ButtonState.Disable:
			{
				this.SetDisableSprite();
			}
			break;
		}
	}
	
	public void SetNormalSprite()
	{
		if(this.m_NormalSprite.enabled)
		{
			this.m_NormalSprite.enabled = true;
		}
		if(this.m_DisableSprite.enabled)
		{
			this.m_DisableSprite.enabled = false;
		}
	}
	
	public void SetDisableSprite()
	{
		if(this.m_NormalSprite.enabled)
		{
			this.m_NormalSprite.enabled = false;
		}
		if(this.m_DisableSprite)
		{
			this.m_DisableSprite.enabled = true;
		}
	}
}
