using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackConfig : MonoBehaviour 
{
	[SerializeField]
	private Vector2 m_AttackUpOffset;
	[SerializeField]
	private Vector2 m_AttackDownOffset;
	[SerializeField]
	private Vector2 m_AttackLeftOffset;
	[SerializeField]
	private Vector2 m_AttackRightOffset;
	[SerializeField]
	private Vector2 m_AttackRightUpOffset;
	[SerializeField]
	private Vector2 m_AttackRightDownOffset;
	[SerializeField]
	private Vector2 m_AttackLeftUpOffset;
	[SerializeField]
	private Vector2 m_AttackLeftDownOffset;
	
	[SerializeField]
	private Vector2 m_BeAttackedUpOffset;
	[SerializeField]
	private Vector2 m_BeAttackedDownOffset;
	[SerializeField]
	private Vector2 m_BeAttackedLeftOffset;
	[SerializeField]
	private Vector2 m_BeAttackedRightOffset;
	[SerializeField]
	private Vector2 m_BeAttackedRightUpOffset;
	[SerializeField]
	private Vector2 m_BeAttackedRightDownOffset;
	[SerializeField]
	private Vector2 m_BeAttackedLeftUpOffset;
	[SerializeField]
	private Vector2 m_BeAttackedLeftDownOffset;
	
	[SerializeField]
	private GameObject m_AttackObjectPrefab;
	[SerializeField]
	private string m_AttackSound;
	
	[SerializeField]
	private List<GameObject> m_FireEffectPrefabs;
	[SerializeField]
	private List<DirectionOffsetConfig> m_FireEffectOffsets;
	
	public Vector2 AttackUpOffset { get{ return this.m_AttackUpOffset; } }
	public Vector2 AttackDownOffset { get{ return this.m_AttackDownOffset; } }
	public Vector2 AttackLeftOffset { get{ return this.m_AttackLeftOffset; } }
	public Vector2 AttackRightOffset { get{ return this.m_AttackRightOffset; } }
	public Vector2 AttackRightUpOffset { get{ return this.m_AttackRightUpOffset; } }
	public Vector2 AttackRightDownOffset { get{ return this.m_AttackRightDownOffset; } }
	public Vector2 AttackLeftUpOffset { get{ return this.m_AttackLeftUpOffset; } }
	public Vector2 AttackLeftDownOffset { get{ return this.m_AttackLeftDownOffset; } }
	
	public Vector2 BeAttackedUpOffset { get{ return this.m_BeAttackedUpOffset; } }
	public Vector2 BeAttackedDownOffset { get{ return this.m_BeAttackedDownOffset; } }
	public Vector2 BeAttackedLeftOffset { get{ return this.m_BeAttackedLeftOffset; } }
	public Vector2 BeAttackedRightOffset { get{ return this.m_BeAttackedRightOffset; } }
	public Vector2 BeAttackedRightUpOffset { get{ return this.m_BeAttackedRightUpOffset; } }
	public Vector2 BeAttackedRightDownOffset { get{ return this.m_BeAttackedRightDownOffset; } }
	public Vector2 BeAttackedLeftUpOffset { get{ return this.m_BeAttackedLeftUpOffset; } }
	public Vector2 BeAttackedLeftDownOffset { get{ return this.m_BeAttackedLeftDownOffset; } }
	
	public GameObject AttackObjectPrefab { get{ return this.m_AttackObjectPrefab; } }
	public string AttackSound { get{ return this.m_AttackSound; } } 
	
	public List<GameObject> FireEffectPrefabs { get { return this.m_FireEffectPrefabs; } }
	public List<DirectionOffsetConfig> FireEffectOffsets { get { return this.m_FireEffectOffsets; } }
}
