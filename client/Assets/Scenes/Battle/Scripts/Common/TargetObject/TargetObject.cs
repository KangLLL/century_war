using UnityEngine;
using System.Collections;

public class TargetObject : IAttackObjectTarget
{
	private GameObject m_Target;
	
	private AttackConfig m_TargetConfig;
	
	private Vector2 m_CalculatedDestinationPosition;
	private Vector3 m_PreviousCalculatedObjectPosition;
	
	public GameObject Target { get { return this.m_Target; } }
	
	public TargetObject(GameObject target, Vector2 targetPosition, Vector2 sourcePosition)
	{
		this.m_Target = target;
		this.m_TargetConfig = target.GetComponent<AttackConfig>();
		this.CalculateTargetPosition(sourcePosition, targetPosition);
	}

	public Vector2 GetDestinationPosition (Vector3 sourcePosition)
	{
		if(this.m_Target != null && this.m_Target.transform.position != this.m_PreviousCalculatedObjectPosition)
		{
			this.CalculateTargetPosition((Vector2)sourcePosition);
		}
		return this.m_CalculatedDestinationPosition;
	}
	
	private void CalculateTargetPosition(Vector2 sourcePosition)
	{
		this.CalculateTargetPosition(sourcePosition, this.m_Target.transform.position);
	}
	
	private void CalculateTargetPosition(Vector2 sourcePosition, Vector2 targetPosition)
	{
		Vector2 offset = Vector2.zero;
		Vector2 newPosition = (Vector2)targetPosition;
		
		if(this.m_TargetConfig != null)
		{
			CharacterDirection direction = DirectionHelper.GetDirectionFormVector(newPosition - sourcePosition);
			switch(direction)
			{
				case CharacterDirection.Up:
				{
					offset = this.m_TargetConfig.BeAttackedUpOffset;
				}
				break;
				case CharacterDirection.Down:
				{
					offset = this.m_TargetConfig.BeAttackedDownOffset;
				}
				break;
				case CharacterDirection.Left:
				{
					offset = this.m_TargetConfig.BeAttackedLeftOffset;
				}
				break;
				case CharacterDirection.Right:
				{
					offset = this.m_TargetConfig.BeAttackedRightOffset;
				}
				break;
				case CharacterDirection.LeftUp:
				{
					offset = this.m_TargetConfig.BeAttackedLeftUpOffset;
				}
				break;
				case CharacterDirection.LeftDown:
				{
					offset = this.m_TargetConfig.BeAttackedLeftDownOffset;
				}
				break;
				case CharacterDirection.RightUp:
				{
					offset = this.m_TargetConfig.BeAttackedRightUpOffset;
				}
				break;
				case CharacterDirection.RightDown:
				{
					offset = this.m_TargetConfig.BeAttackedRightDownOffset;
				}
				break;
			}
		}
		
		this.m_CalculatedDestinationPosition = newPosition + offset;
		this.m_PreviousCalculatedObjectPosition = this.m_Target.transform.position;
	}
}
