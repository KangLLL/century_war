using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities;
using ConfigUtilities.Enums;
using CommandConsts;
using System;

public class BattleRecorder : MonoBehaviour 
{
	private static BattleRecorder s_Sigleton;
	private SortedDictionary<ArmyType, List<RecordUserCommand<ArmyIdentity>>> m_DropArmyCommands;
	private SortedDictionary<MercenaryType, List<RecordUserCommand<MercenaryIdentity>>> m_DropMercenaryCommands;
	private SortedDictionary<int, List<RecordUserCommand<int>>> m_UsePropsCommands;
	
	private int m_GoldTrophy;
	private int m_FoodTrophy;
	private int m_OilTrophy;
	
	private int m_DestroyBuildingCount;
	private float m_DestroyPercentage;
	private bool m_IsDestroyCityHall;
	private bool m_IsDestroyPropsStorage;
	
	private List<PropsType> m_PropsTrophy;
	
	public static BattleRecorder Instance
	{
		get { return s_Sigleton; }
	}
	
	public int GoldTrophy
	{
		get
		{
			return this.m_GoldTrophy;
		}
	}
	
	public int FoodTrophy
	{
		get
		{
			return this.m_FoodTrophy;
		}
	}
	
	public int OilTrophy
	{
		get
		{
			return this.m_OilTrophy;
		}
	}
	
	public List<PropsType> PropsTrophy
	{
		get
		{
			return this.m_PropsTrophy;
		}
	}
	
	public int DestroyBuildingCount
	{
		get
		{
			return this.m_DestroyBuildingCount;
		}
	}
	
	public float DestroyBuildingPercentage
	{
		get
		{
			return this.m_DestroyPercentage;
		}
	}
	
	public bool IsDestroyCityHall
	{
		get
		{
			return this.m_IsDestroyCityHall;
		}
	}
	
	public bool IsDestroyPropsStorage
	{
		get
		{
			return this.m_IsDestroyPropsStorage;
		}
	}
	
	public int BattleStartTime
	{
		get; set; 
	}
	
	public Dictionary<ArmyType, List<RecordUserCommand<ArmyIdentity>>> DropArmies
	{
		get
		{
			Dictionary<ArmyType, List<RecordUserCommand<ArmyIdentity>>> result = new Dictionary<ArmyType, List<RecordUserCommand<ArmyIdentity>>>();
			foreach(ArmyType type in this.m_DropArmyCommands.Keys)
			{
				result.Add(type, new List<RecordUserCommand<ArmyIdentity>>(this.m_DropArmyCommands[type]));
			}
			return result;
		}
	}
	
	public Dictionary<MercenaryType, List<RecordUserCommand<MercenaryIdentity>>> DropMercenaries
	{
		get
		{
			Dictionary<MercenaryType, List<RecordUserCommand<MercenaryIdentity>>> result = new Dictionary<MercenaryType, List<RecordUserCommand<MercenaryIdentity>>>();
			foreach (MercenaryType type in m_DropMercenaryCommands.Keys) 
			{
				result.Add(type, new List<RecordUserCommand<MercenaryIdentity>>(this.m_DropMercenaryCommands[type]));
			}
			return result;
		}
	}
	
	public List<KeyValuePair<PropsType, List<RecordUserCommand<int>>>> UsePropsCommands
	{
		get
		{
			List<KeyValuePair<PropsType, List<RecordUserCommand<int>>>> result = new List<KeyValuePair<PropsType, List<RecordUserCommand<int>>>>();
			foreach (KeyValuePair<int, List<RecordUserCommand<int>>> props in this.m_UsePropsCommands) 
			{
				PropsType type = ((UsePropsCommand)props.Value[0].ConstructCommand).PropsType;
				result.Add(new KeyValuePair<PropsType, List<RecordUserCommand<int>>>(type, props.Value));
			}
			return result;
		}
	}
	
	public List<KeyValuePair<PropsType, int>> UseProps
	{
		get
		{
			List<KeyValuePair<PropsType, int>> result = new List<KeyValuePair<PropsType, int>>();
			foreach (int propsNo in this.m_UsePropsCommands.Keys) 
			{
				PropsType propsType = ((UsePropsCommand)this.m_UsePropsCommands[propsNo][0].ConstructCommand).PropsType;
				result.Add(new KeyValuePair<PropsType, int>(propsType, this.m_UsePropsCommands[propsNo].Count));
			}
			return result;
		}
	}
	
	public int DropArmyCount
	{
		get
		{
			int result = 0;
			foreach(ArmyType type in this.m_DropArmyCommands.Keys)
			{
				result += this.m_DropArmyCommands[type].Count;
			}
			return result;
		}
	}
	
	public int DropMercenaryCount
	{
		get
		{
			int result = 0;
			foreach(MercenaryType type in this.m_DropMercenaryCommands.Keys)
			{
				result += this.m_DropMercenaryCommands[type].Count;
			}
			return result;
		}
	}
	
	public int UsePropsCount
	{
		get
		{
			int result = 0;
			foreach (var props in m_UsePropsCommands.Values) 
			{
				result += props.Count;
			}
			return result;
		}
	}
	
	void Awake()
	{
		s_Sigleton = this;
	}
	
	void Start()
	{
		this.m_DropArmyCommands = new SortedDictionary<ArmyType, List<RecordUserCommand<ArmyIdentity>>>();
		this.m_DropMercenaryCommands = new SortedDictionary<MercenaryType, List<RecordUserCommand<MercenaryIdentity>>>();
		this.m_UsePropsCommands = new SortedDictionary<int, List<RecordUserCommand<int>>>();
		
		this.m_PropsTrophy = new List<PropsType>();
	}
	
	public void RecordUseProps(UsePropsCommand command)
	{
		if(BattleDirector.Instance == null || !BattleDirector.Instance.IsBattleFinished)
		{
			if(!this.m_UsePropsCommands.ContainsKey(command.Identity))
			{
				this.m_UsePropsCommands.Add(command.Identity, new List<RecordUserCommand<int>>());
			}
			RecordUserCommand<int> useCommand = new RecordUserCommand<int>()
			{ ConstructCommand = command, DroppedFrame = TimeTickRecorder.Instance.CurrentTimeTick - this.BattleStartTime };
			this.m_UsePropsCommands[command.Identity].Add(useCommand);
			
			if(BattleDirector.Instance != null)
			{
				UsePropsInBattleRequestParameter request = new UsePropsInBattleRequestParameter();
				request.PropsNo = command.Identity;
				request.PositionX = command.Position.x;
				request.PositionY = command.Position.y;
				request.OperateTime = TimeTickRecorder.Instance.CurrentTimeTick - this.BattleStartTime;
				CommunicationUtility.Instance.UsePropsInBattle(request);
				
				LogicController.Instance.UsePropsInBattle(command.Identity);
			}
		}
	}
	
	public void RecordDropArmy(ConstructArmyCommand command)
	{
		if(BattleDirector.Instance == null || !BattleDirector.Instance.IsBattleFinished)
		{
			if(!this.m_DropArmyCommands.ContainsKey(command.Identity.armyType))
			{
				this.m_DropArmyCommands.Add(command.Identity.armyType, new List<RecordUserCommand<ArmyIdentity>>());
			}
			RecordUserCommand<ArmyIdentity> dropCommand = new RecordUserCommand<ArmyIdentity>()
			{ ConstructCommand = command, DroppedFrame = TimeTickRecorder.Instance.CurrentTimeTick - this.BattleStartTime };
			this.m_DropArmyCommands[command.Identity.armyType].Add(dropCommand);
			
			if(BattleDirector.Instance != null)
			{
				DropArmyRequestParameter request = new DropArmyRequestParameter();
				request.ArmyType = command.Identity.armyType;
				request.ArmyNO = command.Identity.armyNO;
				request.Level = command.Level;
				request.OperateTime = TimeTickRecorder.Instance.CurrentTimeTick - BattleDirector.Instance.MatchStartTick;
				request.PositionX = command.Position.x;
				request.PositionY = command.Position.y;
				CommunicationUtility.Instance.DropArmy(request);
				
				LogicController.Instance.DropArmy(command.Identity);
			}
		}
	}
	
	public void RecordDropMercenary(UserCommand<MercenaryIdentity> command)
	{
		if(BattleDirector.Instance == null || !BattleDirector.Instance.IsBattleFinished)
		{
			if(!this.m_DropMercenaryCommands.ContainsKey(command.Identity.mercenaryType))
			{
				this.m_DropMercenaryCommands.Add(command.Identity.mercenaryType, new List<RecordUserCommand<MercenaryIdentity>>());
			}
			RecordUserCommand<MercenaryIdentity> dropCommand = new RecordUserCommand<MercenaryIdentity>()
			{ ConstructCommand = command, DroppedFrame = TimeTickRecorder.Instance.CurrentTimeTick - this.BattleStartTime };
			this.m_DropMercenaryCommands[command.Identity.mercenaryType].Add(dropCommand);
			
			if(BattleDirector.Instance != null)
			{
				DropMercenaryRequestParameter request = new DropMercenaryRequestParameter();
				request.MercenaryType = command.Identity.mercenaryType;
				request.MercenaryNO = command.Identity.mercenaryNO;
				request.PositionX = command.Position.x;
				request.PositionY = command.Position.y;
				request.OperateTime = TimeTickRecorder.Instance.CurrentTimeTick - BattleDirector.Instance.MatchStartTick;
				CommunicationUtility.Instance.DropMercenary(request);
				
				LogicController.Instance.DropMercenary(command.Identity);
			}
		}
	}
	
	public void RecordPlunderResource(int gold, int food, int oil, BuildingPropertyBehavior property)
	{
		this.m_GoldTrophy += gold;
		this.m_FoodTrophy += food;
		this.m_OilTrophy += oil;
		
		BattleSceneHelper.Instance.Plunder(gold, food, oil);
		
		if(BattleDirector.Instance != null)
		{
			LogicController.Instance.PlunderResource(gold, food, oil);
			
			PlunderResourceRequestParameter request = new PlunderResourceRequestParameter();
			request.OperateTime = TimeTickRecorder.Instance.CurrentTimeTick - BattleDirector.Instance.MatchStartTick;
			BuildingConfigData configData = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(property.BuildingType, property.Level);
			if(gold > 0)
			{
				request.PlunderGold = gold;
				if(configData.CanProduceGold)
				{
					request.BuildingType = property.BuildingType;
					request.BuildingNO = property.BuildingNO;
				}
			}
			if(food > 0)
			{
				request.PlunderFood = food;
				if(configData.CanProduceFood)
				{
					request.BuildingType = property.BuildingType;
					request.BuildingNO = property.BuildingNO;
				}
			}
			if(oil > 0)
			{
				request.PlunderOil = oil;
				if(configData.CanProduceOil)
				{
					request.BuildingType = property.BuildingType;
					request.BuildingNO = property.BuildingNO;
				}
			}
			CommunicationUtility.Instance.PlunderResource(request);
		}
	}
	
	public void RecordDestoryBuilding(BuildingType buildingType)
	{		
		if(buildingType != BuildingType.Wall)
		{
			this.m_DestroyBuildingCount ++;
		}
		this.m_DestroyPercentage = this.m_DestroyBuildingCount / (float)BattleSceneHelper.Instance.TotalSummaryBuildingCount;
		if(buildingType == BuildingType.CityHall)
		{
			this.m_IsDestroyCityHall = true;
		}
		if(buildingType == BuildingType.PropsStorage)
		{
			this.m_IsDestroyPropsStorage = true;
		}
		
		if(BattleDirector.Instance != null)
		{
			DestroyBuildingRequestParameter request = new DestroyBuildingRequestParameter();
			request.OperateTime = TimeTickRecorder.Instance.CurrentTimeTick - BattleDirector.Instance.MatchStartTick;
			request.BuildingType = buildingType;
			CommunicationUtility.Instance.DestroyBuilding(request);
			LogicController.Instance.DestroyBuilding(buildingType);
		}
	}
	
	public void RecordDestroyAchievementBuilding(int buildingNo, AchievementBuildingType type, bool isDropProps)
	{
		if(isDropProps)
		{
			this.m_PropsTrophy.Add(ConfigInterface.Instance.AchievementBuildingConfigHelper.GetAchievementBuildingData(type).NeedPropsType);
		}
		if(BattleDirector.Instance != null)
		{
			DestroyAchievementBuildingInBattleRequestParameter request = new DestroyAchievementBuildingInBattleRequestParameter();
			request.OperateTime = TimeTickRecorder.Instance.CurrentTimeTick - BattleDirector.Instance.MatchStartTick;
			request.AchievementBuildingNo = buildingNo;
			CommunicationUtility.Instance.DestroyAchievementBuildingInBattle(request);
		}
	}
	
	public void ClearRecords()
	{
		this.m_DestroyBuildingCount = 0;
		this.m_DestroyPercentage = 0;
		this.m_IsDestroyCityHall = false;
		this.m_IsDestroyPropsStorage = false;
	}
}
