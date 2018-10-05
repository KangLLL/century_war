using UnityEngine;
using System.Collections;

public class CharacterIdleState : IdleState 
{
	protected CharacterAI CharacterAI
	{
		get
		{
			return (CharacterAI)this.m_AIBehavior;
		}
	}

	protected bool m_IsFindInstantly;
	protected TilePosition m_CurrentPosition;

	public CharacterIdleState(NewAI aiBehavior, bool isFindInstantly) : base(aiBehavior)
	{
		this.m_IsFindInstantly = isFindInstantly;
		this.m_CurrentPosition = PositionConvertor.GetActorTileIndexFromWorldPosition(this.m_AIBehavior.transform.position);
	}

	public override void Initial ()
	{
		if(!this.CharacterAI.BattleMapData.ActorCanPass(this.m_CurrentPosition.Row, this.m_CurrentPosition.Column))
		{
			TilePosition tp = this.CharacterAI.PreviousValidPosition;
			if(tp == null)
			{
				GameObject target = this.CharacterAI.BattleSceneHelper.GetNearestBuildingOfCategory(this.m_AIBehavior.transform.position, this.CharacterAI.FavoriteCategory);
				if(target != null)
				{
					BuildingBasePropertyBehavior property = target.GetComponent<BuildingBasePropertyBehavior>();
					tp = property.GetBuildingFirstActorPosition();
				}
			}
			
			if(tp != null)
			{
				InvaderFindOutState findOutState = new InvaderFindOutState(this.CharacterAI.BattleMapData,tp,this.m_AIBehavior);
				this.m_AIBehavior.ChangeState(findOutState);
			}
		}
		else
		{
			this.CharacterAI.PreviousValidPosition = this.m_CurrentPosition;
		}
		base.Initial ();
	}
}
