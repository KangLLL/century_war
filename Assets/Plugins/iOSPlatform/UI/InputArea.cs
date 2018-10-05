using UnityEngine;
using System.Collections;

[AddComponentMenu("UnityX/Graphics/InputArea")]
public class InputArea : MonoBehaviour
{
    private const int DefaultFontSize = 14;

    public string initialText = "Input Area. Go Go Go! Rock And Roll!";
    public int initialWidth = 200;
    public int initialHeight = 200;
    public int initialFontSize = DefaultFontSize;
    public TextAlignment initialAlignment;
    public Color initialFontColor = Color.white;
    public int initialMaxLength = 140;
    private string m_Text = string.Empty;

    private Transform m_Transform;

    private int m_ControlID;

    public string Text
    {
        get
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                m_Text = _GetInputAreaText(m_ControlID);
            }

            return m_Text;
        }
        set
        {
            m_Text = value;

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                _SetInputAreaText(m_ControlID, m_Text);
            }
        }
    }

    private Color m_FontColor = Color.white;
    public Color FontColor
    {
        get
        {
            return m_FontColor;
        }
        set
        {
            m_FontColor = value;

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                _SetInputAreaFontColor(m_ControlID, m_FontColor.r, m_FontColor.g, m_FontColor.b, m_FontColor.a);
            }
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
                    _SetInputAreaWidth(m_ControlID, m_Width);
                }
                else
                {
                    _SetInputAreaWidth(m_ControlID, m_Width * 2);
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
                    _SetInputAreaHeight(m_ControlID, m_Height);
                }
                else
                {
                    _SetInputAreaHeight(m_ControlID, m_Height * 2);
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
                    _SetInputAreaPosition(m_ControlID, screenPosition.x, screenPosition.y);
                }
                else
                {
                    _SetInputAreaPosition(m_ControlID, screenPosition.x * 2, screenPosition.y * 2);
                }
            }
#endif
        }
    }

    private int m_FontSize;
    public int FontSize
    {
        get
        {
            return m_FontSize;
        }
        set
        {
            m_FontSize = value;

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                _SetInputAreaFontSize(m_ControlID, m_FontSize);
            }

        }
    }

    private int m_MaxLength;
    public int MaxLength
    {
        get
        {
            return m_MaxLength;
        }
        set
        {
            m_MaxLength = value;

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                _SetInputAreaMaxLength(m_ControlID, m_MaxLength);
            }
        }
    }

    private TextAlignment m_Alignment;
    public TextAlignment Alignment
    {
        get
        {
            return m_Alignment;
        }
        set
        {
            m_Alignment = value;

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                _SetInputAreaAlignment(m_ControlID, (int)m_Alignment);
            }
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
                m_Focused = _GetInputAreaFocus(m_ControlID);
            }
            return m_Focused;
        }
		set
		{
			 m_Focused = value;
			 if (Application.platform == RuntimePlatform.IPhonePlayer)
	        {
	            _SetInputAreaFocus(m_ControlID,value);
	        }
        	if (Application.platform == RuntimePlatform.WindowsPlayer)
        	{
				if(m_Focused)
				{
            		GUI.FocusControl(this.gameObject.GetInstanceID().ToString());
				}
        	}
		}
    }

    public GUIStyle m_GuiSytle;

    void Awake()
    {
        m_Transform = this.transform;
        m_Text = this.initialText;
        m_FontColor = this.initialFontColor;
        m_FontSize = this.initialFontSize;
        m_MaxLength = this.initialMaxLength;
        m_Alignment = this.initialAlignment;
        m_Width = this.initialWidth;
        m_Height = this.initialHeight;
       
        m_GuiSytle.wordWrap = true;
        m_GuiSytle.normal.textColor = m_FontColor;
        m_GuiSytle.fontSize = m_FontSize;
        switch (m_Alignment)
        {
            case TextAlignment.Center:
                {
                    m_GuiSytle.alignment = TextAnchor.UpperCenter;
                }
                break;
            case TextAlignment.Left:
                {
                    m_GuiSytle.alignment = TextAnchor.UpperLeft;
                }
                break;
            case TextAlignment.Right:
                {
                    m_GuiSytle.alignment = TextAnchor.UpperRight;
                }
                break;
        }
        m_GuiSytle.normal.textColor = m_FontColor;
#if UNITY_IPHONE
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(m_Transform.position);
            if (UnityXConfig.IsRetina)
            {
                m_ControlID = _CreateNativeInputArea(m_Text, screenPosition.x, screenPosition.y, m_FontColor.r, m_FontColor.g, m_FontColor.b, m_FontColor.a, m_FontSize, m_MaxLength, (int)m_Alignment, m_Width, m_Height);
            }
            else
            {
                m_ControlID = _CreateNativeInputArea(m_Text, screenPosition.x * 2, screenPosition.y * 2, m_FontColor.r, m_FontColor.g, m_FontColor.b, m_FontColor.a, m_FontSize, m_MaxLength, (int)m_Alignment, m_Width * 2, m_Height * 2);
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
                if (UnityXConfig.IsRetina)
                {
                    _SetInputAreaPosition(m_ControlID, screenPosition.x, screenPosition.y);
                }
                else
                {
                    _SetInputAreaPosition(m_ControlID, screenPosition.x * 2, screenPosition.y * 2);
                }
                m_LastPosition = position;
            }
        }
    }
#endif

#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX

    void OnGUI()
    {
        if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);

            int width = m_Width * Screen.width / 960;
            int height = m_Height * Screen.width / 960;
            m_GuiSytle.fontSize = m_FontSize * Screen.width / 960;
            m_GuiSytle.normal.textColor = m_FontColor;
            switch (m_Alignment)
            {
                case TextAlignment.Center:
                    {
                        m_GuiSytle.alignment = TextAnchor.UpperCenter;
                    }
                    break;
                case TextAlignment.Left:
                    {
                        m_GuiSytle.alignment = TextAnchor.UpperLeft;
                    }
                    break;
                case TextAlignment.Right:
                    {
                        m_GuiSytle.alignment = TextAnchor.UpperRight;
                    }
                    break;
            }
            GUI.SetNextControlName(this.gameObject.GetInstanceID().ToString());
            m_Text = GUI.TextArea(new Rect(screenPosition.x - width / 2, Screen.height - (screenPosition.y - height / 2) - height, width, height), m_Text, m_MaxLength, m_GuiSytle);
        }
    }

#endif

    void OnDestroy()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _DestroyNativeInputArea(m_ControlID);
        }
    }



    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern int _CreateNativeInputArea(string tex, float positionX, float positionY, float fontColorR, float fontColorG, float fontColorB, float fontColorA, int fontSize, int maxLength, int alignment, int width, int height);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _DestroyNativeInputArea(int controlID);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _SetInputAreaPosition(int controlID, float x, float y);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _SetInputAreaFontSize(int controlID, int fontSize);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _SetInputAreaWidth(int controlID, int width);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _SetInputAreaHeight(int controlID, int height);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern string _GetInputAreaText(int controlID);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _SetInputAreaText(int controlID, string text);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _SetInputAreaFontColor(int controlID, float ColorR, float ColorG, float ColorB, float ColorA);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _SetInputAreaAlignment(int controlID, int alignment);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _SetInputAreaMaxLength(int controlID, int maxLength);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern bool _GetInputAreaFocus(int controlID);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _SetInputAreaFocus(int controlID, bool focusValue);
}
