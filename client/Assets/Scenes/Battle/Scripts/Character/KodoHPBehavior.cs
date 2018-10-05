using UnityEngine;
using System.Collections;
using ConfigUtilities.Enums;

public class KodoHPBehavior : CharacterHPBehavior 
{
	[SerializeField]
	private GameObject m_BombEffect;
	[SerializeField]
	private DropArmySerializableInformation[] m_DropArmies;
	[SerializeField]
	private MercenaryType[] m_DropMercenaries;
	
	public CharacterFactory Factory { get;set; }
	public DropArmySerializableInformation[] DropArmies { get { return this.m_DropArmies; } }
	public MercenaryType[] DropMercenaries { get { return this.m_DropMercenaries; }  }
	
	protected override void OnDead ()
	{
		this.GenerateCharacter();
		base.OnDead ();
	}
	
	
	public void Bomb()
	{
		if(this.m_CurrentHP > 0)
		{
			if(this.m_BombEffect != null)
			{
				GameObject bombPrefab = GameObject.Instantiate(this.m_BombEffect) as GameObject;
				bombPrefab.transform.position = this.transform.position;
				GameObject ruins = BattleObjectCache.Instance.RuinsObjectParent;
				bombPrefab.transform.parent = ruins.transform;
			}
			
			string attackSoundName = this.GetComponent<AttackConfig>().AttackSound;
			AudioController.Play(attackSoundName);
			
			this.GenerateCharacter();

			this.ClearActor();
			GameObject.Destroy(this.gameObject);
		}
	}
	
	private void GenerateCharacter()
	{
		foreach (DropArmySerializableInformation army in m_DropArmies) 
		{
			this.Factory.ConstructArmy(army.ArmyType, army.ArmyLevel, this.transform.position);
		}
		
		foreach (MercenaryType mercenary in m_DropMercenaries) 
		{	
			this.Factory.ConstructMercenary(mercenary, this.transform.position);
		}
	}
}
