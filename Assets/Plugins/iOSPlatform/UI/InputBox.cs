using UnityEngine;
using System.Collections;

[AddComponentMenu("UnityX/Graphics/InputBox")]
public class InputBox : MonoBehaviour
{
    private const int DefaultFontSize = 14;

    public string initialText = "Input Box";
    public int initialWidth = 160;
    public int initialFontSize = DefaultFontSize;
    public TextAlignment initialAlignment;
    public Color initialFontColor = Color.white;
    public int initialMaxLength = 16;
    private string m_Text = string.Empty;

    private Transform m_Transform;

    private int m_ControlID;

    public string Text
    {
        get
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                m_Text = _GetInputBoxText(m_ControlID);
            }

            return m_Text;
        }
        set
        {
            m_Text = value;

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                _SetInputBoxText(m_ControlID, m_Text);
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
                _SetInputBoxFontColor(m_ControlID, m_FontColor.r, m_FontColor.g, m_FontColor.b, m_FontColor.a);
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
                    _SetInputBoxWidth(m_ControlID, m_Width);
                }
                else
                {
                    _SetInputBoxWidth(m_ControlID, m_Width * 2);
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
                    _SetInputBoxPosition(m_ControlID, screenPosition.x, screenPosition.y);
                }
                else
                {
                    _SetInputBoxPosition(m_ControlID, screenPosition.x * 2, screenPosition.y * 2);
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
                _SetInputBoxFontSize(m_ControlID, m_FontSize);
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
                _SetInputBoxMaxLength(m_ControlID, m_MaxLength);
            }
        }
    }
	
	private KeyboardType m_KeyboardType;
	public KeyboardType KeyboardType
	{
		get
		{
			return m_KeyboardType;
		}
		set
		{
			m_KeyboardType = value;
			
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                _SetInputBoxKeyboardType(m_ControlID, (int)m_KeyboardType);
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
                _SetInputBoxAlignment(m_ControlID, (int)m_Alignment);
            }
        }
    }

    private bool m_GetFocusFlag;
    private bool m_RemoveFocusFlag;
    public bool Focused
    {
        get
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return _GetInputBoxFocus(m_ControlID);
            }
            else
            {
                return GUI.GetNameOfFocusedControl() == this.gameObject.GetInstanceID().ToString();
            }
        }
		set
		{
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                _SetInputBoxFocus(m_ControlID, value);
            }
            else
            {
                if (value)
                {
                    m_GetFocusFlag = true;
                }
                else
                {
                    m_RemoveFocusFlag = true;
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

        
        m_GuiSytle.normal.textColor = m_FontColor;
        m_GuiSytle.fontSize = m_FontSize;
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
#if UNITY_IPHONE
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(m_Transform.position);
            if (UnityXConfig.IsRetina)
            {
                m_ControlID = _CreateNativeInputBox(m_Text, screenPosition.x, screenPosition.y, m_FontColor.r, m_FontColor.g, m_FontColor.b, m_FontColor.a, m_FontSize, m_MaxLength, (int)m_Alignment, m_Width);
            }
            else
            {
                m_ControlID = _CreateNativeInputBox(m_Text, screenPosition.x * 2, screenPosition.y * 2, m_FontColor.r, m_FontColor.g, m_FontColor.b, m_FontColor.a, m_FontSize, m_MaxLength, (int)m_Alignment, m_Width * 2);
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
                    _SetInputBoxPosition(m_ControlID, screenPosition.x, screenPosition.y);
                }
                else
                {
                    _SetInputBoxPosition(m_ControlID, screenPosition.x * 2, screenPosition.y * 2);
                }
                m_LastPosition = position;
            }
        }
    }
#endif


#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX

    void OnGUI()
    {
        //if (Application.platform != RuntimePlatform.IPhonePlayer)
        //{
        //    Vector2 screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);

        //    int width = m_Width * Screen.width / 960;
        //    int height = 62 * Screen.width / 960;
        //    m_GuiSytle.fontSize = m_FontSize * Screen.width / 960;
        //    m_GuiSytle.normal.textColor = m_FontColor;
        //    switch (m_Alignment)
        //    {
        //        case TextAlignment.Center:
        //            {
        //                m_GuiSytle.alignment = TextAnchor.UpperCenter;
        //            }
        //            break;
        //        case TextAlignment.Left:
        //            {
        //                m_GuiSytle.alignment = TextAnchor.UpperLeft;
        //            }
        //            break;
        //        case TextAlignment.Right:
        //            {
        //                m_GuiSytle.alignment = TextAnchor.UpperRight;
        //            }
        //            break;
        //    }
        //    GUI.SetNextControlName(this.gameObject.GetInstanceID().ToString());
        //    m_Text = GUI.TextField(new Rect(screenPosition.x - width / 2, Screen.height - (screenPosition.y - height / 2) - height, width, height), m_Text, m_MaxLength, m_GuiSytle);
        //    if (m_GetFocusFlag)
        //    {
        //        GUI.FocusControl(this.gameObject.GetInstanceID().ToString());
        //        m_GetFocusFlag = false;
        //    }
        //    if (m_RemoveFocusFlag)
        //    {
        //        GUI.SetNextControlName(string.Empty);
        //        GUI.TextField(new Rect(-100, -100, 1, 1), string.Empty);
        //        GUI.FocusControl(string.Empty);
        //        m_RemoveFocusFlag = false;
        //    }
        //}
    }
	
#endif
	 
    void OnDestroy()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _DestroyNativeInputBox(m_ControlID);
        }
    }

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern int _CreateNativeInputBox(string tex, float positionX, float positionY, float fontColorR, float fontColorG, float fontColorB, float fontColorA, int fontSize, int maxLength, int alignment, int width);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _DestroyNativeInputBox(int controlID);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _SetInputBoxPosition(int controlID, float x, float y);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _SetInputBoxFontSize(int controlID, int fontSize);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _SetInputBoxWidth(int controlID, int fontSize);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern string _GetInputBoxText(int controlID);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _SetInputBoxText(int controlID, string text);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _SetInputBoxFontColor(int controlID, float ColorR, float ColorG, float ColorB, float ColorA);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _SetInputBoxAlignment(int controlID, int alignment);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _SetInputBoxMaxLength(int controlID, int maxLength);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern bool _GetInputBoxFocus(int controlID);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _SetInputBoxFocus(int controlID, bool focusValue);
	
	[System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void _SetInputBoxKeyboardType(int controlID, int keyboardType);
}
