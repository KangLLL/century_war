using UnityEngine;
using System.Collections;

public class UIDockGrid : MonoBehaviour {
	[SerializeField] Vector2 iTouch4DockOffset;
	[SerializeField] Vector2 iPadDockOffset;
	[SerializeField] Vector2 iPhone5DockOffset;
    Vector2 iTouch4Size = new Vector2(960,640); 
    Vector2 iPadSize = new Vector2(1024,768);
    Vector2 iPhone5Size = new Vector2(1136,640);
	[SerializeField] Side m_DockType;

    void Awake()
    {
        this.OnDock();
    }
 
    void OnDock()
    { 
	 
            switch (ClientSystemConstants.SCREENRESOLUTION)
            {
                case ScreenResolution.Size1024X768:
                    this.Dock(iPadSize * 0.5f , iPadDockOffset);
                    break;
                case ScreenResolution.Size1136X640:
                    this.Dock(iPhone5Size * 0.5f , iPhone5DockOffset);
                    break;
                case ScreenResolution.Size960X640:
                    this.Dock(iTouch4Size * 0.5f , iTouch4DockOffset);
                    break; 
            } 
    }
				
    void Dock(Vector2 size, Vector2 offset)
    {
	  		Vector3 localPosition = Vector3.zero;
		    if(m_DockType == Side.Bottom || m_DockType == Side.BottomLeft || m_DockType == Side.BottomRight)
                localPosition.y = -size.y + offset.y; 
			
			if(m_DockType == Side.Top || m_DockType == Side.TopLeft || m_DockType == Side.TopRight)
                localPosition.y = size.y + offset.y;  
		
		    if(m_DockType == Side.BottomLeft || m_DockType == Side.TopLeft || m_DockType == Side.Left)
                localPosition.x = -size.x + offset.x; 
		    
		    if(m_DockType == Side.BottomRight || m_DockType == Side.TopRight || m_DockType == Side.Right)
                localPosition.x = size.x + offset.x; 
		   
		    if(m_DockType == Side.Bottom || m_DockType == Side.Top)
                localPosition.x = 0 +offset.x;
            if (m_DockType == Side.Center)
            {
                localPosition.x = 0 + offset.x;
                localPosition.y = 0 + offset.y;
            }
		    this.transform.localPosition = localPosition;
		
    }
}
public enum Side
{
    BottomLeft,
    Left,
    TopLeft,
    Top,
    TopRight,
    Right,
    BottomRight,
    Bottom,
    Center,
}