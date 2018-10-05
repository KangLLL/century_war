using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class UIWindowMain : MonoBehaviour {

    [SerializeField] UIExpProgressBar m_UIExpProgressBar;
    [SerializeField] UIExpProgressBar m_UIGoldProgressBar;
    [SerializeField] UIExpProgressBar m_UIFoodProgressBar;
    [SerializeField] UIExpProgressBar m_UIHonourBar;
    [SerializeField] UIExpProgressBar m_UIGemBar;
    [SerializeField] UIExpProgressBar m_UIBuilderBar;
    [SerializeField] UIExpProgressBar m_UIShieldBar;
    public int CurrentStoreGold { get; set; }
    public int CurrentStoreFood { get; set; }
    public int CurrentStoreGem { get; set; }
	// Update is called once per frame
	void Update () 
    {
        this.SetUserExpBar();
        this.SetUserHonourBar();
        if (SceneManager.Instance.SceneMode == SceneMode.SceneVisit)
            return;
        this.SetUserFoodBar();
        this.SetUserGoldBar();
        this.SetUserGemBar();
        this.SerBuilderBar();
        this.SetShieldBar();
	}
    public void SetUserExpBar()
    {
        PlayerLogicData playerData = null;
        int cityHallLevel = 0;
        switch (SceneManager.Instance.SceneMode)
        {
            case SceneMode.SceneBuild:
                playerData = LogicController.Instance.PlayerData;
                cityHallLevel = LogicController.Instance.GetBuildingObject(new BuildingIdentity(BuildingType.CityHall, 0)).Level;
                break;
            case SceneMode.SceneVisit:
                playerData = LogicController.Instance.CurrentFriend.PlayerData;
                cityHallLevel = LogicController.Instance.CurrentFriend.GetBuildingData(new BuildingIdentity(BuildingType.CityHall, 0)).Level;
                break;
        }

        if (!playerData.IsExpMaximum)
            m_UIExpProgressBar.SetProgressBar((float)playerData.Exp / playerData.CurrentLevelMaxExp, playerData.Exp);
        else
            m_UIExpProgressBar.SetProgressBar(1, StringConstants.PROMPT_MAX_LEVEL);
		
        m_UIExpProgressBar.SetText(playerData.Level.ToString(), playerData.Name, ClientConfigConstants.Instance.GetAgeName(CommonUtilities.CommonUtilities.GetAgeFromCityHallLevel(cityHallLevel)));
    }

    public void SetUserGoldBar()
    {
        m_UIGoldProgressBar.SetProgressBar((float)LogicController.Instance.PlayerData.CurrentStoreGold / LogicController.Instance.PlayerData.GoldMaxCapacity, LogicController.Instance.PlayerData.CurrentStoreGold);
        m_UIGoldProgressBar.SetText(LogicController.Instance.PlayerData.GoldMaxCapacity.ToString());
    }
    public void SetUserFoodBar()
    {
        m_UIFoodProgressBar.SetProgressBar((float)LogicController.Instance.PlayerData.CurrentStoreFood / LogicController.Instance.PlayerData.FoodMaxCapacity, LogicController.Instance.PlayerData.CurrentStoreFood);
        m_UIFoodProgressBar.SetText(LogicController.Instance.PlayerData.FoodMaxCapacity.ToString());
    }
    public void SetUserHonourBar()
    {
        PlayerLogicData playerData = null; 
        switch (SceneManager.Instance.SceneMode)
        {
            case SceneMode.SceneBuild:
                playerData = LogicController.Instance.PlayerData; 
                break;
            case SceneMode.SceneVisit:
                playerData = LogicController.Instance.CurrentFriend.PlayerData; 
                break;
        }
        m_UIHonourBar.SetText(playerData.Honour.ToString());
    }
    public void SetUserGemBar()
    {
        m_UIGemBar.SetProgressBar(0, LogicController.Instance.PlayerData.CurrentStoreGem);
        //m_UIGemBar.SetText(LogicController.Instance.PlayerData.CurrentStoreGem.ToString());
    }
    public void SerBuilderBar()
    {
        m_UIBuilderBar.SetText(LogicController.Instance.IdleBuilderNumber + " / " + LogicController.Instance.AvailableBuilderNumber);
    }
    public void SetShieldBar()
    {
        if (LogicController.Instance.PlayerData.RemainingShieldSecond > 0)
            m_UIShieldBar.SetText(SystemFunction.TimeSpanToString(LogicController.Instance.PlayerData.RemainingShieldSecond));
        else
            m_UIShieldBar.SetText(StringConstants.WITHOUT);
        
    }
}
