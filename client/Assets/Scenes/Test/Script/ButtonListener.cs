using UnityEngine;
using System.Collections;

public class ButtonListener : MonoBehaviour {
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

	public void OnClick()
    {
		if (enabled && m_Controller != null && !string.IsNullOrEmpty(m_Message) /*&& !UIManager.Instance.IsLock*/)
		{
            m_Controller.gameObject.SetActive(true);
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
