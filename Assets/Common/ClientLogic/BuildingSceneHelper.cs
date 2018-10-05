using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class BuildingSceneHelper : MonoBehaviour, ISceneHelper
{
	#region ISceneHelper implementation
	public List<IBuildingInfo> GetBuildings (BuildingType type)
	{
		List<IBuildingInfo> result = new List<IBuildingInfo>();
		result = LogicController.Instance.GetBuildings(type).ToOtherList<IBuildingInfo, BuildingLogicData>();
		return result;
	}

	public List<IBuildingInfo> GetBuildingsExceptTypes (HashSet<BuildingType> types)
	{
		List<IBuildingInfo> result = new List<IBuildingInfo>();
		result = LogicController.Instance.GetBuildingsExceptTypes(types).ToOtherList<IBuildingInfo, BuildingLogicData>();
		return result;
	}

	public List<IBuildingInfo> GetBuildingsOfTypes (HashSet<BuildingType> types)
	{
		List<IBuildingInfo> result = new List<IBuildingInfo>();
		result = LogicController.Instance.GetBuildingsForTypes(types).ToOtherList<IBuildingInfo, BuildingLogicData>();
		return result;
	}
	
	public List<IBuildingInfo> GetAllBuildings ()
	{
		List<IBuildingInfo> result = new List<IBuildingInfo>();
		result = LogicController.Instance.AllBuildings.ToOtherList<IBuildingInfo, BuildingLogicData>();
		return result;
	}
	#endregion
}
