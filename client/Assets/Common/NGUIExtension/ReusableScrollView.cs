using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReusableScrollView : MonoBehaviour 
{
	[SerializeField]
	private UIDraggablePanel m_DraggablePanel;
	[SerializeField]
	private GameObject m_CellPrefab;
	[SerializeField]
	private ReusableDelegate m_Delegate;
	[SerializeField]
	private int m_ReusableCellPad;
	
	private int m_DisplayCount;
	private int m_MaxReusableCellNumber;
	
	private Transform m_CachedTransform;
	private Transform m_CellsParent;
	
	private Transform m_MinAnchor;
	private Transform m_MaxAnchor;
	
	private bool m_Started;
	private Vector2 m_ZeroPosition;
	private int m_TotalCellNumber;
	
	public UIGrid.Arrangement arrangement = UIGrid.Arrangement.Horizontal;
	
	public float cellWidth = 200f;
	public float cellHeight = 200f;
	public int maxPerLine = 0;
	
	private int m_ContentOffset;
	private int m_MinPoolOffset;
	private List<GameObject> m_ReusablePool;

	private void InitialPosition ()
	{
		if(this.m_TotalCellNumber > 0)
		{
			this.m_MinAnchor.localPosition = (arrangement == UIGrid.Arrangement.Horizontal) ?
						new Vector3(0, 0, this.m_MinAnchor.localPosition.z) :
							new Vector3(0, 0, this.m_MinAnchor.localPosition.z);
			this.m_MinAnchor.localScale = Vector3.one;
			
			Dimention2Position pos = this.CalculatePosition(this.m_TotalCellNumber - 1);
			this.m_MaxAnchor.localPosition = (arrangement == UIGrid.Arrangement.Horizontal) ?
						new Vector3(cellWidth * pos.row, -cellHeight * pos.column, this.m_MaxAnchor.localPosition.z) :
							new Vector3(cellWidth * pos.column, -cellHeight * pos.row, this.m_MaxAnchor.localPosition.z);
			this.m_MaxAnchor.localScale = Vector3.one;
			
			for (int i = 0; i <  this.m_ReusablePool.Count; i++)
			{
				pos = this.CalculatePosition(i + 1);
				
				Transform t = this.m_ReusablePool[i].transform;
				float depth = t.localPosition.z;
				
				t.localScale = Vector3.one;
				t.localPosition = (arrangement == UIGrid.Arrangement.Horizontal) ?
					new Vector3(cellWidth * pos.row, -cellHeight * pos.column, depth) :
						new Vector3(cellWidth * pos.column, -cellHeight * pos.row, depth);
			}
			this.m_DraggablePanel.ResetPosition();
		}
	}
	
	void Start()
	{
		this.m_CachedTransform = this.m_DraggablePanel.transform;
		this.m_CellsParent = this.m_DraggablePanel.transform.GetChild(0);
		
		this.m_ReusablePool = new List<GameObject>();
		
		UIPanel panel = this.m_DraggablePanel.GetComponent<UIPanel>();
		this.m_DisplayCount = this.arrangement == UIGrid.Arrangement.Horizontal ? 
			Mathf.CeilToInt(panel.clipRange.z / this.cellWidth) :
			Mathf.CeilToInt(panel.clipRange.w / this.cellHeight);
		
		this.ReloadData();
	}
	
	private void InitialAnchorCell()
	{
		if(this.m_TotalCellNumber > 0)
		{
			this.m_MinAnchor = this.InsertCell(0).transform;
			if(this.m_TotalCellNumber > 1)
			{
				this.m_MaxAnchor = this.InsertCell(this.m_TotalCellNumber - 1).transform;
			}
			else
			{
				this.m_MaxAnchor = this.m_MinAnchor;
			}
		}
	}
	
	private void InitialPoolCellStatus()
	{
		this.m_ContentOffset = 0;
		
		bool isPoolEmpty = this.m_ReusablePool.Count == 0;
		
		for(int i = 0; i < this.m_MaxReusableCellNumber; i++)
		{
			if(i + 2 >= this.m_TotalCellNumber)
			{
				break;
			}
			this.m_MinPoolOffset = 1;
			
			if(isPoolEmpty)
			{
				this.m_ReusablePool.Add(this.InsertCell(i + 1));
			}
			else
			{
				this.m_Delegate.InitialCell(i + 1, this.m_ReusablePool[i]);
			}
		}
		
		this.m_MaxReusableCellNumber = this.m_ReusablePool.Count;
		this.InitialPosition();
	}
	
	public void ReloadData()
	{
		this.m_MaxReusableCellNumber = this.maxPerLine > 0 ? (this.m_DisplayCount + 2 * this.m_ReusableCellPad) * this.maxPerLine
			: this.m_DisplayCount + 2 * this.m_ReusableCellPad;
		
		if(this.m_ReusablePool != null)
		{
			if(this.m_MinAnchor != null)
			{
				GameObject.Destroy(this.m_MinAnchor.gameObject);
				GameObject.Destroy(this.m_MaxAnchor.gameObject);
			}
			foreach (GameObject cell in this.m_ReusablePool)
			{
				GameObject.Destroy(cell);
			}
			
			this.m_ReusablePool.Clear();
			
			this.m_TotalCellNumber = this.m_Delegate.TotalNumberOfCells;
			this.InitialAnchorCell();
			this.InitialPoolCellStatus();
			this.m_Started = true;
		}
	}
	
	void LateUpdate()	
	{
		if(this.m_Started)
		{
			this.m_ZeroPosition = new Vector2(this.m_DraggablePanel.transform.localPosition.x, 
				this.m_DraggablePanel.transform.localPosition.y);
			this.m_ContentOffset = 0;
			this.m_Started = false;
		}
		else
		{
			int offset = this.arrangement == UIGrid.Arrangement.Horizontal ? (int)((this.m_ZeroPosition.x - this.m_CachedTransform.localPosition.x) / cellWidth) :
				(int)((this.m_CachedTransform.localPosition.y - this.m_ZeroPosition.y) / cellHeight);
			if(offset != this.m_ContentOffset)
			{
				bool isToMax = offset > this.m_ContentOffset;
				if(isToMax)
				{
					for(int i = this.m_ContentOffset + 1; i <= offset; i ++)
					{
						this.ReuseCell(isToMax, i);
					}
				}
				else
				{
					for(int i = this.m_ContentOffset -1; i >= offset; i--)
					{
						this.ReuseCell(isToMax, i);
					}
				}
				this.m_DraggablePanel.UpdateScrollbars(true);
				this.m_ContentOffset = offset;
			}
		}
	}

	private GameObject InsertCell(int index)
	{
		GameObject cell = GameObject.Instantiate(this.m_CellPrefab) as GameObject;
        float depth = cell.transform.position.z;
		cell.transform.parent = this.m_CellsParent;
		cell.transform.localPosition = new Vector3(0,0,depth);
		
		this.m_Delegate.InitialCell(index, cell);
		
		return cell;
	}
	
	private void ReuseCell(bool isToMax, int offset)
	{
		int validPerLine = this.maxPerLine > 0 ? this.maxPerLine : 1;
		if(isToMax)
		{
			int maxRow = offset;
			for(int i = 0; i < validPerLine; i ++)
			{
				Dimention2Position minPos = this.CalculatePosition(this.m_MinPoolOffset);
				bool isRecycle = maxRow -  minPos.row > this.m_ReusableCellPad; 
				bool isValid = this.m_MinPoolOffset + this.m_MaxReusableCellNumber < this.m_TotalCellNumber - 1;
				
				if(isRecycle && isValid)
				{
					Dimention2Position newPos = this.CalculatePosition(this.m_MinPoolOffset + this.m_MaxReusableCellNumber);
					int deltaRow = newPos.row - minPos.row;
					int deltaColumn = newPos.column - minPos.column;
						
					GameObject cell = this.m_ReusablePool[0];
					this.m_ReusablePool.RemoveAt(0);
					this.m_ReusablePool.Insert(this.m_ReusablePool.Count,cell);
					this.m_Delegate.InitialCell(this.m_MinPoolOffset + this.m_MaxReusableCellNumber, cell);
					float depth = cell.transform.localPosition.z;
				    cell.transform.localPosition = this.arrangement == UIGrid.Arrangement.Horizontal ?
						new Vector3(cell.transform.localPosition.x + this.cellWidth * deltaRow, cell.transform.localPosition.y - this.cellWidth * deltaColumn, depth) :
						new Vector3(cell.transform.localPosition.x + this.cellWidth * deltaColumn, cell.transform.localPosition.y - this.cellHeight * deltaRow, depth);
					this.m_MinPoolOffset ++;
				}
			}
		}
		else
		{
			int minRow = offset + this.m_DisplayCount;
			for(int i = 0; i < validPerLine; i ++)
			{
				Dimention2Position maxPos = this.CalculatePosition(this.m_MinPoolOffset + this.m_MaxReusableCellNumber - 1);
				
				bool isRecycle = maxPos.row - minRow > this.m_ReusableCellPad;
				bool isValid = this.m_MinPoolOffset > 1;
				
				if(isRecycle && isValid)
				{
					Dimention2Position newPos = this.CalculatePosition(this.m_MinPoolOffset - 1);
					int deltaRow = newPos.row - maxPos.row;
					int deltaColumn = newPos.column - maxPos.column;
					
					GameObject cell = this.m_ReusablePool[this.m_ReusablePool.Count - 1];
					this.m_ReusablePool.RemoveAt(this.m_ReusablePool.Count - 1);
					this.m_ReusablePool.Insert(0, cell);
					this.m_Delegate.InitialCell(this.m_MinPoolOffset - 1, cell);
					float depth = cell.transform.localPosition.z;
				    cell.transform.localPosition = this.arrangement == UIGrid.Arrangement.Horizontal ?
						new Vector3(cell.transform.localPosition.x + this.cellWidth * deltaRow, cell.transform.localPosition.y - this.cellHeight * deltaColumn, depth) :
						new Vector3(cell.transform.localPosition.x + this.cellWidth * deltaColumn, cell.transform.localPosition.y - this.cellHeight * deltaRow, depth);
					this.m_MinPoolOffset --;
				}
			}
		}
	}
	
	private Dimention2Position CalculatePosition(int index)
	{
		Dimention2Position result = new Dimention2Position();
		int validPerLine = this.maxPerLine > 0 ? this.maxPerLine : 1;
		
		result.column = index % validPerLine;
		result.row = index / validPerLine;
		
		return result;
	}
	
	private struct Dimention2Position
	{
		public int row;
		public int column;
	}
}
