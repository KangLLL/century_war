using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingBaseHPBehavior : HPBehavior 
{
	[SerializeField]
	private float m_NotifyRadius;
	
	private float m_NotifyRadiusSqr;
	protected BuildingBasePropertyBehavior m_BaseProperty;
	
	public BattleSceneHelper SceneHelper { get;set; }

	public override void Start ()
	{
		this.m_NotifyRadiusSqr = this.m_NotifyRadius * this.m_NotifyRadius;
		this.m_BaseProperty = this.GetComponent<BuildingBasePropertyBehavior>();
		base.Start ();
	}
	
	protected override void OnDead ()
	{	
		if(this.m_NotifyRadius > 0)
		{
			this.NotifyNearbyInvader();
		}
		base.OnDead ();
	}
	
	private void NotifyNearbyInvader()
	{
		Vector3 anchorPosition = this.m_BaseProperty.AnchorTransform.position;
		TilePosition center = PositionConvertor.GetActorTileIndexFromWorldPosition(anchorPosition);
		int tileRadius = Mathf.CeilToInt(this.m_NotifyRadius / 
			Mathf.Min(ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.width, ClientSystemConstants.ACTOR_TILE_MAP_TILE_SIZE.height));
		List<TilePosition> points = RoundHelper.FillCircle(center.Column, center.Row, tileRadius);
		
		List<GameObject> actors = this.SceneHelper.GetActors(points, TargetType.Ground);
		foreach (GameObject actor in actors) 
		{
			CharacterPropertyBehavior property = actor.GetComponent<CharacterPropertyBehavior>();
			if(property.CharacterType == CharacterType.Invader)
			{
				Vector2 dist = (Vector2)(anchorPosition - actor.transform.position);
				if(this.m_NotifyRadiusSqr >= Vector2.SqrMagnitude(dist))
				{
					actor.GetComponent<CharacterAI>().SetIdle(false);
				}
			}
		}
	}
}
