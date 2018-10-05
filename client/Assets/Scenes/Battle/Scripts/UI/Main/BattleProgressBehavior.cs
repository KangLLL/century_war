using UnityEngine;
using System.Collections;

public class BattleProgressBehavior : MonoBehaviour 
{
	[SerializeField]
	private UILabel m_ProgressLabel;
	[SerializeField]
	private UISprite[] m_ProgressSprites;
	[SerializeField]
	private GameObject m_StarEffectPrefab;
	
	private int m_ProgressStep;
	
	void Update () 
	{
		int currentPercentage = (int)(BattleRecorder.Instance.DestroyBuildingPercentage * 100);
		this.m_ProgressLabel.text =  currentPercentage + "%"; 
		
		int currentStep = currentPercentage < ClientConfigConstants.Instance.BattleProgressStep[0] ? 0 : 
			currentPercentage < ClientConfigConstants.Instance.BattleProgressStep[1] ? 1 : 2;
		if(BattleRecorder.Instance.IsDestroyCityHall)
		{
			currentStep ++;
		}
		if(this.m_ProgressStep != currentStep)
		{
			this.AdvanceProgress(currentStep);
		}
	}
	
	private void AdvanceProgress(int newStep)
	{
		for(int i = this.m_ProgressStep; i < newStep; i++)
		{
			this.m_ProgressSprites[i].spriteName = ClientStringConstants.FULL_FILL_STAR_SPRITE_NAME;
			this.m_ProgressSprites[i].MakePixelPerfect();
			
			GameObject starEffect = GameObject.Instantiate(this.m_StarEffectPrefab) as GameObject;
			starEffect.transform.position = this.m_ProgressSprites[i].transform.position + starEffect.transform.position;
		}
		
		AudioController.Play("ObtainStar");
		this.m_ProgressStep = newStep;
	}
	
	public void Clear()
	{
		this.m_ProgressStep = 0;
		foreach (UISprite starSprite in m_ProgressSprites) 
		{
			starSprite.spriteName = ClientStringConstants.EMPTY_STAR_SPRITE_NAME;
			starSprite.MakePixelPerfect();
		}
	}
}
