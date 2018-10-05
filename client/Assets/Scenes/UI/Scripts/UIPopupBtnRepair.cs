using UnityEngine;
using System.Collections;

public class UIPopupBtnRepair : MonoBehaviour {
    [SerializeField] UIPanel m_UIPanel;
    public AchievementBuildingLogicData AchievementBuildingLogicData { get; set; }
    void Update()
    {
        this.SetBtnData();
    }
    public void SetBtnData()
    {
        if (this.AchievementBuildingLogicData != null)
            this.m_UIPanel.alpha = this.AchievementBuildingLogicData.Life >= this.AchievementBuildingLogicData.MaxLife ? 0.5f : 1.0f;
    }
}
