using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class UITaskModule : ReusableDelegate
{
    [SerializeField] TaskStatus m_TaskStatus;
    [SerializeField] UISprite[] m_ModulBtnBakcground;//0 = Button true;1 = Button false;
    [SerializeField] UIDragPanelContents m_UIDragPanelContents;
    [SerializeField] ReusableScrollView m_ReusableScrollView;
    TweenAlpha m_TweenAlpha;
    List<Task> m_TaskList = new List<Task>();
	// Use this for initialization
    void Awake()
    {
        this.GetTweenComponent();
    }
	void Start () {
        
	}
	
 
    public void SetModulData()
    {
        this.m_TaskList.Clear();
        //List<Task> tempList = new List<Task>();
        switch (this.m_TaskStatus)
        {
            case TaskStatus.Opened:
                this.m_TaskList.AddRange(LogicController.Instance.TaskManager.TaskList.Where(task => task.Status == TaskStatus.Opened));               
                this.m_TaskList.Sort(
                (a,b) =>
                {
                    int resultA = !PlayerPrefs.HasKey(LogicController.Instance.PlayerData.PlayerID.ToString() + ":TaskID:" + a.TaskID)? -1 : 1;
                    int resultB = !PlayerPrefs.HasKey(LogicController.Instance.PlayerData.PlayerID.ToString() + ":TaskID:" + b.TaskID) ? -1 : 1;
                    int result = resultA.CompareTo(resultB);
                    if (result == 0)
                        return a.TaskID.CompareTo(b.TaskID);
                    else
                        return result;
                });
                break;
            case TaskStatus.Completed:
                this.m_TaskList.AddRange(LogicController.Instance.TaskManager.TaskList.Where(task => task.Status == TaskStatus.Completed));
                this.m_TaskList.Sort(
                (a, b) =>
                {
                    int resultA = !PlayerPrefs.HasKey(LogicController.Instance.PlayerData.PlayerID.ToString() + ":TaskID:" + a.TaskID) ? 1 : -1;
                    int resultB = !PlayerPrefs.HasKey(LogicController.Instance.PlayerData.PlayerID.ToString() + ":TaskID:" + b.TaskID) ? 1 : -1;
                    int result = resultA.CompareTo(resultB);
                    if (result == 0)
                        return a.TaskID.CompareTo(b.TaskID);
                    else
                        return result;
                });
                break;
        }
        m_ReusableScrollView.ReloadData();
    }

    public override int TotalNumberOfCells
    {
        get { return m_TaskList.Count; }
    }

    public override void InitialCell(int index, GameObject cell)
    {
        UITaskItem uiTaskItem = cell.GetComponent<UITaskItem>();
        uiTaskItem.SetItemData(this.m_TaskList[index],this);
    }

    public void ShowTaskModul()
    {
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
    public void HideTaskModul(float? duration = null)
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
    void GetTweenComponent()
    {
        m_TweenAlpha = GetComponent<TweenAlpha>();
    }
    void OnFinished()
    {
        this.gameObject.SetActive(false);
    }
}
