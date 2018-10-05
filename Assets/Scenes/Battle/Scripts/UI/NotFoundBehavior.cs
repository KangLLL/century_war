using UnityEngine;
using System.Collections;

public class NotFoundBehavior : MonoBehaviour 
{
	[SerializeField]
	private UILabel m_TimeLable;
	[SerializeField]
	private int m_DisplaySeconds;
	[SerializeField]
	private string m_ShowTimeFormat;
	
	private int m_DisplayTicks;
	private int m_CurrentTick;
	
	public void Show()
	{
		this.gameObject.SetActive(true);
		LockScreen.Instance.EnableInput();
	}
	
	void OnClick()
	{
		Application.LoadLevel(ClientStringConstants.BUILDING_SCENE_LEVEL_NAME);
	}

	void Start () 
	{
		this.m_DisplayTicks = this.m_DisplaySeconds * ClientConfigConstants.Instance.TicksPerSecond;
		this.m_CurrentTick = 0;
	}

	void FixedUpdate () 
	{
		if(this.m_CurrentTick == this.m_DisplayTicks)
		{
			Application.LoadLevel(ClientStringConstants.BUILDING_SCENE_LEVEL_NAME);
		}
		else
		{
			int remainTick = this.m_DisplayTicks - this.m_CurrentTick;
			int second = Mathf.RoundToInt(remainTick / (float)ClientConfigConstants.Instance.TicksPerSecond);
			
			this.m_TimeLable.text = string.Format(this.m_ShowTimeFormat, second);
			this.m_CurrentTick ++;
		}
	}
}
