using UnityEngine;
using System.Collections;

public class UIUpgradeProgressBar : UIProgressCommon
{
    [SerializeField] UISlider m_UISlider2;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    //public void SetProgressBar(float progress, string value)
    //{
    //    m_UISlider.sliderValue = progress;
    //    m_UILabel.text = value;
    //}
    public void SetUpgradeProgressBar2(float progress)
    {
        m_UISlider2.sliderValue = progress;
    }
    //public void SetText(params string[] text)
    //{
    //    base.SetText(text);
    //}
}
