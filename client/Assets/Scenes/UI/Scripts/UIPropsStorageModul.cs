using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class UIPropsStorageModul : ReusableDelegate
{
    [SerializeField] PropModulType m_PropStorageType;
    [SerializeField] UISprite[] m_ModulBtnBakcground;//0 = Button true;1 = Button false;
    [SerializeField] UIDragPanelContents m_UIDragPanelContents;
    [SerializeField] ReusableScrollView m_ReusableScrollView;
    TweenAlpha m_TweenAlpha;
    List<PropsLogicData> m_PropsLogicDataList = new List<PropsLogicData>();
    void Awake()
    {
        this.GetTweenComponent();
    }
    public void SetModulData()
    { 
        
        this.m_PropsLogicDataList.Clear();
        switch (this.m_PropStorageType)
        {
            case PropModulType.All:
                this.m_PropsLogicDataList.AddRange(LogicController.Instance.AllProps);
                this.OrderByCategoryThenByQualityThenByCD();
                break;
            case PropModulType.Attack:
                this.m_PropsLogicDataList.AddRange(LogicController.Instance.AllProps.Where(prop => prop.Category == PropsCategory.Attack));
                this.OrderByQualityThenByCD();
                break;
            case PropModulType.Defend:
                this.m_PropsLogicDataList.AddRange(LogicController.Instance.AllProps.Where(prop => prop.Category == PropsCategory.Defense));
                this.OrderByQualityThenByCD();
                break;
            case PropModulType.Subsidiary:
                this.m_PropsLogicDataList.AddRange(LogicController.Instance.AllProps.Where(prop => prop.Category == PropsCategory.Auxiliary));
                this.OrderByQualityThenByCD();
                break;
            case PropModulType.Special:
                this.m_PropsLogicDataList.AddRange(LogicController.Instance.AllProps.Where(prop => prop.Category == PropsCategory.Special));
                this.OrderByQualityThenByCD();
                break;
        }
        this.m_ReusableScrollView.ReloadData();
    }
    void OrderByCategoryThenByQualityThenByCD()
    {
        this.m_PropsLogicDataList.Sort((a, b) =>
        {
            int order = a.RemainingCD - b.RemainingCD;
            if (order != 0)
                return order;
            else
            {
                order = (int)a.Category - (int)b.Category;
                if (order != 0)
                    return order;
                else
                    return (int)a.Quality - (int)b.Quality;
            }
        });
    }
    void OrderByQualityThenByCD()
    {
        this.m_PropsLogicDataList.Sort((a, b) =>
        {
            int order =  a.RemainingCD - b.RemainingCD; 
            if (order != 0)
                return order;
            else
                return (int)a.Quality - (int)b.Quality;
        });
    }

    public void ShowPropsModul()
    {
        UIManager.Instance.UIWindowPropInfo.HideWindow();
        this.gameObject.SetActive(true);
        this.SetModulBtnState(true);
        this.m_UIDragPanelContents.gameObject.SetActive(true);

        m_TweenAlpha.eventReceiver = null;
        m_TweenAlpha.duration = 0.6f;
        m_TweenAlpha.delay = 0;
        m_TweenAlpha.from = 0;
        m_TweenAlpha.to = 1;
        m_TweenAlpha.Play(true);
    }
    public void HidePropsModul(float? duration = null)
    {
        //this.gameObject.SetActive(false);
        this.SetModulBtnState(false);
        this.m_UIDragPanelContents.gameObject.SetActive(false);

        m_TweenAlpha.eventReceiver = this.gameObject;
        m_TweenAlpha.callWhenFinished = "OnFinished";
        m_TweenAlpha.duration = duration.HasValue ? duration.Value : 0.6f;
        m_TweenAlpha.Play(false);
    }
    void SetModulBtnState(bool isActive)
    {
        m_ModulBtnBakcground[0].color = isActive ? Color.white : Color.clear;
        m_ModulBtnBakcground[1].color = isActive ? Color.clear : Color.white;
    }
    public override void InitialCell(int index, GameObject cell)
    {
        UIPropItem uiPropItem = cell.GetComponent<UIPropItem>();
        if (index < m_PropsLogicDataList.Count)
            uiPropItem.SetItemData(m_PropsLogicDataList[index]);
        else
            uiPropItem.SetItemData(null);
    }
    public override int TotalNumberOfCells
    {
        get
        {
            switch (this.m_PropStorageType)
            {
                case PropModulType.All:
                    return LogicController.Instance.PlayerData.PropsMaxCapacity;
                case PropModulType.Attack:
                    return LogicController.Instance.AllProps.Count(prop => prop.Category == PropsCategory.Attack);
                case PropModulType.Defend:
                    return LogicController.Instance.AllProps.Count(prop => prop.Category == PropsCategory.Defense);
                case PropModulType.Special:
                    return LogicController.Instance.AllProps.Count(prop => prop.Category == PropsCategory.Special);
                case PropModulType.Subsidiary:
                    return LogicController.Instance.AllProps.Count(prop => prop.Category == PropsCategory.Auxiliary);
            }
            return LogicController.Instance.PlayerData.PropsMaxCapacity;
        }
    }
    void GetTweenComponent()
    {
        m_TweenAlpha = GetComponent<TweenAlpha>();
    }
    void OnFinished()
    {
        this.gameObject.SetActive(false);
    }
    
}
public enum PropModulType
{
    All,
    Attack,
    Defend,
    Subsidiary,
    Special
}
