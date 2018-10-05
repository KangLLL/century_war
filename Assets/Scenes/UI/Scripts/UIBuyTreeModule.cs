using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities;
public class UIBuyTreeModule : MonoBehaviour, IUIMoudule
{
    [SerializeField] UIBuyTreeItem m_UIBuyTreeItem;
    [SerializeField] UIGrid m_UIGrid;
    public UIBuyTreeItem UIBuyTreeItem { get; set; }//first Item for newbie guide
    public void SetModulItem()
    { 
        while (m_UIGrid.transform.childCount > 0)
        {
            Transform trans = m_UIGrid.transform.GetChild(0);
            trans.parent = null;
            DestroyImmediate(trans.gameObject);
        }
        List<ProductRemovableObjectConfigData> procs = ConfigInterface.Instance.ProductConfigHelper.GetProductRemovableObjects();

        for (int i = 0; i < procs.Count; i++)
        {
            UIBuyTreeItem uiBuyTreeItem = (Instantiate(m_UIBuyTreeItem.gameObject) as GameObject).GetComponent<UIBuyTreeItem>() as UIBuyTreeItem;
            uiBuyTreeItem.transform.parent = m_UIGrid.transform;
            uiBuyTreeItem.transform.localPosition = Vector3.zero;
            uiBuyTreeItem.ProductRemovableObjectConfigData = procs[i];
            string order = (10000 + i).ToString();//i < 10 ? "0" + i : i.ToString();
            uiBuyTreeItem.name = order + "_" + uiBuyTreeItem.name;
            uiBuyTreeItem.SetItemData();
            if (i == 0)
                this.UIBuyTreeItem = uiBuyTreeItem;
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
