using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommandConsts;

public class UIWindowLeaderboard : UIWindowCommon
{
    [SerializeField] UIPersonalRankModul[] UIPersonalRankModul;
    private UIPersonalRankModul m_CurrentUIPersonalRankModul;
    private List<RankParameterOrder> m_RankParameterOrderList = new List<RankParameterOrder>();
    void Awake()
    {
        this.GetTweenComponent();
    } 
    public override void HideWindow()
    {
        base.HideWindowImmediately();
        UIManager.Instance.UIWindowLeaderboardChildVisit.HideWindow();
        
    }
    public override void ShowWindow()
    {
        this.OnPersonalRank();
        base.ShowWindowImmediately();
        UIDraggablePanel uiDraggablePanel = NGUITools.FindInParents<UIDraggablePanel>(UIPersonalRankModul[0].gameObject);// uiGrid.transform.parent.GetComponent<UIDraggablePanel>();

        uiDraggablePanel.transform.localPosition = Vector3.zero;
        UIScrollRegionAdaptive uiScrollRegionAdaptive = NGUITools.FindInParents<UIScrollRegionAdaptive>(UIPersonalRankModul[0].gameObject);
        uiScrollRegionAdaptive.OnSize();
    }
    protected override void GetTweenComponent()
    {
        base.GetTweenComponent();
    }
    void SetPersonalRankWindowItem()
    {
        if (this.m_CurrentUIPersonalRankModul != null)
            if (!this.m_CurrentUIPersonalRankModul.Equals(UIPersonalRankModul[0]))
                this.m_CurrentUIPersonalRankModul.HidePersonalRankModul();

        this.m_CurrentUIPersonalRankModul = UIPersonalRankModul[0];
        this.m_CurrentUIPersonalRankModul.ShowPersonalRankModul();
        this.m_CurrentUIPersonalRankModul.SetModulData(this.m_RankParameterOrderList);
        
        
    }
    //button message
    void OnPersonalRank()
    {
        if (this.m_CurrentUIPersonalRankModul != null)
        {
            this.m_CurrentUIPersonalRankModul.HidePersonalRankModul(); 
        }
        this.OnRequestRank();
    }
    void OnRequestRank()
    {
        CommunicationUtility.Instance.GetRankData(this, "OnResponseRank", true);
    }
    void OnResponseRank(Hashtable result)
    {
        RankResponseParameter response = new RankResponseParameter();
        response.InitialParameterObjectFromHashtable(result); 
        this.m_RankParameterOrderList.Clear();
        int order = 1;
        for (int i = 0; i < response.DetailList.Count; i++)
        {
            RankParameterOrder rankParamOrder = new RankParameterOrder();
            rankParamOrder.RankDetailResponseParameter = response.DetailList[i];
            if (i > 0)
            {
                if (!response.DetailList[i].Honour.Equals(response.DetailList[i - 1].Honour))
                    order++;
            }
            if (i == 10 || i == 50 /*|| i == 100*/) 
                this.m_RankParameterOrderList.Add(new RankParameterOrder() { Index = -1, Order = -1, RankDetailResponseParameter = new RankDetailResponseParameter() });
             
            rankParamOrder.Order = order;
            rankParamOrder.Index = i;
            this.m_RankParameterOrderList.Add(rankParamOrder); 
            
        } 
        if (response.CurrentPlayerRankData != null)
        {
            RankParameterOrder rankParamOrder = new RankParameterOrder();
            rankParamOrder.RankDetailResponseParameter = response.CurrentPlayerRankData.Detail; 
            this.m_RankParameterOrderList.Add(new RankParameterOrder() {  Index = -1, Order = -1, RankDetailResponseParameter = new RankDetailResponseParameter()});
            rankParamOrder.Order = response.CurrentPlayerRankData.Rank;
            this.m_RankParameterOrderList.Add(rankParamOrder);
        } 
        this.SetPersonalRankWindowItem(); 

    }
}
public class RankParameterOrder
{
    public RankDetailResponseParameter RankDetailResponseParameter;
    public int Order;
    public int Index;
    //public bool IsSeft = false;
}
