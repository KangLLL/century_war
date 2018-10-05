using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using System;
public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject[] m_PopupBtn;
    [SerializeField] UILabel m_BuildingTitle;
    [SerializeField] UICamera m_UICamera;
    public UICamera UICamera { get { return m_UICamera; } }
    [SerializeField] UIWindowBuyArmy m_UIWindowBuyArmy;
    public UIWindowBuyArmy UIWindowBuyArmy { get { return m_UIWindowBuyArmy; } }
    [SerializeField] UIWindowUpagradeBuilding m_UIWindowUpagradeBuilding;
    public UIWindowUpagradeBuilding UIWindowUpagradeBuilding { get { return m_UIWindowUpagradeBuilding; } }
    [SerializeField] UIWindowBuyBuilding m_UIWindowBuyBuilding;
    public UIWindowBuyBuilding UIWindowBuyBuilding { get { return m_UIWindowBuyBuilding; } }
    [SerializeField] UIWindowSelectBuilder m_UIWindowSelectBuilder;
    public UIWindowSelectBuilder UIWindowSelectBuilder { get { return m_UIWindowSelectBuilder; } }
    [SerializeField] UIWindowCostPrompt m_UIWindowCostPrompt;
    public UIWindowCostPrompt UIWindowCostPrompt { get { return m_UIWindowCostPrompt; } }
    [SerializeField] UIWindowArmyInformation m_UIWindowArmyInformation;
    public UIWindowArmyInformation UIWindowArmyInformation { get { return m_UIWindowArmyInformation; } }
    [SerializeField] UIWindowMain m_UIWindowMain;
    public UIWindowMain UIWindowMain { get { return m_UIWindowMain; } }
    [SerializeField] UIWindowBuildingInfomation m_UIWindowBuildingInfomation;
    public UIWindowBuildingInfomation UIWindowBuildingInfomation { get { return m_UIWindowBuildingInfomation; } }
    [SerializeField] UIWindowUpgradeArmy m_UIWindowUpgradeArmy;
    public UIWindowUpgradeArmy UIWindowUpgradeArmy { get { return m_UIWindowUpgradeArmy; } }
    List<GameObject> m_PopupBtnList = new List<GameObject>();
    [SerializeField] UIWindowUpgradeArmyInfo m_UIWindowUpgradeArmyInfo;
    public UIWindowUpgradeArmyInfo UIWindowUpgradeArmyInfo { get { return m_UIWindowUpgradeArmyInfo; } }
    [SerializeField] UIWindowConfirmPrompt m_UIWindowConfirmPrompt;
    public UIWindowConfirmPrompt UIWindowConfirmPrompt { get { return m_UIWindowConfirmPrompt; } }
    public GameObject UIWindowFocus { get; set; }
    [SerializeField] UIWindowEmailChildShield m_UIWindowEmailChildShield;
    public UIWindowEmailChildShield UIWindowEmailChildShield { get { return m_UIWindowEmailChildShield; } }
    [SerializeField] UIWindowEmailChildVisit m_UIWindowEmailChildVisit;
    public UIWindowEmailChildVisit UIWindowEmailChildVisit { get { return m_UIWindowEmailChildVisit; } }
     [SerializeField] UIWindowEmailChildVisit m_UIWindowLeaderboardChildVisit;
    public UIWindowEmailChildVisit UIWindowLeaderboardChildVisit { get { return m_UIWindowLeaderboardChildVisit; } }
    [SerializeField]UIWindowAccount m_UIWindowAccount;
    public UIWindowAccount UIWindowAccount { get { return m_UIWindowAccount; } }
    [SerializeField]UIWindowBuyGem m_UIWindowBuyGem;
    public UIWindowBuyGem UIWindowBuyGem { get { return m_UIWindowBuyGem; } }
    [SerializeField] UIWindowBuyTree m_UIWindowBuyTree;
    public UIWindowBuyTree UIWindowBuyTree { get { return m_UIWindowBuyTree; } }
    [SerializeField] UIWindowBuyResource m_UIWindowBuyResource;
    public UIWindowBuyResource UIWindowBuyResource { get { return m_UIWindowBuyResource; } }
    [SerializeField] UIWindowBuyTreeChild m_UIWindowBuyTreeChild;
    public UIWindowBuyTreeChild UIWindowBuyTreeChild { get { return m_UIWindowBuyTreeChild; } }
    [SerializeField] UIConstructBuilding m_UIWindowShop;
    public UIConstructBuilding UIWindowShop { get { return m_UIWindowShop; } }
    [SerializeField] UIWindowTask m_UIWindowTask;
    public UIWindowTask UIWindowTask { get { return m_UIWindowTask; } }
    [SerializeField] UIConstructBuilding m_UIConstructBuilding;
    public UIConstructBuilding UIConstructBuilding { get { return m_UIConstructBuilding; } }
    //[SerializeField] UIButtonShopping m_UIButtonShopping;
    //public UIButtonShopping UIButtonShopping { get { return m_UIButtonShopping; } }
    [SerializeField] UISelectShopMenu m_UISelectShopMenu;
    public UISelectShopMenu UISelectShopMenu { get { return m_UISelectShopMenu; } }

    [SerializeField] UIWindowBuyMercenary m_UIWindowBuyMercenary;
    public UIWindowBuyMercenary UIWindowBuyMercenary { get { return m_UIWindowBuyMercenary; } }
    [SerializeField] UIWindowMercenaryInfo m_UIWindowMercenaryInfo;
    public UIWindowMercenaryInfo UIWindowMercenaryInfo { get { return m_UIWindowMercenaryInfo; } }
    [SerializeField] UIWindowDefendLog m_UIWindowDefendLog;
    public UIWindowDefendLog UIWindowDefendLog { get { return m_UIWindowDefendLog; } }
    [SerializeField] UIWindowPropsStorage m_UIWindowPropsStorage;
    public UIWindowPropsStorage UIWindowPropsStorage { get { return m_UIWindowPropsStorage; } }
    [SerializeField] UIWindowPropInfo m_UIWindowPropInfo;
    public UIWindowPropInfo UIWindowPropInfo { get { return m_UIWindowPropInfo; } }
    [SerializeField] UIWindowPropDestroy m_UIWindowPropDestroy;
    public UIWindowPropDestroy UIWindowPropDestroy { get { return m_UIWindowPropDestroy; } }

    [SerializeField] CloudBehaviour m_CloudBehaviour; 
    public CloudBehaviour CloudBehaviour { get { return m_CloudBehaviour; } }
    bool m_SceneFocus = true;
    public bool SceneFocus { get { return m_SceneFocus; } set { m_SceneFocus = value; } }

	[SerializeField]
	private LogicTipsBehavior m_TipsBehavior;
 
    #region const
    const int INTERVAL_DISTANCE = 110;
    const float DELAY = 0.03f;
    const int TO_Y = 70;
    const int FROM_Y = -50;
    const float DURATION = 0.1f;
    public event Func<System.Object,bool> MoveToEvent;
    #endregion
    // Use this for initialization
    static UIManager m_Instance;
    static public UIManager Instance
    {
        get
        {
            return m_Instance;
        }
    }

    void Awake()
    {
        m_Instance = this;
        
        m_CloudBehaviour.OnCompleteCloudFade = this.OnCompleteCloudFadeOut;
    }

    void Start()
    {
        LockScreen.Instance.DisableInput();
        this.CloudFadeOut();
        if (SceneManager.Instance.SceneMode != SceneMode.SceneVisit)
            this.m_UIWindowDefendLog.ShowWindow();
    }
    public void CloudFadeOut()
    {
        m_CloudBehaviour.FadeOut();
    }
    public void CloudFadeIn()
    {
        m_CloudBehaviour.FadeIn();
    }
    void OnCompleteCloudFadeOut()
    {
        LockScreen.Instance.EnableInput();
        if (BattleData.IsLoseProps)
            UIErrorMessage.Instance.ErrorMessage(27);
		if(this.m_TipsBehavior != null)
		{
			this.m_TipsBehavior.enabled = true;
		}
    }

    bool m_IsShow = false;
    void ShowPopupBtn(BuildingBehavior buildingBehavior)
    {
        //print("ShowPopupBtn1");
        if (m_IsShow)
            return;
        //print("ShowPopupBtn2");
        BuildingLogicData buildingLogicData = buildingBehavior.BuildingLogicData;
        //Information button
        if (buildingLogicData.CurrentBuilidngState != BuildingEditorState.ReadyForUpdate || SceneManager.Instance.SceneMode == SceneMode.SceneVisit)
        {
            if (buildingLogicData.BuildingIdentity.buildingType != BuildingType.Wall || SceneManager.Instance.SelectedAllWallList.Count == 0)
            {
                m_PopupBtnList.Add(m_PopupBtn[0]);
                ButtonListener buttonListener = m_PopupBtn[0].GetComponent<ButtonListener>();
                buttonListener.Controller = buildingBehavior;
                buttonListener.Message = "ShowWindowBuildingInfomation";
                this.ShowTitle();
            }
        }
        if (SceneManager.Instance.SceneMode == SceneMode.SceneBuild)
        {
            //Accelerate Collect button
            if ((buildingLogicData.CanCollectFood || buildingLogicData.CanCollectGold || buildingLogicData.CanCollectOil) && buildingLogicData.CurrentBuilidngState == BuildingEditorState.Normal)
            {
                m_PopupBtnList.Add(m_PopupBtn[1]);
                ButtonListener buttonListener = m_PopupBtn[1].GetComponent<ButtonListener>();
                buttonListener.Controller = buildingBehavior;
                buttonListener.Message = "AccelerateCollect";
                m_PopupBtn[1].GetComponent<UIPopupBtnAccelerate>().BuildingLogicData = buildingLogicData;
                m_PopupBtn[1].GetComponent<UIPopupBtnAccelerate>().AccelerateType = AccelerateType.Resource;
            }
            //Accelerate TrainTroops button
            if (buildingLogicData.CanProduceArmy && buildingLogicData.CurrentBuilidngState == BuildingEditorState.Normal)
            {
                m_PopupBtnList.Add(m_PopupBtn[1]);
                ButtonListener buttonListener = m_PopupBtn[1].GetComponent<ButtonListener>();
                buttonListener.Controller = buildingBehavior;
                buttonListener.Message = "AccelerateProduce";
                m_PopupBtn[1].GetComponent<UIPopupBtnAccelerate>().BuildingLogicData = buildingLogicData;
                m_PopupBtn[1].GetComponent<UIPopupBtnAccelerate>().AccelerateType = AccelerateType.Army;
            }
            //Cancel button
            if (buildingLogicData.CurrentBuilidngState == BuildingEditorState.Update)
            {
                m_PopupBtnList.Add(m_PopupBtn[2]);
                ButtonListener buttonListener = m_PopupBtn[2].GetComponent<ButtonListener>();
                buttonListener.Controller = buildingBehavior;
                buttonListener.Message = "OnCancelConstruct";
            }
            //Upgrade button
            if (buildingLogicData.CurrentBuilidngState == BuildingEditorState.Normal)
            {
                if (buildingLogicData.BuildingIdentity.buildingType != BuildingType.Wall || SceneManager.Instance.SelectedAllWallList.Count == 0)
                {
                    if (buildingLogicData.Level < buildingLogicData.MaximumLevel)
                    {
                        m_PopupBtnList.Add(m_PopupBtn[3]);
                        ButtonListener buttonListener = m_PopupBtn[3].GetComponent<ButtonListener>();
                        buttonListener.Controller = buildingBehavior;
                        buttonListener.Message = "OnUpgradeBuilding";
                        m_PopupBtn[3].GetComponent<UIPopupBtnUpgrade>().BuildingLogicData = buildingLogicData;
                        m_PopupBtn[3].GetComponent<UIPopupBtnUpgrade>().SetCostItemData();
                    }
                }
            }
            //FinishNow button
            if (buildingLogicData.CurrentBuilidngState == BuildingEditorState.Update)
            {
                m_PopupBtnList.Add(m_PopupBtn[4]);
                ButtonListener buttonListener = m_PopupBtn[4].GetComponent<ButtonListener>();
                buttonListener.Controller = buildingBehavior;
                buttonListener.Message = "OnImmediatelyUpgrade";
            }
            //TrainTroops button 
            if (buildingLogicData.CanProduceArmy && buildingLogicData.CurrentBuilidngState == BuildingEditorState.Normal)
            {
                m_PopupBtnList.Add(m_PopupBtn[5]);
                ButtonListener buttonListener = m_PopupBtn[5].GetComponent<ButtonListener>();
                buttonListener.Controller = buildingBehavior;
                buttonListener.Message = "ShowWindowProduceArmy";
            }

            //StrengthenTroops button
            if (buildingLogicData.CanUpgradeArmy && buildingLogicData.CurrentBuilidngState == BuildingEditorState.Normal)
            {
                m_PopupBtnList.Add(m_PopupBtn[6]);
                ButtonListener buttonListener = m_PopupBtn[6].GetComponent<ButtonListener>();
                buttonListener.Controller = buildingBehavior;
                buttonListener.Message = "ShowWindowUpgradeArmy";
            }
            //Buy mercenary button
            if (buildingLogicData.BuildingType == BuildingType.Tavern && buildingLogicData.CurrentBuilidngState == BuildingEditorState.Normal)
            {
                m_PopupBtnList.Add(m_PopupBtn[18]);
                ButtonListener buttonListener = m_PopupBtn[18].GetComponent<ButtonListener>();
                buttonListener.Controller = buildingBehavior;
                buttonListener.Message = "ShowWindowTavern";
            }
            // PropsStorage button
            if (buildingLogicData.BuildingType == BuildingType.PropsStorage)
            {
                m_PopupBtnList.Add(m_PopupBtn[19]);
                ButtonListener buttonListener = m_PopupBtn[19].GetComponent<ButtonListener>();
                buttonListener.Controller = buildingBehavior;
                buttonListener.Message = "ShowWindowPropsStorage";
            }
            //CreateSpells button
            if (buildingLogicData.CanProduceItem && buildingLogicData.CurrentBuilidngState == BuildingEditorState.Normal)
            {
                m_PopupBtnList.Add(m_PopupBtn[7]);
            }
            //StrengthenSpells button
            if (buildingLogicData.CanUpgradeItem && buildingLogicData.CurrentBuilidngState == BuildingEditorState.Normal)
            {
                m_PopupBtnList.Add(m_PopupBtn[8]);
            }
            //CollectGold button
            if (buildingLogicData.CanCollectGold && buildingLogicData.CurrentBuilidngState == BuildingEditorState.Normal)
            {
                m_PopupBtnList.Add(m_PopupBtn[9]);
                ButtonListener buttonListener = m_PopupBtn[9].GetComponent<ButtonListener>();
                buttonListener.Controller = buildingBehavior;
                buttonListener.Message = "Collect";
                m_PopupBtn[9].GetComponent<UIPopupBtnCollect>().BuildingLogicData = buildingLogicData;
            }
            //CollectOil button
            if (buildingLogicData.CanCollectOil && buildingLogicData.CurrentBuilidngState == BuildingEditorState.Normal)
            {
                m_PopupBtnList.Add(m_PopupBtn[10]);
                ButtonListener buttonListener = m_PopupBtn[10].GetComponent<ButtonListener>();
                buttonListener.Controller = buildingBehavior;
                buttonListener.Message = "Collect";
                m_PopupBtn[10].GetComponent<UIPopupBtnCollect>().BuildingLogicData = buildingLogicData;
            }
            //CollectFoods button
            if (buildingLogicData.CanCollectFood && buildingLogicData.CurrentBuilidngState == BuildingEditorState.Normal)
            {
                m_PopupBtnList.Add(m_PopupBtn[11]);
                ButtonListener buttonListener = m_PopupBtn[11].GetComponent<ButtonListener>();
                buttonListener.Controller = buildingBehavior;
                buttonListener.Message = "Collect";
                m_PopupBtn[11].GetComponent<UIPopupBtnCollect>().BuildingLogicData = buildingLogicData;
            }
            //HelpArmy button
            if (buildingLogicData.CanHelpArmy && buildingLogicData.CurrentBuilidngState == BuildingEditorState.Normal)
            {
                m_PopupBtnList.Add(m_PopupBtn[12]);
            }
            //Clan button
            if (buildingLogicData.CanClan && buildingLogicData.CurrentBuilidngState == BuildingEditorState.Normal)
            {
                m_PopupBtnList.Add(m_PopupBtn[13]);
            }
            //Select Wall row
            if (buildingLogicData.BuildingIdentity.buildingType == BuildingType.Wall)
            {
                if (SceneManager.Instance.SelectedAllWallList.Count == 0)
                {
                    bool enableSelectRow = false;
                    if (((WallBehavior)buildingBehavior).FindNeighourWall(BuildingObstacleDirection.Bottom, 1).HasValue)
                    {
                        if (((WallBehavior)buildingBehavior).FindNeighourWall(BuildingObstacleDirection.Bottom, 1).Value)
                            enableSelectRow = true;
                    }
                    if (((WallBehavior)buildingBehavior).FindNeighourWall(BuildingObstacleDirection.Left, 1).HasValue)
                    {
                        if (((WallBehavior)buildingBehavior).FindNeighourWall(BuildingObstacleDirection.Left, 1).Value)
                            enableSelectRow = true;
                    }
                    if (((WallBehavior)buildingBehavior).FindNeighourWall(BuildingObstacleDirection.Top, 1).HasValue)
                    {
                        if (((WallBehavior)buildingBehavior).FindNeighourWall(BuildingObstacleDirection.Top, 1).Value)
                            enableSelectRow = true;
                    }
                    if (((WallBehavior)buildingBehavior).FindNeighourWall(BuildingObstacleDirection.Right, 1).HasValue)
                    {
                        if (((WallBehavior)buildingBehavior).FindNeighourWall(BuildingObstacleDirection.Right, 1).Value)
                            enableSelectRow = true;
                    }

                    if (enableSelectRow)
                    {
                        m_PopupBtnList.Add(m_PopupBtn[15]);
                        ButtonListener buttonListener = m_PopupBtn[15].GetComponent<ButtonListener>();
                        buttonListener.Controller = buildingBehavior;
                        buttonListener.Message = "OnSelectWallRow";
                    }
                }
            }
            //Rotate Wall row
            if (buildingLogicData.BuildingIdentity.buildingType == BuildingType.Wall)
            {
                if (SceneManager.Instance.SelectedAllWallList.Count > 0)
                    m_PopupBtnList.Add(m_PopupBtn[16]);
                ButtonListener buttonListener = m_PopupBtn[16].GetComponent<ButtonListener>();
                buttonListener.Controller = buildingBehavior;
                buttonListener.Message = "OnRotateWallRow";

            }
            //Upgrade Wall row
            if (buildingLogicData.BuildingIdentity.buildingType == BuildingType.Wall)
            {
                if (SceneManager.Instance.SelectedAllWallList.Count > 0)
                    m_PopupBtnList.Add(m_PopupBtn[17]);
                ButtonListener buttonListener = m_PopupBtn[17].GetComponent<ButtonListener>();
                buttonListener.Controller = buildingBehavior;
                buttonListener.Message = "OnUpradeAllWall";
            }
        }
        float startX = ((m_PopupBtnList.Count - 1) * INTERVAL_DISTANCE) / -2.0f;
        for (int i = 0; i < m_PopupBtnList.Count; i++)
        {
            float x = startX + i * INTERVAL_DISTANCE;
            Vector3 from = new Vector3(x, FROM_Y, 0);
            Vector3 to = new Vector3(x, TO_Y, 0);

            TweenAlpha tweenAlpha = m_PopupBtnList[i].GetComponent<TweenAlpha>();
            if (tweenAlpha != null)
            {
                tweenAlpha.delay = i * DELAY;
                tweenAlpha.from = 0;
                tweenAlpha.to = 1;
                tweenAlpha.duration = DURATION;
                tweenAlpha.Play(true);
            }
            //iTween.Stop(m_PopupBtnList[i].gameObject);
            m_PopupBtnList[i].transform.localPosition = from;
            iTween.MoveTo(m_PopupBtnList[i].gameObject, iTween.Hash(iT.MoveTo.position, to, iT.MoveTo.easetype, iTween.EaseType.easeOutBack, iT.MoveTo.time, 0.2f, iT.MoveTo.islocal, true, iT.MoveTo.delay, i * DELAY, iT.MoveTo.oncomplete, "OnCompleteMoveTo", iT.MoveTo.oncompleteparams, m_PopupBtnList[i].gameObject));
            
        }
        m_IsShow = true;
    }
    public void OnCompleteMoveTo(System.Object param)
    {
        if (this.MoveToEvent != null)
        {
            bool state = this.MoveToEvent(param);
            if (state)
            {
                this.MoveToEvent = null;
            }
        }
    }
    void ShowPopUpBtnByRemovableObject(RemovableObjectBehavior removableObjectBehavior)
    {
        if (m_IsShow)
            return;

        RemovableObjectLogicData removableObjectLogicData = removableObjectBehavior.RemovableObjectLogicData;
        //Remove object
        if (removableObjectLogicData.EditorState == RemovableObjectEditorState.Normal)
        {
            m_PopupBtnList.Add(m_PopupBtn[14]);
            ButtonListener buttonListener = m_PopupBtn[14].GetComponent<ButtonListener>();
            buttonListener.Controller = removableObjectBehavior;
            buttonListener.Message = "OnRemoveObject";
            m_PopupBtn[14].GetComponent<UIPopupBtnRemoveObject>().RemovableObjectLogicData = removableObjectLogicData;
            m_PopupBtn[14].GetComponent<UIPopupBtnRemoveObject>().SetBtnData();
            this.ShowTitle();
        }
        if (removableObjectLogicData.EditorState == RemovableObjectEditorState.Removing)
        {
            m_PopupBtnList.Add(m_PopupBtn[2]);
            ButtonListener buttonListener = m_PopupBtn[2].GetComponent<ButtonListener>();
            buttonListener.Controller = removableObjectBehavior;
            buttonListener.Message = "OnCancelRemoveObject";
        }
        float startX = ((m_PopupBtnList.Count - 1) * INTERVAL_DISTANCE) / -2.0f;
        for (int i = 0; i < m_PopupBtnList.Count; i++)
        {
            float x = startX + i * INTERVAL_DISTANCE;
            Vector3 from = new Vector3(x, FROM_Y, 0);
            Vector3 to = new Vector3(x, TO_Y, 0);
            TweenAlpha tweenAlpha = m_PopupBtnList[i].GetComponent<TweenAlpha>();
            tweenAlpha.delay = i * DELAY;
            tweenAlpha.from = 0;
            tweenAlpha.to = 1;
            tweenAlpha.duration = DURATION;
            tweenAlpha.Play(true);
            //iTween.Stop(m_PopupBtnList[i].gameObject);
            m_PopupBtnList[i].transform.localPosition = from;
            iTween.MoveTo(m_PopupBtnList[i].gameObject, iTween.Hash(iT.MoveTo.position, to, iT.MoveTo.easetype, iTween.EaseType.easeOutBack, iT.MoveTo.time, 0.2f, iT.MoveTo.islocal, true, iT.MoveTo.delay, i * DELAY));

        }
        m_IsShow = true;
    }
    void ShowPopUpBtnByShowDefenseObject(DefenseObjectBehavior defenseObjectBehavior)
    {
        if (m_IsShow)
            return;
        //DefenseObjectLogicData defenseObjectLogicData = defenseObjectBehavior.DefenseObjectLogicData;
        this.ShowTitle();
        m_PopupBtnList.Add(m_PopupBtn[20]);
        ButtonListener buttonListener = m_PopupBtn[20].GetComponent<ButtonListener>();
        buttonListener.Controller = defenseObjectBehavior;
        buttonListener.Message = "OnPropsDestroy";

        float startX = ((m_PopupBtnList.Count - 1) * INTERVAL_DISTANCE) / -2.0f;
        for (int i = 0; i < m_PopupBtnList.Count; i++)
        {
            float x = startX + i * INTERVAL_DISTANCE;
            Vector3 from = new Vector3(x, FROM_Y, 0);
            Vector3 to = new Vector3(x, TO_Y, 0);
            TweenAlpha tweenAlpha = m_PopupBtnList[i].GetComponent<TweenAlpha>();
            tweenAlpha.delay = i * DELAY;
            tweenAlpha.from = 0;
            tweenAlpha.to = 1;
            tweenAlpha.duration = DURATION;
            tweenAlpha.Play(true);
            //iTween.Stop(m_PopupBtnList[i].gameObject);
            m_PopupBtnList[i].transform.localPosition = from;
            iTween.MoveTo(m_PopupBtnList[i].gameObject, iTween.Hash(iT.MoveTo.position, to, iT.MoveTo.easetype, iTween.EaseType.easeOutBack, iT.MoveTo.time, 0.2f, iT.MoveTo.islocal, true, iT.MoveTo.delay, i * DELAY));

        }
        m_IsShow = true;

    }
    void ShowPopUpBtnByAchievObject(AchievementBuildingBehavior achievementBuildingBehavior)
    {
        if (m_IsShow)
            return;
        AchievementBuildingLogicData achievementBuildingLogicData = achievementBuildingBehavior.AchievementBuildingLogicData;
        this.ShowTitle();
        //Building Information
        m_PopupBtnList.Add(m_PopupBtn[0]);
        ButtonListener buttonListener = m_PopupBtn[0].GetComponent<ButtonListener>();
        buttonListener.Controller = achievementBuildingBehavior;
        buttonListener.Message = "ShowWindowBuildingInfomation";
        if (SceneManager.Instance.SceneMode == SceneMode.SceneBuild)
        {
            //Building Remove
            m_PopupBtnList.Add(m_PopupBtn[21]);
            buttonListener = m_PopupBtn[21].GetComponent<ButtonListener>();
            buttonListener.Controller = achievementBuildingBehavior;
            buttonListener.Message = "OnRemoveBuilidng";
            //Building Repair
            m_PopupBtnList.Add(m_PopupBtn[22]);
            buttonListener = m_PopupBtn[22].GetComponent<ButtonListener>();
            buttonListener.Controller = achievementBuildingBehavior;
            buttonListener.Message = "OnRepairBuilidng";
            m_PopupBtn[22].GetComponent<UIPopupBtnRepair>().AchievementBuildingLogicData = achievementBuildingLogicData;
            m_PopupBtn[22].GetComponent<UIPopupBtnRepair>().SetBtnData();
        }
        float startX = ((m_PopupBtnList.Count - 1) * INTERVAL_DISTANCE) / -2.0f;
        for (int i = 0; i < m_PopupBtnList.Count; i++)
        {
            float x = startX + i * INTERVAL_DISTANCE;
            Vector3 from = new Vector3(x, FROM_Y, 0);
            Vector3 to = new Vector3(x, TO_Y, 0);
            TweenAlpha tweenAlpha = m_PopupBtnList[i].GetComponent<TweenAlpha>();
            tweenAlpha.delay = i * DELAY;
            tweenAlpha.from = 0;
            tweenAlpha.to = 1;
            tweenAlpha.duration = DURATION;
            tweenAlpha.Play(true);
            //iTween.Stop(m_PopupBtnList[i].gameObject);
            m_PopupBtnList[i].transform.localPosition = from;
            iTween.MoveTo(m_PopupBtnList[i].gameObject, iTween.Hash(iT.MoveTo.position, to, iT.MoveTo.easetype, iTween.EaseType.easeOutBack, iT.MoveTo.time, 0.2f, iT.MoveTo.islocal, true, iT.MoveTo.delay, i * DELAY));

        }
        m_IsShow = true;
    }
    void HidePopupBtnDelay()
    {
       
        if (!this.m_IsShow)
            return;
        float startX = ((m_PopupBtnList.Count - 1) * INTERVAL_DISTANCE) / -2.0f;
        for (int i = 0; i < m_PopupBtnList.Count; i++)
        {
            float x = startX + i * INTERVAL_DISTANCE;
            Vector3 from = new Vector3(x, FROM_Y, 0);
            TweenAlpha tweenAlpha = m_PopupBtnList[i].gameObject.GetComponent<TweenAlpha>(); 
            if (tweenAlpha != null) 
                    tweenAlpha.Play(false); 
            iTween.MoveTo(m_PopupBtnList[i].gameObject, iTween.Hash(iT.MoveTo.position, from, iT.MoveTo.easetype, iTween.EaseType.easeOutBack, iT.MoveTo.time, 0.2f, iT.MoveTo.islocal, true, iT.MoveTo.delay, i * DELAY));

        }
        m_PopupBtnList.Clear();
        this.m_IsShow = false;
        this.HideTitleDelay();
    }
    void HidePopupBtnImmediately()
    {
        if (!this.m_IsShow)
            return;
        float startX = ((m_PopupBtnList.Count - 1) * INTERVAL_DISTANCE) / -2.0f;
        for (int i = 0; i < m_PopupBtnList.Count; i++)
        {
            float x = startX + i * INTERVAL_DISTANCE;
            Vector3 from = new Vector3(x, FROM_Y, 0);
            TweenAlpha tweenAlpha = m_PopupBtnList[i].gameObject.GetComponent<TweenAlpha>();
            if(tweenAlpha != null)
            {
                m_PopupBtnList[i].GetComponent<UIPanel>().alpha = 0;
                tweenAlpha.Reset();
            }
            iTween.Stop(m_PopupBtnList[i].gameObject);
            m_PopupBtnList[i].transform.localPosition = from;
           

        }
        m_PopupBtnList.Clear();
        this.m_IsShow = false;
        this.HideTitleImmediately();
    }

    public void ShowPopupBtnByCurrentSelect()
    { 
        if (SceneManager.Instance.PickableObjectCurrentSelect != null)
        {
            switch (SceneManager.Instance.PickableObjectCurrentSelect.GetType().ToString())
            {
                case "RemovableObjectBehavior":
                    this.ShowPopUpBtnByRemovableObject((RemovableObjectBehavior)SceneManager.Instance.PickableObjectCurrentSelect);
                    break;
                case "DefenseObjectBehavior":
                    this.ShowPopUpBtnByShowDefenseObject((DefenseObjectBehavior)SceneManager.Instance.PickableObjectCurrentSelect);
                    break;
                //case "WallBehavior":
                   // this.ShowPopupBtn((BuildingBehavior)SceneManager.Instance.PickableObjectCurrentSelect);
                   // break;
                case "AchievementBuildingBehavior":
                    this.ShowPopUpBtnByAchievObject((AchievementBuildingBehavior)SceneManager.Instance.PickableObjectCurrentSelect);
                    break;
                default:
                    this.ShowPopupBtn((BuildingBehavior)SceneManager.Instance.PickableObjectCurrentSelect);
                    break;
            }
            //if(SceneManager.Instance.PickableObjectCurrentSelect is BuildingBehavior)
            //    this.ShowPopupBtn((BuildingBehavior)SceneManager.Instance.PickableObjectCurrentSelect);
            //else
            //    this.ShowPopUpBtnByRemovableObject((RemovableObjectBehavior)SceneManager.Instance.PickableObjectCurrentSelect);
  
            SceneManager.Instance.PickableObjectCurrentSelect.ShowBuildingTitle(true);
        }
    }
    public void HidePopuBtnByCurrentSelect(bool immediately)
    {
        //print("HidePopuBtnByCurrentSelect");
        if (SceneManager.Instance.PickableObjectCurrentSelect != null)
        {
            if (immediately)
                this.HidePopupBtnImmediately();
            else
                this.HidePopupBtnDelay();
            SceneManager.Instance.PickableObjectCurrentSelect.ShowBuildingTitle(false);
        }
    }
    void ShowTitle()
    {
        if (SceneManager.Instance.PickableObjectCurrentSelect == null)
            return;

        switch (SceneManager.Instance.PickableObjectCurrentSelect.GetType().ToString())
        {
            case "RemovableObjectBehavior":
                this.m_BuildingTitle.text = ((RemovableObjectBehavior)SceneManager.Instance.PickableObjectCurrentSelect).RemovableObjectLogicData.Name;
                break;
            case "DefenseObjectBehavior":
                this.m_BuildingTitle.text = ((DefenseObjectBehavior)SceneManager.Instance.PickableObjectCurrentSelect).DefenseObjectLogicData.Name;
                break;
            //case "WallBehavior":
            //    {
            //        string level = ((BuildingBehavior)SceneManager.Instance.PickableObjectCurrentSelect).BuildingLogicData.Level.ToString();
            //        this.m_BuildingTitle.text = ((BuildingBehavior)SceneManager.Instance.PickableObjectCurrentSelect).BuildingLogicData.Name + StringConstants.LEFT_PARENTHESES + level + StringConstants.RIGHT_PARENTHESES + StringConstants.PROMPT_LV;
            //    }
            //    break;
            case "AchievementBuildingBehavior":
                this.m_BuildingTitle.text = ((AchievementBuildingBehavior)SceneManager.Instance.PickableObjectCurrentSelect).AchievementBuildingLogicData.Name;
                break;
            default:
                string level = ((BuildingBehavior)SceneManager.Instance.PickableObjectCurrentSelect).BuildingLogicData.Level.ToString();
                this.m_BuildingTitle.text = ((BuildingBehavior)SceneManager.Instance.PickableObjectCurrentSelect).BuildingLogicData.Name + StringConstants.LEFT_PARENTHESES + level + StringConstants.RIGHT_PARENTHESES + StringConstants.PROMPT_LV;

                break;
        }
        
        //if (SceneManager.Instance.PickableObjectCurrentSelect is BuildingBehavior)
        //{
        //    string level = ((BuildingBehavior)SceneManager.Instance.PickableObjectCurrentSelect).BuildingLogicData.Level.ToString();
        //    this.m_BuildingTitle.text = ((BuildingBehavior)SceneManager.Instance.PickableObjectCurrentSelect).BuildingLogicData.Name + StringConstants.LEFT_PARENTHESES + level + StringConstants.RIGHT_PARENTHESES + StringConstants.PROMPT_LV;
        //}
        //else
        //    this.m_BuildingTitle.text = ((RemovableObjectBehavior)SceneManager.Instance.PickableObjectCurrentSelect).RemovableObjectLogicData.Name;
        //TweenAlpha tweenAlpha = this.m_BuildingTitle.GetComponent<TweenAlpha>();
        //tweenAlpha.delay = 0;
        //tweenAlpha.from = 0;
        //tweenAlpha.to = 1;
        //tweenAlpha.duration = DURATION;
        //tweenAlpha.Play(true);
        //tweenAlpha.alpha = 1;
        this.m_BuildingTitle.alpha = 1;
    }
    void HideTitleDelay()
    {
        //TweenAlpha tweenAlpha = this.m_BuildingTitle.GetComponent<TweenAlpha>();
        //tweenAlpha.alpha = 0;
        //tweenAlpha.Play(false);
        this.m_BuildingTitle.alpha = 0;
    }
    void HideTitleImmediately()
    {
        //TweenAlpha tweenAlpha = this.m_BuildingTitle.GetComponent<TweenAlpha>();
        ////tweenAlpha.Reset();
        //tweenAlpha.Reset();
        //this.m_BuildingTitle.alpha = 0;
        this.m_BuildingTitle.alpha = 0;
    }
}
public enum ButtonType
{
    Information,
    Boost,
    Cancel,
    Upgrade,
    FinishNow,
    TrainTroops,
    StrengthenTroops,
    CreateSpells,
    StrengthenSpells,
    CollectGold,
    CollectOil,
    CollectFoods,
    HelpArmy,
    Clan
}
