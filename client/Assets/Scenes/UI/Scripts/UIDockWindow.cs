using UnityEngine;
using System.Collections;

public class UIDockWindow : MonoBehaviour
{
    [SerializeField] Vector2 iTouch4Size;
    [SerializeField] Vector2 iPadSize;
    [SerializeField] Vector2 iPhone5Size;
    [SerializeField] Transform m_Window; 
    void Awake()
    {
        this.OnSize();
    }
    void OnSize()
    { 
            switch (ClientSystemConstants.SCREENRESOLUTION)
            {
                case ScreenResolution.Size1024X768: 
                    m_Window.localScale = iPadSize;
                    break;
                case ScreenResolution.Size1136X640: 
                    m_Window.localScale = iPhone5Size;
                    break;
                case ScreenResolution.Size960X640: 
                    m_Window.localScale = iTouch4Size;
                    break;
            }
    }
    

}
