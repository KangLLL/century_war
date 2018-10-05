using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public static class AStarPathFinder
{

    static AStarPathFinder()
    {
        s_NodePool = new List<AStarPathNode>(512);
        for (int i = 0; i < 512; i++)
        {
            s_NodePool.Add(new AStarPathNode());
        }
        s_OpenList = new SortedDictionary<int, List<AStarPathNode>>();
        s_CloseList = new List<AStarPathNode>();
        s_OpenDictionary = new Dictionary<int, AStarPathNode>();
        s_CloseDictionary = new Dictionary<int, AStarPathNode>();
    }

    private static List<AStarPathNode> s_NodePool;
	private static int s_CurrentIndex;

    private static SortedDictionary<int, List<AStarPathNode>> s_OpenList;
    private static List<AStarPathNode> s_CloseList;
    private static Dictionary<int, AStarPathNode> s_OpenDictionary;
    private static Dictionary<int, AStarPathNode> s_CloseDictionary;

    private static AStarPathNode GetNewNodeFromPool()
	{
		AStarPathNode result = null;
		if(s_CurrentIndex == s_NodePool.Count)
		{
			result = new AStarPathNode();
			s_NodePool.Add(result);
		}
		else
		{
			result = s_NodePool[s_CurrentIndex];
		}
		
		s_CurrentIndex ++;
		return result;
	}
	
	private static void RecycleAllNode()
	{
		s_CurrentIndex = 0;
	}
	
	public static List<TilePosition> CalculatePathTile(IGCalculator calculator, bool[,] obstacleMap, 
		TilePosition startTile, TilePosition endTile, out List<TilePosition> aStarPath)
	{
		aStarPath = CalculateAStarPahtTile(calculator,startTile,endTile);
		return CalculateLinePath(aStarPath, obstacleMap);
	}
	
	public static List<TilePosition> CalculatePathTile(int[,] weightMap, bool[,] obstacleMap, 
		TilePosition startTile, TilePosition endTile, out List<TilePosition> aStarPath)
	{
		aStarPath = CalculateAStarPathTile(weightMap, startTile, endTile);
		return CalculateLinePath(aStarPath, obstacleMap);
	}
	
	private static List<TilePosition> CalculateLinePath(List<TilePosition> aStarPath, bool[,] obstacleMap)
	{
		List<TilePosition> result = new List<TilePosition>();
		/*
		Debug.Log("------------------------------------------");
		foreach (TilePosition at in aStarPath) 
		{
			Debug.Log("astar pt is row:" + at.Row + "astar pt column is:" + at.Column);
		}
		*/
		List<TilePosition> needLinePath = new List<TilePosition>();
		needLinePath.Add(aStarPath[0]);
		for(int i = 1; i < aStarPath.Count; i ++)
		{
			needLinePath.Add(aStarPath[i]);
			if(obstacleMap[aStarPath[i].Row,aStarPath[i].Column])
			{
				break;
			}
		}
		
		for(int i = 0; i < needLinePath.Count - 1; )
		{
			TilePosition start = needLinePath[i];
			if(i == 0)
			{
				result.Add(start);
			}
			for(int j = needLinePath.Count - 1; j > i; j--)
			{
				TilePosition end = needLinePath[j];
				List<TilePosition> line = LinearizationHelper.BresenhamLine(start, end);
				/*
				Debug.Log("line pt is:" + line.Count + ".start point row is:" + start.Row + "start point column is:" + start.Column +
					".end point row is:" + end.Row + "end point column is:" + end.Column);
				foreach (TilePosition lt in line) 
				{
					//Debug.Log("find the error");
					Debug.Log("row is:" + lt.Row + ",column is:" + lt.Column);
				}
				*/
				bool blocked = false;
				foreach(TilePosition lineIndex in line)
				//for(int k = i==0 ? 1 : 0; k < line.Count; k ++)
				{
					//TilePosition lineIndex = line[k];
					if(obstacleMap[lineIndex.Row, lineIndex.Column])
					{
						//Debug.Log("**********************");
						//Debug.Log(lineIndex.Row + " , " + lineIndex.Column); 
						blocked = true; 
						//Debug.Log("**********************");
						break;
					}
				}
				if(!blocked)
				{
					result.Add(end);
					i = j;
				}
				else
				{
					if(j == i + 1)
					{
						result.Add(end);
						return result;
					}
				}
			}
		}
		
		return result;
	}
	
	
	public static List<TilePosition> CalculateAStarPathTile(int[,] weightMap, TilePosition startTile, TilePosition endTile)
	{
		ArrayGCalculator arrayCalculator = new ArrayGCalculator(weightMap);
		
		List<AStarPathNode> rawPath = CalculatePath(arrayCalculator, startTile.ConvertToAStarPathNode(), endTile.ConvertToAStarPathNode());
		List<TilePosition> result = new List<TilePosition>();
		for(int i = 0; i < rawPath.Count; i++)
		{
			result.Add(rawPath[i].ConvertToTilePosition());
		}
		return result;
	}
	
	public static List<TilePosition> CalculateAStarPahtTile(IGCalculator characterCalculator, TilePosition startTile, TilePosition endTile)
	{
		List<AStarPathNode> rawPath = CalculatePath(characterCalculator, startTile.ConvertToAStarPathNode(), endTile.ConvertToAStarPathNode());
		List<TilePosition> result = new List<TilePosition>();
		for(int i = 0; i < rawPath.Count; i++)
		{
			result.Add(rawPath[i].ConvertToTilePosition());
		}
		return result;
	}
	
	private static List<AStarPathNode> CalculatePath(IGCalculator calculator, AStarPathNode startPathNode, AStarPathNode endPathNode)
    {
        bool existPath = false;
		/*
		Debug.Log("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$");
		Debug.Log(startPathNode.Row + "," + startPathNode.Column);
		*/
		startPathNode.GCalculator = calculator;
		s_OpenList.Add(startPathNode.ValueF, new List<AStarPathNode>(){ startPathNode });
        s_OpenDictionary.Add(startPathNode.Column + (startPathNode.Row << 16), startPathNode);
		RecycleAllNode();
        while (s_OpenList.Count != 0)
        {
            SortedDictionary<int, List<AStarPathNode>>.Enumerator enumerator = s_OpenList.GetEnumerator();
			enumerator.MoveNext();
			AStarPathNode currentPathNode = enumerator.Current.Value[0];
			enumerator.Current.Value.RemoveAt(0);
			if(enumerator.Current.Value.Count == 0)
			{
            	s_OpenList.Remove(enumerator.Current.Key);
			}
            s_OpenDictionary.Remove(currentPathNode.Column + (currentPathNode.Row << 16));
			if (endPathNode.Column == currentPathNode.Column && endPathNode.Row == currentPathNode.Row)
            {
				existPath = true;
				endPathNode = currentPathNode;
				break;
            }
            s_CloseList.Add(currentPathNode);
            s_CloseDictionary.Add(currentPathNode.Column + (currentPathNode.Row << 16), currentPathNode);
			//leftbottom
            AStarPathNode leftBottomPathNode = GetNewNodeFromPool(); //new AStarPathNode();
            leftBottomPathNode.Row = currentPathNode.Row + 1;
			leftBottomPathNode.Column = currentPathNode.Column - 1;
			leftBottomPathNode.GCalculator = calculator;
            DealNeighbourPathNode(currentPathNode, leftBottomPathNode, endPathNode, s_OpenList, s_CloseList, s_OpenDictionary, s_CloseDictionary);
			
            //lefttop
            AStarPathNode leftTopPathNode = GetNewNodeFromPool();//new AStarPathNode();
			leftTopPathNode.Row = currentPathNode.Row - 1;
			leftTopPathNode.Column = currentPathNode.Column - 1;
			leftTopPathNode.GCalculator = calculator;
            DealNeighbourPathNode(currentPathNode, leftTopPathNode, endPathNode, s_OpenList, s_CloseList, s_OpenDictionary, s_CloseDictionary);
            //rightbottom
            AStarPathNode rightBottomPathNode = GetNewNodeFromPool();//new AStarPathNode();
			rightBottomPathNode.Row = currentPathNode.Row + 1;
			rightBottomPathNode.Column = currentPathNode.Column + 1;
			rightBottomPathNode.GCalculator = calculator;
            DealNeighbourPathNode(currentPathNode, rightBottomPathNode, endPathNode, s_OpenList, s_CloseList, s_OpenDictionary, s_CloseDictionary);
            //righttop
            AStarPathNode rightTopPathNode = GetNewNodeFromPool();//new AStarPathNode();
            rightTopPathNode.Row = currentPathNode.Row - 1;
			rightTopPathNode.Column = currentPathNode.Column + 1;
			rightTopPathNode.GCalculator = calculator;
            DealNeighbourPathNode(currentPathNode, rightTopPathNode, endPathNode, s_OpenList, s_CloseList, s_OpenDictionary, s_CloseDictionary);
            
            //bottom
            AStarPathNode bottomPathNode = GetNewNodeFromPool();//new AStarPathNode();
            bottomPathNode.Row = currentPathNode.Row + 1;
			bottomPathNode.Column = currentPathNode.Column;
			bottomPathNode.GCalculator = calculator;
            DealNeighbourPathNode(currentPathNode, bottomPathNode, endPathNode, s_OpenList, s_CloseList, s_OpenDictionary, s_CloseDictionary);
            //left
            AStarPathNode leftPathNode = GetNewNodeFromPool();//new AStarPathNode();
			leftPathNode.Row = currentPathNode.Row;
			leftPathNode.Column = currentPathNode.Column - 1;
			leftPathNode.GCalculator = calculator;
            DealNeighbourPathNode(currentPathNode, leftPathNode, endPathNode, s_OpenList, s_CloseList, s_OpenDictionary, s_CloseDictionary);
            //right
            AStarPathNode rightPathNode = GetNewNodeFromPool();//new AStarPathNode();
			rightPathNode.Row = currentPathNode.Row;
			rightPathNode.Column = currentPathNode.Column + 1;
			rightPathNode.GCalculator = calculator;
            DealNeighbourPathNode(currentPathNode, rightPathNode, endPathNode, s_OpenList, s_CloseList, s_OpenDictionary, s_CloseDictionary);
            //top
            AStarPathNode topPathNode = GetNewNodeFromPool();//new AStarPathNode();
            topPathNode.Row = currentPathNode.Row - 1;
			topPathNode.Column = currentPathNode.Column;
			topPathNode.GCalculator = calculator;
            DealNeighbourPathNode(currentPathNode, topPathNode, endPathNode, s_OpenList, s_CloseList, s_OpenDictionary, s_CloseDictionary);
			
        }
		
        s_OpenList.Clear();
        s_CloseList.Clear();
        s_OpenDictionary.Clear();
        s_CloseDictionary.Clear();

        if (existPath)
        {
            List<AStarPathNode> path = new List<AStarPathNode>();
            AStarPathNode pathNode = endPathNode;
            while (pathNode.Column != startPathNode.Column || pathNode.Row != startPathNode.Row)
            {
                path.Add(pathNode);
                pathNode = pathNode.ParentPathNode;
            }
			path.Add(startPathNode);
            path.Reverse();
            return path;
        }
        return null;
    }

    private static void DealNeighbourPathNode(AStarPathNode currentPathNode, AStarPathNode neighbourPathNode, AStarPathNode endPathNode, SortedDictionary<int, List<AStarPathNode>> openList, List<AStarPathNode> closeList, Dictionary<int, AStarPathNode> openDictionary, Dictionary<int, AStarPathNode> closeDictionary)
    {
        if (neighbourPathNode.IsValidNode() &&
            !IsInList(neighbourPathNode, closeDictionary))
        {
            if (!IsInList(neighbourPathNode, openDictionary))
            {
                neighbourPathNode.ParentPathNode = currentPathNode;
                neighbourPathNode.ValueG = neighbourPathNode.CalculateValueG(currentPathNode);
                neighbourPathNode.ValueH = neighbourPathNode.CalculateValueH(endPathNode);
				if(openList.ContainsKey(neighbourPathNode.ValueF))
				{
					openList[neighbourPathNode.ValueF].Add(neighbourPathNode);
				}
				else
				{
					openList.Add(neighbourPathNode.ValueF, new List<AStarPathNode>() { neighbourPathNode });
				}
                openDictionary.Add(neighbourPathNode.Column + (neighbourPathNode.Row << 16), neighbourPathNode);
            }
            else
            {
                neighbourPathNode = openDictionary[neighbourPathNode.Column + (neighbourPathNode.Row << 16)];
				
				openList[neighbourPathNode.ValueF].Remove(neighbourPathNode);
				if(openList[neighbourPathNode.ValueF].Count == 0)
				{
					openList.Remove(neighbourPathNode.ValueF);
				}
				
                int valueG = neighbourPathNode.CalculateValueG(currentPathNode);
                if (valueG < neighbourPathNode.ValueG)
                {
                    neighbourPathNode.ParentPathNode = currentPathNode;
                    neighbourPathNode.ValueG = valueG;
                    neighbourPathNode.ValueH = neighbourPathNode.CalculateValueH(endPathNode);
                }
				
				if(openList.ContainsKey(neighbourPathNode.ValueF))
				{
					openList[neighbourPathNode.ValueF].Add(neighbourPathNode);
				}
				else
				{
					openList.Add(neighbourPathNode.ValueF, new List<AStarPathNode>() { neighbourPathNode});
				}
            }
        }
    }

    private static bool IsInList(AStarPathNode neighbourPathNode, Dictionary<int, AStarPathNode> dictionary)
    {
        return dictionary.ContainsKey(neighbourPathNode.Column + (neighbourPathNode.Row << 16));
    }
}
