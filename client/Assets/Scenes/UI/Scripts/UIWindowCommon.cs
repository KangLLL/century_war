using UnityEngine;
using System;
using System.Collections;

public class UIWindowCommon : MonoBehaviour {

    public BuildingLogicData BuildingLogicData { get; set; }
    public AchievementBuildingLogicData AchievementBuildingLogicData { get; set; }
    protected TweenPosition m_TweenPosition;
    //TweenScale m_TweenScale;
    TweenAlpha m_TweenAlpha;
    public GameObject ControlerFocus { get; set; }
    public event Action WindowEvent;
    public event Action WindowCloseEvent;
    private Vector3 m_From = new Vector3(2000, 2000, 0);
    private Vector3 m_To = new Vector3(0, 0, -500);

	protected bool m_IsShow;

    public virtual void ShowWindow()
    {
		if(UIManager.Instance.UIWindowFocus == null)
		{
			this.m_IsShow = true;

            this.SetCloseRange();
			UIManager.Instance.UIWindowFocus = this.gameObject;
            UIManager.Instance.SceneFocus = false;
            this.ControlerFocus = null;
	        this.gameObject.SetActive(true);
	        m_TweenPosition.eventReceiver = null;
	        m_TweenPosition.callWhenFinished = null;
	        m_TweenPosition.duration = 0.001f;
	        m_TweenPosition.delay = 0;
            m_TweenPosition.from = this.m_From;
            m_TweenPosition.to = this.m_To;
	        m_TweenPosition.Play(true);
	        m_TweenAlpha.duration = 0.2f;
	        m_TweenAlpha.delay = 0;
	        m_TweenAlpha.from = 0;
	        m_TweenAlpha.to = 1;
	        m_TweenAlpha.Play(true);
            //iTween.Stop(this.gameObject);
	        this.gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 1);
	        iTween.ScaleTo(this.gameObject, iTween.Hash(iT.ScaleTo.scale, Vector3.one, iT.ScaleTo.easetype, iTween.EaseType.easeOutBack, iT.ScaleTo.time, 0.2f,iT.ScaleTo.oncomplete ,"OnCompleteScale"));
	        UIManager.Instance.HidePopuBtnByCurrentSelect(true);
            AudioController.Play("WindowShow");
		}
    }
    public void ShowWindow(Vector3 to)
    {
        if (UIManager.Instance.UIWindowFocus == null)
        {
			this.m_IsShow = true;

            this.SetCloseRange();
            UIManager.Instance.UIWindowFocus = this.gameObject;
            UIManager.Instance.SceneFocus = false;
            this.ControlerFocus = null;
            this.gameObject.SetActive(true);
            m_TweenPosition.eventReceiver = null;
            m_TweenPosition.callWhenFinished = null;
            m_TweenPosition.duration = 0.001f;
            m_TweenPosition.delay = 0;
            m_TweenPosition.from = this.m_From;
            m_TweenPosition.to = to;
            m_TweenPosition.Play(true);
            m_TweenAlpha.duration = 0.2f;
            m_TweenAlpha.delay = 0;
            m_TweenAlpha.from = 0;
            m_TweenAlpha.to = 1;
            m_TweenAlpha.Play(true);
            //iTween.Stop(this.gameObject);
            this.gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 1);
            iTween.ScaleTo(this.gameObject, iTween.Hash(iT.ScaleTo.scale, Vector3.one, iT.ScaleTo.easetype, iTween.EaseType.easeOutBack, iT.ScaleTo.time, 0.2f, iT.ScaleTo.oncomplete, "OnCompleteScale"));
            UIManager.Instance.HidePopuBtnByCurrentSelect(true);
            AudioController.Play("WindowShow");
        }
    }
    public void ShowWindowImmediately(bool closeSceneCamera = true)
    {
        if (UIManager.Instance.UIWindowFocus == null)
        {
			this.m_IsShow = true;

            UIManager.Instance.UIWindowFocus = this.gameObject;
            UIManager.Instance.SceneFocus = false;
            this.ControlerFocus = null;
            CameraManager.Instance.MainCamera.enabled = !closeSceneCamera;
            this.gameObject.SetActive(true);
            this.gameObject.transform.localPosition = this.m_To;
            AudioController.Play("WindowShow");
        }
    }
    public void ShowWindowImmediatelySimplify()
    {
		this.m_IsShow = true;
        this.SetCloseRange();
        this.gameObject.SetActive(true);
        this.gameObject.transform.localPosition = this.m_To;
        AudioController.Play("WindowShow");
    }
    public void HideWindowImmediatelySimplify()
    {
		this.m_IsShow = false;
        this.transform.position = this.m_From;
        //this.gameObject.SetActive(false);
        this.OnFinished();
        AudioController.Play("WindowHide");
    }
    public virtual void HideWindow()
    {
        if (m_TweenPosition == null || m_TweenAlpha == null || UIManager.Instance.UIWindowFocus != this.gameObject)
            return;
		this.m_IsShow = false;
		UIManager.Instance.UIWindowFocus = null;
        UIManager.Instance.SceneFocus = true;
        this.ControlerFocus = this.gameObject;
        m_TweenPosition.eventReceiver = this.gameObject;
        m_TweenPosition.callWhenFinished = "OnFinished";
        m_TweenPosition.delay = 0.2f;
        m_TweenPosition.Play(false);
        m_TweenAlpha.Play(false);
        this.gameObject.transform.localScale = new Vector3(1f, 1f, 1);
        //iTween.Stop(this.gameObject);
        iTween.ScaleTo(this.gameObject, iTween.Hash(iT.ScaleTo.scale, new Vector3(0.3f, 0.3f, 1), iT.ScaleTo.easetype, iTween.EaseType.easeInBack, iT.ScaleTo.time, 0.2f, iT.ScaleTo.oncomplete, "OnCompleteScale"));
        UIManager.Instance.ShowPopupBtnByCurrentSelect();
        AudioController.Play("WindowHide");
    }
    public void HideWindowImmediately(bool openSceneCamera = true,bool active = false)
    {
		this.m_IsShow = false;
        UIManager.Instance.UIWindowFocus = null;
        UIManager.Instance.SceneFocus = true;
        this.ControlerFocus = this.gameObject;
        CameraManager.Instance.MainCamera.enabled = openSceneCamera;
        this.transform.position = this.m_From;
        this.gameObject.SetActive(active);   
        UIManager.Instance.ShowPopupBtnByCurrentSelect();
        AudioController.Play("WindowHide");
    }
    protected virtual void GetTweenComponent()
    {
        m_TweenPosition = GetComponent<TweenPosition>();
        m_TweenAlpha = GetComponent<TweenAlpha>();
    }
    void OnFinished()
    {
        if (this.WindowCloseEvent != null)
        {
            this.WindowCloseEvent();
            this.WindowCloseEvent = null;
        }
        this.gameObject.SetActive(false);
    }
    void OnCompleteScale()
    {
        if (this.WindowEvent != null)
        {
            this.WindowEvent();
            this.WindowEvent = null;
        }
    }
    public void UnRegistWindowEvent()
    {
        this.WindowCloseEvent = null;
        this.WindowEvent = null;
    }
    void SetCloseRange()
    {
        ButtonListener buttonListener = GetComponent<ButtonListener>();
        if (buttonListener != null)
            buttonListener.enabled = !LogicController.Instance.PlayerData.IsNewbie;
    }
}
