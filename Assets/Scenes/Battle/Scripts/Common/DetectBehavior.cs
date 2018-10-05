using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DetectBehavior : MonoBehaviour 
{
	[SerializeField]
	private float m_DetectScope;
	
//	private float m_DetectScopeSqr;
	private List<TilePosition> m_DetectScopeList;
	
	public List<TilePosition> DetectScopeList
	{
		get
		{
			return this.m_DetectScopeList;
		}
	}
	
	// Use this for initialization
	void Start () 
	{
//		this.m_DetectScopeSqr = this.m_DetectScope * this.m_DetectScope;
		this.m_DetectScopeList = RoundHelper.FillCircle(0,0,Mathf.FloorToInt(this.m_DetectScope / 
			Mathf.Min(ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.width, 
			ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.height)));
	}
}
