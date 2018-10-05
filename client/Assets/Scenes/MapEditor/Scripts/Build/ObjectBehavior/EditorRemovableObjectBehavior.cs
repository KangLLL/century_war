using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ConfigUtilities.Enums;
using ConfigUtilities;
using ConfigUtilities.Structs;
using System;

public class EditorRemovableObjectBehavior : EditorObjectBehavior 
{
	public RemovableObjectType RemovableObjectType { get; set; }
	
	void Start()
	{
		this.m_Function = new RemovableObjectFunction() { Position = this.Position, ObjectType = this.RemovableObjectType, OperatorBehavior = this };
		this.m_Function.Initial();
	}
}
