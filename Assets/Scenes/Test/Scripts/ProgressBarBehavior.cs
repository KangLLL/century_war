using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class ProgressBarBehavior : MonoBehaviour {
    //[SerializeField]GameObject m_PBBackground;
    [SerializeField] tk2dSlicedSprite m_Tk2dSlicedSpriteBackground;
    [SerializeField] UI2dTkSlider m_UI2dTkSlider;
    [SerializeField] tk2dTextMesh m_Tk2dTextMesh;
    [SerializeField] tk2dSprite m_Tk2dSpriteIcon;
    public Vector2 ProgressBarOffset{get;set;}
    public Vector2 ProgressBarSize { get; set; }
    int m_RemainingTime = -1;
    const float INTERVAL = 50;
    [SerializeField] Vector3 ICON_OFFSET = new Vector3(-10, 5, -10);
    [SerializeField] Vector3 TEXT_OFFEST = new Vector3(50, 0, -12);
    //[SerializeField]UISlider m_UISlider;
    //[SerializeField]UILabel m_UILabel;
	// Use this for initialization
	void Start () {
        //InitialProgress();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void SetProgressPosition(int order)
    {
        //this.m_UI2dTkSlider.FullSize = this.ProgressBarSize;
        this.m_Tk2dSlicedSpriteBackground.dimensions = this.ProgressBarSize; //backgroundDimensions;
        Vector3 localPosition = this.transform.localPosition;
        localPosition.x = this.ProgressBarOffset.x;
        localPosition.y = this.ProgressBarOffset.y + order * ClientConfigConstants.Instance.ProgressBarInterval;
        localPosition.z = -100;
        this.transform.localPosition = localPosition;
        //localPosition.x = this.ProgressBarSize.x * 0.5f;
        //localPosition.y = 0;
        //localPosition.z = -12;
        m_Tk2dTextMesh.transform.localPosition = TEXT_OFFEST;
        m_Tk2dSpriteIcon.transform.localPosition = ICON_OFFSET;
    }
    public void SetProgressBar(float progress, float remainingTime,bool showIcon,string spriteName)
    {
        this.m_UI2dTkSlider.SliderValue = progress;
        if (this.m_RemainingTime != Mathf.CeilToInt(remainingTime))
        {
            this.m_Tk2dTextMesh.text = SystemFunction.TimeSpanToString(Mathf.CeilToInt(remainingTime));
            this.m_Tk2dTextMesh.Commit();
            this.m_RemainingTime = Mathf.CeilToInt(remainingTime);
            this.SetIcon(showIcon, spriteName);
        }
    }
    public void SetIcon(bool showIcon, string spriteName)
    {
        m_Tk2dSpriteIcon.color = showIcon ? Color.white : Color.clear;
        if (showIcon)
            m_Tk2dSpriteIcon.spriteId = m_Tk2dSpriteIcon.GetSpriteIdByName(spriteName);
    }
}
