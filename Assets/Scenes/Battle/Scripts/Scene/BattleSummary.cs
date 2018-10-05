using UnityEngine;
using System.Collections;
using ConfigUtilities;
using CommonUtilities;

public class BattleSummary : MonoBehaviour 
{
	[SerializeField]
	private GameObject m_MainUI;
	[SerializeField]
	private GameObject m_SummaryUI;
	[SerializeField]
	private GameObject m_InvaderBulletsParent;
	[SerializeField]
	private ObstacleFactory m_BuildingFactory;
	[SerializeField]
	private ExtraRewardDialog m_RewardDialog;
	
	private static BattleSummary s_Sigleton;
	
	private bool m_IsWin;
	private int m_CalculatedHonour;
	
	public bool IsWin
	{
		get
		{
			return this.m_IsWin;
		}
	}
	
	public int CalculatedHonour
	{
		get
		{
			return this.m_CalculatedHonour;
		}
	}
	
	public static BattleSummary Instance
	{
		get
		{
			return s_Sigleton;
		}
	}
	
	void Awake()
	{
		s_Sigleton = this;
	}
	
	void OnDestroy()
	{
		s_Sigleton = null;
	}
	
	public void Summary()
	{
		GameObject.DestroyImmediate(this.m_InvaderBulletsParent);
		
		if(BattleRecorder.Instance.DestroyBuildingPercentage < 0.5 && !BattleRecorder.Instance.IsDestroyCityHall)
		{
			this.OnLose();
		}
		else
		{
			this.OnWin();
			int rewardGold = CommonUtilities.CommonUtilities.GetExtraRewardGold(this.m_BuildingFactory.CurrentRivalCityHallLevle);
			int rewardFood = CommonUtilities.CommonUtilities.GetExtraRewardFood(this.m_BuildingFactory.CurrentRivalCityHallLevle);
			int rewardOil = CommonUtilities.CommonUtilities.GetExtraRewardOil(this.m_BuildingFactory.CurrentRivalCityHallLevle);
			LogicController.Instance.RewardVictoryResource(rewardGold, rewardFood, rewardOil);
			this.m_RewardDialog.ShowReward(rewardGold, rewardFood);
		}
		BattleSceneHelper.Instance.DestroyAllInvaders();
		this.StartCoroutine("Wait");
	}
	
	IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.5f);
		this.m_MainUI.gameObject.SetActive(false);
		yield return new WaitForSeconds(1.5f);
        this.m_SummaryUI.gameObject.SetActive(true);
		
		if(this.m_IsWin)
		{
			AudioController.PlayMusic("BattleWin");
		}
		else
		{
			AudioController.PlayMusic("BattleLost");
		}
    }
	
	private void OnLose()
	{
		int totalHonour = Mathf.FloorToInt(LogicController.Instance.PlayerData.Honour * 
			ConfigInterface.Instance.SystemConfig.MatchObtainHonourPercentage);
		float losePercentage = 1 - BattleRecorder.Instance.DestroyBuildingPercentage;
		this.m_CalculatedHonour  = Mathf.FloorToInt(totalHonour * losePercentage);
		this.m_IsWin = false;
		
		LogicController.Instance.LoseHonour(this.m_CalculatedHonour);
	}
	
	private void OnWin()
	{

		int totalHonour = Mathf.FloorToInt(BattleDirector.Instance.CurrentRivalHonour * 
			ConfigInterface.Instance.SystemConfig.MatchObtainHonourPercentage);
		int cityHallHonour = Mathf.FloorToInt(totalHonour *
			ConfigInterface.Instance.SystemConfig.MatchCityHallHonourPercentage);
		int destroyHonour = totalHonour - cityHallHonour;
		
		int result = Mathf.FloorToInt(destroyHonour * BattleRecorder.Instance.DestroyBuildingPercentage);
		if(BattleRecorder.Instance.IsDestroyCityHall)
		{
			result = result + cityHallHonour;
		}
		this.m_CalculatedHonour = result;
		this.m_IsWin = true;
		
		LogicController.Instance.WinHonour(this.m_CalculatedHonour);
	}
}
