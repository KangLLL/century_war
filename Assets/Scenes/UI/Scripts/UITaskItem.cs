using UnityEngine;
using System.Collections;

public class UITaskItem : MonoBehaviour {
    [SerializeField]
    UILabel[] m_UILabel;//0 = task name;1 = task description;
    [SerializeField] GameObject[] m_AwardObject;//0 = gold; 1 = food; 2 = gem ; 3 = oil; 4= exp
    [SerializeField] UILabel[] m_UILabelAward;//0 = gold; 1 = food; 2 = gem ; 3 = oil; 4= exp
    [SerializeField] UILabel m_UILabelTaskRemainingTime;//remaining time
    [SerializeField]
    GameObject[] m_ButtonAward;//0 = unComplete; 1 = complete;
    [SerializeField] UISprite[] m_UISpriteBackground;//0 = unComplete; 1 = complete;
    [SerializeField] UISprite m_UISpriteNewTask;
    [SerializeField] Vector3 m_RowInterval;
    [SerializeField] Vector3 m_ColumnInterval;
    Vector3 m_IniLocalPosition = new Vector3(0, 0, -3);
    Task m_Task;
    UITaskModule m_UITaskModule;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.OnTimer();
	}

    public void SetItemData(Task task, UITaskModule uiTaskModule)
    { 
        this.m_Task = task;
        this.m_UITaskModule = uiTaskModule;
        m_UILabel[0].text = task.TaskConfigData.Name;
        m_UILabel[1].text = task.Description;
        switch (task.Status)
        {
            case TaskStatus.Opened:
                m_ButtonAward[0].SetActive(false);
                //m_ButtonAward[1].SetActive(false);
                m_UISpriteBackground[0].alpha = 1;
                m_UISpriteBackground[1].alpha = 0;
                break;
            case TaskStatus.Completed:
                m_ButtonAward[0].SetActive(true);
                //m_ButtonAward[1].SetActive(true);
                m_UISpriteBackground[0].alpha = 0;
                m_UISpriteBackground[1].alpha = 1;
                break;
        }

        int[] awardArray = SystemFunction.ConverTObjectToArray(task.TaskConfigData.RewardGold,
                                                               task.TaskConfigData.RewardFood,
                                                               task.TaskConfigData.RewardGem,
                                                               task.TaskConfigData.RewardOil,
                                                               task.TaskConfigData.RewardExp);
        for (int i = 0 ,order = 0; i < awardArray.Length; i++)
        {
            if (awardArray[i] <= 0)
                m_AwardObject[i].SetActive(false);
            else
            {
                m_AwardObject[i].SetActive(true);
                m_UILabelAward[i].text = awardArray[i].ToString();
                int row = order / 2;
                int column = order % 2; 
                m_AwardObject[i].transform.localPosition = m_IniLocalPosition + row * m_RowInterval + column * m_ColumnInterval; 
                order++;
            }
        }
        if (PlayerPrefs.HasKey(LogicController.Instance.PlayerData.PlayerID.ToString() + ":TaskID:" + task.TaskID))
            m_UISpriteNewTask.alpha = 0;
        else 
            m_UISpriteNewTask.alpha = 1;
        m_UILabelTaskRemainingTime.gameObject.SetActive(task.RemainingSeconds.HasValue); 
    }
    void OnTimer()
    {
        if (m_Task == null)
            return;
        if (m_Task.RemainingSeconds.HasValue)
        {
            if (m_Task.RemainingSeconds > 0)
                m_UILabelTaskRemainingTime.text = StringConstants.PROMT_TASK_REMAINING_TIME + SystemFunction.TimeSpanToString(m_Task.RemainingSeconds.Value);
            else
            {
                if (this.m_UITaskModule != null)
                    this.m_UITaskModule.SetModulData();
            }
        }
    }
    void CompleteTask()
    {
        if (!this.enabled)
            return;
        if (this.m_Task.Status == TaskStatus.Completed)
        {
            if (this.RewardCheck())
            {
                //UIManager.Instance.UIWindowTask.HideWindow();
                LogicController.Instance.AwardTask(this.m_Task);
                //UIManager.Instance.UIWindowTask.SetWindowItem();
                UIManager.Instance.UIWindowTask.OnCompleteBtn();
                AudioController.Play("CompleteTask");
                this.enabled = false;
            }
        }
    }
    bool RewardCheck()
    {
        bool result = true;
        if (this.m_Task.TaskConfigData.RewardGold + LogicController.Instance.PlayerData.CurrentStoreGold > LogicController.Instance.PlayerData.GoldMaxCapacity)
        {
            UIErrorMessage.Instance.ErrorMessage(20, StringConstants.RESOURCE_GOLD);
            result = false;
        }
        if (this.m_Task.TaskConfigData.RewardFood + LogicController.Instance.PlayerData.CurrentStoreFood > LogicController.Instance.PlayerData.FoodMaxCapacity)
        {
            UIErrorMessage.Instance.ErrorMessage(20, StringConstants.RESOURCE_FOOD);
            result = false;
        }
        if (this.m_Task.TaskConfigData.RewardOil + LogicController.Instance.PlayerData.CurrentStoreOil > LogicController.Instance.PlayerData.OilMaxCapacity)
        {
            UIErrorMessage.Instance.ErrorMessage(20, StringConstants.RESOURCE_OIL);
            result = false;
        }
        return result;
    }
}
