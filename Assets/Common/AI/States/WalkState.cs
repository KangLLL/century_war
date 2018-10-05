using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class WalkState : AIState 
{
	protected TilePosition m_TargetPosition;
	
	protected TilePosition m_PreviousPosition;
	protected Queue<TilePosition> m_LinePath;
	
	protected IMapData m_MapData;
	protected Vector2 m_TargetOffset;
	
	public float WalkVelocity { get; set; }
	
	protected Vector2 m_MoveVector;
	public Vector2 MoveVector 
	{ 
		get
		{
			return this.m_MoveVector; 
		}
	}
	
	public WalkState(IMapData mapData, TilePosition targetPosition, NewAI aiBehavior) : base(aiBehavior)
	{
		this.m_TargetPosition = targetPosition;
		this.m_LinePath = new Queue<TilePosition>();
		this.m_MapData = mapData;
	}
	
	public override void Initial ()
	{
		this.m_PreviousPosition = PositionConvertor.GetActorTileIndexFromWorldPosition
			(this.m_AIBehavior.gameObject.transform.position);
		
		if(this.m_LinePath == null || this.m_LinePath.Count == 0)
		{
			this.FindPath();
		}
	}
	
	public override void AICalculate ()
	{
		if(this.m_LinePath.Count == 0)
		{
			this.OnTargetReached();
		}
		else
		{
			Vector2 sourcePosition = this.m_AIBehavior.transform.position;
			Vector2 destinationPosition = (Vector2)PositionConvertor.GetWorldPositionFromActorTileIndex(this.m_LinePath.Peek());
			if(this.m_LinePath.Count == 1)
			{
				destinationPosition += this.m_TargetOffset;
			}
			
			float move = this.WalkVelocity;
			float distance = Vector2.Distance(sourcePosition, destinationPosition);
				
			Vector2 result = Vector2.zero;
			Vector2 deltaVector = destinationPosition - sourcePosition;
			
			while(distance <= move)
			{
				this.m_LinePath.Dequeue();
				result += deltaVector;
				move -= distance;
				
				sourcePosition = destinationPosition;
				if(this.m_LinePath.Count == 0)
				{
					distance = 0;
					break;
				}
				destinationPosition = (Vector2)PositionConvertor.GetWorldPositionFromActorTileIndex(this.m_LinePath.Peek());
				if(this.m_LinePath.Count == 1)
				{
					destinationPosition += this.m_TargetOffset;
				}
				distance = Vector2.Distance(sourcePosition, destinationPosition);
				deltaVector = destinationPosition - sourcePosition;
			}
			
			if(distance.IsZero())
			{
				result = deltaVector;
			}
			else
			{
				float percentage = move / distance;
				result += deltaVector * percentage;
			}
			
			this.m_MoveVector = result;
				
			this.m_AIBehavior.transform.position += new Vector3(result.x, result.y, 0);
		
			TilePosition currentPosition = PositionConvertor.GetActorTileIndexFromWorldPosition(this.m_AIBehavior.transform.position);
			if(!currentPosition.Equals(this.m_PreviousPosition))
			{
				this.OnPositionTileChanged(this.m_PreviousPosition, currentPosition);
				this.m_PreviousPosition = currentPosition;
			}
			
			this.m_AnimationController.PlayWalkAnimation(this.m_MoveVector);
			
		}
	}
	
	protected abstract IGCalculator FindPathStrategy { get; }
	
	protected virtual void FindPath()
	{
	}
	
	protected virtual void OnPositionTileChanged(TilePosition oldPosition, TilePosition newPosition)
	{
	}
	
	protected virtual void OnTargetReached()
	{
	}
	
	public virtual void SetPath(List<TilePosition> path)
	{
		this.m_LinePath.Clear();
		for(int i = 1; i < path.Count; i ++)
		{
			TilePosition point = path[i];
			this.m_LinePath.Enqueue(point);
		}
	}
}
