using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public interface ISceneHelper  
{
	List<IBuildingInfo> GetBuildings(BuildingType type);
	List<IBuildingInfo> GetBuildingsExceptTypes(HashSet<BuildingType> types);
	List<IBuildingInfo> GetBuildingsOfTypes(HashSet<BuildingType> types);
	List<IBuildingInfo> GetAllBuildings();
}
