using UnityEngine;
using System.Collections;

public class CameraInitializer : MonoBehaviour 
{
	[SerializeField]
	private Camera m_Camera;
	
	private Vector3 m_InitialPosition;
	private float m_InitialSize;
	
	void Start () 
	{
		this.m_InitialPosition = this.m_Camera.transform.position;
		this.m_InitialSize = this.m_Camera.orthographicSize;
	}
	
	public void Reset()
	{
		this.m_Camera.transform.position = this.m_InitialPosition;
		this.m_Camera.orthographicSize = this.m_InitialSize;
	}
}
