using UnityEngine;
using System.Collections;

public class ButtonBack : MonoBehaviour 
{
	void OnClick()
	{
		AudioController.Play("ButtonClick");
		ReplayDirector.Instance.EndReplay();
	}
}
