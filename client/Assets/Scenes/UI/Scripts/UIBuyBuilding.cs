using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
using ConfigUtilities;
public class UIBuyBuilding : MonoBehaviour {
    [SerializeField] BuildingType m_BuildingType;
    public void OnClick()
    {
        if (!this.enabled)
            return;
        if (UIManager.Instance.UIWindowBuyBuilding.ControlerFocus != null)
            return;
        else
            if (!this.GetComponent<UIItemInfomation>().IsLock)
            {
                if (ConfigInterface.Instance.BuildingNumberRestrictionsConfigHelper.IsMaxNumber(this.m_BuildingType, LogicController.Instance.CurrentCityHallLevel))
                {
                    UIErrorMessage.Instance.ErrorMessage(17);
                    return;
                }
                int nextCityHallLevel = ConfigInterface.Instance.BuildingNumberRestrictionsConfigHelper.GetNextRestrictionCityHallLevel(this.m_BuildingType, LogicController.Instance.CurrentCityHallLevel);
                UIErrorMessage.Instance.ErrorMessage(0, ClientSystemConstants.BUILDING_NAME_DICTIONARY[BuildingType.CityHall], nextCityHallLevel.ToString());
                return;
            }
            else 
                UIManager.Instance.UIWindowBuyBuilding.ControlerFocus = this.gameObject; 
      
        //if (SceneManager.Instance.PickableObjectCurrentSelect != null)
        //{
        //    SceneManager.Instance.PickableObjectCurrentSelect.OnUnSelect(true);
        //    SceneManager.Instance.PickableObjectCurrentSelect = null;
        //}
        UIManager.Instance.UIWindowBuyBuilding.BuyBuilding(m_BuildingType);
        UIManager.Instance.UIWindowBuyBuilding.HideWindow();
        SceneManager.Instance.EnableCreateWallContinuation = false;
   
    }
    public bool CheckLock()
    {
        return this.GetComponent<UIItemInfomation>().IsLock;
    }
}
