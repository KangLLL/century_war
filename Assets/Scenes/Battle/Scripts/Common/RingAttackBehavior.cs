using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RingAttackBehavior : AttackBehavior 
{
	private float m_BlindScope;
	private float m_BlindScopeSqr;
	
	public override void Start ()
	{
		base.Start ();
		
		List<TilePosition> blindArray = RoundHelper.FillCircle(0,0,Mathf.FloorToInt(this.m_BlindScope / 
			Mathf.Min(ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.width, 
			ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.height)));
		
		foreach (TilePosition b in blindArray) 
		{
			this.AttackScopeArray.Remove(b);
		}
	}
	
	public float BlindScope 
	{
		get
		{
			return this.m_BlindScope;
		}
		set	
		{
			this.m_BlindScope = value;
			this.m_BlindScopeSqr = this.m_BlindScope * this.m_BlindScope;
		}
	}
	
	public float BlindScopeSqr
	{
		get
		{
			return this.m_BlindScopeSqr;
		}
	}
}
