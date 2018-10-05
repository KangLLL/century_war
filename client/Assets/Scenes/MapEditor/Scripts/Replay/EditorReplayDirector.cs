using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using CommandConsts;

public class EditorReplayDirector : MonoBehaviour 
{
	[SerializeField]
	private ObstacleFactory m_BuildingFactory;
	[SerializeField]
	private CharacterFactory m_CharacterFactory;
	[SerializeField]
	private BattleSceneDirector m_SceneDirector;
	
	[SerializeField]
	private GameObject m_BulletParent;
	[SerializeField]
	private LoadAgeMap m_Map;
	[SerializeField]
	private ReplayDataReader m_Reader;
	[SerializeField]
	private Age m_CurrentAge;
	[SerializeField]
	private BattlePreloadManager m_PreloadManager;
	
	private int m_TotalReplayTick;
	private bool m_IsReplayStart;
	
	private MatchLogResponseParameter m_Match;
	
	private Queue<DropArmyResponseParameter> m_DropCommands;
	private Queue<DropMercenaryResponseParameter> m_DropMercenaryCommands;
	private Queue<UsePropsResponseParameter> m_UsePropsCommands;
	
	private int m_ReplayStartTick;
	
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
		
		Dictionary<ArmyType, int> armies = new Dictionary<ArmyType, int>();
		List<MercenaryType> mercenaries = new List<MercenaryType>();
		List<PropsType> props = new List<PropsType>();
		foreach (DropArmyResponseParameter army in this.m_Match.DropArmyCommands) 
		{
			if(!armies.ContainsKey(army.ArmyType))
			{
				armies.Add(army.ArmyType, army.Level);
			}
		}
		foreach (DropMercenaryResponseParameter mercenary in this.m_Match.DropMercenaryCommands) 
		{
			mercenaries.Add(mercenary.MercenaryType);
		}
		foreach (UsePropsResponseParameter p in this.m_Match.UsePropsCommands) 
		{
			props.Add(p.PropsType);
		}
		this.m_PreloadManager.Preload(armies, mercenaries, props);
		
		this.ConstructScene(this.m_Match);
		
		this.m_TotalReplayTick = this.m_Match.TotalTime;
		
		this.StartReplay();
	}
	
	private void StartReplay()
	{
		this.m_ReplayStartTick = TimeTickRecorder.Instance.CurrentTimeTick;
		this.m_SceneDirector.SendRunAway();
		BattleSceneHelper.Instance.EnableBuildingAI();
		this.m_IsReplayStart = true;
	}
	
	void FixedUpdate()
	{
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
					UsePropsResponseParameter  props = this.m_UsePropsCommands.Dequeue();
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
		for(int i = 0; i < param.UsePropsCommands.Count; i ++)
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
	
	private void ReplayOver()
	{
		this.m_IsReplayStart = false;

		BattleSceneHelper.Instance.DestroyAllInvaders();
		GameObject.DestroyImmediate(this.m_BulletParent);
		this.EndReplay();
	}
	
	private void EndReplay()
	{
		Application.LoadLevel("MapEditorBuild");
	}
}
