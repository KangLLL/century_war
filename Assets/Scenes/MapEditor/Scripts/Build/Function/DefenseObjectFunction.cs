using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities;
using ConfigUtilities.Enums;
using ConfigUtilities.Structs;

public class DefenseObjectFunction :  EditorCommonBehavior<PropsType, PropsConfigData>
{
	protected override void Construct (PropsType type, TilePosition position)
	{
		EditorFactory.Instance.ConstructDefenseObject(type, position);
	}
	
	public override void Delete ()
	{
		EditorFactory.Instance.DestroyDefenseObject(this.Position, this.ConfigData);
	}
	
	protected override List<TilePosition> GetBuildingObstacleInfo (PropsType type)
	{
		List<TilePosition> result = new List<TilePosition>();
		
		PropsDefenseScopeConfigData scopeConfigData = ConfigInterface.Instance.PropsConfigHelper.GetPropsData(type).FunctionConfigData as PropsDefenseScopeConfigData;
		PropsDefenseScopeLastingConfigData lastingConfigData = ConfigInterface.Instance.PropsConfigHelper.GetPropsData(type).FunctionConfigData as PropsDefenseScopeLastingConfigData;
		
		List<TilePoint> buildingObstacleList = scopeConfigData != null ? scopeConfigData.BuildingObstacleList : lastingConfigData.BuildingObstacleList;
		foreach (TilePoint tp in buildingObstacleList) 
		{
			result.Add(tp.ConvertToTilePosition());
		}
		return result;
	}
	
	protected override PropsConfigData GetConfigData ()
	{
		return ConfigInterface.Instance.PropsConfigHelper.GetPropsData(this.ObjectType);
	}
	
	protected override int GetIndexFromType (PropsType type)
	{
		return (int)type;
	}
	
	protected override PropsType GetTypeFromIndex (int index)
	{
		return (PropsType)index;
	}
	
	protected override PropsType StartType 
	{
		get 
		{
			return PropsType.HoneycombA;
		}
	}
	
	protected override bool IsValidType (PropsType type)
	{
		return ConfigInterface.Instance.PropsConfigHelper.GetPropsData(type).FunctionConfigData is PropsDefenseScopeConfigData ||
			ConfigInterface.Instance.PropsConfigHelper.GetPropsData(type).FunctionConfigData is PropsDefenseScopeLastingConfigData;
	}
}
