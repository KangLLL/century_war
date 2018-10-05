using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class UIUpgradeBuildingModulForbid : UIWindowItemCommon
{
    [SerializeField] UILabel m_UILabel;
    public override void SetWindowItem()
    {
        if (base.BuildingLogicData.BuildingIdentity.buildingType == BuildingType.CityHall)
            m_UILabel.text = string.Format(StringConstants.ERROR_MESSAGE[0], StringConstants.PROMPT_EXP_LEVEL, base.BuildingLogicData.Level + base.BuildingLogicData.UpgradeStep) + StringConstants.PROMT_GET_EXP;
        else
            m_UILabel.text = string.Format(StringConstants.ERROR_MESSAGE[0], ClientSystemConstants.BUILDING_NAME_DICTIONARY[BuildingType.CityHall], base.BuildingLogicData.Level + base.BuildingLogicData.UpgradeStep);
    }
 
}
