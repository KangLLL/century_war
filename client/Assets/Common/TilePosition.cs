using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class TilePosition
{
	private int m_Column;
	private int m_Row;
	
	public int Column 
	{ 
		get
		{
			return this.m_Column;
		}
		set
		{
			this.m_Column = value; 
		}
	}
	
	public int Row 
	{ 
		get
		{
			return this.m_Row;
		}
		set
		{
			this.m_Row = value;
		}
	}
	
	public TilePosition()
	{
	}
	
	public TilePosition(int column, int row)
	{
		this.Column = column;
		this.Row = row;
	}
	
	public void RandomBuildingPosition()
	{
		this.Row = UnityEngine.Random.Range(0, ClientSystemConstants.BUILDING_TILE_MAP_SIZE.height);
		this.Column = UnityEngine.Random.Range(0, ClientSystemConstants.BUILDING_TILE_MAP_SIZE.width);
	}
	
	public void RandomActorPosition()
	{
		this.Row = UnityEngine.Random.Range(0, ClientSystemConstants.ACTOR_TILE_MAP_SIZE.height);
		this.Column = UnityEngine.Random.Range(0, ClientSystemConstants.ACTOR_TILE_MAP_SIZE.width);
	}
	
	public int GetIndexInt()
	{
		return this.Column + (this.Row << 16);
	}
	
	public override bool Equals (object obj)
	{
		TilePosition other = obj as TilePosition;
		if(other == null)
		{
			return false;
		}
		return other.Row == this.Row && other.Column == this.Column;
	}
	
	public override int GetHashCode ()
	{
		return this.Column + (this.Row  << 16);
	}
	
	public static TilePosition operator + (TilePosition c1, TilePosition c2)
	{
		return new TilePosition(c1.Column + c2.Column, c1.Row + c2.Row); 
	}
	
	public static TilePosition operator - (TilePosition c1, TilePosition c2)
	{
		return new TilePosition(c1.Column - c2.Column, c1.Row - c2.Row);
	}
}
