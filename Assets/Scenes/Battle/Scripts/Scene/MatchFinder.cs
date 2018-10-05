using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommandConsts;
using ConfigUtilities.Enums;

public class MatchFinder : MonoBehaviour 
{
	[SerializeField]
	private BattleDirector m_Director;
	[SerializeField]
	private ButtonNextRival m_NextButton;
	[SerializeField]
	private TimeTipsBehavior m_TimeTips;
	[SerializeField]
	private UICamera m_EventHandler;
	[SerializeField]
	private CloudBehaviour m_Cloud;
	[SerializeField]
	private NotFoundBehavior m_NotFound;
	[SerializeField]
	private GameObject m_ActivityView;
	[SerializeField]
	private BattlePreloadManager m_PreloadManager;
	
	[SerializeField]
	private GameObject m_NewbieGuide;
	
	private long m_CurrentRivalID = -1;
	
	void Awake()
	{
		List<KeyValuePair<PropsType, List<int>>> availableProps = new List<KeyValuePair<PropsType, List<int>>>();
		foreach (int propsNo in LogicController.Instance.AvailableBattleProps) 
		{
			PropsLogicData logicData = LogicController.Instance.GetProps(propsNo);
			List<int> noList = new List<int>();
			availableProps.Add(new KeyValuePair<PropsType, List<int>>(logicData.PropsType, noList));

			for(int i = 0; i < logicData.RemainingUseTime; i ++)
			{
				noList.Add(logicData.PropsNo);
			}
		}
		ArmyMenuPopulator.Instance.AvailableProps = availableProps;
		
		ArmyMenuPopulator.Instance.AvailableArmies = LogicController.Instance.AvailableArmies;
		ArmyMenuPopulator.Instance.ArmyLevel = new Dictionary<ArmyType, int>();
		
		for(int i = 0; i < (int)ArmyType.Length; i ++)
		{
			ArmyType type = (ArmyType)i;
			ArmyMenuPopulator.Instance.ArmyLevel.Add(type, LogicController.Instance.GetArmyLevel(type));
		}
		ArmyMenuPopulator.Instance.AvailableMercenaries = LogicController.Instance.AvailableMercenaries;
		
		Dictionary<ArmyType, int> armies = new Dictionary<ArmyType, int>();
		foreach (KeyValuePair<ArmyType, List<ArmyIdentity>> army in LogicController.Instance.AvailableArmies) 
		{
			armies.Add(army.Key, ArmyMenuPopulator.Instance.ArmyLevel[army.Key]);
		}
		List<MercenaryType> mercenaries = new List<MercenaryType>();
		foreach (KeyValuePair<MercenaryType, List<MercenaryIdentity>> mercenary in LogicController.Instance.AvailableMercenaries) 
		{
			mercenaries.Add(mercenary.Key);
		}
		List<PropsType> props = new List<PropsType>();
		foreach (int propsNo in LogicController.Instance.AvailableBattleProps) 
		{
			PropsLogicData data = LogicController.Instance.GetProps(propsNo);
			if(!props.Contains(data.PropsType))
			{
				props.Add(data.PropsType);
			}
		}
		this.m_PreloadManager.Preload(armies, mercenaries, props);
	}
	
	void Start()
	{	
		if(BattleData.RivalInformation != null)
		{
			this.NotifyConstructScene(BattleData.RivalInformation);
			LogicController.Instance.Revenge();
			BattleData.RivalInformation = null;
		}
		else
		{
			BattleData.RelatedLog = null;
			LockScreen.Instance.DisableInput();
			this.FindMatch();
		}
	}
	
	public void FindMatch()
	{
		this.m_ActivityView.SetActive(true);
		this.m_TimeTips.HideTime();
		if(BattleData.IsNewbie)
		{
			CommunicationUtility.Instance.GetNewbieRival(this, "RivalFound", true);
			GameObject.Instantiate(this.m_NewbieGuide);
		}
		else
		{
			if(this.m_CurrentRivalID >= 0)
			{
				SkipRivalRequestParameter parameter = new SkipRivalRequestParameter();
				parameter.RivalID = this.m_CurrentRivalID;
				CommunicationUtility.Instance.SkipRival(parameter);
			}
			CommunicationUtility.Instance.GetRivalData(this, "RivalFound", true); 
		}
	}
	
	private void RivalFound(Hashtable rival)
	{
		this.m_ActivityView.SetActive(false);
		if(rival.Count == 0)
		{
			this.m_NotFound.Show();
		}
		else
		{
			Hashtable rivalData = rival;
			FindRivalResponseParameter parameter = new FindRivalResponseParameter();
			parameter.InitialParameterObjectFromHashtable(rivalData);
			print(parameter.RivalName);
			LogicController.Instance.FindMatch();
			this.NotifyConstructScene(parameter);
		}
	}
	
	private void NotifyConstructScene(FindRivalResponseParameter rival)
	{
		this.m_Director.ConstructScene(rival);
		this.m_CurrentRivalID = rival.RivalID;
		if(BattleData.RivalInformation == null)
		{
			this.m_NextButton.DisplayButton();
		}
		this.m_NextButton.ProcessFinish();
		this.StartCoroutine("Wait");
		this.m_TimeTips.ShowTime();
	}
	
	IEnumerator Wait()
	{
		yield return new WaitForSeconds(1.0f);
		this.m_Cloud.FadeOut();
		yield return new WaitForSeconds(1.5f);
		LockScreen.Instance.EnableInput();
	}
}
