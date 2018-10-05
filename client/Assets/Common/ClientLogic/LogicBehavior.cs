using UnityEngine;
using System.Collections;

public class LogicBehavior: MonoBehaviour 
{
	void Awake()
	{
		GameObject.DontDestroyOnLoad(gameObject);
	}
	
	void Update()
	{
		LogicController.Instance.ProcessLogic();
	}
}
