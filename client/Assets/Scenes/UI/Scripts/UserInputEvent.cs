using UnityEngine;
using System.Collections;

public class UserInputEvent : MonoBehaviour {
    [SerializeField] Vector3 m_From;
    [SerializeField] Vector3 m_To;
    [SerializeField] float m_MoveTime = 0.2f;
    bool m_IsShow = false;
    void Update()
    {
        if (!m_IsShow)
            return;
#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WP8 || UNITY_BLACKBERRY
		if(TouchScreenKeyboard.hideInput) 
           this.MoveBack(); 
#endif

    }
    //UIInput Event
    void OnSubmit(string text)
    {
        this.MoveBack();
    }
    //UIInput Event
    void OnSubmitAccount(string text)
    {
        //this.MoveBack();
    }
    //UIInput Event
    void OnSubmitPassword(string text)
    {
        this.MoveBack();
    }
    //UIInput Event
    void OnSubmitRegistPasswordConform(string text)
    {
        this.MoveBack();
    }
    //UIInput Event
    void OnSubmitPasswordConform(string text)
    {
        this.MoveBack();
    }
    //Button message
    void OnSelectOnTab()
    {
        print("OnSelectOnTab");
        m_IsShow = true;
        iTween.MoveTo(this.gameObject, iTween.Hash(iT.MoveTo.position, this.m_To, iT.MoveTo.easetype, iTween.EaseType.linear, iT.MoveTo.time, this.m_MoveTime, iT.MoveTo.islocal, true));
    }
    void MoveBack()
    {
        m_IsShow = false;
        print("MoveBack");
        iTween.MoveTo(this.gameObject, iTween.Hash(iT.MoveTo.position, this.m_From, iT.MoveTo.easetype, iTween.EaseType.linear, iT.MoveTo.time, this.m_MoveTime, iT.MoveTo.islocal, true));
    }
}
