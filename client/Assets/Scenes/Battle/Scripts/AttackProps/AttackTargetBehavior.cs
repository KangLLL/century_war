using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackTargetBehavior : AttackPropsBehavior 
{
	public int Scope { get;set; }
	public int LastingTicks { get;set; }
	
	private int m_CurrentTicks;
	
	[SerializeField]
	private AttackTargetHPBehavior m_HPBehavior;
	
	private void Effect()
	{
		List<BuildingAI> affectedBuildings = new List<BuildingAI>();
		int distanceSqrt = this.Scope * this.Scope;
		
		Vector2 targetPosition = (Vector2)this.transform.position;
		TilePosition targetTile = PositionConvertor.GetActorTileIndexFromWorldPosition(this.transform.position);
	    int radius = Mathf.CeilToInt(this.Scope / (float)Mathf.Min(ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.width,
			ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.height));
		
		List<TilePosition> affectedTiles = RoundHelper.FillCircle(targetTile.Column, targetTile.Row, radius);
		foreach (TilePosition tile in affectedTiles) 
		{
			if(tile.IsValidActorTilePosition())
			{
				Vector2 p = (Vector2)PositionConvertor.GetWorldPositionFromActorTileIndex(tile);
				Vector2  dis = p - targetPosition;
				if(dis.sqrMagnitude <= distanceSqrt)
				{
					GameObject building = BattleMapData.Instance.GetBulidingObjectFromActorObstacleMap(tile.Row, tile.Column);
					if(building != null)
					{
						BuildingAI ai = building.GetComponent<BuildingAI>();
						if(ai != null && !affectedBuildings.Contains(ai))
						{
							affectedBuildings.Add(ai);
							ai.SetTarget(this.gameObject, (Vector2)this.transform.position);
						}
					}
				}
			}
		}
	}
	
	void FixedUpdate () 
	{
		if(this.m_CurrentTicks == this.m_PlayAnimationTicks)
		{
			this.Effect();
		}
		if((this.m_CurrentTicks - this.m_PlayAnimationTicks) == this.LastingTicks)
		{
			this.EffectDisappear();
			this.m_HPBehavior.SetDead();
		}
		else
		{
			this.m_CurrentTicks ++;
		}
	}
}
