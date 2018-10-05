using UnityEngine;
using System.Collections;

public class ButtonReplay : MonoBehaviour 
{
	[SerializeField]
	private int m_ButtonCDTicks;
	
	private int m_CurrentTick;
	
	void OnClick()
	{
		AudioController.Play("ButtonClick");
		if(this.m_CurrentTick == 0)
		{
			ReplayDirector.Instance.Replay();
			this.m_CurrentTick = this.m_ButtonCDTicks;
		}
	}
	
	void Update()
	{
		if(this.m_CurrentTick > 0)
		{
			this.m_CurrentTick --;
		}
	}
}
