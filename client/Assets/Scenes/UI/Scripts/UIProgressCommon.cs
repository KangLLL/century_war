using UnityEngine;
using System.Collections;

public class UIProgressCommon : MonoBehaviour {
    [SerializeField] UISlider m_UISlider;
    [SerializeField] UILabel m_UILabel;
    [SerializeField] UILabel[] m_UILabelText;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public virtual void SetProgressBar(float progress, float value)
    {
        m_UISlider.sliderValue = progress;
        m_UILabel.text = (Mathf.RoundToInt(value)).ToString();
    }
    public virtual void SetProgressBar(float progress, string value)
    {
        m_UISlider.sliderValue = progress;
        m_UILabel.text = value;
    }
    public virtual void SetText(params string[] text)
    {
        for (int i = 0; i < m_UILabelText.Length; i++)
        {
            m_UILabelText[i].text = text[i];
        }
    }
}