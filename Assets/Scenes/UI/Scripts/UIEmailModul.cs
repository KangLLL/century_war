using UnityEngine;
using System.Collections;

public class UIEmailModul : ReusableDelegate
{
    //[SerializeField] UIEmailItem[] uiEmailItem;
    [SerializeField] LogType m_LogType;
    [SerializeField] UISprite[] m_ModulBtnBakcground;//0 = Button true;1 = Button false;
    [SerializeField] UIDragPanelContents m_UIDragPanelContents;
    [SerializeField] ReusableScrollView m_ReusableScrollView;
    TweenAlpha m_TweenAlpha;
    LogData[] m_LogData;
    void Awake()
    {
        this.GetTweenComponent();
    }
	void Start () {
	
	}
 
    public void SetModulData(LogData[] logdata)
    {
 
        this.m_LogData = logdata;
        this.m_ReusableScrollView.ReloadData();
      
    }
  

    public void ShowEmailModul()
    {
        UIManager.Instance.UIWindowEmailChildVisit.HideWindow();
        this.gameObject.SetActive(true);
        this.SetModulBtnState(true);
        this.m_UIDragPanelContents.gameObject.SetActive(true);
        
        m_TweenAlpha.eventReceiver = null;
        m_TweenAlpha.duration = 0.6f;
        m_TweenAlpha.delay = 0;
        m_TweenAlpha.from = 0;
        m_TweenAlpha.to = 1;
        m_TweenAlpha.Play(true);
    }
    public void HideEmailModul(float? duration = null)
    {
        //this.gameObject.SetActive(false);
        this.SetModulBtnState(false);
        this.m_UIDragPanelContents.gameObject.SetActive(false);

        m_TweenAlpha.eventReceiver = this.gameObject;
        m_TweenAlpha.callWhenFinished = "OnFinished";
        m_TweenAlpha.duration = duration.HasValue ? duration.Value : 0.6f;
        m_TweenAlpha.Play(false);
    }
    void SetModulBtnState(bool isActive)
    {  
        m_ModulBtnBakcground[0].color = isActive ? Color.white : Color.clear;
        m_ModulBtnBakcground[1].color = isActive ? Color.clear : Color.white;
    }
    public override void InitialCell(int index, GameObject cell)
    {
        UIEmailItem uiEmailItem = cell.GetComponent<UIEmailItem>();
        uiEmailItem.SetItemData(this.m_LogData[index], m_LogType);
        //uiEmailItem.Initial();
        //cell.GetComponentInChildren<UILabel>().text = index.ToString();
    }
    public override int TotalNumberOfCells
    {
        get { return this.m_LogData.Length; }
    }
    void GetTweenComponent()
    {
        m_TweenAlpha = GetComponent<TweenAlpha>();
    }
    void OnFinished()
    {
        this.gameObject.SetActive(false);
    }
}
