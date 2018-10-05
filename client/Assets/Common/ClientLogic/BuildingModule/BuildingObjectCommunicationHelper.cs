using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using CommandConsts;


public class BuildingObjectCommunicationHelper  
{
	#region Upgrade Building
	public void SendConstructBuildingRequest(BuildingIdentity id, TilePosition position, int builderNO)
	{
		ConstructBuildingRequestParameter request = new ConstructBuildingRequestParameter();
		request.BuilderBuildingNO = builderNO;
		request.BuildingType = id.buildingType;
		request.BuildingNO = id.buildingNO;
		request.PositionColumn = (byte)position.Column;
		request.PositionRow = (byte)position.Row;
		request.OperateTick = LogicTimer.Instance.GetServerTick();
		CommunicationUtility.Instance.ConstructBuilding(request);
	}

	public void SendUpgradeBuildingRequest(BuildingIdentity id, int builderBuildingNO)
	{
		UpgradeBuildingRequestParameter request = new UpgradeBuildingRequestParameter();
		request.BuildingType = id.buildingType;
		request.BuildingNO = id.buildingNO;
		request.BuilderBuildingNO = builderBuildingNO;
		request.OperateTick = LogicTimer.Instance.GetServerTick();
		CommunicationUtility.Instance.UpgradeBuilding(request);
	}
	
	public void SendCancelUpgradeBuildingRequest(BuildingIdentity id)
	{
		CancelBuildingRequestParameter request = new CancelBuildingRequestParameter();
		request.BuildingType = id.buildingType;
		request.BuildingNO = id.buildingNO;
		request.OperateTick = LogicTimer.Instance.GetServerTick();
		CommunicationUtility.Instance.CancelBuildingUpgrade(request);
	}
	
	public void SendUpgradeFinishRequest(BuildingIdentity id)
	{
		UpgradeBuildingSuccessRequestParameter request = new UpgradeBuildingSuccessRequestParameter();
		request.BuildingType = id.buildingType;
		request.BuildingNO = id.buildingNO;
		request.OperateTick = LogicTimer.Instance.GetServerTick();
		CommunicationUtility.Instance.FinishBuildingUpgrade(request);
	}
	
	public void SendTimeUpUpgradeBuildingRequest(BuildingIdentity id, float remainingSecond)
	{
		UpgradeBuildingTimeUpRequestParameter request = new UpgradeBuildingTimeUpRequestParameter();
		request.BuildingID = new BuildingIDParameter();
		request.BuildingID.BuildingType = id.buildingType;
		request.BuildingID.BuildingNO = id.buildingNO;
		request.OperateTick = LogicTimer.Instance.GetServerTick(remainingSecond);
		CommunicationUtility.Instance.TimeUpBuildingUpgrade(request);
	}
	
	public void SendMoveBuildingRequest(BuildingIdentity id, TilePosition newPosition)
	{
		MoveBuildingRequestParameter request = new MoveBuildingRequestParameter();
		request.BuildingType = id.buildingType;
		request.BuildingNO = id.buildingNO;
		request.DestinationPositionRow = (byte)newPosition.Row;
		request.DestinationPositionColumn = (byte)newPosition.Column;
		CommunicationUtility.Instance.MoveBuilding(request);
	}
	
	public void SendUpgradeBuildingInstantlyRequest(BuildingIdentity id, int costGem)
	{
		UpgradeBuildingSuccessInstantlyRequestParameter request = new UpgradeBuildingSuccessInstantlyRequestParameter();
		request.BuildingID = new BuildingIDParameter();
		request.BuildingID.BuildingType = id.buildingType;
		request.BuildingID.BuildingNO = id.buildingNO;
		request.CostGem = costGem;
		request.OperateTick = LogicTimer.Instance.GetServerTick();
		CommunicationUtility.Instance.FinishBuildingUpgradeInstantly(request);
	}
	#endregion
	
	
	#region Collect
	public void SendCollectGoldRequest(BuildingIdentity buildingID, int quantity, int total, long collectTick)
	{
		CollectRequestParameter request = new CollectRequestParameter();
		request.BuildingType = buildingID.buildingType;
		request.BuildingNO = buildingID.buildingNO;
		request.Quantity = quantity;
		//request.Total = total;
		request.OperateTick = collectTick;
		CommunicationUtility.Instance.CollectGold(request);
	}
	
	public void SendCollectFoodRequest(BuildingIdentity buildingID, int quantity, int total, long collectTick)
	{
		CollectRequestParameter request = new CollectRequestParameter();
		request.BuildingType = buildingID.buildingType;
		request.BuildingNO = buildingID.buildingNO;
		request.Quantity = quantity;
		//request.Total = total;
		request.OperateTick = collectTick;
		CommunicationUtility.Instance.CollectFood(request);
	}
	
	public void SendCollectOilRequest(BuildingIdentity buildingID, int quantity, int total, long collectTick)
	{
		CollectRequestParameter request = new CollectRequestParameter();
		request.BuildingType = buildingID.buildingType;
		request.BuildingNO = buildingID.buildingNO;
		request.Quantity = quantity;
		//request.Total = total;
		request.OperateTick = collectTick;
		CommunicationUtility.Instance.CollectOil(request);
	}
	#endregion
	
	#region Produce Army
	public void SendProduceArmyRequest(BuildingIdentity factoryID, ArmyIdentity armyID, int order)
	{
		ArmyProduceRequestParameter request = new ArmyProduceRequestParameter();
		request.FactoryBuildingType = factoryID.buildingType;
		request.FactoryBuildingNO = factoryID.buildingNO;
		request.ArmyType = armyID.armyType;
		request.ArmyNO = armyID.armyNO;
		request.ProduceOrder = order;
		request.OperateTick = LogicTimer.Instance.GetServerTick();
		CommunicationUtility.Instance.ProduceArmy(request);
	}

	public void SendCancelProduceArmyRequest(ArmyIdentity armyID)
	{
		ArmyProduceCancelReqeustParameter request = new ArmyProduceCancelReqeustParameter();
		request.ArmyType = armyID.armyType;
		request.ArmyNO = armyID.armyNO;
		request.OperateTick = LogicTimer.Instance.GetServerTick();
		CommunicationUtility.Instance.CancelArmyProduce(request);
	}
	
	public void SendFinishProduceArmyRequest(ArmyIdentity armyID,BuildingIdentity campID, float remainingSecond)
	{
		ArmyProduceSuccessRequestParameter request = new ArmyProduceSuccessRequestParameter();
		request.ArmyType = armyID.armyType;
		request.ArmyNO = armyID.armyNO;
		request.OwnerBuildingType = campID.buildingType;
		request.OwnerBuildingNO = campID.buildingNO;
		request.OperateTick = LogicTimer.Instance.GetServerTick(remainingSecond);
		CommunicationUtility.Instance.FinishArmyProduce(request);
	}
	
	public void SendFinishProduceArmyInstantlyRequest(List<ArmyIdentity> armies, List<BuildingIdentity> destinations, int gem)
	{
		ArmyProduceFinishInstantlyRequestParameter parameter = new ArmyProduceFinishInstantlyRequestParameter();
		parameter.Destinations = new List<ArmyProduceSuccessRequestParameter>();
		for(int i = 0; i < armies.Count; i ++)
		{
			ArmyIdentity armyID = armies[i];
			BuildingIdentity campID = destinations[i];
			ArmyProduceSuccessRequestParameter param = new ArmyProduceSuccessRequestParameter();
			param.OwnerBuildingType = campID.buildingType;
			param.OwnerBuildingNO = campID.buildingNO;
			param.ArmyType = armyID.armyType;
			param.ArmyNO = armyID.armyNO;
			parameter.Destinations.Add(param);
		}
		parameter.GemCost = gem;
		parameter.OperateTick = LogicTimer.Instance.GetServerTick();
		CommunicationUtility.Instance.FinishArmyProduceInstantly(parameter);
	}
	
	public void SendFinishUpgradeArmyInstantlyRequest(ArmyType armyType, int gem)
	{
		ArmyUpgradeFinishInstantlyRequestParameter parameter = new ArmyUpgradeFinishInstantlyRequestParameter();
		parameter.ArmyType = armyType;
		parameter.CostGem = gem;
		parameter.OperateTick = LogicTimer.Instance.GetServerTick();
		CommunicationUtility.Instance.FinishArmyUpgradeInstantly(parameter);
	}
	#endregion
	
	#region Upgrade Army
	public void SendUpgradeArmyRequest(ArmyType type, BuildingIdentity laboratoryID)
	{
		ArmyUpgradeRequestParameter request = new ArmyUpgradeRequestParameter();
		request.ArmyType = type;
		request.LaboratoryBuildingType = laboratoryID.buildingType;
		request.LaboratoryBuildingNO = laboratoryID.buildingNO;
		request.OperateTick = LogicTimer.Instance.GetServerTick();
		CommunicationUtility.Instance.UpgradeArmy(request);
	}
	
	public void SendCancelUpgradeArmyRequest(ArmyType type)
	{
		ArmyUpgradeCancelRequestParameter request = new ArmyUpgradeCancelRequestParameter();
		request.ArmyType = type;
		CommunicationUtility.Instance.CancelArmyUpgrade(request);
	}
	
	public void SendFinishUpgradeArmyRequest(ArmyType type, float remainingSecond)
	{
		ArmyUpgradeSuccessRequestParameter request = new ArmyUpgradeSuccessRequestParameter();
		request.ArmyType = type;
		request.OperateTick = LogicTimer.Instance.GetServerTick(remainingSecond);
		CommunicationUtility.Instance.FinishArmyUpgrade(request);
	}
	#endregion
	
	#region Produce Item
	public void SendProduceItemRequest(BuildingIdentity factoryID, ItemIdentity itemID, int order)
	{
		ItemProduceRequestParameter request = new ItemProduceRequestParameter();
		request.FactoryBuildingType = factoryID.buildingType;
		request.FactoryBuildingNO = factoryID.buildingNO;
		request.ItemType = itemID.itemType;
		request.ItemNO = itemID.itemNO;
		request.ProduceOrder = order;
		CommunicationUtility.Instance.ProduceItem(request);
	}
	
	public void SendCancelProduceItemRequest(ItemIdentity itemID)
	{
		ItemProduceCancelRequestParameter request = new ItemProduceCancelRequestParameter();
		request.ItemType = itemID.itemType;
		request.ItemNO = itemID.itemNO;
		CommunicationUtility.Instance.CancelProduceItem(request);
	}
	
	public void SendFinishProduceItemRequest(ItemIdentity itemID, BuildingIdentity campID)
	{
		ItemProduceSuccessRequestParameter request = new ItemProduceSuccessRequestParameter();
		request.ItemType = itemID.itemType;
		request.ItemNO = itemID.itemNO;
		request.OwnerBuildingType = campID.buildingType;
		request.OwnerBuildingNO = campID.buildingNO;
		CommunicationUtility.Instance.FinishProduceItem(request);
	}
	#endregion
	
	#region Upgrade Item
	public void SendUpgradeItemRequest(ItemType type, BuildingIdentity laboratoryID)
	{
		ItemUpgradeRequestParameter request = new ItemUpgradeRequestParameter();
		request.ItemType = type;
		request.LaboratoryBuildingType = laboratoryID.buildingType;
		request.LaboratoryBuildingNO = laboratoryID.buildingNO;
		CommunicationUtility.Instance.UpgradeItem(request);
	}
	
	public void SendCancelUpgradeItemRequest(ItemType type)
	{
		ItemUpgradeCancelRequestParameter request = new ItemUpgradeCancelRequestParameter();
		request.ItemType = type;
		CommunicationUtility.Instance.CancelUpgradeItem(request);
	}
	
	public void SendFinishUpgradeItemRequest(ItemType type)
	{
		ItemUpgradeSuccessRequestParameter reqeust = new ItemUpgradeSuccessRequestParameter();
		reqeust.ItemType = type;
		CommunicationUtility.Instance.FinishUpgradeItem(reqeust);
	}
	#endregion
	
	#region Accelerate
	public void SendAccelerateRequest(BuildingIdentity buildingID, AccelerateType type)
	{
		AccelerateRequestParameter request = new AccelerateRequestParameter();
		request.BuildingType = buildingID.buildingType;
		request.BuildingNO = buildingID.buildingNO;
		request.OperateTick = LogicTimer.Instance.GetServerTick();
		if(type == AccelerateType.Resource)
		{
			CommunicationUtility.Instance.AccelerateResourceProduce(request);
		}
		else if(type == AccelerateType.Army)
		{
			CommunicationUtility.Instance.AccelerateArmyProduce(request);
		}
		else
		{
			CommunicationUtility.Instance.AccelerateItemProduce(request);
		}
	}
	
	public void SendFinishAccelerateRequest(BuildingIdentity buildingID, AccelerateType type, float remainingSecond)
	{
		AccelerateRequestParameter request = new AccelerateRequestParameter();
		request.BuildingType = buildingID.buildingType;
		request.BuildingNO = buildingID.buildingNO;
		request.OperateTick = LogicTimer.Instance.GetServerTick(remainingSecond);
		if(type == AccelerateType.Resource)
		{
			CommunicationUtility.Instance.FinishAccelerateResourceProduce(request);
		}
		else if(type == AccelerateType.Army)
		{
			CommunicationUtility.Instance.FinishAccelerateArmyProduce(request);
		}
		else
		{
			CommunicationUtility.Instance.FinishAccelerateItemProduce(request);
		}
	}
	
	#endregion
}
