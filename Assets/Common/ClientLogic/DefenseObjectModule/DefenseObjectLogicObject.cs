using UnityEngine;
using System.Collections;
using CommandConsts;

public class DefenseObjectLogicObject  
{
	private DefenseObjectData m_Data;
	private DefenseObjectLogicData m_LogicData;
	
	public DefenseObjectLogicObject(DefenseObjectData data)
	{
		this.m_Data = data;
		this.m_LogicData = new DefenseObjectLogicData(data);
	}
	
	public DefenseObjectLogicData Data
	{
		get { return this.m_LogicData; }
	}
	
	public void Add(long defenseObjectID)
	{
		this.m_Data.DefenseObjectID = defenseObjectID;
	}
	
	public void Move(TilePosition newPosition)
	{
		MoveDefenseObjectRequestParameter request = new MoveDefenseObjectRequestParameter();
		request.NewPositionRow = newPosition.Row;
		request.NewPositionColumn = newPosition.Column;
		if(this.m_Data.DefenseObjectID > 0)
		{
			request.DefenseObjectID = this.m_Data.DefenseObjectID;
		}
		else
		{
			request.OldPositionRow = this.m_Data.Position.Row;
			request.OldPositionColumn = this.m_Data.Position.Column;
		}
		this.m_Data.Position = newPosition;
		CommunicationUtility.Instance.MoveDefenseObject(request);
	}
}
