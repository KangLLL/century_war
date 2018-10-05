using UnityEngine;
using System.Collections;
using System;

public class UIWindowPropInfo : UIWindowCommon
{
    [SerializeField] UILabel[] m_UILabel;
    [SerializeField] UILabel m_UILabelBtnText;
    [SerializeField] Vector3 m_WindowOffsetTop = new Vector3(0, 150, -50);
    [SerializeField] Vector3 m_WindowOffsetBottom = new Vector3(0, -150, -50);
    [SerializeField] UISprite[] m_UISprite;//0 window style;1 = use btn bg color; 2 =use btn bg gray
    //[SerializeField] ButtonListener m_ButtonListenerUseBtn;//use prop button
   
    public event Action ClickUse;
    public event Action ClickDestroy; 
    public event Action ListenButton;
    void Update()
    {
        if (this.ListenButton != null)
            this.ListenButton();
    }
    public void ShowWindow(params string[] param)
    {
        this.gameObject.SetActive(true);
        this.SetWindowItem(param);
    }
    public void SetWindowPositon(Vector3 refPosition,Side side)
    {
        switch (side)
        {
            case Side.Top:
                this.transform.position = refPosition + m_WindowOffsetTop;
                break;
            case Side.Bottom:
                this.transform.position = refPosition + m_WindowOffsetBottom;
                break;
            default:
                this.transform.position = refPosition + m_WindowOffsetTop;
                break;
        }
    }
    public void SetUseButton(bool enable,string text,bool ative = true)
    {
        m_UISprite[1].alpha = enable ? 1 : 0;
        m_UISprite[2].alpha = enable ? 0 : 1;
        m_UILabelBtnText.text = text;
        m_UISprite[1].transform.parent.gameObject.SetActive(ative);
        //m_ButtonListenerUseBtn.enabled = enable;
    }
    public void SetWindowStyle(string spriteName)
    {
        m_UISprite[0].spriteName = spriteName;
    }
    public override void HideWindow()
    {
        this.UnRegistDelegate();
        this.gameObject.SetActive(false);
    }
    //Button message
    protected void OnClickUse()
    { 
        if (this.ClickUse != null)
            ClickUse(); 
        this.HideWindow();
    }
    //Button message
    protected void OnClickDestroy()
    { 
        if (this.ClickDestroy != null) 
            ClickDestroy(); 
        this.HideWindow();
    }
    public void UnRegistDelegate()
    {
        this.ClickUse = null;
        this.ClickDestroy = null;
        this.ListenButton = null;
    }
    public void SetWindowItem(params string[] param)
    {
        for (int i = 0; i < param.Length; i++)
        {
            m_UILabel[i].text = param[i];
        }
    }
}
