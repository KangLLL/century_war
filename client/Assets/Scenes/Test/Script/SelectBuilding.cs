using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class SelectBuilding : MonoBehaviour {
    /*
    [SerializeField] BuildingType m_BuildingType;
    [SerializeField] string m_TempBuildingPrefabName;
    ButtonListener m_ButtonListener;
    BuildingConstruct m_BuildingConstruct;
	// Use this for initialization
	void Start () {
        //m_ButtonListener = GetComponent<ButtonListener>();
        m_BuildingConstruct = GameObject.Find("BuildingConstruct").GetComponent<BuildingConstruct>();
	}
	
	// Update is called once per frame
	void Update () {
         
	}
    void OnClick()
    {
        BuildingData buildingData = new BuildingData { BuildingType = m_BuildingType, UpgradeWorkload = 5,
                                                       UpgradeRemainingWorkload = 5,
                                                       BuildingPrefabName = m_TempBuildingPrefabName,
                                                       ActorWorkEfficiency = 1.0f,
                                                       CurrentBuilidngState = BuildingEditorState.Create,
                                                       CanStoreFood = true,
                                                       CanStoreGold = true,
                                                       CanStoreOil = true,
                                                       CanCollectGold = true,
                                                       CurrentStoreGold = 100,
                                                       StoreGoldCapacity = 120,
                                                       ProduceGoldEfficiency = 1,
                                                       CanProduceArmy = true,
                                                       CanUpgradeArmy = true
        };
       // m_ButtonListener.Parameter = buildingData;
        //m_BuildingConstruct.ConstructBuilding(buildingData);
        //ConfigUtilities.BuildingConfigData buildingConfigData = ConfigUtilities.ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(BuildingType.ArmyCamp, 1);
        //ConfigUtilities.ArmyConfigData armyConfigData = ConfigUtilities.ConfigInterface.Instance.ArmyConfigHelper.GetArmyData(ArmyType.Berserker, 1);
        ConfigUtilities.BuildingNumberRestrictionsConfigData buildingNumberRestrictionsConfigData = ConfigUtilities.ConfigInterface.Instance.BuildingNumberRestrictionsConfigHelper.GetBuildingNumberRestrictions(1);
        int count = buildingNumberRestrictionsConfigData.RestrictionDict[BuildingType.ArmyCamp];
        //ConfigUtilities.BuildingConfigData buildingConfigData = null;  
     
    }

*/
}
