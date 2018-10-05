using UnityEngine;
using System.Collections;

public class UIWindowConfirmPrompt : UIWindowCostPrompt
{
    [SerializeField] GameObject[] m_Button;//0 = cancel; 1 = confirm;
    [SerializeField] Vector3 m_LocalPositionCancel = new Vector3(-85, -68, 0);
    [SerializeField] Vector3 m_LocalPositionConfirm = new Vector3(86, -68, 0);
    [SerializeField] Vector3 m_LocalPositionCenter = new Vector3(0, -68, 0);
    // Use this for initialization
    void Awake()
    {
        base.GetTweenComponent();
    }

    // Update is called once per frame

    void Update()
    {
		if(UIManager.Instance.UIWindowFocus == null && this.m_IsShow)
		{
			UIManager.Instance.UIWindowFocus = this.gameObject;
		}
    }

    public void ShowWindow(string title, string context, bool onlyConfirmBtn = false)
    {
        base.m_UILabelText[0].text = title;
        base.m_UILabelText[1].text = context;
        this.SetButtonState(onlyConfirmBtn);
        base.ShowWindow();
    }
    new void OnMission()
    {
        base.OnMission();
    }
    void SetButtonState(bool onlyConfirmBtn)
    {
        if (onlyConfirmBtn)
        {
            m_Button[0].SetActive(false);
            m_Button[1].transform.localPosition = this.m_LocalPositionCenter;
        }
        else
        {
            m_Button[0].SetActive(true);
            m_Button[0].transform.localPosition = this.m_LocalPositionCancel;
            m_Button[1].transform.localPosition = this.m_LocalPositionConfirm;

        }
    }
 
}
