using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class EditorDefenseObjectBehavior : EditorObjectBehavior 
{
	public PropsType PropsType { get; set; }
	
	void Start()
	{
		this.m_Function = new DefenseObjectFunction() { Position = this.Position, ObjectType = this.PropsType, OperatorBehavior = this };
		this.m_Function.Initial();
	}
}
