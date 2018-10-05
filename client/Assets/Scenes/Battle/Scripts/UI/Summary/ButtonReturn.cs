using UnityEngine;
using System.Collections;

public class ButtonReturn : MonoBehaviour 
{
	[SerializeField]
	private CloudBehaviour m_Cloud;
	
	void Start()
	{
		LockScreen.Instance.EnableInput();
	}
	
	void OnClick()
	{
		AudioController.Play("ButtonClick");
		if(BattleDirector.Instance.IsReceivedReplayID)
		{
			LockScreen.Instance.DisableInput();
			this.m_Cloud.FadeIn();
			this.StartCoroutine("Wait");
		}	
	}
	
	IEnumerator Wait()
	{
		yield return new WaitForSeconds(1.5f);
		Application.LoadLevel(ClientStringConstants.BUILDING_SCENE_LEVEL_NAME);
	}
}
