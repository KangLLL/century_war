using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommandConsts;
using ConfigUtilities;
using ConfigUtilities.Enums;
using System;

public class BattleDirector : MonoBehaviour 
{
	[SerializeField]
	private GridFactory m_GridFactory;
	[SerializeField]
	private ObstacleFactory m_BuildingFactory;
	[SerializeField]
	private BattleSceneHelper m_SceneHelper;
	[SerializeField]
	private BattleSceneDirector m_SceneDirector;
	[SerializeField]
	private BattleSummary m_Summary;
	[SerializeField]
	private ButtonNextRival m_NextButton;
	[SerializeField]
	private CloudBehaviour m_Cloud;
	[SerializeField]
	private CameraInitializer m_CameraInitializer;
	[SerializeField]
	private LoadAgeMap m_Map;

	private long m_CurrentRivalID;
	private string m_CurrentRivalName;
	private int m_CurrentRivalLevel;
	private int m_CurrentRivalHonour;
	
	private Nullable<PropsType> m_CurrentRivalPropsType;
	
	private int m_TotalObserveTick;
	private int m_TotalMatchTick;
	 
	private bool m_IsBattleStart;
	private bool m_IsBattleObserve;
	private bool m_IsBattleFinished;
	private int m_MatchObserveStartTick;
	private int m_MatchStartTick;
	
	private bool m_IsReceivedReplayID;
	
	private bool m_EndBattle;
	
	private static BattleDirector s_Sigleton;
	
	public static BattleDirector Instance 	
	{
		get
		{
			return s_Sigleton;
		}
	}
	
	void Awake()
	{
		s_Sigleton = this;
		this.m_TotalObserveTick = ConfigInterface.Instance.SystemConfig.MatchObserveLimitSecond * 
			ClientConfigConstants.Instance.TicksPerSecond;
		this.m_TotalMatchTick = ConfigInterface.Instance.SystemConfig.MatchDurationSecond *
			ClientConfigConstants.Instance.TicksPerSecond;
		
		AudioController.PlayMusic("BattleVisit");
	}
	
	void OnDestroy()
	{
		s_Sigleton = null;
		if(this.m_IsBattleStart)
		{
			BattleData.IsNewbie = false;
		}
	}
	
	public string CurrentRivalName
	{
		get
		{
			return this.m_CurrentRivalName;
		}
	}
	
	public int CurrentRivalLevel
	{
		get
		{
			return this.m_CurrentRivalLevel;
		}
	}
	
	public Age CurrentRivalAge 
	{ 
		get
		{
			return this.m_BuildingFactory.CurrentAge;
		}
	}
	
	public int CurrentRivalHonour
	{
		get
		{
			return this.m_CurrentRivalHonour;
		}
	}
	
	public Nullable<PropsType> CurrentRivalPropsType
	{
		get
		{
			return this.m_CurrentRivalPropsType;
		}
	}
	
	public int MatchObserveStartTick
	{
		get
		{
			return this.m_MatchObserveStartTick;
		}
	}
	
	public int MatchStartTick
	{
		get
		{
			return this.m_MatchStartTick;
		}
	}
	
	public bool IsBattleStart
	{
		get
		{
			return this.m_IsBattleStart;
		}
	}
	
	public bool IsBattleFinished
	{
		get
		{
			return this.m_IsBattleFinished;
		}
	}
	
	public bool IsReceivedReplayID
	{
		get
		{
			return this.m_IsReceivedReplayID;
		}
	}
	
	public void ConstructScene(FindRivalResponseParameter rival)
	{	
		this.m_CameraInitializer.Reset();
		this.m_MatchObserveStartTick = TimeTickRecorder.Instance.CurrentTimeTick;
		//this.CurrentRivalAge = CommonHelper.GetAgeFromPlayerLevel(rival.RivalLevel);
		this.m_IsBattleObserve = true;
		
		this.m_CurrentRivalID = rival.RivalID;
		this.m_CurrentRivalName = rival.RivalName;
		this.m_CurrentRivalLevel = rival.RivalLevel;
		this.m_CurrentRivalHonour = rival.RivalHonour;
		
		this.m_CurrentRivalPropsType = rival.RivalPropsType;
		
		this.m_SceneHelper.ClearObject();
		this.m_GridFactory.Clear();
		this.m_SceneDirector.ClearAllActors();
		Resources.UnloadUnusedAssets();
		
		this.m_BuildingFactory.ConstructBuilding(rival);
		this.m_Map.SetMap(this.CurrentRivalAge);
		BattleMapData.Instance.ConstructGridArray();
		this.m_GridFactory.ConstructGird();
		this.m_SceneDirector.GenerateActors();
		
		if(BattleData.RivalInformation != null)
		{
			this.m_NextButton.HideButton();
		}
	}
	
	public void EndObserve()
	{
		this.m_IsBattleObserve = false;
	}
	
	public void EndMatch()
	{
		LockScreen.Instance.DisableInput();
		this.m_EndBattle = true;
		this.m_IsBattleFinished = true;
	}
	
	IEnumerator Wait()
	{
		yield return new WaitForSeconds(1.5f);
		Application.LoadLevel(ClientStringConstants.BUILDING_SCENE_LEVEL_NAME);
	}
	
	public void GetMatchID(Hashtable result)
	{
		if(result != null)
		{
			CurrentMatchLogResponseParameter response = new CurrentMatchLogResponseParameter();
			response.InitialParameterObjectFromHashtable(result);
			
			BattleData.IsLoseProps = false;
			if(this.m_CurrentRivalPropsType.HasValue && BattleRecorder.Instance.IsDestroyPropsStorage)
			{
				if(LogicController.Instance.AllProps.Count >= LogicController.Instance.PlayerData.PropsMaxCapacity)
				{
					BattleData.IsLoseProps = true;
				}
				else
				{
					LogicController.Instance.GenerateProps(this.m_CurrentRivalPropsType.Value);
				}
			}
			foreach (PropsType trophy in BattleRecorder.Instance.PropsTrophy) 
			{
				if(LogicController.Instance.AllProps.Count >= LogicController.Instance.PlayerData.PropsMaxCapacity)
				{
					break;
				}
				else
				{
					LogicController.Instance.GenerateProps(trophy);
				}
			}
			LogicController.Instance.AddAttackLog(this.GenerateAttackLog(response.MatchID));
		}
		Debug.Log("received");
		this.m_IsReceivedReplayID = true;
	}
	
	private LogData GenerateAttackLog(long matchID)
	{
		LogData result = new LogData();
		
		result.MatchID = matchID;
		result.RivalName = this.m_CurrentRivalName;
		result.RivalID = this.m_CurrentRivalID;
		result.RivalHonour = this.m_CurrentRivalHonour;
		result.PlunderOil = BattleRecorder.Instance.OilTrophy;
		result.PlunderGold = BattleRecorder.Instance.GoldTrophy;
		result.PlumderFood = BattleRecorder.Instance.FoodTrophy;
		result.PlunderHonour = BattleSummary.Instance.IsWin ? BattleSummary.Instance.CalculatedHonour :
			-BattleSummary.Instance.CalculatedHonour;
		result.PlunderProps = this.m_CurrentRivalPropsType;
		result.PropsThropy = BattleRecorder.Instance.PropsTrophy;
		result.IsDestroyCityHall = BattleRecorder.Instance.IsDestroyCityHall;
		result.DestroyBuildingPercentage = (int)(BattleRecorder.Instance.DestroyBuildingPercentage * 100);
		
		result.ElapsedTime = (long)((TimeTickRecorder.Instance.CurrentTimeTick - this.m_MatchStartTick) / 
			(float)ClientConfigConstants.Instance.TicksPerSecond);
		result.ArmyInfos = new Dictionary<ArmyType, DropArmyInfo>();
		Dictionary<ArmyType, List<RecordUserCommand<ArmyIdentity>>> dropArmies = BattleRecorder.Instance.DropArmies;
		foreach (ArmyType type in dropArmies.Keys) 
		{
			int level = ((ConstructArmyCommand)BattleRecorder.Instance.DropArmies[type][0].ConstructCommand).Level;
			result.ArmyInfos.Add(type, new DropArmyInfo() {  Quantity = dropArmies[type].Count, Level = level} );
		}
		result.MercenaryInfos = new Dictionary<MercenaryType, int>();
		Dictionary<MercenaryType, List<RecordUserCommand<MercenaryIdentity>>> dropMercenaries = BattleRecorder.Instance.DropMercenaries;
		foreach (MercenaryType type in dropMercenaries.Keys) 
		{
			result.MercenaryInfos.Add(type, dropMercenaries[type].Count);
		}
		result.PropsInfos = new List<KeyValuePair<PropsType, int>>();
		List<KeyValuePair<PropsType, int>> useProps = BattleRecorder.Instance.UseProps;
		foreach (KeyValuePair<PropsType, int> props in useProps) 
		{
			result.PropsInfos.Add(props);
		}
		result.Version = ClientVersion.Instance.Version;
		return result;
	}
	
	public void StartBattle()
	{
		this.EndObserve();
		this.m_IsBattleStart = true;
		this.m_MatchStartTick = TimeTickRecorder.Instance.CurrentTimeTick;
		BattleRecorder.Instance.BattleStartTime = this.m_MatchStartTick;
		this.m_SceneDirector.SendRunAway();
		this.m_NextButton.HideButton();
		
		AudioController.PlayMusic("BattleStart");
		
		BattleSceneHelper.Instance.EnableBuildingAI();
		int battleRandomSeed = System.Environment.TickCount;
		BattleRandomer.Instance.SetSeed(battleRandomSeed);
		StartMatchRequestParameter request = new StartMatchRequestParameter();
		request.RandomSeed = battleRandomSeed;
		request.Version = ClientVersion.Instance.Version;
		CommunicationUtility.Instance.StartMatch(request);
		
		if(BattleData.RelatedLog != null)
		{
			BattleData.RelatedLog.CanRevenge = false;
		}
	}
	
	void FixedUpdate()
	{
		if(this.m_IsBattleStart)
		{
			int currentTick = TimeTickRecorder.Instance.CurrentTimeTick - this.m_MatchStartTick;
			if(currentTick == this.m_TotalMatchTick)
			{
				this.EndMatch();
			}
		}
		else if(this.m_IsBattleObserve)
		{
			int currentTick = TimeTickRecorder.Instance.CurrentTimeTick - this.m_MatchObserveStartTick;
			if(currentTick >= this.m_TotalObserveTick)
			{
				this.StartBattle();
			}
		}
		if(this.m_EndBattle)
		{
			if(this.m_IsBattleStart)
			{
				this.m_Summary.Summary();
				this.m_IsBattleStart = false;
				
				FinishMatchRequestParameter request = new FinishMatchRequestParameter();
				request.MatchFinishTime = TimeTickRecorder.Instance.CurrentTimeTick - this.m_MatchStartTick;
				CommunicationUtility.Instance.GetMatchID(request, this, "GetMatchID", true);
				
				if(BattleData.IsNewbie)
				{
					this.GetMatchID(null);
					BattleData.IsNewbie = false;
				}
			}
			else
			{
				LockScreen.Instance.DisableInput();
				this.m_IsBattleObserve = false;
				this.m_Cloud.FadeIn();
				CommunicationUtility.Instance.FinishObserve();
				this.StartCoroutine("Wait");
			}
			this.m_EndBattle = false;
		}
	}
}
