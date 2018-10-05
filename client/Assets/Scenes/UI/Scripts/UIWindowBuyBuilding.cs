using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
using ConfigUtilities;
public class UIWindowBuyBuilding : UIWindowCommon
{
    [SerializeField] UILabel[] m_UILabelText;//0=title;1=food;2=gold;3=gem;
    [SerializeField] UIGrid[] m_UIGrid;
    [SerializeField] UIDragPanelContents[] m_UIDraggablePanelContents;
    //[SerializeField] Vector4 m_ClipRange;
    int m_BuilerNo = -1;
    public int BuilerNo { get { return m_BuilerNo; } set { m_BuilerNo = value; } }
    // Use this for initialization
    void Awake()
    {
        this.GetTweenComponent();
    }

 
    public override void ShowWindow()
    {
        base.ShowWindowImmediately();
        UIManager.Instance.UIWindowMain.gameObject.SetActive(false);
    }
    public override void HideWindow()
    {
        base.HideWindowImmediately(true,true);
        UIManager.Instance.UIWindowMain.gameObject.SetActive(true);
    }
    protected override void GetTweenComponent()
    {
        base.GetTweenComponent();
    }
    public void ShowWindows(object param)
    {
        this.m_BuilerNo = -1;
        this.ShowWindow();
        SetWindow((UIMenuType)param);
    }

    public void ShowWindow(UIMenuType uiMenuType, BuildingType? buildingType = null)
    {
        this.ShowWindow();
        SetWindow(uiMenuType, buildingType);
    }

    //Button message
    public void ShowWindowBuyBuilderHurt()
    {
        this.ShowWindow();
        SetWindow(UIMenuType.Composite, BuildingType.BuilderHut);
    }
    void SetWindow(UIMenuType uiMenuType, BuildingType? buildingType = null)
    {
        for (int i = 0; i < m_UIGrid.Length; i++)
        {
            m_UIGrid[i].transform.parent.gameObject.SetActive(false);
            m_UIDraggablePanelContents[i].gameObject.SetActive(false);
        }
        m_UIGrid[(int)uiMenuType].transform.parent.gameObject.SetActive(true);
        m_UIDraggablePanelContents[(int)uiMenuType].gameObject.SetActive(true);
        m_UILabelText[0].text = ClientSystemConstants.UIMENU_TYPE_DICTIONARY[uiMenuType];
        m_UILabelText[1].text = LogicController.Instance.PlayerData.CurrentStoreFood.ToString();
        m_UILabelText[2].text = LogicController.Instance.PlayerData.CurrentStoreGold.ToString();
        m_UILabelText[3].text = LogicController.Instance.PlayerData.CurrentStoreGem.ToString();
        SetItemData(m_UIGrid[(int)uiMenuType], buildingType);
        SpringPanel springPanel = NGUITools.FindInParents<SpringPanel>(m_UIGrid[(int)uiMenuType].gameObject);
        DestroyImmediate(springPanel);
    }
    void SetItemData(UIGrid uiGrid, BuildingType? buildingType = null)
    {
        //int count = uiGrid.transform.childCount;
       // UIItemInfomation[] uiItemInfomation = new UIItemInfomation[count];
        //for (int i = 0; i < count; i++)
        //{
        //    uiItemInfomation[i] = uiGrid.transform.GetChild(i).GetComponent<UIItemInfomation>();
        //}
       UIItemInfomation[] uiItemInfomation = uiGrid.GetComponentsInChildren<UIItemInfomation>(true);
       UIAchievementBuildingInfo[] uiIAchievementBuildingInfo = uiGrid.GetComponentsInChildren<UIAchievementBuildingInfo>(true);
       foreach (UIItemInfomation info in uiItemInfomation)
       {
           if (!buildingType.HasValue)
           { 
               info.gameObject.SetActive(true);
               info.SetCostItemData();
               info.IsLock = info.SetLock(info.SetItemData());
           }
           else
           {
               if (info.BuildingType == buildingType.Value)
               {
                   info.gameObject.SetActive(true);
                   info.transform.localPosition = new Vector3(0, 0, -3);
                   info.SetCostItemData();
                   info.IsLock = info.SetLock(info.SetItemData());
               }
               else
                   info.gameObject.SetActive(false);
                
           }
           info.GetComponent<UIRollWindowInfomation>().HideWindow();
       }

       foreach (UIAchievementBuildingInfo info in uiIAchievementBuildingInfo)
       {
           if (!buildingType.HasValue)
           {
               info.gameObject.SetActive(true);
               info.SetItemData();
           }
           else
               info.gameObject.SetActive(false);
       }
       

       uiGrid.sorted = true;
       uiGrid.Reposition();
       UIDraggablePanel uiDraggablePanel = NGUITools.FindInParents<UIDraggablePanel>(uiGrid.gameObject);// uiGrid.transform.parent.GetComponent<UIDraggablePanel>();
     
       uiDraggablePanel.transform.localPosition = Vector3.zero;
       UIScrollRegionAdaptive uiScrollRegionAdaptive =NGUITools.FindInParents<UIScrollRegionAdaptive>(uiGrid.gameObject);
       uiScrollRegionAdaptive.OnSize();
       //uiDraggablePanel.MoveAbsolute(Vector3.zero);
    }
    public void BuyBuilding(BuildingType buildingType)
    {
        SceneManager.Instance.DestroyTemporaryBuildingBehavior();
        SceneManager.Instance.UnSelectBuilding(); 
        int initialLevel = ConfigInterface.Instance.BuildingConfigHelper.GetInitialLevel(buildingType);
        BuildingConfigData buildingConfigData = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(buildingType, initialLevel);
        SceneManager.Instance.ConstructBuilding(buildingConfigData, buildingType, false); 
    }
    public void BuyAchievementBuilding(AchievementBuildingType achievementBuildingType)
    {
        SceneManager.Instance.DestroyTemporaryBuildingBehavior();
        SceneManager.Instance.UnSelectBuilding();
        AchievementBuildingConfigData achievementBuildingConfigData = ConfigInterface.Instance.AchievementBuildingConfigHelper.GetAchievementBuildingData(achievementBuildingType);
        SceneManager.Instance.ConstructAchievementBuilding(achievementBuildingConfigData, achievementBuildingType);
    }
}
