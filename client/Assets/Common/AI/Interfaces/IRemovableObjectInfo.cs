using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public interface IRemovableObjectInfo : IObstacleInfo 
{
	RemovableObjectType ObjectType { get; }
}
