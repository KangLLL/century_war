using UnityEngine;
using System.Collections;

public class CloudBehaviour : MonoBehaviour
{
    [SerializeField]
    private bool m_VisibleOnStart;

    [SerializeField]
    private UIPanel m_CloudPanel;

    [SerializeField]
    private GameObject m_Cloud1;
    private UISprite m_Cloud1Sprite;
    private Vector3 m_Cloud1StartPosition;
    [SerializeField]
    private Vector3 m_Cloud1EndPosition;

    [SerializeField]
    private GameObject m_Cloud2;
    private Vector3 m_Cloud2StartPosition;
    [SerializeField]
    private Vector3 m_Cloud2EndPosition;

    [SerializeField]
    private GameObject m_Cloud3;
    private UISprite m_Cloud3Sprite;
    private Vector3 m_Cloud3StartPosition;
    [SerializeField]
    private Vector3 m_Cloud3EndPosition;

    [SerializeField]
    private GameObject m_Cloud4;
    private Vector3 m_Cloud4StartPosition;
    [SerializeField]
    private Vector3 m_Cloud4EndPosition;
    public delegate void CompleteCloudFade();
    public  CompleteCloudFade OnCompleteCloudFade;
    private void Awake()
    {
        m_Cloud1Sprite = m_Cloud1.GetComponent<UISprite>();
        m_Cloud3Sprite = m_Cloud3.GetComponent<UISprite>();
        m_Cloud1StartPosition = m_Cloud1.transform.localPosition;
        m_Cloud2StartPosition = m_Cloud2.transform.localPosition;
        m_Cloud3StartPosition = m_Cloud3.transform.localPosition;
        m_Cloud4StartPosition = m_Cloud4.transform.localPosition;
        if (m_VisibleOnStart)
        {
            m_CloudPanel.gameObject.SetActive(true);
            m_Cloud1.transform.localPosition = m_Cloud1EndPosition;
            m_Cloud2.transform.localPosition = m_Cloud2EndPosition;
            m_Cloud3.transform.localPosition = m_Cloud3EndPosition;
            m_Cloud4.transform.localPosition = m_Cloud4EndPosition;
        }
    }


    public void FadeIn()
    {
		iTween.Stop(this.gameObject);
        m_CloudPanel.gameObject.SetActive(true);
        iTween.ValueTo(this.gameObject, iTween.Hash(iT.ValueTo.from, 0f, iT.ValueTo.to, 1f, iT.ValueTo.easetype, iTween.EaseType.linear, iT.ValueTo.time, 1.3f, iT.ValueTo.onupdate, "AlphaValueUpdate"));
        iTween.MoveTo(m_Cloud1, iTween.Hash(iT.MoveTo.islocal, true, iT.MoveTo.position, m_Cloud1EndPosition, iT.MoveTo.easetype, iTween.EaseType.easeOutQuad, iT.MoveTo.time, 1.3f));
        iTween.MoveTo(m_Cloud2, iTween.Hash(iT.MoveTo.islocal, true, iT.MoveTo.position, m_Cloud2EndPosition, iT.MoveTo.easetype, iTween.EaseType.easeOutQuad, iT.MoveTo.time, 1.0f));
        iTween.MoveTo(m_Cloud3, iTween.Hash(iT.MoveTo.islocal, true, iT.MoveTo.position, m_Cloud3EndPosition, iT.MoveTo.easetype, iTween.EaseType.easeOutQuad, iT.MoveTo.time, 1.3f));
        iTween.MoveTo(m_Cloud4, iTween.Hash(iT.MoveTo.islocal, true, iT.MoveTo.position, m_Cloud4EndPosition, iT.MoveTo.easetype, iTween.EaseType.easeOutQuad, iT.MoveTo.time, 1.0f));
    }

    public void FadeOut()
    {
		iTween.Stop(this.gameObject);
        iTween.ValueTo(this.gameObject, iTween.Hash(iT.ValueTo.from, 1f, iT.ValueTo.to, 0f, iT.ValueTo.easetype, iTween.EaseType.linear, iT.ValueTo.time, 1.3f, iT.ValueTo.onupdate, "AlphaValueUpdate", iT.ValueTo.oncomplete, "OnComplete"));
        iTween.MoveTo(m_Cloud1, iTween.Hash(iT.MoveTo.islocal, true, iT.MoveTo.position, m_Cloud1StartPosition, iT.MoveTo.easetype, iTween.EaseType.easeInQuad, iT.MoveTo.time, 1.0f));
        iTween.MoveTo(m_Cloud2, iTween.Hash(iT.MoveTo.islocal, true, iT.MoveTo.position, m_Cloud2StartPosition, iT.MoveTo.easetype, iTween.EaseType.easeInQuad, iT.MoveTo.time, 1.3f));
        iTween.MoveTo(m_Cloud3, iTween.Hash(iT.MoveTo.islocal, true, iT.MoveTo.position, m_Cloud3StartPosition, iT.MoveTo.easetype, iTween.EaseType.easeInQuad, iT.MoveTo.time, 1.0f));
        iTween.MoveTo(m_Cloud4, iTween.Hash(iT.MoveTo.islocal, true, iT.MoveTo.position, m_Cloud4StartPosition, iT.MoveTo.easetype, iTween.EaseType.easeInQuad, iT.MoveTo.time, 1.3f));
    }

    private void AlphaValueUpdate(float value)
    {
        m_Cloud1Sprite.alpha = value;
        m_Cloud3Sprite.alpha = value;
    }

    private void OnComplete()
    {
        m_CloudPanel.gameObject.SetActive(false);
        if (OnCompleteCloudFade != null)
        {
            OnCompleteCloudFade();
            OnCompleteCloudFade = null;
        }
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.J))
    //    {
    //        this.FadeIn();
    //    }
    //    if (Input.GetKeyDown(KeyCode.K))
    //    {
    //        this.FadeOut();
    //    }
    //}
}
