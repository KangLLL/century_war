using UnityEngine;
using System.Collections;

using CommandConsts;
using CommonUtilities;
public class ShopModule  
{
	public void BuyGold(int gold)
	{
		ShopRequestParameter request = new ShopRequestParameter() { BuyResourceQuantity = gold };
		CommunicationUtility.Instance.BuyGold(request);
	}
	
	public void BuyFood(int food)
	{
		ShopRequestParameter request = new ShopRequestParameter() { BuyResourceQuantity = food };
		CommunicationUtility.Instance.BuyFood(request);
	}
	
	public void BuyOil(int oil)
	{
		ShopRequestParameter request = new ShopRequestParameter() { BuyResourceQuantity = oil };
		CommunicationUtility.Instance.BuyOil(request);
	}
	
	public void BuyBuilderHut(int builderNO, TilePosition position)
	{
		BuyBuilderHutRequestParameter request = new BuyBuilderHutRequestParameter();
		request.BuilderNO = builderNO;
		request.PositionRow = (byte)position.Row;
		request.PositionColumn = (byte)position.Column;
		CommunicationUtility.Instance.BuyBuilderHut(request);
	}
	
	public void BuyBuilderHutUpgrade(int builderNO)
	{
		BuyBuilderHutUpgradeRequestParameter request = new BuyBuilderHutUpgradeRequestParameter();
		request.BuilderNO = builderNO;
		request.OperateTick = LogicTimer.Instance.GetServerTick();
		CommunicationUtility.Instance.BuyBuilderHutUpgrade(request);
	}
	
	public void BuyWall(int wallNO, TilePosition position)
	{
		BuyWallRequestParameter request = new BuyWallRequestParameter();
		request.WallNO = wallNO;
		request.PositionRow = (byte)position.Row;
		request.PositionColumn = (byte)position.Column;
		CommunicationUtility.Instance.BuyWall(request);
	}
	
	public void BuyWallUpgrade(int wallNO)
	{
		BuyWallUpgradeRequestParameter request = new BuyWallUpgradeRequestParameter();
		request.WallNO = wallNO;
		CommunicationUtility.Instance.BuyWallUpgrade(request);
	}
}
