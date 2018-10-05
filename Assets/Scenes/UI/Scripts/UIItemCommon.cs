using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
using ConfigUtilities;
public class UIItemCommon : MonoBehaviour {
    [SerializeField] BuildingType m_BuildingType;
    public BuildingType BuildingType { get { return m_BuildingType; } }
    public UILabel[] m_Text;//0=time;1=count;2=description
    public UILabel[] m_TextCost; //0=gold;1=food;2=oil;3=gem
    public UISprite m_UISpriteLock; //lock
    protected Vector3 m_IniLocalPosition = new Vector3(0,-156,-6);
    protected Vector3 m_OffsetLocalPosition = new Vector3(0, 65, 0);
	// Use this for initialization
	void Start () { 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public virtual bool SetItemData()
    { 
        BuildingConfigData buildingConfigData = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(m_BuildingType, 0);
        int upperLimitCount = 0;
        switch (this.m_BuildingType)
        {
            case BuildingType.BuilderHut:
                upperLimitCount = ConfigInterface.Instance.SystemConfig.MaxBuilderNumber;
                break;
            default:
                upperLimitCount = ConfigInterface.Instance.BuildingNumberRestrictionsConfigHelper.GetBuildingNumberRestrictions(LogicController.Instance.CurrentCityHallLevel).RestrictionDict[m_BuildingType];
                break;
        }
        int hasCount = LogicController.Instance.GetBuildingCount(m_BuildingType);
        string[] valueText = SystemFunction.ConverTObjectToArray<string>(SystemFunction.TimeSpanToString(buildingConfigData.UpgradeWorkload), hasCount + " / " + upperLimitCount, buildingConfigData.Description);
         
        for(int i=0;i<m_Text.Length;i++)
        {
            m_Text[i].text = valueText[i];
        } 
        return upperLimitCount > 0 && hasCount < upperLimitCount;
    }
    public virtual bool SetCostItemData()
    {
        BuildingConfigData buildingConfigData = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(m_BuildingType, 0);
        int[] costValue = SystemFunction.ConverTObjectToArray<int>(buildingConfigData.UpgradeGold, buildingConfigData.UpgradeFood, buildingConfigData.UpgradeOil, buildingConfigData.UpgradeGem);
        int[] userHasValue = SystemFunction.ConverTObjectToArray<int>(LogicController.Instance.PlayerData.CurrentStoreGold, LogicController.Instance.PlayerData.CurrentStoreFood, LogicController.Instance.PlayerData.CurrentStoreOil, LogicController.Instance.PlayerData.CurrentStoreGem);
        bool condition = true;
        for (int i = 0, j = 0; i < m_TextCost.Length; i++)
        {
            if (costValue[i] > 0)
            {
                m_TextCost[i].transform.parent.gameObject.SetActive(true);
                m_TextCost[i].text = costValue[i].ToString() + ClientSystemConstants.EXPRESSION_ICON_DICTIONARY[i];
                m_TextCost[i].color = costValue[i] <= userHasValue[i] ? new Color(1, 1, 1, 1) : new Color(1, 0, 0, 1);
                this.SetLabelIconSymbolStyle(m_TextCost[i]);
                m_TextCost[i].transform.parent.localPosition = m_IniLocalPosition + j * m_OffsetLocalPosition;
                j++;
                if (userHasValue[i] < costValue[i])
                    condition = false;
            }
            else
            {
                m_TextCost[i].transform.parent.gameObject.SetActive(false);
            }
        }
        return condition;
    }
    protected virtual void SetLock(bool condition)
    {
        m_UISpriteLock.alpha = condition ? 0 : 1;
    }
    void SetLabelIconSymbolStyle(UILabel uiLabel)
    {
        if (NewbieGuideManager.Instance != null)
        {
            if (LogicController.Instance.PlayerData.IsNewbie)
            {
                if (NewbieGuideManager.Instance.CurrentNewbieProgress < 8)
                    uiLabel.symbolStyle = UIFont.SymbolStyle.Colored;
                else
                    uiLabel.symbolStyle = UIFont.SymbolStyle.Uncolored;
            }
            else
                uiLabel.symbolStyle = UIFont.SymbolStyle.Uncolored;
        }
    }
}
