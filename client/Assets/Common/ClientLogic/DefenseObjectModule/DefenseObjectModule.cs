using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CommandConsts;
using ConfigUtilities;

public class DefenseObjectModule  
{
	private Dictionary<long, DefenseObjectLogicObject> m_DefenseObjects;
	private List<KeyValuePair<long, DefenseObjectLogicObject>> m_UnreceivedIDObjects;
	private long m_TempID;
	
	public DefenseObjectModule()
	{
		this.m_DefenseObjects = new Dictionary<long, DefenseObjectLogicObject>();
		this.m_UnreceivedIDObjects = new List<KeyValuePair<long, DefenseObjectLogicObject>>();
	}
	
	public void InitialDefenseObject(List<DefenseObjectData> objects)
	{
		foreach (DefenseObjectData o in objects) 
		{
			DefenseObjectLogicObject logicObject = new DefenseObjectLogicObject(o);
			this.m_DefenseObjects.Add(logicObject.Data.DefenseObjectID, logicObject);
		}
	}
	
	public List<DefenseObjectLogicData> AllDefenseObjects
	{
		get
		{
			List<DefenseObjectLogicData> result = new List<DefenseObjectLogicData>();
			foreach (KeyValuePair<long, DefenseObjectLogicObject> defenseObject in m_DefenseObjects) 
			{
				result.Add(defenseObject.Value.Data);
			}
			foreach (KeyValuePair<long, DefenseObjectLogicObject> defenseObject in m_UnreceivedIDObjects) 
			{
				result.Add(defenseObject.Value.Data);
			}
			return result;
		}
	}
	
	public DefenseObjectLogicObject GetDefenseLogicObject(long defenseObjectID)
	{
		if(defenseObjectID < 0)
		{
			foreach (KeyValuePair<long, DefenseObjectLogicObject> defenseObject in this.m_UnreceivedIDObjects) 
			{
				if(defenseObject.Key == defenseObjectID)
				{
					return defenseObject.Value;
				}
			}
			return null;
		}
		else
		{
			return this.m_DefenseObjects[defenseObjectID];
		}
	}
	
	public DefenseObjectLogicData AddDefenseObject(int propsNo, TilePosition position)
	{
		DefenseObjectData data = new DefenseObjectData();
		data.Position = position;
		data.DefenseObjectID = --this.m_TempID;
		data.Name = LogicController.Instance.GetProps(propsNo).Name;
		data.ConfigData = new DefenseObjectConfigWrapper(LogicController.Instance.GetProps(propsNo).FunctionConfigData);
		DefenseObjectLogicObject logicObject = new DefenseObjectLogicObject(data);
		CommunicationNotificationCenter.Instance.OnAddDefenseObjectResponse += ReceivedDefenseObjectResponse;
		
		this.m_UnreceivedIDObjects.Add(new KeyValuePair<long, DefenseObjectLogicObject>(data.DefenseObjectID, logicObject));
		
		AddDefenseObjectRequestParameter request = new AddDefenseObjectRequestParameter();
		request.PropsNo = propsNo;
		request.PositionRow = position.Row;
		request.PositionColumn = position.Column;
	
		CommunicationNotificationCenter.Instance.AddDefnseObject(request);
		
		return logicObject.Data;
	}
	
	public void MoveDefenseObject(long defenseObjectID, TilePosition position)
	{
		if(defenseObjectID > 0)
		{
			this.m_DefenseObjects[defenseObjectID].Move(position);
		}
		else
		{
			foreach (KeyValuePair<long, DefenseObjectLogicObject> defenseObject in this.m_UnreceivedIDObjects) 
			{
				if(defenseObject.Key == defenseObjectID)
				{
					defenseObject.Value.Move(position);
					break;
				}
			}
		}
	}
	
	public void DestroyDefenseObject(long defenseObjectID)
	{
		DestroyDefenseObjectRequestParameter request = new DestroyDefenseObjectRequestParameter();
		if(defenseObjectID > 0)
		{
			this.m_DefenseObjects.Remove(defenseObjectID);
			request.DefenseObjectID = defenseObjectID;
		}
		else
		{
			foreach (KeyValuePair<long, DefenseObjectLogicObject> defenseObject in this.m_UnreceivedIDObjects) 
			{
				if(defenseObject.Key == defenseObjectID)
				{
					this.m_UnreceivedIDObjects.Remove(defenseObject);
					request.PositionRow = defenseObject.Value.Data.Position.Row;
					request.PositionColumn = defenseObject.Value.Data.Position.Column;
					break;
				}
			}
		}
		CommunicationUtility.Instance.DestroyDefenseObject(request);
	}
	
	private void ReceivedDefenseObjectResponse(AddDefenseObjectResponseParameter param)
	{
		DefenseObjectLogicObject logicObject = this.m_UnreceivedIDObjects[0].Value;
		logicObject.Add(param.DefenseObjectID);
		this.m_DefenseObjects.Add(param.DefenseObjectID, logicObject);
		this.m_UnreceivedIDObjects.RemoveAt(0);
		
		CommunicationNotificationCenter.Instance.OnAddDefenseObjectResponse -= ReceivedDefenseObjectResponse;
	}
}
