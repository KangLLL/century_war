using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;

public class RemovableObjectPropertyBehavior : ObstaclePropertyBehavior, IRemovableObjectInfo
{
	private RemovableObjectType m_RemovableObjectType;
	
	public RemovableObjectType ObjectType 
	{
		get 
		{
			return this.m_RemovableObjectType;
		}
		set
		{
			this.m_RemovableObjectType = value;
		}
	}
}
