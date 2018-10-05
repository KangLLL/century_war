using UnityEngine;
using System.Collections;

public interface IAttackObjectTarget  
{
	Vector2 GetDestinationPosition(Vector3 sourcePosition);
}
