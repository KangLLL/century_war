using UnityEngine;
using System.Collections;
using ConfigUtilities;

public class RivalInformationBehavior : MonoBehaviour 
{
	[SerializeField]
	private BattleDirector m_Director;
	[SerializeField]
	private BattleSceneHelper m_SceneHelper;
	[SerializeField]
	private UILabel m_RivalNameLabel;
	[SerializeField]
	private UILabel m_RivalLevelLabel;
	[SerializeField]
	private UILabel m_RivalHonourLabel;
	[SerializeField]
	private UILabel m_RivalGoldLabel;
	[SerializeField]
	private UILabel m_RivalFoodLabel;
	[SerializeField]
	private UILabel m_RivalAgeLabel;
	[SerializeField]
	private UISprite m_RivalPropsSprite;
	[SerializeField]
	private UIGrid m_RivalBuffsGrid;
	[SerializeField]
	private GameObject m_BuffPrefab;
	
	private int m_PreviousGold;
	private int m_PreviousFood;
	
	private string m_PreviousRivalName = string.Empty;
	
	void Update()
	{
		if(!string.IsNullOrEmpty(this.m_Director.CurrentRivalName) && !BattleDirector.Instance.IsBattleStart && 
			!this.m_PreviousRivalName.Equals(this.m_Director.CurrentRivalName))
		{
			this.m_RivalNameLabel.text = this.m_Director.CurrentRivalName;
			this.m_RivalLevelLabel.text = this.m_Director.CurrentRivalLevel.ToString();
			this.m_RivalHonourLabel.text = Mathf.FloorToInt(this.m_Director.CurrentRivalHonour * 
				ConfigInterface.Instance.SystemConfig.MatchObtainHonourPercentage).ToString();
			this.m_RivalGoldLabel.text = this.m_SceneHelper.TotalGold.ToString();
			this.m_RivalFoodLabel.text = this.m_SceneHelper.TotalFood.ToString();
			this.m_RivalAgeLabel.text = ClientConfigConstants.Instance.GetAgeName(this.m_Director.CurrentRivalAge);
			
			if(this.m_Director.CurrentRivalPropsType.HasValue)
			{
				this.m_RivalPropsSprite.enabled = true;
				PropsConfigData propsConfig = ConfigInterface.Instance.PropsConfigHelper.GetPropsData(this.m_Director.CurrentRivalPropsType.Value);
				this.m_RivalPropsSprite.spriteName = propsConfig.PrefabName;
				this.m_RivalPropsSprite.MakePixelPerfect();
			}
			else
			{
				this.m_RivalPropsSprite.enabled = false;
			}
			
			this.m_PreviousGold = this.m_SceneHelper.TotalGold;
			this.m_PreviousFood = this.m_SceneHelper.TotalFood;
			
			for(int i = this.m_RivalBuffsGrid.transform.childCount - 1; i >=0; i--)
			{
				GameObject.Destroy(this.m_RivalBuffsGrid.transform.GetChild(i).gameObject);
			}
			foreach (PropsBuffConfigData buff in BuildingBuffSystem.Instance.AllBuffsData) 
			{
				GameObject buffIcon = GameObject.Instantiate(this.m_BuffPrefab) as  GameObject;
				buffIcon.transform.parent = this.m_RivalBuffsGrid.transform;
				
				BuffIconBehavior iconBehavior = buffIcon.GetComponentInChildren<BuffIconBehavior>();
				iconBehavior.IconName = buff.PrefabName;
			}
			this.m_RivalBuffsGrid.repositionNow = true;
			this.m_PreviousRivalName = this.m_Director.CurrentRivalName;
		}
		if(BattleDirector.Instance.IsBattleStart)
		{
			this.RefreshInformation();
		}
	}
	
	private void RefreshInformation()
	{
		if(this.m_PreviousGold != this.m_SceneHelper.TotalGold)
		{
			this.m_RivalGoldLabel.GetComponent<ValueLabelBehavior>().RefreshToValue(this.m_SceneHelper.TotalGold);
			this.m_PreviousGold = this.m_SceneHelper.TotalGold;
		}
		if(this.m_PreviousFood != this.m_SceneHelper.TotalFood)
		{
			this.m_RivalFoodLabel.GetComponent<ValueLabelBehavior>().RefreshToValue(this.m_SceneHelper.TotalFood);
			this.m_PreviousFood = this.m_SceneHelper.TotalFood;
		}
	}
}
