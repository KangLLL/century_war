using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities;
using ConfigUtilities.Structs;

public class DefenseObjectConfigWrapper 
{
	public string PrefabName  { get;private set; }
	public List<TilePosition> BuildingObstacle { get;private set; }
	public int TriggerScope { get;private set; }
	public int Scope { get;private set; }
	
	public DefenseObjectConfigWrapper(object configData)
	{
		if(configData is PropsDefenseScopeConfigData)
		{
			PropsDefenseScopeConfigData data = (PropsDefenseScopeConfigData)configData;
			
			this.PrefabName = data.PrefabName;
			this.BuildingObstacle = new List<TilePosition>();
			foreach (TilePoint tp in data.BuildingObstacleList) 
			{	
				this.BuildingObstacle.Add(tp.ConvertToTilePosition());
			}
			this.TriggerScope = data.TriggerScope;
			this.Scope = data.Scope;
		}
		else if(configData is PropsDefenseScopeLastingConfigData)
		{
			PropsDefenseScopeLastingConfigData data = (PropsDefenseScopeLastingConfigData)configData;
			
			this.PrefabName = data.PrefabName;
			this.BuildingObstacle = new List<TilePosition>();
			foreach (TilePoint tp in data.BuildingObstacleList) 
			{
				this.BuildingObstacle.Add(tp.ConvertToTilePosition());
			}
			this.TriggerScope = data.TriggerScope;
			this.Scope = data.Scope;
		}
	}
}
