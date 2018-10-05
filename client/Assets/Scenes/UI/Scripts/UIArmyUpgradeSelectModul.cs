using UnityEngine;
using System.Collections;

public class UIArmyUpgradeSelectModul : UIWindowItemCommon
{
    [SerializeField] UIArmyUpgradeItem[] m_UIArmyUpgradeItem;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public override void SetWindowItem()
    {
        for (int i = 0; i < m_UIArmyUpgradeItem.Length; i++)
        {
            m_UIArmyUpgradeItem[i].BuildingLogicData = base.BuildingLogicData;
            m_UIArmyUpgradeItem[i].SetItemData();
        }
    }
}
