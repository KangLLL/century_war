using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities;
using ConfigUtilities.Enums;
public class UIBuyTreeItem : MonoBehaviour, IUIItem
{
    [SerializeField] UILabel[] m_UILabel;//0 = title ;1 = price;
    [SerializeField] UISprite m_UISprite;//icon
    public ProductRemovableObjectConfigData ProductRemovableObjectConfigData { get; set; }
    RemovableObjectConfigData m_RemovableObjectConfigData;
    
    public void SetItemData()
    {
         m_RemovableObjectConfigData = ConfigInterface.Instance.RemovableConfigHelper.GetRemovableObjectData(this.ProductRemovableObjectConfigData.RemovableObjectType);
         
         m_UILabel[0].text = m_RemovableObjectConfigData.Name;
         m_UILabel[1].text = this.ProductRemovableObjectConfigData.GemPrice.ToString() + ClientSystemConstants.EXPRESSION_ICON_DICTIONARY[3];
         m_UILabel[1].color = LogicController.Instance.PlayerData.CurrentStoreGem >= this.ProductRemovableObjectConfigData.GemPrice ? Color.white : Color.red;
         m_UISprite.spriteName = this.ProductRemovableObjectConfigData.IconName;
         m_UISprite.MakePixelPerfect();
    }
    public void OnClick()
    {
        if (!this.enabled)
            return;

        if (UIManager.Instance.UIWindowBuyTree.ControlerFocus != null)
            return;
        else
            UIManager.Instance.UIWindowBuyTree.ControlerFocus = gameObject;
        print("UIBuyTreeItem");
        UIManager.Instance.UIWindowBuyTreeChild.ProductRemovableObjectConfigData = this.ProductRemovableObjectConfigData;
        UIManager.Instance.UIWindowBuyTreeChild.RemovableObjectConfigData = this.m_RemovableObjectConfigData;
        UIManager.Instance.UIWindowBuyTreeChild.ShowWindow();
        UIManager.Instance.UIWindowBuyTreeChild.WindowCloseEvent += () => UIManager.Instance.UIWindowBuyTree.ControlerFocus = null;
    }
}
