using UnityEngine;
using System.Collections;

public class TimeTicks : TimeTips
{
    void Start()
    {
        this.m_TimesValueLabel.enabled = false;
    }

    void Update()
    {
        if (ReplayDirector.Instance.IsReplayStart)
        {
            if (!this.m_TimesValueLabel.enabled)
            {
                this.m_TimesValueLabel.enabled = true;
            }

            int totalFrames = ReplayDirector.Instance.TotalReplayTick;
            int elapsedFrames = TimeTickRecorder.Instance.CurrentTimeTick - ReplayDirector.Instance.ReplayStartTick;

            int seconds = Mathf.RoundToInt((totalFrames - elapsedFrames) / (float)ClientConfigConstants.Instance.TicksPerSecond);
            this.DisplayTime(seconds);
        }
    }
}