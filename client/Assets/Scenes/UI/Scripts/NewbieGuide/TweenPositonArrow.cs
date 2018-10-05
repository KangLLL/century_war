using UnityEngine;
using System.Collections;

public class TweenPositonArrow : MonoBehaviour {
    [SerializeField] Vector3 m_To;
	void Start () 
    {
        iTween.MoveTo(this.gameObject, iTween.Hash(iT.MoveTo.position, m_To, iT.MoveTo.easetype,iTween.EaseType.linear,iT.MoveTo.looptype, iTween.LoopType.pingPong, iT.MoveTo.time, 0.4f, iT.MoveTo.islocal, true));
	}
	 
}
