using UnityEngine;
using System.Collections;

public class UIScrollRegionAdaptive : MonoBehaviour 
{
    [SerializeField] Vector4 iTouch4Size;
    [SerializeField] Vector4 iPadSize;
    [SerializeField] Vector4 iPhone5Size;
    
	[SerializeField] UIPanel m_Panel;
	[SerializeField] BoxCollider m_BackgroundCollider;
	[SerializeField] int m_ColliderWidthPad;
	[SerializeField] int m_ColliderHeightPad;
	
    void Awake()
    {
        this.OnSize();
    }

    public void OnSize()
    { 
            switch (ClientSystemConstants.SCREENRESOLUTION)
            {
                case ScreenResolution.Size1024X768: 
                    this.m_Panel.clipRange = iPadSize;//new Vector4(this.m_Panel.clipRange.x, this.m_Panel.clipRange.y,iTouch4Size.x, iTouch4Size.y);
					if(this.m_BackgroundCollider != null)
					{
						this.m_BackgroundCollider.center = new Vector3(this.iPadSize.x, this.iPadSize.y, this.m_BackgroundCollider.center.z);
				 		this.m_BackgroundCollider.size = new Vector3(this.iPadSize.z - this.m_ColliderWidthPad, this.iPadSize.w - this.m_ColliderHeightPad, 1);
					}
                    break;
                case ScreenResolution.Size1136X640: 
                    this.m_Panel.clipRange = iPhone5Size;//new Vector4(this.m_Panel.clipRange.x, this.m_Panel.clipRange.y, iPadSize.x, iPadSize.y);
					if(this.m_BackgroundCollider != null)
					{
						this.m_BackgroundCollider.center = new Vector3(this.iPhone5Size.x, this.iPhone5Size.y, this.m_BackgroundCollider.center.z);
				 		this.m_BackgroundCollider.size = new Vector3(this.iPhone5Size.z - this.m_ColliderWidthPad, this.iPhone5Size.w - this.m_ColliderHeightPad, 1);
					}
                    break;
                case ScreenResolution.Size960X640: 
                    this.m_Panel.clipRange = iTouch4Size;//new Vector4(this.m_Panel.clipRange.x, this.m_Panel.clipRange.y,iPhone5Size.x, iPhone5Size.y);
					if(this.m_BackgroundCollider != null)
					{
						this.m_BackgroundCollider.center = new Vector3(this.iTouch4Size.x, this.iTouch4Size.y, this.m_BackgroundCollider.center.z);
				 		this.m_BackgroundCollider.size = new Vector3(this.iTouch4Size.z - this.m_ColliderWidthPad, this.iTouch4Size.w - this.m_ColliderHeightPad, 1);
					}
                    break;
            }
    }
}
