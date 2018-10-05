using UnityEngine;
using System.Collections;

public class BuildBehavior : MonoBehaviour 
{
	[SerializeField]
	private GameObject m_AudioController;
	[SerializeField]
	private GameObject m_ClientConstants;
	
	void OnClick()
	{
		GameObject.Destroy(this.m_ClientConstants);
		GameObject.Destroy(this.m_AudioController);
		Application.LoadLevel("MapEditorBuild");
	}
}
