using UnityEngine;
using System.Collections;

public class UICameraSize : MonoBehaviour {
	[SerializeField] Camera m_UICamera;
	void Awake()
	{
		this.OnSize();
	}
	void OnSize()
	{ 
            switch (ClientSystemConstants.SCREENRESOLUTION)
            {
                case ScreenResolution.Size1024X768:
                    m_UICamera.orthographicSize = 384;
                    break;
                case ScreenResolution.Size1136X640:
                    m_UICamera.orthographicSize = 320;
                    break;
                case ScreenResolution.Size960X640:
                    m_UICamera.orthographicSize = 320;
                    break;
            }
	}
	
}
