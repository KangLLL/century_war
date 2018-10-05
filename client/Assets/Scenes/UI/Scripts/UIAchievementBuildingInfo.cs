using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
using ConfigUtilities;
using System.Linq;
public class UIAchievementBuildingInfo : MonoBehaviour {
    [SerializeField] AchievementBuildingType m_AchievementBuildingType;
    public UILabel[] m_UILabel;//0 = prop count ;1 = description ;2 = name;
    public UISprite[] m_UISprite;//0 = prop icon
    public UISprite m_UISpriteLock; //lock
    public AchievementBuildingType AchievementBuildingType { get { return m_AchievementBuildingType; } }
    bool m_PropsCondition;
    bool m_BuildingCondition;
    string m_RequestPropName;
    public void SetItemData()
    {
       AchievementBuildingConfigData achievementBuildingConfigData = ConfigInterface.Instance.AchievementBuildingConfigHelper.GetAchievementBuildingData(m_AchievementBuildingType);
       PropsType propsType = achievementBuildingConfigData.NeedPropsType;
       PropsConfigData propsConfigData = ConfigInterface.Instance.PropsConfigHelper.GetPropsData(propsType);
       int currentPropsCount = LogicController.Instance.AllProps.Count(a => a.PropsType == propsType && a.RemainingCD <= 0);
       int needPropsNumber = achievementBuildingConfigData.NeedPropsNumber;
       this.m_PropsCondition = currentPropsCount >= needPropsNumber;
       string currentPropsCountString = !this.m_PropsCondition ? "[FF0000]" + currentPropsCount + "[-]" : currentPropsCount.ToString();
       m_UILabel[0].text = currentPropsCountString + "/" + needPropsNumber.ToString();
       m_UILabel[1].text = achievementBuildingConfigData.Description;
       m_UILabel[2].text = achievementBuildingConfigData.Name;
       m_UISprite[0].spriteName = propsConfigData.PrefabName;
       m_UISprite[0].MakePixelPerfect();
       this.m_BuildingCondition = LogicController.Instance.AllAchievementBuildings.Count < ConfigInterface.Instance.PropsRestrictionConfigHelper.GetPropsRestrictions(LogicController.Instance.CurrentCityHallLevel).MaxAchievementBuildingNumber;
       this.m_RequestPropName = propsConfigData.Name;
       
    }
    void OnClick()
    {
        if (!this.enabled)
            return;

        if (UIManager.Instance.UIWindowBuyBuilding.ControlerFocus != null)
            return;
        else
            if (this.m_BuildingCondition & this.m_PropsCondition)
                UIManager.Instance.UIWindowBuyBuilding.ControlerFocus = this.gameObject;

        if (!this.m_BuildingCondition)
        {
            UIErrorMessage.Instance.ErrorMessage(37);
            return;
        }
        if (!this.m_PropsCondition)
        {
            UIErrorMessage.Instance.ErrorMessage(38, this.m_RequestPropName);
            return;
        }
         
        UIManager.Instance.UIWindowBuyBuilding.BuyAchievementBuilding(m_AchievementBuildingType);
        UIManager.Instance.UIWindowBuyBuilding.HideWindow();
        SceneManager.Instance.EnableCreateWallContinuation = false;

    }
}
