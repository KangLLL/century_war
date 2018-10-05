using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class UIWindowDefendLog : UIWindowCommon
{
    [SerializeField] UIDefendLogModul m_UIDefendLogModul;
    [SerializeField] UILabel m_UILabel;//Honour
    void Awake()
    {
        this.GetTweenComponent();
    }

    public override void HideWindow()
    {
        base.HideWindow();
    }
    public override void ShowWindow()
    {
        //PlayerPrefs.DeleteAll(); 
        if (this.SetWindowItem().Length > 0)
            base.ShowWindow();
    }
    protected override void GetTweenComponent()
    {
        base.GetTweenComponent();
    }
    LogData[] SetWindowItem()
    {
        List<LogData> logDataList = new List<LogData>(LogicController.Instance.PlayerData.DefenseLogs.Where(ld => !PlayerPrefs.HasKey("MatchID:" + ld.MatchID.ToString())));
        
        logDataList.Sort((a, b) => a.ElapsedTime.CompareTo(b.ElapsedTime));
        LogData[] logDatas = logDataList.ToArray();
        this.m_UIDefendLogModul.SetModulData(logDatas);
        int sum = 0;
        foreach (LogData ld in logDatas)
        {
            PlayerPrefs.SetString("MatchID:" + ld.MatchID.ToString(), ld.MatchID.ToString());
            sum += ld.PlunderHonour;
        }
        //int sum = logDatas.Sum(ld => ld.PlunderHonour);
        m_UILabel.text = sum >= 0 ? sum.ToString() : sum.ToString().Insert(1, " ");
        return logDatas;
    }
}
