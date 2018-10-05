using UnityEngine;
using System.Collections;

public class AttackPropsAudioBehavior : MonoBehaviour 
{
	[SerializeField]
	private string m_Sound;

	// Use this for initialization
	void Start () 
	{
		AudioController.Play(this.m_Sound);
	}
}
