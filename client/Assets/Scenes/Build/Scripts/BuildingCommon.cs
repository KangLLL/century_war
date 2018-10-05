using UnityEngine;
using System.Collections;
using System;
using ConfigUtilities.Enums;
using System.Collections.Generic;
using ConfigUtilities;
using CommonUtilities;
public class BuildingCommon : MonoBehaviour {
    public BuildingLogicData BuildingLogicData { get; set; }
    public BuildingConfigData BuildingConfigData { get; set; }
    public BuildingBehavior BuildingBehavior { get; set; }
 
    public Vector2 ProgressBarOffset { get; set; }
    public Vector2 ProgressBarSize { get; set; }
    GameObject m_BuildingFacility;
    public string FacilityName { get; set; }
 
    protected const string PROGRESS_PREFAB_NAME = "BuildingScene/Common/Progress Bar";
    protected SortedDictionary<int, ProgressBarBehavior> m_ProgressBarBehaviorDictionary = new SortedDictionary<int, ProgressBarBehavior>();//4 = Building upgrade;3 = ProduceArmy; 2 = UpgradeArmy;1 = ProduceSpell; 0 = UpgradeSpell;
	//TweenColortk2dSprite m_TweenColorBuildingBk;
    GameObject m_ReadyForUpgradeFX;
    // Use this for initialization
    void Awake()
    {
        this.GetComponent();
    }
	protected virtual void Start () 
    {
        this.CreateFacility();
        this.CreateComponent();
        this.Initial();
	}
    public virtual void OnClick()
    {
        //print("BuildingCommon OnClick");
    }
	// Update is called once per frame
	void Update () {
        
	}

    protected virtual void GetComponent()
    {
        //m_TweenColorBuildingBk = this.transform.FindChild(ClientSystemConstants.BACKGROUND_NAME).GetComponent<TweenColortk2dSprite>();
    }
    //public void OnActiveBuildingFX(bool active)
    //{
    //    if (active)
    //    {
    //        iTween.ScaleTo(m_TweenColorBuildingBk.gameObject, iTween.Hash(iT.ScaleTo.scale, new Vector3(1.15f, 1.15f, 1), iT.MoveTo.easetype, iTween.EaseType.easeOutQuad, iT.ScaleTo.looptype, iTween.LoopType.pingPong, iT.ScaleTo.time, 0.1f, iT.MoveTo.islocal, true));
    //        iTween.MoveTo(m_TweenColorBuildingBk.gameObject, iTween.Hash(iT.MoveTo.position, new Vector3(0, 15, 0), iT.MoveTo.easetype, iTween.EaseType.easeOutQuad, iT.MoveTo.looptype, iTween.LoopType.pingPong, iT.MoveTo.time, 0.1f, iT.MoveTo.islocal, true));
    //        iTween.RotateTo(this.gameObject, iTween.Hash(iT.RotateTo.rotation, Vector3.zero, iT.RotateTo.oncomplete, "OnCompleteMoveTo", iT.RotateTo.delay, 0.2f, iT.RotateTo.time, 0.0f));

    //        m_TweenColorBuildingBk.delay = 0;
    //        m_TweenColorBuildingBk.duration = 0.8f;
    //        m_TweenColorBuildingBk.m_From = Color.white;
    //        m_TweenColorBuildingBk.m_To = Color.gray;
    //        m_TweenColorBuildingBk.style = UITweener.Style.PingPong;
    //        m_TweenColorBuildingBk.Play(true);
    //    }
    //    else
    //    {
    //        m_TweenColorBuildingBk.Reset();
    //        m_TweenColorBuildingBk.Color = Color.white;
    //        m_TweenColorBuildingBk.enabled = false;
    //    }
    //}
    //void OnCompleteMoveTo()
    //{
    //    iTween.Stop(m_TweenColorBuildingBk.gameObject);
    //    m_TweenColorBuildingBk.gameObject.transform.localScale = Vector3.one;
    //    m_TweenColorBuildingBk.gameObject.transform.localPosition = Vector3.zero;
    //}
    protected virtual void Initial()
    {
        foreach (KeyValuePair<int,ProgressBarBehavior> pb in m_ProgressBarBehaviorDictionary)
        {
            pb.Value.ProgressBarOffset = this.ProgressBarOffset;
            pb.Value.ProgressBarSize = this.ProgressBarSize;
            pb.Value.transform.parent = this.transform;
            pb.Value.gameObject.SetActive(false);
        } 
    }
    protected virtual void ShowProgress()
    {
        if (!this.BuildingBehavior.Created)
            return;
        if (SceneManager.Instance.SceneMode == SceneMode.SceneVisit)
        {
            if(this.BuildingLogicData.CurrentBuilidngState != BuildingEditorState.Normal)
                this.ActiveFacility(true);
            else
                this.ActiveFacility(false);
            return;
        }
        if (this.BuildingLogicData.CurrentBuilidngState == BuildingEditorState.Update)
        {
            this.ActiveFacility(true);
            foreach (KeyValuePair<int, ProgressBarBehavior> pb in m_ProgressBarBehaviorDictionary)
            {
                if (pb.Key == 4)
                {
                    pb.Value.gameObject.SetActive(true);
                    pb.Value.SetProgressPosition(0);
                    pb.Value.SetProgressBar((this.BuildingLogicData.UpgradeWorkload - this.BuildingLogicData.UpgradeRemainingWorkload) / this.BuildingLogicData.UpgradeWorkload, this.BuildingLogicData.UpgradeRemainingWorkload / this.BuildingLogicData.AttachedBuilderEfficiency, false, string.Empty);

                }
                else
                    pb.Value.gameObject.SetActive(false);
            }
        }
        else
        {
            this.ActiveFacility(false);
            m_ProgressBarBehaviorDictionary[4].gameObject.SetActive(false);
            if (this.BuildingLogicData.CurrentBuilidngState == BuildingEditorState.Normal)
            {
                int order = 0;
                foreach (KeyValuePair<int, ProgressBarBehavior> pb in m_ProgressBarBehaviorDictionary)
                {
                    switch (pb.Key)
                    {
                        case 0://0 = UpgradeSpell
                            if (this.BuildingLogicData.ItemUpgrade.HasValue)
                            {
                                pb.Value.gameObject.SetActive(true);
                                pb.Value.SetProgressPosition(order);
                                float remainingTime = this.BuildingLogicData.ItemUpgradeRemainingTime;
                                float progress = (this.BuildingLogicData.ItemUpgradeTotalWorkload - this.BuildingLogicData.ItemUpgradeRemainingWorkload) / (float)this.BuildingLogicData.ItemUpgradeTotalWorkload;
                                pb.Value.SetProgressBar(progress, remainingTime, true, ClientSystemConstants.Spell_ICON_COMMON_DICTIONARY[this.BuildingLogicData.ItemUpgrade.Value]);
                                order++;
                            }
                            else
                            {
                                pb.Value.gameObject.SetActive(false);
                            }
                            break;
                        case 1://ProduceSpell
                            if (this.BuildingLogicData.ItemProducts != null)
                            {
                                if (this.BuildingLogicData.ItemProducts.Count > 0)
                                {
                                    pb.Value.gameObject.SetActive(true);
                                    pb.Value.SetProgressPosition(order);
                                    ItemIdentity itemIdentity = this.BuildingLogicData.ItemProducts[0].Value[0];
                                    ItemLogicObject itemLogicObject = LogicController.Instance.GetItemObject(itemIdentity);
                                    float remainingTime = (float)itemLogicObject.ProduceRemainingWorkload / ConfigInterface.Instance.SystemConfig.ProduceItemEfficiency;
                                    float progress = (itemLogicObject.ProduceTotalWorkload - itemLogicObject.ProduceRemainingWorkload) / (float)itemLogicObject.ProduceTotalWorkload;
                                    pb.Value.SetProgressBar(progress, remainingTime, true, ClientSystemConstants.Spell_ICON_COMMON_DICTIONARY[itemIdentity.itemType]);
                                    order++;
                                }
                                else
                                {
                                    pb.Value.gameObject.SetActive(false);
                                }
                            }
                            
                            break;
                        case 2://2 = UpgradeArmy
                            if (this.BuildingLogicData.ArmyUpgrade.HasValue)
                            {
                                if (this.BuildingLogicData.IsArmyProduceBlock && !this.BuildingBehavior.IsClick)
                                    order++;
                                pb.Value.gameObject.SetActive(true);
                                pb.Value.SetProgressPosition(order);
                                float remainingTime = this.BuildingLogicData.ArmyUpgradeRemainingTime;// (float)this.BuildingLogicData.ArmyUpgradeRemainWorkload / this.BuildingLogicData.ArmyUpgradeEfficiency;
                                float progress = (this.BuildingLogicData.ArmyUpgradeTotalWorkload - this.BuildingLogicData.ArmyUpgradeRemainWorkload) / (float)this.BuildingLogicData.ArmyUpgradeTotalWorkload;
                                pb.Value.SetProgressBar(progress, remainingTime, true, ClientSystemConstants.ARMY_UPGRADE_ICON_COMMON_DICTIONARY[this.BuildingLogicData.ArmyUpgrade.Value]);
                                order++;
                            }
                            else
                            {
                                pb.Value.gameObject.SetActive(false);
                            }
                            break;
                        case 3://ProduceArmy
                            if (this.BuildingLogicData.ArmyProducts != null && !this.BuildingLogicData.IsArmyProduceBlock)
                            {
                                if (this.BuildingLogicData.ArmyProducts.Count > 0)
                                {
                                    pb.Value.gameObject.SetActive(true);
                                    pb.Value.SetProgressPosition(order);
                                    ArmyIdentity armyIdentity = this.BuildingLogicData.ArmyProducts[0].Value[0];
                                    ArmyLogicData armyLogicData = LogicController.Instance.GetArmyObjectData(armyIdentity);
                                    float remainingTime = (float)armyLogicData.ProduceRemainingWorkload / ConfigInterface.Instance.SystemConfig.ProduceArmyEfficiency;
                                    
                                    float progress = (armyLogicData.ProduceTotalWorkload - armyLogicData.ProduceRemainingWorkload) / (float)armyLogicData.ProduceTotalWorkload;
                                    pb.Value.SetProgressBar(progress, remainingTime, true, ClientSystemConstants.ARMY_ICON_COMMON_DICTIONARY[armyIdentity.armyType]);
                                    order++;
                                }
                                else
                                    pb.Value.gameObject.SetActive(false);
                            }
                            else
                            {
                                pb.Value.gameObject.SetActive(false);
                            }
                           
                            break;
                    }
                }
            }
        }
    }
 
    protected virtual void CreateFacility()
    {
        if (this.BuildingBehavior.BuildingType != BuildingType.Wall && this.BuildingBehavior.BuildingType != BuildingType.BuilderHut)
        {
            GameObject buildingFacilityPrefab = Resources.Load(ClientStringConstants.BUILDING_SCENE_RESOURCE_PREFAB_PREFIX_NAME + ClientStringConstants.BUILDING_OBJECT_PREFAB_PREFIX_NAME + ClientStringConstants.FACILITIES_OBJECT_PREFAB_PREFIX_NAME + FacilityName, typeof(GameObject)) as GameObject;
            this.m_BuildingFacility = GameObject.Instantiate(buildingFacilityPrefab) as GameObject;
            Vector3 localPosition = this.m_BuildingFacility.transform.position;
            this.m_BuildingFacility.transform.parent = this.transform.FindChild(ClientStringConstants.BUILDING_ANCHOR_OBJECT_NAME);
            this.m_BuildingFacility.transform.localPosition = localPosition;
        }
    }
    protected virtual void ActiveFacility(object param)
    {
       this.m_BuildingFacility.SetActive((bool)param);
    }

 
    protected void ShowWindowBuildingInfomation()
    {
        UIManager.Instance.UIWindowBuildingInfomation.BuildingLogicData = this.BuildingLogicData;
        UIManager.Instance.UIWindowBuildingInfomation.ShowWindow();
    }
    protected virtual void CreateComponent()
    {
        GameObject progressBarBehaviorGo = Resources.Load(PROGRESS_PREFAB_NAME, typeof(GameObject)) as GameObject;
        m_ProgressBarBehaviorDictionary.Add(4, (Instantiate(progressBarBehaviorGo) as GameObject).GetComponent<ProgressBarBehavior>()); 
        #region Store
        if (this.BuildingConfigData.CanStoreGold)
        {
            StoreGold storeGold = this.gameObject.AddComponent<StoreGold>();
            storeGold.BuildingLogicData = this.BuildingLogicData;
            storeGold.BuildingBehavior = this.BuildingBehavior;
        }
        if (this.BuildingConfigData.CanStoreOil)
        {
            StoreOil storeOil = this.gameObject.AddComponent<StoreOil>();
            storeOil.BuildingLogicData = this.BuildingLogicData;
            storeOil.BuildingBehavior = this.BuildingBehavior;
        }
        if (this.BuildingConfigData.CanStoreFood)
        {
            StoreFood storeFood = this.gameObject.AddComponent<StoreFood>();
            storeFood.BuildingLogicData = this.BuildingLogicData;
            storeFood.BuildingBehavior = this.BuildingBehavior;
        }
        if (this.BuildingConfigData.CanStoreItem)
        {
            StoreItem storeItem = this.gameObject.AddComponent<StoreItem>();
            storeItem.BuildingLogicData = this.BuildingLogicData;
            storeItem.BuildingBehavior = this.BuildingBehavior;
        }
        #endregion

        #region Collection
        if (this.BuildingConfigData.CanProduceFood)
        {
            CollectFood collectFood = this.gameObject.AddComponent<CollectFood>();
            collectFood.BuildingLogicData = this.BuildingLogicData;
            collectFood.BuildingBehavior = this.BuildingBehavior;
        }
        if (this.BuildingConfigData.CanProduceGold)
        {
            CollectGold collectGold = this.gameObject.AddComponent<CollectGold>();
            collectGold.BuildingLogicData = this.BuildingLogicData;
            collectGold.BuildingBehavior = this.BuildingBehavior;
        }
        if (this.BuildingConfigData.CanProduceOil)
        {
            CollectOil collectOil = this.gameObject.AddComponent<CollectOil>();
            collectOil.BuildingLogicData = this.BuildingLogicData;
            collectOil.BuildingBehavior = this.BuildingBehavior;
        }
        #endregion
        #region Produce
        if (this.BuildingConfigData.CanProduceArmy)
        {
            ProduceArmy produceArmy = this.gameObject.AddComponent<ProduceArmy>();
            produceArmy.BuildingLogicData = this.BuildingLogicData;
            m_ProgressBarBehaviorDictionary.Add(3,(Instantiate(progressBarBehaviorGo) as GameObject).GetComponent<ProgressBarBehavior>());
            produceArmy.BuildingBehavior = this.BuildingBehavior;
        }

        if (this.BuildingConfigData.CanProduceItem)
        {
            ProduceItem produceItem = this.gameObject.AddComponent<ProduceItem>();
            produceItem.BuildingLogicData = this.BuildingLogicData;
            m_ProgressBarBehaviorDictionary.Add(1,(Instantiate(progressBarBehaviorGo) as GameObject).GetComponent<ProgressBarBehavior>());
            produceItem.BuildingBehavior = this.BuildingBehavior;
        }

        #endregion

        #region Upgrade
        if (this.BuildingConfigData.CanUpgradeArmy)
        {
            UpgradeArmy upgradeArmy = this.gameObject.AddComponent<UpgradeArmy>();
            upgradeArmy.BuildingLogicObject = this.BuildingLogicData;
            m_ProgressBarBehaviorDictionary.Add(2, (Instantiate(progressBarBehaviorGo) as GameObject).GetComponent<ProgressBarBehavior>());
            upgradeArmy.BuildingBehavior = this.BuildingBehavior;
        }
        if (this.BuildingConfigData.CanUpgradeItem)
        {
            UpgradeItem upgradeItem = this.gameObject.AddComponent<UpgradeItem>();
            upgradeItem.BuildingLogicObject = this.BuildingLogicData;
            m_ProgressBarBehaviorDictionary.Add(0, (Instantiate(progressBarBehaviorGo) as GameObject).GetComponent<ProgressBarBehavior>());
            upgradeItem.BuildingBehavior = this.BuildingBehavior;
        }
        #endregion
    }
    public void BuyBuilding(int builderNo)
    {
        switch (this.BuildingBehavior.BuildingType)
        {
            case BuildingType.Wall:
                this.ConstructEvent(new Action(BuyWall));
                break;
            default:
                AudioController.Play("BuildingConstruct");
                this.BuildingLogicData = LogicController.Instance.GetBuildingObject(LogicController.Instance.ConstructBuilding(this.BuildingBehavior.BuildingType, builderNo, this.BuildingBehavior.FirstZoneIndex));
                this.ConstructBuilding();
                break;
        }

    }
    public void BuyBuilderHut()
    {
        BuildingIdentity buildingIdentity = UIManager.Instance.UIWindowBuyBuilding.BuilerNo >= 0 ? LogicController.Instance.BuyBuilderHut(UIManager.Instance.UIWindowBuyBuilding.BuilerNo, this.BuildingBehavior.FirstZoneIndex) : LogicController.Instance.BuyBuilderHut(this.BuildingBehavior.FirstZoneIndex);
        this.BuildingLogicData = LogicController.Instance.GetBuildingObject(buildingIdentity);
        this.BuildingBehavior.BuildingLogicData = this.BuildingLogicData;
        BuildingBehavior bb = this.ConstructBuilding();
        bb.transform.position = PositionConvertor.GetWorldPositionByBuildingTileIndex(this.BuildingBehavior.BuildingLogicData.BuildingPosition);
     
        SceneManager.Instance.CreateUpgradeFX(bb);
    }
    public void BuyWall()
    {
        BuildingIdentity buildingIdentity = LogicController.Instance.BuyWall(this.BuildingBehavior.FirstZoneIndex);
        this.BuildingLogicData = LogicController.Instance.GetBuildingObject(buildingIdentity);
        SceneManager.Instance.EnableCreateWallContinuation = true;
       
        this.ConstructBuilding();
        SceneManager.Instance.LastWallTilePosition = this.BuildingBehavior.FirstZoneIndex;
        //if (this.BuildingLogicData.BuildingType == BuildingType.Wall)
        // {
        int upperLimitCount = ConfigInterface.Instance.BuildingNumberRestrictionsConfigHelper.GetBuildingNumberRestrictions(LogicController.Instance.CurrentCityHallLevel).RestrictionDict[this.BuildingLogicData.BuildingType];
        int hasCount = LogicController.Instance.GetBuildingCount(this.BuildingLogicData.BuildingType);
        if (hasCount < upperLimitCount)
        {
            int initialLevel = ConfigInterface.Instance.BuildingConfigHelper.GetInitialLevel(this.BuildingLogicData.BuildingType);
            BuildingConfigData buildingConfigData = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(this.BuildingLogicData.BuildingType, initialLevel);
            SceneManager.Instance.ConstructBuilding(buildingConfigData, this.BuildingLogicData.BuildingType, false);
        }
        else 
            UIErrorMessage.Instance.ErrorMessage(2);
        // }

    }
    void UpgradeBuilderHut()
    {
        this.CreateUpgradeFX();
        LogicController.Instance.BuyUpgradeBuilderHut(this.BuildingLogicData.BuildingIdentity.buildingNO);
        UIManager.Instance.HidePopuBtnByCurrentSelect(true);
        this.ConstructBuilding();
    }
    void UpgradeWall()
    {
        this.CreateUpgradeFX();
        LogicController.Instance.BuyUpgradeWall(this.BuildingLogicData.BuildingIdentity.buildingNO);
        UIManager.Instance.HidePopuBtnByCurrentSelect(true);
        
        this.ConstructBuilding();
    }
    public BuildingBehavior ConstructBuilding()
    {
        SceneManager.Instance.PickableObjectCurrentSelect = null;
        Destroy(this.gameObject);
        this.BuildingBehavior.enabled = false;
        return SceneManager.Instance.ConstructBuilding(this.BuildingLogicData, true);
    }
    //Button message
    protected virtual void OnConstructBuilding()
    {
        if (this.BuildingBehavior.EnableCreate)
        {
            switch (this.BuildingBehavior.BuildingType)
            {
                case BuildingType.BuilderHut:
                    this.ConstructEvent(new Action(BuyBuilderHut));
                    break;
                case BuildingType.Wall:
                    if (LogicController.Instance.IdleBuilderNumber > 0)
                        this.ConstructEvent(new Action(BuyWall));
                    else
                        this.ShowSelectBuilderWindow(BuilderMenuType.Construct);
                    break;
                default:
                    this.ShowSelectBuilderWindow(BuilderMenuType.Construct);
                    break;
            }
        }
    }
    //Button message
    public void OnUpgradeBuilding()
    {
        switch (this.BuildingLogicData.BuildingIdentity.buildingType)
        {
            case BuildingType.BuilderHut:
                if (this.BuildingLogicData.Level <= LogicController.Instance.CurrentCityHallLevel - this.BuildingLogicData.UpgradeStep)
                    this.ConstructEvent(new Action(UpgradeBuilderHut));
                else
                    UIErrorMessage.Instance.ErrorMessage(0, ClientSystemConstants.BUILDING_NAME_DICTIONARY[BuildingType.CityHall], (this.BuildingLogicData.Level + this.BuildingLogicData.UpgradeStep).ToString());
                break;
            case BuildingType.Wall:
                if (this.BuildingLogicData.Level <= LogicController.Instance.CurrentCityHallLevel - this.BuildingLogicData.UpgradeStep)
                {
                    if (LogicController.Instance.IdleBuilderNumber > 0)
                        this.ConstructEvent(new Action(UpgradeWall));
                    else
                        this.ShowSelectBuilderWindow(BuilderMenuType.Upgrade);
                }
                else
                    UIErrorMessage.Instance.ErrorMessage(0, ClientSystemConstants.BUILDING_NAME_DICTIONARY[BuildingType.CityHall], (this.BuildingLogicData.Level + this.BuildingLogicData.UpgradeStep).ToString());
                break;
            default:
                UIManager.Instance.UIWindowUpagradeBuilding.BuildingLogicData = this.BuildingLogicData;
                UIManager.Instance.UIWindowUpagradeBuilding.ShowWindow();
                break;
        }
    }
    void ShowSelectBuilderWindow(BuilderMenuType builderMenuType)
    {
        BuildingConfigData buildingConfigData = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(this.BuildingBehavior.BuildingType, 0);

        UIManager.Instance.UIWindowSelectBuilder.BuildingConfigData = buildingConfigData;
      
        UIManager.Instance.UIWindowSelectBuilder.BuildingLogicData = this.BuildingLogicData;
        UIManager.Instance.UIWindowSelectBuilder.BuilderMenuType = builderMenuType;
        UIManager.Instance.UIWindowSelectBuilder.BuildingBehavior = this.BuildingBehavior;
        UIManager.Instance.UIWindowSelectBuilder.ShowWindow();
        this.BuildingBehavior.ActiveButton(false);
    }
    void ConstructEvent(Action function)
    {
        BuildingConfigData buildingConfigData = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(this.BuildingBehavior.BuildingType, 0);
        Dictionary<CostType, int> costBalance = this.BuildingBehavior.Created ? SystemFunction.UpgradeCostBalance(this.BuildingLogicData) : SystemFunction.UpgradeCosBalance(buildingConfigData);
        Dictionary<CostType, int> costTotal = this.BuildingBehavior.Created ? SystemFunction.UpgradeTotalCost(this.BuildingLogicData) : SystemFunction.UpgradeTotalCost(buildingConfigData);
        int costGem = this.BuildingBehavior.Created ? this.BuildingLogicData.UpgradeGem : buildingConfigData.UpgradeGem;
        if (costBalance.Count > 0)
        {
            int costBalanceGem = SystemFunction.ResourceConvertToGem(costBalance);
            //int costResourceToGem = SystemFunction.ResourceConvertToGem(costTotal) - costGem;

            string resourceContext = SystemFunction.BuyReusoureContext(costBalance);
            UIManager.Instance.UIWindowCostPrompt.ShowWindow(costBalanceGem, resourceContext, SystemFunction.BuyReusoureTitle(costBalance));
            UIManager.Instance.UIWindowCostPrompt.UnRegistDelegate();
            UIManager.Instance.UIWindowCostPrompt.Click += () =>
            {
                if (SystemFunction.CostUplimitCheck(costTotal[CostType.Gold], costTotal[CostType.Food], costTotal[CostType.Oil]))
                {
                    if (LogicController.Instance.PlayerData.CurrentStoreGem - costBalanceGem/*costResourceToGem*/ < costGem)
                    {
                        print("宝石不足，去商店");
                        UIManager.Instance.UIWindowFocus = null;
                        //UIManager.Instance.UIButtonShopping.GoShopping();
                        UIManager.Instance.UISelectShopMenu.GoShopping();
                    }
                    else
                    {
                        print("宝石换资源!");
                        SystemFunction.BuyResources(costBalance);
                        function.Invoke();
                    }
                }
                else
                    UIErrorMessage.Instance.ErrorMessage(16);
            };
        }
        else
            function.Invoke();
    }
    public void OnImmediatelyUpgrade()
    {
        if (this.BuildingLogicData.CurrentBuilidngState != BuildingEditorState.Update)
            return;
        int costGem = MarketCalculator.GetUpdateTimeCost(Mathf.CeilToInt((float)this.BuildingLogicData.UpgradeRemainingWorkload / this.BuildingLogicData.AttachedBuilderEfficiency));
        string costContext = string.Format(StringConstants.PROMPT_GEM_COST, costGem, StringConstants.COIN_GEM, this.BuildingLogicData.Name, this.BuildingLogicData.Level == 0 ? StringConstants.PROMPT_CONSTRUCT + StringConstants.QUESTION_MARK : StringConstants.PROMPT_UPGRADE + StringConstants.QUESTION_MARK);
        UIManager.Instance.UIWindowCostPrompt.ShowWindow(costGem, costContext, StringConstants.PROMPT_FINISH_INSTANTLY);
        UIManager.Instance.UIWindowCostPrompt.UnRegistDelegate();
        UIManager.Instance.UIWindowCostPrompt.Click += () =>
        {
            if (LogicController.Instance.PlayerData.CurrentStoreGem < costGem)
            {
                print("宝石不足，去商店");
                UIManager.Instance.UIWindowFocus = null;
                //UIManager.Instance.UIButtonShopping.GoShopping();
                UIManager.Instance.UISelectShopMenu.GoShopping();
            }
            else
            {
                if (this.BuildingLogicData.CurrentBuilidngState == BuildingEditorState.Update)
                {
                    print("立即完成升级");
                    this.CreateUpgradeFX();
                    LogicController.Instance.FinishBuildingUpgradeInstantly(this.BuildingLogicData.BuildingIdentity);
                    SceneManager.Instance.ConstructBuilding(this.BuildingLogicData, true);
                    UIManager.Instance.HidePopuBtnByCurrentSelect(true);
                    this.BuildingBehavior.enabled = false;
                    Destroy(this.gameObject);
                    SceneManager.Instance.PickableObjectCurrentSelect = null;
                }
            }
        };
    }
    void CreateUpgradeFX()
    {
        SceneManager.Instance.CreateUpgradeFX(this.BuildingBehavior);
    }
    void CreateReadyForUpgradeFX()
    {
        this.m_ReadyForUpgradeFX = SceneManager.Instance.CreateReadyForUpgradeFX(this.BuildingBehavior);
    }
    public void OnReadyForUpgradeFx()
    {
        if (SceneManager.Instance.SceneMode == SceneMode.SceneVisit)
            return;
        if (this.BuildingBehavior.Created)
        { 
            if (this.BuildingLogicData.CurrentBuilidngState == BuildingEditorState.ReadyForUpdate)
            { 
                if (this.m_ReadyForUpgradeFX == null)
                { 
                    this.CreateReadyForUpgradeFX();
                }
            }
        }
    }
}
