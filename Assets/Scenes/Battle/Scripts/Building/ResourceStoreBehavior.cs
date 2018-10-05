using UnityEngine;
using System.Collections;

public class ResourceStoreBehavior : MonoBehaviour 
{
	[SerializeField]
	private tk2dSpriteAnimator m_SpriteAnimator;
	
	protected BuildingPropertyBehavior m_Property;
	private float m_PreviousPercentage;
	
	// Use this for initialization
	void Start () 
	{
		this.m_Property = this.GetComponent<BuildingPropertyBehavior>();
		this.PlayPercentageAnimation(this.OriginalPercentage);
		this.m_PreviousPercentage = this.CurrentValue / (float)this.TotalValue;
	}
	
	// Update is called once per frame
	void Update () 
	{
		float percentage = this.CurrentValue / (float)this.TotalValue;
		if(percentage != this.m_PreviousPercentage)
		{
			this.m_PreviousPercentage = percentage;
	        if(percentage < this.OriginalPercentage)
			{
				this.PlayPercentageAnimation(percentage);
			}
		}
	}
		
	private void PlayPercentageAnimation(float percentage)
	{
		if(percentage <= ClientConfigConstants.Instance.Store20Criterion)
		{
			this.m_SpriteAnimator.Play(AnimationNameConstants.STORE_PERCENTAGE_20);
		}
		else if(percentage <= ClientConfigConstants.Instance.Store40Criterion)
		{
			this.m_SpriteAnimator.Play(AnimationNameConstants.STORE_PERCENTAGE_40);
		}
		else if(percentage <= ClientConfigConstants.Instance.Store60Criterion)
		{
			this.m_SpriteAnimator.Play(AnimationNameConstants.STORE_PERCENTAGE_60);
		}
		else if(percentage <= ClientConfigConstants.Instance.Store80Criterion)
		{
			this.m_SpriteAnimator.Play(AnimationNameConstants.STORE_PERCENTAGE_80);
		}
		else
		{
			this.m_SpriteAnimator.Play(AnimationNameConstants.STORE_PERCENTAGE_100);
		}
	}
	
	protected virtual float OriginalPercentage
	{
		get { return 1; }
	}
	
	protected virtual int CurrentValue
	{
		get { return 0; }
	}
	
	protected virtual int TotalValue
	{
		get { return 1;}
	}
}
