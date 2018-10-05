using UnityEngine;
using System.Collections;

public class UIBuildingInformationMudulDescription : UIWindowItemCommon
{
    [SerializeField]    UILabel m_UILabel;
	// Use this for initialization
    public override void SetWindowItem()
    {
        m_UILabel.text = base.BuildingLogicData.Description;
    }
    public override void SetWindowItemAchievementBuilding()
    {
        m_UILabel.text = base.AchievementBuildingLogicData.Description;
    }
}
