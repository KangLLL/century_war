using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Structs;
using ConfigUtilities.Enums;

public static class CommonExtesion 
{
	private const string ARMY_SORT_STRING_PREFIX = "0";
	private const string MERCENARY_SORT_STRING_PREFIX = "1";
	private const string PROPS_SORT_STRING_PREFIX = "2";
	
	public static TilePosition ConvertToTilePosition(this AStarPathNode node)
	{
		return new TilePosition(node.Column, node.Row);
	}
	
	public static AStarPathNode ConvertToAStarPathNode(this TilePosition tile)
	{
		AStarPathNode result = new AStarPathNode();
		result.Column = tile.Column;
		result.Row = tile.Row;
		return result;
	}
	
	public static bool IsEdgeBuildingTilePosition(this TilePosition position)
	{
		if(!position.IsValidBuildingTilePosition())
		{
			return false;
		}
		return (position.Column < 1 || position.Column >= ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width - 1) ||
			(position.Row < 1 || position.Row >= ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height - 1);
	}
	
	public static bool IsEdgeActorTilePosition(this TilePosition position)
	{
		return (position.Column < 2 || position.Column >= ClientSystemConstants.ACTOR_TILE_MAP_SIZE.width - 2) ||
			(position.Row < 2 || position.Row >= ClientSystemConstants.ACTOR_TILE_MAP_SIZE.height - 2);
	}
	
	public static bool IsValidBuildingTilePosition(this TilePosition position)
	{
		return (position.Row >= 0 && position.Row < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height) &&
			(position.Column >= 0 && position.Column < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width);
	}
	
	public static bool IsValidActorTilePosition(this TilePosition position)
	{
		return (position.Row >= 0 && position.Row < ClientSystemConstants.ACTOR_TILE_MAP_SIZE.height) && 
			(position.Column >= 0 && position.Column < ClientSystemConstants.ACTOR_TILE_MAP_SIZE.width);
	}
	
	public static bool IsValidBuildingRow(this int row)
	{
		return row >= 0 && row < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height;
	}
	
	public static bool IsValidActorRow(this int row)
	{
		return row >=0 && row < ClientSystemConstants.ACTOR_TILE_MAP_SIZE.height;
	}
	
	public static bool IsValidBuildingColumn(this int column)
	{
		return column >=0 && column < ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width;
	}
	
	public static bool IsValidActorColumn(this int column)
	{
		return column >= 0 && column < ClientSystemConstants.ACTOR_TILE_MAP_SIZE.width;
	}
	
	public static bool IsStaticTarget(this GameObject target)
	{
		return target == null || target.GetComponent<BuildingBasePropertyBehavior>() != null;
	}
	
	public static GridType ConvertToGridType(this int direction)
	{
		GridType gridType = GridType.None;
				
		switch(direction)
		{
			case 0:
			{
				gridType = GridType.None;
			}
			break;
			case 1:
			{
				gridType = GridType.Up;
			}
			break;
			case 2:
			{
				gridType = GridType.Down;
			}
			break;
			case 3:
			{
				gridType = GridType.UpDown;
			}
			break;
			case 4:
			{
				gridType = GridType.Left;
			}
			break;
			case 5:
			{
				gridType = GridType.UpLeft;
			}
			break;
			case 6:
			{
				gridType = GridType.DownLeft;
			}
			break;
			case 7:	
			{
				gridType = GridType.UpDownLeft;
			}
			break;
			case 8:
			{
				gridType = GridType.Right;
			}
			break;
			case 9:
			{
				gridType = GridType.UpRight;
			}
			break;
			case 10:
			{
				gridType = GridType.DownRight;
			}
			break;
			case 11:
			{
				gridType = GridType.UpDownRight;
			}
			break;
			case 12:	
			{
				gridType = GridType.LeftRight;
			}
			break;
			case 13:
			{
				gridType = GridType.UpLeftRight;
			}
			break;
			case 14:
			{
				gridType = GridType.DownLeftRight;
			}
			break;
			case 15:
			{
				gridType = GridType.UpDownLeftRight;
			}
			break;
		}
		return gridType;
	}
	
	public static TilePosition ConvertToTilePosition(this TilePoint point)
	{
		return new TilePosition(point.column, point.row);
	}
	
	public static List<T> ToOtherList<T,P>(this List<P> originalList) where P : T
	{
		List<T> result = new List<T>();
		foreach(P original in originalList)
		{
			result.Add(original);
		}
		return result;
	}
	
	public static TilePosition GetBuildingFirstBuildingPosition(this IObstacleInfo buildingInfo)
	{
		return buildingInfo.BuildingPosition + buildingInfo.BuildingObstacleList[0];
	}
	
	public static TilePosition GetBuildingFirstActorPosition(this IObstacleInfo buildingInfo)
	{
		return buildingInfo.ActorPosition + buildingInfo.ActorObstacleList[0];
	}
	
	public static bool IsBuildingHover(this IObstacleInfo buildingInfo, IMapData mapData)
	{
		if(Application.loadedLevelName != ClientStringConstants.BUILDING_SCENE_LEVEL_NAME)
		{
			return false;
		}
		else
		{
			TilePosition buildingPosition = buildingInfo.GetBuildingFirstBuildingPosition();
			return mapData.GetBuildingObjectFromBuildingObstacleMap(buildingPosition.Row, buildingPosition.Column) == null;
		}
	}
	
	public static string GetUIGridSortString(this ArmyType armyType)
	{
		string result = (int)armyType > 9 ? ((int)armyType).ToString() : "0" + ((int)armyType).ToString();
		result = ARMY_SORT_STRING_PREFIX + result;
		return result;
	}
	
	public static string GetUIGridSortString(this MercenaryType mercenaryType)
	{
		string result = (int)mercenaryType > 9 ? ((int)mercenaryType).ToString() : "0" + ((int)mercenaryType).ToString();
		result = MERCENARY_SORT_STRING_PREFIX + result;
		return result;
	}
	
	public static string GetUIGridSortString(this PropsType propsType)
	{
		string result = (int)propsType > 9 ? ((int)propsType).ToString() : "0" + ((int)propsType).ToString();
		result = PROPS_SORT_STRING_PREFIX + result;
		return result;
	}
}
