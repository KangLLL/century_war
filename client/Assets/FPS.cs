using UnityEngine;
using System.Collections;

public class FPS : MonoBehaviour {
	private TextMesh t;
	// Use this for initialization
	void Start () {
	t = this.GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.deltaTime!=0)
		{
	t.text = ((int)(1/Time.deltaTime)).ToString();
		}
	}
}
