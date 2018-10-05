using UnityEngine;
using System.Collections;

public class HPBarBehavior : MonoBehaviour 
{
	private int m_CurrentDisplayCount;
	private bool m_BlinkFlag;
	[SerializeField]
	private HPBehavior m_HPBehavior;
	[SerializeField]
	private tk2dSlicedSprite m_BarSprite;
	[SerializeField]
	private tk2dSlicedSprite m_BarBackgroundSprite;
	[SerializeField]
	private tk2dSlicedSprite m_HighLightSprite;
	[SerializeField]
	private UI2dTkSlider m_Bar;
	
	void Start()
	{
		this.m_BarSprite.color = ClientSystemConstants.HP_BAR_FULL_COLOR;
		this.m_BarSprite.renderer.enabled = false;
		this.m_BarBackgroundSprite.renderer.enabled = false;
		this.m_HighLightSprite.renderer.enabled = false;
		this.m_BlinkFlag = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(this.m_CurrentDisplayCount > 0)
		{
			if(!this.m_BarBackgroundSprite.renderer.enabled)
			{
				this.m_BarSprite.renderer.enabled = true;
				this.m_BarBackgroundSprite.renderer.enabled = true;
				this.m_HighLightSprite.renderer.enabled = true;
			}
			
			this.m_CurrentDisplayCount --;
			float alphPercentage = 1.0f;
			float remainHPPercentage = (float)this.m_HPBehavior.CurrentHP / this.m_HPBehavior.TotalHP;
			if(this.m_CurrentDisplayCount < ClientConfigConstants.Instance.HPBarFadeOutCount)
			{
				alphPercentage = (float)this.m_CurrentDisplayCount / ClientConfigConstants.Instance.HPBarFadeOutCount;
			}
			else if(remainHPPercentage <= ClientConfigConstants.Instance.HPBarBlinkPercentage)
			{
				
				if(this.m_CurrentDisplayCount %  ClientConfigConstants.Instance.HPBarBlinkFrequencyCount == 0)
				{
					this.m_BlinkFlag = !this.m_BlinkFlag;
				}
				/*
				alphPercentage = this.m_BlinkFlag ? 
					(this.m_CurrentDisplayCount % ClientConfigConstants.Instance.HPBarBlinkFrequencyCount) / (float)ClientConfigConstants.Instance.HPBarBlinkFrequencyCount
						: (ClientConfigConstants.Instance.HPBarBlinkFrequencyCount - (this.m_CurrentDisplayCount  % ClientConfigConstants.Instance.HPBarBlinkFrequencyCount)) / (float)ClientConfigConstants.Instance.HPBarBlinkFrequencyCount;
				Debug.Log(alphPercentage);
				*/
			}
			
			this.m_Bar.SliderValue = remainHPPercentage;
			Color destination = Color.white;
			Color source = Color.white;
			float colorPercentage = 1.0f;
			if(remainHPPercentage >= ClientSystemConstants.HP_MIDDLE_PERCENTAGE)
			{
				destination = ClientSystemConstants.HP_BAR_MIDDLE_COLOR;
				source = ClientSystemConstants.HP_BAR_FULL_COLOR;
				colorPercentage = (1 - remainHPPercentage) / (1 - ClientSystemConstants.HP_MIDDLE_PERCENTAGE);
			}
			else
			{
				destination = ClientSystemConstants.HP_BAR_EMPTY_COLOR;
				source = ClientSystemConstants.HP_BAR_MIDDLE_COLOR;
				colorPercentage = (ClientSystemConstants.HP_MIDDLE_PERCENTAGE - remainHPPercentage) / ClientSystemConstants.HP_MIDDLE_PERCENTAGE;
			}
			
			Color displayColor = Color.Lerp(source, destination, colorPercentage);
			this.m_BarSprite.color = new Color(displayColor.r, displayColor.g, displayColor.b, alphPercentage);
			if(this.m_CurrentDisplayCount < ClientConfigConstants.Instance.HPBarFadeOutCount)
			{
				this.m_BarBackgroundSprite.color = new Color(this.m_BarBackgroundSprite.color.r, this.m_BarBackgroundSprite.color.g,
					this.m_BarBackgroundSprite.color.b, alphPercentage);
				this.m_HighLightSprite.color = new Color(this.m_HighLightSprite.color.r, this.m_HighLightSprite.color.g,
					this.m_HighLightSprite.color.b, alphPercentage);
			}
			else if(!Mathf.Approximately(this.m_BarBackgroundSprite.color.a, 1.0f))
			{
				this.m_BarBackgroundSprite.color = new Color(this.m_BarBackgroundSprite.color.r, this.m_BarBackgroundSprite.color.g,
				                                             this.m_BarBackgroundSprite.color.b, 1.0f);
				this.m_HighLightSprite.color = new Color(this.m_HighLightSprite.color.r, this.m_HighLightSprite.color.g,
				                                         this.m_HighLightSprite.color.b, 1.0f);
			}

			
			this.m_BarSprite.renderer.enabled = this.m_BlinkFlag;
		}
		else
		{
			if(this.m_BarBackgroundSprite.enabled)
			{
				this.m_BarSprite.enabled = false;
				this.m_BarBackgroundSprite.enabled = false;
				this.m_HighLightSprite.enabled = false;
			}
		}
	}
	
	public void DisplayHPBar()
	{
		
		this.m_CurrentDisplayCount = ClientConfigConstants.Instance.HPBarDisplayCount + ClientConfigConstants.Instance.HPBarFadeOutCount;	
	}
}
