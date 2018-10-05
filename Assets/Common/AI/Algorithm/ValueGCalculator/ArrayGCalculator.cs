using UnityEngine;
using System.Collections;

public class ArrayGCalculator : IGCalculator 
{
	private int[,] m_WeightMap;
	
	public ArrayGCalculator(int[,] weightMap)
	{
		this.m_WeightMap = weightMap;
	}

	public int GetGValue(int row, int column)
	{
		return this.m_WeightMap[row, column];
	}
}
