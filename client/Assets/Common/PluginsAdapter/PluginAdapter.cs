using UnityEngine;
using System.Collections;

public class PluginAdapter : MonoBehaviour 
{
	void Start () 
	{
		GameObject.DontDestroyOnLoad(this.gameObject);
	}
}
