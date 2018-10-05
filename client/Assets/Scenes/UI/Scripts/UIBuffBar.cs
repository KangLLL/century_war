using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
public class UIBuffBar : MonoBehaviour {
    [SerializeField] Vector3 m_Interval;
    [SerializeField] UIBuffItem m_UIBuffItem;
    [SerializeField] AlignType m_Align;
    List<UIBuffItem> m_UIBuffItemList = new List<UIBuffItem>();
    IEnumerable<BuffLogicData> m_AllBuffs = new List<BuffLogicData>();
	
	// Update is called once per frame
	void Update () 
    {
        this.ShowWindowItem();
	}
    void ShowWindowItem()
    {
        switch (SceneManager.Instance.SceneMode)
        {
            case SceneMode.SceneBuild:
                this.m_AllBuffs = LogicController.Instance.AllBuffs;
                break;
            case SceneMode.SceneVisit:
                this.m_AllBuffs = LogicController.Instance.CurrentFriend.AllBuffs;
                break;
        }
        bool isChange = false;
        foreach (BuffLogicData buff in this.m_AllBuffs)
        {
            bool has = false;
            m_UIBuffItemList.ForEach(a => { if (a.BuffLogicData.PropsType.Equals(buff.PropsType)) { has = true; return; } });
            if (!has) { this.CreateBuff(buff); isChange = true; }
        }
        m_UIBuffItemList.ForEach(
            a =>
            {
                bool has = false;
                foreach (BuffLogicData buff in this.m_AllBuffs) 
                    if (buff.PropsType.Equals(a.BuffLogicData.PropsType)) { has = true; break; }
                if (!has) { this.RemoveBuff(a); isChange = true; }
            });
        if (isChange) this.OnSortBuff();
    }
    void CreateBuff(BuffLogicData buffLogicData)
    {
        UIBuffItem uiBuffItem = (Instantiate(m_UIBuffItem.gameObject) as GameObject).GetComponent<UIBuffItem>();
        uiBuffItem.BuffLogicData = buffLogicData;
        uiBuffItem.SetItemData(this.transform);
        m_UIBuffItemList.Add(uiBuffItem);
    }
    void RemoveBuff(UIBuffItem uiBuffItem)
    {
        this.m_UIBuffItemList.Remove(uiBuffItem);
        uiBuffItem.RemoveItem();
    }
    void SortBuffMiddle()
    {
        m_UIBuffItemList.Sort((a, b) => (int)a.BuffLogicData.PropsType - (int)b.BuffLogicData.PropsType);
        Vector3 from = new Vector3((m_UIBuffItemList.Count - 1) * this.m_Interval.x / 2, 0, 0);
        for (int i = 0; i < m_UIBuffItemList.Count; i++)
            m_UIBuffItemList[i].SetPosition(from + i * this.m_Interval);
    }
    void SortBuffLeft()
    {
        m_UIBuffItemList.Sort((a, b) => (int)a.BuffLogicData.PropsType - (int)b.BuffLogicData.PropsType);
        Vector3 from = Vector3.zero;
        for (int i = 0; i < m_UIBuffItemList.Count; i++)
            m_UIBuffItemList[i].SetPosition(from + i * this.m_Interval);
    }
    void SortBuffRight()
    {
        m_UIBuffItemList.Sort((a, b) => (int)b.BuffLogicData.PropsType - (int)a.BuffLogicData.PropsType);
        Vector3 from = Vector3.zero;
        for (int i = 0; i < m_UIBuffItemList.Count; i++)
            m_UIBuffItemList[i].SetPosition(from - i * this.m_Interval);
    }
    void OnSortBuff()
    {
        switch (this.m_Align)
        {
            case AlignType.Left:
                this.SortBuffLeft();
                break;
            case AlignType.Middle:
                this.SortBuffMiddle();
                break;
            case AlignType.Right:
                this.SortBuffRight();
                break;
        }
    }
}
public enum AlignType
{
    Left,
    Middle,
    Right
}