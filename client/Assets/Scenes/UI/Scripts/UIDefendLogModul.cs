using UnityEngine;
using System.Collections;

public class UIDefendLogModul : ReusableDelegate 
{
    [SerializeField] ReusableScrollView m_ReusableScrollView;

    LogData[] m_LogData;

    public void SetModulData(LogData[] logdata)
    {
        this.m_LogData = logdata;
        this.m_ReusableScrollView.ReloadData();
    }
    public override void InitialCell(int index, GameObject cell)
    {
        UIDefendLogItem uiDefendLogItem = cell.GetComponent<UIDefendLogItem>();
        uiDefendLogItem.SetItemData(this.m_LogData[index]); 
    }
    public override int TotalNumberOfCells
    {
        get { return this.m_LogData.Length; }
    }
}
