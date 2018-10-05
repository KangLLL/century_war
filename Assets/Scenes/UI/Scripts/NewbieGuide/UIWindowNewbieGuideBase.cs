using UnityEngine;
using System.Collections;
using System;

public class UIWindowNewbieGuideBase : MonoBehaviour
{
    //TweenPosition m_TweenPosition;
    TweenAlpha m_TweenAlpha;
    Vector3 m_DefaultFrom = new Vector3(2000, 2000, 0);
    Vector3 m_DefaultTo = new Vector3(0, 0, -600);
    public virtual void ShowWindow(Nullable<Vector3> toPosition = null, bool enableScale = true)
    {
        this.gameObject.SetActive(true);
        if (enableScale)
        {
            //m_TweenPosition.eventReceiver = null;
            //m_TweenPosition.callWhenFinished = null;
            //m_TweenPosition.duration = 0.001f;
            //m_TweenPosition.delay = 0;
            //m_TweenPosition.from = this.m_DefaultFrom;
            //if (toPosition.HasValue)
            //    m_TweenPosition.to = toPosition.Value;
            //else
            //    m_TweenPosition.to = this.m_DefaultTo;
            //m_TweenPosition.Play(true);
            if (toPosition.HasValue)
                this.transform.localPosition = toPosition.Value;
            else
                this.transform.localPosition = this.m_DefaultTo;
            m_TweenAlpha.duration = 0.2f;
            m_TweenAlpha.delay = 0;
            m_TweenAlpha.from = 0;
            m_TweenAlpha.to = 1;
            m_TweenAlpha.Play(true);
            this.transform.localScale = new Vector3(0.3f, 0.3f, 1);
            iTween.ScaleTo(this.gameObject, iTween.Hash(iT.ScaleTo.scale, Vector3.one, iT.ScaleTo.easetype, iTween.EaseType.easeOutBack, iT.ScaleTo.time, 0.2f));
        }
        else
            this.ShowWindowImmediately(toPosition);
        AudioController.Play("WindowShow");
    }
    public virtual void HideWindow(bool enableScale = true)
    {
        if (enableScale)
        {
            //m_TweenPosition.eventReceiver = this.gameObject;
            //m_TweenPosition.callWhenFinished = "OnFinished";
            //m_TweenPosition.delay = 0.2f;
            //m_TweenPosition.Play(false);
            m_TweenAlpha.Play(false);
            this.gameObject.transform.localScale = new Vector3(1f, 1f, 1);
            iTween.ScaleTo(this.gameObject, iTween.Hash(iT.ScaleTo.scale, new Vector3(0.3f, 0.3f, 1), iT.ScaleTo.easetype, iTween.EaseType.easeInBack, iT.ScaleTo.time, 0.2f, iT.MoveTo.oncomplete, "OnFinished"));
        }
        else
            this.HideWindowImmediately();
        AudioController.Play("WindowHide");
    }
    void HideWindowImmediately()
    {
        this.transform.localPosition = this.m_DefaultFrom;
        this.OnFinished();
    }
    void ShowWindowImmediately(Nullable<Vector3> toPosition = null)
    {
        this.gameObject.SetActive(true);
        if (toPosition.HasValue) 
            this.transform.localPosition = toPosition.Value;
        else
            this.transform.localPosition = this.m_DefaultTo;
    }
    protected virtual void GetTweenComponent()
    {
        //m_TweenPosition = GetComponent<TweenPosition>();
        m_TweenAlpha = GetComponent<TweenAlpha>();
    }
    void OnFinished()
    {
        this.transform.localPosition = this.m_DefaultFrom;
        this.gameObject.SetActive(false);
    }

}
