using UnityEngine;
using System.Collections;

public class ExplosionBehavior : MonoBehaviour 
{
	[SerializeField]
	private tk2dSpriteAnimator m_SpriteAnimator;
	
	void Start () 
	{
		this.m_SpriteAnimator.AnimationCompleted += OnExplosionFinished;
	}
	
	private void OnExplosionFinished(tk2dSpriteAnimator sprite, tk2dSpriteAnimationClip clipId)
	{
		if(this.m_SpriteAnimator == sprite)
		{
			GameObject.Destroy(this.gameObject);
		}
	}
}
