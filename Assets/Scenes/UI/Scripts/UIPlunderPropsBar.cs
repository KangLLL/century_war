using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class UIPlunderPropsBar : MonoBehaviour {
    //[SerializeField] Vector3 m_Interval;
    [SerializeField] UIBuffItem m_UIBuffItem;
    [SerializeField] UIGrid m_UIGride;
    List<UIBuffItem> m_UIBuffItemList = new List<UIBuffItem>();
	// Use this for initialization
	void Start () {

        this.ShowWindowItem();
	}

    void ShowWindowItem()
    {
        List<PropsLogicData> propsLogicDataList = new List<PropsLogicData>(LogicController.Instance.CurrentFriend.AllPlunderableProps.Where(a => a.RemainingCD >= 0));

        foreach (PropsLogicData propsLogicData in propsLogicDataList)
            this.CreateProp(propsLogicData);
        this.SortProps();
    }
    void CreateProp(PropsLogicData propsLogicData)
    { 
        UIBuffItem uiBuffItem = (Instantiate(m_UIBuffItem.gameObject) as GameObject).GetComponent<UIBuffItem>();
        uiBuffItem.PropsLogicData = propsLogicData;
        uiBuffItem.SetPlunderItemDate(this.transform);
        m_UIBuffItemList.Add(uiBuffItem);
    }
    void SortProps()
    {
        m_UIBuffItemList.Sort((a, b) => (int)a.PropsLogicData.PropsType - (int)b.PropsLogicData.PropsType);
        for (int i = 0; i < m_UIBuffItemList.Count; i++)
        {
            //m_UIBuffItemList[i].SetPosition(i * this.m_Interval);
            m_UIBuffItemList[i].gameObject.name = 10000 + i.ToString() + m_UIBuffItemList[i].gameObject.name;
        }
        m_UIGride.Reposition();
    }
}
