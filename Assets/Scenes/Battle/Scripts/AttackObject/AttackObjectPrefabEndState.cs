using UnityEngine;
using System.Collections;

public class AttackObjectPrefabEndState : EndState 
{ 
	public override void Initialize ()
	{
		AudioController.Play(this.m_Config.Sound);
		this.m_Criterion = new FrameRelatedCriterion(0);
		
		GameObject endEffect = GameObject.Instantiate(this.m_Config.EndPrefab) as GameObject;
		endEffect.transform.position = this.m_Behavior.transform.position;
		endEffect.transform.parent = this.m_Behavior.transform.parent;
		base.Initialize ();
	}
}
