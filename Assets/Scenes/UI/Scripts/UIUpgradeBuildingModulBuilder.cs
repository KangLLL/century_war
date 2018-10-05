using UnityEngine;
using System.Collections;
using ConfigUtilities;
using ConfigUtilities.Enums;
public class UIUpgradeBuildingModulBuilder : UIWindowItemCommon {
    [SerializeField] UILabel[] m_TextCost; //0=gold;1=food;2=oil;3=gem
    [SerializeField] UIItemBuilder[] m_UIItemBuilder;//0,1,2,3,4
    protected Vector3 m_IniLocalPosition = new Vector3(-131, 38, -3);
    protected Vector3 m_OffsetLocalPosition = new Vector3(0, -65, 0);
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public override void SetWindowItem()
    {
        SetCostItemData();
        SetBuilderItemData();
    }

     bool SetCostItemData()
    {
        BuildingConfigData buildingConfigData = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(this.BuildingLogicData.BuildingIdentity.buildingType, this.BuildingLogicData.Level);
        int[] costValue = SystemFunction.ConverTObjectToArray<int>(buildingConfigData.UpgradeGold, buildingConfigData.UpgradeFood, buildingConfigData.UpgradeOil, buildingConfigData.UpgradeGem);
        int[] userHasValue = SystemFunction.ConverTObjectToArray<int>(LogicController.Instance.PlayerData.CurrentStoreGold, LogicController.Instance.PlayerData.CurrentStoreFood, LogicController.Instance.PlayerData.CurrentStoreOil, LogicController.Instance.PlayerData.CurrentStoreGem);
        bool condition = true;
        for (int i = 0, j = 0; i < m_TextCost.Length; i++)
        {
            if (costValue[i] > 0)
            {
                m_TextCost[i].transform.parent.gameObject.SetActive(true);
                m_TextCost[i].text = costValue[i].ToString();
                m_TextCost[i].color = costValue[i] <= userHasValue[i] ? new Color(1, 1, 1, 1) : new Color(1, 0, 0, 1);
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
     void SetBuilderItemData()
     {
         //int builderCount = LogicController.Instance.GetBuildingCount(base.BuildingLogicObject.BuildingIdentity.buildingType);
         //int builderCount = ConfigInterface.Instance.SystemConfig.MaxBuilderNumber; //LogicController.Instance.AllBuilderInformation.Count;
         BuildingConfigData buildingConfigData = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(base.BuildingLogicData.BuildingIdentity.buildingType, base.BuildingLogicData.Level);
         for (int i = 0, count = m_UIItemBuilder.Length; i < count; i++)
         {
             m_UIItemBuilder[i].BuildingLogicData = base.BuildingLogicData;
             m_UIItemBuilder[i].BuildingConfigData = buildingConfigData;
             m_UIItemBuilder[i].BuilderMenuType = BuilderMenuType.Upgrade;
             // if (i <= builderCount - 1)

             //BuilderInformation builderInformation = LogicController.Instance.AllBuilderInformation[i];
             BuilderData builderData = LogicController.Instance.AllBuilderInformation[i];
             if (builderData == null)
             {
                 m_UIItemBuilder[i].SetItemDataDisable();
                 m_UIItemBuilder[i].BuilderNo = i;
                 m_UIItemBuilder[i].BuilderState = BuilderState.Disable;
             }
             else
             {
                 if (builderData.CurrentWorkTarget == null)//builder idle
                 {
                     m_UIItemBuilder[i].BuilderData = builderData;
                     m_UIItemBuilder[i].BuilderNo = i;
                     m_UIItemBuilder[i].BuilderState = BuilderState.Idle;
                     m_UIItemBuilder[i].SetItemDataIdle();
                 }
                 else//buider busy
                 {
                     // m_UIItemBuilder[i].BuilderInformation = builderData;
                     m_UIItemBuilder[i].BuilderData = builderData;
                     m_UIItemBuilder[i].BuilderNo = i;
                     m_UIItemBuilder[i].BuilderState = BuilderState.Busy;
                     m_UIItemBuilder[i].SetItemDataBusy();
                 }
             }


             //LogicController.Instance.   
             //BuilderInformation builderInformation = LogicController.Instance.BusyBuilderInformation[i];
             //builderInformation.BuilderID = 3 , 2 ,1
         }
     }
    //int level = LogicController.Instance.GetBuildingObject(this.CurrentBuildBuilding).Level;
    //return ConfigInterface.Instance.BuilderConfigHelper.GetBuilderData(level).BuildEfficiency;
}
