using UnityEngine;
using System.Collections;

public class UIPopMenu : MonoBehaviour
{
    [SerializeField]
    private Vector3 m_MoveDistance;
    [SerializeField]
    GameObject m_PopMenu;
    [SerializeField]
    UISprite m_UISprite;
    bool m_IsOpen = false;
    private Vector3 m_StartPosition;
    private Vector3 m_EndPosition;
    // Use this for initialization
    void Start()
    {
        m_StartPosition = m_PopMenu.transform.localPosition;
        m_EndPosition = m_StartPosition + m_MoveDistance;
    }
    void ShowWindow()
    {
        iTween.MoveTo(m_PopMenu, iTween.Hash(iT.MoveTo.position, m_EndPosition, iT.MoveAdd.easetype, iTween.EaseType.easeOutExpo, iT.MoveAdd.time, 0.5f,iT.MoveTo.islocal ,true));
    }
    void HideWindow()
    {
        iTween.MoveTo(m_PopMenu, iTween.Hash(iT.MoveTo.position, m_StartPosition, iT.MoveAdd.easetype, iTween.EaseType.easeOutExpo, iT.MoveAdd.time, 0.5f, iT.MoveTo.islocal, true));
    }
    // Update is called once per frame
    void Update()
    {

    }
    void OnPress(bool isPress)
    {
        if (isPress)
            this.SetSprite("UI_Icon_002");
        else
            this.SetSprite("UI_Icon_001");
    }
    void SetSprite(string name)
    {
        this.m_UISprite.spriteName = name;
        this.m_UISprite.MakePixelPerfect();
    }
    void SetFlip(Vector3 localEulerAngles)
    {
        this.m_UISprite.transform.localEulerAngles = localEulerAngles;
    }
    void OnClick()
    {
        if (!this.m_IsOpen)
            this.ShowWindow();
        else
            this.HideWindow();
        this.m_IsOpen = !this.m_IsOpen;
        if (this.m_IsOpen)
            this.SetFlip(new Vector3(0, 180, 0));
        else
            this.SetFlip(new Vector3(0, 0, 0));
    }
}
