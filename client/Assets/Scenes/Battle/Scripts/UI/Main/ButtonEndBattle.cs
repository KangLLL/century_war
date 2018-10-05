using UnityEngine;
using System.Collections;

public class ButtonEndBattle : MonoBehaviour 
{
	void OnClick()
	{
		AudioController.Play("ButtonClick");
		BattleDirector.Instance.EndMatch();
	}
}
