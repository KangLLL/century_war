using UnityEngine;
using System.Collections;

public class ButtonListenerExt : MonoBehaviour 
{
	[SerializeField] Component[] m_Controller;
    [SerializeField] public string[] m_Message;
    object[] parameter;
    public Component[] Controller
    {
        get { return m_Controller; }
        set { m_Controller = value; }
    } 
    public string[] Message
    {
        get { return m_Message; }
        set { m_Message = value; }
    }
	public object[] Parameter
	{
		get {return parameter;}
		set{parameter = value;} 
	}

	void OnClick()
	{
		if (enabled && m_Controller.Length > 0 && m_Message.Length > 0)
		{
            
			if (parameter != null)
			{
				for(int i=0;i<m_Message.Length;i++)
				{
					m_Controller[i].gameObject.SetActive(true);
				  	m_Controller[i].SendMessage(m_Message[i], parameter[i], SendMessageOptions.RequireReceiver);
				}
			}
			else
			{
				for(int i=0;i<m_Message.Length;i++)
				{
					m_Controller[i].gameObject.SetActive(true);
					m_Controller[i].SendMessage(m_Message[i], SendMessageOptions.RequireReceiver);
				}
			}
		}
	}
}
