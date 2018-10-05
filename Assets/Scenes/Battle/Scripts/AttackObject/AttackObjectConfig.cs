using UnityEngine;
using System.Collections;

public class AttackObjectConfig : MonoBehaviour 
{
	[SerializeField]
	private tk2dSpriteAnimator m_SpriteAnimator;
	[SerializeField]
	private GameObject m_EndPrefab;
	
	[SerializeField]
	private int m_StartFrame;
	[SerializeField]
	private string m_Sound;
	
	[SerializeField]
	private float m_CurveG;
	[SerializeField]
	private AttackObjectPathType m_PathType;


	public tk2dSpriteAnimator SpriteAnimator
	{
		get
		{
			return this.m_SpriteAnimator;
		}
	}
	
	public int StartFrames
	{
		get
		{
			return this.m_StartFrame;
		}
	}
	
	public string Sound
	{
		get
		{
			return this.m_Sound;
		}
	}
	
	public float CurveG
	{
		get
		{
			return this.m_CurveG;
		}
	}
	
	public GameObject EndPrefab
	{
		get
		{
			return this.m_EndPrefab;
		}
	}
	
	public AttackObjectPathType PathType
	{
		get
		{
			return this.m_PathType;
		}
	}
}
