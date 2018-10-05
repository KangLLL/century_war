using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIWindowGuide : UIWindowNewbieGuideBase
{
    [SerializeField] UILabel m_UILabel;
    [SerializeField] BoxCollider m_BoxCollider;
    [SerializeField] Vector3[] m_GuideWindowOffset;
    [SerializeField] UIAnchor m_GuideWindowAnchor;
    //public event Action Click;
    //public Queue<Action> ClickNext = new Queue<Action>();

    //public event Func<bool> UpdateEvent;
    //public Queue<Func<bool>> UpdateEventNext = new Queue<Func<bool>>();
 
    void Awake()
    {
        this.GetTweenComponent();
    }
    protected override void GetTweenComponent()
    {
        base.GetTweenComponent();
    }

    //void Update()
    //{
    //    if (this.UpdateEvent != null)
    //    {
    //        bool state = this.UpdateEvent();
    //        if (state)
    //        {
    //            this.UpdateEvent = null;
    //            if (this.UpdateEventNext.Count > 0)
    //                this.UpdateEvent = this.UpdateEventNext.Dequeue();
    //        }
    //    }
    //}
    public override void ShowWindow(Vector3? toPosition = null, bool enableScale = true)
    {
        base.ShowWindow(toPosition, enableScale);
    }
    public void ShowWindow(UIAnchor.Side dock, bool enableScale = true, Vector3? toPosition = null)
    {
        this.m_GuideWindowAnchor.side = dock;
        if (toPosition.HasValue)
            base.ShowWindow(toPosition, enableScale);
        else
            base.ShowWindow(this.m_GuideWindowOffset[(int)dock], enableScale);
    }
    public override void HideWindow(bool enableScale = true)
    { 
        base.HideWindow(enableScale);
    }
    public void SetWindowItem(string context)
    {
        m_UILabel.text = context;
    }
    //void OnClick()
    //{
    //    if (this.Click != null)
    //        Click();
    //    this.Click = null;
    //    if (this.ClickNext.Count > 0)
    //        this.Click = this.ClickNext.Dequeue();
    //}
    public void SetClickState(bool enableClick)
    {
        m_BoxCollider.enabled = enableClick;
    }
    public void ActiveChild(bool active)
    {
        for (int i = 0; i < this.transform.childCount; i++)
            this.transform.GetChild(i).gameObject.SetActive(active); 
    }
 
}
