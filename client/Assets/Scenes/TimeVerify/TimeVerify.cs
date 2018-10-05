using UnityEngine;
using System.Collections;
using CommandConsts;

public class TimeVerify : MonoBehaviour 
{
	private float m_LastClientTime;
	private long m_LastServerTime;
	
	void OnClick()
	{
		float now = Time.realtimeSinceStartup;
		//Debug.Log("The client time is:" + now);
		CommunicationUtility.Instance.VerifyTime(this, "ServerTimeReceived", true);
		Debug.Log("The client delta time is:" + (now - this.m_LastClientTime));
		this.m_LastClientTime = now;
	}
	
	void ServerTimeReceived(Hashtable result)
	{
		TimeVerifyResponseParameter response = new TimeVerifyResponseParameter();
		response.InitialParameterObjectFromHashtable(result);
		//Debug.Log("the server tick is:" + response.ServerTick);
		long now = response.ServerTick;
		
		long delta = now - this.m_LastServerTime;
		float deltaValue = (float)(delta / (double)System.TimeSpan.TicksPerSecond);
		Debug.Log("The server delta time is:" + deltaValue);
		this.m_LastServerTime = now;
		
	}
}
