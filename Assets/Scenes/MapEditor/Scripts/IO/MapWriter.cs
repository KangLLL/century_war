using UnityEngine;
using System.Collections;
using System.IO;
using ConfigUtilities.Enums;

public class MapWriter : MonoBehaviour 
{
	private static MapWriter s_Sigleton;
	
	[SerializeField]
	private string m_MapName;
	
	public static MapWriter Instance
	{
		get { return s_Sigleton; }
	}
	
	void Awake()
	{
		s_Sigleton = this;
	}
	
	public void SaveMap()
	{
		var buildings = GameObject.FindObjectsOfType(typeof(EditorBuildingBehavior));
		var objects = GameObject.FindObjectsOfType(typeof(EditorRemovableObjectBehavior));
		var achievementBuildings = GameObject.FindObjectsOfType(typeof(EditorAchievementBuildingBehavior));
		var defenseObjects = GameObject.FindObjectsOfType(typeof(EditorDefenseObjectBehavior));
		BuildingNoGenerator buildingGenerator = new BuildingNoGenerator();
		Hashtable map = new Hashtable();
		ArrayList buildingList = new ArrayList();
		foreach (var b in buildings)
		{
			EditorBuildingBehavior building = (EditorBuildingBehavior)b;
			Hashtable property = new Hashtable();
			property.Add(EditorConfigInterface.Instance.MapBuildingTypeKey, (int)building.BuildingType);
			property.Add(EditorConfigInterface.Instance.MapBuildingNoKey, buildingGenerator.GetBuildingNO(building.BuildingType));
			property.Add(EditorConfigInterface.Instance.MapBuildingLevelKey, building.Level);
			property.Add(EditorConfigInterface.Instance.MapBuildingRowKey, building.Position.Row);
			property.Add(EditorConfigInterface.Instance.MapBuildingColumnKey, building.Position.Column);
			
			buildingList.Add(property);
		}
		map.Add(EditorConfigInterface.Instance.MapBuildingKey, buildingList);
		ArrayList objectList = new ArrayList();
		foreach (var o in objects) 
		{
			EditorRemovableObjectBehavior removableObject = (EditorRemovableObjectBehavior)o;
			Hashtable property = new Hashtable();
			property.Add(EditorConfigInterface.Instance.MapRemovableObjectTypeKey, (int)removableObject.RemovableObjectType);
			property.Add(EditorConfigInterface.Instance.MapRemovableObjectRowKey, removableObject.Position.Row);
			property.Add(EditorConfigInterface.Instance.MapRemovableObjectColumnKey, removableObject.Position.Column);
			
			objectList.Add(property);
		}
		map.Add(EditorConfigInterface.Instance.MapRemovableObjectKey, objectList);
		ArrayList achievementBuildingList = new ArrayList();
		foreach (var a in achievementBuildings) 
		{
			EditorAchievementBuildingBehavior achievementBuilding = (EditorAchievementBuildingBehavior)a;
			Hashtable property = new Hashtable();
			property.Add(EditorConfigInterface.Instance.MapAchievementBuildingTypeKey, (int)achievementBuilding.AchievementBuildingType);
			property.Add(EditorConfigInterface.Instance.MapAchievementBuildingRowKey, achievementBuilding.Position.Row);
			property.Add(EditorConfigInterface.Instance.MapAchievementBuildingColumnKey, achievementBuilding.Position.Column);
			
			achievementBuildingList.Add(property);
		}
		map.Add(EditorConfigInterface.Instance.MapAchievementBuildingKey, achievementBuildingList);
		ArrayList defenseObjectList = new ArrayList();
		foreach (var d in defenseObjects) 
		{
			EditorDefenseObjectBehavior defenseObject = (EditorDefenseObjectBehavior)d;
			Hashtable property = new Hashtable();
			property.Add(EditorConfigInterface.Instance.MapDefenseObjectTypeKey, (int)defenseObject.PropsType);
			property.Add(EditorConfigInterface.Instance.MapDefenseObjectRowKey, defenseObject.Position.Row);
			property.Add(EditorConfigInterface.Instance.MapDefenseObjectColumnKey, defenseObject.Position.Column);
			
			defenseObjectList.Add(property);
		}
		map.Add(EditorConfigInterface.Instance.MapDefenseObjectKey, defenseObjectList);
		
		FileStream fs =  Application.platform == RuntimePlatform.OSXEditor ? 
			new FileStream(this.m_MapName + "."  + EditorConfigInterface.Instance.MapSuffix,FileMode.Create) :
			new FileStream(EditorConfigInterface.Instance.MapStorePath + "/" +
			this.m_MapName + "."  + EditorConfigInterface.Instance.MapSuffix, FileMode.Create);
		StreamWriter sw = new StreamWriter(fs);
		
		sw.Write(JSONHelper.jsonEncode(map));
		sw.Close();
		
		Debug.Log("write finish!");
	}
}
