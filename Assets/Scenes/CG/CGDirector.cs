using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommandConsts;
using ConfigUtilities.Enums;

public class CGDirector : MonoBehaviour 
{
	[SerializeField]
	private ObstacleFactory m_BuildingFactory;
	[SerializeField]
	private CharacterFactory m_CharacterFactory;
	[SerializeField]
	private BattleSceneDirector m_SceneDirector;
	[SerializeField]
	private CloudBehaviour m_Cloud;
	
	[SerializeField]
	private GameObject m_BulletParent;
	[SerializeField]
	private LoadAgeMap m_Map;
	[SerializeField]
	private ReplayDataReader m_Reader;
	[SerializeField]
	private Age m_CurrentAge;
	[SerializeField]
	private CGNewbieGuide m_Guide;
	[SerializeField]
	private BattlePreloadManager m_PreloadManager;
	
	private int m_TotalCGTick;
	private bool m_IsCGStart;
	private bool m_IsCGEnd;
	private MatchLogResponseParameter m_Match;
	
	private Queue<DropArmyResponseParameter> m_DropCommands;
	private Queue<DropMercenaryResponseParameter> m_DropMercenaryCommands;
	private Queue<UsePropsResponseParameter> m_UsePropsCommands;
	
	private int m_CGStartTick;
	
	private static CGDirector s_Sigleton;
	
	public static CGDirector Instance 	
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
	
	public Age CurrentAge 
	{ 
		get
		{
			return this.m_CurrentAge;
		}
	}
	
	void Start()
	{
		Resources.UnloadUnusedAssets();
		AudioController.PlayMusic("BattleStart");
		this.m_DropCommands = new Queue<DropArmyResponseParameter>();
		this.m_DropMercenaryCommands = new Queue<DropMercenaryResponseParameter>();
		this.m_UsePropsCommands = new Queue<UsePropsResponseParameter>();
		this.m_Match = this.m_Reader.GetReplayData();
		this.ConstructScene(this.m_Match);
		
		this.m_TotalCGTick = this.m_Match.TotalTime;
		
		this.StartCoroutine("DelayCloudFadeOut");
	}
	
	public void StartCG()
	{
		this.m_CGStartTick = TimeTickRecorder.Instance.CurrentTimeTick;
		this.m_SceneDirector.SendRunAway();
		BattleSceneHelper.Instance.EnableBuildingAI();
		this.m_IsCGStart = true;
		
		Dictionary<ArmyType, int> armies = new Dictionary<ArmyType, int>();
		List<MercenaryType> mercenaries = new List<MercenaryType>();
		List<PropsType> props = new List<PropsType>();
		foreach (DropArmyResponseParameter army in this.m_DropCommands) 
		{
			if(!armies.ContainsKey(army.ArmyType))
			{
				armies.Add(army.ArmyType, army.Level);
			}
		}
		foreach (DropMercenaryResponseParameter mercenary in this.m_DropMercenaryCommands) 
		{
			mercenaries.Add(mercenary.MercenaryType);
		}
		foreach (UsePropsResponseParameter p in this.m_UsePropsCommands) 
		{
			props.Add(p.PropsType);
		}
		this.m_PreloadManager.Preload(armies, mercenaries, props);
	}
	
	void FixedUpdate()
	{
		if(this.m_IsCGStart)
		{
			int currentTick = TimeTickRecorder.Instance.CurrentTimeTick - this.m_CGStartTick;
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
			if(currentTick > this.m_TotalCGTick)
			{
				this.CGOver();
			}
		}
	}

    IEnumerator DelayCloudFadeOut()
    {
        yield return new WaitForSeconds(0.5f);
        m_Cloud.FadeOut();
		yield return new WaitForSeconds(3f);
		this.m_Guide.StartGuide();
    }

	public void ConstructScene(MatchLogResponseParameter param)
	{	
		BattleRandomer.Instance.SetSeed(param.RandomSeed);
		for(int i = 0; i < param.DropArmyCommands.Count; i ++)
		{
			DropArmyResponseParameter army = param.DropArmyCommands[i];
			this.m_DropCommands.Enqueue(army);
		}
		for(int i = 0; i < param.DropMercenaryCommands.Count; i ++)
		{
			DropMercenaryResponseParameter mercenary = param.DropMercenaryCommands[i];
			this.m_DropMercenaryCommands.Enqueue(mercenary);
		}
		for(int i = 0; i < param.UsePropsCommands.Count; i++)
		{
			UsePropsResponseParameter props = param.UsePropsCommands[i];
			this.m_UsePropsCommands.Enqueue(props);
		}
		BattleSceneHelper.Instance.ClearObject();
		this.m_SceneDirector.ClearAllActors();
		Resources.UnloadUnusedAssets();
		
		this.m_BuildingFactory.ConstructBuilding(param.RivalInformation);
		this.m_Map.SetMap(this.CurrentAge);
		BattleMapData.Instance.ConstructGridArray();
		this.m_SceneDirector.GenerateActors();
		BattleRecorder.Instance.ClearRecords();
	}
	
	public void CGOver()
	{
		if(!this.m_IsCGEnd)
		{
			this.m_IsCGStart = false;
			this.m_IsCGEnd = true;
	
			BattleSceneHelper.Instance.DestroyAllInvaders();
			GameObject.DestroyImmediate(this.m_BulletParent);
			this.EndCG();
		}
	}
	
	private void EndCG()
	{
		this.m_Cloud.FadeIn();
		this.StartCoroutine("Wait");
	}
	
	IEnumerator Wait()
	{
		yield return new WaitForSeconds(1.5f);
		Application.LoadLevel(ClientStringConstants.BUILDING_SCENE_LEVEL_NAME);
	}
}
