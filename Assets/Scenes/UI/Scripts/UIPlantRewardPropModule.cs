using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;
public class UIPlantRewardPropModule : ReusableDelegate
{
    [SerializeField] ReusableScrollView m_ReusableScrollView;
    List<KeyValuePair<PropsType, int>> m_GeneratePropsRateList = new List<KeyValuePair<PropsType, int>>();
    public void SetModulData(List<KeyValuePair<PropsType, int>> generatePropsRateList)
    {
        this.m_GeneratePropsRateList = generatePropsRateList;
        this.m_ReusableScrollView.ReloadData();

    }
    public override void InitialCell(int index, GameObject cell)
    {
        UIPlantRewardPropItem uiUIPlantRewardPropItem = cell.GetComponent<UIPlantRewardPropItem>();
        uiUIPlantRewardPropItem.SetItemData(m_GeneratePropsRateList[index].Key);
    }
    public override int TotalNumberOfCells
    {
        get { return this.m_GeneratePropsRateList.Count; }
    }
}
