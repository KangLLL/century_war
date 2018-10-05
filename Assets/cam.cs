using UnityEngine;
using System.Collections;

public class cam : MonoBehaviour {

	public Transform target;
	
	// Use this for initialization
	void Start () {
		this.transform.LookAt(target);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
