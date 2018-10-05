using UnityEngine;
using System.Collections;

public class UIRootBehavior : MonoBehaviour 
{
	private Camera m_MainCamera;
	private float m_PreviousSize;
	private Vector2 m_localPosition;
	
	void Start () 
	{
		this.m_MainCamera = Camera.main;
		this.m_localPosition = new Vector2(this.transform.localPosition.x, this.transform.localPosition.y);
		this.m_PreviousSize = ClientSystemConstants.CAMARE_ORIGINAL_SIZE;
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		if(this.m_MainCamera.orthographicSize != this.m_PreviousSize)
		{
			this.m_PreviousSize = this.m_MainCamera.orthographicSize;
			float percentage = this.m_PreviousSize / ClientSystemConstants.CAMARE_ORIGINAL_SIZE;
			this.transform.localPosition = new Vector3((this.m_localPosition * percentage).x,
				(this.m_localPosition * percentage).y, 0);
			this.transform.localScale = new Vector3(percentage, percentage, 1);
		}
	}
}