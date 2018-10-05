using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
using System.Collections.Generic;
using ConfigUtilities;
public class UIBuildingInformationmMudulAppend : UIWindowItemCommon
{
    [SerializeField] UIItemAppend m_BuildingAppend;
    //[SerializeField] UIItemAppend[] m_ArmyAppend;//Berserker,Ranger,Marauder
    //[SerializeField] UIItemAppend[] m_MercenaryAppend;//Slinger = 0,Hercules = 1
    [SerializeField] PrefabDictionary m_ArmyTypeDict;
    [SerializeField] PrefabDictionary m_MercenaryTypeDict;
    [SerializeField] UIGrid m_UIGrid;
    [SerializeField] UIDraggablePanel m_UIDraggablePanel;
    [SerializeField] Vector4 m_ClipRange;
    [SerializeField] TweenAlpha m_TweenAlphaPanel;
    // Update is called once per frame
    void Update()
    {

    }
    public override void SetWindowItem()
    { 

        while (m_UIGrid.transform.childCount > 0)
        {
            Transform trans = m_UIGrid.transform.GetChild(0);
            trans.parent = null;
            DestroyImmediate(trans.gameObject);
        }
        if (SceneManager.Instance.SceneMode == SceneMode.SceneBuild)
        {
            switch (base.BuildingLogicData.BuildingIdentity.buildingType)
            {
                case BuildingType.ArmyCamp:
                    foreach (KeyValuePair<ArmyType, List<ArmyIdentity>> v in LogicController.Instance.AvailableArmies)
                    {
                        int level = LogicController.Instance.GetArmyLevel(v.Value[0].armyType);
                        int count = v.Value.Count;
                        UIItemAppend uiItemAppend = InstantiateComponent(m_ArmyTypeDict[v.Value[0].armyType.ToString()].GetComponent<UIItemAppend>());
                        uiItemAppend.SetItemData(true, StringConstants.PROMPT_LEVEL + level.ToString(), "X" + count.ToString());
                    }
                    foreach (KeyValuePair<MercenaryType, List<MercenaryIdentity>> v in LogicController.Instance.AvailableMercenaries)
                    {
                        int count = v.Value.Count;
                        UIItemAppend uiItemAppend = InstantiateComponent(m_MercenaryTypeDict[v.Value[0].mercenaryType.ToString()].GetComponent<UIItemAppend>());
                        uiItemAppend.SetItemData(true, string.Empty, "X" + count.ToString());
                    }
                    break;
            }
        }
        else
        {
            switch (base.BuildingLogicData.BuildingIdentity.buildingType)
            {
                case BuildingType.ArmyCamp:
                    foreach (KeyValuePair<ArmyType, int> v in LogicController.Instance.CurrentFriend.TotalArmies)
                    {
                        int level = LogicController.Instance.CurrentFriend.GetArmyLevel(v.Key);
                        int count = v.Value;
                        UIItemAppend uiItemAppend = InstantiateComponent(m_ArmyTypeDict[v.Key.ToString()].GetComponent<UIItemAppend>());
                        uiItemAppend.SetItemData(true, StringConstants.PROMPT_LEVEL + level.ToString(), "X" + count.ToString());
                    }
                    foreach(KeyValuePair<MercenaryType,int> v in LogicController.Instance.CurrentFriend.TotalMercenaries)
                    {
                        int count = v.Value;
                        UIItemAppend uiItemAppend = InstantiateComponent(m_MercenaryTypeDict[v.Key.ToString()].GetComponent<UIItemAppend>());
                        uiItemAppend.SetItemData(true, string.Empty, "X" + count.ToString());
                    }
                    break;
            }
        }
        //m_UIGrid.sorted = true;
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