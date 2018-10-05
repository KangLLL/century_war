using UnityEngine;
using System.Collections;

public class UIWindowSearch : UIWindowCommon
{
    [SerializeField] UIWindowItemCommon[] m_UIWindowItemCommon;
        void Awake()
    {
        this.GetTweenComponent();
    }
    protected override void GetTweenComponent()
    {
        base.GetTweenComponent();
    }
    public override void HideWindow()
    {
        base.HideWindowImmediately();  
    }
    public override void ShowWindow()
    {
        SetWindowItemData();
        base.ShowWindowImmediately();
    }
    public void SetWindowItemData()
    {
        for (int i = 0; i < m_UIWindowItemCommon.Length; i++)
        {
            m_UIWindowItemCommon[i].BuildingLogicData = base.BuildingLogicData;
            m_UIWindowItemCommon[i].SetWindowItem();
        }
    }
 
}
