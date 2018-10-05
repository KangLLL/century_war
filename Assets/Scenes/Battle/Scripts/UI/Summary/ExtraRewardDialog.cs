using UnityEngine;
using System.Collections;

public class ExtraRewardDialog : MonoBehaviour 
{
	[SerializeField]
	private UILabel m_GoldLabel;
	[SerializeField]
	private UILabel m_FoodLabel;
	
	public void ShowReward(int gold, int food)
	{
		this.gameObject.SetActive(true);
		this.m_GoldLabel.text = "+" + gold.ToString();
		this.m_FoodLabel.text = "+" + food.ToString();
	}
}
