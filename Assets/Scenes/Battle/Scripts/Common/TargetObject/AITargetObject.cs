using UnityEngine;
using System.Collections;

public class AITargetObject 
{
	private GameObject m_Target;
	private Vector2 m_ObjectPreviousPosition;
	private Vector2 m_TargetPosition;
	
	public AITargetObject(GameObject target, Vector2 targetPosition)
	{
		this.m_Target = target;
		this.m_TargetPosition = targetPosition;
		this.m_ObjectPreviousPosition = target.transform.position;
	}
	
	public Vector2 TargetPosition 
	{
		get
		{
			if((Vector2)this.m_Target.transform.position != this.m_ObjectPreviousPosition)
			{
				this.m_TargetPosition = (Vector2)this.m_Target.transform.position;
				this.m_ObjectPreviousPosition = this.m_TargetPosition;
			}
			return this.m_TargetPosition;
		}
	}
	
	public GameObject Target { get { return this.m_Target; } }
}
