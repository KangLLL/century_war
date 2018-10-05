using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
public class UIWindowBuildingInfomation : UIWindowCommon
{
    [SerializeField] UIWindowItemCommon[] m_UIWindowItemCommon;//progress,information,append,Description
    [SerializeField] TweenColortk2dSprite m_TweenColortk2dSprite;
    [SerializeField] UIBuildingInformationmMudulAppend m_UIBuildingInformationmMudulAppend;
    // Use this for initialization
    void Awake()
    {
        this.GetTweenComponent();
    }

    // Update is called once per frame
    void Update()
    {

    }
    protected override void GetTweenComponent()
    {
        base.GetTweenComponent();
    }
    public override void ShowWindow()
    {
        base.ShowWindow();
        this.SetWindowItem();
        m_TweenColortk2dSprite.duration = 0.2f;
        m_TweenColortk2dSprite.delay = 0;
        m_TweenColortk2dSprite.m_From = new Color(1, 1, 1, 0);
        m_TweenColortk2dSprite.m_To = new Color(1, 1, 1, 1);
        m_TweenColortk2dSprite.Play(true);
        m_UIBuildingInformationmMudulAppend.ShowPanel();
    }
    public override void HideWindow()
    {
        base.HideWindow();
        m_TweenColortk2dSprite.Play(false);
        m_UIBuildingInformationmMudulAppend.HidePanel();
    }
    void SetWindowItem()
    {
        for (int i = 0; i < m_UIWindowItemCommon.Length; i++)
        {
            m_UIWindowItemCommon[i].BuildingLogicData = base.BuildingLogicData;
        }
        //progress,information,append,Description
        BuildingType buildingType = base.BuildingLogicData.BuildingIdentity.buildingType;
        if (buildingType == BuildingType.ArmyCamp || buildingType == BuildingType.Tavern)//||buildingType== BuildingType.ClanCastle || buildingType == BuildingType.Temple)
        {
            m_UIWindowItemCommon[1].gameObject.SetActive(false); 
            m_UIWindowItemCommon[2].gameObject.SetActive(true);
            m_UIWindowItemCommon[2].SetWindowItem();
        }
        else
        {
            if (buildingType == BuildingType.Fortress || buildingType == BuildingType.DefenseTower || buildingType == BuildingType.Artillery)// || buildingType == BuildingType.Mortar || buildingType == BuildingType.MagicTower)
            {
                m_UIWindowItemCommon[1].gameObject.SetActive(true);
                m_UIWindowItemCommon[1].SetWindowItem();
                m_UIWindowItemCommon[2].gameObject.SetActive(false);
            }
            else
            {
                m_UIWindowItemCommon[1].gameObject.SetActive(false);
                m_UIWindowItemCommon[2].gameObject.SetActive(false);
            }
        }
        m_UIWindowItemCommon[0].SetWindowItem();
        m_UIWindowItemCommon[3].SetWindowItem();
    }
    public void ShowWindowAchievementBuilding()
    {
        base.ShowWindow();
        this.SetWindowItemAchievementBuilding();
        m_TweenColortk2dSprite.duration = 0.2f;
        m_TweenColortk2dSprite.delay = 0;
        m_TweenColortk2dSprite.m_From = new Color(1, 1, 1, 0);
        m_TweenColortk2dSprite.m_To = new Color(1, 1, 1, 1);
        m_TweenColortk2dSprite.Play(true);
        m_UIBuildingInformationmMudulAppend.ShowPanel();
    }
    void SetWindowItemAchievementBuilding()
    {
        for (int i = 0; i < m_UIWindowItemCommon.Length; i++)
        {
            m_UIWindowItemCommon[i].AchievementBuildingLogicData = base.AchievementBuildingLogicData;
        }
        m_UIWindowItemCommon[0].gameObject.SetActive(true);
        m_UIWindowItemCommon[1].gameObject.SetActive(false);
        m_UIWindowItemCommon[2].gameObject.SetActive(false);
        m_UIWindowItemCommon[3].gameObject.SetActive(true);
        m_UIWindowItemCommon[0].SetWindowItemAchievementBuilding();
        m_UIWindowItemCommon[3].SetWindowItemAchievementBuilding();
    }
}
