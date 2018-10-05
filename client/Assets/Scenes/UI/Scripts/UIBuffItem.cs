using UnityEngine;
using System.Collections;

public class UIBuffItem : MonoBehaviour
{
    [SerializeField] UISprite m_UISprite;
    public BuffLogicData BuffLogicData { get; set; }
    public PropsLogicData PropsLogicData { get; set; }
    public void SetItemData(Transform parent)
    {
        this.transform.parent = parent;
        m_UISprite.spriteName = this.BuffLogicData.PrefabName;
        m_UISprite.MakePixelPerfect();
    }
    public void SetPlunderItemDate(Transform parent)
    {
        this.transform.parent = parent;
        m_UISprite.spriteName = this.PropsLogicData.PrefabName;
        m_UISprite.MakePixelPerfect();
    }
    public void RemoveItem()
    {
        Destroy(this.gameObject);
    }
    public void SetPosition(Vector3 localPosition)
    {
        this.transform.localPosition = localPosition;
    }
 
}
