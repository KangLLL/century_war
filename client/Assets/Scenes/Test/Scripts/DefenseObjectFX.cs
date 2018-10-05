using UnityEngine;
using System.Collections;

public class DefenseObjectFX : MonoBehaviour {
    [SerializeField] GameObject m_Background;
	// Use this for initialization
	void Start () {
        this.SetFX();
	}
    void SetFX()
    {
        iTween.ScaleTo(m_Background.gameObject, iTween.Hash(iT.ScaleTo.scale, new Vector3(1.5f, 1.5f, 1), iT.MoveTo.easetype, iTween.EaseType.easeOutQuad, iT.ScaleTo.looptype, iTween.LoopType.pingPong, iT.ScaleTo.time, 0.2f, iT.MoveTo.islocal, true));
        iTween.MoveTo(m_Background.gameObject, iTween.Hash(iT.MoveTo.position, new Vector3(0, 15, 0), iT.MoveTo.easetype, iTween.EaseType.easeOutQuad, iT.MoveTo.looptype, iTween.LoopType.pingPong, iT.MoveTo.time, 0.2f, iT.MoveTo.islocal, true));
        iTween.RotateTo(this.gameObject, iTween.Hash(iT.RotateTo.rotation, Vector3.zero, iT.RotateTo.oncomplete, "OnCompleteMoveTo", iT.RotateTo.delay, 0.4f, iT.RotateTo.time, 0.0f));
    }
    void OnCompleteMoveTo()
    {
        iTween.Stop(m_Background.gameObject);
        m_Background.gameObject.transform.localScale = Vector3.one;
        m_Background.gameObject.transform.localPosition = Vector3.zero;
    }

}
