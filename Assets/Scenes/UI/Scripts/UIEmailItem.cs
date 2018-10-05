using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommandConsts;
using System;
using ConfigUtilities.Enums;
using ConfigUtilities; 
public class UIEmailItem : MonoBehaviour {
    [SerializeField] UILabel[] m_UILabelEnemy;//0 = name;1 = gold;2 = food ;3 = oil;4 = RivalHonour 5 = time
    [SerializeField] UISprite m_UISpriteProp;//PropsStorage
    [SerializeField] UILabel[] m_UILabelSelf;//0 = success or fail;1 = percent; 2 = PlumderHonour
    [SerializeField] UISprite[] m_Star;
    [SerializeField] UISprite[] m_Background;//0 = background success;1 = background fail;2 = background promt success; 3 background promt fail;
    [SerializeField] GameObject[] m_Button;// 0 = replayback btn 1 = revenge
    [SerializeField] UILabel m_TextRevenge;// revenge text
    [SerializeField] GameObject[] m_Resourcel;//0 = gold ;1 = food ; 2 = oil;
    //[serializefield] uiitemappend[] m_uiitemappendarmy;
    //[serializefield] uiitemappend[] m_uiitemappendmercenary;
    //[serializefield] uiitemappend[] m_uiitemappendprops;
    //[SerializeField] Transform m_AttackList;
    //[SerializeField] Vector3 m_AttackIconInterval = new Vector3(50, 0, 0);
    //[SerializeField] Transform m_PropsThropyList;
    //[SerializeField] Vector3 m_PropsThropyIconInterval = new Vector3(30, 0, 0);
    [SerializeField] PrefabDictionary m_PropsTypeDict;
    [SerializeField] PrefabDictionary m_ArmyTypeDict;
    [SerializeField] PrefabDictionary m_MercenaryTypeDict;
    [SerializeField] UIItemAppend m_PropsThropyUIItemAppend;

    //[SerializeField] UIGrid m_UIGridAttack;
    //[SerializeField] UIGrid m_UIGridPropsThropy;
    [SerializeField] UIGridPage m_UIGridPageAttackList;
    [SerializeField] UIGridPage m_UIGridPagePropThropyList;
    [SerializeField] GameObject m_NextPageAttackListBtn;
    [SerializeField] GameObject m_NextPagePropThropyListBtn;
  
    //[SerializeField] UIDraggableCamera m_UIDraggableCamera;
    private Vector3 m_VisitWindowOffsetPosition = new Vector3(-170, 50, -50);
    private LogData m_LogData;

    //public void Initial()
    //{
    //    m_UIDraggableCamera.rootForBounds = UIManager.Instance.Root;
    //}
    void OnClick()
    {
        if (UIManager.Instance.UIWindowEmailChildVisit.ControlerFocus != this.gameObject)
        {
            this.ShowVisitWindow();
            UIManager.Instance.UIWindowEmailChildVisit.ControlerFocus = this.gameObject;
        }
        else
        {
            if (UIManager.Instance.UIWindowEmailChildVisit.gameObject.activeSelf)
                UIManager.Instance.UIWindowEmailChildVisit.HideWindow();
            else
                this.ShowVisitWindow();
        }
    }
    void OnDrag()
    {
        UIManager.Instance.UIWindowEmailChildVisit.HideWindow();
    }
    public void SetItemData(LogData logData, LogType logType)
    {
        this.m_LogData = logData;
        m_UILabelEnemy[0].text = logData.RivalName;

        m_UILabelEnemy[1].text = logData.PlunderGold.ToString();
        m_UILabelEnemy[2].text = logData.PlumderFood.ToString();
        
        //m_UILabelEnemy[3].text = logData.PlunderOil.ToString();
        m_Resourcel[2].SetActive(false);
        if (logData.PlunderProps.HasValue)
        {
            m_UISpriteProp.spriteName = ConfigInterface.Instance.PropsConfigHelper.GetPropsData(logData.PlunderProps.Value).PrefabName;
            m_UISpriteProp.gameObject.SetActive(true);
            m_UISpriteProp.MakePixelPerfect();
        }
        else
            m_UISpriteProp.gameObject.SetActive(false);
        
        m_UILabelEnemy[4].text = logData.RivalHonour.ToString();
        m_UILabelEnemy[5].text = SystemFunction.TimeSpanToString((int)logData.ElapsedTime) + StringConstants.ELAPSED_AGO;
        
        switch (logType)
        {
            case LogType.Defend:
                m_UILabelSelf[0].text = logData.RankStar > 0 ? StringConstants.PROMPT_DEFEND_FAIL : StringConstants.PROMPT_DEFEND_SUCCESS;
                m_Background[0].color = logData.RankStar > 0 ? Color.clear : Color.white;
                m_Background[1].color = logData.RankStar > 0 ? Color.white : Color.clear;
                m_Background[2].color = logData.RankStar > 0 ? Color.clear : Color.white;
                m_Background[3].color = logData.RankStar > 0 ? Color.white : Color.clear;
                break;
            case LogType.Attack:
                m_UILabelSelf[0].text = logData.RankStar > 0 ? StringConstants.PROMPT_ATTACK_SUCCESS : StringConstants.PROMPT_ATTACK_FAIL;
                m_Background[0].color = logData.RankStar > 0 ? Color.white : Color.clear;
                m_Background[1].color = logData.RankStar > 0 ? Color.clear : Color.white;
                m_Background[2].color = logData.RankStar > 0 ? Color.white : Color.clear;
                m_Background[3].color = logData.RankStar > 0 ? Color.clear : Color.white;
                break;
        }
        
        m_UILabelSelf[1].text = logData.DestroyBuildingPercentage + "%";
        m_UILabelSelf[2].text = logData.PlunderHonour.ToString();

        for (int i = 1; i <= m_Star.Length; i++)
        {
            m_Star[i - 1].color = i <= logData.RankStar ? Color.white : Color.clear;
        }
       
        m_Button[0].SetActive(logData.IsReplayable); 
        m_Button[1].SetActive(logData.CanRevenge);
        m_TextRevenge.gameObject.SetActive(logData.CanRevenge);
       
        this.CreateArmyIcon();
    }
    void CreateArmyIcon()
    {
        while (m_UIGridPageAttackList.transform.childCount > 0)
        {
            Transform trans = m_UIGridPageAttackList.transform.GetChild(0);
            trans.parent = null;
            Destroy(trans.gameObject);
        }
        while (m_UIGridPagePropThropyList.transform.childCount > 0)
        {
            Transform trans = m_UIGridPagePropThropyList.transform.GetChild(0);
            trans.parent = null;
            Destroy(trans.gameObject);
        }
        
        List<KeyValuePair<ArmyType, DropArmyInfo>> armyInfoList = new List<KeyValuePair<ArmyType, DropArmyInfo>>(this.m_LogData.ArmyInfos);
        int indext = 0;
        int start = 10000;
        for (int i = 0; i < armyInfoList.Count; i++)
        {
            UIItemAppend uiItemAppend = (Instantiate(m_ArmyTypeDict[armyInfoList[i].Key.ToString()]) as GameObject).GetComponent<UIItemAppend>();
            uiItemAppend.transform.parent = m_UIGridPageAttackList.transform;
            uiItemAppend.transform.localPosition = Vector3.zero;
            uiItemAppend.transform.name = (start + indext).ToString();
            int level = armyInfoList[i].Value.Level;
            int count = armyInfoList[i].Value.Quantity;
            //uiItemAppend.SetItemData(ClientSystemConstants.ARMY_ICON_COMMON_DICTIONARY[armyInfoList[i].Key], true,StringConstants.PROMPT_LEVEL + level.ToString(), "X" + count.ToString());
            uiItemAppend.SetItemData( true, StringConstants.PROMPT_LEVEL + level.ToString(), "X" + count.ToString());
            indext++;
        }
        List<KeyValuePair<MercenaryType, int>> mercenaryInfoList = new List<KeyValuePair<MercenaryType, int>>(this.m_LogData.MercenaryInfos);
        for (int i = 0; i < mercenaryInfoList.Count; i++)
        {
            UIItemAppend uiItemAppend = (Instantiate(m_MercenaryTypeDict[mercenaryInfoList[i].Key.ToString()]) as GameObject).GetComponent<UIItemAppend>();
            uiItemAppend.transform.parent = m_UIGridPageAttackList.transform;
            uiItemAppend.transform.name = (start + indext).ToString();
            uiItemAppend.transform.localPosition = Vector3.zero;
            //uiItemAppend.transform.localScale = Vector3.zero;
            int count = mercenaryInfoList[i].Value;
            //uiItemAppend.SetItemData(ClientSystemConstants.MERCENARY_ICON_COMMON_DICTIONARY[mercenaryInfoList[i].Key], false, string.Empty, "X" + count.ToString());
            uiItemAppend.SetItemData(false, string.Empty, "X" + count.ToString());
            indext++;
        }
        List<KeyValuePair<PropsType, int>> propsInfosList = new List<KeyValuePair<PropsType, int>>(this.m_LogData.PropsInfos);
        for (int i = 0; i < propsInfosList.Count; i++)
        {
            UIItemAppend uiItemAppend = (Instantiate(m_PropsTypeDict[propsInfosList[i].Key.ToString()]) as GameObject).GetComponent<UIItemAppend>();
            uiItemAppend.transform.parent = m_UIGridPageAttackList.transform;
            uiItemAppend.transform.name = (start + indext).ToString();
            uiItemAppend.transform.localPosition = Vector3.zero;
            //uiItemAppend.transform.localScale = m_AttackList.localScale;
            int count = propsInfosList[i].Value;
            uiItemAppend.SetItemData(false, string.Empty, "X" + count.ToString());
            indext++;
        }
       
        List<PropsType> propsThropyList = new List<PropsType>(this.m_LogData.PropsThropy);
        for (int i = 0; i < propsThropyList.Count; i++)
        {
            UIItemAppend uiItemAppend = (Instantiate(m_PropsThropyUIItemAppend.gameObject) as GameObject).GetComponent<UIItemAppend>();
            uiItemAppend.transform.parent = m_UIGridPagePropThropyList.transform;
            uiItemAppend.transform.localPosition = Vector3.zero;
            //uiItemAppend.transform.localScale = m_PropsThropyList.localScale; 
            uiItemAppend.transform.localScale = Vector3.one;
            uiItemAppend.transform.name = (start + i).ToString();
            uiItemAppend.SetItemData(ConfigInterface.Instance.PropsConfigHelper.GetPropsData(propsThropyList[i]).PrefabName,false);
            uiItemAppend.MakePixelPerfect();
        }
        m_UIGridPageAttackList.ResetPage();
        m_UIGridPageAttackList.SetPage();
        m_UIGridPagePropThropyList.ResetPage();
        m_UIGridPagePropThropyList.SetPage();
        this.SetTurnPageButton();
    }
 
    void SetTurnPageButton()
    {
        m_NextPageAttackListBtn.SetActive(m_UIGridPageAttackList.GetPageCount() > 1);
        m_NextPagePropThropyListBtn.SetActive(m_UIGridPagePropThropyList.GetPageCount() > 1);
    }
    //Button message
    void OnNextPagePropsThropy()
    {
        m_UIGridPagePropThropyList.TurnPage();
        m_UIGridPagePropThropyList.SetPage();
    }
    //Button message
    void OnNextPageAttackList()
    {
        m_UIGridPageAttackList.TurnPage();
        m_UIGridPageAttackList.SetPage();
    }
    void OnReplayback()
    {
        if (m_LogData.IsReplayable)
        {
            ReplayData.MatchID = this.m_LogData.MatchID;
            //Application.LoadLevel(ClientStringConstants.BATTLE_REPLAY_LEVEL_NAME);
            UIManager.Instance.CloudFadeIn();
            StartCoroutine("LoadPlaybackScene");
        }
    }
    void ShowVisitWindow()
    {
        //UIManager.Instance.UIWindowEmailChildVisit.SetWindowItem(this.m_LogData.RivalName);
        UIManager.Instance.UIWindowEmailChildVisit.transform.position = this.transform.position + this.m_VisitWindowOffsetPosition;
        UIManager.Instance.UIWindowEmailChildVisit.UnRegistDelegate();
        UIManager.Instance.UIWindowEmailChildVisit.MissionEvent1 = () =>
        {
            this.OnVisitRival();
        };
        UIManager.Instance.UIWindowEmailChildVisit.MissionEvent2 = () =>
        {
            this.OnViewClan();
        };
        UIManager.Instance.UIWindowEmailChildVisit.ShowWindow(this.m_LogData.RivalName); 
    }
    void OnRevenge()
    {
        if (LogicController.Instance.AvailableArmies.Count == 0 && LogicController.Instance.AvailableMercenaries.Count ==0)
        {
            UIErrorMessage.Instance.ErrorMessage(4);
            return;
        }
        if (LogicController.Instance.PlayerData.RemainingShieldSecond > 0)
        {
            UIManager.Instance.UIWindowEmailChildShield.MissionEvent = ()=>
            {
                this.OnRequestRevenge();
            };
            UIManager.Instance.UIWindowEmailChildShield.ShowWindow();
            return;
        }
        this.OnRequestRevenge();
    }
    void OnRequestRevenge()
    {
        LockScreen.Instance.DisableInput();
        StartRevengeRequestParameter parameter = new StartRevengeRequestParameter();
        parameter.RevengeMatchID = this.m_LogData.MatchID;
        CommunicationUtility.Instance.StartRevenge(parameter, this, "OnResponseRevenge", true);
    }
    void OnResponseRevenge(Hashtable hash)
    {
        StartRevengeResponseParameter response = new StartRevengeResponseParameter();
        response.InitialParameterObjectFromHashtable(hash);
        if (response.FailType.HasValue)
        {
            switch (response.FailType.Value)
            {
                case RevengeFailType.RivalIsBeAttacked:
                    UIErrorMessage.Instance.ErrorMessage(6);
                    break;
                case RevengeFailType.RivalIsOnline:
                    UIErrorMessage.Instance.ErrorMessage(3);
                    break;
                case RevengeFailType.RivalShieldIsActive:
                    UIErrorMessage.Instance.ErrorMessage(5);
                    break;
            }
            LockScreen.Instance.EnableInput();
        }
        else
        {
            BattleData.RivalInformation = response.RivalInformation;
            BattleData.RelatedLog = this.m_LogData; 
            //Application.LoadLevel(ClientStringConstants.BATTLE_SCENE_LEVEL_NAME);
            UIManager.Instance.CloudFadeIn();
            StartCoroutine("LoadBattleScene");
        }

    }
    void OnVisitRival()
    {
        this.OnRequestVisitRival();
        StartCoroutine("DelayCloudFadeIn");
        UIManager.Instance.CloudFadeIn();
    }
    void OnViewClan()
    {
        this.OnRequestViewClan();
    }
    void OnRequestViewClan()
    {
    }
    void OnResponseViewClan(Hashtable hash)
    {
    }
    void OnRequestVisitRival()
    {
        LockScreen.Instance.DisableInput();
        UIManager.Instance.CloudFadeIn();
        VisitFriendRequestParameter parameter = new VisitFriendRequestParameter();
        parameter.FriendID = this.m_LogData.RivalID;
        CommunicationUtility.Instance.VisitFriend(parameter, this, "OnResponseVisit", true);
    }
    void OnResponseVisit(Hashtable hash)
    { 
        FriendResponseParameter response = new FriendResponseParameter();
        response.InitialParameterObjectFromHashtable(hash);
        FriendData fd = new FriendData();
        fd.InitialWithResponseData(response,this.m_LogData.RivalName);
        LogicController.Instance.CurrentFriend = fd;
        StartCoroutine("LoadVisitScene");
    }

    bool cloudState;
    IEnumerator LoadVisitScene()
    {
        while (!cloudState)
        {
            yield return null;
        }
        Application.LoadLevel(ClientStringConstants.VISIT_SCENE_LEVEL_NAME);
    }
    IEnumerator LoadBattleScene()
    {
        yield return new WaitForSeconds(1.5f);
        Application.LoadLevel(ClientStringConstants.BATTLE_SCENE_LEVEL_NAME);
    }
    IEnumerator LoadPlaybackScene()
    {
        yield return new WaitForSeconds(1.5f);
        Application.LoadLevel(ClientStringConstants.BATTLE_REPLAY_LEVEL_NAME);
    }
    IEnumerator DelayCloudFadeIn()
    {
        yield return new WaitForSeconds(1.5f);
        cloudState = true;
    }

}

 

 
public enum LogType
{
    Defend,
    Attack
}
