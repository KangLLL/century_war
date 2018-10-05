using UnityEngine;
using System.Collections;
using ConfigUtilities;

public class UserInformationBehavior : MonoBehaviour 
{
	[SerializeField]
	private UILabel m_MaxGoldLabel;
	[SerializeField]
	private UILabel m_GoldLabel;
	[SerializeField]
	private UILabel m_MaxFoodLabel;
	[SerializeField]
	private UILabel m_FoodLabel;
	[SerializeField]
	private UISlider m_GoldSlider;
	[SerializeField]
	private UISlider m_FoodSlider;
	[SerializeField]
	private UILabel m_HonourLabel;
	
	private int m_PreviousGold;
	private int m_PreviousFood;
	
	void Start()
	{
		PlayerLogicData playerData = LogicController.Instance.PlayerData;
		this.m_MaxGoldLabel.text = playerData.GoldMaxCapacity.ToString();
		this.m_MaxFoodLabel.text = playerData.FoodMaxCapacity.ToString();
		this.m_HonourLabel.text = "-" + Mathf.FloorToInt(playerData.Honour * 
			ConfigInterface.Instance.SystemConfig.MatchObtainHonourPercentage).ToString();
		this.m_GoldSlider.sliderValue = (float)playerData.CurrentStoreGold / playerData.GoldMaxCapacity;
		this.m_FoodSlider.sliderValue = (float)playerData.CurrentStoreFood / playerData.FoodMaxCapacity;
		this.m_GoldLabel.text = playerData.CurrentStoreGold.ToString();
		this.m_FoodLabel.text = playerData.CurrentStoreFood.ToString();
		
		this.m_PreviousGold = playerData.CurrentStoreGold;
		this.m_PreviousFood = playerData.CurrentStoreFood;
	}
	
	// Update is called once per frame
	void Update () 
	{
		this.RefreshInformation();
	}
	
	private void RefreshInformation()
	{
		PlayerLogicData playerData = LogicController.Instance.PlayerData;
		if(this.m_PreviousGold != playerData.CurrentStoreGold)
		{
			this.m_GoldLabel.GetComponent<ValueLabelBehavior>().RefreshToValue(playerData.CurrentStoreGold);
			this.m_GoldSlider.sliderValue = (float)playerData.CurrentStoreGold / playerData.GoldMaxCapacity;
			this.m_PreviousGold = playerData.CurrentStoreGold;
		}
		if(this.m_PreviousFood != playerData.CurrentStoreFood)
		{
			this.m_FoodLabel.GetComponent<ValueLabelBehavior>().RefreshToValue(playerData.CurrentStoreFood);
			this.m_FoodSlider.sliderValue = (float)playerData.CurrentStoreFood / playerData.FoodMaxCapacity;
			this.m_PreviousFood = playerData.CurrentStoreFood;
		}
	}
}
