using UnityEngine;
using System.Collections;
using CommandConsts;

public class UIRankItem : MonoBehaviour {
    [SerializeField] UILabel[] m_UILabel;//0 = order ;1 = level; 2 = name; 3 = honour ; 4 = rank rise/drop/flat
    [SerializeField] UISprite[] m_UISpriteBg;//0 = bg1;1 = bg2; 2 = bg3;
    [SerializeField] UISprite[] m_UISpriteIcon;//0 = rank order background; 1 = visit self icon
    [SerializeField] UISprite[] m_UISpriteRank;//rank rise/drop/flat
    RankParameterOrder m_RankParam;
    private Vector3 m_VisitWindowOffsetPosition = new Vector3(-62, 0, -50);
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        //if (this.m_RankParam != null)
        //{
        //    if (this.m_RankParam.Order == -1)
        //        this.gameObject.SetActive(false);
        //    else
        //        this.gameObject.SetActive(true);
        //}
	
	}
    public void SetItemData(RankParameterOrder rankParam)
    {
        this.m_RankParam = rankParam;
        if (rankParam.Order == -1)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else 
            this.gameObject.SetActive(true);

        m_UILabel[0].text = rankParam.Order.ToString() + ".";
        m_UILabel[1].text = rankParam.RankDetailResponseParameter.Level.ToString();
        m_UILabel[2].text = rankParam.RankDetailResponseParameter.Name;
        m_UILabel[3].text = rankParam.RankDetailResponseParameter.Honour.ToString();
        if (rankParam.Order > 3)
            m_UISpriteIcon[0].alpha = 0;
        else
        {
            m_UISpriteIcon[0].alpha = 1;
            m_UISpriteIcon[0].spriteName = ClientSystemConstants.RANK_ORDER_ICON_DICTIONARY[rankParam.Order];

        }
        if(rankParam.RankDetailResponseParameter.IsSelf)
        {
            m_UISpriteBg[0].color = Color.clear;
            m_UISpriteBg[1].color = Color.clear;
            m_UISpriteBg[2].color = Color.white;
            //m_UISpriteIcon[1].color = Color.clear;
        }
        else
        {
            //m_UISpriteIcon[1].color = Color.white;
            if (rankParam.Index % 2 == 0)
            {
                m_UISpriteBg[0].color = Color.clear;
                m_UISpriteBg[1].color = Color.white;
                m_UISpriteBg[2].color = Color.clear;
            }
            else
            {
                m_UISpriteBg[0].color = Color.white;
                m_UISpriteBg[1].color = Color.clear;
                m_UISpriteBg[2].color = Color.clear;
            }
        }

        switch (System.Math.Sign(rankParam.RankDetailResponseParameter.Trend))
        {
            case -1:
                m_UISpriteRank[0].color = Color.clear;
                m_UISpriteRank[1].color = Color.white;
                m_UISpriteRank[2].color = Color.clear;
                m_UILabel[4].text = Mathf.Abs(rankParam.RankDetailResponseParameter.Trend).ToString();
                break;
            case 0:
                m_UISpriteRank[0].color = Color.clear;
                m_UISpriteRank[1].color = Color.clear;
                m_UISpriteRank[2].color = Color.white;
                m_UILabel[4].text = "";
                break;
            case 1:
                m_UISpriteRank[0].color = Color.white;
                m_UISpriteRank[1].color = Color.clear;
                m_UISpriteRank[2].color = Color.clear;
                m_UILabel[4].text = Mathf.Abs(rankParam.RankDetailResponseParameter.Trend).ToString();
                
                break;
        }
    }
    void OnDrag()
    {
        UIManager.Instance.UIWindowLeaderboardChildVisit.HideWindow();
    }
    void OnClick()
    {
        if (!this.m_RankParam.RankDetailResponseParameter.IsSelf)
        {
            if (UIManager.Instance.UIWindowLeaderboardChildVisit.ControlerFocus != this.gameObject)
            {
                this.ShowVisitWindow();
                UIManager.Instance.UIWindowLeaderboardChildVisit.ControlerFocus = this.gameObject;
            }
            else
            {
                if (UIManager.Instance.UIWindowLeaderboardChildVisit.gameObject.activeSelf)
                    UIManager.Instance.UIWindowLeaderboardChildVisit.HideWindow();
                else
                    this.ShowVisitWindow();
            }
        }
    }
    void ShowVisitWindow()
    {
        //UIManager.Instance.UIWindowEmailChildVisit.SetWindowItem(this.m_LogData.RivalName);
        UIManager.Instance.UIWindowLeaderboardChildVisit.transform.position = this.transform.position + this.m_VisitWindowOffsetPosition;
        UIManager.Instance.UIWindowLeaderboardChildVisit.UnRegistDelegate();
        UIManager.Instance.UIWindowLeaderboardChildVisit.MissionEvent1 = () =>
        {
            this.OnVisitFriend();
        };
        UIManager.Instance.UIWindowLeaderboardChildVisit.MissionEvent2 = () =>
        {
            this.OnViewClan();
        };
        UIManager.Instance.UIWindowLeaderboardChildVisit.ShowWindow(this.m_RankParam.RankDetailResponseParameter.Name);
    }
    void OnVisitFriend()
    {
        this.OnRequestVisitFriend();
        StartCoroutine("DelayCloudFadeIn");
        UIManager.Instance.CloudFadeIn();
    }
    void OnRequestVisitFriend()
    {
        print("OnRequestVisitFriend");
        LockScreen.Instance.DisableInput();
        UIManager.Instance.CloudFadeIn();
        VisitFriendRequestParameter parameter = new VisitFriendRequestParameter();
        parameter.FriendID = this.m_RankParam.RankDetailResponseParameter.PlayerID;
        CommunicationUtility.Instance.VisitFriend(parameter, this, "OnResponseVisitFriend", true);
    }
    void OnResponseVisitFriend(Hashtable hash)
    {
        print("OnResponseVisitFriend");
        FriendResponseParameter response = new FriendResponseParameter();
        response.InitialParameterObjectFromHashtable(hash);
        FriendData fd = new FriendData();
        fd.InitialWithResponseData(response, this.m_RankParam.RankDetailResponseParameter.Name);
        
        LogicController.Instance.CurrentFriend = fd;
        StartCoroutine("LoadVisitScene");
        //Application.LoadLevel(ClientStringConstants.VISIT_SCENE_LEVEL_NAME); 

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
    bool cloudState;
    IEnumerator LoadVisitScene()
    {
        while (!cloudState)
        {
            yield return null;
        }
        LockScreen.Instance.EnableInput();
        Application.LoadLevel(ClientStringConstants.VISIT_SCENE_LEVEL_NAME);
    }

    IEnumerator DelayCloudFadeIn()
    {
        yield return new WaitForSeconds(1.5f);
        cloudState = true;
    }
}
