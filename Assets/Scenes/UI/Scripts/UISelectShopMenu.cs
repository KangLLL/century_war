using UnityEngine;
using System.Collections;

public class UISelectShopMenu : MonoBehaviour
{
    [SerializeField] UIShopMenuType m_UIShopMenuType;
    [SerializeField]
    //UIWindowCommon m_Parent;
    public void OnClick()
    {
        if (!this.enabled)
            return;
        if (UIManager.Instance.UIWindowShop.ControlerFocus != null)
            return;
        else
            UIManager.Instance.UIWindowShop.ControlerFocus = this.gameObject;

        UIManager.Instance.UIWindowShop.HideWindow();

        switch (this.m_UIShopMenuType)
        {
            case UIShopMenuType.Gem:
                //UIManager.Instance.UIWindowBuyGem.ShowWindow();
                this.GoShopping();
                break;
            case UIShopMenuType.Tree:
                UIManager.Instance.UIWindowBuyTree.ShowWindow();
                break;
            case UIShopMenuType.Resource:
                UIManager.Instance.UIWindowBuyResource.ShowWindow();
                break;
        } 
    }
	
    public void GoShopping()
    {
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			 UIManager.Instance.UIWindowBuyGem.ShowWindow();
		}
    }
}
