using UnityEngine;
using System.Collections;

public class AntiMutiClickButtonBehavior : MonoBehaviour 
{
	private bool m_IsClicked;

	void OnClick()
	{
		if(!this.m_IsClicked)
		{
			this.Click();
			this.m_IsClicked = true;
		}
	}

	void Update()
	{
		this.m_IsClicked = false;
	}

	protected virtual void Click()
	{
	}
}
