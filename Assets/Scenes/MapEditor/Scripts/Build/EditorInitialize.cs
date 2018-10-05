using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ConfigUtilities;
using CommonUtilities;
using System.Data;
using CommandConsts;

public class EditorInitialize : MonoBehaviour 
{
	private const string CONFIG_PATH = "ConfigTable.zip";
	
	private bool m_IsLoadSceneFromDisk;
	
	void Awake()
	{
		FileStream fileStream = new FileStream(Application.persistentDataPath + "/" + CONFIG_PATH, FileMode.Open);
		/*
		BinaryWriter writer = new BinaryWriter(fileStream);
		writer.Write(this.m_wwwConfigTable.bytes);
		writer.Close();
		*/
		MemoryStream uncompressedStream = new MemoryStream();
		CompressionUtility.DecompressStream(fileStream, uncompressedStream);
		
		BinaryFormatter bft = new BinaryFormatter();
		DataResource.Resource = (DataSet)bft.Deserialize(uncompressedStream);
		
		fileStream.Close();
		uncompressedStream.Close();
	}
	
	// Use this for initialization
	void Start () 
	{
		//Debug.Log(ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(ConfigUtilities.Enums.BuildingType.CityHall, 1).Description);
		//Debug.Log(ConfigInterface.Instance.ArmyConfigHelper.GetArmyCapacityCost(ConfigUtilities.Enums.ArmyType.Berserker));
	}
	
	void Update()
	{
		if(!this.m_IsLoadSceneFromDisk)
		{
			Hashtable map = MapReader.Instance.LoadMap();
			
			if(map != null)
			{				
				FindRivalResponseParameter param = DataConvertor.ConvertJSONToParameter(map);
				foreach (var building in param.Buildings)
				{
					EditorFactory.Instance.ConstructBuilding(building.BuildingType, building.Level, 
						new TilePosition(building.PositionColumn, building.PositionRow));
				}
				foreach (var removableObject in param.Objects) 
				{
					EditorFactory.Instance.ConstructRemovableObject(removableObject.ObjectType,
						new TilePosition(removableObject.PositionColumn, removableObject.PositionRow));
				}
				foreach (var achievementBuilding in param.AchievementBuildings) 
				{
					EditorFactory.Instance.ConstructAchievementBuilding(achievementBuilding.AchievementBuildingType, 
						new TilePosition(achievementBuilding.PositionColumn, achievementBuilding.PositionRow));
				}
				foreach (var defenseObject in param.DefenseObjects) 
				{	
					EditorFactory.Instance.ConstructDefenseObject(defenseObject.PropsType,
						new TilePosition(defenseObject.PositionColumn, defenseObject.PositionRow));
				}
			}
			this.m_IsLoadSceneFromDisk = true;
		}
	}
}
