using UnityEngine;
using System.Collections;

public class MeteorDisplay : MonoBehaviour 
{
	[SerializeField]
	private tk2dSpriteAnimator m_FallAnimator;
	[SerializeField]
	private tk2dSpriteAnimator m_ExplosionAnimator;

	// Update is called once per frame
	void Start () 
	{
		this.m_FallAnimator.AnimationCompleted = (sprite, clipId) => {
			this.m_ExplosionAnimator.transform.parent.gameObject.SetActive(true);
			this.m_FallAnimator.transform.parent.gameObject.SetActive(false);
		};
	
		this.m_ExplosionAnimator.AnimationCompleted = (sprite, clipId) => {
			GameObject.Destroy(this.gameObject);
		};
	}
}
