using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackPropsBehavior : MonoBehaviour 
{
	[SerializeField]
	private GameObject[] m_DisplayPrefabs;
	[SerializeField]
	protected int m_PlayAnimationTicks;
	
	public Transform ParentNode { get; set; }
	public List<GameObject> DisplayObjects { get; private set; }
	
	public virtual void Start()
	{
		this.ConstructDisplayObject(); 
	}
	
	public void ConstructDisplayObject()
	{
		if(this.m_DisplayPrefabs != null)
		{	
			this.DisplayObjects = new List<GameObject>();
			foreach (GameObject p in m_DisplayPrefabs) 
			{
				GameObject go = GameObject.Instantiate(p) as GameObject;
				go.transform.position = this.transform.position + go.transform.position;
				go.transform.parent = this.ParentNode;
				this.DisplayObjects.Add(go);
			}
		}
	}
	
	public void EffectDisappear()
	{
		if(Application.loadedLevelName == ClientStringConstants.BATTLE_SCENE_LEVEL_NAME)
		{
			if(BattleSceneHelper.Instance.TotalInvaderCount == 0 && 
				BattleRecorder.Instance.DropArmyCount == ArmyMenuPopulator.Instance.TotalArmyCount &&
				BattleRecorder.Instance.DropMercenaryCount == ArmyMenuPopulator.Instance.TotalMercenaryCount)
			{
				BattleDirector.Instance.EndMatch();
			}	
		}
	}
}
