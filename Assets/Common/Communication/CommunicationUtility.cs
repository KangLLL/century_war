using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommandConsts;
using ExitGames.Client.Photon;

public class CommunicationUtility : MonoBehaviour 
{
	[SerializeField]
	private CommunicationManager m_Manager;
	
	private Dictionary<byte, List<ReceiverInformation>> m_ReceiverDict;
	
	public bool IsConnectedToServer
	{
		get
		{
			return m_Manager.IsConnectedToServer;
		}
	}
	
	private static CommunicationUtility s_instance;
	public static CommunicationUtility Instance
	{
		get
		{
			return s_instance;
		}
	}
	
	void Awake()
	{
		s_instance = this;
		GameObject.DontDestroyOnLoad(gameObject);
	}
	
	void Start()
	{
		this.m_ReceiverDict = new Dictionary<byte, List<ReceiverInformation>>();
	}
	
	public void ConnectToServer()
	{
		this.m_Manager.ConnectToServer();
	}
	
	public void ConnectToServer(string serverIP)
	{
		this.m_Manager.ConnectToServer(serverIP);
	}
	
	public void DisconnectToServer()
	{
		this.m_Manager.DisconnectToServer();
	}
	
	public void RemoveInvalidReceiver()
	{
		List<byte> invalidReceiverList = new List<byte>();
		foreach(byte eventCode in this.m_ReceiverDict.Keys)
		{
			List<ReceiverInformation> receivers = this.m_ReceiverDict[eventCode];
			for(int i = receivers.Count - 1; i >= 0; i --)
			{
				ReceiverInformation receiver = receivers[i];
				if(receiver.Receiver == null)
				{
					receivers.Remove(receiver);
				}
			}
			
			if(receivers.Count == 0)
			{
				invalidReceiverList.Add(eventCode);
			}
		}
		
		foreach(byte invalidReceiver in invalidReceiverList)
		{
			this.m_ReceiverDict.Remove(invalidReceiver);
		}
	}
	
	public void GetVersion(Component receiver, string methodName, bool isListenOnce, VersionRequestParameter parameter)
	{
		this.CommunicateWithServer(receiver, methodName, isListenOnce, parameter.GetHashtableFromParameter(), ClientCommandConsts.GET_VERSION_COMMAND,
			ServerCommandConsts.VERSION_RESPONSE);
	}
	
	public void GetConfigTableMD5(Component receiver, string methodName, bool isListenOnce)
	{
		this.CommunicateWithServer(receiver, methodName, isListenOnce,null, ClientCommandConsts.CONFIG_TABLE_MD5_COMMAND, 
			ServerCommandConsts.CONFIG_TABLE_MD5_RESPONSE);
	}
	
	public void GetUserData(Component receiver, string methodName, bool isListenOnce, LoginRequestParameter parameter)
	{
		this.CommunicateWithServer(receiver, methodName, isListenOnce, parameter.GetHashtableFromParameter(), ClientCommandConsts.PLAYER_LOGIN_COMMAND,
			ServerCommandConsts.PLAYER_LOGIN_SUCCESS);
	}
	
	public void GetRivalData(Component receiver, string methodName, bool isListenOnce)
	{
		this.CommunicateWithServer(receiver, methodName, isListenOnce, null, ClientCommandConsts.FIND_RIVAL_COMMAND, ServerCommandConsts.FIND_RIVAL_RESPONSE);
	}
	
	public void SkipRival(SkipRivalRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.SKIP_RIVAL_COMMAND);
	}
	
	public void ConstructBuilding(ConstructBuildingRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(),ClientCommandConsts.BUILDING_BUILD_COMMAND);
	}
	
	public void UpgradeBuilding(UpgradeBuildingRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.BUILDING_UPGRADE_COMMAND);
	}
	
	public void MoveBuilding(MoveBuildingRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.BUILDING_MOVE_COMMAND);
	}
	
	public void CancelBuildingUpgrade(CancelBuildingRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.BUILDING_CANCEL_COMMAND);
	}
	
	public void FinishBuildingUpgrade(UpgradeBuildingSuccessRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.BUILDING_UPGRADE_SUCCESS_COMMAND);
	}
	
	public void GenerateRemovableObject(GenerateRemovableObjectRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.GENERATE_REMOVABLE_OBJECT_COMMAND);
	}
	
	public void RemoveObject(RemoveObjectRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.REMOVE_OBJECT_COMMAND);
	}
	
	public void CancelRemoveObject(CancelRemoveRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.CANCEL_REMOVE_COMMAND);
	}
	
	public void TimeUpRemoveOject(RemoveTimeUpRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.REMOVE_TIME_UP_COMMAND);
	}
	
	public void FinishRemoveObject(FinishRemoveObjectRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.FINISH_REMOVE_COMMAND);
	}
	
	public void TimeUpBuildingUpgrade(UpgradeBuildingTimeUpRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.BUILDING_UPGRADE_TIME_UP_COMMAND);
	}
	
	
	public void FinishBuildingUpgradeInstantly(UpgradeBuildingSuccessInstantlyRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.BUILDING_UPGRADE_SUCCESS_INSTANTLY_COMMAND);
	}
	
	public void ProduceArmy(ArmyProduceRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.ARMY_PRODUCE_COMMAND);
	}
	
	public void CancelArmyProduce(ArmyProduceCancelReqeustParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.ARMY_PRODUCE_CANCEL_COMMAND);
	}
	
	public void FinishArmyProduce(ArmyProduceSuccessRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.ARMY_PRODUCE_SUCCESS_COMMAND);
	}
	
	public void UpgradeArmy(ArmyUpgradeRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.ARMY_UPGRADE_COMMAND);
	}
	
	public void CancelArmyUpgrade(ArmyUpgradeCancelRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.ARMY_UPGRADE_CANCEL_COMMAND);
	}
	
	public void FinishArmyUpgrade(ArmyUpgradeSuccessRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.ARMY_UPGRADE_SUCCESS_COMMAND);
	}
	
	public void FinishArmyUpgradeInstantly(ArmyUpgradeFinishInstantlyRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.ARMY_UPGRADE_SUCCESS_INSTANTLY_COMMAND);
	}
	
	public void FinishArmyProduceInstantly(ArmyProduceFinishInstantlyRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.ARMY_PRODUCE_SUCCESS_INSTANTLY_COMMAND);
	}
	
	public void ProduceItem(ItemProduceRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(),ClientCommandConsts.ITEM_PRODUCE_COMMAND);
	}
	
	public void CancelProduceItem(ItemProduceCancelRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.ITEM_PRODUCE_CANCEL_COMMAND);
	}
	
	public void FinishProduceItem(ItemProduceSuccessRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.ITEM_PRODUCE_SUCCESS_COMMAND);
	}
	
	public void UpgradeItem(ItemUpgradeRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.ITEM_UPGRADE_CAMMOND);
	}
	
	public void CancelUpgradeItem(ItemUpgradeCancelRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.ITEM_UPGRADE_CANCEL_COMMAND);
	}
	
	public void FinishUpgradeItem(ItemUpgradeSuccessRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.ITEM_UPGRADE_SUCCESS_COMMAND);
	}
	
	public void CollectGold(CollectRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.COLLECT_GOLD_COMMAND);
	}
	
	public void CollectFood(CollectRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.COLLECT_FOOD_COMMAND);
	}
	
	public void CollectOil(CollectRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.COLLECT_OIL_COMMAND);
	}
	
	public void AccelerateResourceProduce(AccelerateRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.ACCELERATE_RESOURCE_BEGIN_COMMAND);
	}
	
	public void FinishAccelerateResourceProduce(AccelerateRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.ACCELERATE_RESOURCE_FINISH_COMMAND);
	}
	
	public void AccelerateArmyProduce(AccelerateRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.ACCELERATE_ARMY_BEGIN_COMMAND);
	}
	
	public void FinishAccelerateArmyProduce(AccelerateRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.ACCELERATE_ARMY_FINISH_COMMAND);
	}
	
	public void AccelerateItemProduce(AccelerateRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.ACCELERATE_ITEM_BEGIN_COMMAND);
	}
	
	public void FinishAccelerateItemProduce(AccelerateRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.ACCELERATE_ITEM_FINISH_COMMAND);
	}
	
	public void BuyGold(ShopRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.BUY_GOLD_COMMAND);
	}
	
	public void BuyFood(ShopRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.BUY_FOOD_COMMAND);
	}
	
	public void BuyOil(ShopRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.BUY_OIL_COMMAND);
	}
	
	public void BuyBuilderHut(BuyBuilderHutRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.BUY_BUILDER_HUT_COMMAND);
	}
	
	public void BuyBuilderHutUpgrade(BuyBuilderHutUpgradeRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.BUY_BUILDER_HUT_UPGRADE_COMMAND);
	}
	
	public void BuyWall(BuyWallRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.BUY_WALL_COMMAND);
	}
	
	public void BuyWallUpgrade(BuyWallUpgradeRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.BUY_WALL_UPGRADE_COMMAND);
	}
	
	public void BuyRemovableObject(BuyRemovableObjectRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.BUY_REMOVABLE_OBJECT_COMMAND);
	}
	
	public void StartMatch(StartMatchRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.START_MATCH_COMMAND);
	}
	
	public void DropArmy(DropArmyRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.DROP_ARMY_COMMAND);
	}
	
	public void PlunderResource(PlunderResourceRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.PLUNDER_RESOURCE_COMMAND);
	}
	
	public void DestroyBuilding(DestroyBuildingRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.DESTROY_BUILDING_COMMAND);
	}
	
	public void DestroyAchievementBuildingInBattle(DestroyAchievementBuildingInBattleRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.DESTROY_ACHIEVEMENT_BUILDING_IN_BATTLE_COMMAND);
	}
	
	public void TriggerDefenseObject(TriggerDefenseObjectRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.TRIGGER_DEFENSE_OBJECT_COMMAND);
	}
	
	public void FinishObserve()
	{
		this.CommunicateWithServer(null, ClientCommandConsts.FINISH_OBSERVE_COMMAND);
	}
	
	public void GetMatchID(FinishMatchRequestParameter parameter, Component receiver, string methodName, bool isListenOnce)
	{
		this.CommunicateWithServer(receiver, methodName, isListenOnce, parameter.GetHashtableFromParameter(), 
			ClientCommandConsts.FINISH_MATCH_COMMAND, ServerCommandConsts.ATTACK_LOG_RESPONSE);
	}
	
	public void GetReplayDetail(MatchLogRequestParameter parameter, Component receiver, string methodName, bool isListenOnce)
	{
		this.CommunicateWithServer(receiver, methodName, isListenOnce, parameter.GetHashtableFromParameter(),
			ClientCommandConsts.GET_MATCH_LOG_COMMAND, ServerCommandConsts.LOG_DETAIL_RESPONSE);
	}
	
	public void StartRevenge(StartRevengeRequestParameter parameter, Component receiver, string methodName, bool isListenOnce)
	{
		this.CommunicateWithServer(receiver, methodName, isListenOnce, parameter.GetHashtableFromParameter(),
			ClientCommandConsts.START_REVENGE_COMMAND, ServerCommandConsts.START_REVENGE_RESPONSE);
	}
	
	public void VisitFriend(VisitFriendRequestParameter parameter, Component receiver, string methodName, bool isListenOnce)
	{
		this.CommunicateWithServer(receiver, methodName, isListenOnce, parameter.GetHashtableFromParameter(),
			ClientCommandConsts.VISIT_FRIEND_COMMAND, ServerCommandConsts.FRIEND_DETAIL_RESPONSE);
	}
	
	public void GetRankData(Component receiver, string methodName, bool isListenOnce)
	{
		this.CommunicateWithServer(receiver, methodName, isListenOnce, null, 
			ClientCommandConsts.GET_RANK_COMMAND, ServerCommandConsts.RANK_DETAIL_RESPONSE);
	}
	
	public  void GetPurchaseID(Component receiver, string methodName, bool isListenOnce)
	{
		this.CommunicateWithServer(receiver,methodName,isListenOnce,null,
			ClientCommandConsts.GENERATE_PURCHASE_ID_COMMAND, ServerCommandConsts.GENERATE_PURCHASE_ID_RESPONSE);
	}
	
	public void ConfirmNdPurchase(ConfirmNdPurchaseRequestParameter parameter, Component receiver, string methodName, bool isListenOnce)
	{
		this.CommunicateWithServer(receiver, methodName, isListenOnce, parameter.GetHashtableFromParameter(),
			ClientCommandConsts.CONFIRM_ND_PURCHASE_COMMAND, ServerCommandConsts.CONFIRM_ND_PURCHASE_RESPONSE);
	}
	
	public void ConfirmApplePurchase(ConfirmApplePurchaseRequestParameter parameter, Component receiver, string methodName, bool isListenOnce)
	{
		this.CommunicateWithServer(receiver, methodName, isListenOnce, parameter.GetHashtableFromParameter(),
			ClientCommandConsts.CONFIRM_APPLE_PURCHASE_COMMAND, ServerCommandConsts.CONFIRM_APPLE_PURCHASE_RESPONSE);
	}
	
	public void SetDeviceToken(DeviceTokenRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.DEVICE_TOKEN_COMMAND);
	}
	
	public void MountAccount(MountAccountRequestParameter parameter, Component receiver, string methodName, bool isListenOnce)
	{
		this.CommunicateWithServer(receiver, methodName, isListenOnce, parameter.GetHashtableFromParameter(),
			ClientCommandConsts.MOUNT_ACCOUNT_COMMAND, ServerCommandConsts.MOUNT_ACCOUNT_RESPONSE);
	}
	
	public void ChangeName(ChangeNameRequestParameter parameter, Component receiver, string methodName, bool isListenOnce)
	{
		this.CommunicateWithServer(receiver, methodName, isListenOnce, parameter.GetHashtableFromParameter(),
			ClientCommandConsts.CHANGE_NAME_COMMAND, ServerCommandConsts.CHANGE_NAME_RESPONSE);
	}
	
	public void SwitchAccount(SwitchAccountRequestParameter parameter, Component receiver, string methodName, bool isListenOnce)
	{
		this.CommunicateWithServer(receiver, methodName, isListenOnce, parameter.GetHashtableFromParameter(),
			ClientCommandConsts.SWITCH_ACCOUNT_COMMAND, ServerCommandConsts.SWITCH_ACCOUNT_RESPONSE);
	}
	
	public void RegisterAccount(RegisterAccountRequestParameter parameter, Component receiver, string methodName, bool isListenOnce)
	{
		this.CommunicateWithServer(receiver, methodName, isListenOnce, parameter.GetHashtableFromParameter(),
			ClientCommandConsts.REGISTER_ACCOUNT_COMMAND, ServerCommandConsts.REGISTER_ACCOUNT_RESPONSE);
	}
	
	public void LoginAccount(LoginAccountRequestParameter parameter, Component receiver, string methodName, bool isListenOnce)
	{
		this.CommunicateWithServer(receiver, methodName, isListenOnce, parameter.GetHashtableFromParameter(),
			ClientCommandConsts.LOGIN_ACCOUNT_COMMAND, ServerCommandConsts.LOGIN_ACCOUNT_RESPONSE);
	}

	public void LogoutAccount()
	{
		this.CommunicateWithServer(null, ClientCommandConsts.LOGOUT_ACCOUNT_COMMAND);
	}
	
	public  void CompleteTask(CompleteTaskRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.COMPLETE_TASK_COMMAND);
	}
	
	public void AwardTask(AwardTaskRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.AWARD_TASK_COMMAND);
	}
	
	public void TimeOutTask(TimeOutTaskRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.TIME_OUT_TASK_COMMAND);
	}
	
	public void GetNewbieRival(Component receiver, string methodName, bool isListenOnce)
	{
		this.CommunicateWithServer(receiver, methodName, isListenOnce, null, 
			ClientCommandConsts.FIND_NEWBIE_RIVAL_COMMAND, ServerCommandConsts.FIND_RIVAL_RESPONSE);
	}
	
	public void CompleteNewbieGuide()
	{
		this.CommunicateWithServer(null, ClientCommandConsts.FINISH_NEWBIE_PROCESS);
	}
	
	public void HireMercenary(HireMercenaryRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(),ClientCommandConsts.HIRE_MERCENARY_COMMAND);
	}
	
	public void DropMercenary(DropMercenaryRequestParameter paramter)
	{
		this.CommunicateWithServer(paramter.GetHashtableFromParameter(),ClientCommandConsts.DROP_MERCENARY_COMMAND);
	}
	
	public void DestroyProps(DestroyPropsRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.DESTROY_PROPS_COMMAND);
	}
	
	public void AddDefenseObject(AddDefenseObjectRequestParameter parameter, Component receiver, string methodName, bool isLisenOnce)
	{
		this.CommunicateWithServer(receiver, methodName, isLisenOnce, parameter.GetHashtableFromParameter(), 
			ClientCommandConsts.ADD_DEFENSE_OBJECT_COMMAND, ServerCommandConsts.ADD_DEFENSE_OBJECT_RESPONSE);
	}
	
	public void AddDefenseObject(AddDefenseObjectRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.ADD_DEFENSE_OBJECT_COMMAND);
	}
	
	public void MoveDefenseObject(MoveDefenseObjectRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.MOVE_DEFENSE_OBJECT_COMMAND);
	}
	
	public void DestroyDefenseObject(DestroyDefenseObjectRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.DESTROY_DEFENSE_OBJECT_COMMAND);
	}
	
	public void AddPlayerBuff(AddPlayerBuffRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.ADD_PLAYER_BUFF_COMMAND);
	}
	
	public void AddPropsInBattle(OperateAttackPropsRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.ADD_PROPS_IN_BATTLE_COMMAND);
	}
	
	public void RemovePropsInBattle(OperateAttackPropsRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.REMOVE_PROPS_IN_BATTLE_COMMAND);
	}
	
	public void UsePropsInBattle(UsePropsInBattleRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.USE_PROPS_IN_BATTLE_COMMAND);
	}
	
	public void AddPlayerShield(AddPlayerShieldRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.ADD_PLAYER_SHIELD_COMMAND);
	}
	
	public void BuildAchievementBuilding(BuildAchievementBuildingRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.BUILD_ACHIEVEMENT_BUILDING_COMMAND);
	}
	
	public void MoveAchievementBuilding(MoveAchievementBuildingRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.MOVE_ACHIEVEMENT_BUILDING_COMMAND);
	}
	
	public void DestroyAchievementBuilding(DestroyAchievementBuildingRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.DESTROY_ACHIEVEMENT_BUILDING_COMMAND);
	}
	
	public void RepairAchievementBuilding(RepairAchievementBuildingRequestParameter parameter)
	{
		this.CommunicateWithServer(parameter.GetHashtableFromParameter(), ClientCommandConsts.REPAIR_ACHIEVEMENT_BUILDING_COMMAND);
	}
	
	public void VerifyTime(Component receiver, string methodName, bool isListenOnce)
	{
		this.CommunicateWithServer(receiver,methodName,isListenOnce,null,
			ClientCommandConsts.TIME_VERIFY_COMMAND, ServerCommandConsts.TIME_VERIFY_RESPONSE);
	}
	
	private void CommunicateWithServer(Component receiver, string methodName, bool isListenOnce,  Hashtable parameter, 
		byte clientCommandCode, byte serverEventCode)
	{
		this.m_Manager.Communicate(clientCommandCode, parameter);
		ReceiverInformation info = new ReceiverInformation() 
			{ Receiver = receiver, MethodName = methodName, IsListenOnce = isListenOnce };
		if(!this.m_ReceiverDict.ContainsKey(serverEventCode))
		{
			this.m_ReceiverDict.Add(serverEventCode, new List<ReceiverInformation>());
		}
		if(!this.m_ReceiverDict[serverEventCode].Contains(info))
		{
			this.m_ReceiverDict[serverEventCode].Add(info);
		}
	}
	
	private void CommunicateWithServer(Hashtable parameter, byte clientCommandCode)
	{
		this.m_Manager.Communicate(clientCommandCode, parameter);
	}
	
	public void EventReceiver(EventData data)
	{
		if(this.m_ReceiverDict.ContainsKey(data.Code))
		{
			List<ReceiverInformation> receivers = this.m_ReceiverDict[data.Code];
			for(int i = receivers.Count - 1; i >= 0; i --)
			{
				ReceiverInformation receiver = receivers[i];
				if(receiver.Receiver == null)
				{
					receivers.Remove(receiver);
				}
				else
				{
					if(data.Parameters == null || data.Parameters.Count == 0)
					{
						receiver.Receiver.SendMessage(receiver.MethodName, new Hashtable(), SendMessageOptions.DontRequireReceiver);
					}
					else
					{
						receiver.Receiver.SendMessage(receiver.MethodName, 
							(Hashtable)data.Parameters[CommunicationConsts.EXTERNAL_SURFACE_KEY], SendMessageOptions.DontRequireReceiver);
					}
					if(receiver.IsListenOnce)
					{
						receivers.Remove(receiver);
					}
				}
			}
		}
	}
}
