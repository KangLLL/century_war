using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;
using ConfigUtilities.Structs;
using ConfigUtilities;
using System.Collections.Generic;

public class RemovableObjectFunction : EditorCommonBehavior<RemovableObjectType, RemovableObjectConfigData>
{
	protected override void Construct (RemovableObjectType type, TilePosition position)
	{
		EditorFactory.Instance.ConstructRemovableObject(type, position);
	}
	
	public override void Delete ()
	{
		EditorFactory.Instance.DestroyRemovableObject(this.Position, this.ConfigData);
	}
	
	protected override RemovableObjectConfigData GetConfigData ()
	{
		return ConfigInterface.Instance.RemovableConfigHelper.GetRemovableObjectData(this.ObjectType);
	}
	
	protected override List<TilePosition> GetBuildingObstacleInfo (RemovableObjectType type)
	{
		List<TilePosition> result = new List<TilePosition>();
		foreach (TilePoint point in ConfigInterface.Instance.RemovableConfigHelper.GetRemovableObjectData(type).BuildingObstacleList)
		{
			result.Add(point.ConvertToTilePosition());
		}
		return result;
	}
	
	protected override int GetIndexFromType (RemovableObjectType type)
	{
		return (int)type;
	}
	
	protected override RemovableObjectType GetTypeFromIndex (int index)
	{
		return (RemovableObjectType)index;
	}
	
	protected override RemovableObjectType StartType 
	{
		get 
		{
			return RemovableObjectType.SmallStone;
		}
	}
}
