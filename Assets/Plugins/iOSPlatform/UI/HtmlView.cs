using UnityEngine;
using System.Collections;


[AddComponentMenu("UnityX/Graphics/HtmlView")]
public class HtmlView : MonoBehaviour
{
    private const int DefaultFontSize = 14;

    public string initialURL = "about:blank";
    public int initialWidth = 480;
    public int initialHeight = 320;

    private Transform m_Transform;

    private int m_ControlID;

    private string m_URL;
    public string URL
    {
        get
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                m_URL = _GetHtmlViewURL(m_ControlID);
            }

            return m_URL;
        }
        set
        {
            m_URL = value;

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                _SetHtmlViewURL(m_ControlID, m_URL);
            }
        }
    }

	private string m_RequestURL;
	public string RequestURL
	{
		get
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                m_RequestURL = _GetHtmlViewRequestURL(m_ControlID);
            }
            return m_RequestURL;
		}
	}

    private int m_Width;
    public int Width
    {
        get
        {
            return m_Width;
        }
        set
        {
            m_Width = value;
#if UNITY_IPHONE
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (UnityXConfig.IsRetina)
                {
                    _SetHtmlViewWidth(m_ControlID, m_Width);
                }
                else
                {
                    _SetHtmlViewWidth(m_ControlID, m_Width * 2);
                }
            }
#endif
      }
    }

    private int m_Height;
    public int Height
    {
        get
        {
            return m_Height;
        }
        set
        {
            m_Height = value;          
#if UNITY_IPHONE
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (UnityXConfig.IsRetina)
                {
                    _SetHtmlViewHeight(m_ControlID, m_Height);
                }
                else
                {
                    _SetHtmlViewHeight(m_ControlID, m_Height * 2);
                }
            }
#endif
        }
    }

    public Vector2 Position
    {
        get
        {
            return new Vector2(m_Transform.position.x, m_Transform.position.y);
        }
        set
        {
            Vector3 position = m_Transform.position;
            position.x = value.x;
            position.y = value.y;
#if UNITY_IPHONE
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Vector2 screenPosition = Camera.main.WorldToScreenPoint(m_Transform.position);
                if (UnityXConfig.IsRetina)
                {
                    _SetHtmlViewPosition(m_ControlID, screenPosition.x, screenPosition.y);
                }
                else
                {
                    _SetHtmlViewPosition(m_ControlID, screenPosition.x * 2, screenPosition.y * 2);
                }
            }
#endif
        }
    }

    private bool m_Focused;
    public bool Focused
    {
        get
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                m_Focused = GUI.GetNameOfFocusedControl() == this.gameObject.GetInstanceID().ToString();
            }

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                m_Focused = _GetHtmlViewFocus(m_ControlID);
            }
            return m_Focused;
        }
		set
		{
			m_Focused = value;
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				_SetHtmlViewFocus(m_ControlID,value);
			}
			else
			{
				if(m_Focused)
				{
			        if (Application.platform == RuntimePlatform.WindowsPlayer)
			        {
			            GUI.FocusControl(this.gameObject.GetInstanceID().ToString());
			        }
				}
			}
		}
    }
	
	public HtmlViewState Status
	{
		get
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
        	{
            	return HtmlViewState.WebLoadFailed;
        	}
			else	
			{
				return (HtmlViewState)(_GetHtmlViewStatus(m_ControlID));
			}
		}
	}

    void Start()
    {
        m_Transform = this.transform;
        m_URL = this.initialURL;

        m_Width = this.initialWidth;
        m_Height = this.initialHeight;
#if UNITY_IPHONE
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(m_Transform.position);
            if (UnityXConfig.IsRetina)
            {
                m_ControlID = _CreateNativeHtmlView(m_URL, screenPosition.x, screenPosition.y, m_Width, m_Height);
            }
            else
            {
                m_ControlID = _CreateNativeHtmlView(m_URL, screenPosition.x * 2, screenPosition.y * 2, m_Width * 2, m_Height * 2);
            }
        }
#endif
    }

#if UNITY_IPHONE
    Vector3 m_LastPosition = new Vector3();
    void Update()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Vector3 position = m_Transform.position;
            if (position != m_LastPosition)
            {
                Vector2 screenPosition = Camera.main.WorldToScreenPoint(m_Transform.position);
                _SetHtmlViewPosition(m_ControlID, screenPosition.x, screenPosition.y);
                m_LastPosition = position;
            }
        }
    }
#endif

#if !UNITY_IPHONE
    private GUIStyle m_GuiSytle = new GUIStyle();
    void OnGUI()
    {
        if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);
          
            int width = m_Width * Screen.width / 960;
            int height = 62 * Screen.width / 960;
            GUI.SetNextControlName(this.gameObject.GetInstanceID().ToString());
            GUI.Label(new Rect(screenPosition.x - width / 2, Screen.height - (screenPosition.y - height / 2) - height, width, height), "HtmlView only display on the device!", m_GuiSytle);
            
        }
    }

#endif

    void OnDestroy()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _DestroyNativeHtmlView(m_ControlID);
        }
    }



    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern int _CreateNativeHtmlView(string url, float positionX, float positionY, int width, int height);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _DestroyNativeHtmlView(int controlID);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _SetHtmlViewPosition(int controlID, float x, float y);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _SetHtmlViewWidth(int controlID, int width);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _SetHtmlViewHeight(int controlID, int height);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern string _GetHtmlViewURL(int controlID);
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern string _GetHtmlViewRequestURL(int controlID);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _SetHtmlViewURL(int controlID, string url);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern bool _GetHtmlViewFocus(int controlID);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _SetHtmlViewFocus(int controlID, bool focusValue);
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
	private static extern int _GetHtmlViewStatus(int controlID);
}
