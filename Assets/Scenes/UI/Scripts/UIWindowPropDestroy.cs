using UnityEngine;
using System.Collections;
using System;

public class UIWindowPropDestroy : MonoBehaviour
{

    [SerializeField] UILabel[] m_UILabelText;
    public event Action Click;
    Vector3 m_To = new Vector3(0, 0, -100);
    public void ShowWindow(string title, string context)
    {
        this.gameObject.SetActive(true);
        this.transform.localPosition = m_To;
        this.m_UILabelText[0].text = title;
        this.m_UILabelText[1].text = context; 
    }
    public void HideWindow()
    {
        this.UnRegistDelegate();
        this.gameObject.SetActive(false);
    }
    public void UnRegistDelegate()
    {
        this.Click = null;
    }

    //Button message;
    protected void OnClickDestroy()
    { 
        if (this.Click != null) 
            Click();
       this.HideWindow();
    }
}
