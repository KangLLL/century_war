using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities;
using CommonUtilities;
using System;
using ConfigUtilities.Enums;

public class UISearchMuiltiplayerModul : UIWindowItemCommon
{
    [SerializeField] UILabel[] m_UILabel;//0 = Honour ;1 = cost gold
    GuideTrainAramyies m_GuideTrainAramyies = new GuideTrainAramyies();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.m_UILabel[1].text = this.GetFindMatchValue().ToString();
	
	}
    public override void SetWindowItem()
    {
        m_UILabel[0].text = LogicController.Instance.PlayerData.Honour.ToString();
        m_UILabel[1].text = this.GetFindMatchValue().ToString();
    }
    void OnFindMatch(Action function)
    {
        if (LogicController.Instance.AvailableArmies.Count == 0 && LogicController.Instance.AvailableMercenaries.Count == 0)
        {
            UIErrorMessage.Instance.ErrorMessage(9);
            this.m_GuideTrainAramyies.OnGuideTrainAramies();
            return;
        }
        int costGold = this.GetFindMatchValue();
        if (costGold > LogicController.Instance.PlayerData.CurrentStoreGold)
        {
            //UIErrorMessage.Instance.ErrorMessage(8);
            int costGem = MarketCalculator.GetGoldCost(costGold);
            UIManager.Instance.UIWindowCostPrompt.UnRegistDelegate();
            UIManager.Instance.UIWindowCostPrompt.ShowWindow(costGem, string.Format(StringConstants.PROMPT_RESOURCE_COST, costGold - LogicController.Instance.PlayerData.CurrentStoreGold, StringConstants.RESOURCE_GOLD) + StringConstants.QUESTION_MARK, StringConstants.PROMT_REQUEST_RESOURCE + StringConstants.RESOURCE_GOLD);
            UIManager.Instance.UIWindowCostPrompt.Click += () =>
            {
                if (SystemFunction.CostUplimitCheck(costGold, 0, 0))
                {
                    if (LogicController.Instance.PlayerData.CurrentStoreGem  < costGem)
                    {
                        print("宝石不足，去商店");
                        UIManager.Instance.UIWindowFocus = null;
                        //UIManager.Instance.UIButtonShopping.GoShopping();
                        UIManager.Instance.UISelectShopMenu.GoShopping();
                    }
                    else
                    {
                        print("宝石换资源!");
                        LogicController.Instance.BuyGold(costGold); 
                        function.Invoke();
                    }
                }
                else
                    UIErrorMessage.Instance.ErrorMessage(16);
            };
            return;
        }
        function.Invoke();
    }
    void Battle()
    {
        print("Battle");
        LockScreen.Instance.DisableInput();
        UIManager.Instance.CloudBehaviour.FadeIn();
        StartCoroutine("LoadLevelCoroutine");
    }
    IEnumerator LoadLevelCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        Application.LoadLevel(ClientStringConstants.BATTLE_SCENE_LEVEL_NAME);
    }
    int GetFindMatchValue()
    {
        return ConfigInterface.Instance.SystemConfig.FindMatchCost + LogicController.Instance.PlayerData.CityHallLevel * ConfigInterface.Instance.SystemConfig.FindMatchPlusPerCityHallLevel;
    }

    //Temp Click Event
    void OnClick()
    {
        if (LogicController.Instance.PlayerData.RemainingShieldSecond > 0)
        {
            UIManager.Instance.UIWindowEmailChildShield.MissionEvent = () => this.OnFindMatch(this.Battle);
            UIManager.Instance.UIWindowEmailChildShield.ShowWindow();
            return;
        }
        else
            this.OnFindMatch(this.Battle);
    }
 
}
public class GuideTrainAramyies : NewbieGuide
{
    public void OnGuideTrainAramies()
    {
        if (LogicController.Instance.PlayerData.IsNewbie)
            return;
        List<BuildingLogicData> buildingLogicDataList = LogicController.Instance.GetBuildings(BuildingType.Barracks);
        if (buildingLogicDataList.Count > 0)
        {
            BuildingBehavior buildingBehavior = SceneManager.Instance.GetBuildingBehaviorFromObstacleMap(buildingLogicDataList[0].BuildingPosition.Row, buildingLogicDataList[0].BuildingPosition.Column);
            if (buildingBehavior != null)
            {
                NewbieGuideManager.Instance.ClearEventQueue();
                base.CameraFollowTarget(buildingBehavior.BuildingAnchor);
                base.CreateSceneGuideArrow(buildingBehavior.BuildingAnchor.gameObject, ArrowOffsetType.SceneSelectBuilding, ArrowDirection.Down);
                NewbieGuideManager.Instance.Waiting(90, () =>
                {
                    base.DestroyGuideArrow();
                }
               );
            }
        }
    }
}