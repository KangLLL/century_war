using UnityEngine;
using System.Collections;

public class UIButtonShopping : MonoBehaviour 
{
	private bool m_IsLogin;
	// Use this for initialization
	void Start () 
	{
	}

    void OnClick()
    {
        this.GoShopping();
    }
  
    public void GoShopping()
    {
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			 UIManager.Instance.UIWindowBuyGem.ShowWindow();
		}
    }

 
}
