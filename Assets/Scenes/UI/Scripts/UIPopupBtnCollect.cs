using UnityEngine;
using System.Collections;

public class UIPopupBtnCollect : MonoBehaviour {
    [SerializeField] UIPanel m_UIPanel;
    [SerializeField] ResourceType m_ResourceType;
    public BuildingLogicData BuildingLogicData { get; set; }

	// Update is called once per frame
	void Update () 
    {
        if (this.BuildingLogicData != null)
            this.SetBtnData();
    }
    public void SetBtnData()
    {
        this.m_UIPanel.alpha = SystemFunction.CheckCollectValidityByButton(this.BuildingLogicData, m_ResourceType) ? 1 : 0.5f;
    }
}
