using UnityEngine;
using System.Collections;
using System;
using CommandConsts;

public class CommunicationNotificationCenter : MonoBehaviour 
{
	private static CommunicationNotificationCenter s_Sigleton;
	
	public static CommunicationNotificationCenter Instance
	{
		get { return s_Sigleton; }
	}

	public event Action<AddDefenseObjectResponseParameter> OnAddDefenseObjectResponse;
	private bool m_IsListenDefenseObject;
	
	void Start () 
	{
		s_Sigleton = this;
	}
	
	void OnDestroy()
	{
		s_Sigleton = null;
	}
	
	public void AddDefnseObject(AddDefenseObjectRequestParameter request)
	{
		if(!this.m_IsListenDefenseObject)
		{
			CommunicationUtility.Instance.AddDefenseObject(request, this, "ReceivedDefenseObjectResponse", true);
			this.m_IsListenDefenseObject = true;
		}
		else
		{
			CommunicationUtility.Instance.AddDefenseObject(request);
		}
	}
	
	private void ReceivedDefenseObjectResponse(Hashtable result)
	{
		if(this.OnAddDefenseObjectResponse != null)
		{
			AddDefenseObjectResponseParameter param = new AddDefenseObjectResponseParameter();
			param.InitialParameterObjectFromHashtable(result);
			this.OnAddDefenseObjectResponse(param);
		}
	}
}
