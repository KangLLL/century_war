using UnityEngine;
using System.Collections;
using System;
public class UIWindowMask : UIWindowNewbieGuideBase
{
    [SerializeField] UISprite m_UISprite;// background;
    [SerializeField] GameObject m_Parent;
    void Awake()
    {
        this.GetTweenComponent();
    }
    protected override void GetTweenComponent()
    {
        base.GetTweenComponent();
    }
    public override void ShowWindow(Vector3? toPosition = null, bool enableScale = true)
    {
        base.ShowWindow(toPosition, enableScale);
    }
    public void ShowWindow(bool showMask,Vector3? toPosition = null, bool enableScale = true)
    { 
        m_UISprite.alpha = showMask ? 0.5f : 0;
        this.ShowWindow(toPosition, enableScale);
    }
 
    public override void HideWindow(bool enableScale = true)
    {
        base.HideWindow(enableScale);
    }
 
    //public void ChangeParentNode(GameObject parent)
    //{
    //    this.transform.parent = null;
    //    Destroy(GetComponent<UIPanel>());
    //    this.transform.parent = parent.transform;
    //    this.gameObject.SetActive(false);
    //    this.gameObject.SetActive(true);
    //}
    //public void ResetParentNode()
    //{
    //    this.ChangeParentNode(this.m_Parent);
    //}
 
}
