using UnityEngine;
using System.Collections;

public class TimeTips : MonoBehaviour 
{
	[SerializeField]
	protected UILabel m_TimesValueLabel;

	protected void DisplayTime(int remainingSeconds)
    {
        int minute = remainingSeconds / 60;
        int second = remainingSeconds % 60;
        if (minute > 0)
        {
            this.m_TimesValueLabel.text = string.Format("{0}分{1}秒", minute, second);
        }
        else
        {
            this.m_TimesValueLabel.text = string.Format("{0}秒", second);
        }
    }
}
