using UnityEngine;
using System.Collections;

public class CharacterAI : BattleAI 
{
	[SerializeField]
	protected CharacterAttack m_AttackBehavior;
	[SerializeField]
	protected CharacterPropertyBehavior m_PropertyBehavior;
	
	private BuildingCategory m_FavoriteCategory;
	private bool m_ShowTarget;
	
	public BattleSceneHelper BattleSceneHelper { get; set; }
	public BattleMapData BattleMapData { get;set; }

	public TilePosition PreviousValidPosition { get;set; }

	public BuildingCategory FavoriteCategory
	{
		get
		{
			return this.m_FavoriteCategory;
		}
		set
		{
			this.m_FavoriteCategory = value;
		}
	}
	
	public CharacterAttack AttackBehavior { get { return this.m_AttackBehavior; } }
	public CharacterPropertyBehavior PropertyBehavior { get { return this.m_PropertyBehavior; } }
	public bool IsShowTarget
	{
		get
		{
			bool result = this.m_ShowTarget;
			this.m_ShowTarget = false;
			return result;
		}
	}
	
	public float PushAttenuateFactor { get; set; }
	public float PushFactor { get; set;  }
	
	public override void Start ()
	{
		this.m_ShowTarget = true;
		base.Start();
	}
	
	public virtual void SetIdle(bool isResponseInstantly)
	{
		InvaderIdleState idleState = new InvaderIdleState(this, isResponseInstantly);
		this.ChangeState(idleState);
	}
	
	public virtual void SetPush(int pushTicks, int pushVelocity, Vector3 pushSource)
	{
		TilePosition currentPosition = PositionConvertor.GetActorTileIndexFromWorldPosition(this.transform.position);
		if(BattleMapData.Instance.ActorCanPass(currentPosition.Row, currentPosition.Column))
		{
			PushState pushState = new PushState(this, pushTicks, pushVelocity, this.transform.position - pushSource);
			this.ChangeState(pushState);
		}
	}
}
