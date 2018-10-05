using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InvaderFindOutState : WalkState 
{
	public InvaderFindOutState(IMapData mapData, TilePosition targetPosition, NewAI aiBehavior) :
		base(mapData, targetPosition, aiBehavior)
	{
		this.m_StateName = "InvaderFindOut";
		this.WalkVelocity = this.CharacterAI.PropertyBehavior.MoveVelocity;
	}
	
	private CharacterAI CharacterAI
	{
		get
		{
			return (CharacterAI)this.m_AIBehavior;
		}
	}
	
	protected override IGCalculator FindPathStrategy 
	{
		get 
		{
			return new CharacterGCalculator(this.m_MapData, this.m_TargetPosition.Row, this.m_TargetPosition.Column);
		}
	}
	
	protected override void FindPath ()
	{
		TilePosition startPosition = this.m_PreviousPosition;

		List<TilePosition> checkList = new List<TilePosition>();
		List<TilePosition> addList = new List<TilePosition>();
		checkList.Add(startPosition);
		int currentLayer = 0;
		this.m_LinePath = new Queue<TilePosition>();

		int deltaRow = this.m_TargetPosition.Row - startPosition.Row;
		int deltaColumn = this.m_TargetPosition.Column - startPosition.Column;
		
		while(checkList.Count > 0)
		{
			currentLayer ++;
			addList.Clear();
			foreach (TilePosition item in checkList) 
			{
				if(this.m_MapData.ActorCanPass(item.Row,item.Column))
				{
					this.m_LinePath.Enqueue(item);
					return;
				}
				
				TilePosition left = new TilePosition(item.Column - 1, item.Row);
				TilePosition right = new TilePosition(item.Column + 1, item.Row);
				TilePosition top = new TilePosition(item.Column, item.Row + 1);
				TilePosition bottom = new TilePosition(item.Column, item.Row - 1);
				
				if(this.IsNeedCheck(left,startPosition, currentLayer, deltaRow, deltaColumn))
				{
					addList.Add(left);
				}
				if(this.IsNeedCheck(right,startPosition, currentLayer, deltaRow, deltaColumn))
				{
					addList.Add(right);
				}
				if(this.IsNeedCheck(top,startPosition, currentLayer, deltaRow, deltaColumn))
				{
					addList.Add(top);
				}
				if(this.IsNeedCheck(bottom,startPosition, currentLayer, deltaRow, deltaColumn))
				{
					addList.Add(bottom);
				}
			}
			checkList.Clear();
			checkList.AddRange(addList);
		}
	}
	
	private bool IsNeedCheck(TilePosition position, TilePosition startPosition, int currentLayer, int deltaRow, int deltaColumn)
	{
		if(!position.IsValidActorTilePosition())
		{
			return false;
		}

		int dRow = position.Row - startPosition.Row;
		int dColumn = position.Column - startPosition.Column;
		if(dRow * deltaRow < 0 || dColumn * deltaColumn < 0 || 
		   (deltaRow == 0 && dRow != 0) ||
		   (deltaColumn == 0 && dColumn != 0))
		{
			return false;
		}

		int distance = Mathf.Abs(position.Row - startPosition.Row) + Mathf.Abs(position.Column - startPosition.Column);
		return distance ==  currentLayer;
	}
	
	protected override void OnTargetReached ()
	{
		this.CharacterAI.SetIdle(true);
	}
	
	protected override void OnPositionTileChanged (TilePosition oldPosition, TilePosition newPosition)
	{
		this.CharacterAI.BattleMapData.RefreshInformationWithMoveActor(this.m_AIBehavior.gameObject, oldPosition, newPosition);
	}
}
