using UnityEngine;
using System.Collections;
using ConfigUtilities;

public class TimeTipsBehavior : TimeTips 
{
	[SerializeField]
	private UILabel m_TimesTitleLabel;
	
	private int m_MatchTotalFrame;
	private int m_MatchObserveTotalFrame;

	// Use this for initialization
	void Start () 
	{
		this.HideTime();
		
		this.m_MatchTotalFrame = ClientConfigConstants.Instance.TicksPerSecond * ConfigInterface.Instance.SystemConfig.MatchDurationSecond;
		this.m_MatchObserveTotalFrame = ClientConfigConstants.Instance.TicksPerSecond * ConfigInterface.Instance.SystemConfig.MatchObserveLimitSecond;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(this.m_TimesTitleLabel.enabled && !BattleDirector.Instance.IsBattleFinished)
		{
			if(BattleDirector.Instance.IsBattleStart)
			{
				int matchElapsedFrame = TimeTickRecorder.Instance.CurrentTimeTick - BattleDirector.Instance.MatchStartTick;
				int remainingMatchFrames = this.m_MatchTotalFrame - matchElapsedFrame;
				int remainingMatchSeconds = Mathf.RoundToInt((float)remainingMatchFrames / ClientConfigConstants.Instance.TicksPerSecond);
				this.DisplayTime(remainingMatchSeconds);
			}
			else
			{
				int observeElapsedFrame = TimeTickRecorder.Instance.CurrentTimeTick - BattleDirector.Instance.MatchObserveStartTick;
				int remainingObserveFrames = this.m_MatchObserveTotalFrame - observeElapsedFrame;
				int remainingObserveSeconds = Mathf.RoundToInt((float)remainingObserveFrames / ClientConfigConstants.Instance.TicksPerSecond);
				this.DisplayTime(remainingObserveSeconds);
			}
		}
	}
	
	public void ShowTime()
	{
		this.m_TimesTitleLabel.enabled = true;
		this.m_TimesValueLabel.enabled = true;
	}
	
	public void HideTime()
	{
		this.m_TimesTitleLabel.enabled = false;
		this.m_TimesValueLabel.enabled = false;
	}
}
