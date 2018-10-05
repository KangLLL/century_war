using UnityEngine;
using System.Collections;

public class UIRollWindowInfomation : UIRollWindowCommon {
    [SerializeField] TweenAlpha[] m_TweenAlpha;//0=Front content 1=Back content
    [SerializeField] TweenRotation m_TweenRotationBk;
    bool m_Forward;
	// Use this for initialization
    void Awake()
    { this.GetTweenComponent(); }
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public override void HideWindow()
    {
        base.HideWindow();
        this.ShowTweenAlpha(false);
        m_TweenRotationBk.Play(false);
    }
    public override void ShowWindow()
    { 
        base.ShowWindow();
        this.ShowTweenAlpha(true);
        ShowTweenRotation();
    }
    protected override void GetTweenComponent()
    {
        base.GetTweenComponent();
    }
    void ShowTweenAlpha(bool forward)
    {
        this.m_Forward = forward;
        m_TweenAlpha[0].duration = 0.001f;
        m_TweenAlpha[0].delay = 0.1f;
        m_TweenAlpha[0].from = 1;
        m_TweenAlpha[0].to = 0;
        m_TweenAlpha[0].Play(forward);

        m_TweenAlpha[1].duration = 0.001f;
        m_TweenAlpha[1].delay = 0.1f;
        m_TweenAlpha[1].from = 0;
        m_TweenAlpha[1].to = 1;
        m_TweenAlpha[1].Play(forward);
        m_TweenAlpha[1].eventReceiver = this.gameObject;
        m_TweenAlpha[1].callWhenFinished = "OnFinishedTween";
    }
    void ShowTweenRotation()
    {
        m_TweenRotationBk.duration = 0.001f;
        m_TweenRotationBk.delay = 0.1f;
        m_TweenRotationBk.from = Vector3.zero;
        m_TweenRotationBk.to = new Vector3(0, 0, 180);
        m_TweenRotationBk.Play(true);
    }
    void OnFinishedTween()
    {
        m_TweenAlpha[0].gameObject.SetActive(!this.m_Forward);
    }
}
