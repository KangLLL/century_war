using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommandConsts;
using ConfigUtilities.Enums;

public class ReplayDirector : MonoBehaviour 
{
	[SerializeField]
	private GridFactory m_GridFactory;
	[SerializeField]
	private ObstacleFactory m_BuildingFactory;
	[SerializeField]
	private CharacterFactory m_CharacterFactory;
	[SerializeField]
	private BattleSceneDirector m_SceneDirector;
	[SerializeField]
	private CloudBehaviour m_Cloud;
	
	[SerializeField]
	private GameObject m_ProgressIcon;
	[SerializeField]
	private GameObject m_TimeTicks;
	[SerializeField]
	private GameObject m_SpeedButton;
	[SerializeField]
	private GameObject m_VeilObject;
	[SerializeField]
	private GameObject m_ReplayButton;
	
	[SerializeField]
	private GameObject m_BulletParent;
	[SerializeField]
	private LoadAgeMap m_Map;
	
	[SerializeField]
	private BattlePreloadManager m_PreloadManager;
	
	private int m_TotalReplayTick;
	private bool m_IsReplayStart;
	private bool m_IsReceivedInformation;
	
	private MatchLogResponseParameter m_CurrentReplay;
	
	private Queue<DropArmyResponseParameter> m_DropCommands;
	private Queue<DropMercenaryResponseParameter> m_DropMercenaryCommands;
	private Queue<UsePropsResponseParameter> m_UsePropsCommands;
	
	private int m_ReplayStartTick;
	
	private static ReplayDirector s_Sigleton;
	
	public static ReplayDirector Instance 	
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
	
	public int TotalReplayTick
	{
		get
		{
			return this.m_TotalReplayTick;
		}
	}
	
	public int ReplayStartTick
	{
		get
		{
			return this.m_ReplayStartTick;
		}
	}
	
	public bool IsReplayStart
	{
		get
		{
			return this.m_IsReplayStart;
		}
	}
	
	public Age CurrentAge 
	{ 
		get
		{
			return this.m_BuildingFactory.CurrentAge;
		}
	}
	
	void Start()
	{
		Resources.UnloadUnusedAssets();
		AudioController.PlayMusic("BattleStart");
		
		this.m_DropCommands = new Queue<DropArmyResponseParameter>();
		this.m_DropMercenaryCommands = new Queue<DropMercenaryResponseParameter>();
		this.m_UsePropsCommands = new Queue<UsePropsResponseParameter>();
		MatchLogRequestParameter request = new MatchLogRequestParameter();
		request.MatchID = ReplayData.MatchID;
		CommunicationUtility.Instance.GetReplayDetail(request, this, "GetReplayDetail", true);
				
	}
	
	void FixedUpdate()
	{
		if(this.m_IsReceivedInformation)
		{
			this.m_ReplayStartTick = TimeTickRecorder.Instance.CurrentTimeTick;
			this.m_IsReceivedInformation = false;
			this.m_SceneDirector.SendRunAway();
			BattleSceneHelper.Instance.EnableBuildingAI();
			this.m_IsReplayStart = true;
		}
		if(this.m_IsReplayStart)
		{
			int currentTick = TimeTickRecorder.Instance.CurrentTimeTick - this.m_ReplayStartTick;
			if(this.m_DropCommands.Count > 0)
			{
				while(this.m_DropCommands.Count > 0 && currentTick >= this.m_DropCommands.Peek().OperateTime)
				{
					DropArmyResponseParameter army = this.m_DropCommands.Dequeue();
					this.m_CharacterFactory.ConstructArmy(army.ArmyType, army.Level, 
						new Vector3(army.PositionX, army.PositionY, 0));
				}
			}
			if(this.m_DropMercenaryCommands.Count > 0)
			{
				while(this.m_DropMercenaryCommands.Count > 0 && currentTick >= this.m_DropMercenaryCommands.Peek().OperateTime)
				{
					DropMercenaryResponseParameter mercenary = this.m_DropMercenaryCommands.Dequeue();
					this.m_CharacterFactory.ConstructMercenary(mercenary.MercenaryType, 
						new Vector3(mercenary.PositionX, mercenary.PositionY,0));
				}
			}
			if(this.m_UsePropsCommands.Count > 0)
			{
				while(this.m_UsePropsCommands.Count > 0 && currentTick >= this.m_UsePropsCommands.Peek().OperateTime)
				{
					UsePropsResponseParameter props = this.m_UsePropsCommands.Dequeue();
					this.m_CharacterFactory.UseProps(props.PropsType,
						new Vector3(props.PositionX, props.PositionY,0));
				}
			}
			if(currentTick > this.m_TotalReplayTick)
			{
				this.ReplayOver();
			}
		}
	}
	
	public void GetReplayDetail(Hashtable result)
	{
		MatchLogResponseParameter param = new MatchLogResponseParameter();
		param.InitialParameterObjectFromHashtable(result);
		
		
		Dictionary<ArmyType, int> armies = new Dictionary<ArmyType, int>();
		List<MercenaryType> mercenaries = new List<MercenaryType>();
		List<PropsType> props = new List<PropsType>();
		
		foreach(DropArmyResponseParameter drop in param.DropArmyCommands)
		{
			if(!armies.ContainsKey(drop.ArmyType))
			{
				armies.Add(drop.ArmyType, drop.Level);
			}
		}
		
		foreach (DropMercenaryResponseParameter drop in param.DropMercenaryCommands) 
		{
			if(!mercenaries.Contains(drop.MercenaryType))
			{
				mercenaries.Add(drop.MercenaryType);
			}
		}
		
		foreach (UsePropsResponseParameter command in param.UsePropsCommands) 
		{
			if(!props.Contains(command.PropsType))
			{
				props.Add(command.PropsType);
			}
		}
		
		this.m_PreloadManager.Preload(armies, mercenaries, props);
		
		this.m_TotalReplayTick = param.TotalTime;
		this.ConstructScene(param);
		
        StartCoroutine("DelayCloudFadeOut");
    }

    IEnumerator DelayCloudFadeOut()
    {
        yield return new WaitForSeconds(0.5f);
        m_Cloud.FadeOut();
		yield return new WaitForSeconds(3f);
		this.m_IsReceivedInformation = true;
    }

	public void ConstructScene(MatchLogResponseParameter param)
	{	
		BattleRandomer.Instance.SetSeed(param.RandomSeed);
		param.DropArmyCommands.Sort((x, y) => x.OperateTime.CompareTo(y.OperateTime));
		for(int i = 0; i < param.DropArmyCommands.Count; i ++)
		{
			DropArmyResponseParameter army = param.DropArmyCommands[i];
			this.m_DropCommands.Enqueue(army);
		}
		param.DropMercenaryCommands.Sort((x, y) => x.OperateTime.CompareTo(y.OperateTime));
		for(int i = 0; i < param.DropMercenaryCommands.Count; i ++)
		{
			DropMercenaryResponseParameter mercenary = param.DropMercenaryCommands[i];
			this.m_DropMercenaryCommands.Enqueue(mercenary);
		}
		param.UsePropsCommands.Sort((x, y) => x.OperateTime.CompareTo(y.OperateTime));
		for(int i = 0; i < param.UsePropsCommands.Count; i++)
		{
			UsePropsResponseParameter props = param.UsePropsCommands[i];
			this.m_UsePropsCommands.Enqueue(props);
		}
		
		BattleSceneHelper.Instance.ClearObject();
		this.m_GridFactory.Clear();
		this.m_SceneDirector.ClearAllActors();
		Resources.UnloadUnusedAssets();
		
		this.m_BuildingFactory.ConstructBuilding(param.RivalInformation);
		this.m_Map.SetMap(this.CurrentAge);
		BattleMapData.Instance.ConstructGridArray();
		this.m_GridFactory.ConstructGird();
		this.m_SceneDirector.GenerateActors();
		BattleRecorder.Instance.ClearRecords();
		
		this.m_CurrentReplay = param;
	}
	
	private void ReplayOver()
	{
		this.m_IsReplayStart = false;
		this.m_ProgressIcon.SetActive(false);
		this.m_SpeedButton.SetActive(false);
		this.m_TimeTicks.SetActive(false);
		this.m_VeilObject.SetActive(true);
		this.m_ReplayButton.SetActive(true);
		
		BattleSceneHelper.Instance.DestroyAllInvaders();
		for (int i = this.m_BulletParent.transform.childCount - 1; i >=0; i --) 
		{
			GameObject.DestroyImmediate(this.m_BulletParent.transform.GetChild(i).gameObject);
		}
	}
	
	public void EndReplay()
	{
		Time.timeScale = 1;
		this.m_Cloud.FadeIn();
		this.StartCoroutine("Wait");
		LockScreen.Instance.DisableInput();
	}
	
	IEnumerator Wait()
	{
		yield return new WaitForSeconds(1.5f);
		Application.LoadLevel(ClientStringConstants.BUILDING_SCENE_LEVEL_NAME);
	}
	
	public void Replay()
	{
		this.m_ProgressIcon.SetActive(true);
		this.m_SpeedButton.SetActive(true);
		this.m_TimeTicks.SetActive(true);
		this.m_VeilObject.SetActive(false);
		this.m_ReplayButton.SetActive(false);
		this.m_ProgressIcon.GetComponent<BattleProgressBehavior>().Clear();
		this.m_SpeedButton.GetComponent<ButtonSpeed>().Clear();
		this.ConstructScene(this.m_CurrentReplay);
		this.m_IsReceivedInformation = true;
	}
}
