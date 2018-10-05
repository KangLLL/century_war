using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonSpeed : MonoBehaviour 
{
	[SerializeField]
	private UILabel m_TitleLable;
	private string[] m_TitleArray;
	private int m_SpeedIndex;
	
	void Start () 
	{
		this.m_TitleArray = new string[]{
			ClientStringConstants.REPLAY_SPEED_1_TITLE,
			ClientStringConstants.REPLAY_SPEED_2_TITLE,
			ClientStringConstants.REPLAY_SPEED_4_TITLE
		};
		this.m_SpeedIndex = 0;
	}
	
	void OnClick()
	{
		AudioController.Play("ButtonClick");
		this.m_SpeedIndex ++;
		this.m_SpeedIndex = this.m_SpeedIndex == ClientConfigConstants.Instance.ReplayValidScale.Length ? 
			0 : this.m_SpeedIndex;
		
		Time.timeScale = ClientConfigConstants.Instance.ReplayValidScale[this.m_SpeedIndex];
		
		this.m_TitleLable.text = this.m_TitleArray[this.m_SpeedIndex];
	}
	
	public void Clear()
	{
		this.m_SpeedIndex = 0;
		Time.timeScale = ClientConfigConstants.Instance.ReplayValidScale[this.m_SpeedIndex];
		this.m_TitleLable.text = this.m_TitleArray[this.m_SpeedIndex];
	}
}
