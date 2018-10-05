using UnityEngine;
using System.Collections;
using ConfigUtilities;
using ConfigUtilities.Enums;

public class SummaryInformation : MonoBehaviour 
{
	[SerializeField]
	private UILabel m_ResultLabel;
	[SerializeField]
	private UILabel m_PercentageLabel;
	[SerializeField]
	private UILabel m_GoldLabel;
	[SerializeField]
	private UILabel m_FoodLabel;
	[SerializeField]
	private GameObject m_TrophyPropsPrefab;
	[SerializeField]
	private Transform m_TrophyPropsParent;
	[SerializeField]
	private UILabel m_HonourLabel;
	[SerializeField]
	private UISprite[] m_ProgressSprites;
	[SerializeField]
	private GameObject m_ReturnButton;
	[SerializeField]
	private GameObject m_StarEffectPrefab;
	
	[SerializeField]
	private BattleSummary m_Summary;
	[SerializeField]
	private GameObject m_ActivityView;
	
	private int m_PropsIndex;
		
	// Use this for initialization
	void Start () 
	{
		this.m_ResultLabel.text = this.m_Summary.IsWin ? ClientStringConstants.BATTLE_WIN_TITLE : 
			ClientStringConstants.BATTLE_LOSE_TITLE;
		this.m_PercentageLabel.text = Mathf.FloorToInt(BattleRecorder.Instance.DestroyBuildingPercentage * 100) + "%";
		this.m_GoldLabel.text = BattleRecorder.Instance.GoldTrophy.ToString();
		this.m_FoodLabel.text = BattleRecorder.Instance.FoodTrophy.ToString();
		
		if(BattleRecorder.Instance.IsDestroyPropsStorage && BattleDirector.Instance.CurrentRivalPropsType.HasValue)
		{
			this.AddPropsIcon(BattleDirector.Instance.CurrentRivalPropsType.Value);
		}
	    foreach(PropsType type in BattleRecorder.Instance.PropsTrophy)
		{
			this.AddPropsIcon(type);
		}
		
		this.m_HonourLabel.text = this.m_Summary.IsWin ? this.m_Summary.CalculatedHonour.ToString() :
			"-" + this.m_Summary.CalculatedHonour;
		
		
		int totalPercentage = (int)(BattleRecorder.Instance.DestroyBuildingPercentage * 100); 
		int progress = totalPercentage < ClientConfigConstants.Instance.BattleProgressStep[0] ? 0 : 
			totalPercentage < ClientConfigConstants.Instance.BattleProgressStep[1] ? 1 : 2;
		if(BattleRecorder.Instance.IsDestroyCityHall)
		{
			progress ++;
		}
		for(int i = 0; i < progress; i ++)
		{
			this.m_ProgressSprites[i].spriteName =  i == 1 ? ClientStringConstants.SUMMARY_SCREEN_FULL_FILL_BIG_STAR_SPRITE_NAME :
				ClientStringConstants.SUMMARY_SCREEN_FULL_FILL_SMALL_STAR_SPRITE_NAME;
			this.m_ProgressSprites[i].MakePixelPerfect();
			
			GameObject starEffect = GameObject.Instantiate(this.m_StarEffectPrefab) as GameObject;
			starEffect.transform.position = this.m_ProgressSprites[i].transform.position + starEffect.transform.position;
		}
		
		if(progress > 0)
		{
			AudioController.Play("ObtainStar");
		}
		
		this.m_ActivityView.SetActive(true);
	}
	
	void Update()
	{
		if(BattleDirector.Instance.IsReceivedReplayID && !this.m_ReturnButton.activeSelf)
		{
			this.m_ReturnButton.SetActive(true);
			this.m_ActivityView.SetActive(false);
		}
	}
	
	private void AddPropsIcon(PropsType propsType)
	{
		GameObject propsObject = GameObject.Instantiate(this.m_TrophyPropsPrefab) as GameObject;
		UISprite propsSprite = propsObject.GetComponent<UISprite>();
		propsSprite.spriteName = ConfigInterface.Instance.PropsConfigHelper.
			GetPropsData(propsType).PrefabName;
		propsObject.transform.parent = this.m_TrophyPropsParent;
		propsSprite.MakePixelPerfect();
		
		string order = this.m_PropsIndex < 10 ? "0" + this.m_PropsIndex : this.m_PropsIndex.ToString();
		propsObject.name = order + "_" + propsObject.name;
		this.m_PropsIndex ++;
	}
}
