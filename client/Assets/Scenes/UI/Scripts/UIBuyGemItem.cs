using UnityEngine;
using System.Collections;

public class UIBuyGemItem : MonoBehaviour,IUIItem {
    [SerializeField] UILabel[] m_UILabel;//0 = title ;1 = description;2 = price;
    [SerializeField] UISprite m_UISprite;//icon
    public ShopItemInformation ShopItemInformation { get; set; }
    public string ProductsIconName { get; set; }
 
    public void SetItemData()
    {
        m_UILabel[0].text = this.ShopItemInformation.LocaleTitle;
        m_UILabel[1].text = this.ShopItemInformation.GemQuantity.ToString();
        m_UILabel[2].text = /*this.ShopItemInformation.CurrencySymbol */"/RMB" + this.ShopItemInformation.Price;
        m_UISprite.spriteName = this.ProductsIconName;
        m_UISprite.MakePixelPerfect();
    }
    void OnClick()
    {
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			LockScreen.Instance.DisableInput();
			NdShopUtility.Instance.PurchaseProduct(this.ShopItemInformation.ProductID);
		}
    }
}
