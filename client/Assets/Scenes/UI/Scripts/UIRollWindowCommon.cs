using UnityEngine;
using System.Collections;

public class UIRollWindowCommon : MonoBehaviour {
    TweenRotation m_TweenRotation;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    protected virtual void GetTweenComponent()
    {
        m_TweenRotation = GetComponent<TweenRotation>(); 
    }
    public virtual void ShowWindow()
    {
        m_TweenRotation.duration = 0.2f;
        m_TweenRotation.delay = 0;
        m_TweenRotation.from = new Vector3(0, 0, 0);
        m_TweenRotation.to = new Vector3(180, 0, 0);
        m_TweenRotation.Play(true);
  
    }
    public virtual void HideWindow()
    { 
        m_TweenRotation.Play(false);
    }
}
