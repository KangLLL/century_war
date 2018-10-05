using UnityEngine;
using System.Collections;

public class UIDockGridRoll : MonoBehaviour
{
    [SerializeField]
    Vector3 iTouch4DockOffset;
    [SerializeField]
    Vector3 iPadDockOffset;
    [SerializeField]
    Vector3 iPhone5DockOffset;
    void Awake()
    {
        this.OnDock();
    }
    void OnDock()
    {

        switch (ClientSystemConstants.SCREENRESOLUTION)
        {
            case ScreenResolution.Size1024X768:
                this.transform.localPosition = iPadDockOffset;
                break;
            case ScreenResolution.Size1136X640:
                this.transform.localPosition = iPhone5DockOffset;
                break;
            case ScreenResolution.Size960X640:
                this.transform.localPosition = iTouch4DockOffset;
                break;
        }
    }
}
