using UnityEngine;
using System.Collections;

public class AnimatedSpriteLifeBehavior : MonoBehaviour 
{
	[SerializeField]
	private tk2dSpriteAnimator m_SpriteAnimator;

	void Start () 
	{
		this.m_SpriteAnimator.AnimationCompleted = (sprite, clipId) => {
			GameObject.Destroy(this.gameObject);
		};
	}
}
