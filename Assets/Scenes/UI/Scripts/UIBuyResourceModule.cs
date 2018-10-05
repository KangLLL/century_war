using UnityEngine;
using System.Collections;

public class UIBuyResourceModule :MonoBehaviour, IUIMoudule
{
    [SerializeField] UIBuyResourceItem[] m_UIBuyResourceItem;
    [SerializeField] UIGrid m_UIGrid;

    public void SetModulItem()
    {
        for (int i = 0; i < m_UIBuyResourceItem.Length; i++)
            m_UIBuyResourceItem[i].SetItemData();
        m_UIGrid.sorted = true;
        m_UIGrid.Reposition();
        UIDraggablePanel uiDraggablePanel = NGUITools.FindInParents<UIDraggablePanel>(m_UIGrid.gameObject);

        uiDraggablePanel.transform.localPosition = Vector3.zero;
        UIScrollRegionAdaptive uiScrollRegionAdaptive = NGUITools.FindInParents<UIScrollRegionAdaptive>(m_UIGrid.gameObject);
        uiScrollRegionAdaptive.OnSize();
    }

}
