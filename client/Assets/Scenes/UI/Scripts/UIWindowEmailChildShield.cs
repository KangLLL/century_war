using UnityEngine;
using System.Collections;
using System;
public class UIWindowEmailChildShield : UIWindowCommon
{
    public Action MissionEvent;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public new void ShowWindow()
    {
        this.gameObject.SetActive(true);
        this.transform.localPosition = new Vector3(0, 0, -600);
    }
    public new void HideWindow()
    {
        this.UnRegistDelegate();
        this.transform.localPosition = new Vector3(2000, 200, -600);
        this.gameObject.SetActive(false);
    }
    protected void OnMission()
    {
        if (this.MissionEvent != null)
        {
            MissionEvent();
        }
        this.HideWindow();
    }
    void UnRegistDelegate()
    {
        this.MissionEvent = null;
    }
}
