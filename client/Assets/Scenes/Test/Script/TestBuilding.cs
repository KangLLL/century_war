using UnityEngine;
using System.Collections;

public class TestBuilding : MonoBehaviour {
[SerializeField] GameObject m_Building;
[SerializeField]
UIButton m_UIButton;
	// Use this for initialization
	void Start () {
        m_UIButton.isEnabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameObject.Instantiate(m_Building);
        }
	
	}
}
