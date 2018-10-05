using UnityEngine;
using System.Collections;

public class UIWindowTask : UIWindowCommon {
    [SerializeField] UITaskModule[] m_UITaskModule;
    private UITaskModule m_CurrentUITaskModule;
    void Awake()
    {
        this.GetTweenComponent();
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public override void ShowWindow()
    {
        //PlayerPrefs.DeleteAll();
        base.ShowWindow();
        this.SetWindowItem();
    }
    public override void HideWindow()
    {
        this.ClearNewTask();
        base.HideWindow();
        if(this.m_CurrentUITaskModule != null)
            this.m_CurrentUITaskModule.HideTaskModul(0.2f);
    }
    public void SetWindowItem()
    {
        if (this.m_CurrentUITaskModule != null)
            if (!this.m_CurrentUITaskModule.Equals(m_UITaskModule[0]))
                this.m_CurrentUITaskModule.HideTaskModul();

        this.m_CurrentUITaskModule = m_UITaskModule[0];
        this.m_CurrentUITaskModule.ShowTaskModul();
        this.m_CurrentUITaskModule.SetModulData();
    }
    void ClearNewTask()
    {
        foreach (Task task in LogicController.Instance.TaskManager.TaskList)
        {
            if (!PlayerPrefs.HasKey(LogicController.Instance.PlayerData.PlayerID.ToString() + ":TaskID:" + task.TaskID))
                PlayerPrefs.SetString(LogicController.Instance.PlayerData.PlayerID.ToString() + ":TaskID:" + task.TaskID, task.TaskID.ToString());
        }
    }
    //button message
    void OnUnCompleteBtn()
    {
        //if (!this.m_CurrentUITaskModule.Equals(m_UITaskModule[0]))
        {

            this.m_CurrentUITaskModule.HideTaskModul();
            this.m_CurrentUITaskModule = m_UITaskModule[0];
            this.m_CurrentUITaskModule.ShowTaskModul();
            this.m_CurrentUITaskModule.SetModulData();
        }
    }
    //button message
    public void OnCompleteBtn()
    {
        //if (!this.m_CurrentUITaskModule.Equals(m_UITaskModule[1]))
        {
            this.m_CurrentUITaskModule.HideTaskModul();
            this.m_CurrentUITaskModule = m_UITaskModule[1];
            this.m_CurrentUITaskModule.ShowTaskModul();
            this.m_CurrentUITaskModule.SetModulData();
        }
    }
}
