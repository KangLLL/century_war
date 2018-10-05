using UnityEngine;
using System.Collections;

public class ButtonListenerContinuous : MonoBehaviour {
    [SerializeField] int m_Acceleration;
    [SerializeField] int m_MinInterval;
    [SerializeField] int m_MaxInterval;
 
    int m_CurrentInterval;
    int m_DecelerateInterval;
 
    bool m_EnableContinuous = false;
    [SerializeField]
    private Component m_Controller; 
    public Component Controller
    {
        get { return m_Controller; }
        set { m_Controller = value; }
    }
    [SerializeField]
    public string m_Message;

    public string Message
    {
        get { return m_Message; }
        set { m_Message = value; }
    }

    private object parameter;
    public object Parameter
    {
        get
        {
            return parameter;
        }
        set
        {
            parameter = value;
        }
    }
    void FixedUpdate()
    {
        if (m_EnableContinuous)
        {
            m_CurrentInterval--;
            if (m_CurrentInterval <= 0)
            {
                m_DecelerateInterval -= m_Acceleration;
                this.SendMessage();
                m_CurrentInterval = Mathf.Clamp(m_DecelerateInterval, m_MinInterval, m_MaxInterval);
            }
        }
    }
    void OnPress(bool isPress)
    { 
        m_CurrentInterval = m_MaxInterval;
        m_DecelerateInterval = m_MaxInterval;
        m_EnableContinuous = true;
        if (!isPress)
        {
            m_EnableContinuous = false;
        }
    }
    void OnDisable()
    {
        m_EnableContinuous = false;
    }
	void OnHover (bool isHOver)
	{
		if(!isHOver)
		{
			this.m_EnableContinuous = isHOver;
		}
	}
	void OnClick()
	{
		this.SendMessage();
	}
    void SendMessage()
    {
        if (enabled && m_Controller != null && !string.IsNullOrEmpty(m_Message))
        {
            if (parameter != null)
            {
                m_Controller.SendMessage(m_Message, parameter, SendMessageOptions.RequireReceiver);
            }
            else
            {
                m_Controller.SendMessage(m_Message, SendMessageOptions.RequireReceiver);
            }
        }
    }
}
