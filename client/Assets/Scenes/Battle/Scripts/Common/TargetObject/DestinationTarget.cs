using UnityEngine;
using System.Collections;

public class DestinationTarget : IAttackObjectTarget 
{
	private Vector2 m_Destination;
	
	public DestinationTarget(Vector2 destination)
	{
		this.m_Destination = destination;
	}

	public Vector2 GetDestinationPosition (Vector3 sourcePosition)
	{
		return this.m_Destination;
	}
}
