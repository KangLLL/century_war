using UnityEngine;
using System.Collections;
using System;

public class AStarPathNode : IComparable<AStarPathNode> 
{	
	private int m_Row;
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
	
	private int m_Column;
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
	
    private int m_ValueH;
    public int ValueH
    {
        get
        {
            return m_ValueH;
        }
        set
        {
            m_ValueH = value;
        }
    }

    private int m_ValueG;
    public int ValueG
    {
        get
        {
            return m_ValueG;
        }
        set
        {
            m_ValueG = value;
        }
    }

    public int ValueF
    {
        get
        {
            return m_ValueG + m_ValueH;
        }
    }

    private AStarPathNode m_ParentPathNode;
    public AStarPathNode ParentPathNode
    {
        get
        {
            return m_ParentPathNode;
        }
        set
        {
            m_ParentPathNode = value;
        }
    }
	
	private IGCalculator m_GCalculator;
	public IGCalculator GCalculator
	{
		get
		{
			return m_GCalculator;
		}
		set
		{
			m_GCalculator = value;
		}
	}
	
	public int CalculateValueG(AStarPathNode parentPathNode)
	{
		return this.m_GCalculator.GetGValue(this.m_Row, this.m_Column) + parentPathNode.ValueG;
	}

    public int CalculateValueH(AStarPathNode endPathNode)
    {
		int deltaRow = Mathf.Abs(endPathNode.Row - this.m_Row);
		int deltaColumn = Mathf.Abs(endPathNode.Column - this.m_Column);
		
		return deltaRow+deltaColumn;
    }
	
	public bool IsValidNode()
	{
		return this.m_Row.IsValidActorRow() && this.m_Column.IsValidActorColumn();
	}


    #region IComparable<PathNode> 成员

    int IComparable<AStarPathNode>.CompareTo(AStarPathNode other)
    {
        return this.ValueF - other.ValueF;
    }

    #endregion
}
