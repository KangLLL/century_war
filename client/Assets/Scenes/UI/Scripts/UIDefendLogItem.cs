using UnityEngine;
using System.Collections;

public class UIDefendLogItem : MonoBehaviour
{
    [SerializeField] UILabel[] m_UILabel;//level,name,time

    public void SetItemData(LogData logData)
    {
        m_UILabel[0].text = logData.RivalLevel.ToString();
        m_UILabel[1].text = logData.RivalName;
        m_UILabel[2].text = SystemFunction.TimeSpanToString((int)logData.ElapsedTime) + StringConstants.ELAPSED_AGO;
    }
}
