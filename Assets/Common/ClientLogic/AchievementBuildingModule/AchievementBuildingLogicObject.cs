using UnityEngine;
using System.Collections;
using ConfigUtilities;
using CommandConsts;

public class AchievementBuildingLogicObject : LogicObject
{
	private AchievementBuildingData m_Data;
	private AchievementBuildingLogicData m_LogicData;
	
	public AchievementBuildingLogicObject(AchievementBuildingData data)
	{
		this.m_Data = data;
		this.m_LogicData = new AchievementBuildingLogicData(data);
	}
	
	public AchievementBuildingLogicData Data { get { return this.m_LogicData; } }
	
	public void Move(TilePosition newPosition)
	{
		this.m_Data.BuildingPosition = newPosition;
		
		MoveAchievementBuildingRequestParameter request = new MoveAchievementBuildingRequestParameter();
		request.AchievementBuildingNo = this.m_Data.BuildingNo;
		request.PositionRow = this.m_Data.BuildingPosition.Row;
		request.PositionColumn = this.m_Data.BuildingPosition.Column;
		CommunicationUtility.Instance.MoveAchievementBuilding(request);
	}
	
	public void Repair(int recoverLife)
	{
		this.m_Data.Life += recoverLife;
	}
}
