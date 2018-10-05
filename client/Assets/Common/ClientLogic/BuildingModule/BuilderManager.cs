using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities;


public class BuilderManager
{
	private BuilderObject[] m_Builders;
	private int m_UnlockedBuilerNumber;
	
	private int m_BusyBuilderNumber;
	
	private BuilderObject[] Builders
	{
		get
		{
			if(this.m_Builders == null)
			{
				this.m_Builders = new BuilderObject[ConfigInterface.Instance.SystemConfig.MaxBuilderNumber];
			}
			return this.m_Builders;
		}
	}
	
	public BuilderManager()
	{
		this.m_BusyBuilderNumber = 0;
	}
	
	public void AddBuilder(int buildingNO)
	{
		this.Builders[buildingNO] = new BuilderObject(new BuilderInformation(buildingNO));
		this.m_UnlockedBuilerNumber ++ ;
	}
	
	public void AddBusyBuilder(int builderNO, IObstacleInfo targetInfo)
	{
		this.Builders[builderNO].Build(targetInfo);
		this.m_BusyBuilderNumber ++;
	}
	
	public void SendBuilder(int builderNO, IObstacleInfo targetInfo)
	{
		BuildingLogicData builderHutData = LogicController.Instance.GetBuildingObject(
			new BuildingIdentity(ConfigUtilities.Enums.BuildingType.BuilderHut, builderNO));
		int builderLevel = builderHutData.Level;
		
		BuildingSceneDirector.Instance.SendBuilderToBuild(builderNO, builderLevel, builderHutData.BuildingPosition,
			targetInfo, SceneManager.Instance);
		
		this.AddBusyBuilder(builderNO, targetInfo);
	}
	
	public void RecycleBuilder(int builderNO)
	{
		if(Application.loadedLevelName.Equals(ClientStringConstants.BUILDING_SCENE_LEVEL_NAME))
		{
			BuildingSceneDirector.Instance.SendBuilderReturn(builderNO);
		}
		this.Builders[builderNO].BuildOver();
		this.m_BusyBuilderNumber --;
	}
	
	public BuilderData[] AllBuilders
	{
		get
		{
			if(this.m_Builders == null)
			{
				return null;
			}
			BuilderData[] result = new BuilderData[ConfigInterface.Instance.SystemConfig.MaxBuilderNumber];
			for(int i = 0; i < ConfigInterface.Instance.SystemConfig.MaxBuilderNumber; i++)
			{
				result[i] = this.m_Builders[i] == null ? null : this.m_Builders[i].BuilderData;
			}
			return result;
		}
	}
	
	public int AvailableBuilderNumber { get { return this.m_UnlockedBuilerNumber; } }
	public int IdleBuilderNumber { get { return this.m_UnlockedBuilerNumber - this.m_BusyBuilderNumber; } }
	public int BusyBuilderNumber { get { return this.m_BusyBuilderNumber; } }
}
