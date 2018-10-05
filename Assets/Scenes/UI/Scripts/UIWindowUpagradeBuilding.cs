using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
public class UIWindowUpagradeBuilding : UIWindowCommon {
    [SerializeField] UIWindowItemCommon[] m_UIWindowItemCommon;//progress,information,append,builder,forbid
    [SerializeField] TweenColortk2dSprite m_TweenColortk2dSprite;
    [SerializeField] UIUpgradeBuildingModulAppend m_UIUpgradeBuildingModulAppend;
	// Use this for initialization
    void Awake()
    {
        this.GetTweenComponent();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    protected override void GetTweenComponent()
    {
        base.GetTweenComponent();
    }
    public override void ShowWindow()
    {
        base.ShowWindow();
        SetWindowItem();
        m_TweenColortk2dSprite.duration = 0.2f;
        m_TweenColortk2dSprite.delay = 0;
        m_TweenColortk2dSprite.m_From = new Color(1, 1, 1, 0);
        m_TweenColortk2dSprite.m_To = new Color(1, 1, 1, 1);
        m_TweenColortk2dSprite.Play(true);
        m_UIUpgradeBuildingModulAppend.ShowPanel();
    }
    public override void HideWindow()
    {
        base.HideWindow();
        m_TweenColortk2dSprite.Play(false);
        m_UIUpgradeBuildingModulAppend.HidePanel();
    }
    void SetWindowItem()
    {
        for (int i = 0; i < m_UIWindowItemCommon.Length; i++)
        {
            m_UIWindowItemCommon[i].BuildingLogicData = base.BuildingLogicData;
        }
       //progress,information,append,builder,forbid
       BuildingType buildingType = base.BuildingLogicData.BuildingIdentity.buildingType;
       if (buildingType == BuildingType.CityHall)
       {
           if (LogicController.Instance.CurrentCityHallLevel + base.BuildingLogicData.UpgradeStep <= LogicController.Instance.PlayerData.Level)
           {
               m_UIWindowItemCommon[4].gameObject.SetActive(false);
               m_UIWindowItemCommon[3].gameObject.SetActive(true);
               m_UIWindowItemCommon[3].SetWindowItem();
           }
           else
           {
               m_UIWindowItemCommon[4].gameObject.SetActive(true);
               m_UIWindowItemCommon[4].SetWindowItem();
               m_UIWindowItemCommon[3].gameObject.SetActive(false);
           }
       }
       else
       {
           if (base.BuildingLogicData.Level + base.BuildingLogicData.UpgradeStep <= LogicController.Instance.CurrentCityHallLevel)
           {
               m_UIWindowItemCommon[4].gameObject.SetActive(false);
               m_UIWindowItemCommon[3].gameObject.SetActive(true);
               m_UIWindowItemCommon[3].SetWindowItem();
           }
           else
           {
               m_UIWindowItemCommon[4].gameObject.SetActive(true); 
               m_UIWindowItemCommon[4].SetWindowItem();
               m_UIWindowItemCommon[3].gameObject.SetActive(false);
           }
       }
       if (buildingType == BuildingType.CityHall || buildingType == BuildingType.Barracks || buildingType == BuildingType.Tavern)//|| buildingType == BuildingType.Temple)
       {
           m_UIWindowItemCommon[1].gameObject.SetActive(false);
           m_UIWindowItemCommon[2].gameObject.SetActive(true);
           m_UIWindowItemCommon[2].SetWindowItem();
       }
       else
           if (buildingType == BuildingType.Fortress || buildingType == BuildingType.DefenseTower || buildingType == BuildingType.Artillery)//|| buildingType == BuildingType.Mortar || buildingType == BuildingType.MagicTower)
           {
               m_UIWindowItemCommon[1].gameObject.SetActive(true);
               m_UIWindowItemCommon[2].gameObject.SetActive(false);
               m_UIWindowItemCommon[1].SetWindowItem();
           }
           else
           {
               m_UIWindowItemCommon[1].gameObject.SetActive(false);
               m_UIWindowItemCommon[2].gameObject.SetActive(false);
           }
       m_UIWindowItemCommon[0].SetWindowItem();
    }
}
