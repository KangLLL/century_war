using UnityEngine;
using System.Collections;

public class AwardTextBehaviour : MonoBehaviour {
    [SerializeField] tk2dTextMesh m_tk2dTextMesh;
    [SerializeField] Vector3 m_To = new Vector3(0, 500, 0);
    [SerializeField] float m_DestroyTime = 2;
	// Use this for initialization
	void Start () {
        this.MoveTo();
	}
	
 

    public void SetText(string text,Color color)
    {
        m_tk2dTextMesh.text = text;
        m_tk2dTextMesh.color = color;
        m_tk2dTextMesh.Commit();
        TweenColortk2dTextMesh tc = m_tk2dTextMesh.GetComponent<TweenColortk2dTextMesh>();
        tc.m_From = color;
        Color col = color;
        col.a = 0;
        tc.m_To = col;
    }
    void MoveTo()
    {
        Destroy(this.gameObject, m_DestroyTime);
        iTween.MoveTo(this.gameObject, iTween.Hash(iT.MoveTo.position, m_To + this.transform.position, iT.MoveTo.easetype, iTween.EaseType.linear, iT.MoveTo.time, this.m_DestroyTime, iT.MoveTo.islocal, false));
    }
}
