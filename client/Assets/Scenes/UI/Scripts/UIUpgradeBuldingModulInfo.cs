using UnityEngine;
using System.Collections;

public class UIUpgradeBuldingModulInfo : UIWindowItemCommon
{
    [SerializeField] UILabel[] m_UILabelText;//Damage range,Damage type,Damage target,Favorite
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public override void SetWindowItem()
    {
        m_UILabelText[0].text = base.BuildingLogicData.ApMaxScope.ToString();
        m_UILabelText[1].text = ClientSystemConstants.ATTACKTYPE_DICTIONARY[base.BuildingLogicData.AttackType];
        m_UILabelText[2].text = ClientSystemConstants.TARGETTYPE_DICTIONARY[base.BuildingLogicData.TargetType]; 
        m_UILabelText[3].text = ClientSystemConstants.ARMYCATEGORY_DICTIONARY[BuildingLogicData.Favorite];
    }
}
