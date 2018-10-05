using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;
public class UIUpgradeBuildingModulAppend : UIWindowItemCommon
{
    [SerializeField] UIItemAppend m_BuildingAppend;
    //[SerializeField] UIItemAppend[] m_ArmyAppend;//Berserker,Ranger,Marauder
    //[SerializeField] UIItemAppend[] m_MercenaryAppend;//  Slinger = 0, Hercules = 1,
    [SerializeField] PrefabDictionary m_ArmyTypeDict;
    [SerializeField] PrefabDictionary m_MercenaryTypeDict;
    [SerializeField] UIItemAppend m_SpellAppend;//MagicBottle
    [SerializeField] UIGrid m_UIGrid;
    [SerializeField] TweenAlpha m_TweenAlphaPanel;
    [SerializeField] UIDraggablePanel m_UIDraggablePanel;
    [SerializeField] Vector4 m_ClipRange;
	// Use this for initialization
 
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public override void SetWindowItem()
    {
        print("UIUpgradeBuildingModulAppend");
        while (m_UIGrid.transform.childCount > 0)
        {
            Transform trans = m_UIGrid.transform.GetChild(0);
            trans.parent = null;
            DestroyImmediate(trans.gameObject);
        }
        int buildingLevel = base.BuildingLogicData.Level;
        int buildingLevelMax = base.BuildingLogicData.MaximumLevel;
        int buldingLevelNext = buildingLevel + 1 > buildingLevelMax ? buildingLevel : buildingLevel + 1;
    
        switch (base.BuildingLogicData.BuildingIdentity.buildingType)
        {
            case BuildingType.CityHall:
                  Dictionary<BuildingType,int> currentRestrictionDict = ConfigInterface.Instance.BuildingNumberRestrictionsConfigHelper.GetBuildingNumberRestrictions(buildingLevel).RestrictionDict;
                  Dictionary<BuildingType,int> nextRestrictionDict = ConfigInterface.Instance.BuildingNumberRestrictionsConfigHelper.GetBuildingNumberRestrictions(buldingLevelNext).RestrictionDict;
                for (int i = 1, count = (int)BuildingType.Length; i < count; i++)
                {
                    int currentLevelCount =currentRestrictionDict[(BuildingType)i];
                    int nextLevelCount = nextRestrictionDict[(BuildingType)i];
                    if (currentLevelCount == 0 && nextLevelCount>0)//new
                    { 
                        UIItemAppend uiItemAppend = InstantiateComponent(m_BuildingAppend);
                        uiItemAppend.SetItemData(ClientSystemConstants.BUILDING_ICON_DICTIONARY[(BuildingType)i], false, "New", string.Empty, string.Empty);
                        uiItemAppend.MakePixelPerfect();
                    }
                    else
                        if (nextLevelCount - currentLevelCount > 0 && currentLevelCount > 0)//append
                        {
                            UIItemAppend uiItemAppend = InstantiateComponent(m_BuildingAppend);
                            uiItemAppend.SetItemData(ClientSystemConstants.BUILDING_ICON_DICTIONARY[(BuildingType)i], false, "X" + (nextLevelCount - currentLevelCount) , string.Empty, string.Empty);
                            uiItemAppend.MakePixelPerfect();
                        }
                }
                break;
            case BuildingType.Barracks:
                for (int i = 0, count = (int)ArmyType.Length;i<count ; i++)
                {
                    int barracksLevel = ConfigInterface.Instance.ArmyConfigHelper.GetArmyRequireBuildingLevel((ArmyType)i);
                    if (base.BuildingLogicData.Level + base.BuildingLogicData.UpgradeStep == barracksLevel)
                    {
                        //UIItemAppend uiItemAppend = InstantiateComponent(m_ArmyAppend[i]);
                        UIItemAppend uiItemAppend = InstantiateComponent(m_ArmyTypeDict[((ArmyType)i).ToString()].GetComponent<UIItemAppend>());
                        //uiItemAppend.SetItemData(ClientSystemConstants.ARMY_ICON_COMMON_DICTIONARY[(ArmyType)i], false, string.Empty, string.Empty);
                        uiItemAppend.SetItemData(false, string.Empty, string.Empty);
                    }
                }
                
                break;
            case BuildingType.Tavern: 
                List<MercenaryType> mercenaryTypes = ConfigInterface.Instance.MercenaryConfigHelper.GetAvailableMercenaries(base.BuildingLogicData.Level + base.BuildingLogicData.UpgradeStep);
                for (int i = 0; i < mercenaryTypes.Count; i++)
                {
                    UIItemAppend uiItemAppend = InstantiateComponent(m_MercenaryTypeDict[mercenaryTypes[i].ToString()].GetComponent<UIItemAppend>());
                    //UIItemAppend uiItemAppend = InstantiateComponent(m_MercenaryAppend[(int)mercenaryTypes[i]]);
                    //uiItemAppend.SetItemData(ClientSystemConstants.MERCENARY_ICON_COMMON_DICTIONARY[mercenaryTypes[i]], false, string.Empty, string.Empty);
                    uiItemAppend.SetItemData( false, string.Empty, string.Empty);
                }
                break;
		 
        }
        m_UIGrid.Reposition();
        m_UIDraggablePanel.ResetPosition();
        m_UIDraggablePanel.transform.localPosition = Vector3.zero;
        UIPanel uiPanel = NGUITools.FindInParents<UIPanel>(m_UIGrid.gameObject);
        uiPanel.clipRange = m_ClipRange;
    }
    UIItemAppend InstantiateComponent(UIItemAppend source)
    {
        UIItemAppend component = (GameObject.Instantiate(source.gameObject) as GameObject).GetComponent<UIItemAppend>();
        component.transform.parent = m_UIGrid.transform;
        component.transform.localScale = m_UIGrid.transform.localScale;
        //component.gameObject.SetActive(true);
        component.transform.localPosition = new Vector3(0, 0, 0); 
        return component;
    }
    public void ShowPanel()
    {
        m_TweenAlphaPanel.duration = 0.2f;
        m_TweenAlphaPanel.delay = 0;
        m_TweenAlphaPanel.from = 0;
        m_TweenAlphaPanel.to = 1;
        m_TweenAlphaPanel.Play(true);
    }
    public void HidePanel()
    {
        m_TweenAlphaPanel.Play(false);
    }
}
