using UnityEngine;
using System.Collections;

public class CameraAdaptive : MonoBehaviour { 
    [SerializeField] Vector2 iTouch4CameraSize;
    [SerializeField] Vector2 iPadCameraSize;
    [SerializeField] Vector2 iPhone5CameraSize;
	// Use this for initialization
	void Start () {
        this.OnSize();
	}

    void OnSize()
    {
        switch (ClientSystemConstants.SCREENRESOLUTION)
        {
            case ScreenResolution.Size1024X768:
                ClientSystemConstants.CAMERA_SIZE_MIN = iPadCameraSize.x;
                ClientSystemConstants.CAMERA_SIZE_MAX = iPadCameraSize.y;
                break;
            case ScreenResolution.Size1136X640:
                ClientSystemConstants.CAMERA_SIZE_MIN = iPhone5CameraSize.x;
                ClientSystemConstants.CAMERA_SIZE_MAX = iPhone5CameraSize.y;
                break;
            case ScreenResolution.Size960X640:
                ClientSystemConstants.CAMERA_SIZE_MIN = iTouch4CameraSize.x;
                ClientSystemConstants.CAMERA_SIZE_MAX = iTouch4CameraSize.y;
                break;
        }
    }
}
