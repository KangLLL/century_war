using UnityEngine;
using System.Collections;

public class BattleSaveBehavior : MonoBehaviour 
{
	void OnClick()
	{
		BattleWriter.Instance.SaveBattle();
	}
}
