using UnityEngine;
using System.Collections;

public class WaitingBehavior : MonoBehaviour 
{	
	[SerializeField]
	private UILabel m_RemainingTimeLable;
	[SerializeField]
	private UISlider m_Progress;
	[SerializeField]
	private int m_FirstWaitMinSeconds;
	[SerializeField]
	private int m_FirstWaitMaxSeconds;
	[SerializeField]
	private int m_SecondWaitMinSeconds;
	[SerializeField]
	private int m_SecondWaitMaxSeconds;
	[SerializeField]
	private int m_LastWaitSeconds;
	
	private int m_WaitCount;
	private int m_WaitTotalSeconds;
	private float m_StartTime;
	
	// Use this for initialization
	void Start () 
	{
		this.m_WaitTotalSeconds = CommonHelper.GetRandomNumber(this.m_FirstWaitMinSeconds, this.m_FirstWaitMaxSeconds + 1);
		this.m_StartTime = Time.realtimeSinceStartup;
		this.m_WaitCount = 1;
	}
	
	
	// Update is called once per frame
	void Update () 
	{
		float elapsedTime = Time.realtimeSinceStartup - this.m_StartTime;
		if(elapsedTime >= this.m_WaitTotalSeconds)
		{
			this.m_StartTime = Time.realtimeSinceStartup;
			this.m_WaitTotalSeconds = this.m_WaitCount > 1 ? this.m_LastWaitSeconds :
				CommonHelper.GetRandomNumber(this.m_SecondWaitMinSeconds, this.m_SecondWaitMaxSeconds + 1);
			this.m_RemainingTimeLable.text = SystemFunction.TimeSpanToString(this.m_WaitTotalSeconds);
			if(this.m_Progress != null)
			{
				this.m_Progress.sliderValue = 0;
			}
			this.m_WaitCount ++;
		}
		else
		{
			this.m_RemainingTimeLable.text = SystemFunction.TimeSpanToString(Mathf.CeilToInt(this.m_WaitTotalSeconds - elapsedTime));
			if(this.m_Progress != null)
			{
				this.m_Progress.sliderValue = elapsedTime / this.m_WaitTotalSeconds;
			}
		}
	}
	
	public void Show()
	{
		AudioController.Play("WindowShow");
		this.gameObject.SetActive(true);
	    
		this.gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 1);
	    iTween.ScaleTo(this.gameObject, iTween.Hash(iT.ScaleTo.scale, Vector3.one, iT.ScaleTo.easetype, iTween.EaseType.easeOutBack, iT.ScaleTo.time, 0.2f));
	}
}
