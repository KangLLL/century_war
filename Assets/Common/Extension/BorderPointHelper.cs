using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities;
using ConfigUtilities.Enums;

public class BorderPointHelper  
{
	private static Dictionary<BuildingType, List<TilePosition>> s_BuildingBorderDict = new Dictionary<BuildingType, List<TilePosition>>();
	private static Dictionary<BuildingType, List<TilePosition>> s_BuildingInflateOneBorderDict = new Dictionary<BuildingType, List<TilePosition>>();
	private static Dictionary<RemovableObjectType, List<TilePosition>> s_RemovableObjectBorderDict = new Dictionary<RemovableObjectType, List<TilePosition>>();
	private static Dictionary<RemovableObjectType, List<TilePosition>> s_RemovableObjectInflateOneBorderDict = new Dictionary<RemovableObjectType, List<TilePosition>>();
	
	
	public static List<TilePosition> GetBorder(IObstacleInfo obstacle)
	{
		if(obstacle is IBuildingInfo)
		{
			IBuildingInfo building = obstacle as IBuildingInfo;
			
			if(!s_BuildingBorderDict.ContainsKey(building.BuildingType))
			{
				List<TilePosition> border = FindBorder(obstacle);
				s_BuildingBorderDict.Add(building.BuildingType, border);
			}
			return s_BuildingBorderDict[building.BuildingType];
		}
		else
		{
			IRemovableObjectInfo removableObject = obstacle as IRemovableObjectInfo;
			
			if(!s_RemovableObjectBorderDict.ContainsKey(removableObject.ObjectType))
			{
				List<TilePosition> border = FindBorder(obstacle);
				s_RemovableObjectBorderDict.Add(removableObject.ObjectType, border);
			}
			return s_RemovableObjectBorderDict[removableObject.ObjectType];
		}
	}
	
	public static List<TilePosition> GetInflateOneBorder(IObstacleInfo obstacle)
	{
		return GetInflateOneBorder(obstacle, true);
	}
	
	public static List<TilePosition> GetInflateOneBorder(IObstacleInfo obstacle, bool isActor)
	{
		if(obstacle is IBuildingInfo)
		{
			IBuildingInfo building = obstacle as IBuildingInfo;
			if(!s_BuildingInflateOneBorderDict.ContainsKey(building.BuildingType))
			{
				List<TilePosition> inflateOneObstacleList = Inflate(obstacle, isActor);
				List<TilePosition> border = FindBorder(inflateOneObstacleList);
				s_BuildingInflateOneBorderDict.Add(building.BuildingType,border);
			}
			return s_BuildingInflateOneBorderDict[building.BuildingType];
		}
		else
		{
			IRemovableObjectInfo removableObject = obstacle as IRemovableObjectInfo;
			if(!s_RemovableObjectInflateOneBorderDict.ContainsKey(removableObject.ObjectType))
			{
				List<TilePosition> inflateOneObstacleList = Inflate(obstacle, isActor);
				List<TilePosition> border = FindBorder(inflateOneObstacleList);
				s_RemovableObjectInflateOneBorderDict.Add(removableObject.ObjectType, border);
			}
			return s_RemovableObjectInflateOneBorderDict[removableObject.ObjectType];
		}
	}
	
	private static List<TilePosition> Inflate(IObstacleInfo obstacleInfo, bool isActor)
	{
		ObstacleConfigData data = null;
		if(obstacleInfo is IBuildingInfo)
		{
			data = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(((IBuildingInfo)obstacleInfo).BuildingType, 0);
		}
		else
		{
			data = ConfigInterface.Instance.RemovableConfigHelper.GetRemovableObjectData(((IRemovableObjectInfo)obstacleInfo).ObjectType);
		}
		
		List<TilePosition> obstacleList = new List<TilePosition>();
		if(isActor)
		{
			foreach (ConfigUtilities.Structs.TilePoint obstacle in data.ActorObstacleList) 
			{			
				obstacleList.Add(obstacle.ConvertToTilePosition());
			}
		}
		else
		{
			foreach (ConfigUtilities.Structs.TilePoint obstacle in data.BuildingObstacleList) 
			{			
				obstacleList.Add(obstacle.ConvertToTilePosition());
			}
		}
		
		List<TilePosition> inflateList = new List<TilePosition>();
		
		foreach (TilePosition obstacle in obstacleList)
		{
			TilePosition up = new TilePosition(obstacle.Column, obstacle.Row - 1);
			TilePosition down = new TilePosition(obstacle.Column, obstacle.Row + 1);
			TilePosition left = new TilePosition(obstacle.Column - 1, obstacle.Row);
			TilePosition right = new TilePosition(obstacle.Column + 1, obstacle.Row);
			TilePosition leftUp = new TilePosition(obstacle.Column - 1, obstacle.Row - 1);
			TilePosition rightUp = new TilePosition(obstacle.Column + 1, obstacle.Row - 1);
			TilePosition leftDown = new TilePosition(obstacle.Column - 1, obstacle.Row + 1);
			TilePosition rightDown = new TilePosition(obstacle.Column + 1, obstacle.Row + 1);
			
			if(!obstacleList.Contains(up) && !inflateList.Contains(up))
			{
				inflateList.Add(up);
			}
			if(!obstacleList.Contains(down) && !inflateList.Contains(down))
			{
				inflateList.Add(down);
			}
			if(!obstacleList.Contains(left) && !inflateList.Contains(left))
			{
				inflateList.Add(left);
			}
			if(!obstacleList.Contains(right) && !inflateList.Contains(right))
			{
				inflateList.Add(right);
			}
			if(!obstacleList.Contains(leftUp) && !inflateList.Contains(leftUp))
			{
				inflateList.Add(leftUp);
			}
			if(!obstacleList.Contains(rightUp) && !inflateList.Contains(rightUp))
			{
				inflateList.Add(rightUp);
			}
			if(!obstacleList.Contains(leftDown) && !inflateList.Contains(leftDown))
			{
				inflateList.Add(leftDown);
			}
			if(!obstacleList.Contains(rightDown) && !inflateList.Contains(rightDown))
			{
				inflateList.Add(rightDown);
			}
		}
		List<TilePosition> result = new List<TilePosition>();
		result.AddRange(obstacleList);
		result.AddRange(inflateList);
		return result;
	}
	
	private static List<TilePosition> FindBorder(IObstacleInfo obstacleInfo)
	{
		ObstacleConfigData data = null;
		if(obstacleInfo is IBuildingInfo)
		{
			data = ConfigInterface.Instance.BuildingConfigHelper.GetBuildingData(((IBuildingInfo)obstacleInfo).BuildingType, 0);
		}
		else
		{
			data = ConfigInterface.Instance.RemovableConfigHelper.GetRemovableObjectData(((IRemovableObjectInfo)obstacleInfo).ObjectType);
		}
		
		List<TilePosition> obstacleList = new List<TilePosition>();
		
		int minRow = 255;
		int maxRow = -255;
		int minColumn = 255;
		int maxColumn = -255;
		
		foreach (ConfigUtilities.Structs.TilePoint obstacle in data.ActorObstacleList) 
		{
			minRow = Mathf.Min(minRow, obstacle.row);
			maxRow = Mathf.Max(maxRow, obstacle.row);
			minColumn = Mathf.Min(minColumn, obstacle.column);
			maxColumn = Mathf.Max(maxColumn, obstacle.column);
			
			obstacleList.Add(obstacle.ConvertToTilePosition());
		}
		
		return FindBorder (obstacleList, minRow, maxRow, minColumn, maxColumn);
	}
	
	private static List<TilePosition> FindBorder (List<TilePosition> obstacleList)
	{
		int minRow = 255;
		int maxRow = -255;
		int minColumn = 255;
		int maxColumn = -255;
		
		foreach (TilePosition obstacle in obstacleList) 
		{
			minRow = Mathf.Min(minRow, obstacle.Row);
			maxRow = Mathf.Max(maxRow, obstacle.Row);
			minColumn = Mathf.Min(minColumn, obstacle.Column);
			maxColumn = Mathf.Max(maxColumn, obstacle.Column);
		}
		
		return FindBorder(obstacleList, minRow, maxRow, minColumn, maxColumn);
	}
	
	private static List<TilePosition> FindBorder (List<TilePosition> obstacleList, int minRow, int maxRow, int minColumn, int maxColumn)
	{
		bool[,] bitmap = new bool[maxRow - minRow + 1, maxColumn - minColumn + 1];
		
		for(int i = 0; i < bitmap.GetLength(0); i++)
		{
			for(int j = 0; j < bitmap.GetLength(1); j++)
			{
				TilePosition tp = new TilePosition(minColumn + j, minRow + i);
				bitmap[i,j] = obstacleList.Contains(tp);
			}
		}
		
		List<TilePosition> result = new List<TilePosition>();
		foreach (TilePosition obstacle in obstacleList) 
		{
			if(obstacle.Row == minRow || obstacle.Row == maxRow || obstacle.Column == minColumn || obstacle.Column == maxColumn)
			{
				result.Add(obstacle);
			}
			else
			{
				int r = obstacle.Row - minRow;
				int c = obstacle.Column - minColumn;
				if(!(bitmap[r-1, c] && bitmap[r+1, c] && bitmap[r, c-1] && bitmap[r, c+1]))
				{
					result.Add(obstacle);
				}
			}
		}
		return result;
	}
	
	public static TilePosition FindValidInflateOneBorderPoint(IObstacleInfo obstacleInfo)
	{
		return FindValidInflateOneBorderPoint(obstacleInfo, true);
	}
	
	public static TilePosition FindValidInflateOneBorderPoint(IObstacleInfo obstacleInfo, bool isActor)
	{
		List<TilePosition> borders = GetInflateOneBorder(obstacleInfo, isActor);
		
		int index = Random.Range(0, borders.Count);
		TilePosition result = isActor ? obstacleInfo.ActorPosition + borders[index] :
			obstacleInfo.BuildingPosition + borders[index];
		if(isActor)
		{
			while(!result.IsValidActorTilePosition())
			{
				index = Random.Range(0, borders.Count);
			 	result = obstacleInfo.ActorPosition + borders[index];
			}
		}
		else
		{
			while(!result.IsValidBuildingTilePosition())
			{
				index = Random.Range(0, borders.Count);
			 	result = obstacleInfo.BuildingPosition + borders[index];
			}
		}
		//Debug.Log(result.Row + " , " + result.Column);
		return result;
	}
}
