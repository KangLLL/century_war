using UnityEngine;
using System.Collections;
using ConfigUtilities;
using CommonUtilities;

public class ButtonNextRival : MonoBehaviour 
{
	[SerializeField]
	private MatchFinder m_Finder;
	[SerializeField]
	private UILabel m_CostLabel;
	[SerializeField]
	private CloudBehaviour m_Cloud;
	
	[SerializeField]
	private UIWindowCostPrompt m_CostPrompt;
	
	private int m_CostValue;
	
	private bool m_IsProcessClick;
	
	void Start()
	{
		this.m_CostValue = ConfigInterface.Instance.SystemConfig.FindMatchCost + LogicController.Instance.PlayerData.CityHallLevel * 
			ConfigInterface.Instance.SystemConfig.FindMatchPlusPerCityHallLevel;
		this.m_CostLabel.text = this.m_CostValue.ToString();
	}
	
	void OnClick()
	{
		if(!this.m_IsProcessClick)
		{
			AudioController.Play("ButtonClick");
			
			if(LogicController.Instance.PlayerData.CurrentStoreGold >= this.m_CostValue)
			{
				this.StartCoroutine("Wait");
				this.m_Cloud.FadeIn();
				LockScreen.Instance.DisableInput();
				BattleDirector.Instance.EndObserve();
				this.m_IsProcessClick = true;
			}
			else
			{
				int needGold = this.m_CostValue - LogicController.Instance.PlayerData.CurrentStoreGold;
				int costGem = MarketCalculator.GetGoldCost(needGold);
				if(LogicController.Instance.PlayerData.CurrentStoreGem < costGem)
				{
					UIErrorMessage.Instance.ErrorMessage(ClientStringConstants.NO_ENOUGH_GOLD_WARNING_MESSAGE);
				}
				else
				{
					string costMessage = string.Format(StringConstants.PROMPT_RESOURCE_COST, needGold, StringConstants.RESOURCE_GOLD);
					this.m_CostPrompt.ShowWindow(costGem, costMessage);
					this.m_CostPrompt.Click += BuyResource;
					this.m_CostPrompt.WindowCloseEvent += CancelBuyResource;
					this.m_IsProcessClick = true;
				}
			}
		}
	}
	
	public void BuyResource()
	{
		int needGold = this.m_CostValue - LogicController.Instance.PlayerData.CurrentStoreGold;
		
		LogicController.Instance.BuyGold(needGold);
		this.m_CostPrompt.Click -= BuyResource;
		this.m_IsProcessClick = false;
		this.OnClick();
	}
	
	private void CancelBuyResource()
	{
		this.m_CostPrompt.WindowCloseEvent -= CancelBuyResource;
		this.m_IsProcessClick = false;
	}
	
	public void ProcessFinish()
	{
		this.m_IsProcessClick = false;
	}
	
	public void HideButton()
	{
		this.gameObject.SetActive(false);
		this.m_CostPrompt.HideWindow();
	}
	
	public void DisplayButton()
	{
		this.gameObject.SetActive(true);//LogicController.Instance.PlayerData.CurrentStoreGold >= this.m_CostValue);
	}
	
	IEnumerator Wait()
	{
		yield return new WaitForSeconds(1.5f);
		this.m_Finder.FindMatch();
	}
}
