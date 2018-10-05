using UnityEngine;
using System.Collections;

public class UILimit : MonoBehaviour {
    float m_CameraSize;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    
	void LateUpdate() {
       LimitCamera();
	
	}
    void LimitCamera()
    {
        if (Camera.main.orthographicSize != m_CameraSize)
        {
            float scale = Camera.main.orthographicSize / ClientSystemConstants.CAMERA_SIZE_STANDARD;
            Vector3 localScale = this.transform.localScale;
            localScale.x = scale;
            localScale.y = scale;
            this.transform.localScale = localScale;
            m_CameraSize = scale;
        }
    }
}
