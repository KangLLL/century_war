using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
using ConfigUtilities;

public class StatisticsValidBuildingCount : MonoBehaviour
{
    [SerializeField] UILabel m_UILabel;
    [SerializeField] BuildingResourceType m_BuildingResourceType;
    [SerializeField] UISprite m_UISprite;//Background icon
    int m_CurrentCityHallLevel = -1;
    int m_CurrentGold = -1;
    int m_CurrentFood = -1;
    int m_CurrentGem = -1;
	// Use this for initialization

	
	// Update is called once per frame
	void Update () 
    {
        this.StatisticsCount();
	}
    void StatisticsCount()
    {
        if (this.m_CurrentCityHallLevel != LogicController.Instance.CurrentCityHallLevel||
            this.m_CurrentGold != LogicController.Instance.PlayerData.CurrentStoreGold||
            this.m_CurrentFood != LogicController.Instance.PlayerData.CurrentStoreFood||
            this.m_CurrentGem != LogicController.Instance.PlayerData.CurrentStoreGem)
        {
            switch (this.m_BuildingResourceType)
            {
                case BuildingResourceType.All:
                    {
                        int count = this.GetCompsiteCount() + this.GetDefenseCount() + this.GetMilitaryCount() + this.GetResourceCount();
                        if (count > 0)
                        {
                            ActiveSelf(true);
                            m_UILabel.text = count.ToString();
                        }
                        else
                            ActiveSelf(false);
                        break;
                    }
                case BuildingResourceType.Composite:
                    {
                        int count = this.GetCompsiteCount();
                        if (count > 0)
                        {
                            ActiveSelf(true);
                            m_UILabel.text = count.ToString();
                        }
                        else
                            ActiveSelf(false);
                        break;
                    }
                case BuildingResourceType.Defense:
                    {
                        int count = this.GetDefenseCount();
                        if (count > 0)
                        {
                            ActiveSelf(true);
                            m_UILabel.text = count.ToString();
                        }
                        else
                            ActiveSelf(false);
                        break;
                    }
                case BuildingResourceType.Military:
                    {
                        int count = this.GetMilitaryCount();
                        if (count > 0)
                        {
                            ActiveSelf(true);
                            m_UILabel.text = count.ToString();
                        }
                        else
                            ActiveSelf(false);
                        break;
                    }
                case BuildingResourceType.Resource:
                    {
                        int count = this.GetResourceCount();
                        if (count > 0)
                        {
                            ActiveSelf(true);
                            m_UILabel.text = count.ToString();
                        }
                        else
                            ActiveSelf(false);
                        break;
                    }
            }
            this.m_CurrentCityHallLevel = LogicController.Instance.CurrentCityHallLevel;
        }
    }
    int GetCompsiteCount()
    {
        int count = ConfigInterface.Instance.BuildingNumberRestrictionsConfigHelper.GetBuildingNumberRestrictions(LogicController.Instance.CurrentCityHallLevel).RestrictionDict[BuildingType.PropsStorage] - LogicController.Instance.GetBuildingCount(BuildingType.PropsStorage);
        count += ConfigInterface.Instance.BuildingNumberRestrictionsConfigHelper.GetBuildingNumberRestrictions(LogicController.Instance.CurrentCityHallLevel).RestrictionDict[BuildingType.Tavern] - LogicController.Instance.GetBuildingCount(BuildingType.Tavern);
        return count;
    }
    int GetDefenseCount()
    {
        int count = ConfigInterface.Instance.BuildingNumberRestrictionsConfigHelper.GetBuildingNumberRestrictions(LogicController.Instance.CurrentCityHallLevel).RestrictionDict[BuildingType.Fortress] - LogicController.Instance.GetBuildingCount(BuildingType.Fortress);
        count += ConfigInterface.Instance.BuildingNumberRestrictionsConfigHelper.GetBuildingNumberRestrictions(LogicController.Instance.CurrentCityHallLevel).RestrictionDict[BuildingType.DefenseTower] - LogicController.Instance.GetBuildingCount(BuildingType.DefenseTower);
        count += ConfigInterface.Instance.BuildingNumberRestrictionsConfigHelper.GetBuildingNumberRestrictions(LogicController.Instance.CurrentCityHallLevel).RestrictionDict[BuildingType.Wall] - LogicController.Instance.GetBuildingCount(BuildingType.Wall);
        return count;
    }
    int GetMilitaryCount()
    {
        int count = ConfigInterface.Instance.BuildingNumberRestrictionsConfigHelper.GetBuildingNumberRestrictions(LogicController.Instance.CurrentCityHallLevel).RestrictionDict[BuildingType.Barracks] - LogicController.Instance.GetBuildingCount(BuildingType.Barracks);
        count += ConfigInterface.Instance.BuildingNumberRestrictionsConfigHelper.GetBuildingNumberRestrictions(LogicController.Instance.CurrentCityHallLevel).RestrictionDict[BuildingType.ArmyCamp] - LogicController.Instance.GetBuildingCount(BuildingType.ArmyCamp);
        return count;
    }
    int GetResourceCount()
    {
        int count = ConfigInterface.Instance.BuildingNumberRestrictionsConfigHelper.GetBuildingNumberRestrictions(LogicController.Instance.CurrentCityHallLevel).RestrictionDict[BuildingType.GoldMine] - LogicController.Instance.GetBuildingCount(BuildingType.GoldMine);
        count += ConfigInterface.Instance.BuildingNumberRestrictionsConfigHelper.GetBuildingNumberRestrictions(LogicController.Instance.CurrentCityHallLevel).RestrictionDict[BuildingType.Farm] - LogicController.Instance.GetBuildingCount(BuildingType.Farm);
        count += ConfigInterface.Instance.BuildingNumberRestrictionsConfigHelper.GetBuildingNumberRestrictions(LogicController.Instance.CurrentCityHallLevel).RestrictionDict[BuildingType.GoldStorage] - LogicController.Instance.GetBuildingCount(BuildingType.GoldStorage);
        count += ConfigInterface.Instance.BuildingNumberRestrictionsConfigHelper.GetBuildingNumberRestrictions(LogicController.Instance.CurrentCityHallLevel).RestrictionDict[BuildingType.FoodStorage] - LogicController.Instance.GetBuildingCount(BuildingType.FoodStorage);
        return count;
    }
    void ActiveSelf(bool isActive)
    {
        this.m_UISprite.alpha = isActive ? 1 : 0;
        this.m_UILabel.alpha = isActive ? 1 : 0;
    }
}
public enum BuildingResourceType
{
    All,
    Composite,
    Defense,
    Military,
    Resource
}