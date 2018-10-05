using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DefenderBehavior : MonoBehaviour 
{
	[SerializeField]
	private DetectBehavior m_Detect;
	
	/*
	protected override void AICalculate ()
	{
		if(this.m_CurrentState == CharacterState.IdleState)
		{
			if(this.IsResponseable)
			{
				this.FindTarget();
			}
		}
		else if(this.m_CurrentState == CharacterState.WalkState)
		{
			if(this.m_LinePath.Count == 0)
			{
				this.SetIdle();
			}
			if(this.IsResponseable)
			{
				this.FindTarget();
			}
		}
		else
		{
			if(this.m_Target == null || 
				Vector2.SqrMagnitude((Vector2)(this.transform.position - this.m_Target.transform.position)) > this.m_AttackBehavior.AttackScopeSqr)
			{
				if(this.IsResponseable)
				{
					this.FindTarget();
				}
			}
			else
			{	
				this.m_TargetPosition = this.m_Target.transform.position;
				this.m_TargetTilePosition = PositionConvertor.GetTileIndexFromWorldPosition(this.m_TargetPosition);
			}
		}
	}
	
	private void FindTarget()
	{
		this.m_CurrentState = CharacterState.IdleState;
		TilePosition tilePosition = PositionConvertor.GetTileIndexFromWorldPosition(this.transform.position);
		foreach(TilePosition offset in this.m_Detect.DetectScopeList)
		{
			TilePosition position = tilePosition + offset;
			if(position.IsValidTilePosition())
			{
				List<GameObject> actors = this.MapData.ActorArray[position.Row, position.Column];
				foreach(GameObject actor in actors)
				{
					CharacterPropertyBehavior property = actor.GetComponent<CharacterPropertyBehavior>();
					
					if(property.CharacterType == CharacterType.Invader)
					{
						this.m_Target = actor;
						this.m_TargetPosition = actor.transform.position;
						this.m_TargetTilePosition = position;
						
						if(Vector2.SqrMagnitude((Vector2)(this.transform.position - actor.transform.position)) <= 
							this.m_AttackBehavior.AttackScopeSqr)
						{
							this.m_CurrentState = CharacterState.AttackState;
							break;
						}
						else
						{
							this.m_CurrentState = CharacterState.WalkState;
						}
					} 
				}
			}
			if(this.m_CurrentState != CharacterState.IdleState)
			{
				break;
			}
		}
		if(this.m_CurrentState == CharacterState.IdleState)
		{
			this.m_Target = null;
			this.m_TargetTilePosition = null;
		}
		else
		{
			this.FindPath(tilePosition);
		}
	}
	
	private void FindPath(TilePosition currentPosition)
	{
		
		List<TilePosition> aStarPath;
		CharacterGCalculator characterCalculator = new CharacterGCalculator(this.m_WeightData, this.CurrentTargetPosition.Row, 
			this.CurrentTargetPosition.Column);
		List<TilePosition> linePath = AStarPathFinder.CalculatePathTile(characterCalculator, this.MapData.ActorObstacleArray, 
			currentPosition,this.CurrentTargetPosition, out aStarPath);
		this.m_LinePath.Clear();
		for(int i = 1; i < linePath.Count; i ++)
		{
			TilePosition linePoint = linePath[i];
			this.m_LinePath.Enqueue(linePoint);
		}
		
	}
*/
}
