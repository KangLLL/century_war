using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIErrorMessage : MonoBehaviour {
    [SerializeField] UILabel m_UILabel;
    [SerializeField] Vector3 INITIAL_LOCAL_POSITION = new Vector3(0, 0, -3000);
    [SerializeField] Vector3 OFFSET_LOCAL_POSITION = new Vector3(0, 50, 0);

    static UIErrorMessage m_Instance;
    static public UIErrorMessage Instance
    {
        get
        {
            return m_Instance;
        }
    }
    
    
    void Awake()
    {
        m_Instance = this;
    }
	
	void OnDestroy()
	{
		m_Instance = null;
	}

    public void ErrorMessage(int errorCode,params string[] value)
    {
		string text = string.Format(StringConstants.ERROR_MESSAGE[errorCode], value);
		this.ErrorMessage(text);
    }

    public void ErrorMessage(string text, Color? color = null)
	{
		GameObject go = GameObject.Instantiate(m_UILabel.gameObject) as GameObject;
        go.transform.parent = this.transform;
        go.GetComponent<UILabel>().text = text;
        go.GetComponent<TweenAlpha>().Play(true);
        go.name = go.name + Mathf.RoundToInt(Time.time * 100000);
        if (color.HasValue)
            go.GetComponent<UILabel>().color = color.Value;
        Destroy(go, 5);
        List<Transform> list = new List<Transform>(); 
        for (int i = 0; i < this.transform.childCount; i++)
        {
            list.Add(this.transform.GetChild(i));
        }
        list.Sort((a, b) => { return string.Compare(b.name, a.name); });
        for (int i = 0 ; i<list.Count; i++)
        {
            Transform tran = list[i];
            tran.localPosition = INITIAL_LOCAL_POSITION + i * OFFSET_LOCAL_POSITION;
        }
	}
}
