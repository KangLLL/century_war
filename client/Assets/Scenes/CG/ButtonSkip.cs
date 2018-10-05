using UnityEngine;
using System.Collections;

public class ButtonSkip : MonoBehaviour 
{
	[SerializeField]
	private CGDirector m_Director;
	
	void OnClick()
	{
		this.m_Director.CGOver();
	}
}
