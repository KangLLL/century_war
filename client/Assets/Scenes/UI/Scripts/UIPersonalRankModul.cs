using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UIPersonalRankModul : ReusableDelegate
{
    [SerializeField] UISprite[] m_ModulBtnBakcground;//0 = Button true;1 = Button false;
    [SerializeField] UIDragPanelContents m_UIDragPanelContents;
    [SerializeField] ReusableScrollView m_ReusableScrollView;
    private List<RankParameterOrder> m_RankParameterOrderList = new List<RankParameterOrder>();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }
    public void ShowPersonalRankModul()
    {
        UIManager.Instance.UIWindowLeaderboardChildVisit.HideWindow();
        this.gameObject.SetActive(true);
        this.SetModulBtnState(true);
        this.m_UIDragPanelContents.gameObject.SetActive(true);
    }
    public void HidePersonalRankModul()
    {
        this.gameObject.SetActive(false);
        this.SetModulBtnState(false);
        this.m_UIDragPanelContents.gameObject.SetActive(false);
    }
    public void HideEmailModul()
    {
        this.gameObject.SetActive(false);
        this.SetModulBtnState(false);
    }
    void SetModulBtnState(bool isActive)
    {
        m_ModulBtnBakcground[0].color = isActive ? Color.white : Color.clear;
        m_ModulBtnBakcground[1].color = isActive ? Color.clear : Color.white;
    }
    public void SetModulData(object param)
    {
        this.m_RankParameterOrderList = param as List<RankParameterOrder>;
        this.m_ReusableScrollView.ReloadData();
    }
    public override void InitialCell(int index, GameObject cell)
    {
        //print("index=" + index);
        //if (index == 10 || index == 50 || index == 100)
        //{
        //    print("index index=" + index);
        //    cell.SetActive(false);
        //    return;
        //}
        //else
        //    cell.SetActive(true);
        UIRankItem uiRankItem = cell.GetComponent<UIRankItem>();

        //print("order =" + this.m_RankParameterOrderList[index].Order);
        //print("this.m_RankParameterOrderList[index].name =" + this.m_RankParameterOrderList[index].RankDetailResponseParameter.Name);
        uiRankItem.SetItemData(this.m_RankParameterOrderList[index]);
        //cell.GetComponentInChildren<UILabel>().text = index.ToString();
    }
    public override int TotalNumberOfCells
    {
        get { return m_RankParameterOrderList.Count; }
    }
}
