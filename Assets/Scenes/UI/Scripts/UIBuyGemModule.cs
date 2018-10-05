using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UIBuyGemModule : MonoBehaviour ,IUIMoudule
{
    [SerializeField] UIBuyGemItem m_UIBuyGemItem;
    [SerializeField] UIGrid m_UIGrid;
	// Use this for initialization
	void Start () {
	
	}
	
	public void DestroyItems()
	{
		while (m_UIGrid.transform.childCount > 0)
        {
            Transform trans = m_UIGrid.transform.GetChild(0);
            trans.parent = null;
            DestroyImmediate(trans.gameObject);
        }
	}
	
    public void SetModulItem()
    {
		this.DestroyItems();
        
        List<ShopItemInformation> sii = CommonHelper.PlatformType == ConfigUtilities.Enums.PlatformType.Nd ?
			NdShopUtility.Instance.ShopItems : iOSShopUtility.Instance.ShopItems;
        
        for (int i = 0; i < sii.Count; i++)
        {
            UIBuyGemItem uiBuyGemItem = (Instantiate(m_UIBuyGemItem.gameObject) as GameObject).GetComponent<UIBuyGemItem>() as UIBuyGemItem;
            uiBuyGemItem.transform.parent = m_UIGrid.transform;
            uiBuyGemItem.transform.localPosition = Vector3.zero;
            uiBuyGemItem.ShopItemInformation = sii[i];
            uiBuyGemItem.ProductsIconName = sii[i].IconName;
            uiBuyGemItem.name = uiBuyGemItem.name + i;
            uiBuyGemItem.SetItemData();
        }
        m_UIGrid.sorted = true;
        m_UIGrid.Reposition();
        UIDraggablePanel uiDraggablePanel = NGUITools.FindInParents<UIDraggablePanel>(m_UIGrid.gameObject);
        //uiDraggablePanel.ResetPosition();
        uiDraggablePanel.transform.localPosition = Vector3.zero;
        UIScrollRegionAdaptive uiScrollRegionAdaptive = NGUITools.FindInParents<UIScrollRegionAdaptive>(m_UIGrid.gameObject);
        uiScrollRegionAdaptive.OnSize();
    }
 
}
