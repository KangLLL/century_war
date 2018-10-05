using UnityEngine;
using System.Collections;
using System.Linq;
public class StatisticsValidTaskCount : MonoBehaviour {
    [SerializeField] UILabel m_UILabel;
    [SerializeField] UISprite m_UISprite;//background
    [SerializeField] StatisticsType m_StatisticsType;
    //int m_CurrentCount = -1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.Statistics();
	
	}
    int CompleteTaskCount()
    {
        int count = LogicController.Instance.TaskManager.TaskList.Count(task => task.Status == TaskStatus.Completed);
        this.SetText(count);
        return count;
    }
    int UnCompleteNewTaskCount()
    {
        int count = LogicController.Instance.TaskManager.TaskList.Count(task => task.Status == TaskStatus.Opened && !PlayerPrefs.HasKey(LogicController.Instance.PlayerData.PlayerID.ToString() + ":TaskID:" + task.TaskID));
        this.SetText(count);
        return count;
    }
    int AllTaskCount()
    {
        int count = this.CompleteTaskCount() + this.UnCompleteNewTaskCount();
        this.SetText(count);
        return count;
    }
    void Statistics()
    {
        switch (this.m_StatisticsType)
        {
            case StatisticsType.UnComplete:
                this.UnCompleteNewTaskCount();
                break;
            case StatisticsType.Complete:
                this.CompleteTaskCount();
                break;
            case StatisticsType.All:
                this.AllTaskCount();
                break;
        }
    }
    void SetText(int count)
    {
        if (count > 0)
        {
            this.m_UILabel.alpha = 1;
            this.m_UISprite.alpha = 1;
            this.m_UILabel.text = count.ToString();

        }
        else
        {
            this.m_UILabel.alpha = 0;
            this.m_UISprite.alpha = 0;
        }
    }
}
public enum StatisticsType
{
    Complete,
    UnComplete,
    All
}
