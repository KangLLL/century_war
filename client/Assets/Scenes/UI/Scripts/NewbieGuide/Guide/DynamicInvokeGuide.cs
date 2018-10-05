using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DynamicInvokeGuide : MonoBehaviour {
    public event Action Click;
    public Queue<Action> ClickNext = new Queue<Action>();
	// Use this for initialization
	void Start () {
	
	}
	 
    void OnClick()
    { 
        if (this.Click != null)
            Click();
        this.Click = null;
        if (this.ClickNext.Count > 0)
            this.Click = this.ClickNext.Dequeue();
        if (this.Click == null)
            Destroy(this);
    }
}
