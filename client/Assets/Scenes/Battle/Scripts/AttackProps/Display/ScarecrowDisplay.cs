using UnityEngine;
using System.Collections;

public class ScarecrowDisplay : MonoBehaviour 
{
	[SerializeField]
	private tk2dSpriteAnimator m_AppearSpriteAnimator;
	[SerializeField]
	private GameObject m_ScarecrowObject;
	[SerializeField]
	private GameObject m_GravityObject;

	void Start () 
	{
		this.m_AppearSpriteAnimator.AnimationCompleted = (sprite, clipId) => {
			this.m_AppearSpriteAnimator.transform.parent.gameObject.SetActive(false);
			this.m_ScarecrowObject.SetActive(true);
			this.m_GravityObject.SetActive(true);
		};
	}
}
