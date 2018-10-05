using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UIGridPage : MonoBehaviour {

    [SerializeField] UIGridExt2 m_UIGrid;
    [SerializeField] int m_PerPageCount;
    int m_CurrentPage = 0;
	// Use this for initialization
	void Start () {
	
	}
    public void SetPage()
    {
        //int page = this.GetPageCount();
        List<Transform> transList = new List<Transform>();
        for (int i = 0; i < m_UIGrid.transform.childCount; i++)
        {
            Transform trans = m_UIGrid.transform.GetChild(i);
            transList.Add(trans);
            trans.gameObject.SetActive(false);
        }
        transList.Sort((a, b) => string.Compare(a.name, b.name));
        int start = m_CurrentPage * m_PerPageCount;
        int end = (m_CurrentPage + 1) * m_PerPageCount - 1;
        end = end < m_UIGrid.transform.childCount ? end : m_UIGrid.transform.childCount - 1;
        for (int j = start; j <= end; j++)
            transList[j].gameObject.SetActive(true);
        
        m_UIGrid.Reposition();
    }
    public void TurnPage()
    {
        m_CurrentPage++;
        m_CurrentPage = m_CurrentPage >= this.GetPageCount() ? 0 : m_CurrentPage;
        
    }
    public void ResetPage()
    {
        m_CurrentPage = 0;
    }
    public int GetPageCount()
    {
       return m_PerPageCount <= 0 ? 1 : Mathf.CeilToInt((float)m_UIGrid.transform.childCount / m_PerPageCount);
    }
	
 
}
